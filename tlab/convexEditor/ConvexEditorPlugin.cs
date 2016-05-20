//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

function ConvexEditorPlugin::onPluginLoaded( %this ) {
	
	if ( !isObject( ConvexActionsMenu ) ) {
		singleton PopupMenu( ConvexActionsMenu ) {
			superClass = "MenuBuilder";
			barTitle = "Sketch";
			Item[0] = "Hollow Selected Shape" TAB "" TAB "ConvexEditorGui.hollowSelection();";
			item[1] = "Recenter Selected Shape" TAB "" TAB "ConvexEditorGui.recenterSelection();";
		};
	}

	%this.popupMenu = ConvexActionsMenu;
}

function ConvexEditorPlugin::onActivated( %this ) {
	//%this.readSettings();
	EditorGui.bringToFront( ConvexEditorGui );
	ConvexEditorGui.setVisible( true );
	ConvexEditorToolbar.setVisible( true );
	ConvexEditorGui.makeFirstResponder( true );
	// Set the status bar here until all tool have been hooked up
	EditorGuiStatusBar.setInfo( "Sketch Tool." );
	EditorGuiStatusBar.setSelection( "" );
	// Add our menu.
	Lab.insertDynamicMenu(ConvexActionsMenu);
	// Sync the pallete button state with the gizmo mode.
	%mode = GlobalGizmoProfile.mode;

	switch$ (%mode) {
	case "None":
		ConvexEditorNoneModeBtn.performClick();

	case "Move":
		ConvexEditorMoveModeBtn.performClick();

	case "Rotate":
		ConvexEditorRotateModeBtn.performClick();

	case "Scale":
		ConvexEditorScaleModeBtn.performClick();
	}

	Parent::onActivated( %this );
}

function ConvexEditorPlugin::onDeactivated( %this ) {
	// %this.writeSettings();
	//ConvexEditorGui.setVisible( false );
	//ConvexEditorOptionsWindow.setVisible( false );
	//ConvexEditorTreeWindow.setVisible( false );
	//ConvexEditorToolbar.setVisible( false );
	// Remove our menu.
	Lab.removeDynamicMenu(ConvexActionsMenu);
	// Lab.menuBar.remove( ConvexActionsMenu );
	Parent::onDeactivated( %this );
}

function ConvexEditorPlugin::onEditMenuSelect( %this, %editMenu ) {
	%hasSelection = false;

	if ( ConvexEditorGui.hasSelection() )
		%hasSelection = true;

	%editMenu.enableItem( 3, false ); // Cut
	%editMenu.enableItem( 4, false ); // Copy
	%editMenu.enableItem( 5, false ); // Paste
	%editMenu.enableItem( 6, %hasSelection ); // Delete
	%editMenu.enableItem( 8, %hasSelection ); // Deselect
}

function ConvexEditorPlugin::handleDelete( %this ) {
	ConvexEditorGui.handleDelete();
}

function ConvexEditorPlugin::handleDeselect( %this ) {
	ConvexEditorGui.handleDeselect();
}

function ConvexEditorPlugin::handleCut( %this ) {
	//WorldEditorInspectorPlugin.handleCut();
}

function ConvexEditorPlugin::handleCopy( %this ) {
	//WorldEditorInspectorPlugin.handleCopy();
}

function ConvexEditorPlugin::handlePaste( %this ) {
	//WorldEditorInspectorPlugin.handlePaste();
}

function ConvexEditorPlugin::isDirty( %this ) {
	return ConvexEditorGui.isDirty;
}

function ConvexEditorPlugin::onSaveMission( %this, %missionFile ) {
	if( ConvexEditorGui.isDirty ) {
		MissionGroup.save( %missionFile );
		ConvexEditorGui.isDirty = false;
	}
}
