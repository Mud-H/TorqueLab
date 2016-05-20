//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
// Initialization and shutdown code for particle editor plugin.
$TLab_PluginName_IpsEditor = "IPS Particle Editor";

//---------------------------------------------------------------------------------------------
//==============================================================================
function initIpsEditor() {
	info( "TorqueLab","->","Initializing IPS Particle Editor" );
	execIpsEd(true);
	//Lab.createPlugin("IpsEditor","Particle Editor");
	Lab.addPluginGui("IpsEditor",IpsEditorTools);
	Lab.addPluginToolbar("IpsEditor",IpsEditorToolbar);
	IpsEditorPlugin.superClass = "WEditorPlugin";
	IpsEditorPlugin.customPalette = "SceneEditorPalette";
	%map = new ActionMap();
	%map.bindCmd( keyboard, "1", "EWorldEditorNoneModeBtn.performClick();", "" );  // Select
	%map.bindCmd( keyboard, "2", "EWorldEditorMoveModeBtn.performClick();", "" );  // Move
	%map.bindCmd( keyboard, "3", "EWorldEditorRotateModeBtn.performClick();", "" );  // Rotate
	%map.bindCmd( keyboard, "4", "EWorldEditorScaleModeBtn.performClick();", "" );  // Scale
	IpsEditorPlugin.map = %map;
	new ScriptObject( IpsEditor );
	new PersistenceManager( IPS_EmitterSaver );
	new PersistenceManager( IPS_ParticleSaver );
	new PersistenceManager( IPS_CompositeSaver );
	new SimSet( IPS_UnlistedParticles );
	new SimSet( IPS_UnlistedEmitters );
	new SimSet( IPS_UnlistedComposites );
	new SimSet( IPS_AllParticles );
}
//------------------------------------------------------------------------------
//==============================================================================
function execIpsEd(%loadgui) {
	if (%loadgui) {
		exec( "./gui/IpsEditorTools.gui" );
		exec( "tlab/IpsEditor/gui/IpsEditorToolbar.gui" );
	}

	
	exec( "tlab/IpsEditor/IpsEditorPlugin.cs" );
	exec( "tlab/IpsEditor/IpsEditorParams.cs" );
	execPattern( "tlab/IpsEditor/scripts/*.cs" );
	execPattern( "tlab/IpsEditor/particle/*.cs" );
	execPattern( "tlab/IpsEditor/emitter/*.cs" );
	execPattern( "tlab/IpsEditor/composite/*.cs" );
}
//------------------------------------------------------------------------------
//==============================================================================
function destroyIpsEditor() {
}
//------------------------------------------------------------------------------
