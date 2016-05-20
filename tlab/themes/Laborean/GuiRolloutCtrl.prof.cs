//==============================================================================
// TorqueLab -> Default Containers Profiles
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================





//==============================================================================
// GuiRolloutCtrl Profiles
//==============================================================================

//==============================================================================
//ToolsRolloutProfile Style
//------------------------------------------------------------------------------
singleton GuiControlProfile( ToolsRolloutProfile : ToolsDefaultProfile ) {
	border = "0";
	borderColor = "200 200 200";
	hasBitmapArray = true;
	bitmap = "tlab/themes/Laborean/container-assets/GuiRolloutProfile.png";
	textoffset = "8 0";

	fontSize = "20";
	fontColors[0] = "231 231 231 255";
	fontColor = "231 231 231 255";
	fontColors[7] = "255 0 255 255";
	opaque = "1";
	fillColor = "32 32 32 255";
	fillColorHL = "228 228 235 255";
	fillColorNA = "White";
	fillColorSEL = "98 100 137 255";
	category = "ToolsContainers";
   fontColors[5] = "255 0 255 255";
   fontColorLinkHL = "255 0 255 255";
   bevelColorHL = "Magenta";
};

//------------------------------------------------------------------------------
singleton GuiControlProfile(ToolsRolloutProfile_S1 : ToolsRolloutProfile) {
	bitmap = "tlab/themes/Laborean/container-assets/GuiRolloutProfile.png";
	fontColors[4] = "Magenta";
	fontColorLink = "Magenta";

	fontSize = "15";
	fontColors[0] = "210 210 210 255";
	fontColors[9] = "Magenta";
	fontColor = "210 210 210 255";
   bevelColorLL = "Magenta";
   textOffset = "4 0";
};
//------------------------------------------------------------------------------




singleton GuiControlProfile(ToolsRolloutSub : ToolsRolloutProfile)
{
   bitmap = "tlab/themes/Laborean/container-assets/GuiRolloutProfileSub.png";
   opaque = "1";
   fillColor = "32 32 32 255";

   fontSize = "17";
   fontColors[0] = "253 253 253 255";
   fontColor = "253 253 253 255";
};

singleton GuiControlProfile(ToolsRolloutMain : ToolsRolloutProfile)
{
   bitmap = "tlab/themes/Laborean/container-assets/GuiRolloutMain.png";
};
