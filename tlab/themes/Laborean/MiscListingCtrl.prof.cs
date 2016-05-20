//==============================================================================
// TorqueLab -> Default Containers Profiles
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================



//==============================================================================
// GuiTreeViewCtrl
//==============================================================================

//==============================================================================
// ToolsTreeViewProfile Style
//------------------------------------------------------------------------------
singleton GuiControlProfile( ToolsTreeViewProfile : ToolsDefaultProfile ) {
	bitmap = "tlab/themes/Laborean/element-assets/GuiTreeViewProfile.png";
	autoSizeHeight = "0";
	canKeyFocus = true;
	fillColor = "255 255 255";
	fillColorHL = "67 67 67 143";
	fillColorSEL = "99 101 138 96";
	fillColorNA = "255 255 255";
	fontColor = "254 238 192 255";
	fontColorHL = "22 210 254 255";
	fontColorSEL= "255 255 255";
	fontColorNA = "200 200 200";
	borderColor = "128 000 000";
	borderColorHL = "255 228 235";
	opaque = false;
	border = false;
	category = "Tools";
	fontColors[2] = "200 200 200 255";

	fontSize = "16";
	fontColors[0] = "254 238 192 255";
	fontColors[6] = "255 0 255 255";
   fontColors[1] = "22 210 254 255";
};
//------------------------------------------------------------------------------

singleton GuiControlProfile(ToolsTreeViewProfile_S1 : ToolsTreeViewProfile) {
	fontSize = "14";
};

//==============================================================================
// GuiListBoxCtrl
//==============================================================================
singleton GuiControlProfile(ToolsListBox : ToolsDefaultProfile) {

	fontSize = "14";
	fontColor = "254 222 155 255";
	fontColorHL = "0 0 0";
	fontColorSEL= "255 255 255";
	fontColorNA = "200 200 200";
	fillColorSEL = "0 24 39 255";
	fontSize = "18";
	opaque = "1";
	fillColor = "15 234 245 255";
	fillColorHL = "241 12 50 255";
	fillColorNA = "0 255 48 255";
	mouseOverSelected = "1";
	fontColors[0] = "254 222 155 255";
	fontColors[2] = "200 200 200 255";
	textOffset = "0 -2";
};

singleton GuiControlProfile(ToolTreeBrowseMain : ToolsTreeViewProfile)
{
   fontSize = "17";

   fontColors[0] = "213 213 213 255";
   fontColor = "213 213 213 255";
};

singleton GuiControlProfile(ToolsTreeBehaviorProfile : ToolsTreeViewProfile)
{
   bitmap = "tlab/avTools/aiManager/images/BehaviorTreeView.png";

   fontSize = "19";
   fontColors[6] = "255 0 255 255";
};
