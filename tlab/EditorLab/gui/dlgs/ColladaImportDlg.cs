//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$ColladaImportCheckboxes = "ignoreNodeScale adjustCenter adjustFloor forceUpdateMaterials loadLights";

//==============================================================================
function ColladaImportDlg::exportPrefs(%this) {
	//export("$pref::ColladaImport::*", "tlab/EditorLab/settings/prefs/colladaImport.cs", false);
}


//==============================================================================
function ColladaImportTreeView::onDefineIcons(%this) {
	// Set the tree view icon indices and texture paths
	%this._imageNone = 0;
	%this._imageNode = 1;
	%this._imageMesh = 2;
	%this._imageMaterial = 3;
	%this._imageLight = 4;
	%this._imageAnimation = 5;
	%this._imageExNode = 6;
	%this._imageExMaterial = 7;
	%icons = ":" @                                                    // no icon
				"tlab/art/icons/iconTables/ColladaImport/iconNode:" @             // normal node
				"tlab/art/icons/iconTables/ColladaImport/iconMesh:" @             // mesh
				"tlab/art/icons/iconTables/ColladaImport/iconMaterial:" @         // new material
				"tlab/art/icons/iconTables/ColladaImport/iconLight:" @            // light
				"tlab/art/icons/iconTables/ColladaImport/iconAnimation:" @        // sequence
				"tlab/art/icons/iconTables/ColladaImport/iconIgnoreNode:" @       // ignored node
				"tlab/art/icons/iconTables/ColladaImport/iconExistingMaterial";   // existing material
	%this.buildIconTable( %icons );
}
//------------------------------------------------------------------------------
function ColladaImportDlg::showDialog(%this, %shapePath, %cmd) {
	%this.path = %shapePath;
	%this.cmd = %cmd;
	ColladaImportOptions.expanded = false;
	// Only allow loading lights if creating a new scene object
	%canLoadLights = (strstr(%this.cmd, "SEP_Creator.create") != -1);
	// Check for an existing TSShapeConstructor object. Need to exec the script
	// manually as the DAE resource may not have been loaded yet
	%csPath = filePath(%this.path) @ "/" @ fileBase(%this.path) @ ".cs";

	if (isFile(%csPath))
		exec(%csPath);

	%this.constructor = findConstructor(%this.path);
	// Only show the import dialog if required. Note that 'enumColladaScene' will
	// fail if the COLLADA file is missing, or a cached.dts is available.
	$collada::forceLoadDAE = Lab.forceLoadDAE;

	if ( (fileExt(%shapePath) $= ".dts") ||
			!enumColladaForImport(%shapePath, ColladaImportTreeView) ) {
		eval(%cmd);
		$collada::forceLoadDAE = false;

		// Load lights from the DAE if possible
		if (%canLoadLights && (%this.constructor > 0) && (%this.constructor.loadLights == 1))
			%this.loadLights();

		return;
	}

	$collada::forceLoadDAE = false;
	// Initialise GUI
	ColladaImportTreeView.onDefineIcons();
	%this-->window.text = "COLLADA Import:" SPC %this.path;
	%this-->upAxis.clear();
	%this-->upAxis.add("X_AXIS", 1);
	%this-->upAxis.add("Y_AXIS", 2);
	%this-->upAxis.add("Z_AXIS", 3);
	%this-->lodType.clear();
	%this-->lodType.add("DetectDTS", 1);
	%this-->lodType.add("SingleSize", 2);
	%this-->lodType.add("TrailingNumber", 3);
	%this-->loadLights.setActive(%canLoadLights);
	// Set model details
	%this-->nodes.setText(ColladaImportTreeView._nodeCount);
	%this-->meshes.setText(ColladaImportTreeView._meshCount);
	%this-->polygons.setText(ColladaImportTreeView._polygonCount);
	%this-->materials.setText(ColladaImportTreeView._materialCount);
	%this-->lights.setText(ColladaImportTreeView._lightCount);
	%this-->animations.setText(ColladaImportTreeView._animCount);
	%this.updateOverrideUpAxis(false);
	%this.updateOverrideScale(false);

	if (%this.constructor > 0) {
		if (%this.constructor.upAxis !$= "DEFAULT") {
			%this-->upAxis.setText(%this.constructor.upAxis);
			%this.updateOverrideUpAxis(true);
		}

		if (%this.constructor.unit > 0) {
			%this-->scale.setText(%this.constructor.unit);
			%this.updateOverrideScale(true);
		}

		%this-->lodType.setText(%this.constructor.lodType);
		%this-->singleDetailSize.setText(%this.constructor.singleDetailSize);
		%this-->materialPrefix.setText(%this.constructor.matNamePrefix);
		%this-->alwaysImport.setText(strreplace(%this.constructor.alwaysImport, "\t", ";"));
		%this-->neverImport.setText(strreplace(%this.constructor.neverImport, "\t", ";"));
		%this-->alwaysImportMesh.setText(strreplace(%this.constructor.alwaysImportMesh, "\t", ";"));
		%this-->neverImportMesh.setText(strreplace(%this.constructor.neverImportMesh, "\t", ";"));

		foreach(%opt in $ColladaImportCheckboxes) {
			eval("%value = %this.constructor."@%opt@";");
			eval("%pref = $pref::ColladaImport::"@%opt@";");
			info("Checking collada import setting:",%opt,"Pref=",%pref,"Constructor value=",%value);

			if (%value $= "")
				%value = %pref;

			eval("%this-->"@%opt@".setStateOn(%value);");
		}

		// %this-->ignoreNodeScale.setStateOn(%this.constructor.ignoreNodeScale);
		// %this-->adjustCenter.setStateOn(%this.constructor.adjustCenter);
		//%this-->adjustFloor.setStateOn(%this.constructor.adjustFloor);
		//%this-->forceUpdateMaterials.setStateOn(%this.constructor.forceUpdateMaterials);
		//%this-->loadLights.setStateOn(%this.constructor.loadLights);
	} else {
		// Default settings
		%this-->lodType.setText("DetectDTS");
		%this-->singleDetailSize.setText("2");
		%this-->materialPrefix.setText("");
		%this-->alwaysImport.setText("");
		%this-->neverImport.setText("");
		%this-->alwaysImportMesh.setText("");
		%this-->neverImportMesh.setText("");
		%this-->ignoreNodeScale.setStateOn($pref::ColladaImport::ignoreNodeScale);
		%this-->adjustCenter.setStateOn($pref::ColladaImport::adjustCenter);
		%this-->adjustFloor.setStateOn($pref::ColladaImport::adjustFloor);
		%this-->forceUpdateMaterials.setStateOn($pref::ColladaImport::forceUpdateMaterials);
		%this-->loadLights.setStateOn($pref::ColladaImport::loadLights);
	}

	Canvas.pushDialog(%this);
	ColladaImportTreeView.refresh("all");
}

