//==============================================================================
// TorqueLab -> Scene Tree Reparenting Callbacks
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// Set object selected in scene (Trees and WorldEditor)
//==============================================================================

//==============================================================================
function SceneObjectsTree::onBeginReparenting( %this ) {
	if( isObject( %this.reparentUndoAction ) )
		%this.reparentUndoAction.delete();

	%action = UndoActionReparentObjects::create( %this );
	%this.reparentUndoAction = %action;
}
//------------------------------------------------------------------------------

//==============================================================================
function SceneObjectsTree::onReparent( %this, %obj, %oldParent, %newParent ) {
	%this.reparentUndoAction.add( %obj, %oldParent, %newParent );
}
//------------------------------------------------------------------------------

//==============================================================================
function SceneObjectsTree::onEndReparenting( %this ) {
	%action = %this.reparentUndoAction;
	%this.reparentUndoAction = "";

	if( %action.numObjects > 0 ) {
		if( %action.numObjects == 1 )
			%action.actionName = "Reparent Object";
		else
			%action.actionName = "Reparent Objects";

		%action.addToManager( Editor.getUndoManager() );
		EWorldEditor.syncGui();
	} else
		%action.delete();
}
//------------------------------------------------------------------------------