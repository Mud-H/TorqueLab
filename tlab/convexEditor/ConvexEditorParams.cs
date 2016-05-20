//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================


function ConvexEditorPlugin::initDefaultCfg( %this,%cfgArray ) {
	%cfgArray.group[1] = "General settings";
	%cfgArray.setVal("MaterialName",       "Grid512_OrangeLines_Mat" TAB "MaterialName" TAB "TextEdit" TAB "" TAB "ConvexEditorGui" TAB "1");
}
