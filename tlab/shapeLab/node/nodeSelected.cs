//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
function ShapeLab::setActiveNode( %this, %name ) {
	if (%name $= "")
	 {
	 	
		// Disable delete button and edit boxes
		if ( ShapeLab.currentMainOptionsPage $= "0" )
			ShapeLabPropWindow-->deleteBtn.setActive( false );

		ShapeLabNodes-->nodeName.setActive( false );
		ShapeLabNodes-->nodePosition.setActive( false );
		ShapeLabNodes-->nodeRotation.setActive( false );
		ShapeLabNodes-->nodeName.setText( "" );
		ShapeLabNodes-->nodePosition.setText( "" );
		ShapeLabNodes-->nodeRotation.setText( "" );
		ShapeLabShapeView.selectedNode = -1;
		return;
		
	}
	// Enable delete button and edit boxes
		if ( ShapeLab.currentMainOptionsPage $= "0" )
			ShapeLabPropWindow-->deleteBtn.setActive( true );

		ShapeLabNodes-->nodeName.setActive( true );
		ShapeLabNodes-->nodePosition.setActive( true );
		ShapeLabNodes-->nodeRotation.setActive( true );
		
		ShapeLabNodes-->nodeName.setText( %name );
		//Build parent node menu
		ShapeLabNodeParentMenu.build(%name);


		if ( ShapeLabNodes.isWorldTransform ) {
			// Global transform
			%txfm = ShapeLab.shape.getNodeTransform( %name, 1 );
			ShapeLabNodes-->nodePosition.setText( getWords( %txfm, 0, 2 ) );
			ShapeLabNodes-->nodeRotation.setText( getWords( %txfm, 3, 6 ) );
		} else {
			// Local transform (relative to parent)
			%txfm = ShapeLab.shape.getNodeTransform( %name, 0 );
			ShapeLabNodes-->nodePosition.setText( getWords( %txfm, 0, 2 ) );
			ShapeLabNodes-->nodeRotation.setText( getWords( %txfm, 3, 6 ) );
		}

		ShapeLabShapeView.selectedNode = ShapeLab.shape.getNodeIndex( %name );
}
//------------------------------------------------------------------------------
//==============================================================================
function ShapeLab::selectTreeNode( %this, %index ) {
	ShapeLab_NodeTree.clearSelection();

	if ( %index > 0 ) {
		%name = ShapeLab.shape.getNodeName( %index );
		%id = ShapeLab_NodeTree.findItemByName( %name );

		if ( %id > 0 )
			ShapeLab_NodeTree.selectItem( %id );
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function ShapeLabNodeParentMenu::build( %this, %name ) {
	ShapeLabNodeParentMenu.clear();
	// Node parent list => ancestor and sibling nodes only (can't re-parent to a descendent)
	%parentNames = ShapeLab.getNodeNames( "", "<root>", %name );
	%count = getWordCount( %parentNames );
	for ( %i = 0; %i < %count; %i++ )
			ShapeLabNodeParentMenu.add( getWord(%parentNames, %i), %i );
	
	%pName = ShapeLab.shape.getNodeParentName( %name );

	if ( %pName $= "" )
		%pName = "<root>";

	ShapeLabNodeParentMenu.setText( %pName );
}
//------------------------------------------------------------------------------
//==============================================================================
// Selected Node Info Scripts
//==============================================================================

//==============================================================================
function ShapeLabNodeParentMenu::onSelect( %this, %id, %text ) {
	%id = ShapeLab_NodeTree.getSelectedItem();

	if ( %id > 0 ) {
		%name = ShapeLab_NodeTree.getItemText( %id );
		ShapeLab.doSetNodeParent( %name, %text );
	}
}
//------------------------------------------------------------------------------


