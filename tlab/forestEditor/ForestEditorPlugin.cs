//==============================================================================
// LabEditor ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// Forest Editor Params - Used set default settings and build plugins options GUI
//==============================================================================
function ForestEditorPlugin::initParamsArray( %this,%cfgArray ) {
	$ForestEditorCfg = newScriptObject("ForestEditorCfg");
	%cfgArray.group[%groupId++] = "General settings";
	%cfgArray.setVal("DefaultBrush",    "BaseBrush" TAB "DefaultBrush" TAB "TextEdit" TAB "" TAB "ForestEditorPlugin" TAB %groupId);
	%cfgArray.setVal("BrushPressure",       "2" TAB "Brush pressure" TAB "TextEdit" TAB "" TAB "ForestEditorCfg" TAB %groupId);
	%cfgArray.setVal("BrushSize",       "5" TAB "BrushSize" TAB "TextEdit" TAB "" TAB "ForestEditorCfg" TAB %groupId);
	%cfgArray.setVal("BrushHardness",       "2" TAB "BrushHardness" TAB "TextEdit" TAB "" TAB "ForestEditorCfg" TAB %groupId);
	%cfgArray.setVal("GlobalScale",       "1" TAB "GlobalScale" TAB "TextEdit" TAB "" TAB "ForestEditorCfg" TAB %groupId);
	%cfgArray.group[%groupId++] = "Default brush settings";
	%cfgArray.setVal("DefaultBrushPressure",    "20" TAB "Default brush pressure" TAB "TextEdit" TAB "" TAB "ForestEditorCfg" TAB %groupId);
	%cfgArray.setVal("DefaultBrushHardness",    "50" TAB "Default brush gardness" TAB "TextEdit" TAB "" TAB "ForestEditorCfg" TAB %groupId);
	%cfgArray.setVal("DefaultBrushSize",    "5" TAB "Default brush size" TAB "TextEdit" TAB "" TAB "ForestEditorCfg" TAB %groupId);
	%cfgArray.setVal("DefaultGlobalScale",    "1" TAB "Default global scale pressure" TAB "TextEdit" TAB "" TAB "ForestEditorCfg" TAB %groupId);
}
//==============================================================================
// Plugin Object Callbacks - Called from TLab plugin management scripts
//==============================================================================
//==============================================================================
// Called when TorqueLab is launched for first time
function ForestEditorPlugin::onPluginLoaded( %this ) {	
	new PersistenceManager( ForestDataManager );
	FEP_Manager.init();

	if (isObject(theForest) && ForestEditorGui.isMethod("setActiveForest"))
		ForestEditorGui.setActiveForest(theForest);

	if ( !isObject( ForestItemDataSet ) )
		new SimSet( ForestItemDataSet );

	if ( !isObject( ForestMeshGroup ) )
		new SimGroup( ForestMeshGroup );

	MissionCleanup.add(ForestMeshGroup);
	ForestEditMeshTree.open( ForestItemDataSet );
	ForestEditTabBook.selectPage(0);
	FEP_BrushManager.setBrushPressure();
	FEP_BrushManager.setBrushHardness();
	FEP_BrushManager.setBrushSize();
	FEP_BrushManager.setGlobalScale();
	
}

