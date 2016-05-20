//==============================================================================
// TorqueLab -> ShapeLab -> Shape Selection
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================



//==============================================================================
// Handle a selection in the MissionGroup shape selector
function SLE_ShapeOptionsBook::onTabSelected( %this, %text,%id ) {
	logd("SLE_ShapeOptionsBook::onTabSelected( %this, %text,%id )", %this,%text,%id );
	ShapeLab.currentShapeOptionsPage = %id;
}
//------------------------------------------------------------------------------
//==============================================================================
// Handle a selection in the MissionGroup shape selector
function ShapeLabPlugin::addFileBrowserMesh( %this, %file,%createCmd ) {
	devLog("ShapeLabPlugin::addFileBrowserMesh( %this, %file,%createCmd )", %this, %file,%createCmd );
	ShapeLab.selectFilePath(%file);
}
//------------------------------------------------------------------------------
//==============================================================================
function ShapeLabPlugin::openShape( %this, %path, %discardChangesToCurrent ) {
//    Lab.setEditor( ShapeLabPlugin );
	if( ShapeLab.isDirty() && !%discardChangesToCurrent ) {
		LabMsgYesNoCancel( "ShapeLab Save Changes?",
						 "Save changes to current shape?",
						 "ShapeLab.saveChanges(); ShapeLabPlugin.openShape(\"" @ %path @ "\");",
						 "ShapeLabPlugin.openShape(\"" @ %path @ "\",true);" );
		return;
	}

	ShapeLab.selectShape( %path );
	ShapeLabShapeView.fitToShape();
}
//------------------------------------------------------------------------------
//==============================================================================
// Handle a selection in the shape selector list
function ShapeLab::selectFilePath( %this, %path ) {
	ShapeLabPlugin.openShape(%path);
	return;
	// Prompt user to save the old shape if it is dirty
	if ( ShapeLab.isDirty() ) {
		%cmd = "ColladaImportDlg.showDialog( \"" @ %path @ "\", \"ShapeLab.selectShape( \\\"" @ %path @ "\\\", ";
		LabMsgYesNoCancel( "selectFilePath Shape Modified", "Would you like to save your changes?", %cmd @ "true );\" );", %cmd @ "false );\" );" );
	} else {
		%cmd = "ShapeLab.selectShape( \"" @ %path @ "\", false );";
		ColladaImportDlg.showDialog( %path, %cmd );
	}
}

