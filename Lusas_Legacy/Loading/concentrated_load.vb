Imports System.Collections.Generic
Imports System

Imports Grasshopper.Kernel
Imports Rhino.Geometry

Imports RMA.OpenNURBS
Imports LusasM15_2


Public Class concentrated_load
    Inherits GH_Component

    Public Overrides ReadOnly Property Exposure() As GH_Exposure
        Get
            Return GH_Exposure.primary
        End Get
    End Property

    Public Sub New()
        MyBase.New(“concentrated_load", "con",
                   "Create concentrated load on a point",
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
        pManager.AddNumberParameter("pore_pressure", "pore", "pore pressure", GH_ParamAccess.item)
        pManager.AddIntegerParameter("point_ID", "ID", "Points to assign support to", GH_ParamAccess.list)
        pManager.AddTextParameter("object_type", "type", "line or point", GH_ParamAccess.item)
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
            Dim pore As Double = 0
            If (Not Da.GetData(8, pore)) Then Return

            'Create concentrated load
            Dim con_load As IFLoadingConcentrated = modeller.db.createLoadingConcentrated(Name)
            con_load.setConcentrated(Fx, Fy, Fz, Mx, My, Mz, hinge, hinge, pore)

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

            con_load.assignTo(obj_set)

            Da.SetData(0, con_load.getID)
        End If
    End Sub

    Public Overrides ReadOnly Property ComponentGuid() As Guid
        Get
            Return New Guid("{97c36d46-a144-47db-98f2-ffa714a027fb}")
        End Get
    End Property

    Protected Overrides ReadOnly Property Internal_Icon_24x24() As System.Drawing.Bitmap
        Get
            Return My.Resources.concentrated_load
        End Get
    End Property
End Class