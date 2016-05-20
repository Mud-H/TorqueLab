//==============================================================================
// TorqueLab -> Default Containers Profiles
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// GuiButtonCtrl Profiles
//==============================================================================
 singleton SFXProfile(SFX_TLab_ButtonUp)
   {
      filename    = "tlab/art/sound/button_up.ogg";
      description = AudioGui;
      preload = true;
   };
    singleton SFXProfile(SFX_TLab_ButtonDown)
   {
      filename    = "tlab/art/sound/button_down.ogg";
      description = AudioGui;
      preload = true;
   };
//==============================================================================
//ToolsButtonProfile Style
//------------------------------------------------------------------------------
singleton GuiControlProfile( ToolsButtonProfile : ToolsDefaultProfile ) {
	fontSize = "16";
	fontType = "Calibri";
	fontColor = "254 254 254 255";
	justify = "center";
	category = "ToolsButtons";
	opaque = "1";
	border = "-2";
	fontColors[0] = "254 254 254 255";
	fontColors[2] = "200 200 200 255";
	fontColorNA = "200 200 200 255";
	bitmap = "tlab/themes/Laborean/buttons-assets/GuiButtonProfile.png";
	hasBitmapArray = "1";
	fixedExtent = "0";
	bevelColorLL = "255 0 255 255";
	textOffset = "0 -1";
	autoSizeWidth = "1";
	autoSizeHeight = "1";
	borderThickness = "0";
	fontColors[9] = "Fuchsia";
	fontColors[6] = "Fuchsia";
	fontColors[4] = "255 0 255 255";
	fontColorLink = "255 0 255 255";
	fontColors[3] = "255 255 255 255";
	fontColorSEL = "255 255 255 255";
	fontColors[7] = "Magenta";
	soundButtonDown = SFX_TLab_ButtonDown;
	soundButtonUp = SFX_TLab_ButtonUp;
   tab = "1";
};
//------------------------------------------------------------------------------
singleton GuiControlProfile(ToolsButtonProfile_S1 : ToolsButtonProfile) {
	fontSize = "14";	
	justify = "Top";
	textOffset = "0 0";	
};
singleton GuiControlProfile(ToolsButtonProfile_L : ToolsButtonProfile) {
	justify = "left";
};

//==============================================================================
//ToolsButtonAlt Style - Inverted Color Style
//------------------------------------------------------------------------------
singleton GuiControlProfile(ToolsButtonAlt : ToolsButtonProfile)
{
   bitmap = "tlab/themes/Laborean/buttons-assets/GuiButtonAlt.png";
};
//------------------------------------------------------------------------------
//==============================================================================
//ToolsButtonStyle - Various fancier styles
//------------------------------------------------------------------------------
singleton GuiControlProfile(ToolsButtonStyleA : ToolsButtonProfile)
{
   bitmap = "tlab/themes/Laborean/buttons-assets/GuiButtonStyleA.png";
   border = "0";
};



//==============================================================================
//ToolsButtonHighlight Style - Special style for highlighting stuff
//------------------------------------------------------------------------------


singleton GuiControlProfile( ToolsButtonArray : ToolsButtonProfile) {
	fillColor = "225 243 252 255";
	fillColorHL = "225 243 252 0";
	fillColorNA = "225 243 252 0";
	fillColorSEL = "225 243 252 0";
	//tab = true;
	//canKeyFocus = true;

	fontSize = "14";
	fontColor = "250 250 247 255";
	fontColorSEL = "43 107 206";
	fontColorHL = "244 244 244";
	fontColorNA = "100 100 100";
	border = "-2";
	borderColor   = "153 222 253 255";
	borderColorHL = "156 156 156";
	borderColorNA = "153 222 253 0";
	//bevelColorHL = "255 255 255";
	//bevelColorLL = "0 0 0";
	fontColors[1] = "244 244 244 255";
	fontColors[2] = "100 100 100 255";
	fontColors[3] = "43 107 206 255";
	fontColors[9] = "255 0 255 255";
	fontColors[0] = "250 250 247 255";
	modal = 1;
	bitmap = "tlab/themes/Laborean/buttons-assets/GuiButtonArray.png";
};
//------------------------------------------------------------------------------

