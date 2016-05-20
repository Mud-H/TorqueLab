//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
/*
%ar.setVal("FIELD",       "DEFAULT" TAB "NAME" TAB "CTRLTYPE" TAB "CTRLSETTINGS" TAB "UPDATESET" TAB GROUPID);
%ar.setVal("FIELD",       "DEFAULT" TAB "NAME" TAB "TextEdit" TAB "" TAB "" TAB %gid);
*/
//==============================================================================
$Cfg_TLab_Object_DropTypes = "atOrigin atCamera atCameraRot belowCamera screenCenter atCentroid toTerrain belowSelection";


function Lab::initCommonParams( %this ) {
//==============================================================================
//COMMON EDITOR SETTINGS
//==============================================================================
//------------------------------------------------------------------------------

$CommonControlMode = "Camera Player";
   %this.createParamsCategory("Common","Common Editor");
	//Binds and inputs
	/*	%gid = 0;
		%ar = %this.newParamsArray("Input","Common","");
		%ar.group[%gid++] = "Mouse settings";
		%ar.prefGroup = "$Mouse::";
		%ar.autoSyncPref = true;
		%ar.setVal("MouseSpeed",      "2.0" TAB "Mouse speed" TAB "SliderEdit" TAB "range>>0 1.5;;precision>>2" TAB "$Mouse::CameraSpeed" TAB %gid);
		%ar.setVal("MouseScrollSpeed",      "1" TAB "Mouse scroll speed" TAB "SliderEdit" TAB "range>>0 3.15;;precision>>2" TAB "" TAB %gid);
		*///General
		//Objects manipulation
	%gid = 0;
	//%ar = %this.newParamsArray("Objects","WorldEditor");
	%ar = %this.createParamsArray("Common","Objects" TAB "Scene Objects");
	%ar.group[%gid++] = "Scene Objects Options";
	%ar.setVal("gridSnap",       "0" TAB "Grid snapping" TAB "CheckBox" TAB "" TAB "EWorldEditor" TAB %gid);
	%ar.setVal("snapGround",       "0" TAB "snapGround" TAB "CheckBox" TAB "" TAB "EWorldEditor.stickToGround" TAB %gid);
	%ar.setVal("snapSoft",       "0" TAB "snapSoft" TAB "CheckBox" TAB "" TAB "EWorldEditor.setSoftSnap(*val*);" TAB %gid);
	%ar.setVal("snapSoftSize",       "2.0" TAB "snapSoftSize" TAB "TextEdit" TAB "" TAB "EWorldEditor.setSoftSnapSize(*val*);" TAB %gid);
	%ar.setVal("dropType",       "screenCenter" TAB "dropType" TAB "TextEdit" TAB "" TAB "EWorldEditor" TAB %gid);
	%ar.setVal("forceLoadDAE",       "0" TAB "forceLoadDAE" TAB "CheckBox" TAB "" TAB "Lab" TAB %gid);
	%ar.setVal("fadeIcons",       "1" TAB "fadeIcons" TAB "CheckBox" TAB "" TAB "EWorldEditor" TAB %gid);
	%ar.setVal("fadeIconsDist",       "8" TAB "fadeIconsDist" TAB "TextEdit"  TAB "" TAB "EWorldEditor" TAB %gid);
	%ar.setVal("boundingBoxCollision",       "0" TAB "boundingBoxCollision" TAB "TextEdit" TAB "" TAB "EWorldEditor" TAB %gid);
	%ar.setVal("dropAtScreenCenterScalar",       "1.0" TAB "dropAtScreenCenterScalar" TAB "TextEdit" TAB "" TAB "EWorldEditor" TAB %gid);
	%ar.setVal("dropAtScreenCenterMax",       "100.0" TAB "dropAtScreenCenterMax" TAB "TextEdit" TAB "EWorldEditor" TAB "" TAB %gid);
	%ar.setVal("renderObjHandle",       "1" TAB "renderObjHandle" TAB "CheckBox" TAB "" TAB "EWorldEditor" TAB %gid);
	%ar.setVal("renderObjText",       "1" TAB "renderObjText" TAB "CheckBox" TAB "" TAB "EWorldEditor" TAB %gid);
	%ar.setVal("renderPopupBackground",       "1" TAB "renderPopupBackground" TAB "CheckBox" TAB "" TAB "EWorldEditor" TAB %gid);
	%ar.setVal("renderSelectionBox",       "1" TAB "renderSelectionBox" TAB "CheckBox" TAB "" TAB "EWorldEditor" TAB %gid);
	%ar.setVal("showMousePopupInfo",       "1" TAB "showMousePopupInfo" TAB "CheckBox" TAB "" TAB "EWorldEditor" TAB %gid);
	%ar.setVal("IgnoreDropSelRotation",       "0" TAB "When droping at selection, ignore the rotation" TAB "Checkbox" TAB "" TAB "" TAB %gid);
	%ar.setVal("visibleDistanceScale",       "1" TAB "visibleDistanceScale" TAB "TextEdit" TAB "" TAB "" TAB %gid);
	%ar.setVal("allowMultiInspect",       "1" TAB "allowMultiInspect" TAB "CheckBox" TAB "" TAB "Scene" TAB %gid);
	%ar.setVal("autoInspect",       "1" TAB "autoInspect" TAB "CheckBox" TAB "" TAB "Scene" TAB %gid);
	%ar.setVal("autoInspectApply",       "1" TAB "autoInspectApply" TAB "CheckBox" TAB "" TAB "Scene" TAB %gid);
	
	%gid = 0;
	%ar = %this.createParamsArray("Common","Color" TAB "Colors");
	%ar.group[%gid++] = "World editor colors settings";
	%ar.setVal("gridColor",       "102 102 102 100" TAB "gridColor" TAB "Color" TAB "mode>>int" TAB "EWorldEditor" TAB %gid);
	%ar.setVal("gridOriginColor",      "255 255 255 100"  TAB "gridOriginColor" TAB "Color" TAB "mode>>int" TAB "EWorldEditor" TAB %gid);
	%ar.setVal("gridMinorTickColor",     "51 51 51 100"     TAB "gridMinorTickColor" TAB "Color" TAB "mode>>int" TAB "EWorldEditor" TAB %gid);
	%ar.setVal("objectTextColor",        "255 255 255 255" TAB "objectTextColor" TAB "Color" TAB "mode>>int" TAB "EWorldEditor" TAB %gid);
	%ar.setVal("objMouseOverColor",       "0 255 0 255"   TAB "objMouseOverColor" TAB "Color" TAB "mode>>int" TAB "EWorldEditor" TAB %gid);
	%ar.setVal("objMouseOverSelectColor",     "0 0 255 255"    TAB "objMouseOverSelectColor" TAB "Color" TAB "mode>>int" TAB "EWorldEditor" TAB %gid);
	%ar.setVal("objSelectColor",   "255 0 0 255"      TAB "objSelectColor" TAB "Color" TAB "mode>>int" TAB "EWorldEditor" TAB %gid);
	%ar.setVal("faceSelectColor",     "255 255 0 255"   TAB "faceSelectColor" TAB "Color" TAB "mode>>int" TAB "EWorldEditor" TAB %gid);
	%ar.setVal("selectionBoxColor",     "255 255 0 255"    TAB "selectionBoxColor" TAB "Color" TAB "mode>>int" TAB "EWorldEditor" TAB %gid);
	%ar.setVal("raceSelectColor",     "255 255 0 255"   TAB "raceSelectColor" TAB "Color" TAB "mode>>int" TAB "EWorldEditor" TAB %gid);
	%ar.setVal("popupBackgroundColor",     "100 100 100 255"   TAB "popupBackgroundColor" TAB "Color" TAB "mode>>int" TAB "EWorldEditor" TAB %gid);
	%ar.setVal("popupTextColor",     "255 255 0 255"   TAB "popupTextColor" TAB "Color" TAB "mode>>int" TAB "EWorldEditor" TAB %gid);
	%ar.setVal("dragRectColor",     "255 255 0 255"   TAB "Drag rectangle" TAB "Color" TAB "mode>>int" TAB "EWorldEditor" TAB %gid);
	%ar.setVal("uvEditorHandleColor",     "255 255 0 255"   TAB "uvEditorHandleColor" TAB "Color" TAB "mode>>int" TAB "EWorldEditor" TAB %gid);
	%ar.setVal("selectGridColor",       "102 102 102 100" TAB "Selection Grid color" TAB "ColorSlider" TAB "mode>>int" TAB "Lab.setGizmoGridColor(*val*);" TAB %gid);
	
	//Camera
	%gid = 0;
   %ar = %this.createParamsArray("Common","Grid" TAB "Grid & Units");
	%ar.group[%gid++] = "Editor grids and units settings";
	%ar.prefGroup = "$WEditor::";
	%ar.autoSyncPref = true;
	%ar.setVal("gridSize",      "1" TAB "Grid size" TAB "SliderEdit" TAB "range>>0 20;;ticks>>79;;snap>>1;;precision>>0" TAB "Lab.setGridSize(*val*);" TAB %gid);
	%ar.setVal("forceToGrid",      "1" TAB "Force object to fit grid" TAB "Checkbox" TAB "" TAB "$WEditor::forceToGrid" TAB %gid);
	%ar.setVal("forceToGridNoZ",      "1" TAB "Don't force frid on Z Axis" TAB "Checkbox" TAB "" TAB "$WEditor::forceToGridNoZ" TAB %gid);
	%ar.setVal("gridStep",      "1" TAB "Grid size steps" TAB "TextEdit" TAB "" TAB "" TAB %gid);
	%ar.setVal("renderPlane",       "0" TAB "renderPlane" TAB "Checkbox" TAB "" TAB "" TAB %gid);
	%ar.setVal("renderPlaneHashes",       "0" TAB "renderPlaneHashes" TAB "Checkbox" TAB "" TAB "" TAB %gid);
	%ar.setVal("planeDim",       "500" TAB "planeDim" TAB "TextEdit" TAB "" TAB "" TAB %gid);
	
   //Gizmo
	%gid = 0;
	%ar = %this.createParamsArray("Common","Gizmo" TAB "Manipulation Gizmo");
	%ar.group[%gid++] = "Axis gizmo settings";
	%ar.useGroupPref = true; 
	
	%ar.setVal("gridSize",       "10" TAB "gridSize" TAB "SliderEdit" TAB "range>>0 200" TAB "Lab.setGizmoGridSize(*val*);" TAB %gid);
	%ar.setVal("planeDim",       "500" TAB "planeDim" TAB "SliderEdit" TAB "range>>0 1000" TAB "GlobalGizmoProfile" TAB %gid);
	%ar.setVal("screenLength",       "100" TAB "Gizmo size" TAB "SliderEdit" TAB "range>>0 200;;validate>>flen 2" TAB "GlobalGizmoProfile" TAB "flen 1" TAB %gid);
	%ar.setVal("rotateScalar",       "0.8" TAB "rotateScalar" TAB "SliderEdit" TAB "range>>0 2;;tickAt>>0.01" TAB "Lab.setGizmoScalar(\"rotate\",*val*);" TAB %gid);
	%ar.setVal("scaleScalar",       "0.8" TAB "scaleScalar" TAB "SliderEdit" TAB "range>>0 2;;tickAt>>0.01" TAB "Lab.setGizmoScalar(\"scale\",*val*);" TAB %gid);
	%ar.setVal("snapToGrid",       "0" TAB "snapToGrid" TAB "Checkbox" TAB "" TAB "Lab.setGridSnap(*val*);" TAB %gid);
	%ar.setVal("alwaysRotationSnap",       "0" TAB "Always snap rotation" TAB "Checkbox" TAB "" TAB "GlobalGizmoProfile" TAB %gid);
	%ar.setVal("allowSnapRotations",       "0" TAB "allowSnapRotations" TAB "Checkbox" TAB "" TAB "GlobalGizmoProfile" TAB %gid);
	%ar.setVal("rotationSnap",       "15" TAB "rotationSnap" TAB "SliderEdit" TAB "range>>0 45;;tickAt 1" TAB "GlobalGizmoProfile" TAB %gid);
	%ar.setVal("allowSnapScale",       "0" TAB "allowSnapScale" TAB "Checkbox" TAB "" TAB "GlobalGizmoProfile" TAB %gid);
	%ar.setVal("scaleSnap",       "15" TAB "scaleSnap" TAB "SliderEdit" TAB "range>>0 2;;tickAt 0.05" TAB "GlobalGizmoProfile" TAB %gid);
	%ar.setVal("renderWhenUsed",       "0" TAB "renderWhenUsed" TAB "Checkbox" TAB "" TAB "GlobalGizmoProfile" TAB %gid);
	%ar.setVal("renderInfoText",       "1" TAB "renderInfoText" TAB "Checkbox" TAB "" TAB "GlobalGizmoProfile" TAB %gid);
	%ar.setVal("renderPlane",       "1" TAB "renderPlane" TAB "Checkbox" TAB "" TAB "GlobalGizmoProfile" TAB %gid);
	%ar.setVal("renderPlaneHashes",       "1" TAB "renderPlaneHashes" TAB "Checkbox" TAB "" TAB "GlobalGizmoProfile" TAB %gid);
	%ar.setVal("renderSolid",       "0" TAB "renderSolid" TAB "Checkbox" TAB "" TAB "GlobalGizmoProfile" TAB %gid);
	%ar.setVal("renderMoveGrid",       "1" TAB "renderMoveGrid" TAB "Checkbox" TAB "" TAB "GlobalGizmoProfile" TAB %gid);
	
	
	
	
	
	
	//Camera
	%gid = 0;
	%ar = %this.createParamsArray("Common","Camera");
	
	%ar.prefGroup = "$Camera::";
	%ar.autoSyncPref = true;
	%ar.generalSyncFunc = "Lab.syncCameraGui();";
	%ar.group[%gid++] = "Camera views settings";
	%ar.setVal("movementSpeed",      "1" TAB "Camera movement speed" TAB "sliderEdit" TAB "range>>0 500;;tickat>>1" TAB "$Camera::movementSpeed"TAB %gid);
	%ar.setVal("cameraSpeed",      "1" TAB "Camera speed" TAB "SliderEdit" TAB "range>>0 200;;precision>>0" TAB "$Camera::cameraSpeed"TAB %gid);
	%ar.setVal("CamViewEnabled",       "0" TAB "CamViewEnabled" TAB "CheckBox" TAB "" TAB "" TAB %gid);
	%ar.setVal("cameraDisplayMode",       "Standard Camera" TAB "Initial Camera Mode" TAB "TextEdit" TAB "" TAB "Lab" TAB %gid);
	%ar.setVal("cameraDisplayType",       $EditTsCtrl::DisplayTypePerspective TAB "cameraDisplayType" TAB "TextEdit" TAB "" TAB "Lab" TAB %gid);
	%ar.setVal("DefaultControlMode",       "1" TAB "Default control object" TAB "DropDown"  TAB "itemList>>$CommonControlMode" TAB "" TAB %groupId);
   %ar.setVal("LaunchDefaultAlways",       "1" TAB "Always start with default control" TAB "Checkbox" TAB "" TAB "Lab" TAB %gid);	
	%ar.setVal("orthoFOV",       "50" TAB "orthoFOV" TAB "TextEdit" TAB "" TAB "Lab" TAB %gid);
	%ar.setVal("renderOrthoGrid",       "1" TAB "renderOrthoGrid" TAB "TextEdit" TAB "" TAB "EWorldEditor" TAB %gid);
	%ar.setVal("invertYAxis",       "0" TAB "invertYAxis" TAB "Checkbox" TAB "" TAB "Lab" TAB %gid);
	%ar.setVal("invertXAxis",       "0" TAB "invertXAxis" TAB "Checkbox" TAB "" TAB "Lab" TAB %gid);
	%ar.setVal("MouseMoveMultiplier",      "1" TAB "Mouse camera speed scalar" TAB "sliderEdit" TAB "range>>0 5;;tickat>>0.01" TAB "$Camera::MouseMoveMultiplier" TAB %gid);
	%ar.setVal("MouseScrollMultiplier",      "1" TAB "Mouse camera scroll scalar" TAB "sliderEdit" TAB "range>>0 5;;tickat>>0.01" TAB "$Camera::MouseScrollMultiplier" TAB %gid);
//------------------------------------------------------------------------------
	
	
//==============================================================================
//Interface EDITOR SETTINGS
//==============================================================================
//------------------------------------------------------------------------------
%this.createParamsCategory("UI","Interface");

	
	$FrameMainSizes = "Thin Normal Large";
	//Binds and inputs
	%gid = 0;
	%ar = %this.createParamsArray("UI","Frame" TAB "Workplace");
	%ar.prefGroup = "$LabCfg_EditorUI_";
	%ar.autoSyncPref = true;
	//%ar.style = "StyleA";
	%ar.group[%gid++] = "Main editor frame";	
   %ar.setVal("ToolFrameWidth",      "280" TAB "Tool frame column size" TAB "TextEdit" TAB "" TAB "FW.setToolsWidth(*val*);" TAB %gid);
	%ar.setVal("SideFrameWidth",      "220" TAB "Side frame column size" TAB "TextEdit" TAB "" TAB "FW.setSideBarWidth(*val*);" TAB %gid);
	%ar.setVal("ToolFrameLocked",      "0" TAB "Lock side frame resizing" TAB "Checkbox" TAB "ff>>gg" TAB "Lab.lockEditorFrameContent(*val*);" TAB %gid);
	
	%ar.setVal("ToolFrameLocked",      "0" TAB "Show Physics Toolbar" TAB "Checkbox" TAB "ff>>gg" TAB "Lab.togglePhysicToolbar(*val*);" TAB %gid);
//Binds and inputs
	%gid = 0;
	%ar = %this.createParamsArray("UI","Menu" TAB "Toolbar & Menu");
	%ar.prefGroup = "$LabCfg_EditorUI_";
	%ar.autoSyncPref = true;
	//%ar.style = "StyleA";
	%ar.group[%gid++] = "Main editor frame";
	%ar.setVal("useNativeMenu",       "0" TAB "Use the native menu" TAB "Checkbox" TAB "" TAB "Lab.setNativeMenuSystem(*val*);" TAB %gid);
	%ar.setVal("defaultHandle",       "tlab/art/icons/default/DefaultHandle" TAB "defaultHandle" TAB "TextEdit" TAB "" TAB "EWorldEditor" TAB %gid);
	%ar.setVal("lockedHandle",       "tlab/art/icons/default/LockedHandle" TAB "lockedHandle" TAB "TextEdit" TAB "" TAB "EWorldEditor" TAB %gid);
	%ar.setVal("selectHandle",       "tlab/art/icons/default/SelectHandle" TAB "selectHandle" TAB "TextEdit" TAB "" TAB "EWorldEditor" TAB %gid);
//------------------------------------------------------------------------------

	
//==============================================================================
//GENERAL EDITOR SETTINGS
//==============================================================================
//------------------------------------------------------------------------------
%this.createParamsCategory("General","General Settings");
	%gid = 0;
	%ar = %this.createParamsArray("General","Misc" TAB "Miscellaneous");
	//$Cfg_Common_General_levelsDirectory
	%ar.group[%gid++] = "General Lab Editor settings";
	%ar.setVal("undoLimit",       "40" TAB "undoLimit" TAB "TextEdit" TAB "" TAB "Lab" TAB %gid);
	
	%ar.setVal("TorsionPath",       "C:\Program Files (x86)\Torsion" TAB "Path to Torsion" TAB "TextEdit_2l" TAB "" TAB "$Cfg_TorsionPath" TAB %gid);
	
	%ar.setVal("newLevelFile",       "tlab/art/Levels/BlankRoom.mis" TAB "newLevelFile" TAB "FileSelect" TAB "" TAB "Lab" TAB %gid);
	%ar.setVal("levelsDirectory",       "levels" TAB "levelsDirectory" TAB "TextEdit" TAB "" TAB "Lab" TAB %gid);
	
	%ar.setVal("documentationLocal",       "../../../Documentation/Official Documentation.html" TAB "documentationLocal" TAB "TextEdit" TAB "" TAB "Lab" TAB %gid);
	%ar.setVal("documentationReference",       "../../../Documentation/Torque 3D - Script Manual.chm" TAB "documentationReference" TAB "TextEdit" TAB "" TAB "Lab" TAB %gid);
	%ar.setVal("documentationURL",       "http://www.garagegames.com/products/torque-3d/documentation/user" TAB "documentationURL" TAB "TextEdit" TAB "" TAB "Lab" TAB %gid);
	%ar.setVal("forumURL",       "http://www.garagegames.com/products/torque-3d/forums" TAB "forumURL" TAB "TextEdit" TAB "" TAB "Lab" TAB %gid);

	//Binds and inputs
	%gid = 0;
	%ar = %this.createParamsArray("General","Console");
	%ar.prefGroup = "$pref::Console::";
	//%ar.autoSyncPref = true;
	//%ar.prefModeOnly = true;
	//%ar.style = "StyleA";
	%ar.group[%gid++] = "Main editor frame";
	%ar.setVal("TraceLogLevel",      "1" TAB "TraceLogLevel" TAB "SliderEdit" TAB "range>>0 5;;tickat>>1" TAB "$pref::Console::" TAB %gid);
	%ar.setVal("ShowNotes",      "1" TAB "ShowNotes" TAB "SliderEdit" TAB "range>>0 5;;tickat>>1" TAB "$pref::Console::" TAB %gid);
	%ar.setVal("ShowInfos",      "1" TAB "ShowInfos" TAB "SliderEdit" TAB "range>>0 5;;tickat>>1" TAB "$pref::Console::" TAB %gid);
	%ar.setVal("DevLogLevel",      "1" TAB "DevLogLevel" TAB "SliderEdit" TAB "range>>0 5;;tickat>>1" TAB "$pref::Console::" TAB %gid);
	%ar.setVal("ShowParamLog",      "0" TAB "ShowParamLog" TAB "Checkbox" TAB "" TAB "$pref::Console::" TAB %gid);
	

//==============================================================================
//TOOLS EDITOR SETTINGS
//==============================================================================
//------------------------------------------------------------------------------
%this.createParamsCategory("Tool","Tools Settings");
	%gid = 0;
	%ar = %this.createParamsArray("Tool","LabMat" TAB "Material Editor Dialog");
	//$Cfg_Common_General_levelsDirectory
	%ar.group[%gid++] = "Material Editor Dialog settings";
	%ar.setVal("showMapDiffuse",       "0" TAB "showMapDiffuse" TAB "Checkbox" TAB "" TAB "LabMaterialEditor.toggleMap(\"diffuse\",*val*);" TAB %gid);
	%ar.setVal("showMapNormal",       "0" TAB "showMapNormal" TAB "Checkbox" TAB "" TAB "LabMaterialEditor.toggleMap(\"normal\",*val*);" TAB %gid);
   %ar.setVal("PBRenabled",       "1" TAB "Enable PBR Materials" TAB "checkbox"  TAB "" TAB "LabMat.activatePBR(*val*);" TAB %groupId);
		
	

}
//------------------------------------------------------------------------------