//------------------------------------------------------------------------------
//==============================================================================
// Handle a selection in the MissionGroup shape selector
function ShapeLabPlugin::onSelectObject( %this, %obj ) {
	devLog("ShapeLabPlugin::onSelectObject( %this, %obj )", %this, %obj );
	%path = ShapeLab.getObjectShapeFile( %obj );

	if ( %path !$= "" )
		ShapeLabPropWindow.onSelect( %path );

	// Set the object type (for required nodes and sequences display)
	%objClass = %obj.getClassName();
	%hintId = -1;
	%count = ShapeLabHintGroup.getCount();

	for ( %i = 0; %i < %count; %i++ ) {
		%hint = ShapeLabHintGroup.getObject( %i );

		if ( %objClass $= %hint.objectType ) {
			%hintId = %hint;
			break;
		} else if ( isMemberOfClass( %objClass, %hint.objectType ) ) {
			%hintId = %hint;
		}
	}

	ShapeLabHintMenu.setSelected( %hintId );
}
//------------------------------------------------------------------------------
/*//==============================================================================
// Handle a selection in the MissionGroup shape selector
function ShapeLabPlugin::onSceneTreeSelected( %this, %obj ) {
	devLog("ShapeLabPlugin::onSceneTreeSelected( %this, %obj )",%this, %obj );
	%path = ShapeLab.getObjectShapeFile( %obj );
	if (%path !$= "")
		ShapeLabPlugin.openShape(%path);
	
}
//------------------------------------------------------------------------------*/
//==============================================================================
// Select Object Functions
//==============================================================================
//ShapeLab.shape = ShapeLab.findConstructor("art/Packs/AI/DroidSphere/GlitchTest.dts"); );
//==============================================================================
// Select the current WorldEditor selection
function ShapeLab::selectWorldEditorShape( %this) {
	%count = EWorldEditor.getSelectionSize();

	for (%i = 0; %i < %count; %i++) {
		%obj = EWorldEditor.getSelectedObject(%i);
		%shapeFile = ShapeLab.getObjectShapeFile(%obj);

		//If we have a valid shapefile, make the object the current selection
		if (%shapeFile !$= "") {
			//if (!isObject(ShapeLab.shape) || (ShapeLab.shape.baseShape !$= %shapeFile)) {
			//Clear the tree in case and make the current object selected
			//Scene.selectObject(%obj,true);
			//ShapeLabShapeTreeView.clearSelection();
			//ShapeLabShapeTreeView.onSelect(%obj);
			//Set the Editor shape
			ShapeLab.selectShape(%shapeFile, ShapeLab.isDirty());
			// 'fitToShape' only works after the GUI has been rendered, so force a repaint first
			Canvas.repaint();
			ShapeLabShapeView.fitToShape();
			break; //Only one shape can be selected at a time so leave
			//}
		}
	}
}
//------------------------------------------------------------------------------
//==============================================================================
// Handle a selection in the shape selector list
function ShapeLabPropWindow::onSelect( %this, %path ) {
	// Prompt user to save the old shape if it is dirty
	if (ShapeLab.currentFilePath $= %path)
	{
		devLog("Shape is already selected");
	//	return;
	}
		
	if ( ShapeLab.isDirty() ) {
		%cmd = "ColladaImportDlg.showDialog( \"" @ %path @ "\", \"ShapeLab.selectShape( \\\"" @ %path @ "\\\", ";
		LabMsgYesNoCancel( "onSelect Shape Modified", "Would you like to save your changes?", %cmd @ "true );\" );", %cmd @ "false );\" );" );
	} else {
		%cmd = "ShapeLab.selectShape( \"" @ %path @ "\", false );";
		ColladaImportDlg.showDialog( %path, %cmd );
	}
}

//------------------------------------------------------------------------------
//==============================================================================
function ShapeLab::selectShape( %this, %path, %saveOld ) {
	if (ShapeLab.currentFilePath $= %path){
		devLog("Shape is alreadySelected");
		//return;
	}
	
	if ( %saveOld ) {
		// Save changes to a TSShapeConstructor script
		%this.saveChanges();
	} else if ( ShapeLab.isDirty() ) {
		// Purge all unsaved changes
		%oldPath = ShapeLab.shape.baseShape;
		ShapeLab.shape.delete();
		ShapeLab.shape = 0;
		reloadResource( %oldPath );   // Force game objects to reload shape
	}


	ShapeLabShapeView.setModel( "" );
	ShapeLab.currentFileName = "";
	ShapeLab.selectedSequence = "";
	
	
	devLog("Path is:",%path);
	%path = strreplace(%path,"//","/");
	devLog("FIXEDPath is:",%path);
	
	// Initialise the shape preview window
	if ( !ShapeLabShapeView.setModel( %path ) ) {
		LabMsgOK( "Error", "Failed to load '" @ %path @ "'. Check the console for error messages." );
		return;
	}
	ShapeLab.currentFilePath = %path;
	ShapeLab.currentSeqPath = "";
ShapeLab.currentFileName = fileBase(%path)@fileExt(%path);
	ShapeLabShapeView.fitToShape();
	ShapeLabUndoManager.clearAll();
	ShapeLab.setDirty( false );
	// Get ( or create ) the TSShapeConstructor object for this shape
	ShapeLab.shape = ShapeLab.findConstructor(%path );

	if ( ShapeLab.shape <= 0 ) {
		ShapeLab.shape = %this.createConstructor( %path );

		if ( ShapeLab.shape <= 0 ) {
			error( "ShapeLab: Error - could not select " @ %path );
			return;
		}
	}
	// Initialise the editor windows	
	ShapeLabDetails.selectedShapeChanged();
	ShapeLabMountWindow.onShapeSelectionChanged();	
	
	ShapeLabCollisions.onShapeSelectionChanged();
	ShapeLabPropWindow.onShapeSelectionChanged();
	ShapeLabShapeView.onShapeSelectionChanged();
	
	
	// Update object type hints
	ShapeLabHints.updateHints();
	// Update editor status bar
	EditorGuiStatusBar.setSelection( %path );
}
//------------------------------------------------------------------------------
/*
//==============================================================================
// Handle a selection in the MissionGroup shape selector
function ShapeLabShapeTreeView::onSelect( %this, %obj ) {
	%path = ShapeLab.getObjectShapeFile( %obj );

	if ( %path !$= "" )
		ShapeLabSelectWindow.onSelect( %path );

	// Set the object type (for required nodes and sequences display)
	%objClass = %obj.getClassName();
	%hintId = -1;
	%count = ShapeLabHintGroup.getCount();

	for ( %i = 0; %i < %count; %i++ ) {
		%hint = ShapeLabHintGroup.getObject( %i );

		if ( %objClass $= %hint.objectType ) {
			%hintId = %hint;
			break;
		} else if ( isMemberOfClass( %objClass, %hint.objectType ) ) {
			%hintId = %hint;
		}
	}

	ShapeLabHintMenu.setSelected( %hintId );
}
//------------------------------------------------------------------------------
*/
//==============================================================================
// Open a Shape file
//==============================================================================

