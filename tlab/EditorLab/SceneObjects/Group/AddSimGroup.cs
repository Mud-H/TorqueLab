//==============================================================================
// TorqueLab -> Editor Gui General Scripts
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
function Scene::addSimGroup( %this, %groupCurrentSelection ) {
	%activeSelection = EWorldEditor.getActiveSelection();

	if ( %activeSelection.getObjectIndex( MissionGroup ) != -1 ) {
		LabMsgOK( "Error", "Cannot add MissionGroup to a new SimGroup" );
		return;
	}

	// Find our parent.
	%parent = %this.getActiveSimGroup();

	if( !%groupCurrentSelection && isObject( %activeSelection ) && %activeSelection.getCount() > 0 ) {
		%firstSelectedObject = %activeSelection.getObject( 0 );

		if( %firstSelectedObject.isMemberOfClass( "SimGroup" ) )
			%parent = %firstSelectedObject;
		else if( %firstSelectedObject.getId() != MissionGroup.getId() )
			%parent = %firstSelectedObject.parentGroup;
	}

	// If we are about to do a group-selected as well,
	// starting recording an undo compound.

	if( %groupCurrentSelection )
		Editor.getUndoManager().pushCompound( "Group Selected" );

	%name =
		// Create the SimGroup.
	%object = new SimGroup() {
		parentGroup = %parent;
	};

	if (%activeSelection.getObject( 0 ).internalName !$="")
		%object.internalName = %activeSelection.getObject( 0 ).internalName;

	MECreateUndoAction::submit( %object );

	// Put selected objects into the group, if requested.

	if( %groupCurrentSelection && isObject( %activeSelection ) ) {
		%undo = UndoActionReparentObjects::create( SceneEditorTree );
		%numObjects = %activeSelection.getCount();

		for( %i = 0; %i < %numObjects; %i ++ ) {
			%sel = %activeSelection.getObject( %i );
			%undo.add( %sel, %sel.parentGroup, %object );
			%object.add( %sel );
		}

		%undo.addToManager( Editor.getUndoManager() );
	}

	// Stop recording for group-selected.

	if( %groupCurrentSelection )
		Editor.getUndoManager().popCompound();

	// When not grouping selection, make the newly created SimGroup the
	// current selection.

	if( !%groupCurrentSelection ) {
		EWorldEditor.clearSelection();
		EWorldEditor.selectObject( %object );
	}

	// Refresh the Gui.
	//%this.syncGui();
}
//------------------------------------------------------------------------------