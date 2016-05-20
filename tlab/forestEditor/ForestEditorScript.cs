//==============================================================================
// LabEditor ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
function ForestEditorPlugin::onEditMenuSelect( %this, %editMenu ) {
	%hasSelection = false;
	%selTool = ForestTools->SelectionTool;

	if ( ForestEditorGui.getActiveTool() == %selTool )
		if ( %selTool.getSelectionCount() > 0 )
			%hasSelection = true;

	%editMenu.enableItem( 3, %hasSelection ); // Cut
	%editMenu.enableItem( 4, %hasSelection ); // Copy
	%editMenu.enableItem( 5, %hasSelection ); // Paste
	%editMenu.enableItem( 6, %hasSelection ); // Delete
	%editMenu.enableItem( 8, %hasSelection ); // Deselect
}

//==============================================================================
// Callbacks Handlers - Called on specific editor actions
//==============================================================================

function ForestEditorPlugin::handleDelete( %this ) {
	ForestTools->SelectionTool.deleteSelection();
}

function ForestEditorPlugin::handleDeselect( %this ) {
	ForestTools->SelectionTool.clearSelection();
}

function ForestEditorPlugin::handleCut( %this ) {
	ForestTools->SelectionTool.cutSelection();
}

function ForestEditorPlugin::handleCopy( %this ) {
	ForestTools->SelectionTool.copySelection();
}

function ForestEditorPlugin::handlePaste( %this ) {
	ForestTools->SelectionTool.pasteSelection();
}