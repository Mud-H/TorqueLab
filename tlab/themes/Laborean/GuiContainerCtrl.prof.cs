//==============================================================================
// TorqueLab -> Default ToolBoxes Profiles
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// Box Content Profiles - Define areas for contents
//==============================================================================
//==============================================================================
//ToolsBoxContentA Style
singleton GuiControlProfile( ToolsBoxContentA : ToolsDefaultProfile ) {	
	category = "ToolsContainers";	
   fillColor = "18 18 18 218";
   bitmap = "tlab/themes/Laborean/container-assets/GuiBoxContentA.png";
   hasBitmapArray = "0";
   border = "1";
   opaque = "1";
   borderColorNA = "76 76 76 71";
   bevelColorHL = "55 55 55 134";
   bevelColorLL = "51 51 51 69";
};
//------------------------------------------------------------------------------
//==============================================================================
//ToolsBoxContentA Style
singleton GuiControlProfile( ToolsBoxContentB : ToolsDefaultProfile ) {	
	category = "ToolsContainers";	
   fillColor = "25 25 25 0";
   bitmap = "tlab/themes/Laborean/container-assets/GuiBoxContentB.png";
   hasBitmapArray = "0";
   border = "-2";
   opaque = "1";
};
//------------------------------------------------------------------------------

//==============================================================================
//ToolsBoxContentB Style
singleton GuiControlProfile( ToolsBoxContentC : ToolsDefaultProfile ) {	
	category = "ToolsContainers";	
   bitmap = "tlab/themes/Laborean/container-assets/GuiBoxContentC.png";
   fillColor = "32 32 32 0";
};
//------------------------------------------------------------------------------



//==============================================================================
// GuiContainer Profiles
//==============================================================================



//==============================================================================
//ToolsBoxTitle Style
//------------------------------------------------------------------------------
singleton GuiControlProfile( ToolsBoxTitle : ToolsDefaultProfile ) {
	opaque = false;
	border = -2;
	category = "ToolsContainers";
	bitmap = "tlab/themes/Laborean/container-assets/GuiBoxTitleThin.png";
	fontColors[2] = "0 0 0 255";
	fontColorNA = "0 0 0 255";
	fontSize = "17";
	fontColors[0] = "222 222 222 255";
	fontColor = "222 222 222 255";
	fontColors[4] = "Magenta";
	fontColorLink = "Magenta";
   fillColor = "171 171 171 0";
};
//------------------------------------------------------------------------------

singleton GuiControlProfile(ToolsBoxTitle_Thin : ToolsBoxTitle) {
	bitmap = "tlab/themes/Laborean/container-assets/GuiBoxTitleThin.png";
	hasBitmapArray = "1";
	category = "ToolsContainers";
	fontSize = "16";
   border = "0";
   textOffset = "4 1";
   fontType = "Calibri Bold";
};



singleton GuiControlProfile(ToolsBoxPanelA : ToolsBoxContentA)
{
   bitmap = "tlab/themes/Laborean/container-assets/GuiBoxPanelA.png";
   opaque = "0";
   fillColor = "92 92 92 167";
   border = "0";
   hasBitmapArray = "1";
};

singleton GuiControlProfile(ToolsBoxFrameA : ToolsBoxContentA)
{
   bitmap = "tlab/themes/Laborean/container-assets/GuiBoxFrameA.png";
   border = "0";
   hasBitmapArray = "1";
};
