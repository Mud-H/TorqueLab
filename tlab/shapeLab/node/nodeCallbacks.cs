//==============================================================================
// TorqueLab -> ShapeLab -> Mounted Shapes
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// ShapeLab -> Node Editing
//==============================================================================

//==============================================================================
// Update the GUI in response to a node being added
function ShapeLab::onNodeAdded( %this, %nodeName, %oldTreeIndex ) {
	// --- MISC ---
	ShapeLabShapeView.refreshShape();
	ShapeLabShapeView.updateNodeTransforms();
	ShapeLabHints.updateHints();

	// --- MOUNT WINDOW ---
	if ( ShapeLabMountWindow.isMountableNode( %nodeName ) ) {
		ShapeLabMountWindow-->mountNode.add( %nodeName );
		ShapeLabMountWindow-->mountNode.sort();
	}

	// --- NODES TAB ---
	%id = ShapeLab_NodeTree.addNodeTree( %nodeName );

	if ( %oldTreeIndex <= 0 ) {
		// This is a new node => make it the current selection
		if ( %id > 0 ) {
			ShapeLab_NodeTree.clearSelection();
			ShapeLab_NodeTree.selectItem( %id );
		}
	} else {
		// This node has been un-deleted. Inserting a new item puts it at the
		// end of the siblings, but we want to restore the original order as
		// if the item was never deleted, so move it up as required.
		%childIndex = ShapeLab_NodeTree.getChildIndexByName( %nodeName );

		while ( %childIndex > %oldTreeIndex ) {
			ShapeLab_NodeTree.moveItemUp( %id );
			%childIndex--;
		}
	}

	// --- DETAILS TAB ---
	ShapeLabDetails-->objectNode.add( %nodeName );
}
//------------------------------------------------------------------------------
//==============================================================================
// Update the GUI in response to a node(s) being removed
function ShapeLab::onNodeRemoved( %this, %nameList, %nameCount ) {
	// --- MISC ---
	ShapeLabShapeView.refreshShape();
	ShapeLabShapeView.updateNodeTransforms();
	ShapeLabHints.updateHints();

	// Remove nodes from the mountable list, and any shapes mounted to the node
	for ( %i = 0; %i < %nameCount; %i++ ) {
		%nodeName = getField( %nameList, %i );
		ShapeLabMountWindow-->mountNode.clearEntry( ShapeLabMountWindow-->mountNode.findText( %nodeName ) );

		for ( %j = ShapeLabMountWindow-->mountList.rowCount()-1; %j >= 1; %j-- ) {
			%text = ShapeLabMountWindow-->mountList.getRowText( %j );

			if ( getField( %text, 1 ) $= %nodeName ) {
				ShapeLabShapeView.unmountShape( %j-1 );
				ShapeLabMountWindow-->mountList.removeRow( %j );
			}
		}
	}

	// --- NODES TAB ---
	%lastName = getField( %nameList, %nameCount-1 );
	%id = ShapeLab_NodeTree.findItemByName( %lastName );   // only need to remove the parent item

	if ( %id > 0 ) {
		ShapeLab_NodeTree.removeItem( %id );

		if ( ShapeLab_NodeTree.getSelectedItem() <= 0 )
			ShapeLab.setActiveNode("");

		//ShapeLab.onNodeSelectionChanged( -1 );
	}

	// --- DETAILS TAB ---
	for ( %i = 0; %i < %nameCount; %i++ ) {
		%nodeName = getField( %nameList, %i );
		ShapeLabDetails-->objectNode.clearEntry( ShapeLabDetails-->objectNode.findText( %nodeName ) );
	}
}
//------------------------------------------------------------------------------
//==============================================================================
// Update the GUI in response to a node being renamed
function ShapeLab::onNodeRenamed( %this, %oldName, %newName ) {
	// --- MISC ---
	ShapeLabHints.updateHints();
	// --- MOUNT WINDOW ---
	// Update entries for any shapes mounted to this node
	%rowCount = ShapeLabMountWindow-->mountList.rowCount();

	for ( %i = 1; %i < %rowCount; %i++ ) {
		%text = ShapeLabMountWindow-->mountList.getRowText( %i );

		if ( getField( %text, 1 ) $= %oldName ) {
			%text = setField( %text, 1, %newName );
			ShapeLabMountWindow-->mountList.setRowById( ShapeLabMountWindow-->mountList.getRowId( %i ), %text );
		}
	}

	// Update list of mountable nodes
	ShapeLabMountWindow-->mountNode.clearEntry( ShapeLabMountWindow-->mountNode.findText( %oldName ) );

	if ( ShapeLabMountWindow.isMountableNode( %newName ) ) {
		ShapeLabMountWindow-->mountNode.add( %newName );
		ShapeLabMountWindow-->mountNode.sort();
	}

	// --- NODES TAB ---
	%id = ShapeLab_NodeTree.findItemByName( %oldName );
	ShapeLab_NodeTree.editItem( %id, %newName, 0 );

	if ( ShapeLab_NodeTree.getSelectedItem() == %id )
		ShapeLabNodes-->nodeName.setText( %newName );

	// --- DETAILS TAB ---
	%id = ShapeLabDetails-->objectNode.findText( %oldName );

	if ( %id != -1 ) {
		ShapeLabDetails-->objectNode.clearEntry( %id );
		ShapeLabDetails-->objectNode.add( %newName, %id );
		ShapeLabDetails-->objectNode.sortID();

		if ( ShapeLabDetails-->objectNode.getText() $= %oldName )
			ShapeLabDetails-->objectNode.setText( %newName );
	}
}
//------------------------------------------------------------------------------
//==============================================================================
// Update the GUI in response to a node's parent being changed
function ShapeLab::onNodeParentChanged( %this, %nodeName ) {
	// --- MISC ---
	ShapeLabShapeView.updateNodeTransforms();
	// --- NODES TAB ---
	ShapeLabNodeParentMenu.build(%nodeName);
	%isSelected = 0;
	%id = ShapeLab_NodeTree.findItemByName( %nodeName );

	if ( %id > 0 ) {
		%isSelected = ( ShapeLab_NodeTree.getSelectedItem() == %id );
		ShapeLab_NodeTree.removeItem( %id );
	}

	ShapeLab_NodeTree.addNodeTree( %nodeName );

	if ( %isSelected )
		ShapeLab_NodeTree.selectItem( ShapeLab_NodeTree.findItemByName( %nodeName ) );
}
//------------------------------------------------------------------------------
//==============================================================================
function ShapeLab::onNodeTransformChanged( %this, %nodeName ) {
	// Default to the selected node if none is specified
	if ( %nodeName $= "" ) {
		%id = ShapeLab_NodeTree.getSelectedItem();

		if ( %id > 0 )
			%nodeName = ShapeLab_NodeTree.getItemText( %id );
		else
			return;
	}

	// --- MISC ---
	ShapeLabShapeView.updateNodeTransforms();

	%isWorld = ShapeLabNodes.isWorldTransform;
	if ( %isWorld )
		GlobalGizmoProfile.setFieldValue(alignment, World);
	else
		GlobalGizmoProfile.setFieldValue(alignment, Object);

	// --- NODES TAB ---
	// Update the node transform fields if necessary
	%id = ShapeLab_NodeTree.getSelectedItem();

	if ( ( %id > 0 ) && ( ShapeLab_NodeTree.getItemText( %id ) $= %nodeName ) ) {
		//ShapeLabNodes.isWorldTransform = ShapeLabNodes-->world.getValue();
		%transform = ShapeLab.shape.getNodeTransform( %nodeName, %isWorld );
		ShapeLabNodes-->nodePosition.setText( getWords( %transform, 0, 2 ) );
		ShapeLabNodes-->nodeRotation.setText( getWords( %transform, 3, 6 ) );
	}
}
//------------------------------------------------------------------------------
