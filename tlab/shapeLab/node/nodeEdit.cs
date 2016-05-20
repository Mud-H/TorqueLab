//==============================================================================
// TorqueLab -> ShapeLab -> Node Editing
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
function ShapeLab::toggleTransformMode( %this ) {
	ShapeLabNodes.isWorldTransform = !ShapeLabNodes.isWorldTransform;
	ShapeLabNodes.isObjectTransform = !ShapeLabNodes.isWorldTransform;
}
function ShapeLab::changeNodeTransformMode( %this, %mode ) {
	EditorGuiToolbarStack-->transformToggleBtn.setStateOn(ShapeLabNodes.isWorldTransform);
	%isWorld = (%mode $= "World") ? true : false;
	%id = ShapeLab_NodeTree.getSelectedItem();
	if ( %id > 0 )
		%nodeName = ShapeLab_NodeTree.getItemText( %id );
	else
		return;
	
	
	//ShapeLabNodes.isWorldTransform = %isWorld;
	//ShapeLabNodes.isObjectTransform = !%isWorld;
	%transform = ShapeLab.shape.getNodeTransform( %nodeName, ShapeLabNodes.isWorldTransform );
	ShapeLabNodes-->nodePosition.setText( getWords( %transform, 0, 2 ) );
	ShapeLabNodes-->nodeRotation.setText( getWords( %transform, 3, 6 ) );	
}

//==============================================================================
//ShapeLabNodes.onAddNode("Base");
function ShapeLabNodes::addNode( %this, %name ) {
	// Add a new node, using the currently selected node as the initial parent
	if ( %name $= "" )
		%name = ShapeLab.getUniqueName( "node", "myNode" );

	%id = ShapeLab_NodeTree.getSelectedItem();

	if ( %id <= 0 )
		%parent = "";
	else
		%parent = ShapeLab_NodeTree.getItemText( %id );

	ShapeLab.doAddNode( %name, %parent, "0 0 0 0 0 1 0" );
}
//------------------------------------------------------------------------------
//==============================================================================
function ShapeLabNodes::deleteNode( %this,%name ) {
	// Remove the node and all its children from the shape
	if (%name $= "") {
		%id = ShapeLab_NodeTree.getSelectedItem();

		if ( %id > 0 )
			%name = ShapeLab_NodeTree.getItemText( %id );
	}

	if (%name !$= "")
		ShapeLab.doRemoveShapeData( "Node", %name );
}
//------------------------------------------------------------------------------
//==============================================================================
function ShapeLabNodes::renameNode( %this,%oldName ) {
	if (%oldName $= "") {
		%id = ShapeLab_NodeTree.getSelectedItem();

		if ( %id > 0 )
			%oldName = ShapeLab_NodeTree.getItemText( %id );
	}

	%newName = %this-->nodeName.getText();

	if ( %newName !$= "" && %oldName !$= "")
		ShapeLab.doRenameNode( %oldName, %newName );
}
//------------------------------------------------------------------------------
//==============================================================================
function ShapeLabNodes::updateNodeTransform( %this,%name ) {
	if (%name $= "") {
		%id = ShapeLab_NodeTree.getSelectedItem();

		if ( %id > 0 )
			%name = ShapeLab_NodeTree.getItemText( %id );
	}

	if (%name $= "")
		return;

	// Get the node transform from the gui
	%pos = %this-->nodePosition.getText();
	%rot = %this-->nodeRotation.getText();
	%txfm = %pos SPC %rot;
	%isWorld = ShapeLabNodes-->worldTransform.getValue();

	// Do a quick sanity check to avoid setting wildly invalid transforms
	for ( %i = 0; %i < 7; %i++ ) {  // "x y z aa.x aa.y aa.z aa.angle"
		if ( getWord( %txfm, %i ) $= "" )
			return;
	}

	ShapeLab.doEditNodeTransform( %name, %txfm, %isWorld, -1 );
}
//------------------------------------------------------------------------------
