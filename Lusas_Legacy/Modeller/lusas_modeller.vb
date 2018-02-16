Imports System.Collections.Generic
Imports System

Imports Grasshopper.Kernel
Imports Rhino.Geometry

Imports RMA.OpenNURBS
Imports LusasM15_2

Public Class lusas_modeller
    Inherits Grasshopper.Kernel.GH_Component
    Public Sub New()
        MyBase.New(“modeller", "luM",
                   "Runs LUSAS Modeller",
                   "Bridges Tools", "Lusas")
    End Sub

    Public Overrides ReadOnly Property Exposure() As GH_Exposure
        Get
            Return GH_Exposure.primary
        End Get
    End Property

    Protected Overrides Sub RegisterInputParams(ByVal pManager As GH_InputParamManager)
        pManager.AddBooleanParameter("execute", "E", "Execute?", GH_ParamAccess.item)
        pManager.AddTextParameter("file_name", "fN", "File name for database", GH_ParamAccess.item)
        pManager.AddBooleanParameter("open?", "O/N", "Open an exisiting database, otherwise create a new one", GH_ParamAccess.item)
    End Sub

    Protected Overrides Sub RegisterOutputParams(ByVal pManager As GH_OutputParamManager)
    End Sub

    'So instance can be used across components
    Public Shared m_lusas As LusasWinApp
    Protected Overrides Sub SolveInstance(ByVal Da As IGH_DataAccess)
        Dim Execute As Boolean = False
        If (Da.GetData(0, Execute)) Then
            If Execute Then
                ' Create an instance of modeller
                m_lusas = New LusasM15_2.LusasWinApp

                'Set model properties and save
                Dim fileName As String = Nothing
                If (Not Da.GetData(1, fileName)) Then Return

                Dim open As Boolean = False
                If (Not Da.GetData(2, open)) Then Return
                If open Then
                    m_lusas.openDatabase(fileName)
                Else
                    m_lusas.newDatabase()
                    m_lusas.db.setLogicalUpAxis("Z")
                    m_lusas.db.setModelUnits("kN,m,t,s,C")
                    m_lusas.db.saveAs(fileName)
                    m_lusas.database.setTimescaleUnits("Seconds")
                    m_lusas.textWin.writeLine("Model saved")
                End If

            End If
        End If
    End Sub

    Public Overrides ReadOnly Property ComponentGuid() As Guid
        Get
            Return New Guid("{4a12f763-6da2-42be-9a30-1fc48d56cf98}")
        End Get
    End Property

    Protected Overrides ReadOnly Property Internal_Icon_24x24() As System.Drawing.Bitmap
        Get
            Return My.Resources.lusas_modeller
        End Get
    End Property
End Class