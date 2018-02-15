Imports System.Collections.Generic
Imports System

Imports Grasshopper.Kernel
Imports Rhino.Geometry

Imports RMA.OpenNURBS
Imports LusasM15_2


Public Class create_lines
    Inherits GH_Component

    Public Overrides ReadOnly Property Exposure() As GH_Exposure
        Get
            Return GH_Exposure.primary
        End Get
    End Property

    Public Sub New()
        MyBase.New(“create_lines", "cL",
                   "Creates lines in LUSAS",
                   "Bridges Tools", "Geometry")
    End Sub

    Protected Overrides Sub RegisterInputParams(ByVal pManager As GH_InputParamManager)
        pManager.AddTextParameter("name", "n", "Name of geometry group", GH_ParamAccess.item)
        pManager.AddCurveParameter("line_List", "lL", "Line list to create geometry: lines", GH_ParamAccess.list)
        pManager.AddBooleanParameter("active?", "act?", "active component?", GH_ParamAccess.item)
    End Sub
    Protected Overrides Sub RegisterOutputParams(ByVal pManager As GH_OutputParamManager)
        pManager.Register_IntegerParam("line_ID", "lID", "To be used in other LUSAS components")
        pManager.Register_IntegerParam("point_ID", "pID", "To be used in other LUSAS components", GH_ParamAccess.tree)
    End Sub

    Protected Overrides Sub SolveInstance(ByVal Da As IGH_DataAccess)
        Dim activate As Boolean = False
        If (Not Da.GetData(2, activate)) Then Return
        If activate Then
            Dim modeller As LusasWinApp = lusas_modeller.m_lusas
            modeller.suppressMessages(0)
            modeller.textWin.writeLine("Creating lines...")
            modeller.suppressMessages(1)

            Dim group_name As String = ""
            If (Not Da.GetData(0, group_name)) Then Return

            Dim line_group As IFObjectSet

            'Check if geometry group exists
            If modeller.db.existsGroupByName(group_name) Then
                line_group = modeller.db.getGroupByName(group_name)
                line_group.Delete("Line")
                line_group.Delete("Point")
            Else
                line_group = modeller.db.createGroup(group_name)
            End If

            Dim ID_list As New List(Of Double)
            Dim linesDB As IFLine

            Dim lines As New List(Of Curve)
            If (Not Da.GetDataList(1, lines)) Then Return

            For Each line As Curve In lines
                'Skip over invalid inputs
                If Not line.IsValid Then Continue For
                linesDB = modeller.db.createLineByCoordinates(
            line.PointAtStart.X, line.PointAtStart.Y, line.PointAtStart.Z,
                line.PointAtEnd.X, line.PointAtEnd.Y, line.PointAtEnd.Z)
                ID_list.Add(linesDB.getID())
                line_group.add("Line", linesDB.getID)
            Next line

            modeller.suppressMessages(0)
            Dim final_msg As String = line_group.count("line") & " lines created and added to" & group_name
            modeller.textWin.writeLine(final_msg)
            modeller.suppressMessages(1)

            'Add Lower Order Features (points) to geometry group so they can be deleted when rerun
            line_group.addLOF()

            line_group.merge("Point")

            Da.SetDataList(0, ID_list)

            modeller.suppressMessages(0)

        End If
    End Sub

    Public Overrides ReadOnly Property ComponentGuid() As Guid
        Get
            Return New Guid("{7bb394a8-6c2f-4ee7-8ccc-5025f9c5c0c5}")
        End Get
    End Property

    Protected Overrides ReadOnly Property Internal_Icon_24x24() As System.Drawing.Bitmap
        Get
            Return My.Resources.create_line
        End Get
    End Property
End Class