function ForestEditorPlugin::onWorldEditorShutdown( %this ) {
	if ( isObject( ForestBrushGroup ) )
		ForestBrushGroup.delete();

	if ( isObject( MissionForestBrushGroup ) )
		MissionForestBrushGroup.delete();

	if ( isObject( ForestDataManager ) )
		ForestDataManager.delete();
}
//------------------------------------------------------------------------------
//==============================================================================
// Called when the Plugin is activated (Active TorqueLab plugin)
function ForestEditorPlugin::onActivated( %this ) {
	//FEP_Manager.attachMissionBrushData();
	FEP_Manager.initBrushData();
	EditorGui.bringToFront( ForestEditorGui );
	ForestEditorGui.setVisible( true );
	ForestEditorGui.makeFirstResponder( true );
	//ForestEditToolbar.setVisible( true );
	Parent::onActivated(%this);
	ForestEditMeshTree.initTree();
	ForestEditBrushTree.initTree();
	// Open the Brush tab.
	ForestEditTabBook.selectPage(0);
	// Sync the pallete button state
	%forestBrushSize = %this.getCfg("BrushSize");
	%this.previousBrushSize = ETerrainEditor.getBrushSize();
	ETerrainEditor.setBrushSize(%forestBrushSize);
	// And toolbar.
	%tool = ForestEditorGui.getActiveTool();

	if ( isObject( %tool ) )
		%tool.onActivated();

	if ( !isObject( %tool ) ) {
		ForestEditorPaintModeBtn.performClick();

		if ( ForestEditBrushTree.getItemCount() > 0 ) {
			ForestEditBrushTree.selectItem( 0, true );
		}
	} else if ( %tool == ForestTools->SelectionTool ) {
		%mode = GlobalGizmoProfile.mode;

		switch$ (%mode) {
		case "None":
			ForestEditorSelectModeBtn.performClick();

		case "Move":
			ForestEditorMoveModeBtn.performClick();

		case "Rotate":
			ForestEditorRotateModeBtn.performClick();

		case "Scale":
			ForestEditorScaleModeBtn.performClick();
		}
	} else if ( %tool == ForestTools->BrushTool ) {
		%mode = ForestTools->BrushTool.mode;

		switch$ (%mode) {
		case "Paint":
			ForestEditorPaintModeBtn.performClick();

		case "Erase":
			ForestEditorEraseModeBtn.performClick();

		case "EraseSelected":
			ForestEditorEraseSelectedModeBtn.performClick();
		}
	}

	if ( %this.showError )
		LabMsgOK( "Error", "Your art/forest folder does not contain a valid brushes.cs. Brushes you create will not be saved!" );

	//Check if forest brushes settings are set, if not set defaults
	if (  ForestEditorPlugin.brushPressure $= "")
		FEP_BrushManager.setBrushPressure();

	if (  ForestEditorPlugin.brushHardness $= "")
		FEP_BrushManager.setBrushHardness();

	if (  ForestEditorPlugin.brushSize $= "")
		FEP_BrushManager.setBrushSize();

	if (  ForestEditorPlugin.globalScale $= "")
		FEP_BrushManager.setGlobalScale();	


}
//------------------------------------------------------------------------------
//==============================================================================
// Called when the Plugin is deactivated (active to inactive transition)
function ForestEditorPlugin::onDeactivated( %this ) {
	//FEP_Manager.detachMissionBrushData();
	ForestEditorGui.setVisible( false );
	ETerrainEditor.setBrushSize( this.previousBrushSize);
	%tool = ForestEditorGui.getActiveTool();

	if ( isObject( %tool ) )
		%tool.onDeactivated();

	// Also take this opportunity to save.
	ForestDataManager.saveDirty();
	Parent::onDeactivated(%this);
}
//------------------------------------------------------------------------------
//==============================================================================
// Called from TorqueLab after plugin is initialize to set needed settings
function ForestEditorPlugin::onPluginCreated( %this ) {

}
//------------------------------------------------------------------------------

//==============================================================================
// Called when the mission file has been saved
function ForestEditorPlugin::onNewLevelLoaded( %this, %file ) {
	if (isObject(theForest) && ForestEditorGui.isMethod("setActiveForest"))
		ForestEditorGui.setActiveForest(theForest);

	Parent::onNewLevelLoaded( %this, %file );
}
//------------------------------------------------------------------------------
function ForestEditorPlugin::isDirty( %this ) {
	%dirty = %this.dirty || ForestEditorGui.isDirty();
	return %dirty;
}

function ForestEditorPlugin::clearDirty( %this ) {
	%this.dirty = false;
}

function ForestEditorPlugin::onSaveMission( %this, %missionFile ) {
	ForestDataManager.saveDirty();

	if (isObject(theForest)) {
		%file = theForest.datafile;

		if (!isFile(%file))
			%file = filePath(theForest.getFilename())@"/data.forest";

		if (isFile(%file))
			theForest.saveDataFile(%file);
	}

	FEP_Manager.saveBrushData();
}


//==============================================================================
// Called when TorqueLab is closed
function ForestEditorPlugin::onEditorSleep( %this ) {
}
//------------------------------------------------------------------------------

