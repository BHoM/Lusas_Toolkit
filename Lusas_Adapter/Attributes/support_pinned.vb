Imports System.Collections.Generic
Imports System

Imports Grasshopper.Kernel
Imports Rhino.Geometry

Imports RMA.OpenNURBS
Imports LusasM15_2


Public Class support_pinned
    Inherits GH_Component

    Public Overrides ReadOnly Property Exposure() As GH_Exposure
        Get
            Return GH_Exposure.primary
        End Get
    End Property

    Public Sub New()
        MyBase.New(“support_pinned", "sup_pin",
                   "Create and assign support conditions to points and lines",
                   "Bridges Tools", "Attributes")
    End Sub

    Protected Overrides Sub RegisterInputParams(pManager As GH_Component.GH_InputParamManager)
        pManager.AddTextParameter("support_name", "sN", "Support name", GH_ParamAccess.item)
        pManager.AddTextParameter("translation_x", "Fx", "R = Restrained, F = Free", GH_ParamAccess.item)
        pManager.AddTextParameter("translation_y", "Fy", "R = Restrained, F = Free", GH_ParamAccess.item)
        pManager.AddTextParameter("translation_z", "Fz", "R = Restrained, F = Free", GH_ParamAccess.item)
        pManager.AddIntegerParameter("point_ID", "ID", "Points to assign support to", GH_ParamAccess.list)
        pManager.AddTextParameter("object_type", "type", "line or point", GH_ParamAccess.item)
        pManager.AddBooleanParameter("active?", "act?", "active component?", GH_ParamAccess.item)
    End Sub

    Protected Overrides Sub RegisterOutputParams(pManager As GH_Component.GH_OutputParamManager)
        pManager.Register_IntegerParam("support_ID", "s_ID", "ID to be used for other LUSAS components")
    End Sub

    Protected Overrides Sub SolveInstance(DA As IGH_DataAccess)
        Dim activate As Boolean = False
        If (Not DA.GetData(6, activate)) Then Return
        If activate Then
            Dim modeller As LusasWinApp = lusas_modeller.m_lusas

            'Retrieve structural profile for free/fixed
            Dim name As String = ""
            If (Not DA.GetData(0, name)) Then Return

            Dim Fx As String = "R"
            If (Not DA.GetData(1, Fx)) Then Return

            Dim Fy As String = "R"
            If (Not DA.GetData(2, Fy)) Then Return

            Dim Fz As String = "R"
            If (Not DA.GetData(3, Fz)) Then Return

            Dim free As String = "F"

            'Create support
            Dim support As IFSupport = modeller.db.createSupportStructural(name)
            support.setStructural(Fx, Fy, Fz, free, free, free, free, free, free)

            'Get object list and type
            Dim obj_ID As New List(Of Integer)
            If (Not DA.GetDataList(4, obj_ID)) Then Return
            Dim obj_type As String = ""
            If (Not DA.GetData(5, obj_type)) Then Return

            'Create object set and assign
            Dim obj_set As IFObjectSet = modeller.newObjectSet
            For Each value As Integer In obj_ID
                obj_set.add(obj_type, value)
            Next

            support.assignTo(obj_set)

            DA.SetData(0, support.getID)
        End If
    End Sub

    Protected Overrides ReadOnly Property Icon() As System.Drawing.Bitmap
        Get
            Return My.Resources.support
        End Get
    End Property

    Public Overrides ReadOnly Property ComponentGuid() As Guid
        Get
            Return New Guid("970693d5-07d3-4a9a-84fc-92ff3be37af2")
        End Get
    End Property
End Class