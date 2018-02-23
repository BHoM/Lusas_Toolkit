Imports System.Collections.Generic
Imports System

Imports Grasshopper.Kernel
Imports Rhino.Geometry

Imports RMA.OpenNURBS
Imports LusasM15_2


Public Class mesh_surface_regular_div
    Inherits GH_Component

    Public Overrides ReadOnly Property Exposure() As GH_Exposure
        Get
            Return GH_Exposure.primary
        End Get
    End Property

    Public Sub New()
        MyBase.New(“mesh_surface_regular_div", "s_r_d",
                   "Create a regular surface mesh using divisions",
                   "Bridges Tools", "Attributes")
    End Sub

    Protected Overrides Sub RegisterInputParams(ByVal pManager As GH_InputParamManager)
        pManager.AddTextParameter("mesh_name", "m_n", "name to be assigned to mesh", GH_ParamAccess.item)
        pManager.AddTextParameter("element_type", "et", "triangular or quadilateral", GH_ParamAccess.item)
        pManager.AddIntegerParameter("x_divisions", "x", "number of x divisions", GH_ParamAccess.item)
        pManager.AddIntegerParameter("y_divisions", "y", "number of y divisions", GH_ParamAccess.item)
        pManager.AddBooleanParameter("transitions", "trans", "Allow transition mesh patterns", GH_ParamAccess.item)
        pManager.AddIntegerParameter("surface_ID", "ID", "Geometry to assign mesh to", GH_ParamAccess.list)
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

            Dim name As String = ""
            If (Not Da.GetData(0, name)) Then Return

            'Initialise mesh
            Dim surface_mesh As IFMeshSurface = modeller.db.createMeshSurface(name)

            Dim element_type As String = ""
            If (Not Da.GetData(1, element_type)) Then Return
            Dim x_division As Integer = 0
            If (Not Da.GetData(2, x_division)) Then Return
            Dim y_division As Integer = 0
            If (Not Da.GetData(3, y_division)) Then Return
            Dim transition As Boolean = False
            If (Not Da.GetData(4, transition)) Then Return

            If x_division = 0 And y_division = 0 Then
                surface_mesh.setRegular(element_type,, transition)
            ElseIf x_division = 0 Then
                surface_mesh.setRegular(element_type,, y_division, transition)
            ElseIf y_division = 0 Then
                surface_mesh.setRegular(element_type, x_division,, transition)
            End If

            'Get object list and type
            Dim obj_ID As New List(Of Integer)
            If (Not Da.GetDataList(5, obj_ID)) Then Return

            'Create Object set And assign
            Dim obj_set As IFObjectSet = modeller.newObjectSet
            For Each value As Integer In obj_ID
                obj_set.add("surface", value)
            Next

            surface_mesh.assignTo(obj_set)
            modeller.database.updateMesh()

            Da.SetData(0, surface_mesh.getID)
        End If
    End Sub

    Public Overrides ReadOnly Property ComponentGuid() As Guid
        Get
            Return New Guid("{2473bc85-849c-4ba0-ba10-fb3ca3859363}")
        End Get
    End Property

    Protected Overrides ReadOnly Property Internal_Icon_24x24() As System.Drawing.Bitmap
        Get
            Return My.Resources.surface_mesh
        End Get
    End Property
End Class