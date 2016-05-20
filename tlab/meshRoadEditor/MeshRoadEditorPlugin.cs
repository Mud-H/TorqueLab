//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
function MeshRoadEditorPlugin::onPluginLoaded( %this ) {	
	// Add ourselves to the Editor Settings window
}
//------------------------------------------------------------------------------
//==============================================================================
function MeshRoadEditorPlugin::onActivated( %this ) {
	LabPaletteArray->MeshRoadEditorAddRoadMode.performClick();
	EditorGui.bringToFront( MeshRoadEditorGui );
	MeshRoadEditorGui.makeFirstResponder( true );
	MeshRoadTreeView.open(ServerMeshRoadSet,true);
	// Store this on a dynamic field
	// in order to restore whatever setting
	// the user had before.
	%this.prevGizmoAlignment = GlobalGizmoProfile.alignment;
	// The DecalEditor always uses Object alignment.
	//GlobalGizmoProfile.alignment = "Object";
	Lab.setGizmoAlignment("Object");
	// Set the status bar here until all tool have been hooked up
	EditorGuiStatusBar.setInfo("Mesh road editor.");
	EditorGuiStatusBar.setSelection("");
	MRoadManager.Init();
	Parent::onActivated(%this);
	MeshRoad_TabBook.selectPage(0);
}
//------------------------------------------------------------------------------
//==============================================================================
function MeshRoadEditorPlugin::onDeactivated( %this ) {
	// Restore the previous Gizmo
	// alignment settings.
	Lab.setGizmoAlignment(%this.prevGizmoAlignment);
	//GlobalGizmoProfile.alignment = %this.prevGizmoAlignment;
	Parent::onDeactivated(%this);
}
//------------------------------------------------------------------------------
//==============================================================================
function MeshRoadEditorPlugin::onEditMenuSelect( %this, %editMenu ) {
	%hasSelection = false;

	if( isObject( MeshRoadEditorGui.road ) )
		%hasSelection = true;

	%editMenu.enableItem( 3, false ); // Cut
	%editMenu.enableItem( 4, false ); // Copy
	%editMenu.enableItem( 5, false ); // Paste
	%editMenu.enableItem( 6, %hasSelection ); // Delete
	%editMenu.enableItem( 8, false ); // Deselect
}
//------------------------------------------------------------------------------
//==============================================================================
function MeshRoadEditorPlugin::handleDelete( %this ) {
	MeshRoadEditorGui.deleteNode();
}
//------------------------------------------------------------------------------
//==============================================================================
function MeshRoadEditorPlugin::handleEscape( %this ) {
	return MeshRoadEditorGui.onEscapePressed();
}
//------------------------------------------------------------------------------
//==============================================================================
function MeshRoadEditorPlugin::isDirty( %this ) {
	return MeshRoadEditorGui.isDirty;
}
//------------------------------------------------------------------------------
//==============================================================================
function MeshRoadEditorPlugin::onSaveMission( %this, %missionFile ) {
	if( MeshRoadEditorGui.isDirty ) {
		MissionGroup.save( %missionFile );
		MeshRoadEditorGui.isDirty = false;
	}
}
//------------------------------------------------------------------------------
//==============================================================================