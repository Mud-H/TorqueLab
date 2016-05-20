//==============================================================================
// TorqueLab -> ShapeLab -> Mounted Shapes
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
function ShapeLabDetails::onShapeSelectionChanged( %this ) {
	ShapeLabDetails.selectShape();
}
//==============================================================================

//==============================================================================
// ShapeLab -> Details Actions Callbacks
//==============================================================================

//==============================================================================
function ShapeLab::onDetailRenamed( %this, %oldName, %newName ) {
	// --- DETAILS TAB ---
	// Rename detail entry
	%id = ShapeLab_DetailTree.findItemByName( %oldName );

	if ( %id > 0 ) {
		%size = ShapeLab_DetailTree.getItemValue( %id );
		ShapeLab_DetailTree.editItem( %id, %newName, %size );

		// Sync text if item is selected
		if ( ShapeLab_DetailTree.isItemSelected( %id ) &&
				( ShapeLabDetails-->meshName.getText() !$= %newName ) )
			ShapeLabDetails-->meshName.setText( stripTrailingNumber( %newName ) );
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function ShapeLab::onDetailSizeChanged( %this, %oldSize, %newSize ) {
	// --- MISC ---
	ShapeLabShapeView.refreshShape();
	%dl = ShapeLab.shape.getDetailLevelIndex( %newSize );
	devLog("ShapeLab::onDetailSizeChanged( %this, %oldSize, %newSize )",%oldSize, %newSize,"DL",%dl);

	if ( ShapeLabAdv_Details-->detailSize.getText() $= %oldSize ) {
		ShapeLabShapeView.currentDL = %dl;
		ShapeLabAdv_Details-->detailSize.setText( %newSize );
		ShapeLabDetails-->meshSize.setText( %newSize );
	}

	// --- DETAILS TAB ---
	// Update detail entry then resort details by size
	%id = ShapeLab_DetailTree.findItemByValue( %oldSize );
	%detName = ShapeLab.shape.getDetailLevelName( %dl );
	ShapeLab_DetailTree.editItem( %id, %detName, %newSize );

	for ( %sibling = ShapeLab_DetailTree.getPrevSibling( %id );
			( %sibling > 0 ) && ( ShapeLab_DetailTree.getItemValue( %sibling ) < %newSize );
			%sibling = ShapeLab_DetailTree.getPrevSibling( %id ) )
		ShapeLab_DetailTree.moveItemUp( %id );

	for ( %sibling = ShapeLab_DetailTree.getNextSibling( %id );
			( %sibling > 0 ) && ( ShapeLab_DetailTree.getItemValue( %sibling ) > %newSize );
			%sibling = ShapeLab_DetailTree.getNextSibling( %id ) )
		ShapeLab_DetailTree.moveItemDown( %id );

	// Update size values for meshes of this detail
	for ( %child = ShapeLab_DetailTree.getChild( %id );
			%child > 0;
			%child = ShapeLab_DetailTree.getNextSibling( %child ) ) {
		%meshName = stripTrailingNumber( ShapeLab_DetailTree.getItemText( %child ) );
		ShapeLab_DetailTree.editItem( %child, %meshName SPC %newSize, "" );
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function ShapeLab::onObjectRenamed( %this, %oldName, %newName ) {
	// --- DETAILS TAB ---
	// Rename tree entries for this object
	%count = ShapeLab.shape.getMeshCount( %newName );

	for ( %i = 0; %i < %count; %i++ ) {
		%size = getTrailingNumber( ShapeLab.shape.getMeshName( %newName, %i ) );
		%id = ShapeLab_DetailTree.findItemByName( %oldName SPC %size );

		if ( %id > 0 ) {
			ShapeLab_DetailTree.editItem( %id, %newName SPC %size, "" );

			// Sync text if item is selected
			if ( ShapeLab_DetailTree.isItemSelected( %id ) &&
					( ShapeLabDetails-->meshName.getText() !$= %newName ) )
				ShapeLabDetails-->meshName.setText( %newName );
		}
	}
}

//==============================================================================
function ShapeLab::onObjectNodeChanged( %this, %objName ) {
	// --- MISC ---
	ShapeLabShapeView.refreshShape();

	// --- DETAILS TAB ---
	// Update the node popup menu if this object is selected
	if ( ShapeLabDetails-->meshName.getText() $= %objName ) {
		%nodeName = ShapeLab.shape.getObjectNode( %objName );

		if ( %nodeName $= "" )
			%nodeName = "<root>";

		%id = ShapeLabDetails-->objectNode.findText( %nodeName );
		ShapeLabDetails-->objectNode.setSelected( %id, false );
	}
}
//------------------------------------------------------------------------------
function ShapeLab::onDetailRemoved( %this, %detailName ) {
	// --- MISC ---
	ShapeLabShapeView.refreshShape();
	// --- COLLISION WINDOW ---
	// Remove object from target list if it no longer exists
	%objName = stripTrailingNumber( %detailName );

	if ( ShapeLab.shape.getObjectIndex( %objName ) == -1 ) {
		%id = ShapeLab_CreateColRollout-->colTarget.findText( %objName );

		if ( %id != -1 )
			ShapeLab_CreateColRollout-->colTarget.clearEntry( %id );
	}

	// --- DETAILS TAB ---
	// Determine which item to select next
	%id = ShapeLab_DetailTree.findItemByName( %detailName );

	if ( %id > 0 ) {
		%nextId = ShapeLab_DetailTree.getPrevSibling( %id );

		if ( %nextId <= 0 ) {
			%nextId = ShapeLab_DetailTree.getNextSibling( %id );

			if ( %nextId <= 0 )
				%nextId = 2;
		}

		// Remove the entry from the tree
		%detailSize = getTrailingNumber( %detailName );
		ShapeLab_DetailTree.removeMeshEntry( %detailName, %detailSize );

		// Change selection if needed
		if ( ShapeLab_DetailTree.getSelectedItem() == -1 )
			ShapeLab_DetailTree.selectItem( %nextId );
	}
}

//==============================================================================
// MESHES ACTION CALLBACKS
//==============================================================================
function ShapeLab::onMeshSizeChanged( %this, %meshName, %oldSize, %newSize ) {
	// --- MISC ---
	ShapeLabShapeView.refreshShape();
	// --- DETAILS TAB ---
	// Move the mesh to the new location in the tree
	%selected = ShapeLab_DetailTree.getSelectedItem();
	%id = ShapeLab_DetailTree.findItemByName( %meshName SPC %oldSize );
	ShapeLab_DetailTree.removeMeshEntry( %meshName SPC %oldSize );
	%newId = ShapeLab_DetailTree.addMeshEntry( %meshName SPC %newSize );

	// Re-select the new entry if it was selected
	if ( %selected == %id ) {
		ShapeLab_DetailTree.clearSelection();
		ShapeLab_DetailTree.selectItem( %newId );
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function ShapeLab::onMeshRemoved( %this, %meshName ) {
	// --- MISC ---
	ShapeLabShapeView.refreshShape();
	// --- COLLISION WINDOW ---
	// Remove object from target list if it no longer exists
	%objName = stripTrailingNumber( %meshName );

	if ( ShapeLab.shape.getObjectIndex( %objName ) == -1 ) {
		%id = ShapeLab_CreateColRollout-->colTarget.findText( %objName );

		if ( %id != -1 )
			ShapeLab_CreateColRollout-->colTarget.clearEntry( %id );
	}

	// --- DETAILS TAB ---
	// Determine which item to select next
	%id = ShapeLab_DetailTree.findItemByName( %meshName );
	if ( %id > 0 ) {
		%nextId = ShapeLab_DetailTree.getPrevSibling( %id );
		if ( %nextId <= 0 ) {
			%nextId = ShapeLab_DetailTree.getNextSibling( %id );
			if ( %nextId <= 0 )
				%nextId = 2;
		}

		// Remove the entry from the tree
		%meshSize = getTrailingNumber( %meshName );
		ShapeLab_DetailTree.removeMeshItem( %id, %meshName, %meshSize );

		// Change selection if needed
		if ( ShapeLab_DetailTree.getSelectedItem() == -1 )
			ShapeLab_DetailTree.selectItem( %nextId );
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function ShapeLab::onMeshAdded( %this, %meshName ) {
	// --- MISC ---
	ShapeLabShapeView.refreshShape();
	ShapeLabShapeView.updateNodeTransforms();

	// --- COLLISION WINDOW ---
	// Add object to target list if it does not already exist
	if ( !ShapeLab.isCollisionMesh( %meshName ) ) {
		%objName = stripTrailingNumber( %meshName );
		%id = ShapeLab_CreateColRollout-->colTarget.findText( %objName );

		if ( %id == -1 )
			ShapeLab_CreateColRollout-->colTarget.add( %objName );
	}

	// --- DETAILS TAB ---
	%id = ShapeLab_DetailTree.addMeshEntry( %meshName );
	ShapeLab_DetailTree.clearSelection();
	ShapeLab_DetailTree.selectItem( %id );
}
//==============================================================================
// IMPOSTER ACTION CALLBACKS
//==============================================================================

function ShapeLab::onImposterAdded( %this, %meshName ) {
	show(ShapeLab_ImposterWait);
	// Need to de-highlight the current material, or the imposter will have the
	// highlight effect baked in!
	ShapeLabMaterials.updateSelectedMaterial( false );
	%dl = ShapeLab.shape.addImposter( %this.newSize, %val[1], %val[2], %val[3], %val[4], %val[5], %val[6] );
	
	hide(ShapeLab_ImposterWait);
	// Restore highlight effect
	ShapeLabMaterials.updateSelectedMaterial( ShapeLabMaterials-->highlightMaterial.getValue() );
	devLog("ActionEditImposter::DoIt: dl =",%dl);

	if ( %dl != -1 ) {
		ShapeLabShapeView.refreshShape();
		ShapeLabShapeView.currentDL = %dl;
		ShapeLabAdv_Details-->detailSize.setText( %this.newSize );
		ShapeLabDetails-->meshSize.setText( %this.newSize );
		ShapeLab.updateDetail();
		return true;
	}

	return false;
}

function ShapeLab::onImposterRemoved( %this ) {
	ShapeLabShapeView.refreshShape();
	ShapeLabShapeView.currentDL = 0;
	ShapeLab.updateDetail();
}