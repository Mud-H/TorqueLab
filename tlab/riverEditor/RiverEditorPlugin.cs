//==============================================================================
// LabEditor ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// Road Editor Params - Used set default settings and build plugins options GUI
//==============================================================================

//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function RiverEditorPlugin::initParamsArray( %this,%cfgArray ) {
	%cfgArray.group[%gId++] = "General settings";
	%cfgArray.setVal("DefaultWidth",       "10" TAB "DefaultWidth" TAB "TextEdit" TAB "" TAB "RiverEditorGui" TAB %gId);
	%cfgArray.setVal("DefaultDepth",   "5" TAB "DefaultDepth" TAB "TextEdit" TAB "" TAB "RiverEditorGui" TAB %gId);
	%cfgArray.setVal("DefaultNormal","0 0 1" TAB "DefaultNormal" TAB "TextEdit" TAB "" TAB "RiverEditorGui" TAB %gId);
	%cfgArray.group[%gId++] = "Color settings";
	%cfgArray.setVal("HoverSplineColor",       "255 0 0 255" TAB "HoverSplineColor" TAB "ColorEdit" TAB "mode>>init" TAB "RiverEditorGui" TAB %gId);
	%cfgArray.setVal("SelectedSplineColor",       "0 255 0 255" TAB "SelectedSplineColor" TAB "ColorEdit" TAB "mode>>init" TAB "RiverEditorGui" TAB %gId);
	%cfgArray.setVal("HoverNodeColor",       "255 255 255 255" TAB "HoverNodeColor" TAB "ColorEdit" TAB "mode>>init" TAB "RiverEditorGui" TAB %gId);
}
//------------------------------------------------------------------------------

//==============================================================================
// Plugin Object Callbacks - Called from TLab plugin management scripts
//==============================================================================
function RiverEditorPlugin::onPluginLoaded( %this ) {	
	// Add ourselves to the Editor Settings window
}

function RiverEditorPlugin::onActivated( %this ) {
	$River::EditorOpen = true;
	LabPaletteArray->RiverEditorAddRiverMode.performClick();
	EditorGui.bringToFront( RiverEditorGui );
	RiverEditorGui.setVisible(true);
	RiverEditorGui.makeFirstResponder( true );
	RiverEditorToolbar.setVisible(true);
	RiverEditorOptionsWindow.setVisible( true );
	RiverEditorTreeWindow.setVisible( true );
	RiverTreeView.open(ServerRiverSet,true);
	// Store this on a dynamic field
	// in order to restore whatever setting
	// the user had before.
	%this.prevGizmoAlignment = GlobalGizmoProfile.alignment;
	// The DecalEditor always uses Object alignment.
	GlobalGizmoProfile.alignment = "Object";
	// Set the status bar here until all tool have been hooked up
	EditorGuiStatusBar.setInfo("River editor.");
	EditorGuiStatusBar.setSelection("");
	// Allow the Gui to setup.
	//RiverEditorGui.onEditorActivated();
	Parent::onActivated(%this);
	RiverManager.autoCollapsePill = true;
	RiverManager.showPositionEdit = false;
	RiverEd_PropertiesBook.selectPage(0);
	RiverManager.updateRiverData();
}

function RiverEditorPlugin::onDeactivated( %this ) {
	$River::EditorOpen = false;
	RiverEditorGui.setVisible(false);
	RiverEditorToolbar.setVisible(false);
	RiverEditorOptionsWindow.setVisible( false );
	RiverEditorTreeWindow.setVisible( false );
	// Restore the previous Gizmo
	// alignment settings.
	GlobalGizmoProfile.alignment = %this.prevGizmoAlignment;
	// Allow the Gui to cleanup.
	RiverEditorGui.onEditorDeactivated();
	Parent::onDeactivated(%this);
}



function RiverEditorPlugin::isDirty( %this ) {
	return RiverEditorGui.isDirty;
}

function RiverEditorPlugin::onSaveMission( %this, %missionFile ) {
	if( RiverEditorGui.isDirty ) {
		MissionGroup.save( %missionFile );
		RiverEditorGui.isDirty = false;
	}
}

function RiverEditorPlugin::onEditMenuSelect( %this, %editMenu ) {
	%hasSelection = false;

	if( isObject( RiverEditorGui.river ) )
		%hasSelection = true;

	%editMenu.enableItem( 3, false ); // Cut
	%editMenu.enableItem( 4, false ); // Copy
	%editMenu.enableItem( 5, false ); // Paste
	%editMenu.enableItem( 6, %hasSelection ); // Delete
	%editMenu.enableItem( 8, false ); // Deselect
}

function RiverEditorPlugin::handleDelete( %this ) {
	RiverEditorGui.deleteNode();
}

function RiverEditorPlugin::handleEscape( %this ) {
	return RiverEditorGui.onEscapePressed();
}