//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// Plugin Object Params - Used set default settings and build plugins options GUI
//==============================================================================

//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function NavEditorPlugin::initParamsArray( %this,%array ) {
	$NavEditorCfg = newScriptObject("NavEditorCfg");
	%array.group[%groupId++] = "General settings";
	%array.setVal("renderMesh",       "1" TAB "renderMesh" TAB "checkbox"  TAB "" TAB "$Nav::Editor::renderMesh" TAB %groupId);
	%array.setVal("renderPortals",       "1" TAB "renderPortals" TAB "checkbox"  TAB "" TAB "$Nav::Editor::renderPortals" TAB %groupId);
	%array.setVal("renderBVTree",       "1" TAB "renderBVTree" TAB "checkbox"  TAB "" TAB "$Nav::Editor::renderBVTree" TAB %groupId);
	%array.setVal("spawnClass",       "AIPlayer" TAB "spawnClass" TAB "TextEdit"  TAB "" TAB "NavEditorGui" TAB %groupId);
	%array.setVal("spawnDatablock",       "DemoPlayerData" TAB "spawnDatablock" TAB "TextEdit"  TAB "" TAB "NavEditorGui" TAB %groupId);
	%array.setVal("backgroundBuild",       "1" TAB "backgroundBuild" TAB "checkbox"  TAB "" TAB "NavEditorGui" TAB %groupId);
	%array.setVal("saveIntermediates",       "1" TAB "saveIntermediates" TAB "checkbox"  TAB "" TAB "NavEditorGui" TAB %groupId);
	%array.setVal("playSoundWhenDone",       "1" TAB "playSoundWhenDone" TAB "checkbox"  TAB "" TAB "NavEditorGui" TAB %groupId);
}

//==============================================================================
// Plugin Object Callbacks - Called from TLab plugin management scripts
//==============================================================================
//------------------------------------------------------------------------------
// Material Editor


//==============================================================================
// Plugin Object Callbacks - Called from TLab plugin management scripts
//==============================================================================

//==============================================================================
// Called when TorqueLab is launched for first time
function NavEditorPlugin::onPluginLoaded( %this ) {	
	// Bind shortcuts for the nav editor.
	%map = new ActionMap();
	%map.bindCmd(keyboard, "1", "ENavEditorSelectModeBtn.performClick();", "");
	%map.bindCmd(keyboard, "2", "ENavEditorLinkModeBtn.performClick();", "");
	%map.bindCmd(keyboard, "3", "ENavEditorCoverModeBtn.performClick();", "");
	%map.bindCmd(keyboard, "4", "ENavEditorTileModeBtn.performClick();", "");
	%map.bindCmd(keyboard, "5", "ENavEditorTestModeBtn.performClick();", "");
	%map.bindCmd(keyboard, "c", "NavEditorConsoleBtn.performClick();", "");
	NavEditorPlugin.map = %map;
	NavEditorConsoleDlg.init();
	// Add items to World Editor Creator
	Scene.beginGroup("Navigation");
	Scene.registerMissionObject("CoverPoint", "Cover point");
	Scene.endGroup();
	$Nav::Editor::renderBVTree = 0;
	$Nav::Editor::renderPortals = 0;
	$Nav::Editor::renderMesh = 0;
	$CoverPoint::isRenderable = 0;
}
//------------------------------------------------------------------------------
//==============================================================================
// Called when the Plugin is activated (Active TorqueLab plugin)
function NavEditorPlugin::onActivated( %this ) {
	// Set a global variable so everyone knows we're editing!
	$Nav::EditorOpen = true;
	// Start off in Select mode.
	NavEditorGui.prepSelectionMode();
	NavEditorGui.makeFirstResponder(true);
	$Nav::Editor::renderBVTree = NavEditorPlugin.renderBVTree;
	$Nav::Editor::renderPortals = NavEditorPlugin.renderPortals;
	$Nav::Editor::renderMesh = NavEditorPlugin.renderMesh;
	$CoverPoint::isRenderable = NavEditorPlugin.renderCover;

	// Inspect the ServerNavMeshSet, which contains all the NavMesh objects
	// in the mission.
	if(!isObject(ServerNavMeshSet))
		new SimSet(ServerNavMeshSet);

	if(ServerNavMeshSet.getCount() == 0)
		MessageBoxYesNo("No NavMesh", "There is no NavMesh in this level. Would you like to create one?" SPC
							 "If not, please use the Nav Editor to create a new NavMesh.",
							 "Canvas.pushDialog(CreateNewNavMeshDlg);");

	NavTreeView.open(ServerNavMeshSet, true);
	// Push our keybindings to the top. (See initializeNavEditor for where this
	// map was created.)
	%this.map.push();
	// Store this on a dynamic field
	// in order to restore whatever setting
	// the user had before.
	%this.prevGizmoAlignment = GlobalGizmoProfile.alignment;
	// Always use Object alignment.
	GlobalGizmoProfile.alignment = "Object";
	// Set the status until some other editing mode adds useful information.
	EditorGuiStatusBar.setInfo("Navigation editor.");
	EditorGuiStatusBar.setSelection("");
	// Allow the Gui to setup.
	NavEditorGui.onEditorActivated();
	Parent::onActivated(%this);
	show(NavEditorConsoleDlg-->StatusLeft);
	show(NavEditorConsoleDlg-->OutputScroll);
}
//------------------------------------------------------------------------------