//==============================================================================
// GuiCheckboxCtrl Profiles
//==============================================================================

//==============================================================================
//ToolsCheckBoxProfile Style
//------------------------------------------------------------------------------
singleton GuiControlProfile( ToolsCheckBoxProfile : ToolsDefaultProfile ) {
	opaque = false;
	fillColor = "232 232 232";
	border = false;
	borderColor = "100 100 100";
	fontSize = "15";
	fontColor = "234 234 234 255";
	fontColorHL = "80 80 80";
	fontColorNA = "200 200 200";
	fixedExtent = 1;
	justify = "left";
	bitmap = "tlab/themes/Laborean/buttons-assets/GuiCheckboxProfile.png";
	hasBitmapArray = true;
	category = "Tools";
	fontColors[0] = "234 234 234 255";
	fontColors[1] = "80 80 80 255";
	fontColors[2] = "200 200 200 255";
	fontColors[4] = "Fuchsia";
	fontColorLink = "Fuchsia";
	textOffset = "0 -3";
   fontColors[6] = "Fuchsia";
};


//==============================================================================
//ToolsCheckBoxAlt Style
//------------------------------------------------------------------------------
singleton GuiControlProfile( ToolsCheckBoxAlt : ToolsCheckBoxProfile ) {
	opaque = false;
	fillColor = "232 232 232";
	border = false;
	borderColor = "100 100 100";
	fontSize = "15";
	fontColor = "234 234 234 255";
	fontColorHL = "80 80 80";
	fontColorNA = "200 200 200";
	fixedExtent = 1;
	justify = "left";
	bitmap = "tlab/themes/Laborean/buttons-assets/GuiCheckboxAlt.png";
	hasBitmapArray = true;
	category = "Tools";
	fontColors[0] = "234 234 234 255";
	fontColors[1] = "80 80 80 255";
	fontColors[2] = "200 200 200 255";
	fontColors[4] = "Fuchsia";
	fontColorLink = "Fuchsia";
	textOffset = "0 -3";
   fontColors[6] = "Fuchsia";
};



//==============================================================================
// GuiRadioCtrl Profiles
//==============================================================================

//==============================================================================
//ToolsRadioProfile Style
//------------------------------------------------------------------------------
singleton GuiControlProfile(ToolsRadioProfile : ToolsDefaultProfile) {
	fillColor = "254 253 253 255";
	fillColorHL = "221 221 221 255";
	fillColorNA = "200 200 200 255";
	fontSize = "14";
	textOffset = "-3 10";
	bitmap = "tlab/themes/Laborean/buttons-assets/GuiRadioProfile.png";
	hasBitmapArray = "1";
	fontColors[0] = "250 250 250 255";
	fontColor = "250 250 250 255";
	border = "0";
	fontColors[2] = "Black";
	fontColorNA = "Black";
	justify = "Left";
	category = "Tools";
	fontColors[8] = "Magenta";
	fontColors[7] = "255 0 255 255";
	autoSizeHeight = "1";
};

//==============================================================================
// Swatch Button Profile -> Used in stock code (Do not remove)
//==============================================================================
//------------------------------------------------------------------------------
singleton GuiControlProfile(ToolsSwatchButtonProfile : ToolsDefaultProfile) {
	fillColor = "254 254 254 255";
	fillColorHL = "221 221 221 255";
	fillColorNA = "200 200 200 255";
	fontSize = "24";
	textOffset = "16 10";
	bitmap = "tlab/themes/Laborean/element-assets/GuiButtonProfile.png";
	hasBitmapArray = "0";
	fontColors[0] = "253 253 253 255";
	fontColor = "253 253 253 255";
	border = "0";
	fontColors[2] = "Black";
	fontColorNA = "Black";
	category = "Tools";
	modal = "0";
	opaque = "1";
	fillColorSEL = "99 101 138 156";
	borderColor = "100 100 100 255";
	cursorColor = "0 0 0 79";
};







