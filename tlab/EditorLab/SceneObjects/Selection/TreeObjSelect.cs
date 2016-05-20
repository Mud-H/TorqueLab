//==============================================================================
// TorqueLab -> Scene Tree Selection Callbacks
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

$SelectTreeOnly = false;
$TreeBasicSel = true;
//==============================================================================
// SceneTree Select/Unselect related callbacks
//==============================================================================

//trace(0);
//==============================================================================
function SceneObjectsTree::onSelect(%this, %obj) {	
	logd("SceneObjectsTree::onSelect ");

	//Simply clear WorldEditor
	//EWorldEditor.clearSelection();
	if ($SelectTreeOnly || $TreeBasicSel)
		return;

	
	//if (EWorldEditor.isVisible())
		//EWorldEditor.selectObject(%obj);	

	//Scene.onSelect(%obj);
	//EWorldEditor.selectObject(%obj);
	//Scene.onSelect(%obj);	
}
//------------------------------------------------------------------------------
//==============================================================================
function SceneObjectsTree::onUnselect(%this, %obj) {
	logd("SceneObjectsTree::onUnSelect",%obj);
	
	//Scene.onUnSelect(%obj);
	//Scene.unselectObject(%obj,%this);
	Scene.onRemoveSelection(%obj, %this);
}
//------------------------------------------------------------------------------
//==============================================================================

function SceneObjectsTree::onAddSelection(%this, %obj, %isLastSelection) {
	logd("SceneObjectsTree::onAddSelection IsLast",%isLastSelection);
	
	//if ($SelectTreeOnly)
	if ($SelectTreeOnly)
		return;

	Scene.onAddSelection(%obj, %isLastSelection,%this);
}
//------------------------------------------------------------------------------
//==============================================================================
// Called when an item with no child is selected
function SceneObjectsTree::onInspect(%this, %obj) {
	
	//if (isObject(%this.myInspector)){
	//devLog("Updating tree owned inspector",%this.myInspector);
	//%this.myInspector.inspect(%obj);
	//}
	
}
//------------------------------------------------------------------------------
//==============================================================================
// SceneTree Selection related callbacks
//==============================================================================


//==============================================================================
// Called after the current tree selection was cleared
function SceneObjectsTree::onClearSelection(%this) {
	logd("SceneBrowserTree::onClearSelection");
	//Scene.doInspect("");
}
//------------------------------------------------------------------------------
//==============================================================================
// Called after a single object was removed from tree selection
function SceneObjectsTree::onRemoveSelection(%this, %obj) {
	logd("SceneObjectsTree::onRemoveSelection",%obj);
	
	//This is important to unselect object in WorldEditor
	Scene.onRemoveSelection(%obj,%this);

}
//------------------------------------------------------------------------------

//==============================================================================
// SceneTree Deletion related callbacks
//==============================================================================
//==============================================================================
// Called just before selection deletion process start
function SceneObjectsTree::onDeleteSelection( %this ) {
	%this.undoDeleteList = "";
}
//------------------------------------------------------------------------------

//==============================================================================
// Called prior object deletion, would abort deletion if returning true
function SceneObjectsTree::onDeleteObject( %this, %object ) {
	// Don't delete locked objects
	if( %object.locked )
		return true;

	if( %object == SceneEd.objectGroup )
		Scene.setNewObjectGroup( MissionGroup );

	// Append it to our list.
	%this.undoDeleteList = %this.undoDeleteList TAB %object;
	// We're gonna delete this ourselves in the
	// completion callback.
	return true;
}
//------------------------------------------------------------------------------
//==============================================================================
// Called after a tree object have beenn deleted
function SceneObjectsTree::onObjectDeleteCompleted( %this ) {
	// This can be called when a deletion is attempted but nothing was
	// actually deleted ( cannot delete the root of the tree ) so only submit
	// the undo if we really deleted something.
	if ( %this.undoDeleteList !$= "" )
		MEDeleteUndoAction::submit( %this.undoDeleteList );

	Scene.onObjectDeleteCompleted();
}
//------------------------------------------------------------------------------
//==============================================================================
// SceneTree Object UnSelect Functions
//==============================================================================



//==============================================================================
// SceneTree Object Deletion Functions
//==============================================================================

