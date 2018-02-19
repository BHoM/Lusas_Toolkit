Imports System.Collections.Generic
Imports System

Imports Grasshopper.Kernel
Imports Rhino.Geometry

Imports RMA.OpenNURBS
Imports LusasM15_2


Public Class isotropic_full
    Inherits GH_Component

    Public Overrides ReadOnly Property Exposure() As GH_Exposure
        Get
            Return GH_Exposure.primary
        End Get
    End Property

    Public Sub New()
        MyBase.New(“isotropic_full", "iso_full",
                   "Create isotropic material properties - full",
                   "Bridges Tools", "Attributes")
    End Sub

    Protected Overrides Sub RegisterInputParams(pManager As GH_Component.GH_InputParamManager)
        pManager.AddTextParameter("material_name", "mat_n", "Name assigned to material", GH_ParamAccess.item)
        pManager.AddNumberParameter("Young's_modulus", "y_mod", "Young's modulus of the isotropic material", GH_ParamAccess.item)
        pManager.AddNumberParameter("Poisson's_ratio", "p_rat", "Poisson's ratio of the isotropic material", GH_ParamAccess.item)
        pManager.AddNumberParameter("mass_density", "m_den", "Mass per unit volume of the isotropic material", GH_ParamAccess.item)
        pManager.AddNumberParameter("temperature", "T0", "Installation temperature of isotropic material", GH_ParamAccess.item)
        pManager.AddNumberParameter("alpha", "a", "Coefficient of thermal expansions", GH_ParamAccess.item)
        pManager.AddNumberParameter("mass_damping", "m_dam", "Mass Rayleigh damping constant", GH_ParamAccess.item)
        pManager.AddNumberParameter("stiff_damping", "s_dam", "Stiffness Rayleigh damping constant", GH_ParamAccess.item)
        pManager.AddIntegerParameter("line_ID", "ID", "ID of lines to be assigned material properties", GH_ParamAccess.list)
        pManager.AddBooleanParameter("active?", "act?", "active component?", GH_ParamAccess.item)
    End Sub

    Protected Overrides Sub RegisterOutputParams(pManager As GH_Component.GH_OutputParamManager)
        pManager.Register_IntegerParam("material_ID", "m_ID", "ID to be used for other LUSAS components")
    End Sub

    Protected Overrides Sub SolveInstance(DA As IGH_DataAccess)
        Dim activate As Boolean = False
        If (Not DA.GetData(9, activate)) Then Return
        If activate Then
            Dim modeller As LusasWinApp = lusas_modeller.m_lusas

            'Retrieve inputs
            Dim mat_name As String = ""
            Dim yng_mod As Double = 0
            Dim poi_rat As Double = 0
            Dim mass_density As Double = 0
            Dim temp As Double = 0
            Dim alpha As Double = 0
            Dim m_dam As Double = 0
            Dim s_dam As Double = 0

            If (Not DA.GetData(0, mat_name)) Then Return
            If (Not DA.GetData(1, yng_mod)) Then Return
            If (Not DA.GetData(2, poi_rat)) Then Return
            If (Not DA.GetData(3, mass_density)) Then Return
            If (Not DA.GetData(4, temp)) Then Return
            If (Not DA.GetData(5, alpha)) Then Return
            If (Not DA.GetData(6, m_dam)) Then Return
            If (Not DA.GetData(7, s_dam)) Then Return


            'Create Material Attribute
            Dim material As IFMaterialIsotropic = modeller.db.createIsotropicMaterial(
                mat_name, yng_mod, poi_rat, mass_density, alpha, m_dam, s_dam, temp)

            'Retrieve line list
            Dim line_list As New List(Of Integer)
            If (Not DA.GetDataList(8, line_list)) Then Return

            'Create object set 
            Dim obj_set As IFObjectSet = modeller.newObjectSet
            For Each value As Integer In line_list
                obj_set.add("Line", value)
            Next

            'Assign material property to object set
            material.assignTo(obj_set)
            'Update this so "line" is an input inherited from the 

            'Output material ID
            DA.SetData(0, material.getID)

            '1. Solution exception:Public member 'creatematerial' on type 'MaterialLibraryModule' not found.
            'Get external module
            'Dim test_material As IFMaterial = modeller.getModule("MaterialLibrary").creatematerial("MyAttribute", "Concrete", "Europe", "EN1992-1-1:2004/2014", "C40/50")


        End If
    End Sub

    Protected Overrides ReadOnly Property Icon() As System.Drawing.Bitmap
        Get
            Return My.Resources.elastic_material
        End Get
    End Property

    Public Overrides ReadOnly Property ComponentGuid() As Guid
        Get
            Return New Guid("{58f5afe2-17f1-48b7-8971-464658e35ac9}")
        End Get
    End Property
End Class