Imports System.Collections.Generic
Imports System

Imports Grasshopper.Kernel
Imports Rhino.Geometry

Imports RMA.OpenNURBS
Imports LusasM15_2


Public Class mesh_line_div_no_release
    Inherits GH_Component

    Public Overrides ReadOnly Property Exposure() As GH_Exposure
        Get
            Return GH_Exposure.primary
        End Get
    End Property

    Public Sub New()
        MyBase.New(“line_mesh_div_no_release", "l_m_d_n_r",
                   "Create a line mesh using divisions without end releases",
                   "Bridges Tools", "Attributes")
    End Sub

    Protected Overrides Sub RegisterInputParams(ByVal pManager As GH_InputParamManager)
        pManager.AddTextParameter("mesh_name", "m_n", "name to be assigned to mesh", GH_ParamAccess.item)
        pManager.AddTextParameter("element type", "e_t", "structural element type", GH_ParamAccess.item)
        pManager.AddIntegerParameter("number_of_divisions", "n_d", "number of divisions on line", GH_ParamAccess.item)
        pManager.AddNumberParameter("transtition_ratio", "tra_r", "uniform stransistion ratio of first to last element", GH_ParamAccess.item)
        pManager.AddIntegerParameter("line_ID", "ID", "Geometry to assign mesh to", GH_ParamAccess.list)
        pManager.AddNumberParameter("beta_a", "b_a", "beta angle to apply to mesh", GH_ParamAccess.item)
        pManager.AddBooleanParameter("active?", "act?", "active component?", GH_ParamAccess.item)
    End Sub
    Protected Overrides Sub RegisterOutputParams(ByVal pManager As GH_OutputParamManager)
        pManager.Register_IntegerParam("mesh_ID", "m_ID", "ID to be used for other LUSAS components")
    End Sub

    Protected Overrides Sub SolveInstance(ByVal Da As IGH_DataAccess)
        Dim activate As Boolean = False
        If (Not Da.GetData(6, activate)) Then Return
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

            'Get object list and type
            Dim obj_ID As New List(Of Integer)
            If (Not Da.GetDataList(4, obj_ID)) Then Return

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
            Return New Guid("{0fd4273a-ea73-450e-b735-8d58e1575447}")
        End Get
    End Property

    Protected Overrides ReadOnly Property Internal_Icon_24x24() As System.Drawing.Bitmap
        Get
            Return My.Resources.line_mesh_div
        End Get
    End Property
End Class