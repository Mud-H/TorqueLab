//==============================================================================
// Lab GuiManager -> Init Lab GUI Manager System
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$ProfileUpdateSkipCategories = "Tools Core Editor Lab";
$ProfileUpdateColorList = "colorFill colorFont";

$ProfileAutoSave = true;

$GLab_JustifyOptions = "Left Center Right Top Bottom";
$GLab_ProfileCategories = "GameContainer GameButton GameText GameElement GameList GameCore";

$GLab_FontList = "Davidan\tBoostSSK\tBrela\tCarval\tCrimson Text\tLuthier Regular\tRoboto Slab Regular\tWP DOMINO novel";

$GLab::TabId = 0;
$GLab_SelectedObject = "ToolsDefaultProfile";

$GLab_Caption_NoFieldSelected = "Click field name to select";
$GLab_Text_FieldStarMean = "*  \c1= \c2Field value is set in a parent profile";


$GLab_ShowGameProfile = true;

$GLab_UpdateColorsOnSetChanged = true;
$GLab_RescanProfilesOnSave = true;

$GLab_EmbedColorSetInProfile = false;
//==============================================================================
// Load the GuiManager scripts and guis if specified
function initGuiLab(%loadGui) {
	GlobalActionMap.bindCmd(keyboard, "f5", "toggleEdDlg(LabGuiManager);","");
	newScriptObject("GLab");
	newScriptObject("LGTools");
	$arProfData = newArrayObject("arProfData");
	if (isFile("tlab/EditorLab/gui/editorDialogs/guiLab/prefs.cs"))
		exec("tlab/EditorLab/gui/editorDialogs/guiLab/prefs.cs");
	/*


	  if (%loadGui)
	     exec("tlab/EditorLab/gui/editorDialogs/guiLab/LabGuiManager.gui");

	  exec("tlab/EditorLab/gui/editorDialogs/guiLab/LabGuiManager.cs");

	execPattern("tlab/EditorLab/gui/editorDialogs/guiLab/pageProfile/*.cs");
	execPattern("tlab/EditorLab/gui/editorDialogs/guiLab/pageStyle/*.cs");
	execPattern("tlab/EditorLab/gui/editorDialogs/guiLab/pagePreset/*.cs");
	execPattern("tlab/EditorLab/gui/editorDialogs/guiLab/pageOption/*.cs");

	  execPattern("tlab/EditorLab/gui/editorDialogs/guiLab/profileScripts/*.cs");
	   execPattern("tlab/EditorLab/gui/editorDialogs/guiLab/profileTools/*.cs");
	    */
	//GLab.initColorManager();
	initGuiSystem();
}
//------------------------------------------------------------------------------
function postGuiLab() {
	GLab.initColorManager(true);
	scanAllProfileFile();
}


//==============================================================================
// Build the Game related Profiles data list
function initGameProfilesData() {
	doGuiGroupAction("GLab_GameProfileMenu","clear()");
	newSimSet("GameProfileGroup");

	foreach( %obj in GuiDataGroup ) {
		if( !%obj.isMemberOfClass( "GuiControlProfile" ) )
			continue;

		%startCat = getSubStr(%obj.category,0,3);

		if (%startCat !$= "Gam")
			$GLab_IsGameProfile[%obj.getName()] = true;

		if (strFind(%obj.getName(),"Tools") || strFind(%obj.getName(),"Lab") || %obj.category $= "Tools" || strFind(%obj.getName(),"Inspector"))
			$GLab_IsToolProfile[%obj.getName()] = true;

		//  else
		//   continue;
		// }
		$ProfileDefault["fontSize"] = %obj.fontSize;
		GameProfileGroup.add( %obj);
		doGuiGroupAction("GLab_GameProfileMenu","add(\""@%obj.getName()@"\",\""@%obj.getId()@"\")");
	}
}
//------------------------------------------------------------------------------

function initGuiSystem(%check) {
	if (%check && $GuiSystemLoaded)
		return;

	if (!isObject(GameProfileGroup)) {
		initGameProfilesData();
	}

	if (!$ProfileScanDone) {
		//scanAllProfileFile();
	}

	//GLab.updateColors();
	$CanvasSize = Canvas.getExtent();
	$GuiSystemLoaded = true;
}