function NavEditorGui::onEditorActivated(%this) {
	if(%this.selectedObject)
		%this.selectObject(%this.selectedObject);

	%this.prepSelectionMode();
}

function NavEditorGui::onEditorDeactivated(%this) {
	if(%this.getMesh())
		%this.deselect();
}
//==============================================================================
// Called when the Plugin is deactivated (active to inactive transition)
function NavEditorPlugin::onDeactivated( %this ) {
	$Nav::EditorOpen = false;
	NavEditorPlugin.renderBVTree =  $Nav::Editor::renderBVTree;
	NavEditorPlugin.renderPortals =  $Nav::Editor::renderPortals;
	NavEditorPlugin.renderMesh =  $Nav::Editor::renderMesh;
	NavEditorPlugin.renderCover =  $CoverPoint::isRenderable;
	$Nav::Editor::renderBVTree = 0;
	$Nav::Editor::renderPortals = 0;
	$Nav::Editor::renderMesh = 0;
	$CoverPoint::isRenderable = 0;
	%this.map.pop();
	// Restore the previous Gizmo alignment settings.
	GlobalGizmoProfile.alignment = %this.prevGizmoAlignment;
	// Allow the Gui to cleanup.
	NavEditorGui.onEditorDeactivated();
	Parent::onDeactivated(%this);
}
//------------------------------------------------------------------------------
//==============================================================================
// Called from TorqueLab after plugin is initialize to set needed settings
function NavEditorPlugin::onPluginCreated( %this ) {
}
//------------------------------------------------------------------------------

//==============================================================================
// Called when the mission file has been saved
function NavEditorPlugin::onSaveMission( %this, %file ) {
	if(NavEditorGui.isDirty) {
		MissionGroup.save(%file);
		NavEditorGui.isDirty = false;
	}
}
//------------------------------------------------------------------------------
//==============================================================================
// Called when TorqueLab is closed
function NavEditorPlugin::onEditorSleep( %this ) {
}
//------------------------------------------------------------------------------
//==============================================================================
//Called when editor is selected from menu
function NavEditorPlugin::onEditMenuSelect( %this, %editMenu ) {
	WEditorPlugin.onEditMenuSelect( %editMenu );
}
//------------------------------------------------------------------------------

function NavEditorPlugin::handleDelete(%this) {
	// Event happens when the user hits 'delete'.
	NavEditorGui.deleteSelected();
}

function NavEditorPlugin::handleEscape(%this) {
	return NavEditorGui.onEscapePressed();
}

function NavEditorPlugin::isDirty(%this) {
	return NavEditorGui.isDirty;
}
