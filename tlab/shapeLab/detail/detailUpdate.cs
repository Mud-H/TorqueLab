//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// ShapeLab -> Update Shape Detail
//==============================================================================
function ShapeLabDetails::selectedShapeChanged( %this ) {
	ShapeLab_DetailTree.clear();
	// --- DETAILS TAB ---
	// Add detail levels and meshes to tree
	ShapeLab_DetailTree.clearSelection();
	ShapeLab_DetailTree.removeItem( 0 );
	%root = ShapeLab_DetailTree.insertItem( 0, "<root>", "", "" );
	
	if (!isObject(ShapeLab.shape))
		return;
	%objCount = ShapeLab.shape.getObjectCount();

	for ( %i = 0; %i < %objCount; %i++ ) {
		%objName = ShapeLab.shape.getObjectName( %i );
		%meshCount = ShapeLab.shape.getMeshCount( %objName );

		for ( %j = 0; %j < %meshCount; %j++ ) {
			%meshName = ShapeLab.shape.getMeshName( %objName, %j );
			ShapeLab_DetailTree.addMeshEntry( %meshName, 1 );
		}
	}

	// Initialise object node list
	ShapeLabDetails-->objectNode.clear();
	ShapeLabDetails-->objectNode.add( "<root>" );
	%nodeCount = ShapeLab.shape.getNodeCount();

	for ( %i = 0; %i < %nodeCount; %i++ )
		ShapeLabDetails-->objectNode.add( ShapeLab.shape.getNodeName( %i ) );
}
//------------------------------------------------------------------------------
//==============================================================================
// ShapeLab -> Update Shape Detail
//==============================================================================
//==============================================================================
function ShapeLab::updateDetail( %this ) {
	%detailCount = ShapeLab.shape.getDetailLevelCount();
	ShapeLabAdv_Details-->detailSlider.range = "0" SPC ( %detailCount-1 );

	if ( %detailCount >= 2 )
		ShapeLabAdv_Details-->detailSlider.ticks = %detailCount - 2;
	else
		ShapeLabAdv_Details-->detailSlider.ticks = 0;

	// Initialise imposter settings
	ShapeLabDetails-->bbUseImposters.setValue( ShapeLab.shape.getImposterDetailLevel() != -1 );

	// Update detail parameters
	if ( ShapeLabShapeView.currentDL < %detailCount ) {
		%settings = ShapeLab.shape.getImposterSettings( ShapeLabShapeView.currentDL );
		%isImposter = getWord( %settings, 0 );
		//ShapeLabAdv_Details-->imposterInactive.setVisible( !%isImposter );
		ShapeLabDetails-->bbEquatorSteps.setText( getField( %settings, 1 ) );
		ShapeLabDetails-->bbPolarSteps.setText( getField( %settings, 2 ) );
		ShapeLabDetails-->bbDetailLevel.setText( getField( %settings, 3 ) );
		ShapeLabDetails-->bbDimension.setText( getField( %settings, 4 ) );
		ShapeLabDetails-->bbIncludePoles.setValue( getField( %settings, 5 ) );
		ShapeLabDetails-->bbPolarAngle.setText( getField( %settings, 6 ) );
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function ShapeLabDetails::updateChangedDetail( %this ) {
	%guiView = ShapeLabShapeView;

// Update slider
	if ( mRound( ShapeLabAdv_Details-->detailSlider.getValue() ) != %guiView.currentDL )
		ShapeLabAdv_Details-->detailSlider.setValue( %guiView.currentDL );

	ShapeLabDetails-->detailSize.setText( %guiView.detailSize );
	ShapeLab.updateDetail();
	%id = ShapeLab_DetailTree.getSelectedItem();

	if ( ( %id <= 0 ) || ( %guiView.currentDL != ShapeLab_DetailTree.getDetailLevelFromItem( %id ) ) ) {
		%id = ShapeLab_DetailTree.findItemByValue( %guiView.detailSize );

		if ( %id > 0 ) {
			ShapeLab_DetailTree.clearSelection();
			ShapeLab_DetailTree.selectItem( %id );
		}
	}
}
//------------------------------------------------------------------------------
//==============================================================================
// ShapeLab -> Update Shape Detail
//==============================================================================
function ShapeLabDetails::updateDetailSize( %this,%value ) {
	if (%value $= "")
		%value = ShapeLabAdv_Details-->detailSize.getText();

	// Change the size of the current detail level
	%oldSize = ShapeLab.shape.getDetailLevelSize( ShapeLabShapeView.currentDL );
	ShapeLab.doEditDetailSize( %oldSize, %value );
}
//------------------------------------------------------------------------------

function ShapeLabDetails::editDetailSize( %this,%newSize ) {
	%id = ShapeLab_DetailTree.getSelectedItem();

	if ( !ShapeLab_DetailTree.isParentItem( %id ) ) {
		
		%name = ShapeLab_DetailTree.getItemText(%id);
		ShapeLab.doEditMeshSize(%name,%newSize);
		return;
		%size = getTrailingNumber( %name );
		%strLen = strlen(%size);
		%stockName = getSubStr(%name,0,strlen(%name)-%strLen);
		%newName = %name @ %newSize;
		ShapeLab_DetailTree.removeMeshEntry(%name);
		ShapeLab_DetailTree.addMeshEntry(%newName);
		warnLog("Changing mesh size was",%name,"Now",%newName);
		
	}

	// Change the size of the selected detail level
	%oldSize = ShapeLab_DetailTree.getItemValue( %id );
	ShapeLab.doEditDetailSize( %oldSize, %newSize );
}

function ShapeLabDetails::renameDetail( %this,%newName ) {
	if (%newName $= "")
		return;

	// Check if we are renaming a detail or a mesh
	%id = ShapeLab_DetailTree.getSelectedItem();
	%oldName = ShapeLab_DetailTree.getItemText( %id );

	if ( ShapeLab_DetailTree.isParentItem( %id ) ) {
		// Rename the selected detail level
		%oldSize = getTrailingNumber( %oldName );
		ShapeLab.doRenameDetail( %oldName, %newName @ %oldSize );
	} else {
		// Rename the selected mesh
		ShapeLab.doRenameObject( stripTrailingNumber( %oldName ), %newName );
	}
}

function ShapeLabDetails::changeObjectNode( %this,%node ) {
	if (%node $= "")
		%node = %this-->objectNode.getText();

	// This command is only valid for meshes (not details)
	%id = ShapeLab_DetailTree.getSelectedItem();

	if ( !ShapeLab_DetailTree.isParentItem( %id ) ) {
		%meshName = ShapeLab_DetailTree.getItemText( %id );
		%objName = stripTrailingNumber( %meshName );
		%node = %this-->objectNode.getText();

		if ( %node $= "<root>" )
			%node = "";

		ShapeLab.doSetObjectNode( %objName, %node );
	}
}
