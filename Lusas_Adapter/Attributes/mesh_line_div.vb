Imports System.Collections.Generic
Imports System

Imports Grasshopper.Kernel
Imports Rhino.Geometry

Imports RMA.OpenNURBS
Imports LusasM15_2


Public Class mesh_line_div
    Inherits GH_Component

    Public Overrides ReadOnly Property Exposure() As GH_Exposure
        Get
            Return GH_Exposure.primary
        End Get
    End Property

    Public Sub New()
        MyBase.New(“line_mesh_div", "l_m_d",
                   "Create a line mesh using divisions",
                   "Bridges Tools", "Attributes")
    End Sub

    Protected Overrides Sub RegisterInputParams(ByVal pManager As GH_InputParamManager)
        pManager.AddTextParameter("mesh_name", "m_n", "name to be assigned to mesh", GH_ParamAccess.item)
        pManager.AddTextParameter("element type", "e_t", "structural element type", GH_ParamAccess.item)
        pManager.AddIntegerParameter("number_of_divisions", "n_d", "number of divisions on line", GH_ParamAccess.item)
        pManager.AddNumberParameter("transtition_ratio", "tra_r", "uniform stransistion ratio of first to last element", GH_ParamAccess.item)
        pManager.AddBooleanParameter("release_both", "r", "releases same on start/end?", GH_ParamAccess.item)
        pManager.AddBooleanParameter("thx_start", "thx_s", "release rotation about x at start", GH_ParamAccess.item)
        pManager.AddBooleanParameter("thy_start", "thy_s", "release rotation about y at start", GH_ParamAccess.item)
        pManager.AddBooleanParameter("thz_start", "thz_s", "release rotation about z at start", GH_ParamAccess.item)
        pManager.AddBooleanParameter("thx_end", "thx_e", "release rotation about x at end", GH_ParamAccess.item)
        pManager.AddBooleanParameter("thy_end", "thy_e", "release rotation about y at end", GH_ParamAccess.item)
        pManager.AddBooleanParameter("thz_end", "thz_e", "release rotation about z at end", GH_ParamAccess.item)
        pManager.AddIntegerParameter("line_ID", "ID", "Geometry to assign mesh to", GH_ParamAccess.list)
        pManager.AddNumberParameter("beta_a", "b_a", "beta angle to apply to mesh", GH_ParamAccess.item)
        pManager.AddBooleanParameter("active?", "act?", "active component?", GH_ParamAccess.item)
    End Sub
    Protected Overrides Sub RegisterOutputParams(ByVal pManager As GH_OutputParamManager)
        pManager.Register_IntegerParam("mesh_ID", "m_ID", "ID to be used for other LUSAS components")
    End Sub

    Protected Overrides Sub SolveInstance(ByVal Da As IGH_DataAccess)
        Dim activate As Boolean = False
        If (Not Da.GetData(13, activate)) Then Return
        If activate Then
            Dim modeller As LusasWinApp = lusas_modeller.m_lusas

            'Retrieve inputs
            Dim name As String = ""
            If (Not Da.GetData(0, name)) Then Return

            'Initialise mesh
            Dim line_mesh As IFMeshLine = modeller.db.createMeshLine(name)

            'Set structural element type
            Dim type As String = ""
            If (Not Da.GetData(1, type)) Then Return

            line_mesh.setSpacing(type)

            'Set division number
            Dim div As Integer = 0
            If (Not Da.GetData(2, div)) Then Return
            Dim tran_ratio As Double = 0
            If (Not Da.GetData(3, tran_ratio)) Then Return
            line_mesh.addSpacing(div, tran_ratio)

            'Set releases
            Dim is_same As Boolean = False
            If (Not Da.GetData(4, is_same)) Then Return

            line_mesh.setEndReleasesSameAsStart(is_same)

            'Retrieve releases
            Dim thx_s As Boolean = False
            If (Not Da.GetData(5, thx_s)) Then Return
            Dim thy_s As Boolean = False
            If (Not Da.GetData(6, thy_s)) Then Return
            Dim thz_s As Boolean = False
            If (Not Da.GetData(7, thz_s)) Then Return
            Dim thx_e As Boolean = False
            If (Not Da.GetData(8, thx_e)) Then Return
            Dim thy_e As Boolean = False
            If (Not Da.GetData(9, thy_e)) Then Return
            Dim thz_e As Boolean = False
            If (Not Da.GetData(10, thz_e)) Then Return


            If is_same Then
                If thx_s Then
                    line_mesh.setRelease("Start", "THX")
                End If
                If thx_s Then
                    line_mesh.setRelease("Start", "THY")
                End If
                If thx_s Then
                    line_mesh.setRelease("Start", "THZ")
                End If
            Else
                If thx_s Then
                    line_mesh.setRelease("Start", "THX")
                End If
                If thx_s Then
                    line_mesh.setRelease("Start", "THY")
                End If
                If thx_s Then
                    line_mesh.setRelease("Start", "THZ")
                End If
                If thx_s Then
                    line_mesh.setRelease("End", "THX")
                End If
                If thx_s Then
                    line_mesh.setRelease("End", "THY")
                End If
                If thx_s Then
                    line_mesh.setRelease("End", "THZ")
                End If
            End If

            'Get object list and type
            Dim obj_ID As New List(Of Integer)
            If (Not Da.GetDataList(11, obj_ID)) Then Return

            'Create Object set And assign
            Dim obj_set As IFObjectSet = modeller.newObjectSet
            For Each value As Integer In obj_ID
                obj_set.add("line", value)
            Next

            line_mesh.assignTo(obj_set)

            Da.SetData(0, line_mesh.getID)
        End If
    End Sub

    Public Overrides ReadOnly Property ComponentGuid() As Guid
        Get
            Return New Guid("{038a7162-e3b2-446f-b066-688d3e46ba34}")
        End Get
    End Property

    Protected Overrides ReadOnly Property Internal_Icon_24x24() As System.Drawing.Bitmap
        Get
            Return My.Resources.line_mesh_div
        End Get
    End Property
End Class