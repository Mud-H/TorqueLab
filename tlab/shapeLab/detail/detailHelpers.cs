//==============================================================================
// TorqueLab -> ShapeLab -> Detail/Mesh Editing
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// ShapeLab -> Detail/Mesh Editing
//==============================================================================

function ShapeLabDetails::onWake( %this ) {
	// Initialise popup menus
	%this-->bbType.clear();
	%this-->bbType.add( "None", 0 );
	%this-->bbType.add( "Billboard", 1 );
	%this-->bbType.add( "Z Billboard", 2 );
	%this-->AddShapeToDetailMenu.clear();
	%this-->AddShapeToDetailMenu.add( "current detail", 0 );
	%this-->AddShapeToDetailMenu.add( "new detail", 1 );
	%this-->AddShapeToDetailMenu.setSelected( 0, false );
	ShapeLab_DetailTree.onDefineIcons();
}



//==============================================================================
// Details functions
//==============================================================================
function ShapeLabDetails::onToggleDetails( %this, %useDetails ) {
	ShapeLabAdv_Details-->detailSlider.setActive(%useDetails);
	//ShapeLabAdv_Details-->levelsInactive.visible = %useDetails;
}

//==============================================================================
// Imposter functions
//==============================================================================

//==============================================================================
// Toggle details imposters
function ShapeLabDetails::onToggleImposter( %this, %useImposter ) {
	%hasImposterDetail = ( ShapeLab.shape.getImposterDetailLevel() != -1 );
	ShapeLabAdv_Details-->imposterActive.visible = %useImposter;

	ShapeLab_ImposterSettings.visible = %useImposter;
	if ( %useImposter == %hasImposterDetail )
		return;

	if ( %useImposter ) {
		// Determine an unused detail size
		for ( %detailSize = 0; %detailSize < 50; %detailSize++ ) {
			if ( ShapeLab.shape.getDetailLevelIndex( %detailSize ) == -1 )
				break;
		}

		// Set some initial values for the imposter
		%bbEquatorSteps = 6;
		%bbPolarSteps = 0;
		%bbDetailLevel = 0;
		%bbDimension = 128;
		%bbIncludePoles = 0;
		%bbPolarAngle = 0;
		// Add a new imposter detail level to the shape
		ShapeLab.doEditImposter( -1, %detailSize, %bbEquatorSteps, %bbPolarSteps,
										 %bbDetailLevel, %bbDimension, %bbIncludePoles, %bbPolarAngle );
	} else {
		// Remove the imposter detail level
		ShapeLab.doRemoveImposter();
	}
}
//==============================================================================




function ShapeLab::autoAddDetails( %this, %dest ) {
	// Sets of LOD files are named like:
	//
	// MyShape_LOD200.dae
	// MyShape_LOD64.dae
	// MyShape_LOD2.dae
	//
	// Determine the base name of the input file (MyShape_LOD in the example above)
	// and use that to find any other shapes in the set.
	%base = fileBase( %dest.baseShape );
	%pos = strstr( %base, "_LOD" );

	if ( %pos < 0 ) {
		echo( "Not an LOD shape file" );
		return;
	}

	%base = getSubStr( %base, 0, %pos + 4 );
	echo( "Base is: " @ %base );
	%filePatterns = filePath( %dest.baseShape ) @ "/" @ %base @ "*" @ fileExt( %dest.baseShape );
	echo( "Pattern is: " @ %filePatterns );
	%fullPath = findFirstFileMultiExpr( %filePatterns );

	while ( %fullPath !$= "" ) {
		%fullPath = makeRelativePath( %fullPath, getMainDotCSDir() );

		if ( %fullPath !$= %dest.baseShape ) {
			echo( "Found LOD shape file: " @ %fullPath );
			// Determine the detail size ( number after the base name ), then add the
			// new mesh
			%size = strreplace( fileBase( %fullPath ), %base, "" );
			ShapeLab.addLODFromFile( %dest, %fullPath, %size, 0 );
		}

		%fullPath = findNextFileMultiExpr( %filePatterns );
	}

	if ( %this.shape == %dest ) {
		ShapeLabShapeView.refreshShape();
		ShapeLab.updateDetail();
	}
}

function ShapeLab::addLODFromFile( %this, %dest, %filename, %size, %allowUnmatched ) {
	// Get (or create) a TSShapeConstructor object for the source shape. Need to
	// exec the script manually as the resource may not have been loaded yet
	%csPath = filePath( %filename ) @ "/" @ fileBase( %filename ) @ ".cs";

	if ( isFile( %csPath ) )
		exec( %csPath );

	%source = ShapeLab.findConstructor( %filename );

	if ( %source == -1 )
		%source = ShapeLab.createConstructor( %filename );

	%source.lodType = "SingleSize";
	%source.singleDetailSize = %size;
	// Create a temporary TSStatic to ensure the resource is loaded
	%temp = new TSStatic() {
		shapeName = %filename;
		collisionType = "None";
	};
	%meshList = "";

	if ( isObject( %temp ) ) {
		// Add a new mesh for each object in the source shape
		%objCount = %source.getObjectCount();

		for ( %i = 0; %i < %objCount; %i++ ) {
			%objName = %source.getObjectName( %i );
			echo( "Checking for object " @ %objName );

			if ( %allowUnmatched || ( %dest.getObjectIndex( %objName ) != -1 ) ) {
				// Add the source object's highest LOD mesh to the destination shape
				echo( "Adding detail size" SPC %size SPC "for object" SPC %objName );
				%srcName = %source.getMeshName( %objName, 0 );
				%destName = %objName SPC %size;
				%dest.addMesh( %destName, %filename, %srcName );
				%meshList = %meshList TAB %destName;
			}
		}

		%temp.delete();
	}

	return trim( %meshList );
}
