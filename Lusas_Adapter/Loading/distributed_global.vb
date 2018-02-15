Imports System.Collections.Generic
Imports System

Imports Grasshopper.Kernel
Imports Rhino.Geometry

Imports RMA.OpenNURBS
Imports LusasM15_2


Public Class distributed_global_z
    Inherits GH_Component

    Public Overrides ReadOnly Property Exposure() As GH_Exposure
        Get
            Return GH_Exposure.primary
        End Get
    End Property

    Public Sub New()
        MyBase.New(“distributed_global_z", "dis_g_z",
                   "Create a total distributed load or per unit length/area in the Z direction",
                   "Bridges Tools", "Loading")
    End Sub

    Protected Overrides Sub RegisterInputParams(ByVal pManager As GH_InputParamManager)
        pManager.AddTextParameter("load_name", "lN", "load name", GH_ParamAccess.item)
        pManager.AddNumberParameter("force_z", "Fz", "concentrated load in Z", GH_ParamAccess.item)
        pManager.AddTextParameter("load_type", "lT", "Total, (per unit) length or (per unit) area", GH_ParamAccess.item)
        pManager.AddIntegerParameter("element_ID", "ID", "Points/lines/surfaces/volume to assign load to", GH_ParamAccess.list)
        pManager.AddTextParameter("object_type", "type", "point/line/surface/volume", GH_ParamAccess.item)
        pManager.AddBooleanParameter("active?", "act?", "active component?", GH_ParamAccess.item)
    End Sub
    Protected Overrides Sub RegisterOutputParams(ByVal pManager As GH_OutputParamManager)
        pManager.Register_IntegerParam("load_ID", "l_ID", "ID to be used for other LUSAS components")
    End Sub

    Protected Overrides Sub SolveInstance(ByVal Da As IGH_DataAccess)
        Dim activate As Boolean = False
        If (Not Da.GetData(5, activate)) Then Return
        If activate Then
            Dim modeller As LusasWinApp = lusas_modeller.m_lusas

            'Retrieve loading information
            Dim name As String = ""
            If (Not Da.GetData(0, name)) Then Return
            Dim Fz As Double = 0
            If (Not Da.GetData(1, Fz)) Then Return
            Dim type As String = ""
            If (Not Da.GetData(2, type)) Then Return

            'Create concentrated load
            Dim dis_glo As IFLoadingGlobalDistributed = modeller.db.createLoadingGlobalDistributed(name)
            dis_glo.setGlobalDistributed(type, 0, 0, Fz, 0, 0, 0, 0, 0)
            'Get object list and type
            Dim obj_ID As New List(Of Integer)
            If (Not Da.GetDataList(3, obj_ID)) Then Return
            Dim obj_type As String = ""
            If (Not Da.GetData(4, obj_type)) Then Return

            'Create object set and assign
            Dim obj_set As IFObjectSet = modeller.newObjectSet
            For Each value As Integer In obj_ID
                obj_set.add(obj_type, value)
            Next

            dis_glo.assignTo(obj_set)

            Da.SetData(0, dis_glo.getID)
        End If
    End Sub

    Public Overrides ReadOnly Property ComponentGuid() As Guid
        Get
            Return New Guid("{d36a96ab-4558-4a05-90c9-2790cf6068f4}")
        End Get
    End Property

    Protected Overrides ReadOnly Property Internal_Icon_24x24() As System.Drawing.Bitmap
        Get
            Return My.Resources.global_distributed
        End Get
    End Property
End Class