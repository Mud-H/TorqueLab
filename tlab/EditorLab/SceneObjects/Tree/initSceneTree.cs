//==============================================================================
// TorqueLab -> Editor Gui General Scripts
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
function SceneObjectsTree::onDefineIcons( %this ) {
	%icons = "tlab/art/icons/iconTables/TreeViewBase/default:" @
				"tlab/art/icons/iconTables/TreeViewBase/folderclosed:" @
				"tlab/art/icons/iconTables/TreeViewBase/groupclosed:" @
				"tlab/art/icons/iconTables/TreeViewBase/folderopen:" @
				"tlab/art/icons/iconTables/TreeViewBase/groupopen:" @
				"tlab/art/icons/iconTables/TreeViewBase/hidden:" @
				"tlab/art/icons/iconTables/TreeViewBase/shll_icon_passworded_hi:" @
				"tlab/art/icons/iconTables/TreeViewBase/shll_icon_passworded:" @
				"tlab/art/icons/iconTables/TreeViewBase/default";
	%this.buildIconTable(%icons);
}
//------------------------------------------------------------------------------

//==============================================================================
function SceneObjectsTree::onAdd( %this ) {
	Scene.SceneTrees = strAddWord(Scene.SceneTrees,%this.getId(),1);
}
//------------------------------------------------------------------------------
//==============================================================================
function SceneObjectsTree::onRemove( %this ) {
	Scene.SceneTrees = strRemoveWord(Scene.SceneTrees,%this.getId());
}
//------------------------------------------------------------------------------
//==============================================================================
/// @name EditorPlugin Methods
/// @{
function SceneObjectsTree::handleRenameObject( %this, %name, %obj ) {
	logd(" SceneObjectsTree::handleRenameObject(",%name, %obj);

	if (!isObject(%obj))
		return;

	%field = ( %this.renameInternal ) ? "internalName" : "name";
	%isDirty = LabObj.set(%obj,%field,%name);
	info("Group:",%obj,"Is Dirty",%isDirty);
}

//------------------------------------------------------------------------------



function clearTrees(  ) {
	foreach$(%tree in Scene.SceneTrees)
		%tree.clearSelection();
	EWorldEditor.clearSelection();
}



/*
DECLARE_CALLBACK( bool, onDeleteObject, ( SimObject* object ) );
      DECLARE_CALLBACK( bool, isValidDragTarget, ( S32 id, const char* value ) );
      DECLARE_CALLBACK( void, onDefineIcons, () );
      DECLARE_CALLBACK( void, onAddGroupSelected, ( SimGroup* group ) );
      DECLARE_CALLBACK( void, onAddSelection, ( S32 itemOrObjectId, bool isLastSelection ) );
      DECLARE_CALLBACK( void, onSelect, ( S32 itemOrObjectId ) );
      DECLARE_CALLBACK( void, onInspect, ( S32 itemOrObjectId ) );
      DECLARE_CALLBACK( void, onRemoveSelection, ( S32 itemOrObjectId ) );
      DECLARE_CALLBACK( void, onUnselect, ( S32 itemOrObjectId ) );
      DECLARE_CALLBACK( void, onDeleteSelection, () );
      DECLARE_CALLBACK( void, onObjectDeleteCompleted, () );
      DECLARE_CALLBACK( void, onKeyDown, ( S32 modifier, S32 keyCode ) );
      DECLARE_CALLBACK( void, onMouseUp, ( S32 hitItemId, S32 mouseClickCount ) );
      DECLARE_CALLBACK( void, onMouseDragged, () );
      DECLARE_CALLBACK( void, onRightMouseDown, ( S32 itemId, const Point2I& mousePos, SimObject* object = NULL ) );
      DECLARE_CALLBACK( void, onRightMouseUp, ( S32 itemId, const Point2I& mousePos, SimObject* object = NULL ) );
      DECLARE_CALLBACK( void, onBeginReparenting, () );
      DECLARE_CALLBACK( void, onEndReparenting, () );
      DECLARE_CALLBACK( void, onReparent, ( S32 itemOrObjectId, S32 oldParentItemOrObjectId, S32 newParentItemOrObjectId ) );
      DECLARE_CALLBACK( void, onDragDropped, () );
      DECLARE_CALLBACK( void, onAddMultipleSelectionBegin, () );
      DECLARE_CALLBACK( void, onAddMultipleSelectionEnd, () );
      DECLARE_CALLBACK( bool, canRenameObject, ( SimObject* object ) );
      DECLARE_CALLBACK( bool, handleRenameObject, ( const char* newName, SimObject* object ) );
      DECLARE_CALLBACK( void, onClearSelection, () );
      */