//==============================================================================
/*
function ShapeLabPlugin::open(%this, %filename) {


	// Select the new shape
	if (isObject(ShapeLab.shape) && (ShapeLab.shape.baseShape $= %filename)) {
		// Shape is already selected => re-highlight the selected material if necessary
		ShapeLabMaterials.updateSelectedMaterial(ShapeLabMaterials-->highlightMaterial.getValue());
	} else if (%filename !$= "") {
		ShapeLab.selectShape(%filename, ShapeLab.isDirty());
		// 'fitToShape' only works after the GUI has been rendered, so force a repaint first
		Canvas.repaint();
		ShapeLabShapeView.fitToShape();
	}
}
*/
//==============================================================================
// Open new shape
//==============================================================================


//==============================================================================
// Shape Constructor Functions
//==============================================================================
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



// Update the GUI in response to the shape selection changing
function ShapeLabPropWindow::onShapeSelectionChanged( %this ) {
	// --- NODES TAB ---
	ShapeLab_NodeTree.removeItem( 0 );
	%rootId = ShapeLab_NodeTree.insertItem( 0, "<root>", 0, "" );
	%count = ShapeLab.shape.getNodeCount();

	for ( %i = 0; %i < %count; %i++ ) {
		%name = ShapeLab.shape.getNodeName( %i );

		if ( ShapeLab.shape.getNodeParentName( %name ) $= "" )
			ShapeLab_NodeTree.addNodeTree( %name );
	}
	
	ShapeLab.setActiveNode("");	
	//ShapeLab.onNodeSelectionChanged( -1 );    // no node selected
	// --- SEQUENCES TAB ---
	ShapeLab.updateShapeSequenceData();
	
	/*
	ShapeLab_SeqPillStack.clear(); //Clear the new sequence lisitng stack
	ShapeLabSequenceList.clear();
	ShapeLabSequenceList.addRow( -1, "Name" TAB "Cyclic" TAB "Blend" TAB "Frames" TAB "Priority" );
	ShapeLabSequenceList.setRowActive( -1, false );
	ShapeLabSequenceList.addRow( 0, "<rootpose>" TAB "" TAB "" TAB "" TAB "" );
	%count = ShapeLab.shape.getSequenceCount();

	for ( %i = 0; %i < %count; %i++ ) {
		%name = ShapeLab.shape.getSequenceName( %i );

		// Ignore __backup__ sequences (only used by editor)
		if ( !startswith( %name, "__backup__" ) ) {
			ShapeLabSequenceList.addItem( %name );
			ShapeLab.addSequencePill(%name);
		}
	}

	*/
	

	// --- MATERIALS TAB ---
	ShapeLab.updateMaterialList();
}
