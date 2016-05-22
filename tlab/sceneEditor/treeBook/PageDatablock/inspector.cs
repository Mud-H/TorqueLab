//==============================================================================
// TorqueLab -> Datablock Editor - Events Manager
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// Inspector Events
//==============================================================================
/*
//==============================================================================
function DatablockEditorInspector::onInspectorFieldModified( %this, %object, %fieldName, %arrayIndex, %oldValue, %newValue ) {
	logd(" DatablockEditorInspector::onInspectorFieldModified",%object, %fieldName, %arrayIndex, %oldValue, %newValue );
	
	// Same work to do as for the regular WorldEditor Inspector.	
	Inspector::onInspectorFieldModified( %this, %object, %fieldName, %arrayIndex, %oldValue, %newValue );
		DatablockEditorPlugin.flagDatablockAsDirty( %object, true );

}
//------------------------------------------------------------------------------
//==============================================================================
function DatablockEditorInspector::onFieldSelected( %this, %fieldName, %fieldTypeStr, %fieldDoc ) {
	DatablockFieldInfoControl.setText( "<font:ArialBold:14>" @ %fieldName @ "<font:ArialItalic:14> (" @ %fieldTypeStr @ ") " NL "<font:Arial:14>" @ %fieldDoc );
}
//------------------------------------------------------------------------------
//==============================================================================
function DatablockEditorInspector::onBeginCompoundEdit( %this ) {
	Editor.getUndoManager().pushCompound( "Multiple Field Edit" );
}
//------------------------------------------------------------------------------
//==============================================================================
function DatablockEditorInspector::onEndCompoundEdit( %this, %discard ) {
	Editor.getUndoManager().popCompound( %discard );
}
//------------------------------------------------------------------------------
//==============================================================================
function DatablockEditorInspector::onClear( %this ) {
	DatablockFieldInfoControl.setText( "" );
}
//------------------------------------------------------------------------------


*/

//==============================================================================
function DbEd::setFilters( %this,%id ) {
	switch$(%id) {
	case "":
		DatablockEditorInspector.groupFilters = "";

	case "0":
		DatablockEditorInspector.groupFilters = "+Scripting -Object";

	case "1":
		DatablockEditorInspector.groupFilters = "-Scripting +Object";

	case "2":
		DatablockEditorInspector.groupFilters = "-Scripting +Object";
	}

	DatablockEditorInspector.refresh();
}
//------------------------------------------------------------------------------