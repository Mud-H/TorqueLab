//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//=============================================================================================
//    Event Handlers.
//=============================================================================================

//---------------------------------------------------------------------------------------------

function GuiEditor::onDelete(%this) {
	GuiEditorTreeView.update();
	// clear out the gui inspector.
	GuiEditorInspectFields.update(0);
}

//---------------------------------------------------------------------------------------------

function GuiEditor::onSelectionMoved( %this, %ctrl ) {
	GuiEditorInspectFields.update( %ctrl );
}

//---------------------------------------------------------------------------------------------

function GuiEditor::onSelectionResized( %this, %ctrl ) {
	GuiEditorInspectFields.update( %ctrl );
}

//---------------------------------------------------------------------------------------------

function GuiEditor::onSelect(%this, %ctrl) {
	if( !%this.dontSyncTreeViewSelection ) {
		GuiEditorTreeView.clearSelection();
		GuiEditorTreeView.addSelection( %ctrl );
	}

	GuiEditorInspectFields.update( %ctrl );
	GuiEditorSelectionStatus.setText( "1 Control Selected" );
	//GuiEd.templateTargetChanged(%ctrl);
}

//---------------------------------------------------------------------------------------------

function GuiEditor::onAddSelected( %this, %ctrl ) {
	if( !%this.dontSyncTreeViewSelection ) {
		GuiEditorTreeView.addSelection( %ctrl );
		GuiEditorTreeView.scrollVisibleByObjectId( %ctrl );
	}

	GuiEditorSelectionStatus.setText( %this.getNumSelected() @ " Controls Selected" );
	// Add to inspection set.
	GuiEditorInspectFields.addInspect( %ctrl );
	//GuiEd.templateTargetChanged(%ctrl);
}

//---------------------------------------------------------------------------------------------

function GuiEditor::onRemoveSelected( %this, %ctrl ) {
	if( !%this.dontSyncTreeViewSelection )
		GuiEditorTreeView.removeSelection( %ctrl );

	GuiEditorSelectionStatus.setText( %this.getNumSelected() @ " Controls Selected" );
	// Remove from inspection set.
	GuiEditorInspectFields.removeInspect( %ctrl );
}

//---------------------------------------------------------------------------------------------

function GuiEditor::onClearSelected( %this ) {
	if( !%this.dontSyncTreeViewSelection )
		GuiEditorTreeView.clearSelection();

	GuiEditorInspectFields.update( 0 );
	GuiEditorSelectionStatus.setText( "" );
}

//---------------------------------------------------------------------------------------------

function GuiEditor::onControlDragged( %this, %payload, %position ) {
	// Make sure we have the right kind of D&D.
	if( !%payload.parentGroup.isInNamespaceHierarchy( "GuiDragAndDropControlType_GuiControl" ) )
		return;

	// use the position under the mouse cursor, not the payload position.
	%position = VectorSub( %position, GuiEditorContent.getGlobalPosition() );
	%x = getWord( %position, 0 );
	%y = getWord( %position, 1 );
	%target = GuiEditor.getContentControl().findHitControl( %x, %y );

	// Make sure the target is a valid parent for our payload.

	while(    ( !%target.isContainer || !%target.acceptsAsChild( %payload ) )
				 && %target != GuiEditor.getContentControl() )
		%target = %target.getParent();

	if( %target != %this.getCurrentAddSet() )
		%this.setCurrentAddSet( %target );
}

//---------------------------------------------------------------------------------------------

function GuiEditor::onControlDropped(%this, %payload, %position) {
	// Make sure we have the right kind of D&D.
	if( !%payload.parentGroup.isInNamespaceHierarchy( "GuiDragAndDropControlType_GuiControl" ) )
		return;

	%pos = %payload.getGlobalPosition();
	%x = getWord(%pos, 0);
	%y = getWord(%pos, 1);
	%this.addNewCtrl(%payload);
	%payload.setPositionGlobal(%x, %y);
	%this.setFirstResponder();
}

//---------------------------------------------------------------------------------------------

function GuiEditor::onGainFirstResponder(%this) {
	%this.enableMenuItems(true);
	// JCF: don't just turn them all on!
	// Undo/Redo is only enabled if those actions exist.
	%this.updateUndoMenu();
}

//---------------------------------------------------------------------------------------------

function GuiEditor::onLoseFirstResponder(%this) {
	%this.enableMenuItems(false);
}

//---------------------------------------------------------------------------------------------

function GuiEditor::onHierarchyChanged( %this ) {
	GuiEditorTreeView.update();
}

//---------------------------------------------------------------------------------------------

function GuiEditor::onMouseModeChange( %this ) {
	GuiEditorStatusBar.setText( GuiEditorStatusBar.getMouseModeHelp() );
}
