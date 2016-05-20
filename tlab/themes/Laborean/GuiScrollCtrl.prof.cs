//==============================================================================
// TorqueLab -> Default Containers Profiles
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// GuiScrollCtrl Profiles
//==============================================================================
//==============================================================================
//ToolsScrollProfile Style - With Default Dark Background
//------------------------------------------------------------------------------
singleton GuiControlProfile( ToolsScrollProfile : ToolsDefaultProfile ) {
	bitmap = "tlab/themes/Laborean/container-assets/GuiScrollProfile.png";
	opaque = "1";
	fillColor = "22 22 22 255";
	category = "ToolsContainers";
	border = "1";
	borderThickness = "1";
	borderColor = "148 155 148 43";
	borderColorHL = "50 50 50 255";
	borderColorNA = "51 51 51 255";
	bevelColorHL = "3 3 3 255";
	bevelColorLL = "12 12 12 255";
};
//------------------------------------------------------------------------------

singleton GuiControlProfile(ToolsScrollFillBgA : ToolsScrollProfile)
{
   fillColor = "43 43 43 255";
};