function ColladaImportDlg::readDtsConfig(%this) {
	%filename = filePath( %this.path ) @ "/" @ fileBase( %this.path ) @ ".cfg";
	%filename2 = filePath( %this.path ) @ "/" @ "dtsScene.cfg";
	%fo = new FileObject();

	if ( %fo.openForRead( %filename ) || %fo.openForRead( %filename2 ) ) {
		%alwaysImport = "";
		%neverImport = "";
		%mode = "none";

		while ( !%fo.isEOF() ) {
			%line = trim( %fo.readLine() );

			if ( %line $= "AlwaysExport:" )        // Start of the AlwaysExport list
				%mode = "always";
			else if ( %line $= "NeverExport:" )    // Start of the NeverExport list
				%mode = "never";
			else if ( startswith( %line, "+" ) || startswith( %line, "-" ) )   // Boolean parameters (not supported)
				%mode = "none";
			else if ( startswith( %line, "=" ) )   // Float and integer parameters (not supported)
				%mode = "none";
			else if ( !startswith( %line, "//" ) ) { // Non-commented lines
				switch$ (%mode) {
				case "always":
					%alwaysImport = %alwaysImport TAB %line;

				case "never":
					%neverImport = %neverImport TAB %line;
				}
			}
		}

		%fo.close();
		%alwaysImport = strreplace( trim( %alwaysImport ), "\t", ";" );
		%neverImport = strreplace( trim( %neverImport ), "\t", ";" );
		%this-->alwaysImport.setText( %alwaysImport );
		%this-->neverImport.setText( %neverImport );
	} else {
		error( "Failed to open " @ %filename @ " or " @ %filename2 @ " for reading" );
	}

	%fo.delete();
}

