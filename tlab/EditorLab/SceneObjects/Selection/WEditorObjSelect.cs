//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
function WorldEditor::addSelectionToAddGroup(%this) {
	for(%i = 0; %i < %this.getSelectionSize(); %i++) {
		%obj = %this.getSelectedObject(%i);
		$InstantGroup.add(%obj);
	}
}
//------------------------------------------------------------------------------


//==============================================================================
function EWorldEditor::areAllSelectedObjectsOfType( %this, %className ) {
	%activeSelection = %this.getActiveSelection();

	if( !isObject( %activeSelection ) )
		return false;

	%count = %activeSelection.getCount();

	for( %i = 0; %i < %count; %i ++ ) {
		%obj = %activeSelection.getObject( %i );

		if( !%obj.isMemberOfClass( %className ) )
			return false;
	}

	return true;
}
//------------------------------------------------------------------------------

//==============================================================================
function EWorldEditor::deleteMissionObject( %this, %object ) {
	// Unselect in editor tree.
	%id = SceneEditorTree.findItemByObjectId( %object );
	SceneEditorTree.selectItem( %id, false );
	// Delete object.
	MEDeleteUndoAction::submit( %object );
	EWorldEditor.isDirty = true;
	SceneEditorTree.buildVisibleTree( true );
}
//------------------------------------------------------------------------------
//==============================================================================
function WorldEditor::getSelectionLockCount(%this) {
	%ret = 0;

	for(%i = 0; %i < %this.getSelectionSize(); %i++) {
		%obj = %this.getSelectedObject(%i);

		if(%obj.locked)
			%ret++;
	}

	return %ret;
}
//------------------------------------------------------------------------------
//==============================================================================
function WorldEditor::getSelectionHiddenCount(%this) {
	%ret = 0;

	for(%i = 0; %i < %this.getSelectionSize(); %i++) {
		%obj = %this.getSelectedObject(%i);

		if(%obj.hidden)
			%ret++;
	}

	return %ret;
}
//------------------------------------------------------------------------------

//==============================================================================
function WorldEditor::onSelectionCentroidChanged( %this ) {
	Scene.setDirty();

	if (%this.lastCentroidPos !$="") {
		%offset = VectorSub(%this.getSelectionCentroid(),%this.lastCentroidPos);
		%this.lastMoveOffset = %offset;
	}

	%this.lastCentroidPos = %this.getSelectionCentroid();
	
	Scene.syncSelectionGui();

	// Refresh inspector.
	Scene.doRefreshInspect();
	
}
//------------------------------------------------------------------------------
//==============================================================================
function WorldEditor::onSelect( %this, %obj,%scriptSide ) {
	logd("WorldEditor::onSelect");
	//Check to tell that the selection is called from a group
	%obj.byGroup = false;	
	//foreach$(%tree in Scene.SceneTrees)
		//%tree.addSelection( %obj,true);

	Scene.onAddSelection(%obj,true,%this);
//	foreach$(%tree in Scene.SceneTrees)
	//	%tree.setSelectedItem( %obj,false,true);

	//Store the source Location of Object 0 in case we drag copy
	_setShadowVizLight( %obj );
	//SceneInspector.inspect( %obj );
	

	//Scene.onSelect(%obj);
	// Inform the camera
	
	
}
//------------------------------------------------------------------------------

//==============================================================================
function WorldEditor::onMultiSelect( %this, %set,%arg ) {
	logd("WorldEditor::onMultiSelect",%set,%arg);
	// This is called when completing a drag selection ( on3DMouseUp )
	// so we can avoid calling onSelect for every object. We can only
	// do most of this stuff, like inspecting, on one object at a time anyway.	
	foreach( %obj in %set ) {		
		SceneEditorTree.setSelectedItem( %obj,false,true);		
	}	


	//Delay the inspect for instant tree select
	if ($InspectorMultiManual)
		Scene.schedule(40,"onInspect",%set.getObject(0));
	else	
		foreach( %obj in %set ) {		
				Scene.doAddInspect(%obj,false);
			}	
}
//------------------------------------------------------------------------------
//==============================================================================
function WorldEditor::onUnSelect( %this, %obj ) {
	logd("WorldEditor::onUnSelect",%obj);
	%this.lastCentroidPos = "";
	%this.lastMoveOffset = "";
	
	Scene.onRemoveSelection(%obj, %this);		
	
}
//------------------------------------------------------------------------------
//==============================================================================
function WorldEditor::onClearSelection( %this ) {
	logd("onClearSelection");
	Scene.onSelectionChanged();
	
}
//------------------------------------------------------------------------------
