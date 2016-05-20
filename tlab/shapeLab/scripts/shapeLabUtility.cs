//==============================================================================
// TorqueLab -> ShapeLab -> Utility Methods
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// ShapeLab -> Utility Methods
//==============================================================================

//==============================================================================
// Nodes Methods
//==============================================================================



//==============================================================================
// Recursively get names for a node and its children
function ShapeLab::getNodeNames( %this, %nodeName, %names, %exclude ) {
	if ( %nodeName $= %exclude )
		return %names;

	%count = %this.shape.getNodeChildCount( %nodeName );

	for ( %i = 0; %i < %count; %i++ ) {
		%childName = %this.shape.getNodeChildName( %nodeName, %i );
		%names = %this.getNodeNames( %childName, %names, %exclude );
	}

	%names = %names TAB %nodeName;
	return trim( %names );
}
//------------------------------------------------------------------------------
//==============================================================================
// ShapeLab -> Utility Methods
//==============================================================================


if ( !isObject( ShapeLab ) ) new ScriptObject( ShapeLab ) {
	shape = -1;
	deletedCount = 0;
};


// Capitalise the first letter of the input string
function strcapitalise( %str ) {
	%len = strlen( %str );
	return strupr( getSubStr( %str,0,1 ) ) @ getSubStr( %str,1,%len-1 );
}

function ShapeLab::getObjectShapeFile( %this, %obj ) {
	// Get the path to the shape file used by the given object (not perfect, but
	// works for the vast majority of object types)
	%path = "";

	if ( %obj.isMemberOfClass( "TSStatic" ) )
		%path = %obj.shapeName;
	else if ( %obj.isMemberOfClass( "PhysicsShape" ) || %obj.isMemberOfClass( "Px3Shape" ) )
      %path = %obj.getDataBlock().shapeName;
	else if ( %obj.isMemberOfClass( "GameBase" ) )
		%path = %obj.getDataBlock().shapeFile;

	return %path;
}

// Check if the given name already exists
function ShapeLab::nameExists( %this, %type, %name ) {
	if ( ShapeLab.shape == -1 )
		return false;

	if ( %type $= "node" )
		return ( ShapeLab.shape.getNodeIndex( %name ) >= 0 );
	else if ( %type $= "sequence" )
		return ( ShapeLab.shape.getSequenceIndex( %name ) >= 0 );
	else if ( %type $= "object" )
		return ( ShapeLab.shape.getObjectIndex( %name ) >= 0 );
}

// Check if the given 'hint' name exists (spaces could also be underscores)
function ShapeLab::hintNameExists( %this, %type, %name ) {
	if ( ShapeLab.nameExists( %type, %name ) )
		return true;

	// If the name contains spaces, try replacing with underscores
	%name = strreplace( %name, " ", "_" );

	if ( ShapeLab.nameExists( %type, %name ) )
		return true;

	return false;
}

// Generate a unique name from a given base by appending an integer
function ShapeLab::getUniqueName( %this, %type, %name ) {
	for ( %idx = 1; %idx < 100; %idx++ ) {
		%uniqueName = %name @ %idx;

		if ( !%this.nameExists( %type, %uniqueName ) )
			break;
	}

	return %uniqueName;
}

function ShapeLab::getProxyName( %this, %seqName ) {
	return "__proxy__" @ %seqName;
}

function ShapeLab::getUnproxyName( %this, %proxyName ) {
	return strreplace( %proxyName, "__proxy__", "" );
}

function ShapeLab::getBackupName( %this, %seqName ) {
	return "__backup__" @ %seqName;
}

// Check if this mesh name is a collision hint
function ShapeLab::isCollisionMesh( %this, %name ) {
	return ( startswith( %name, "ColBox" ) ||
				startswith( %name, "ColSphere" ) ||
				startswith( %name, "ColCapsule" ) ||
				startswith( %name, "ColConvex" ) );
}

//
function ShapeLab::getSequenceSource( %this, %seqName ) {
	%source = %this.shape.getSequenceSource( %seqName );
	// Use the sequence name as the source for DTS built-in sequences
	%src0 = getField( %source, 0 );
	%src1 = getField( %source, 1 );

	if ( %src0 $= %src1 )
		%source = setField( %source, 1, "" );

	if ( %src0 $= "" )
		%source = setField( %source, 0, %seqName );

	return %source;
}



// Get the list of meshes for a particular object
function ShapeLab::getObjectMeshList( %this, %name ) {
	%list = "";
	%count = %this.shape.getMeshCount( %name );

	for ( %i = 0; %i < %count; %i++ )
		%list = %list TAB %this.shape.getMeshName( %name, %i );

	return trim( %list );
}

// Get the list of meshes for a particular detail level
function ShapeLab::getDetailMeshList( %this, %detSize ) {
	%list = "";
	%objCount = ShapeLab.shape.getObjectCount();

	for ( %i = 0; %i < %objCount; %i++ ) {
		%objName = ShapeLab.shape.getObjectName( %i );
		%meshCount = ShapeLab.shape.getMeshCount( %objName );

		for ( %j = 0; %j < %meshCount; %j++ ) {
			%size = ShapeLab.shape.getMeshSize( %objName, %j );

			if ( %size == %detSize )
				%list = %list TAB %this.shape.getMeshName( %objName, %j );
		}
	}

	return trim( %list );
}
