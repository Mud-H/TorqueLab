//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
function EWorldEditor::addSimGroup( %this, %groupCurrentSelection ) {
	%activeSelection = %this.getActiveSelection();

	if ( %activeSelection.getObjectIndex( MissionGroup ) != -1 ) {
		LabMsgOK( "Error", "Cannot add MissionGroup to a new SimGroup" );
		return;
	}

	// Find our parent.
	%parent = MissionGroup;

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

	// Create the SimGroup.
	%object = new SimGroup() {
		parentGroup = %parent;
	};
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
	%this.syncGui();
}
//------------------------------------------------------------------------------
//==============================================================================
//Create a SimSet with the Selected objects
function Lab::groupSelectedObjects(%this,%groupName) {
	%count = EWorldEditor.getSelectionSize();
	%grpCount = LabSceneObjectGroups.getCount();

	if (%groupName $= "") %groupName = "ObjGroup_"@%grpCount+1;

	%setName = getUniqueName(%groupName);
	%newSet = newSimset(%setName,LabSceneObjectGroups);
	%newSet.internalName = getUniqueInternalName( "Group", LabSceneObjectGroups, true );
	%newSet.isObjectGroup = true;

	for( %i=0; %i<%count; %i++) {
		%obj = EWorldEditor.getSelectedObject( %i );

		if (isObject( %obj.partOfSet )) {
			%obj.partOfSet.remove(%obj);

			if (!%obj.partOfSet.getCount()) {
				delObj(%obj.partOfSet);
			}
		}

		%obj.partOfSet = %newSet.getName();
		%newSet.add(%obj);
	}
}
//------------------------------------------------------------------------------

//==============================================================================
// Remove selected objects from their group (delete group if empty)
function Lab::ungroupSelectedObjects(%this,%groupName) {
	%count = EWorldEditor.getSelectionSize();

	for( %i=0; %i<%count; %i++) {
		%obj = EWorldEditor.getSelectedObject( %i );

		if (isObject( %obj.partOfSet )) {
			%obj.partOfSet.remove(%obj);

			if (!%obj.partOfSet.getCount()) {
				delObj(%obj.partOfSet);
			}
		}

		%obj.partOfSet = "";
	}

	EWorldEditor.clearSelection();
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::selectObjectGroup(%this,%groupName,%append) {
	//If no group specified, select the group of selected object (0)
	if (!isObject(%groupName)) {
		%selectObj = EWorldEditor.getSelectedObject(0);

		if (isObject(%selectObj.partOfSet)) {
			EWorldEditor.clearSelection();

			foreach(%obj in %selectObj.partOfSet) {
				EWorldEditor.selectObject(%obj);
			}
		}
	} else {
		//Select the objects of specified group
		if (!%append)
			EWorldEditor.clearSelection();

		foreach(%obj in %groupName) {
			%obj.byGroup = true;
			EWorldEditor.selectObject(%obj);
		}
	}
}
//------------------------------------------------------------------------------
//==============================================================================
//Called from code to get the group into which new object will be pasted
function EWorldEditor::getNewObjectGroup( %this ) {
	return Scene.getActiveSimGroup();
}
//------------------------------------------------------------------------------
//==============================================================================
function EWorldEditor::selectAllObjectsInSet( %this, %set, %deselect ) {
	if( !isObject( %set ) )
		return;

	foreach( %obj in %set ) {
		if (%obj.isMemberOfClass("SimSet")){
			EWorldEditor.selectAllObjectsInSet(%obj,%deselect);
			continue;
		}
		if( %deselect )
			%this.unselectObject( %obj );
		else
			%this.selectObject( %obj );
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function EWorldEditor::toggleLockChildren( %this, %simGroup ) {
	foreach( %child in %simGroup ) {
		if( %child.isMemberOfClass( "SimGroup" ) )
			%this.toggleLockChildren( %child );
		else
			%child.setLocked( !%child.locked );
	}

	EWorldEditor.syncGui();
}
//------------------------------------------------------------------------------
//==============================================================================
function EWorldEditor::toggleHideChildren( %this, %simGroup ) {
	devLog("toggleHideChildren",%simGroup);

	foreach( %child in %simGroup ) {
		if( %child.isMemberOfClass( "SimGroup" ) )
			%this.toggleHideChildren( %child );
		else
			%this.hideObject( %child, !%child.hidden );
	}

	EWorldEditor.syncGui();
}
//------------------------------------------------------------------------------

