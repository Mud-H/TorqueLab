//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// Scene Editor Params - Used set default settings and build plugins options GUI
//==============================================================================
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function ShapeLab::clearNodeTree( %this ) {
	ShapeLab_NodeTree.removeItem( 0 );
}
//------------------------------------------------------------------------------

//==============================================================================
// Plugin Object Callbacks - Called from TLab plugin management scripts
//==============================================================================
//==============================================================================
function ShapeLab_NodeTree::onClearSelection( %this ) {
	
	ShapeLab.setActiveNode("");	
	//ShapeLab.onNodeSelectionChanged( -1 );
}
//------------------------------------------------------------------------------
//==============================================================================
function ShapeLab_NodeTree::onSelect( %this, %id ) {
	// Update the node name and transform controls
	if ( %id > 0 )		
		%name = ShapeLab_NodeTree.getItemText( %id );
	else
		%name = "";
		
	ShapeLab.setActiveNode(%name);		
	

	// Update orbit position if orbiting the selected node
	if ( ShapeLabShapeView.orbitNode ) {
		%name = %this.getItemText( %id );
		%transform = ShapeLab.shape.getNodeTransform( %name, 1 );
		ShapeLabShapeView.setOrbitPos( getWords( %transform, 0, 2 ) );
	}
}
//------------------------------------------------------------------------------
//==============================================================================
// Determine the index of a node in the tree relative to its parent
function ShapeLab_NodeTree::getChildIndexByName( %this, %name ) {
	%parentName = ShapeLab.shape.getNodeParentName(%name);
	%parentId = %this.findItemByName( %parentName );	
	if ( %childId <= 0 )
		return 0;   // bad!

	%childId = %this.getChild( %parentId );

	if ( %childId <= 0 )
		return 0;   // bad!

	%index = 0;

	while ( %childId != %id ) {
		%childId = %this.getNextSibling( %childId );
		%index++;
	}

	return %index;
}
//------------------------------------------------------------------------------
//==============================================================================
// Add a node and its children to the node tree view
function ShapeLab_NodeTree::addNodeTree( %this, %nodeName ) {
	// Abort if already added => something dodgy has happened and we'd end up
	// recursing indefinitely
	if ( %this.findItemByName( %nodeName ) ) {
		error( "Recursion error in ShapeLab_NodeTree::addNodeTree" );
		return 0;
	}

	// Find parent and add me to it
	%parentName = ShapeLab.shape.getNodeParentName( %nodeName );

	if ( %parentName $= "" )
		%parentName = "<root>";

	%parentId = %this.findItemByName( %parentName );
	%id = %this.insertItem( %parentId, %nodeName, 0, "" );
	// Add children
	%count = ShapeLab.shape.getNodeChildCount( %nodeName );

	for ( %i = 0; %i < %count; %i++ )
		%this.addNodeTree( ShapeLab.shape.getNodeChildName( %nodeName, %i ) );

	return %id;
}
//------------------------------------------------------------------------------
//==============================================================================
// Add a node and its children to the node tree view
function ShapeLab_NodeTree::onDeleteSelection( %this, %nodeName ) {
	%id = ShapeLab_NodeTree.getSelectedItem();
	if ( %id <= 0 ) 
		return;
		
	%name = ShapeLab_NodeTree.getItemText( %id );
	devLog("Deleting node:",%name);
	ShapeLabNodes.deleteNode(%name);
}
//------------------------------------------------------------------------------
