//==============================================================================
// TorqueLab -> ShapeLab -> Shape Selection
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// ShapeLab -> Collision editing
//==============================================================================

function ShapeLabCollisions::onShapeSelectionChanged( %this ) {
	%this.lastColSettings = "" TAB "Bounds";
	// Initialise collision mesh target list
	ShapeLab_CreateColRollout-->colTarget.clear();
	ShapeLab_CreateColRollout-->colTarget.add( "Bounds" );
	%objCount = ShapeLab.shape.getObjectCount();

	for ( %i = 0; %i < %objCount; %i++ ) {
		ShapeLab_CreateColRollout-->colTarget.add( ShapeLab.shape.getObjectName( %i ) );
	}

	ShapeLab_CreateColRollout-->colTarget.setSelected( %this-->colTarget.findText( "Bounds" ), false );
}

function ShapeLabCollisions::onCollisionChanged( %this ) {
	// Sync collision settings
	%colData = %this.lastColSettings;
	%typeId = %this-->colType.findText( getField( %colData, 0 ) );
	%this-->colType.setSelected( %typeId, false );
	%targetId = %this-->colTarget.findText( getField( %colData, 1 ) );
	%this-->colTarget.setSelected( %targetId, false );

	if ( %this-->colType.getText() $= "Convex Hulls" ) {
		show(ShapeLabColCreate_Hull);
		hide(ShapeLabColCreate_NoHull);
		%this-->hullDepth.setValue( getField( %colData, 2 ) );
		%this-->hullDepthText.setText( mFloor( %this-->hullDepth.getValue() ) );
		%this-->hullMergeThreshold.setValue( getField( %colData, 3 ) );
		%this-->hullMergeText.setText( mFloor( %this-->hullMergeThreshold.getValue() ) );
		%this-->hullConcaveThreshold.setValue( getField( %colData, 4 ) );
		%this-->hullConcaveText.setText( mFloor( %this-->hullConcaveThreshold.getValue() ) );
		%this-->hullMaxVerts.setValue( getField( %colData, 5 ) );
		%this-->hullMaxVertsText.setText( mFloor( %this-->hullMaxVerts.getValue() ) );
		%this-->hullMaxBoxError.setValue( getField( %colData, 6 ) );
		%this-->hullMaxBoxErrorText.setText( mFloor( %this-->hullMaxBoxError.getValue() ) );
		%this-->hullMaxSphereError.setValue( getField( %colData, 7 ) );
		%this-->hullMaxSphereErrorText.setText( mFloor( %this-->hullMaxSphereError.getValue() ) );
		%this-->hullMaxCapsuleError.setValue( getField( %colData, 8 ) );
		%this-->hullMaxCapsuleErrorText.setText( mFloor( %this-->hullMaxCapsuleError.getValue() ) );
	} else {
		hide(ShapeLabColCreate_Hull);
		show(ShapeLabColCreate_NoHull);
	}
}



//==============================================================================
// TorqueLab Collision Functions
//==============================================================================
