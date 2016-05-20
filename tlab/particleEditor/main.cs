//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
// Initialization and shutdown code for particle editor plugin.

$TLab_PluginName_["ParticleEditor"] = "Particle Editor";
//---------------------------------------------------------------------------------------------
//==============================================================================
function initParticleEditor() {
	info( "TorqueLab","->","Initializing Particle Editor" );
	execParticleEd(true);
	//Lab.createPlugin("ParticleEditor","Particle Editor");
	Lab.addPluginGui("ParticleEditor",ParticleEditorTools);
	Lab.addPluginToolbar("ParticleEditor",ParticleEditorToolbar);
	ParticleEditorPlugin.superClass = "WEditorPlugin";
	ParticleEditorPlugin.customPalette = "SceneEditorPalette";
	%map = new ActionMap();
	%map.bindCmd( keyboard, "1", "EWorldEditorNoneModeBtn.performClick();", "" );  // Select
	%map.bindCmd( keyboard, "2", "EWorldEditorMoveModeBtn.performClick();", "" );  // Move
	%map.bindCmd( keyboard, "3", "EWorldEditorRotateModeBtn.performClick();", "" );  // Rotate
	%map.bindCmd( keyboard, "4", "EWorldEditorScaleModeBtn.performClick();", "" );  // Scale
	ParticleEditorPlugin.map = %map;
	new ScriptObject( ParticleEditor );
	new PersistenceManager( PE_EmitterSaver );
	new PersistenceManager( PE_ParticleSaver );
	new SimSet( PE_UnlistedParticles );
	new SimSet( PE_UnlistedEmitters );
	new SimSet( PE_AllParticles );
}
//------------------------------------------------------------------------------
//==============================================================================
function execParticleEd(%loadgui) {
	if (%loadgui) {
		exec( "./gui/ParticleEditorTools.gui" );
		exec( "tlab/particleEditor/gui/ParticleEditorToolbar.gui" );
	}

	
	exec( "tlab/particleEditor/ParticleEditorPlugin.cs" );
	exec( "tlab/particleEditor/ParticleEditorParams.cs" );
	execPattern( "tlab/particleEditor/scripts/*.cs" );
	execPattern( "tlab/particleEditor/particle/*.cs" );
	execPattern( "tlab/particleEditor/emitter/*.cs" );
	
}
//------------------------------------------------------------------------------
//==============================================================================
function destroyParticleEditor() {
}
//------------------------------------------------------------------------------
