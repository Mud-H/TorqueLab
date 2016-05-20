//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// DetailTree Init
//==============================================================================
//==============================================================================
function ShapeLab_DetailTree::onDefineIcons(%this) {
	// Set the tree view icon indices and texture paths
	%this._imageNone = 0;
	%this._imageHidden = 1;
	%icons = ":" @                                        // no icon
				"tlab/art/buttons/default/visible_i:";               // hidden
	%this.buildIconTable( %icons );
}
//------------------------------------------------------------------------------
//==============================================================================
// DetailTree Functions
//==============================================================================


//==============================================================================
function ShapeLab_DetailTree::onSelect( %this, %id ) {
	
	%name = %this.getItemText( %id );
	%value = %this.getItemValue( %id );
	%baseName = stripTrailingNumber( %name );
	%size = getTrailingNumber( %name );
	ShapeLabDetails-->meshName.setText( %baseName );
	ShapeLabDetails-->meshSize.setText( %size );
	// Select the appropriate detail level
	%dl = %this.getDetailLevelFromItem( %id );
	ShapeLabShapeView.currentDL = %dl;
devLog("ShapeLab_DetailTree onSelect ID:",%id,"Text",%name,"Value",%value);

	//Check if it's a detail node, Mesh can't have childs
	if ( ShapeLab_DetailTree.isParentItem( %id ) ) {
		// Selected a detail => disable mesh controls
		ShapeLabDetails-->editMeshActive.setVisible( false );
		ShapeLabShapeView.selectedObject = -1;
		ShapeLabShapeView.selectedObjDetail = 0;
	} else {
		// Selected a mesh => sync mesh controls
		ShapeLabDetails-->editMeshActive.setVisible( true );

		switch$ ( ShapeLab.shape.getMeshType( %name ) ) {
		case "normal":
			ShapeLabDetails-->bbType.setSelected( 0, false );

		case "billboard":
			ShapeLabDetails-->bbType.setSelected( 1, false );

		case "billboardzaxis":
			ShapeLabDetails-->bbType.setSelected( 2, false );
		}

		%node = ShapeLab.shape.getObjectNode( %baseName );

		if ( %node $= "" )
			%node = "<root>";

		ShapeLabDetails-->objectNode.setSelected( ShapeLabDetails-->objectNode.findText( %node ), false );
		ShapeLabShapeView.selectedObject = ShapeLab.shape.getObjectIndex( %baseName );
		ShapeLabShapeView.selectedObjDetail = %dl;
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function ShapeLab_DetailTree::onRightMouseUp( %this, %itemId, %mouse ) {
	// Open context menu if this is a Mesh item
	if ( !%this.isParentItem( %itemId ) ) {
		if( !isObject( "ShapeLabMeshPopup" ) ) {
			new PopupMenu( ShapeLabMeshPopup ) {
				superClass = "MenuBuilder";
				isPopup = "1";
				item[ 0 ] = "Hidden" TAB "" TAB "ShapeLab_DetailTree.onHideMeshItem( %this._objName, !%this._itemHidden );";
				item[ 1 ] = "-";
				item[ 2 ] = "Hide all" TAB "" TAB "ShapeLab_DetailTree.onHideMeshItem( \"\", true );";
				item[ 3 ] = "Show all" TAB "" TAB "ShapeLab_DetailTree.onHideMeshItem( \"\", false );";
			};
		}

		ShapeLabMeshPopup._objName = stripTrailingNumber( %this.getItemText( %itemId ) );
		ShapeLabMeshPopup._itemHidden = ShapeLabShapeView.getMeshHidden( ShapeLabMeshPopup._objName );
		ShapeLabMeshPopup.checkItem( 0, ShapeLabMeshPopup._itemHidden );
		ShapeLabMeshPopup.showPopup( Canvas );
	}
}
//------------------------------------------------------------------------------
//==============================================================================

function ShapeLab_DetailTree::addMeshEntry( %this, %name, %noSync ) {
	// Add new detail level if required
	//Get the Detail Size by getting the ending numbers
	%size = getTrailingNumber( %name );
	
	//Find a detail item for this size (detailSIZE)
	%detailID = %this.findItemByValue( %size );

	//If no detail item foound create one
	if ( %detailID <= 0 ) {
		%detailID = %this.addDetailEntry(%size,%noSync);			
	}
	
	return %this.insertItem( %detailID, %name, "M_"@%size, "" );
}
//------------------------------------------------------------------------------
//==============================================================================

function ShapeLab_DetailTree::addDetailEntry( %this, %size, %noSync ) {
		%dl = ShapeLab.shape.getDetailLevelIndex( %size );
		%detName = ShapeLab.shape.getDetailLevelName( %dl );
		%detailID = ShapeLab_DetailTree.insertItem( 1, %detName, %size, "" );
		
		%this.reorderDetails(%detailID,%size);		
		
		if ( !%noSync )
			ShapeLab.updateDetail();
	return %detailID;
}
//------------------------------------------------------------------------------
//==============================================================================

function ShapeLab_DetailTree::reorderDetails( %this, %detailID, %size ) {
	// Sort details by decreasing size
		for ( 	%sibling = ShapeLab_DetailTree.getPrevSibling( %detailID );	
					( %sibling > 0 ) && ( ShapeLab_DetailTree.getItemValue( %sibling ) < %size );	
					%sibling = ShapeLab_DetailTree.getPrevSibling( %detailID ) )
			ShapeLab_DetailTree.moveItemUp( %detailID );

}
//------------------------------------------------------------------------------
//==============================================================================
function ShapeLab_DetailTree::removeMeshItem( %this, %id, %name, %size ) {
	//%size = getTrailingNumber( %name );
	//Check if collision mesh
	
	if (%id >= 0)
		%this.removeItem( %id );
	
	ShapeLabDetails.selectedShapeChanged();
	
}
//------------------------------------------------------------------------------

//==============================================================================
function ShapeLab_DetailTree::removeMeshEntry( %this, %name, %size ) {
	//%size = getTrailingNumber( %name );
	//Check if collision mesh
	if (%size < 0)
		%this.removeItem( %id );
		
	%id = ShapeLab_DetailTree.findItemByName( %name );
	if ( ShapeLab.shape.getDetailLevelIndex( %size ) < 0 ) {
		// Last mesh of a detail level has been removed => remove the detail level
		%this.removeItem( %this.getParent( %id ) );
		ShapeLab.updateDetail();
	} else
		%this.removeItem( %id );
}
//------------------------------------------------------------------------------
//==============================================================================
function ShapeLab_DetailTree::onHideMeshItem( %this, %objName, %hide ) {
	if ( %hide )
		%imageId = %this._imageHidden;
	else
		%imageId = %this._imageNone;

	if ( %objName $= "" ) {
		// Show/hide all
		ShapeLabShapeView.setAllMeshesHidden( %hide );

		for ( %parent = %this.getChild(%this.getFirstRootItem()); %parent > 0; %parent = %this.getNextSibling(%parent) )
			for ( %child = %this.getChild(%parent); %child > 0; %child = %this.getNextSibling(%child) )
				%this.setItemImages( %child, %imageId, %imageId );
	} else {
		// Show/hide all meshes for this object
		ShapeLabShapeView.setMeshHidden( %objName, %hide );
		%count = ShapeLab.shape.getMeshCount( %objName );

		for ( %i = 0; %i < %count; %i++ ) {
			%meshName = ShapeLab.shape.getMeshName( %objName, %i );
			%id = ShapeLab_DetailTree.findItemByName( %meshName );

			if ( %id > 0 )
				%this.setItemImages( %id, %imageId, %imageId );
		}
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function ShapeLab_DetailTree::onDeleteObject( %this, %obj ) {
	devLog("ShapeLab_DetailTree::onDeleteObject OBJ:",%obj);
}
function ShapeLab_DetailTree::onDeleteSelection( %this ) {
	devLog("ShapeLab_DetailTree::onDeleteSelection");
}
//==============================================================================
// DetailTree Helpers
//==============================================================================


//==============================================================================
// Get the detail level index from the ID of an item in the details tree view
function ShapeLab_DetailTree::getDetailLevelFromItem( %this, %id ) {
	devLog("ShapeLab_DetailTree::getDetailLevelFromItem ID:",%id,"IsDetaik",%this.isParentItem( %id ));

	//Get detailSize (Mesh have a M_ prefix before detailSize so replace it with ""
	%detSize = strreplace(ShapeLab_DetailTree.getItemValue( %id ),"M_","");

	devLog("ShapeLab_DetailTree::getDetailLevelFromItem Detail:",%detSize);
	return ShapeLab.shape.getDetailLevelIndex( %detSize );
}
//------------------------------------------------------------------------------
