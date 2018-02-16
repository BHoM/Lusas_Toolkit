Imports System.Collections.Generic
Imports System

Imports Grasshopper.Kernel
Imports Rhino.Geometry

Imports RMA.OpenNURBS
Imports LusasM15_2


Public Class beam_internal_point_load
    Inherits GH_Component

    Public Overrides ReadOnly Property Exposure() As GH_Exposure
        Get
            Return GH_Exposure.primary
        End Get
    End Property

    Public Sub New()
        MyBase.New(“beam_internal_point_load", "inter_p",
                   "Create concentrated load on a point along a beam element",
                   "Bridges Tools", "Loading")
    End Sub

    Protected Overrides Sub RegisterInputParams(ByVal pManager As GH_InputParamManager)
        pManager.AddTextParameter("load_name", "name", "load name", GH_ParamAccess.item)
        pManager.AddNumberParameter("force_x", "Fx", "concentrated load in X", GH_ParamAccess.item)
        pManager.AddNumberParameter("force_y", "Fy", "concentrated load in Y", GH_ParamAccess.item)
        pManager.AddNumberParameter("force_z", "Fz", "concentrated load in Z", GH_ParamAccess.item)
        pManager.AddNumberParameter("moment_x", "Mx", "moment about X", GH_ParamAccess.item)
        pManager.AddNumberParameter("moment_y", "My", "moment about Y", GH_ParamAccess.item)
        pManager.AddNumberParameter("moment_z", "Mz", "moment about Z", GH_ParamAccess.item)
        pManager.AddTextParameter("type", "load_type", "parametric/actual", GH_ParamAccess.item)
        pManager.AddTextParameter("dir_type", "d_type", "local/global", GH_ParamAccess.item)
        pManager.AddNumberParameter("distance", "dist", "distance/position of load", GH_ParamAccess.item)
        pManager.AddTextParameter("postion", "pos", "nodal/beam", GH_ParamAccess.item)
        pManager.AddIntegerParameter("element_ID", "ID", "element ID", GH_ParamAccess.list)
        pManager.AddTextParameter("object_type", "type", "line", GH_ParamAccess.item)
        pManager.AddTextParameter("loadcase_name", "lc", "name of loadcase", GH_ParamAccess.list)
        pManager.AddBooleanParameter("active?", "act?", "active component?", GH_ParamAccess.item)
    End Sub
    Protected Overrides Sub RegisterOutputParams(ByVal pManager As GH_OutputParamManager)
        pManager.Register_IntegerParam("load_ID", "l_ID", "ID to be used for other LUSAS components")
    End Sub

    Protected Overrides Sub SolveInstance(ByVal Da As IGH_DataAccess)
        Dim activate As Boolean = False
        If (Not Da.GetData(14, activate)) Then Return
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

            Dim load_type As String = ""
            If (Not Da.GetData(7, load_type)) Then Return

            Dim d_type As String = ""
            If (Not Da.GetData(8, d_type)) Then Return

            Dim dist As Double = 0
            If (Not Da.GetData(9, dist)) Then Return

            Dim pos As String = ""
            If (Not Da.GetData(10, pos)) Then Return

            'Create beam load
            Dim internal_p_load As IFLoadingBeamPoint = modeller.db.createLoadingBeamPoint(name)
            internal_p_load.setBeamPoint(load_type, d_type, pos)
            internal_p_load.addRow(dist, Fx, Fy, Fz, Mx, My, Mz)

            'Get object list and type
            Dim obj_ID As New List(Of Integer)
            If (Not Da.GetDataList(11, obj_ID)) Then Return
            Dim obj_type As String = ""
            If (Not Da.GetData(12, obj_type)) Then Return
            Dim assignment_loadcase As New List(Of String)
            If (Not Da.GetDataList(13, assignment_loadcase)) Then Return

            'Create object set and assign
            Dim obj_set As IFObjectSet = modeller.newObjectSet
            For Each value As Integer In obj_ID
                obj_set.add(obj_type, value)
            Next
            Dim assignment As IFAssignment = modeller.newAssignment
            For Each loadcase_name As String In assignment_loadcase
                assignment.setLoadset(loadcase_name)
            Next

            internal_p_load.assignTo(obj_set, assignment)

            Da.SetData(0, internal_p_load.getID)
        End If
    End Sub

    Public Overrides ReadOnly Property ComponentGuid() As Guid
        Get
            Return New Guid("{b08a13f2-177f-4fde-bd83-62ffb271fce6}")
        End Get
    End Property

    Protected Overrides ReadOnly Property Internal_Icon_24x24() As System.Drawing.Bitmap
        Get
            Return My.Resources.internal_p_beam
        End Get
    End Property
End Class

