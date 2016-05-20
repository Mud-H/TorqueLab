//==============================================================================
// TorqueLab -> ShapeLab -> Shape Selection
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

function ShapeLab::updateCollisionMesh( %this, %type, %target, %depth, %merge, %concavity,
		%maxVerts, %boxMax, %sphereMax, %capsuleMax ) {
	%colDetailSize = -1;
	%colNode = "Col" @ %colDetailSize;
	// TreeView items are case sensitive, but TSShape names are not, so fixup case
	// if needed
	%index = ShapeLab.shape.getNodeIndex( %colNode );

	if ( %index != -1 )
		%colNode = ShapeLab.shape.getNodeName( %index );

	// First remove the old detail and collision nodes
	%meshList = ShapeLab.getDetailMeshList( %colDetailSize );
	%meshCount = getFieldCount( %meshList );

	if ( %meshCount > 0 ) {
		ShapeLab.shape.removeDetailLevel( %colDetailSize );

		for ( %i = 0; %i < %meshCount; %i++ )
			ShapeLab.onMeshRemoved( getField( %meshList, %i ) );
	}

	%nodeList = ShapeLab.getNodeNames("Col-1", "" );
	%nodeCount = getFieldCount( %nodeList );

	if ( %nodeCount > 0 ) {
		for ( %i = 0; %i < %nodeCount; %i++ )
			ShapeLab.shape.removeNode( getField( %nodeList, %i ) );

		ShapeLab.onNodeRemoved( %nodeList, %nodeCount );
	}
devLog("Collision type to be created", %type);
	// Add the new node and geometry
	if ( %type $= "" )
		return;

	if ( !ShapeLab.shape.addCollisionDetail( %colDetailSize, %type, %target,
			%depth, %merge, %concavity, %maxVerts,
			%boxMax, %sphereMax, %capsuleMax ) )
		return false;
	devLog("Collision created", %type, %target,
			%depth, %merge, %concavity, %maxVerts,
			%boxMax, %sphereMax, %capsuleMax);
		
	// Update UI
	%meshList = ShapeLab.getDetailMeshList( %colDetailSize );
	ShapeLab.onNodeAdded( %colNode, ShapeLab.shape.getNodeCount() );    // will also add child nodes
	%count = getFieldCount( %meshList );

	for ( %i = 0; %i < %count; %i++ )
		ShapeLab.onMeshAdded( getField( %meshList, %i ) );

	ShapeLabCollisions.lastColSettings = %type TAB %target TAB %depth TAB %merge TAB
													%concavity TAB %maxVerts TAB %boxMax TAB %sphereMax TAB %capsuleMax;
	ShapeLabCollisions.onCollisionChanged();
	
	return true;
}
//==============================================================================
function ShapeLabCollisions::revertChanges( %this ) {
	devLog("ShapeLabCollisions::revertChanges(%this)",%this);
}
//------------------------------------------------------------------------------
//==============================================================================
function ShapeLabCollisions::updateHulls( %this ) {
	devLog("ShapeLabCollisions::updateHulls(%this)",%this);
}
//------------------------------------------------------------------------------

//==============================================================================
function ShapeLabCollisions::editCollision( %this ) {
	// If the shape already contains a collision detail size-1, warn the user
	// that it will be removed
	if ( ( ShapeLab.shape.getDetailLevelIndex( -1 ) >= 0 ) &&
			( getField(%this.lastColSettings, 0) $= "" ) ) {
		LabMsgYesNo( "Warning", "Existing collision geometry at detail size " @
						 "-1 will be removed, and this cannot be undone. Do you want to continue?",
						 "ShapeLabCollisions.editCollisionOK();", "" );
	} else {
		%this.editCollisionOK();
	}
}
//------------------------------------------------------------------------------
//==============================================================================
$NoColUndo = true;
function ShapeLabCollisions::editCollisionOK( %this ) {
	%type = %this-->colType.getText();
	%target = %this-->colTarget.getText();
	%depth = %this-->hullDepth.getValue();
	%merge = %this-->hullMergeThreshold.getValue();
	%concavity = %this-->hullConcaveThreshold.getValue();
	%maxVerts = %this-->hullMaxVerts.getValue();
	%maxBox = %this-->hullMaxBoxError.getValue();
	%maxSphere = %this-->hullMaxSphereError.getValue();
	%maxCapsule = %this-->hullMaxCapsuleError.getValue();
	if (%type $= "convex hulls"){
		devLog("convex hulls-- %target, %depth, %merge, %concavity, %maxVerts,  %maxBox, %maxSphere, %maxCapsule");
		devLog( %target, %depth, %merge, %concavity, %maxVerts, %maxBox, %maxSphere, %maxCapsule);
	}
	if ($NoColUndo)
	ShapeLab.updateCollisionMesh( %type, %target, %depth, %merge, %concavity, %maxVerts,
										  %maxBox, %maxSphere, %maxCapsule );
	else
	ShapeLab.doEditCollision( %type, %target, %depth, %merge, %concavity, %maxVerts,
										  %maxBox, %maxSphere, %maxCapsule );
}
//------------------------------------------------------------------------------

