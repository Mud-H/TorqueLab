//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// Scene Editor Params - Used set default settings and build plugins options GUI
//==============================================================================
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function ShapeLabPlugin::initParamsArray( %this,%array ) {
	$SceneEdCfg = newScriptObject("ShapeLabCfg");
	%array.group[%gId++] = "General settings";	
	%array.setVal("PreviewColorBG",     "0 0 0 0.9"   TAB "BackgroundColor" TAB "ColorEdit" TAB "" TAB "ShapeLabPreviewGui-->previewBackground.setColor(*val*);" TAB %gid);
	%array.setVal("HighlightMaterial",   "1" TAB "HighlightMaterial" TAB "TextEdit" TAB "" TAB "" TAB %gId);
	%array.setVal("ShowNodes","1" TAB "ShowNodes" TAB "TextEdit" TAB "" TAB "" TAB %gId);
	%array.setVal("ShowBounds",       "0" TAB "ShowBounds" TAB "TextEdit" TAB "" TAB "" TAB %gId);
	%array.setVal("ShowObjBox",       "1" TAB "ShowObjBox" TAB "TextEdit" TAB "" TAB "" TAB %gId);
	%array.setVal("RenderMounts",       "1" TAB "RenderMounts" TAB "TextEdit" TAB "" TAB "" TAB %gId);
	%array.setVal("RenderCollision",       "0" TAB "RenderCollision" TAB "TextEdit" TAB "" TAB "" TAB %gId);
	%array.setVal("AdvancedWindowVisible",       "1" TAB "RenderCollision" TAB "TextEdit" TAB "" TAB "" TAB %gId);
	%array.setVal("AnimationBarVisible",       "1" TAB "RenderCollision" TAB "TextEdit" TAB "" TAB "" TAB %gId);
	%array.group[%gId++] = "Grid settings";
	%array.setVal("ShowGrid",       "1" TAB "ShowGrid" TAB "TextEdit" TAB "" TAB "ShapeLabShapeView" TAB %gId);
	%array.setVal("GridSize",       "0.1" TAB "GridSize" TAB "TextEdit" TAB "" TAB "ShapeLabShapeView" TAB %gId);
	%array.setVal("GridDimension",       "40 40" TAB "GridDimension" TAB "TextEdit" TAB "" TAB "ShapeLabShapeView" TAB %gId);
	%array.group[%gId++] = "Sun settings";
	%array.setVal("SunDiffuseColor",       "255 255 255 255" TAB "SunDiffuseColor" TAB "TextEdit" TAB "" TAB "ShapeLabShapeView" TAB %gId);
	%array.setVal("SunAmbientColor",       "180 180 180 255" TAB "SunAmbientColor" TAB "TextEdit" TAB "" TAB "ShapeLabShapeView" TAB %gId);
	%array.setVal("SunAngleX",       "45" TAB "SunAngleX" TAB "TextEdit" TAB "" TAB "ShapeLabShapeView" TAB %gId);
	%array.setVal("SunAngleZ",       "135" TAB "SunAngleZ" TAB "TextEdit" TAB "" TAB "ShapeLabShapeView" TAB %gId);
}
//------------------------------------------------------------------------------

//==============================================================================
// Plugin Object Callbacks - Called from TLab plugin management scripts
//==============================================================================

