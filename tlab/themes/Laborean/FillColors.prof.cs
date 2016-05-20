//==============================================================================
// TorqueLab -> Panels Profiles
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
// Special Panels which simply set predefined fill color and appropriate text
//==============================================================================

//==============================================================================
// Color Panels -> Main theme colors (A-B-C)
//==============================================================================

//------------------------------------------------------------------------------



singleton GuiControlProfile(ToolsFillBackgroundA : ToolsDefaultProfile)
{
   fillColor = "15 15 15 255";
   opaque = "1";
   fontColors[7] = "255 0 255 255";
};

singleton GuiControlProfile(ToolsFillBackgroundB : ToolsFillBackgroundA)
{
   fillColor = "22 22 22 255";
};


singleton GuiControlProfile(ToolsFillLightA : ToolsFillBackgroundA)
{
   fillColor = "215 215 215 255";
};

singleton GuiControlProfile(ToolsOverlayDisabledProfile : ToolsFillBackgroundA)
{
   fontColors[9] = "255 0 255 255";
   fillColor = "32 32 32 162";
};

singleton GuiControlProfile(ToolsFillLightTrans_20 : ToolsFillBackgroundA)
{
   fillColor = "136 136 136 16";
};

singleton GuiControlProfile(ToolsFillBackgroundC : ToolsFillBackgroundB)
{
   fillColor = "32 32 32 255";
};

singleton GuiControlProfile(ToolsFillBackgroundD : ToolsFillBackgroundC)
{
   fillColor = "42 42 42 255";
};
