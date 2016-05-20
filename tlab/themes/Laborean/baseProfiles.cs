//==============================================================================
// TorqueLab -> Core Profiles
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
// Those profiles are defined in the code directly
//==============================================================================


new GuiControlProfile (ToolsDefaultProfile) {
	tab = false;
	canKeyFocus = false;
	hasBitmapArray = false;
	mouseOverSelected = false;
	// fill color
	opaque = "0";
	fillColor = "243 241 241 0";
	fillColorHL ="229 229 236 0";
	fillColorSEL = "99 101 138 0";
	fillColorNA = "255 255 255 0";
	// border color
	border = 0;
	borderColor   = "101 101 101 0";
	borderColorHL = "50 50 50 50";
	borderColorNA = "75 75 75";
	// font
	fontType = "Calibri";
	fontSize = 16;
	fontCharset = ANSI;
	fontColor = "0 0 0 0";
	fontColorHL = "0 0 0";
	fontColorNA = "0 0 0";
	fontColorSEL= "255 255 255";
	fontColors[5] = "255 0 255 255";
	// bitmap information
	bitmapBase = "";
	textOffset = "0 0";
	// used by guiTextControl
	modal = "1";
	justify = "left";
	autoSizeWidth = false;
	autoSizeHeight = false;
	returnTab = false;
	numbersOnly = false;
	cursorColor = "0 0 0 0";
	category = "Tools";
	borderThickness = "0";
	fontColors[9] = "255 0 255 255";
	fontColors[0] = "0 0 0 0";
	// sounds
	soundButtonDown = "";
	soundButtonOver = "";
	soundButtonUp = "";
};


new GuiControlProfile (ToolsSolidDefaultProfile : ToolsDefaultProfile) {
	opaque = true;
	border = true;
	category = "Tools";
};

new GuiControlProfile (ToolsTransparentProfile : ToolsDefaultProfile) {
	opaque = false;
	border = false;
	category = "Tools";
	modal = "1";
	fontColors[9] = "Magenta";
};

new GuiControlProfile( ToolsGroupBorderProfile ) {
	border = false;
	opaque = false;
	hasBitmapArray = true;
	bitmap = "tlab/themes/common/assets/group-border";
	category = "Tools";
};


singleton GuiControlProfile (ToolsToolTipProfile : ToolsDefaultProfile) {
	// fill color
	fillColor = "239 237 222";
	// border color
	borderColor   = "138 134 122";
	// font
	
	fontSize = 14;
	fontColor = "0 0 0";
	category = "Tools";
	fontColors[4] = "Fuchsia";
	fontColorLink = "Fuchsia";
};



singleton GuiControlProfile( ToolsDefaultProfile_NoModal : ToolsDefaultProfile ) {
	modal = false;
	category = "Tools";
	fontColors[5] = "Fuchsia";
	fontColors[7] = "255 0 255 255";
	fontColorLinkHL = "Fuchsia";
   opaque = "1";
};


new GuiControlProfile( ToolsProgressBitmapProfile ) {
	border = false;
	hasBitmapArray = true;
	bitmap = "tlab/themes/common/assets/rl-loadingbar";
	category = "Tools";
};










new GuiControlProfile( ToolsGuiListBoxProfile ) {
	tab = true;
	canKeyFocus = true;
	category = "Tools";
};





new GuiControlProfile( ToolsGuiFormProfile : ToolsDefaultProfile ) {
	opaque = false;
	border = 5;
	justify = "center";
	profileForChildren = ToolsButtonProfile;
	opaque = false;
	hasBitmapArray = true;
	bitmap = "tlab/themes/common/assets/button";
	category = "Tools";
};

// ----------------------------------------------------------------------------
singleton GuiControlProfile( ToolsBackFillProfile : ToolsDefaultProfile ) {
	opaque = true;
	fillColor = "0 94 94";
	border = "1";
	borderColor = "255 128 128";

	fontSize = 12;
	fontColor = "0 0 0";
	fontColorHL = "50 50 50";
	fixedExtent = 1;
	justify = "center";
	category = "Editor";
	fontColors[1] = "50 50 50 255";
	fontColors[9] = "255 0 255 255";
};


//==============================================================================
// TorqueLab Top Menu Bar Profiles
//==============================================================================
//==============================================================================
// MenuBar Background
singleton GuiControlProfile (ToolsMenuBarProfile  : ToolsDefaultProfile) {
	opaque = true;
	border = "1";
	category = "Tools";
	fillColor = "22 22 22 255";	
	fontSize = "18";
	fontColors[0] = "0 226 255 255";
	fontColor = "0 226 255 255";
	justify = "Bottom";
	textOffset = "10 4";
	fontColors[8] = "255 0 255 255";
	fillColorHL = "97 97 97 66";
	fillColorNA = "127 64 29 255";
	fontColors[1] = "37 183 254 255";
	fontColors[2] = "208 132 6 255";
	fontColors[3] = "240 185 39 255";
	fontColorHL = "37 183 254 255";
	fontColorNA = "208 132 6 255";
	fontColorSEL = "240 185 39 255";
	cursorColor = "255 0 255 255";
	modal = true;
	borderColorHL = "254 254 222 236";
	borderColorNA = "255 213 0 210";
	bevelColorHL = "106 106 106 255";
	bevelColorLL = "3 3 213 255";
	autoSizeWidth = "1";
	autoSizeHeight = "1";
	borderColor = "0 148 255 67";
	fontColors[7] = "255 0 255 255";
	fontColors[9] = "Fuchsia";
   fontColors[5] = "Magenta";
   fontColorLinkHL = "Magenta";
};


