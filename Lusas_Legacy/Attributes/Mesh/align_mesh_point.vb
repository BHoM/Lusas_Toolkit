Imports System.Collections.Generic
Imports System

Imports Grasshopper.Kernel
Imports Rhino.Geometry

Imports RMA.OpenNURBS
Imports LusasM15_2


Public Class align_mesh_point
    Inherits GH_Component

    Public Overrides ReadOnly Property Exposure() As GH_Exposure
        Get
            Return GH_Exposure.primary
        End Get
    End Property

    Public Sub New()
        MyBase.New(“align_mesh_point", "a_m_p",
                   "Create a line mesh using divisions",
                   "Bridges Tools", "Attributes")
    End Sub

    Protected Overrides Sub RegisterInputParams(ByVal pManager As GH_InputParamManager)
        pManager.AddIntegerParameter("PointID_list", "pID", "List of points to align line mesh to", GH_ParamAccess.list)
        pManager.AddIntegerParameter("LineID_list", "lID", "List of lines to modify mesh", GH_ParamAccess.list)
        pManager.AddIntegerParameter("MeshID", "mID", "Mesh ID to modify", GH_ParamAccess.item)
        pManager.AddBooleanParameter("active?", "act?", "active component?", GH_ParamAccess.item)
    End Sub
    Protected Overrides Sub RegisterOutputParams(ByVal pManager As GH_OutputParamManager)
    End Sub

    Protected Overrides Sub SolveInstance(ByVal Da As IGH_DataAccess)
        Dim activate As Boolean = False
        If (Not Da.GetData(3, activate)) Then Return
        If activate Then
            Dim modeller As LusasWinApp = lusas_modeller.m_lusas

            Dim orientation_point_list As New List(Of Integer)
            If (Not Da.GetDataList(0, orientation_point_list)) Then Return
            Dim line_align_list As New List(Of Integer)
            If (Not Da.GetDataList(1, line_align_list)) Then Return
            Dim mesh_ID As New Integer
            If (Not Da.GetData(2, mesh_ID)) Then Return

            For i As Integer = 0 To orientation_point_list.Count - 1
                Dim assignment As IFAssignment = modeller.newAssignment
                Dim orientation_point As IFPoint = modeller.db.getObject("Point", orientation_point_list(i))
                assignment.setAllDefaults()
                assignment.setOrientationPoint(orientation_point)
                Dim align_line As IFLine = modeller.db.getObject("Line", line_align_list(i))
                modeller.db.getAttribute("Line Mesh", mesh_ID).assignTo(align_line, assignment)
                modeller.db.updateMesh()
            Next i
        End If
    End Sub

    Public Overrides ReadOnly Property ComponentGuid() As Guid
        Get
            Return New Guid("{f475bbcf-de57-442e-928a-d5ba97786ade}")
        End Get
    End Property

    Protected Overrides ReadOnly Property Internal_Icon_24x24() As System.Drawing.Bitmap
        Get
            Return My.Resources.line_mesh_div
        End Get
    End Property
End Class