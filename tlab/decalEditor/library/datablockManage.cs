//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

function RetargetDecalButton::onClick( %this ) {
	%id = DecalDataList.getSelectedItem();
	%datablock = DecalDataList.getItemText(%id );

	if( !isObject(%datablock) ) {
		LabMsgOK("Error", "A valid Decal Template must be selected.");
		return;
	}

	// This is the first place IODropdown is used. The # in the function passed replaced with the output
	// of the preset menu.
	IODropdown("Retarget Decal Instances",
				  "Retarget DecalInstances from " @ %datablock.getName() @ " over to....",
				  "decalDataSet",
				  "DecalEditorGui.retargetDecalDatablock(" @ %datablock.getName() @ ", #);",
				  "");
	DecalEditorGui.rebuildInstanceTree();
}

function NewDecalButton::onClick( %this ) {
	%name = getUniqueName( "NewDecalData" );
	%str = "datablock DecalData( " @ %name @ " ) { Material = \"WarningMaterial\"; };";
	eval( %str );
	DecalPMan.setDirty( %name, $decalDataFile );

	if ( strchr(LibraryTabControl.text, "*") $= ""  )
		LibraryTabControl.text = LibraryTabControl.text @ "*";

	DecalDataList.doMirror();
	%id = DecalDataList.findItemText( %name );
	DecalDataList.setSelected( %id, true );
	Canvas.pushDialog( DecalEditDlg );
	DecalInspector.inspect( %name );
}

function DeleteDecalButton::onClick( %this ) {
	if( DecalEditorTabBook.getSelectedPage() == 0 ) { // library
		%id = DecalDataList.getSelectedItem();
		%datablock = DecalDataList.getItemText(%id );
		LabMsgYesNoCancel("Delete Decal Datablock?",
								"Are you sure you want to delete<br><br>" @ %datablock @ "<br><br> Datablock deletion won't take affect until the engine is quit.",
								"DecalEditorGui.deleteSelectedDecalDatablock();",
								"",
								"" );
	} else { // instances
		DecalEditorGui.deleteSelectedDecal();
	}
}

// Intended for gui use. The undo/redo functionality for deletion of datablocks
// will enable itself automatically after using this function.
function DecalEditorGui::deleteSelectedDecalDatablock() {
	%id = DecalDataList.getSelectedItem();
	%datablock = DecalDataList.getItemText(%id );
	DecalEditorGui.deleteDecalDatablock( %datablock );

	if( %datablock.getFilename() !$= "" ) {
		DecalPMan.removeDirty( %datablock );
		DecalPMan.removeObjectFromFile( %datablock );
	}

	DecalDataList.addFilteredItem( %datablock );
}