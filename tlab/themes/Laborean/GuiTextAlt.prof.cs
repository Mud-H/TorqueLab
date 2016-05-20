//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// Dark text profile for Light background
//==============================================================================
//==============================================================================
singleton GuiControlProfile(ToolsTextAlt : ToolsDefaultProfile) {
	fontColors[0] = "250 224 180 255";
	fontColors[1] = "252 189 81 255";
	fontColors[2] = "254 227 83 255";
	fontColors[3] = "254 3 62 255";
	fontColors[4] = "238 255 0 255";
	fontColors[5] = "3 254 148 255";
	fontColors[6] = "3 21 254 255";
	fontColors[7] = "254 236 3 255";
	fontColors[8] = "254 3 43 255";
	fontColors[9] = "3 48 248 255";
	fontColor = "250 224 180 255";
	fontColorHL = "252 189 81 255";
	fontColorNA = "254 227 83 255";
	fontColorSEL = "254 3 62 255";
	fontColorLink = "238 255 0 255";
	fontColorLinkHL = "3 254 148 255";

	fillColor = "238 236 240 255";
	bevelColorHL = "255 0 255 255";
	justify = "left";
	category = "ToolsText";
	fontSize = "16";
};
singleton GuiControlProfile(ToolsTextAlt_C : ToolsTextAlt) {
	locked = true;
	justify = "Center";
   fontColors[0] = "250 224 180 255";
   fontColor = "250 224 180 255";
	
};
singleton GuiControlProfile(ToolsTextAlt_R : ToolsTextAlt) {
	locked = true;
	justify = "Right";
};
//------------------------------------------------------------------------------
//==============================================================================
singleton GuiControlProfile(ToolsTextAlt_S1 : ToolsTextAlt) {
	fontSize = "18";
};
singleton GuiControlProfile(ToolsTextAlt_S1_C : ToolsTextAlt_S1) {
	locked = true;
	justify = "Center";
	fillColorNA = "TransparentWhite";
};
singleton GuiControlProfile(ToolsTextAlt_S1_R : ToolsTextAlt_S1) {
	locked = true;
	justify = "Right";
};
//------------------------------------------------------------------------------
singleton GuiControlProfile(ToolsTextAlt_H1 : ToolsTextAlt)
{
	fontSize = "26";  
   justify = "Center";
};

singleton GuiControlProfile(ToolsTextAlt_H1_C : ToolsTextAlt_H1)
{
	justify = "Center";
	modal = "0";
};
singleton GuiControlProfile(ToolsTextAlt_H2 : ToolsTextAlt)
{
	fontSize = "22";  
   justify = "Center";
};

singleton GuiControlProfile(ToolsTextAlt_H2_C : ToolsTextAlt_H2)
{
	justify = "Center";
	modal = "0";
};

singleton GuiControlProfile(ToolsTextAlt_H3 : ToolsTextAlt)
{
   fontSize = "18";
};
singleton GuiControlProfile(ToolsTextAlt_H3_C : ToolsTextAlt_H3)
{
   justify = "Center";
};
