//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
// ->Added Gui style support
// -->Delete Profiles when reloaded
// -->Image array store in Global
//==============================================================================

//==============================================================================
// GuiTextEditCtrl Profiles
//==============================================================================

//==============================================================================
//ToolsTextEdit
//------------------------------------------------------------------------------
singleton GuiControlProfile( ToolsTextEdit : ToolsDefaultProfile ) {
	bitmap = "tlab/themes/Laborean/element-assets/GuiTextEditProfile.png";
	bevelColorLL = "255 0 255 255";
	fontSize = "15";
	textOffset = "0 0";
	justify = "Top";
	fontColors[0] = "252 254 252 255";
	fontColors[1] = "34 102 132 150";
	fontColors[2] = "94 94 94 255";
	fontColors[3] = "254 185 5 255";
	fontColors[9] = "255 0 255 255";
	fontColor = "252 254 252 255";
	fontColorHL = "34 102 132 150";
	fontColorNA = "94 94 94 255";
	fontColorSEL = "254 185 5 255";
	fontColors[7] = "255 0 255 255";
	fontColors[6] = "Magenta";
	hasBitmapArray = "1";
	border = "-2";
	tab = "1";
	canKeyFocus = "1";
   fontColors[4] = "Fuchsia";
   fontColorLink = "Fuchsia";

   category = "ToolsText";
};
//------------------------------------------------------------------------------
singleton GuiControlProfile(ToolsTextEdit_S1 : ToolsTextEdit) {
	fontSize = "14";      
};
//------------------------------------------------------------------------------
//ToolsTextEdit Blue border variation
singleton GuiControlProfile( ToolsTextEdit_Num : ToolsTextEdit ) {
	numbersOnly = true;
	fillColorNA = "224 3 197 255";
	fontColors[9] = "Fuchsia";
	cursorColor = "21 78 224 255";
	opaque = "1";
	fillColorHL = "71 255 0 204";
	fillColorSEL = "32 12 101 255";
	borderColor = "0 255 83 241";
	borderColorHL = "255 78 0 255";
	fontColors[1] = "197 143 3 255";
	fontColorHL = "197 143 3 255";
};
//------------------------------------------------------------------------------



singleton GuiControlProfile(ToolsTextEditMain : ToolsTextEdit)
{
   bitmap = "tlab/themes/Laborean/element-assets/GuiTextEditMain.png";
   border = "-1";
};

singleton GuiControlProfile(ToolsTextEditAlt : ToolsTextEditMain)
{
   bitmap = "tlab/themes/Laborean/element-assets/GuiTextEditAlt.png";
   border = "-2";
   borderThickness = "2";
};


singleton GuiControlProfile(ToolsTextPadEdit : ToolsTextEdit)
{
   fillColor = "238 236 241 9";
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
   category = "ToolsText";
   dynamicField = "defaultValue";
};
