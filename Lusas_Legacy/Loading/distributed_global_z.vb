Imports System.Collections.Generic
Imports System

Imports Grasshopper.Kernel
Imports Rhino.Geometry

Imports RMA.OpenNURBS
Imports LusasM15_2


Public Class distributed_global
    Inherits GH_Component

    Public Overrides ReadOnly Property Exposure() As GH_Exposure
        Get
            Return GH_Exposure.primary
        End Get
    End Property

    Public Sub New()
        MyBase.New(“distributed_global", "dis_g",
                   "Create a total distributed load or per unit length/area",
                   "Bridges Tools", "Loading")
    End Sub

    Protected Overrides Sub RegisterInputParams(ByVal pManager As GH_InputParamManager)
        pManager.AddTextParameter("load_name", "lN", "load name", GH_ParamAccess.item)
        pManager.AddNumberParameter("force_x", "Fx", "concentrated load in X", GH_ParamAccess.item)
        pManager.AddNumberParameter("force_y", "Fy", "concentrated load in Y", GH_ParamAccess.item)
        pManager.AddNumberParameter("force_z", "Fz", "concentrated load in Z", GH_ParamAccess.item)
        pManager.AddNumberParameter("moment_x", "Mx", "moment about X", GH_ParamAccess.item)
        pManager.AddNumberParameter("moment_y", "My", "moment about Y", GH_ParamAccess.item)
        pManager.AddNumberParameter("moment_z", "Mz", "moment about Z", GH_ParamAccess.item)
        pManager.AddNumberParameter("hinge_rotation", "hinge", "moment about hinge", GH_ParamAccess.item)
        pManager.AddTextParameter("load_type", "lT", "Total, (per unit) length or (per unit) area", GH_ParamAccess.item)
        pManager.AddIntegerParameter("element_ID", "ID", "Points/lines/surfaces/volume to assign load to", GH_ParamAccess.list)
        pManager.AddTextParameter("object_type", "type", "point/line/surface/volume", GH_ParamAccess.item)
        pManager.AddTextParameter("loadcase_name", "lc", "name of loadcase", GH_ParamAccess.list)
        pManager.AddBooleanParameter("active?", "act?", "active component?", GH_ParamAccess.item)
    End Sub
    Protected Overrides Sub RegisterOutputParams(ByVal pManager As GH_OutputParamManager)
        pManager.Register_IntegerParam("load_ID", "l_ID", "ID to be used for other LUSAS components")
    End Sub

    Protected Overrides Sub SolveInstance(ByVal Da As IGH_DataAccess)
        Dim activate As Boolean = False
        If (Not Da.GetData(12, activate)) Then Return
        If activate Then
            Dim modeller As LusasWinApp = lusas_modeller.m_lusas

            'Retrieve loading information
            Dim name As String = ""
            If (Not Da.GetData(0, name)) Then Return
            Dim Fx As Double = 0
            If (Not Da.GetData(1, Fx)) Then Return
            Dim Fy As Double = 0
            If (Not Da.GetData(2, Fy)) Then Return
            Dim Fz As Double = 0
            If (Not Da.GetData(3, Fz)) Then Return
            Dim Mx As Double = 0
            If (Not Da.GetData(4, Mx)) Then Return
            Dim My As Double = 0
            If (Not Da.GetData(5, My)) Then Return
            Dim Mz As Double = 0
            If (Not Da.GetData(6, Mz)) Then Return
            Dim hinge As Double = 0
            If (Not Da.GetData(7, hinge)) Then Return
            Dim type As String = 0
            If (Not Da.GetData(8, type)) Then Return

            'Create concentrated load
            Dim dis_glo As IFLoadingGlobalDistributed = modeller.db.createLoadingGlobalDistributed(Name)
            dis_glo.setGlobalDistributed(type, Fx, Fy, Fz, Mx, My, Mz, hinge, hinge)
            'Get object list and type
            Dim obj_ID As New List(Of Integer)
            If (Not Da.GetDataList(9, obj_ID)) Then Return
            Dim obj_type As String = ""
            If (Not Da.GetData(10, obj_type)) Then Return
            Dim assignment_loadcase As New List(Of String)
            If (Not Da.GetDataList(11, assignment_loadcase)) Then Return


            'Create object set and assign
            Dim obj_set As IFObjectSet = modeller.newObjectSet
            For Each value As Integer In obj_ID
                obj_set.add(obj_type, value)
            Next
            Dim assignment As IFAssignment = modeller.newAssignment
            For Each loadcase_name As String In assignment_loadcase
                assignment.setLoadset(loadcase_name)
            Next

            dis_glo.assignTo(obj_set)

            Da.SetData(0, dis_glo.getID)
        End If
    End Sub

    Public Overrides ReadOnly Property ComponentGuid() As Guid
        Get
            Return New Guid("{12c2f0df-e34a-407e-ad0b-eb497166fb1b}")
        End Get
    End Property

    Protected Overrides ReadOnly Property Internal_Icon_24x24() As System.Drawing.Bitmap
        Get
            Return My.Resources.global_distributed
        End Get
    End Property
End Class