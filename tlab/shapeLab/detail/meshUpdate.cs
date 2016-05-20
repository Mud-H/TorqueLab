//==============================================================================
// TorqueLab -> ShapeLab -> Mounted Shapes
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// ShapeLab -> Node Editing
//==============================================================================



//==============================================================================
// ShapeLab -> Update Shape Mesh
//==============================================================================
function ShapeLab::editMeshSize( %this ) {
	%newSize = ShapeLabDetails-->meshSize.getText();
	// Check if we are changing the size for a detail or a mesh
	%id = ShapeLab_DetailTree.getSelectedItem();

	if ( ShapeLab_DetailTree.isParentItem( %id ) ) {
		// Change the size of the selected detail level
		%oldSize = ShapeLab_DetailTree.getItemValue( %id );
		ShapeLab.doEditDetailSize( %oldSize, %newSize );
	} else {
		// Change the size of the selected mesh
		%meshName = ShapeLab_DetailTree.getItemText( %id );
		ShapeLab.doEditMeshSize( %meshName, %newSize );
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function ShapeLabDetails::deleteMesh( %this,%id ) {
	if (%id $= "")
		%id = ShapeLab_DetailTree.getSelectedItem();

	if ( ShapeLab_DetailTree.isParentItem( %id ) ) {
		%detSize = ShapeLab_DetailTree.getItemValue( %id );
		ShapeLab.doRemoveShapeData( "Detail", %detSize );
	} else {
		%name = ShapeLab_DetailTree.getItemText( %id );
		ShapeLab.doRemoveShapeData( "Mesh", %name );
	}
}

//------------------------------------------------------------------------------
//==============================================================================
function ShapeLabDetails::addMeshFromFile( %this, %path ) {
	if ( %path $= "" ) {
		getLoadFilename( "DTS Files|*.dts|COLLADA Files|*.dae|Google Earth Files|*.kmz", %this @ ".addMeshFromFile", ShapeLabDetails.lastPath );
		return;
	}

	%path = makeRelativePath( %path, getMainDotCSDir() );
	ShapeLabDetails.lastPath = %path;

	%addMode = ShapeLabDetails-->AddShapeToDetailMenu.getText();
	// Determine the detail level to use for the new geometry
	switch$(%addMode)
	{
		case "current detail":
			%size = ShapeLab.shape.getDetailLevelSize( ShapeLabShapeView.currentDL );
		case "new detail":
			// Check if the file has an LODXXX hint at the end of it
		%base = fileBase( %path );
		%pos = strstr( %base, "_LOD" );

		if ( %pos > 0 )
			%size = getSubStr( %base, %pos + 4, strlen( %base ) ) + 0;
		else
			%size = 2;

		// Make sure size is not in use
		while ( ShapeLab.shape.getDetailLevelIndex( %size ) != -1 )
			%size++;
	}	
	
	//Call the actual function which will add the shape to detail size
	ShapeLab.doAddMeshFromFile( %path, %size );
}
//------------------------------------------------------------------------------
//==============================================================================
function ShapeLab::editMeshBillboard( %this ) {
	// This command is only valid for meshes (not details)
	%id = ShapeLab_DetailTree.getSelectedItem();

	if ( !ShapeLab_DetailTree.isParentItem( %id ) ) {
		%meshName = ShapeLab_DetailTree.getItemText( %id );
		%bbType = ShapeLabDetails-->bbType.getText();

		switch$ ( %bbType ) {
		case "None":
			%bbType = "normal";

		case "Billboard":
			%bbType = "billboard";

		case "Z Billboard":
			%bbType = "billboardzaxis";
		}

		ShapeLab.doEditMeshBillboard( %meshName, %bbType );
	}
}
//------------------------------------------------------------------------------

//==============================================================================
// Update IMPOSTER
//==============================================================================

function ShapeLabDetails::editImposter( %this ) {
	// Modify the parameters of the current imposter detail level
	%detailSize = ShapeLab.shape.getDetailLevelSize( ShapeLabShapeView.currentDL );
	%bbDimension = ShapeLabAdv_Details-->bbDimension.getText();
	%bbDetailLevel = ShapeLabAdv_Details-->bbDetailLevel.getText();
	%bbEquatorSteps = ShapeLabAdv_Details-->bbEquatorSteps.getText();
	%bbIncludePoles = ShapeLabAdv_Details-->bbIncludePoles.getValue();
	%bbPolarSteps = ShapeLabAdv_Details-->bbPolarSteps.getText();
	%bbPolarAngle = ShapeLabAdv_Details-->bbPolarAngle.getText();
	ShapeLab.doEditImposter( ShapeLabShapeView.currentDL, %detailSize,
									 %bbEquatorSteps, %bbPolarSteps, %bbDetailLevel, %bbDimension,
									 %bbIncludePoles, %bbPolarAngle );
}
//------------------------------------------------------------------------------



