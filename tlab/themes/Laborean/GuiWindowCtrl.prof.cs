//==============================================================================
// TorqueLab -> Default Containers Profiles
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// GuiWindowCtrl Profiles
//==============================================================================

//==============================================================================
//ToolsWindowProfile Style
//------------------------------------------------------------------------------
singleton GuiControlProfile( ToolsWindowProfile : ToolsDefaultProfile ) {
	opaque = "1";
	border = "0";
	fillColor = "15 15 15 255";
	fillColorHL = "41 41 41 255";
	fillColorNA = "41 41 41 255";
	fontColor = "199 199 199 255";
	fontColorHL = "0 0 0 255";
	bevelColorHL = "255 255 255";
	bevelColorLL = "Black";
	text = "untitled";
	bitmap = "tlab/themes/Laborean/container-assets/GuiWindowProfile.png";
	textOffset = "10 2";
	hasBitmapArray = true;
	justify = "left";
	category = "ToolsContainers";

	fontSize = "21";
	fontColors[0] = "199 199 199 255";
	cursorColor = "0 0 0 255";
	fontColors[7] = "255 0 255 255";
	fontColors[5] = "255 0 255 255";
	fontColorLinkHL = "255 0 255 255";
};
//------------------------------------------------------------------------------

//==============================================================================
//ToolsWindowDialog Style
//------------------------------------------------------------------------------
singleton GuiControlProfile(ToolsWindowDialog : ToolsWindowProfile) {
	bitmap = "tlab/themes/Laborean/container-assets/GuiWindowDialog.png";
	fillColor = "37 37 37 255";
	fontColors[0] = "245 245 245 255";
	fontColor = "245 245 245 255";
	textOffset = "8 1";
	fontColors[3] = "255 255 255 255";
	fontColorSEL = "255 255 255 255";
	fontColors[9] = "255 0 255 255";
	fillColorHL = "228 228 235 255";
	fillColorNA = "255 255 255 255";
	fillColorSEL = "194 194 196 255";
	borderColor = "200 200 200 255";

	fontSize = "20";
	fontColors[7] = "255 0 255 255";
	bevelColorLL = "Black";
   fontColors[5] = "255 0 255 255";
   fontColorLinkHL = "255 0 255 255";
};
//------------------------------------------------------------------------------



singleton GuiControlProfile(ToolsWindowProfile : ToolsWindowProfile)
{
   bevelColorLL = "Black";
   fontSize = "22";
   textOffset = "16 1";
};

singleton GuiControlProfile(ToolsWindowPanel : ToolsWindowProfile)
{
   bitmap = "tlab/themes/Laborean/container-assets/GuiWindowPanel.png";
   opaque = "0";
   fillColor = "21 21 21 255";
   fillColorHL = "41 41 41 143";
   border = "-1";
   fontColors[0] = "236 236 236 255";
   fontColor = "236 236 236 255";
   textOffset = "8 3";
};

singleton GuiControlProfile(ToolsWindowRolloutThin : ToolsWindowDialog)
{
   bitmap = "tlab/themes/Laborean/container-assets/GuiWindowRolloutThin.png";
   border = "-5";

   fontSize = "15";
   textOffset = "8 0";
   fontColors[0] = "173 173 173 255";
   fontColor = "173 173 173 255";
   fillColor = "21 21 21 255";
   fillColorERR = "255 0 0 255";
   fontColors[4] = "Fuchsia";
   fontColorLink = "Fuchsia";
   fillColorSEL = "98 100 137 255";
};

singleton GuiControlProfile(ToolsWindowRollout : ToolsWindowProfile)
{
   bitmap = "tlab/themes/Laborean/container-assets/GuiWindowRollout.png";
};
