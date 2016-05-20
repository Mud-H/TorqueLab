//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// Initialize default plugin settings
function MeshRoadEditorPlugin::initDefaultSettings( %this ) {
	Lab.addDefaultSetting(  "DefaultWidth",         "10" );
	Lab.addDefaultSetting(  "DefaultDepth",         "5" );
	Lab.addDefaultSetting(  "DefaultNormal",        "0 0 1" );
	Lab.addDefaultSetting(  "HoverSplineColor",     "255 0 0 255" );
	Lab.addDefaultSetting(  "SelectedSplineColor",  "0 255 0 255" );
	Lab.addDefaultSetting(  "HoverNodeColor",       "255 255 255 255" ); //<-- Not currently used
	Lab.addDefaultSetting(  "TopMaterialName",      "DefaultRoadMaterialTop" );
	Lab.addDefaultSetting(  "BottomMaterialName",   "DefaultRoadMaterialOther" );
	Lab.addDefaultSetting(  "SideMaterialName",     "DefaultRoadMaterialOther" );
}
//------------------------------------------------------------------------------

//==============================================================================
// Automated editor plugin setting interface
function MeshRoadEditorPlugin::buildParams(%this,%params ) {
//-------------------------------------------------
// Group 1 Configuration
	%gid++;
	%pid = 0;
	%params.groupData[%gid] = "General settings" TAB "Params_Stack" TAB "Rollout";
	%params.groupParam[%gid,%pid++] = "DefaultWidth"  TAB "" TAB "SliderEdit" TAB "range::0 10;;precision::2";
	%params.groupParam[%gid,%pid++] = "DefaultDepth"  TAB "" TAB "SliderEdit" TAB "range::0 10;;precision::2";
	%params.groupParam[%gid,%pid++] = "DefaultNormal"  TAB "" TAB "TextEdit";
	%params.groupParam[%gid,%pid++] = "HoverSplineColor"  TAB "" TAB "Color";
	%params.groupParam[%gid,%pid++] = "SelectedSplineColor"  TAB "" TAB "Color";
	%params.groupParam[%gid,%pid++] = "HoverNodeColor"  TAB "" TAB "Color";
	%params.groupParam[%gid,%pid++] = "TopMaterialName"  TAB "" TAB "TextEdit";
	%params.groupParam[%gid,%pid++] = "BottomMaterialName"  TAB "" TAB "TextEdit";
	%params.groupParam[%gid,%pid++] = "SideMaterialName"  TAB "" TAB "TextEdit";
	return %params;
}
//------------------------------------------------------------------------------