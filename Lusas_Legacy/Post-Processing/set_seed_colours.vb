Imports System.Collections.Generic
Imports System

Imports Grasshopper.Kernel
Imports Rhino.Geometry

Imports RMA.OpenNURBS
Imports LusasM15_2


Public Class set_seed_colours
    Inherits Grasshopper.Kernel.GH_Component

    Public Overrides ReadOnly Property Exposure() As GH_Exposure
        Get
            Return GH_Exposure.primary
        End Get
    End Property

    Public Sub New()
        MyBase.New(“set_seed_colours", "sSC",
                   "Set seed colours for contours",
                   "Bridges Tools", "Post-Processing")
    End Sub
    Protected Overrides Sub RegisterInputParams(ByVal pManager As GH_InputParamManager)
        pManager.AddIntegerParameter("colours", "c", "Seed colours as hex integers", GH_ParamAccess.list)
        pManager.AddNumberParameter("intervals", "i", "Intervals to be manually added", GH_ParamAccess.list)
        pManager.AddBooleanParameter("active?", "act?", "active component?", GH_ParamAccess.item)
    End Sub
    Protected Overrides Sub RegisterOutputParams(ByVal pManager As GH_OutputParamManager)
    End Sub

    Protected Overrides Sub SolveInstance(ByVal Da As IGH_DataAccess)
        Dim activate As Boolean = False
        If (Not Da.GetData(2, activate)) Then Return
        If activate Then
            Dim modeller As LusasM15_2.LusasWinApp = lusas_modeller.m_lusas

            Dim colours As New List(Of Integer)
            If (Not Da.GetDataList(0, colours)) Then Return

            Dim interval As New List(Of Double)
            If (Not Da.GetDataList(1, interval)) Then Return

            modeller.view.contours.setSeedColours(colours.ToArray)
            modeller.view.contours.setRangeManual(interval.ToArray)

        End If
    End Sub

    Public Overrides ReadOnly Property ComponentGuid() As Guid
        Get
            Return New Guid("{ed30d6c8-c5ff-4083-a135-e56deb4d745c}")
        End Get
    End Property

    Protected Overrides ReadOnly Property Internal_Icon_24x24() As System.Drawing.Bitmap
        Get
            Return My.Resources.create_point
        End Get
    End Property
End Class