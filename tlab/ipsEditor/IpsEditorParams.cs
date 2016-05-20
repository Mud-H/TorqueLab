//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================


function IpsEditorPlugin::initDefaultSettings( %this ) {
	Lab.addDefaultSetting( "selectedEmitter", IPSE_Selector.getText() );
	Lab.addDefaultSetting( "selectedParticle", IPSP_Selector.getText() );
	Lab.addDefaultSetting( "selectedTab", IPS_TabBook.getSelectedPage() );
	Lab.addDefaultSetting( "DefaultEmitter", "DefaultEmitter" );
	Lab.addDefaultSetting( "DefaultParticle", "DefaultParticle" );
}

//==============================================================================
// Automated editor plugin setting interface
function IpsEditorPlugin::buildParams(%this,%params ) {
// Group 1 Configuration
	%gid++;
	%pid = 0;
	%params.groupData[%gid] = "General settings" TAB "Params_Stack" TAB "Rollout";
	%params.groupParam[%gid,%pid++] = "DefaultEmitter"  TAB "" TAB "TextEdit";
	%params.groupParam[%gid,%pid++] = "DefaultParticle"  TAB "" TAB "TextEdit";
	return %params;
}
//------------------------------------------------------------------------------
