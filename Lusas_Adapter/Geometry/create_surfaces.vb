Imports System.Collections.Generic
Imports System

Imports Grasshopper.Kernel
Imports Rhino.Geometry

Imports RMA.OpenNURBS
Imports LusasM15_2


Public Class create_surfaces
    Inherits GH_Component

    Public Overrides ReadOnly Property Exposure() As GH_Exposure
        Get
            Return GH_Exposure.primary
        End Get
    End Property

    Public Sub New()
        MyBase.New(“create_surfaces", "cS",
                   "Creates surfaces in LUSAS",
                   "Bridges Tools", "Geometry")
    End Sub

    Protected Overrides Sub RegisterInputParams(ByVal pManager As GH_InputParamManager)
        pManager.AddTextParameter("name", "n", "Name of geometry group", GH_ParamAccess.item)
        pManager.AddBrepParameter("brep_List", "bL", "Brep list to create geometry: surfaces", GH_ParamAccess.list)
        pManager.AddBooleanParameter("active?", "act?", "active component?", GH_ParamAccess.item)
    End Sub
    Protected Overrides Sub RegisterOutputParams(ByVal pManager As GH_OutputParamManager)
        pManager.Register_IntegerParam("surface_ID", "sID", "To be used in other LUSAS components")
        pManager.Register_IntegerParam("point_ID", "pID", "To be used in other LUSAS components", GH_ParamAccess.tree)
    End Sub

    Protected Overrides Sub SolveInstance(ByVal Da As IGH_DataAccess)
        Dim activate As Boolean = False
        If (Not Da.GetData(2, activate)) Then Return
        If activate Then
            Dim modeller As LusasWinApp = lusas_modeller.m_lusas
            modeller.suppressMessages(0)
            modeller.textWin.writeLine("Creating surfaces...")
            modeller.suppressMessages(1)
            Dim group_name As String = ""
            If (Not Da.GetData(0, group_name)) Then Return

            Dim surface_group As IFObjectSet

            'Check if geometry group exists
            If modeller.db.existsGroupByName(group_name) Then
                surface_group = modeller.db.getGroupByName(group_name)
                surface_group.Delete("Surface")
                surface_group.Delete("Line")
                surface_group.Delete("Point")
            Else
                surface_group = modeller.db.createGroup(group_name)
            End If

            Dim ID_list As New List(Of Double)
            Dim surfaceDB As IFSurface

            Dim breps As New List(Of Brep)
            If (Not Da.GetDataList(1, breps)) Then Return

            'Join curves
            'Flip curve 
            'Recreate brep
            'Deconstruct brep and proceed

            Dim geomData As IFGeometryData = modeller.newGeometryData
            geomData.mergeDefiningGeometryOn()
            For Each brep As Brep In breps
                'Skip over invalid inputs
                If Not brep.IsValid Then Continue For

                Dim linesDB As IFLine
                Dim temp_set As IFObjectSet = modeller.db.createGroup("temp")
                For Each brep_edge As Curve In brep.DuplicateEdgeCurves
                    linesDB = modeller.db.createLineByCoordinates(brep_edge.PointAtStart.X, brep_edge.PointAtStart.Y,
                                                        brep_edge.PointAtStart.Z, brep_edge.PointAtEnd.X,
                                                        brep_edge.PointAtEnd.Y, brep_edge.PointAtEnd.Z)
                    temp_set.add("Line", linesDB.getID)
                Next brep_edge

                'Create surface in Lusas
                temp_set.merge("Point")
                temp_set.removeLOF()
                surfaceDB = modeller.db.createSurfaceBy(temp_set)
                ID_list.Add(surfaceDB.getID())
                surface_group.add("Surface", surfaceDB.getID)
                modeller.db.getGroupByName("temp").ungroup()
            Next brep

            modeller.suppressMessages(0)
            Dim final_msg As String = surface_group.count("Surface") & " surfaces created and added to" & group_name
            modeller.textWin.writeLine(final_msg)
            modeller.suppressMessages(1)
            'Add Lower Order Features (points) to geometry group so they can be deleted when rerun
            surface_group.addLOF()

            surface_group.merge("Point")
            surface_group.merge("Line")

            Da.SetDataList(0, ID_list)
            modeller.suppressMessages(0)
        End If
    End Sub

    Public Overrides ReadOnly Property ComponentGuid() As Guid
        Get
            Return New Guid("{fec3b2c3-abec-4ba7-b545-b8be365cd82c}")
        End Get
    End Property

    Protected Overrides ReadOnly Property Internal_Icon_24x24() As System.Drawing.Bitmap
        Get
            Return My.Resources.create_surface
        End Get
    End Property
End Class