//------------------------------------------------------------------------------
//==============================================================================
// MenuBar Items
singleton GuiControlProfile( ToolsMenuProfile : ToolsMenuBarProfile) {
	opaque = true;
	fillcolor = "32 32 32 255";
	fontColor = "213 213 213 255";
	fontColorHL = "64 222 254 255";
	category = "Tools";
	fillColorHL = "43 43 43 241";	
	fontSize = "18";
	fontColors[0] = "213 213 213 255";
	fontColors[1] = "64 222 254 255";
	fillColorNA = "29 132 21 252";
	fillColorSEL = "11 43 190 255";
	justify = "Center";
	fontColors[2] = "102 62 27 188";
	fontColors[3] = "254 227 97 255";
	fontColorNA = "102 62 27 188";
	fontColorSEL = "254 227 97 255";
	cursorColor = "0 0 0 255";
	modal = true;
	border = "0";
	borderColor = "122 118 122 255";
	borderColorHL = "141 141 141 255";
	borderColorNA = "120 113 120 255";
	fontColors[7] = "255 0 255 255";
	bevelColorHL = "104 104 104 255";
	bevelColorLL = "157 157 157 255";
	bitmap = "tlab/themes/Laborean/lab-assets/GuiMenuBitmaps.png";
	hasBitmapArray = "1";
	fontColors[4] = "Fuchsia";
	fontColorLink = "Fuchsia";
	textOffset = "0 0";
};
//------------------------------------------------------------------------------
singleton GuiControlProfile (ToolsMenubarBitmapProfile ) {
   opaque = false;
	border = -2;
	bitmap = "tlab/art/assets/default/menubar";
	category = "Editor";
};
//------------------------------------------------------------------------------

singleton GuiControlProfile( ToolsGuiTextPadProfile : ToolsDefaultProfile ) {

	fontSize ="14";
	tab = true;
	canKeyFocus = true;
	// Deviate from the Default
	opaque=true;
	fillColor = "21 21 21 255";
	border = 0;
	category = "Tools";
	fillColorHL = "254 222 155 255";
   fontColors[0] = "254 222 155 255";
   fontColors[3] = "254 222 155 255";
   fontColor = "254 222 155 255";
   fontColorHL = "0 0 0 255";
};
singleton GuiControlProfile(ToolsTextPadBox : ToolsGuiTextPadProfile)
{
   
   fontSize = "21";
   fontColors[0] = "254 254 254 245";
   fontColors[3] = "254 248 234 255";
   fontColor = "254 254 254 245";
   fontColorSEL = "254 248 234 255";
   textOffset = "0 0";
   cursorColor = "102 132 203 255";
   autoSizeWidth = "1";
   autoSizeHeight = "1";
};


//------------------------------------------------------------------------------
singleton GuiControlProfile( ToolsTextPadScroll : ToolsTextPadBox ) {
	bitmap = "tlab/themes/Laborean/container-assets/GuiScrollProfile.png";
	opaque = "1";
	fillColor = "22 22 22 255";
	category = "Tools";
	border = "1";
	borderThickness = "1";
	borderColor = "148 155 148 43";
	borderColorHL = "50 50 50 255";
	borderColorNA = "51 51 51 255";
	bevelColorHL = "3 3 3 255";
	bevelColorLL = "12 12 12 255";
};
//------------------------------------------------------------------------------
singleton GuiControlProfile( ToolsTextPadEdit : ToolsTextPadBox ) {
	category = "Tools";
   fillColor = "238 236 241 9";
   fillColorHL = "229 229 236 0";
   fillColorSEL = "99 101 138 0";
   borderColorHL = "231 231 231 236";
   fontSize = "17";
   fontColors[0] = "236 236 236 255";
   fontColors[1] = "3 3 3 255";
   fontColors[2] = "254 227 83 255";
   fontColors[3] = "0 0 2 255";
   fontColors[4] = "62 99 254 255";
   fontColor = "236 236 236 255";
   fontColorHL = "3 3 3 255";
   fontColorNA = "254 227 83 255";
   fontColorSEL = "0 0 2 255";
   fontColorLink = "62 99 254 255";
   tab = "1";
   canKeyFocus = "1";
   autoSizeWidth = "0";
   autoSizeHeight = "0";
   dynamicField = "defaultValue";
  
};

//------------------------------------------------------------------------------
singleton GuiControlProfile( ToolsTextPadLightBox : ToolsTextPadBox ) {
	category = "Tools";
};

singleton GuiControlProfile( ToolsTextPadLightScroll : ToolsTextPadScroll ) {
	bitmap = "tlab/themes/Laborean/container-assets/GuiScrollProfile.png";
	opaque = "1";
	borderColor = "148 155 148 43";
	borderColorHL = "50 50 50 255";
	borderColorNA = "51 51 51 255";
	bevelColorHL = "3 3 3 255";
	bevelColorLL = "12 12 12 255";
};

singleton GuiControlProfile( ToolsTextPadLightEdit : ToolsTextPadEdit ) {
   fillColor = "238 236 241 9";
   fillColorHL = "229 229 236 0";
   fillColorSEL = "99 101 138 0";
   borderColorHL = "231 231 231 236";  
   fontColors[2] = "254 227 83 255";
   fontColors[3] = "0 0 2 255";
   fontColorNA = "254 227 83 255";
   fontColorSEL = "0 0 2 255";
};

singleton GuiControlProfile( ToolsDragDropProfile ) {
	justify = "center";
	fontColor = "0 0 0";
	border = 0;
	textOffset = "0 0";
	opaque = true;
	fillColor = "221 221 221 150";
	category = "Tools";
};
