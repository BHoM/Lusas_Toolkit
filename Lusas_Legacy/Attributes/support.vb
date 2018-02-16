Imports System.Collections.Generic
Imports System

Imports Grasshopper.Kernel
Imports Rhino.Geometry

Imports RMA.OpenNURBS
Imports LusasM15_2


Public Class support
    Inherits GH_Component

    Public Overrides ReadOnly Property Exposure() As GH_Exposure
        Get
            Return GH_Exposure.primary
        End Get
    End Property

    Public Sub New()
        MyBase.New(“support", "sup",
                   "Create and assign support conditions to points and lines",
                   "Bridges Tools", "Attributes")
    End Sub

    Protected Overrides Sub RegisterInputParams(pManager As GH_Component.GH_InputParamManager)
        pManager.AddTextParameter("support_name", "sN", "Support name", GH_ParamAccess.item)
        pManager.AddTextParameter("translation_x", "Fx", "R = Restrained, F = Free", GH_ParamAccess.item)
        pManager.AddTextParameter("translation_y", "Fy", "R = Restrained, F = Free", GH_ParamAccess.item)
        pManager.AddTextParameter("translation_z", "Fz", "R = Restrained, F = Free", GH_ParamAccess.item)
        pManager.AddTextParameter("rotation_x", "Mx", "R = Restrained, F = Free", GH_ParamAccess.item)
        pManager.AddTextParameter("rotation_y", "My", "R = Restrained, F = Free", GH_ParamAccess.item)
        pManager.AddTextParameter("rotation_z", "Mz", "R = Restrained, F = Free", GH_ParamAccess.item)
        pManager.AddTextParameter("hinge_rotation", "hinge", "R = Restrained, F = Free", GH_ParamAccess.item)
        pManager.AddTextParameter("pore_pressure", "pore", "R = Restrained, F = Free", GH_ParamAccess.item)
        pManager.AddIntegerParameter("point_ID", "ID", "Points to assign support to", GH_ParamAccess.list)
        pManager.AddTextParameter("object_type", "type", "line or point", GH_ParamAccess.item)
        pManager.AddBooleanParameter("active?", "act?", "active component?", GH_ParamAccess.item)
    End Sub

    Protected Overrides Sub RegisterOutputParams(pManager As GH_Component.GH_OutputParamManager)
        pManager.Register_IntegerParam("support_ID", "s_ID", "ID to be used for other LUSAS components")
    End Sub

    Protected Overrides Sub SolveInstance(DA As IGH_DataAccess)
        Dim activate As Boolean = False
        If (Not DA.GetData(11, activate)) Then Return
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

            Dim Mx As String = "R"
            If (Not DA.GetData(4, Mx)) Then Return

            Dim My As String = "R"
            If (Not DA.GetData(5, My)) Then Return

            Dim Mz As String = "R"
            If (Not DA.GetData(6, Mz)) Then Return

            Dim hinge As String = "R"
            If (Not DA.GetData(7, hinge)) Then Return

            Dim pore As String = "R"
            If (Not DA.GetData(8, pore)) Then Return

            'Create support
            Dim support As IFSupport = modeller.db.createSupportStructural(name)
            support.setStructural(Fx, Fy, Fz, Mx, My, Mz, hinge, hinge, pore)

            'Get object list and type
            Dim obj_ID As New List(Of Integer)
            If (Not DA.GetDataList(9, obj_ID)) Then Return
            Dim obj_type As String = ""
            If (Not DA.GetData(10, obj_type)) Then Return

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
            Return New Guid("14fa5540-4628-4866-be0b-d8f0978d004b")
        End Get
    End Property
End Class