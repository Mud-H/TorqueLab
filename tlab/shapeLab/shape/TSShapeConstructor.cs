//==============================================================================
// TorqueLab -> TSShapeConstructor Functions
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// Reference function for future system
//==============================================================================
/*

// The TSShapeConstructor object allows you to apply a set of transformations
// to a 3space shape after it is loaded by Torque, but _before_ the shape is used
// by any other object (eg. Player, StaticShape etc). The sort of transformations
// available include adding, renaming and removing nodes and sequences. This GUI
// is a visual wrapper around TSShapeConstructor which allows you to build up the
// transformation set without having to get your hands dirty with TorqueScript.
//
// Removing a node, sequence, mesh or detail poses a problem. These operations
// permanently delete a potentially large amount of data scattered throughout
// the shape, and there is no easy way to restore it if the user 'undoes' the
// delete. Although it is possible to store the deleted data somewhere and restore
// it on undo, it is not easy to get right, and ugly as hell to implement. For
// example, removing a node would require storing the node name, the
// translation/rotation/scale matters bit for each sequence, all node transform
// keyframes, the IDs of any objects that were attached to the node, skin weights
// etc, then restoring all that data into the original place on undo. Frankly,
// TSShape was never designed to be modified dynamically like that.
//
// So......currently we wimp out completely and just don't support undo for those
// remove operations. Lame, I know, but the best I can do for now.
//
// This file implements all of the actions that can be applied by the GUI. Each
// action has 3 methods:
//
//    doit: called the first time the action is performed
//    undo: called to undo the action
//    redo: called to redo the action (usually the same as doit)
//
// In each case, the appropriate change is made to the shape, and the GUI updated.
//
// TSShapeConstructor keeps track of all the changes made and provides a simple
// way to save the modifications back out to a script file.

function ShapeLab::findConstructor( %this, %path ) {
	%count = TSShapeConstructorGroup.getCount();

	for ( %i = 0; %i < %count; %i++ ) {
		%obj = TSShapeConstructorGroup.getObject( %i );

		if ( %obj.baseShape $= %path )
			return %obj;
	}

	return -1;
}

function ShapeLab::createConstructor( %this, %path ) {
	%name = strcapitalise( fileBase( %path ) ) @ strcapitalise( getSubStr( fileExt( %path ), 1, 3 ) );
	%name = strreplace( %name, "-", "_" );
	%name = strreplace( %name, ".", "_" );
	%name = getUniqueName( %name );
	return new TSShapeConstructor( %name ) {
		baseShape = %path;
	};
}

function ShapeLab::saveConstructor( %this, %constructor ) {
	%savepath = filePath( %constructor.baseShape ) @ "/" @ fileBase( %constructor.baseShape ) @ ".cs";
	new PersistenceManager( shapeEd_perMan );
	shapeEd_perMan.setDirty( %constructor, %savepath );
	shapeEd_perMan.saveDirtyObject( %constructor );
	shapeEd_perMan.delete();
}




function ColladaImportDlg::onOK(%this) {
	ColladaImportDlg.exportPrefs();
	Canvas.popDialog(%this);
	ColladaImportTreeView.clear();

	// Need to create a TSShapeConstructor object if any settings are not
	// at the default values
	if ((%this-->overrideUpAxis.getValue() != 0)       ||
			(%this-->overrideScale.getValue() != 0)        ||
			(%this-->lodType.getText() !$= "DetectDTS")    ||
			(%this-->singleDetailSize.getText() !$= "2")   ||
			(%this-->materialPrefix.getText() !$= "")      ||
			(%this-->alwaysImport.getText() !$= "")        ||
			(%this-->neverImport.getText() !$= "")         ||
			(%this-->alwaysImportMesh.getText() !$= "")    ||
			(%this-->neverImportMesh.getText() !$= "")     ||
			(%this-->ignoreNodeScale.getValue() != 0)      ||
			(%this-->adjustCenter.getValue() != 0)         ||
			(%this-->adjustFloor.getValue() != 0)          ||
			(%this-->forceUpdateMaterials.getValue() != 0) ||
			(%this-->loadLights.getValue() != 0)) {
		if (%this.constructor <= 0) {
			// Create a new TSShapeConstructor object
			%this.constructor = ShapeLab.createConstructor(%this.path);
		}
	}

	if (%this.constructor > 0) {
		// Store values from GUI
		if (%this-->overrideUpAxis.getValue())
			%this.constructor.upAxis = %this-->upAxis.getText();
		else
			%this.constructor.upAxis = "DEFAULT";

		if (%this-->overrideScale.getValue())
			%this.constructor.unit = %this-->scale.getText();
		else
			%this.constructor.unit = -1;

		%this.constructor.lodType = %this-->lodType.getText();
		%this.constructor.singleDetailSize = %this-->singleDetailSize.getText();
		%this.constructor.matNamePrefix = %this-->materialPrefix.getText();
		%this.constructor.alwaysImport = strreplace(%this-->alwaysImport.getText(), ";", "\t");
		%this.constructor.neverImport = strreplace(%this-->neverImport.getText(), ";", "\t");
		%this.constructor.alwaysImportMesh = strreplace(%this-->alwaysImportMesh.getText(), ";", "\t");
		%this.constructor.neverImportMesh = strreplace(%this-->neverImportMesh.getText(), ";", "\t");
		%this.constructor.ignoreNodeScale = %this-->ignoreNodeScale.getValue();
		%this.constructor.adjustCenter = %this-->adjustCenter.getValue();
		%this.constructor.adjustFloor = %this-->adjustFloor.getValue();
		%this.constructor.forceUpdateMaterials = %this-->forceUpdateMaterials.getValue();
		%this.constructor.loadLights = %this-->loadLights.getValue();
		// Save new settings to file
		ShapeLab.saveConstructor( %this.constructor );
	}

	// Load the shape (always from the DAE)
	$collada::forceLoadDAE = true;
	eval(%this.cmd);
	$collada::forceLoadDAE = false;

	// Optionally load the lights from the DAE as well (only if adding a new shape
	// to the scene)
	if (%this-->loadLights.getValue())
		%this.loadLights();
}

%source = ShapeLab.findConstructor( %filename );

	if ( %source == -1 )
		%source = ShapeLab.createConstructor( %filename );

	// Get ( or create ) the TSShapeConstructor object for this shape
	ShapeLab.shape = ShapeLab.findConstructor( %path );

	if ( ShapeLab.shape <= 0 ) {
		ShapeLab.shape = %this.createConstructor( %path );

		if ( ShapeLab.shape <= 0 ) {
			error( "ShapeLab: Error - could not select " @ %path );
			return;
		}
	}

*/
