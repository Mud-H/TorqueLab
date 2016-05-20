//==============================================================================
// TorqueLab -> Scene Tree Group Callbacks
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// Set object selected in scene (Trees and WorldEditor)
//==============================================================================

//==============================================================================
function SceneObjectsTree::onAddGroupSelected(%this, %simGroup) {
	logd("SceneObjectsTree::onAddGroupSelected %simGroup",%simGroup);
	
	Scene.setNewObjectGroup(%group);
}
//------------------------------------------------------------------------------
//==============================================================================
function SceneObjectsTree::onAddMultipleSelectionBegin(%this) {
	logd("SceneObjectsTree::onAddMultipleSelectionBegin");
}
//------------------------------------------------------------------------------
//==============================================================================
function SceneObjectsTree::onAddMultipleSelectionEnd(%this) {
	logd("SceneObjectsTree::onAddMultipleSelectionEnd");
}
//------------------------------------------------------------------------------