function ColladaImportDlg::writeDtsConfig(%this) {
	%filename = filePath( %this.path ) @ "/" @ fileBase( %this.path ) @ ".cfg";
	%fo = new FileObject();

	if ( %fo.openForWrite( %filename ) ) {
		// AlwaysImport
		%fo.writeLine("AlwaysExport:");
		%alwaysImport = trim( strreplace( %this-->alwaysImport.getText(), ";", "\t" ) );
		%count = getFieldCount( %alwaysImport );

		for (%i = 0; %i < %count; %i++)
			%fo.writeLine( getField( %alwaysImport, %i ) );

		%fo.writeLine("");
		// NeverImport
		%fo.writeLine("NeverExport:");
		%neverImport = trim( strreplace( %this-->neverImport.getText(), ";", "\t" ) );
		%count = getFieldCount( %neverImport );

		for (%i = 0; %i < %count; %i++)
			%fo.writeLine( getField( %neverImport, %i ) );

		%fo.writeLine("");
		%fo.close();
	} else {
		error( "Failed to open " @ %filename @ " for writing" );
	}

	%fo.delete();
}

function ColladaImportDlg::updateOverrideUpAxis(%this, %override) {
	%this-->overrideUpAxis.setStateOn(%override);
	%this-->upAxis.setActive(%override);

	if (!%override)
		%this-->upAxis.setText(ColladaImportTreeView._upAxis);
}

function ColladaImportDlg::updateOverrideScale(%this, %override) {
	%this-->overrideScale.setStateOn(%override);
	%this-->scale.setActive(%override);

	if (!%override)
		%this-->scale.setText(ColladaImportTreeView._unit);
}

function ColladaImportTreeView::refresh(%this, %what) {
	%shapeRoot = %this.getFirstRootItem();
	%materialsRoot = %this.getNextSibling(%shapeRoot);
	%animRoot = %this.getNextSibling(%materialsRoot);

	// Refresh nodes
	if ((%what $= "all") || (%what $= "nodes")) {
		// Indicate whether nodes will be ignored on import
		%this._alwaysImport = strreplace(ColladaImportDlg-->alwaysImport.getText(), ";", "\t");
		%this._neverImport = strreplace(ColladaImportDlg-->neverImport.getText(), ";", "\t");
		%this._alwaysImportMesh = strreplace(ColladaImportDlg-->alwaysImportMesh.getText(), ";", "\t");
		%this._neverImportMesh = strreplace(ColladaImportDlg-->neverImportMesh.getText(), ";", "\t");
		%this.refreshNode(%this.getChild(%shapeRoot));
	}

	// Refresh materials
	if ((%what $= "all") || (%what $= "materials")) {
		%matPrefix = ColladaImportDlg-->materialPrefix.getText();
		%id = %this.getChild(%materialsRoot);

		while (%id > 0) {
			%baseName = %this.getItemValue(%id);
			%name = %matPrefix @ %baseName;
			// Indicate whether material name is already mapped
			%this.editItem(%id, %name, %baseName);
			%mapped = getMaterialMapping(%name);

			if (%mapped $= "") {
				%this.setItemTooltip(%id, "A new material will be mapped to this name");
				%this.setItemImages(%id, %this._imageMaterial, %this._imageMaterial);
			} else {
				%this.setItemTooltip(%id, %mapped SPC "is already mapped to this material name");
				%this.setItemImages(%id, %this._imageExMaterial, %this._imageExMaterial);
			}

			%id = %this.getNextSibling(%id);
		}
	}

	// Refresh animations
	if ((%what $= "all") || (%what $= "animations")) {
		%id = %this.getChild(%animRoot);

		while (%id > 0) {
			%this.setItemImages(%id, %this._imageAnim, %this._imageAnim);
			%id = %this.getNextSibling(%id);
		}
	}
}

function ColladaImportTreeView::refreshNode(%this, %id) {
	while (%id > 0) {
		switch$ (%this.getItemValue(%id)) {
		case "mesh":

			// Check if this mesh will be ignored on import
			if (strIsMatchMultipleExpr(%this._alwaysImportMesh, %this.getItemText(%id)) ||
					!strIsMatchMultipleExpr(%this._neverImportMesh, %this.getItemText(%id)) ) {
				%this.setItemTooltip(%id, "");
				%this.setItemImages(%id, %this._imageMesh, %this._imageMesh);
			} else {
				%this.setItemTooltip(%id, "This mesh will be ignored on import");
				%this.setItemImages(%id, %this._imageExNode, %this._imageExNode);
			}

		case "light":
			%this.setItemImages(%id, %this._imageLight, %this._imageLight);

		case "node":

			// Check if this node will be ignored on import
			if (strIsMatchMultipleExpr(%this._alwaysImport, %this.getItemText(%id)) ||
					!strIsMatchMultipleExpr(%this._neverImport, %this.getItemText(%id)) ) {
				%this.setItemTooltip(%id, "");
				%this.setItemImages(%id, %this._imageNode, %this._imageNode);
			} else {
				%this.setItemTooltip(%id, "This node will be ignored on import");
				%this.setItemImages(%id, %this._imageExNode, %this._imageExNode);
			}
		}

		// recurse through children and siblings
		%this.refreshNode(%this.getChild(%id));
		%id = %this.getNextSibling(%id);
	}
}

