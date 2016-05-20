//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================


function ParticleEditorPlugin::initDefaultSettings( %this ) {
	Lab.addDefaultSetting( "selectedEmitter", PEE_EmitterSelector.getText() );
	Lab.addDefaultSetting( "selectedParticle", PEP_ParticleSelector.getText() );
	Lab.addDefaultSetting( "selectedTab", PE_TabBook.getSelectedPage() );
	Lab.addDefaultSetting( "DefaultEmitter", "DefaultEmitter" );
	Lab.addDefaultSetting( "DefaultParticle", "DefaultParticle" );
}

//==============================================================================
// Automated editor plugin setting interface
function ParticleEditorPlugin::buildParams(%this,%params ) {
// Group 1 Configuration
	%gid++;
	%pid = 0;
	%params.groupData[%gid] = "General settings" TAB "Params_Stack" TAB "Rollout";
	%params.groupParam[%gid,%pid++] = "DefaultEmitter"  TAB "" TAB "TextEdit";
	%params.groupParam[%gid,%pid++] = "DefaultParticle"  TAB "" TAB "TextEdit";
	return %params;
}
//------------------------------------------------------------------------------