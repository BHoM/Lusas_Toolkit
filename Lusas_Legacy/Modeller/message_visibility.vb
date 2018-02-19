Imports System.Collections.Generic
Imports System

Imports Grasshopper.Kernel
Imports Rhino.Geometry

Imports RMA.OpenNURBS
Imports LusasM15_2


Public Class message_visibility
    Inherits GH_Component

    Public Overrides ReadOnly Property Exposure() As GH_Exposure
        Get
            Return GH_Exposure.primary
        End Get
    End Property

    Public Sub New()
        MyBase.New(“message_visibility", "msg",
                   "Set supression level of modeller messages",
                   "Bridges Tools", "Loading")
    End Sub

    Protected Overrides Sub RegisterInputParams(ByVal pManager As GH_InputParamManager)
        pManager.AddIntegerParameter("Supression level", "Sprs", "0, 1 or 2", GH_ParamAccess.item)
    End Sub
    Protected Overrides Sub RegisterOutputParams(ByVal pManager As GH_OutputParamManager)
        pManager.Register_StringParam("Supression level", "sL", "Supression level", GH_ParamAccess.item)
    End Sub

    Protected Overrides Sub SolveInstance(ByVal Da As IGH_DataAccess)
        Dim activate As Boolean = False
        If (Not Da.GetData(1, activate)) Then Return
        If activate Then
            Dim modeller As LusasWinApp = lusas_modeller.m_lusas
            Dim iSupress As Integer
            If (Not Da.GetData(0, iSupress)) Then Return
            modeller.suppressMessages(iSupress)

            Dim supression_level As String = ""
            If iSupress = 0 Then
                supression_level = "Show all messages"
            ElseIf iSupress = 1 Then
                supression_level = "Show no messages at all"
            Else
                supression_level = "Show all messages (including popups) only in text output window."
            End If

        End If
    End Sub

    Public Overrides ReadOnly Property ComponentGuid() As Guid
        Get
            Return New Guid("{2935878e-c062-4c2d-9dc3-73a95a82e498}")
        End Get
    End Property

    Protected Overrides ReadOnly Property Internal_Icon_24x24() As System.Drawing.Bitmap
        Get
            Return My.Resources.create_loadcase
        End Get
    End Property
End Class