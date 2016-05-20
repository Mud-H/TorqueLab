//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================



//==============================================================================
function EWorldEditor::convertSelectionToPolyhedralObjects( %this, %className ) {
	%group = Scene.getNewObjectGroup();
	%undoManager = Editor.getUndoManager();
	%activeSelection = %this.getActiveSelection();

	while( %activeSelection.getCount() != 0 ) {
		%oldObject = %activeSelection.getObject( 0 );
		%newObject = %this.createPolyhedralObject( %className, %oldObject );

		if( isObject( %newObject ) ) {
			%undoManager.pushCompound( "Convert ConvexShape to " @ %className );
			%newObject.parentGroup = %oldObject.parentGroup;
			MECreateUndoAction::submit( %newObject );
			MEDeleteUndoAction::submit( %oldObject );
			%undoManager.popCompound();
		}
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function EWorldEditor::convertSelectionToConvexShape( %this ) {
	%group = Scene.getNewObjectGroup();
	%undoManager = Editor.getUndoManager();
	%activeSelection = %this.getActiveSelection();

	while( %activeSelection.getCount() != 0 ) {
		%oldObject = %activeSelection.getObject( 0 );
		%newObject = %this.createConvexShapeFrom( %oldObject );

		if( isObject( %newObject ) ) {
			%undoManager.pushCompound( "Convert " @ %oldObject.getClassName() @ " to ConvexShape" );
			%newObject.parentGroup = %oldObject.parentGroup;
			MECreateUndoAction::submit( %newObject );
			MEDeleteUndoAction::submit( %oldObject );
			%undoManager.popCompound();
		}
	}
}
//------------------------------------------------------------------------------
