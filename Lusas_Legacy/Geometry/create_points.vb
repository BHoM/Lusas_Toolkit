Imports System.Collections.Generic
Imports System

Imports Grasshopper.Kernel
Imports Rhino.Geometry

Imports RMA.OpenNURBS
Imports LusasM15_2


Public Class create_points
    Inherits Grasshopper.Kernel.GH_Component

    Public Overrides ReadOnly Property Exposure() As GH_Exposure
        Get
            Return GH_Exposure.primary
        End Get
    End Property

    Public Sub New()
        MyBase.New(“create_points", "cPo",
                   "Creates points in LUSAS",
                   "Bridges Tools", "Geometry")
    End Sub
    Protected Overrides Sub RegisterInputParams(ByVal pManager As GH_InputParamManager)
        pManager.AddPointParameter("point_list", "pL", "Point list to create geometry: points", GH_ParamAccess.list)
        pManager.AddBooleanParameter("delete?", "d", "Delete existing points?", GH_ParamAccess.item)
        pManager.AddBooleanParameter("active?", "act?", "active component?", GH_ParamAccess.item)
    End Sub
    Protected Overrides Sub RegisterOutputParams(ByVal pManager As GH_OutputParamManager)
    End Sub

    Protected Overrides Sub SolveInstance(ByVal Da As IGH_DataAccess)
        Dim activate As Boolean = False
        If (Not Da.GetData(2, activate)) Then Return
        If activate Then
            Dim modeller As LusasM15_2.LusasWinApp = lusas_modeller.m_lusas

            'Declare placeholder variables and assign initial invalid data.
            Dim points As New List(Of Rhino.Geometry.Point3d)

            'Retrieve input data.
            If (Not Da.GetDataList(0, points)) Then Return

            ' Get the geometry data object
            Dim geomData As LusasM15_2.IFGeometryData = modeller.geometryData()

            modeller.textWin.writeLine("Set defaults")

            ' Set the defaults
            geomData.setAllDefaults()

            modeller.textWin.writeLine("Add coordinates")

            Dim delete As Boolean = False
            'Retrieve input data.
            If (Da.GetData(1, delete)) Then
                'Remove all points in the model
                geomData.removeAllCoords()
                modeller.db.deleteContents(geomData)
            End If

            For i = 0 To points.Count - 1
                'Abort on invalid igrnputs.
                If Not points(i).IsValid Then Return
                geomData.addCoords(points(i).X, points(i).Y, points(i).Z)
            Next
            modeller.textWin.writeLine("Coordinates added")

            modeller.textWin.writeLine("Create geometric points")
            Dim pointDB As IFObjectSet = modeller.db.createPoint(geomData)
            modeller.textWin.writeLine("Geometric points created")
        End If
    End Sub

    Public Overrides ReadOnly Property ComponentGuid() As Guid
        Get
            Return New Guid("{1b902c7c-c230-4f31-972a-4cb3d9325f32}")
        End Get
    End Property

    Protected Overrides ReadOnly Property Internal_Icon_24x24() As System.Drawing.Bitmap
        Get
            Return My.Resources.create_point
        End Get
    End Property
End Class