//==============================================================================
// Called when TorqueLab is launched for first time
function ShapeLabPlugin::onPluginLoaded( %this ) {	
	// Add ourselves to the Editor Settings window
	ShapeLabPreview.resize( -1, 526, 593, 53 );
	// Initialise gui

	//ShapeLabAdvancedWindow-->tabBook.selectPage(0);
	//ShapeLabPropWindow-->tabBook.selectPage(0);

	ShapeLabToggleButtonValue( ShapeLabToolbar-->orbitNodeBtn, 0 );
	ShapeLabToggleButtonValue( ShapeLabToolbar-->ghostMode, 0 );
	// Initialise hints menu
	ShapeLabHintMenu.clear();
	%count = ShapeLabHintGroup.getCount();
	ShapeLabThreadViewer.threadID = -1;
	for (%i = 0; %i < %count; %i++) {
		%hint = ShapeLabHintGroup.getObject(%i);
		ShapeLabHintMenu.add(%hint.objectType, %hint);
	}

	if ( !%this.isGameReady ) {
		// Activate the Shape Lab
		// Lab.setEditor( %this, true );
		// Get editor settings (note the sun angle is not configured in the settings
		// dialog, so apply the settings here instead of in readSettings)
		ShapeLabPreviewGui.fitIntoParents();
		ShapeLabPreviewGui-->previewBackground.fitIntoParents();
		ShapeLabShapeView.fitIntoParents();
		$wasInWireFrameMode = $gfx::wireframe;
		ShapeLabToolbar-->wireframeMode.setStateOn($gfx::wireframe);

		if ( GlobalGizmoProfile.getFieldValue(alignment) $= "Object" )
			ShapeLabNodes-->object.setStateOn(1);
		else
			ShapeLabNodes-->world.setStateOn(1);

		// Initialise and show the shape editor
		//ShapeLabShapeTreeView.open(MissionGroup);
		//ShapeLabShapeTreeView.buildVisibleTree(true);
		EditorGui.bringToFront(ShapeLabPreviewGui);
		LabPaletteArray->WorldEditorMove.performClick();
		// Switch to the ShapeLab UndoManager
		//%this.oldUndoMgr = Editor.getUndoManager();
		//Editor.setUndoManager( ShapeLabUndoManager );
		ShapeLabShapeView.setDisplayType( Lab.cameraDisplayType );
		%this.initStatusBar();
		// Customise menu bar
		%this.oldCamFitCmd = %this.replaceMenuCmd( "Camera", 8, "ShapeLabShapeView.fitToShape();" );
		%this.oldCamFitOrbitCmd = %this.replaceMenuCmd( "Camera", 9, "ShapeLabShapeView.fitToShape();" );
	}

	%this.isGameReady = true;
	ShapeLabPreviewSwatch.setColor("0 0 0 0.9");
	ShapeLab.currentShapeOptionsPage = 0;
	ShapeLab.currentMainOptionsPage = 0;
	ShapeLab.currentAnimOptionsPage = 0;
		SLE_MainOptionsBook.selectPage(0);
	
}
//------------------------------------------------------------------------------
//==============================================================================
// Called when the Plugin is activated (Active TorqueLab plugin)
$SimpleActivation = false;
function ShapeLabPlugin::onActivated(%this) {
	if ( !isObject( ShapeLabUndoManager ) )
	{
		ShapeLab.undoManager = new UndoManager( ShapeLabUndoManager ) {
			numLevels = 200;
			internalName = "ShapeLabUndoManager";
		};	
	}
	//show(ShapeLabSelectWindow);
	//show(ShapeLabPropWindow);
	//Assign the Camera fit to the GuiShapeLabPreview
	Lab.fitCameraGui = ShapeLabShapeView;
	ShapeLab.initTriggerList();
	// Try to start with the shape selected in the world editor
	//if (!isObject(ShapeLab.shape))
		ShapeLab.selectWorldEditorShape();
	ShapeLabPlugin.updateAnimBar();
	ShapeLab.initCollisionPage();
	Parent::onActivated(%this,$SimpleActivation);
	ShapeLab.setDirty(ShapeLab.isDirty());
	SLE_MainOptionsBook.selectPage(ShapeLab.currentMainOptionsPage);
	SLE_ShapeOptionsBook.selectPage(ShapeLab.currentShapeOptionsPage);
	SLE_AnimOptionsBook.selectPage(ShapeLab.currentAnimOptionsPage);
	%this.oldUndoMgr = Editor.getUndoManager();
		Editor.setUndoManager( ShapeLabUndoManager );
}
//------------------------------------------------------------------------------
//==============================================================================
// Called when the Plugin is deactivated (active to inactive transition)
function ShapeLabPlugin::onDeactivated(%this) {
	// Notify game objects if shape has been modified
	if ( ShapeLab.isDirty() )
		ShapeLab.shape.notifyShapeChanged();

	$gfx::wireframe = $wasInWireFrameMode;
	ShapeLabMaterials.updateSelectedMaterial(false);

	if( EditorGui-->MatEdPropertiesWindow.visible ) {
		ShapeLabMaterials.editSelectedMaterialEnd( true );
	}

	// Restore the original undo manager
	Editor.setUndoManager( %this.oldUndoMgr );
	// Restore menu bar
	%this.replaceMenuCmd( "Camera", 8, %this.oldCamFitCmd );
	%this.replaceMenuCmd( "Camera", 9, %this.oldCamFitOrbitCmd );
	Parent::onDeactivated(%this);
}
//------------------------------------------------------------------------------
//==============================================================================
// Called from TorqueLab after plugin is initialize to set needed settings
function ShapeLabPlugin::onPluginCreated( %this ) {
	
}
//------------------------------------------------------------------------------
//==============================================================================
// Called from TorqueLab when exitting the mission
function ShapeLabPlugin::onExitMission( %this ) {
	// unselect the current shape
	ShapeLabShapeView.setModel( "" );

	if (ShapeLab.shape != -1)
		delObj(ShapeLab.shape);

if (!isObject(ShapeLab))
	return;
	ShapeLab.shape = 0;
	
	ShapeLabUndoManager.clearAll();
	ShapeLab.setDirty( false );
	//ShapeLabSequenceList.clear();
	//ShapeLabNodeTreeView.removeItem( 0 );
	ShapeLab.setActiveNode("");		
	//ShapeLab.onNodeSelectionChanged( -1 );
	ShapeLab_DetailTree.removeItem( 0 );
	ShapeLabMaterialList.clear();
	ShapeLabMountWindow-->mountList.clear();
	//ShapeLabThreadList.clear();
	ShapeLab.clearNodeTree();
	ShapeLab_ThreadIdList.clear();
	ShapeLab_ThreadSeqList.clear();
}
//------------------------------------------------------------------------------
//==============================================================================




function ShapeLabPlugin::onPreSave( %this ) {
	ShapeLabShapeView.selectedNode = "-1";
	ShapeLabShapeView.selectedObject = "-1";
	ShapeLabShapeView.selectedObjDetail = "-1";
	ShapeLabShapeView.activeThread = "-1";
}


// Replace the command field in an Editor PopupMenu item (returns the original value)
function ShapeLabPlugin::replaceMenuCmd(%this, %menuTitle, %id, %newCmd) {
	if (!$Cfg_UI_Menu_UseNativeMenu) return;

	%menu = Lab.findMenu( %menuTitle );
	%cmd = getField( %menu.item[%id], 2 );
	%menu.setItemCommand( %id, %newCmd );
	return %cmd;
}


//==============================================================================
// Called when the Plugin is activated (Active TorqueLab plugin)
function ShapeLabPlugin::preparePlugin(%this) {
}
