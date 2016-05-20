//==============================================================================
// TorqueLab -> Default Containers Profiles
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// GuiTabBookCtrl Profiles
//==============================================================================

//==============================================================================
//ToolsTabBookMain Style
//------------------------------------------------------------------------------
singleton GuiControlProfile( ToolsTabBookMain : ToolsDefaultProfile ) {
	hasBitmapArray = true;
	category = "ToolsContainers";
	fontColors[9] = "255 0 255 255";
	autoSizeWidth = "1";
	autoSizeHeight = "1";
	profileForChildren = "ToolsBoxDarkC_Top";
	bitmap = "tlab/themes/Laborean/container-assets/GuiTabBookProfile.png";
	border = "5";
	borderThickness = "14";

	fontSize = "15";
	justify = "Center";
	fontColors[0] = "220 220 220 184";
	fontColor = "220 220 220 184";
	textOffset = "0 0";
	fontColors[3] = "220 220 220 255";
	fontColorSEL = "220 220 220 255";
	opaque = "0";
	fillColor = "32 80 18 0";
	borderColor = "118 118 118 0";
   fontColors[1] = "254 254 254 255";
   fontColorHL = "254 254 254 255";
};
//------------------------------------------------------------------------------

singleton GuiControlProfile(ToolsTabBookMain_S1 : ToolsTabBookMain)
{
   fontSize = "14";
   fontColors[0] = "254 248 217 255";
   fontColor = "254 248 217 255";
   bitmap = "tlab/themes/Laborean/container-assets/GuiTabBookProfileThin.png";
   justify = "Bottom";
   textOffset = "0 -1";

};



singleton GuiControlProfile(ToolsTabBookMainDrag : ToolsTabBookMain)
{
   opaque = "1";
   fillColor = "22 22 22 150";
   fillColorHL = "229 229 236 255";
};

singleton GuiControlProfile(ToolsTabBookAlt : ToolsTabBookMain)
{
   bitmap = "tlab/themes/Laborean/container-assets/GuiTabBookAlt.png";
};

singleton GuiControlProfile(ToolsTabBookBasic : ToolsTabBookMain)
{
   bitmap = "tlab/themes/Laborean/container-assets/GuiTabBookBasic.png";
};