//==============================================================================
// ShapeLab Action -> Update the collision mesh
//==============================================================================

//==============================================================================
// Build the action object
function ShapeLab::doEditCollision( %this, %type, %target, %depth, %merge, %concavity,
													%maxVerts, %boxMax, %sphereMax, %capsuleMax ) {
	%colData = ShapeLabCollisions.lastColSettings;
	%action = %this.createAction( ActionEditCollision, "Edit shape collision" );
	%action.oldType = getField( %colData, 0 );
	%action.oldTarget = getField( %colData, 1 );
	%action.oldDepth = getField( %colData, 2 );
	%action.oldMerge = getField( %colData, 3 );
	%action.oldConcavity = getField( %colData, 4 );
	%action.oldMaxVerts = getField( %colData, 5 );
	%action.oldBoxMax = getField( %colData, 6 );
	%action.oldSphereMax = getField( %colData, 7 );
	%action.oldCapsuleMax = getField( %colData, 8 );
	%action.newType = %type;
	%action.newTarget = %target;
	%action.newDepth = %depth;
	%action.newMerge = %merge;
	%action.newConcavity = %concavity;
	%action.newMaxVerts = %maxVerts;
	%action.newBoxMax = %boxMax;
	%action.newSphereMax = %sphereMax;
	%action.newCapsuleMax = %capsuleMax;
	%this.doAction( %action );
}
//------------------------------------------------------------------------------
//==============================================================================
//Update the collision mesh using TSShapeConstructor::addCollisionDetail
/*------------------------------------------------------------------------------
DefineTSShapeConstructorMethod( addCollisionDetail, bool, ( S32 size, const char* type, const char* target, S32 depth, F32 merge, F32 concavity, S32 maxVerts, F32 boxMaxError, F32 sphereMaxError, F32 capsuleMaxError ), ( 4, 30, 30, 32, 0, 0, 0 ),
   ( size, type, target, depth, merge, concavity, maxVerts, boxMaxError, sphereMaxError, capsuleMaxError ), false,
   "Autofit a mesh primitive or set of convex hulls to the shape geometry. Hulls "
   "may optionally be converted to boxes, spheres and/or capsules based on their "
   "volume.\n"
   "@param size size for this detail level\n"
   "@param type one of: box, sphere, capsule, 10-dop x, 10-dop y, 10-dop z, 18-dop, "
      "26-dop, convex hulls. See the Shape Lab documentation for more details "
      "about these types.\n"
   "@param target geometry to fit collision mesh(es) to; either \"bounds\" (for the "
      "whole shape), or the name of an object in the shape\n"
   "@param depth maximum split recursion depth (hulls only)\n"
   "@param merge volume % threshold used to merge hulls together (hulls only)\n"
   "@param concavity volume % threshold used to detect concavity (hulls only)\n"
   "@param maxVerts maximum number of vertices per hull (hulls only)\n"
   "@param boxMaxError max % volume difference for a hull to be converted to a "
      "box (hulls only)\n"
   "@param sphereMaxError max % volume difference for a hull to be converted to "
      "a sphere (hulls only)\n"
   "@param capsuleMaxError max % volume difference for a hull to be converted to "
      "a capsule (hulls only)\n"
   "@return true if successful, false otherwise\n\n"
   "@tsexample\n"
   "%this.addCollisionDetail( -1, \"box\", \"bounds\" );\n"
   "%this.addCollisionDetail( -1, \"convex hulls\", \"bounds\", 4, 30, 30, 32, 0, 0, 0 );\n"
   "%this.addCollisionDetail( -1, \"convex hulls\", \"bounds\", 4, 30, 30, 32, 50, 50, 50 );\n"
   "@endtsexample\n" )
//----------------------------------------------------------------------------*/
//------------------------------------------------------------------------------
//==============================================================================
function ActionEditCollision::updateCollision( %this, %type, %target, %depth, %merge, %concavity,
		%maxVerts, %boxMax, %sphereMax, %capsuleMax ) {
	%colDetailSize = -1;
	%colNode = "Col" @ %colDetailSize;
	// TreeView items are case sensitive, but TSShape names are not, so fixup case
	// if needed
	%index = ShapeLab.shape.getNodeIndex( %colNode );

	if ( %index != -1 )
		%colNode = ShapeLab.shape.getNodeName( %index );

	// First remove the old detail and collision nodes
	%meshList = ShapeLab.getDetailMeshList( %colDetailSize );
	%meshCount = getFieldCount( %meshList );

	if ( %meshCount > 0 ) {
		ShapeLab.shape.removeDetailLevel( %colDetailSize );

		for ( %i = 0; %i < %meshCount; %i++ )
			ShapeLab.onMeshRemoved( getField( %meshList, %i ) );
	}

	%nodeList = ShapeLab.getNodeNames("Col-1", "" );
	%nodeCount = getFieldCount( %nodeList );

	if ( %nodeCount > 0 ) {
		for ( %i = 0; %i < %nodeCount; %i++ )
			ShapeLab.shape.removeNode( getField( %nodeList, %i ) );

		ShapeLab.onNodeRemoved( %nodeList, %nodeCount );
	}
devLog("Collision type to be created", %type);
	// Add the new node and geometry
	if ( %type $= "" )
		return;

	if ( !ShapeLab.shape.addCollisionDetail( %colDetailSize, %type, %target,
			%depth, %merge, %concavity, %maxVerts,
			%boxMax, %sphereMax, %capsuleMax ) )
		return false;
	devLog("Collision created", %type, %target,
			%depth, %merge, %concavity, %maxVerts,
			%boxMax, %sphereMax, %capsuleMax);
		
	// Update UI
	%meshList = ShapeLab.getDetailMeshList( %colDetailSize );
	ShapeLab.onNodeAdded( %colNode, ShapeLab.shape.getNodeCount() );    // will also add child nodes
	%count = getFieldCount( %meshList );

	for ( %i = 0; %i < %count; %i++ )
		ShapeLab.onMeshAdded( getField( %meshList, %i ) );

	ShapeLabCollisions.lastColSettings = %type TAB %target TAB %depth TAB %merge TAB
													%concavity TAB %maxVerts TAB %boxMax TAB %sphereMax TAB %capsuleMax;
	ShapeLabCollisions.onCollisionChanged();
	
	return true;
}
//------------------------------------------------------------------------------
//==============================================================================
// Do the Edit Collision Action
function ActionEditCollision::doit( %this ) {
	//ShapeLabWaitGui.show( "Generating collision geometry..." );
	%success = %this.updateCollision( %this.newType, %this.newTarget, %this.newDepth, %this.newMerge,
												 %this.newConcavity, %this.newMaxVerts, %this.newBoxMax,
												 %this.newSphereMax, %this.newCapsuleMax );
	//ShapeLabWaitGui.hide();
	return %success;
}
//------------------------------------------------------------------------------
//==============================================================================
// UnDo the Edit Collision Action
function ActionEditCollision::undo( %this ) {
	Parent::undo( %this );
	//ShapeLabWaitGui.show( "Generating collision geometry..." );
	%this.updateCollision( %this.oldType, %this.oldTarget, %this.oldDepth, %this.oldMerge,
								  %this.oldConcavity, %this.oldMaxVerts, %this.oldBoxMax,
								  %this.oldSphereMax, %this.oldCapsuleMax );
	//ShapeLabWaitGui.hide();
}
//------------------------------------------------------------------------------