function ColladaImportDlg::onCancel(%this) {
	ColladaImportDlg.exportPrefs();
	Canvas.popDialog(%this);
	ColladaImportTreeView.clear();
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

function ColladaImportDlg::loadLights(%this) {
	// Get the ID of the last object added
	%obj = MissionGroup.getObject(MissionGroup.getCount()-1);
	// Create a new SimGroup to hold the model and lights
	%group = new SimGroup();
	loadColladaLights(%this.path, %group, %obj);

	// Delete the SimGroup if no lights were found. Otherwise, add the model to
	// the group as well.
	if (%group.getCount() > 0) {
		%group.add(%obj);
		%group.bringToFront(%obj);
		MissionGroup.add(%group);

		if (SceneEditorTree.isVisible()) {
			SceneEditorTree.removeItem(SceneEditorTree.findItemByObjectId(%obj));
			SceneEditorTree.buildVisibleTree(true);
		}
	} else {
		%group.delete();
	}
}

function updateTSShapeLoadProgressUI(%progress, %msg) {
	// Check if the loading GUI is visible and use that instead of the
	// separate import progress GUI if possible
	if ( isObject(DlgLoadingLevel) && DlgLoadingLevel.isAwake() ) {
		%loadProgressCtrl = DlgLoadingLevel-->LoadingProgress;
		%loadProgressTxtCtrl = DlgLoadingLevel-->LoadingProgressTxt;

		// Save/Restore load progress at the start/end of the import process
		if ( %progress == 0 ) {
			ColladaImportProgress.savedProgress = %loadProgressCtrl.getValue();
			ColladaImportProgress.savedText = %loadProgressTxtCtrl.getValue();
			ColladaImportProgress.msgPrefix = "Importing " @ %msg;
			%msg = "Reading file into memory...";
		} else if ( %progress == 1.0 ) {
			DlgLoadingLevel-->LoadingProgress.setValue( ColladaImportProgress.savedProgress );
			DlgLoadingLevel-->LoadingProgressTxt.setValue( ColladaImportProgress.savedText );
		}

		%msg = ColladaImportProgress.msgPrefix @ ": " @ %msg;
		%progressCtrl = DlgLoadingLevel-->LoadingProgress;
		%textCtrl = DlgLoadingLevel-->LoadingProgressTxt;
	} else {
		// Show/Hide gui at the start/end of the import process
		if ( %progress == 0 ) {
			ColladaImportProgress-->window.text = "Importing" SPC %msg;
			%msg = "Reading file into memory...";
			Canvas.pushDialog(ColladaImportProgress);
		} else if ( %progress == 1.0 ) {
			Canvas.popDialog(ColladaImportProgress);
		}

		%progressCtrl = ColladaImportProgress-->progressBar;
		%textCtrl = ColladaImportProgress-->progressText;
	}

	// Update progress indicators
	if (%progress == 0) {
		%progressCtrl.setValue(0.001);
		%textCtrl.setText(%msg);
	} else if (%progress != 1.0) {
		%progressCtrl.setValue(%progress);
		%textCtrl.setText(%msg);
	}

	Canvas.repaint(33);
}


// Convert all COLLADA models that match the given pattern (defaults to *) to DTS
function convertColladaModels(%pattern) {
	// Force loading the COLLADA file (to ensure cached DTS is updated)
	$collada::forceLoadDAE = true;
	%fullPath = findFirstFile("*.dae");

	while (%fullPath !$= "") {
		// Check if this file is inside the given path
		%fullPath = makeRelativePath(%fullPath, getMainDotCSDir());

		if ((%pattern $= "") || strIsMatchMultipleExpr(%pattern, %fullPath)) {
			// Load the model by creating a temporary TSStatic
			echo("Converting " @ %fullPath @ " to DTS...");
			%temp = new TSStatic() {
				shapeName = %fullPath;
				collisionType = "None";
			};
			%temp.delete();
		}

		%fullPath = findNextFile("*.dae");
	}

	$collada::forceLoadDAE = false;
}
