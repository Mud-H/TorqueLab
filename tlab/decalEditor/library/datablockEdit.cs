//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//------------------------------------------------------------------------------
// Delete Decal Datablocks

// This functionality solely depends on the undo/redo datablock callbacks in
// source.

function DecalEditorGui::redoDeleteDecalDatablock( %this, %datablock ) {
	// Remove the object from file and place a filter
	if( %datablock.getFilename() !$= "" ) {
		DecalPMan.removeDirty( %datablock );
		DecalPMan.removeObjectFromFile( %datablock );
	}

	DecalDataList.addFilteredItem( %datablock );
}

function DecalEditorGui::undoDeleteDecalDatablock( %this, %datablock ) {
	// Replace the object in file and remove the filter
	%filename = %datablock.getFilename();

	if( %datablock.getFilename() !$= "" ) {
		DecalPMan.setDirty( %datablock, %filename );
		DecalPMan.saveDirty();
	}

	DecalDataList.removeFilteredItem( %datablock );
}

function DecalInspector::onInspectorFieldModified( %this, %object, %fieldName, %arrayIndex, %oldValue, %newValue ) {
	if( %fieldName $= "Material" )
		DecalEditorGui.updateDecalPreview( %newValue );

	// Same work to do as for the regular WorldEditor Inspector.
	Inspector::onInspectorFieldModified( %this, %object, %fieldName, %arrayIndex, %oldValue, %newValue );

	if (%oldValue != %newValue || %oldValue !$= %newValue)
		%this.setDirty(%object);
}

function DecalInspector::setDirty( %this, %object ) {
	DecalPMan.setDirty( %object );

	if ( strchr(LibraryTabControl.text, "*") $= ""  )
		LibraryTabControl.text = LibraryTabControl.text @ "*";
   
   show(DecalEd_SaveDecalsButton);
}

function DecalInspector::removeDirty() {
	if ( strchr(LibraryTabControl.text, "*") !$= ""  )
		LibraryTabControl.text = stripChars(LibraryTabControl.text, "*");
}