//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
function DatablockEditorPlugin::initDefaultCfg( %this,%cfgArray ) {
	%cfgArray.group[1] = "General settings";
	%cfgArray.setVal("libraryTab",       "0" TAB "libraryTab" TAB "TextEdit" TAB "" TAB "DatablockEditorPlugin" TAB "1");
}
//==============================================================================
// Initialize default plugin settings
function DatablockEditorPlugin::initDefaultSettings( %this ) {
	Lab.addDefaultSetting("libraryTab", "0");
	Lab.addDefaultSetting( "selectedDatablock", "" );
}
//------------------------------------------------------------------------------

//==============================================================================
// Automated editor plugin setting interface
function DatablockEditorPlugin::buildParams(%this,%params ) {
//-------------------------------------------------
// Group 1 Configuration
	%gid++;
	%pid = 0;
	%params.groupData[%gid] = "General settings" TAB "Params_Stack" TAB "Rollout";
	%params.groupParam[%gid,%pid++] = "libraryTab"  TAB "" TAB "TextEdit";
	return %params;
}
//------------------------------------------------------------------------------