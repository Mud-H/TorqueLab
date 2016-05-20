//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
function FEP_Manager::createLevelItemGroup( %this ) {
}
//------------------------------------------------------------------------------


//==============================================================================
// Brush Tree Functions
//==============================================================================

//==============================================================================
function FEP_Manager::deleteSelectedItem( %this ) {
	%selItemList = ForestManagerItemTree.getSelectedItemsList();
	%selObjList = ForestManagerItemTree.getSelectedObjectList();
	devLog("Selected ITEMS:",%selItemList);
	devLog("Selected OBJECTS:",%selObjList);
}
//------------------------------------------------------------------------------
//==============================================================================
// Brush Tree Callbacks
//==============================================================================
//==============================================================================
function ForestManagerItemTree::onAddGroupSelected( %this,%simGroup ) {
	devLog("ForestManagerItemTree::onAddGroupSelected( %this,%simGroup )",%this,%simGroup );
}
//------------------------------------------------------------------------------
//==============================================================================
function ForestManagerItemTree::onAddMultipleSelectionBegin( %this,%arg1 ) {
	devLog("ForestManagerItemTree::onAddMultipleSelectionBegin( %this,%arg1 )",%this,%arg1 );
}
//------------------------------------------------------------------------------
//==============================================================================
function ForestManagerItemTree::onAddMultipleSelectionEnd( %this,%arg1 ) {
	devLog("ForestManagerItemTree::onAddMultipleSelectionEnd( %this,%arg1 )",%this,%arg1 );
}
//------------------------------------------------------------------------------
//==============================================================================
function ForestManagerItemTree::onBeginReparenting( %this,%arg1 ) {
	devLog("ForestManagerItemTree::onBeginReparenting( %this,%arg1 )",%this,%arg1 );
}
//------------------------------------------------------------------------------
//==============================================================================
function ForestManagerItemTree::onClearSelection( %this,%arg1 ) {
	devLog("ForestManagerItemTree::onClearSelection( %this,%arg1 )",%this,%arg1 );
}
//------------------------------------------------------------------------------
//==============================================================================
function ForestManagerItemTree::onDeleteObject( %this,%obj ) {
	devLog("ForestManagerItemTree::onDeleteObject( %this,%obj )",%this,%obj );
}
//------------------------------------------------------------------------------
//==============================================================================
function ForestManagerItemTree::onDeleteSelection( %this,%arg1 ) {
	devLog("ForestManagerItemTree::onDeleteSelection( %this,%arg1 )",%this,%arg1 );
}
//------------------------------------------------------------------------------
//==============================================================================
function ForestManagerItemTree::onDragDropped( %this,%arg1 ) {
	devLog("ForestManagerItemTree::onDragDropped( %this,%arg1 )",%this,%arg1 );
}
//------------------------------------------------------------------------------
//==============================================================================
function ForestManagerItemTree::onEndReparenting( %this,%arg1 ) {
	devLog("ForestManagerItemTree::onEndReparenting( %this,%arg1 )",%this,%arg1 );
}
//------------------------------------------------------------------------------
//==============================================================================
function ForestManagerItemTree::onInspect( %this,%item ) {
	devLog("ForestManagerItemTree::onInspect( %this,%item )",%this,%item );
}
//------------------------------------------------------------------------------
//==============================================================================
function ForestManagerItemTree::onKeyDown( %this,%modifier,%keyCode ) {
	devLog("ForestManagerItemTree::onKeyDown( %this,%modifier,%keyCode )",%this,%modifier,%keyCode );
}
//------------------------------------------------------------------------------
//==============================================================================
function ForestManagerItemTree::onMouseDragged( %this,%arg1 ) {
	devLog("ForestManagerItemTree::onMouseDragged( %this,%arg1 )",%this,%arg1 );
}
//------------------------------------------------------------------------------
//==============================================================================
function ForestManagerItemTree::onObjectDeleteCompleted( %this,%arg1 ) {
	devLog("ForestManagerItemTree::onObjectDeleteCompleted( %this,%arg1 )",%this,%arg1 );
}
//------------------------------------------------------------------------------
//==============================================================================
function ForestManagerItemTree::onRemoveSelection( %this,%item ) {
	devLog("ForestManagerItemTree::onRemoveSelection( %this,%item )",%this,%item );
}
//------------------------------------------------------------------------------
//==============================================================================
function ForestManagerItemTree::onReparent( %this,%item,%oldParent,%newParent ) {
	devLog("ForestManagerItemTree::onReparent( %this,%item,%oldParent,%newParent )",%this,%item,%oldParent,%newParent );
}
//------------------------------------------------------------------------------
//==============================================================================
function ForestManagerItemTree::onRightMouseDown( %this,%itemId,%mousePos,%obj ) {
	devLog("ForestManagerItemTree::onRightMouseDown( %this,%itemId,%mousePos,%obj )",%this,%itemId,%mousePos,%obj );
}
//------------------------------------------------------------------------------
//==============================================================================
function ForestManagerItemTree::onRightMouseUp( %this,%itemId,%mousePos,%obj ) {
	devLog("ForestManagerItemTree::onRightMouseUp( %this,%itemId,%mousePos,%obj )",%this,%itemId,%mousePos,%obj );
}
//------------------------------------------------------------------------------
//==============================================================================
function ForestManagerItemTree::onSelect( %this,%item ) {
	devLog("ForestManagerItemTree::onSelect( %this,%item )",%this,%item );
}
//------------------------------------------------------------------------------
//==============================================================================
function ForestManagerItemTree::onUnselect( %this,%item ) {
	devLog("ForestManagerItemTree::onUnselect( %this,%item )",%this,%item );
}
//------------------------------------------------------------------------------