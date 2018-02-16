Imports System.Collections.Generic
Imports System

Imports Grasshopper.Kernel
Imports Rhino.Geometry

Imports RMA.OpenNURBS
Imports LusasM15_2


Public Class create_loadcase
    Inherits GH_Component

    Public Overrides ReadOnly Property Exposure() As GH_Exposure
        Get
            Return GH_Exposure.primary
        End Get
    End Property

    Public Sub New()
        MyBase.New(“create_loadcase", "cLc",
                   "Create loadcase to be used for assignments",
                   "Bridges Tools", "Loading")
    End Sub

    Protected Overrides Sub RegisterInputParams(ByVal pManager As GH_InputParamManager)
        pManager.AddTextParameter("loadcase_name", "lcN", "Name of loadcase", GH_ParamAccess.item)
        pManager.AddTextParameter("analysis_name", "an", "Name of analysis [optional]", GH_ParamAccess.item)
        pManager.AddIntegerParameter("loadcase_ID", "lcID", "Force loadcase ID [optional]", GH_ParamAccess.item)
        pManager.AddBooleanParameter("active?", "act?", "active component?", GH_ParamAccess.item)
    End Sub
    Protected Overrides Sub RegisterOutputParams(ByVal pManager As GH_OutputParamManager)
        pManager.Register_StringParam("loadcase_name", "lc_n", "Loadcase name to be used for other LUSAS components")
    End Sub

    Protected Overrides Sub SolveInstance(ByVal Da As IGH_DataAccess)
        Dim activate As Boolean = False
        If (Not Da.GetData(3, activate)) Then Return
        If activate Then
            Dim modeller As LusasWinApp = lusas_modeller.m_lusas

            'Retrieve inputs
            Dim name As String = ""
            If (Not Da.GetData(0, name)) Then Return

            'DEVELOP: Check for invalid analysis name, if invalid default to first analysis name
            Dim analysis_name As String = ""
            Da.GetData(1, analysis_name)

            Dim loadcase_ID As Integer = modeller.db.getNextAvailableLoadcaseID
            Da.GetData(2, loadcase_ID)

            'Create new loadcase
            If analysis_name = "" And loadcase_ID = 0 Then
                modeller.db.createLoadcase(name)
            ElseIf analysis_name = "" Then
                modeller.db.createLoadcase(name,, loadcase_ID)
            ElseIf loadcase_ID = 0 Then
                modeller.db.createLoadcase(name, analysis_name,)
            Else
                modeller.db.createLoadcase(name, analysis_name, loadcase_ID)
            End If

            Da.SetData(0, name)

        End If
    End Sub

    Public Overrides ReadOnly Property ComponentGuid() As Guid
        Get
            Return New Guid("{60970167-ec69-4588-9ff5-2193683be72c}")
        End Get
    End Property

    Protected Overrides ReadOnly Property Internal_Icon_24x24() As System.Drawing.Bitmap
        Get
            Return My.Resources.create_loadcase
        End Get
    End Property
End Class