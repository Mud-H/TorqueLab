//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

$Lab_REP_DefaultNodeWidthRange = "0 50";
//==============================================================================
// Road Editor Params - Used set default settings and build plugins options GUI
//==============================================================================
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function RoadEditorPlugin::initParamsArray( %this,%cfgArray ) {
	%cfgArray.group[%gId++] = "General settings";
	%cfgArray.setVal("DefaultWidth",       "10" TAB "Default Width" TAB "SliderEdit" TAB "range::0 100;tickAt 1" TAB "RoadEditorGui" TAB %gId);
	%cfgArray.setVal("borderMovePixelSize",       "20" TAB "borderMovePixelSize" TAB "TextEdit" TAB "" TAB "RoadEditorGui" TAB %gId);
	%cfgArray.setVal("borderMoveSpeed",       "0.1" TAB "borderMoveSpeed" TAB "TextEdit" TAB "" TAB "RoadEditorGui" TAB %gId);
	%cfgArray.setVal("MaterialName",       "DefaultDecalRoadMaterial" TAB "MaterialName" TAB "TextEdit" TAB "" TAB "RoadEditorGui" TAB %gId);
	%cfgArray.group[%gId++] = "Color settings";
	%cfgArray.setVal("HoverSplineColor",   "255 0 0 255" TAB "HoverSplineColor" TAB "TextEdit" TAB "" TAB "RoadEditorGui" TAB %gId);
	%cfgArray.setVal("SelectedSplineColor","0 255 0 255" TAB "SelectedSplineColor" TAB "TextEdit" TAB "" TAB "RoadEditorGui" TAB %gId);
	%cfgArray.setVal("HoverNodeColor",       "255 255 255 255" TAB "HoverNodeColor" TAB "TextEdit" TAB "" TAB "RoadEditorGui" TAB %gId);
	%cfgArray.group[%gId++] = "Console settings";
	%cfgArray.setVal("consoleFrameColor",       "255 0 0 255" TAB "consoleFrameColor" TAB "TextEdit" TAB "" TAB "RoadEditorGui" TAB %gId);
	%cfgArray.setVal("consoleFillColor",       "0 0 0 0" TAB "consoleFillColor" TAB "TextEdit" TAB "" TAB "RoadEditorGui" TAB %gId);
	%cfgArray.setVal("consoleSphereLevel",       "1" TAB "consoleSphereLevel" TAB "TextEdit" TAB "" TAB "RoadEditorGui" TAB %gId);
	%cfgArray.setVal("consoleCircleSegments",       "32" TAB "consoleCircleSegments" TAB "TextEdit" TAB "" TAB "RoadEditorGui" TAB %gId);
	%cfgArray.setVal("consoleLineWidth",       "1" TAB "consoleLineWidth" TAB "TextEdit" TAB "" TAB "RoadEditorGui" TAB %gId);
}
//------------------------------------------------------------------------------

//==============================================================================
// Plugin Object Callbacks - Called from TLab plugin management scripts
//==============================================================================

//==============================================================================
// Called when TorqueLab is launched for first time
function RoadEditorPlugin::onPluginLoaded( %this ) {	
	// Add ourselves to the Editor Settings window
}
//------------------------------------------------------------------------------
//==============================================================================
function RoadEditorPlugin::onActivated( %this ) {
	RoadEd_TabBook.selectPage(0);
	LabPaletteArray->RoadEditorAddRoadMode.performClick();
	EditorGui.bringToFront( RoadEditorGui );
	RoadEditorGui.makeFirstResponder( true );
	RoadTreeView.open(ServerDecalRoadSet,true);
	// Set the status bar here until all tool have been hooked up
	EditorGuiStatusBar.setInfo("Road editor.");
	EditorGuiStatusBar.setSelection("");
	RoadEditorGui.prepSelectionMode();
	RoadManager.updateRoadData();
	Parent::onActivated(%this);
	
	RoadEd_TabBook.selectPage(0);
}
//------------------------------------------------------------------------------
//==============================================================================
function RoadEditorPlugin::onDeactivated( %this ) {
	Parent::onDeactivated(%this);
}
//------------------------------------------------------------------------------
//==============================================================================
function RoadEditorPlugin::onEditMenuSelect( %this, %editMenu ) {
	%hasSelection = false;

	if( isObject( RoadEditorGui.road ) )
		%hasSelection = true;

	%editMenu.enableItem( 3, false ); // Cut
	%editMenu.enableItem( 4, false ); // Copy
	%editMenu.enableItem( 5, false ); // Paste
	%editMenu.enableItem( 6, %hasSelection ); // Delete
	%editMenu.enableItem( 8, false ); // Deselect
}
//------------------------------------------------------------------------------
//==============================================================================
function RoadEditorPlugin::handleDelete( %this ) {
	RoadEditorGui.onDeleteKey();
}
//------------------------------------------------------------------------------
//==============================================================================
function RoadEditorPlugin::handleEscape( %this ) {
	devLog("RoadEditorPlugin::handleEscape");
	return RoadEditorGui.onEscapePressed();
}
//------------------------------------------------------------------------------
//==============================================================================
function RoadEditorPlugin::isDirty( %this ) {
	return RoadEditorGui.isDirty;
}
//------------------------------------------------------------------------------
//==============================================================================
function RoadEditorPlugin::onSaveMission( %this, %missionFile ) {
	if( RoadEditorGui.isDirty ) {
		MissionGroup.save( %missionFile );
		RoadEditorGui.isDirty = false;
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function RoadEditorPlugin::setEditorFunction( %this ) {
	%terrainExists = parseMissionGroup( "TerrainBlock" );

	if( %terrainExists == false )
		LabMsgYesNoCancel("No Terrain","Would you like to create a New Terrain?", "Canvas.pushDialog(CreateNewTerrainGui);");

	return %terrainExists;
}
//------------------------------------------------------------------------------
