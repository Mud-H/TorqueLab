//==============================================================================
// Boost! -> GuiControl Functions Helpers
// Copyright NordikLab Studio, 2013
//==============================================================================
//==============================================================================
// Schedule global on-off - Used to limit output of fast logs
//==============================================================================

function ForestEditMeshTree::initTree( %this ) {
	ForestEditMeshTree.open( ForestItemDataSet );
	ForestManagerItemTree.open( ForestItemDataSet );
}

function ForestEditorGui::setItemFilter( %this,%text ) {
	%filtertext = strreplace(%text,"Filter...","");
	ForestEditMeshTree.setFilterText(%filtertext);
	FEP_ItemFilter.setText(%text);
}

function ForestEditorGui::newMeshGroup( %this ) {
	%internalName = getUniqueInternalName( "Group", ForestBrushGroup, true );
	%brush = new SimSet() {
		internalName = %internalName;
		parentGroup = ForestMeshGroup;
	};
	ForestItemDataSet.add(%brush);
	MECreateUndoAction::submit( %brush );
	ForestEditMeshTree.open( ForestItemDataSet );
	ForestEditMeshTree.buildVisibleTree(true);
	%item = ForestEditMeshTree.findItemByObjectId( %brush );
	ForestEditMeshTree.clearSelection();
	ForestEditMeshTree.addSelection( %item );
	ForestEditMeshTree.scrollVisible( %item );
	ForestEditorPlugin.dirty = true;
}
function ForestEditorGui::newMesh( %this ) {
	%spec = "All Mesh Files|*.dts;*.dae|DTS|*.dts|DAE|*.dae";
	%dlg = new OpenFileDialog() {
		Filters        = %spec;
		DefaultPath    = $Pref::WorldEditor::LastPath;
		DefaultFile    = "";
		ChangePath     = true;
	};
	%ret = %dlg.Execute();

	if ( %ret ) {
		$Pref::WorldEditor::LastPath = filePath( %dlg.FileName );
		%fullPath = makeRelativePath( %dlg.FileName, getMainDotCSDir() );
		%file = fileBase( %fullPath );
	}

	%dlg.delete();

	if ( !%ret )
		return;

	%name = getUniqueName( %file );
	%str = "datablock TSForestItemData( " @ %name @ " ) { shapeFile = \"" @ %fullPath @ "\"; };";
	eval( %str );

	if ( isObject( %name ) ) {
		ForestEditMeshTree.clearSelection();
		ForestEditMeshTree.buildVisibleTree( true );
		%item = ForestEditMeshTree.findItemByObjectId( %name.getId() );
		ForestEditMeshTree.scrollVisible( %item );
		ForestEditMeshTree.addSelection( %item );
		ForestDataManager.setDirty( %name, "art/forest/managedItemData.cs" );
		%element = new ForestBrushElement() {
			internalName = %name;
			forestItemData = %name;
			parentGroup = ForestBrushGroup;
		};
		ForestEditBrushTree.clearSelection();
		ForestEditBrushTree.buildVisibleTree( true );
		%item = ForestEditBrushTree.findItemByObjectId( %element.getId() );
		ForestEditBrushTree.scrollVisible( %item );
		ForestEditBrushTree.addSelection( %item );
		pushInstantGroup();
		%action = new MECreateUndoAction() {
			actionName = "Create TSForestItemData";
		};
		popInstantGroup();
		%action.addObject( %name );
		%action.addObject( %element );
		%action.addToManager( Editor.getUndoManager() );
		ForestEditorPlugin.dirty = true;
	}
}

function ForestEditorGui::deleteMesh( %this ) {
	%obj = ForestEditMeshTree.getSelectedObject();

	// Can't delete itemData's that are in use without
	// crashing at the moment...

	if ( isObject( %obj ) ) {
		LabMsgOkCancel( "Warning",
							 "Deleting this mesh will also delete BrushesElements and ForestItems referencing it.",
							 "ForestEditorGui.okDeleteMesh(" @ %obj @ ");",
							 "" );
	}
}

function ForestEditorGui::okDeleteMesh( %this, %mesh ) {
	// Remove mesh from file
	ForestDataManager.removeObjectFromFile( %mesh, "art/forest/managedItemData.cs" );
	// Submitting undo actions is handled in code.
	%this.deleteMeshSafe( %mesh );
	// Update TreeViews.
	ForestEditBrushTree.buildVisibleTree( true );
	ForestEditMeshTree.buildVisibleTree( true );
	ForestEditorPlugin.dirty = true;
}


function ForestEditMeshTree::onDragDropped( %this ) {
	ForestEditorPlugin.dirty = true;
}

function ForestEditMeshTree::onDeleteObject( %this, %obj ) {
	// return true - skip delete.
	return true;
}

function ForestEditMeshTree::onDoubleClick( %this ) {
	%obj = %this.getSelectedObject();
	%name = getUniqueInternalName( %obj.getName(), ForestBrushGroup, true );
	%element = new ForestBrushElement() {
		internalName = %name;
		forestItemData = %obj.getName();
		parentGroup = ForestBrushGroup;
	};
	//ForestDataManager.setDirty( %element, "art/forest/brushes.cs" );
	ForestEditBrushTree.clearSelection();
	ForestEditBrushTree.buildVisibleTree( true );
	%item = ForestEditBrushTree.findItemByObjectId( %element );
	ForestEditBrushTree.scrollVisible( %item );
	ForestEditBrushTree.addSelection( %item );
	ForestEditorPlugin.dirty = true;
}






