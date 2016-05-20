//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//=============================================================================================
//    IpsEditorPlugin.
//=============================================================================================

//---------------------------------------------------------------------------------------------

function IpsEditorPlugin::onPluginLoaded( %this ) {	
	IPSC_Editor.visible = 0;
}

//---------------------------------------------------------------------------------------------

function IpsEditorPlugin::onActivated( %this ) {
	if( !%this.isInitialized ) {
		IpsEditor.initEditor();
		%this.isInitialized = true;
	}

	//SceneEditorToolbar.setVisible( true );
	//EditorGui.bringToFront( IPS_Window);
	//IPS_Window.setVisible( true );
	IPS_Window.makeFirstResponder( true );
	IpsEditor.resetEmitterNode();
	// Set the status bar here
	EditorGuiStatusBar.setInfo( "Particle editor." );
	EditorGuiStatusBar.setSelection( "" );
	Parent::onActivated( %this );
}

//---------------------------------------------------------------------------------------------

function IpsEditorPlugin::onDeactivated( %this ) {
	//SceneEditorToolbar.setVisible( false );
	//IPS_Window.setVisible( false );

	if( isObject( $IpsEditor::emitterNode) )
		$IpsEditor::emitterNode.delete();

	Parent::onDeactivated( %this );
}

//---------------------------------------------------------------------------------------------

function IpsEditorPlugin::onExitMission( %this ) {
	// Force Particle Editor to re-initialize.
	%this.isInitialized = false;
	Parent::onExitMission( %this );
}
