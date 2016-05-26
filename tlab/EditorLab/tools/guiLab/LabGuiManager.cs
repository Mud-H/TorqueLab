//==============================================================================
// GameLab -> Interface Development Gui
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================


//==============================================================================
function LabGuiManager::onWake( %this ) {
	if( !isObject( "GameProfilesPM" ) )
		new PersistenceManager( GameProfilesPM );

	if ($GLab_MainBook_ActivePageId $= "")
		$GLab_MainBook_ActivePageId = 0;

	GLab_MainBook.selectPage($GLab_MainBook_ActivePageId);
	GLab_ProfileBook.selectPage($GLab::TabId);
	exec("tlab/EditorLab/tools/guiLab/LabGuiManager.cs");
	$true = true;
	$Color = "12 88 133 254";
	$TextSample = "TextBaseMed sample text éç!? CPG cpg";
	initGuiSystem();
	hide(GLab_NewProfileDlg);
	hide(wParams_ProfileSet);
	GLab.initProfilePage();
	GLab.initStylePage();
	GLab.initPresetPage();
	GLab.initOptionPage();

	if (GLab_MainWindow.extent.y >= LabGuiManager.extent.y) {
		%pos = GLab_MainWindow.position;
		%extent = GLab_MainWindow.extent.x SPC LabGuiManager.extent.y-30;
		GLab_MainWindow.resize(%pos.x,%pos.y,%extent.x,%extent.y);
		// GLab_MainWindow.extent.y = LabGuiManager.extent.y;
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function LabGuiManager::onSleep( %this ) {
	GLab.savePrefs();
}
//------------------------------------------------------------------------------


//==============================================================================
function GLab_MainBook::onTabSelected( %this,%text,%id ) {
	$GLab_MainBook_ActivePageId = %id;
	$GLab_MainBook_ActivePageText = %text;
}
//------------------------------------------------------------------------------


//==============================================================================
function GLab::exportPrefs( %this ) {
	export("$GLab_pref_*", "tlab/EditorLab/tools/guiLab/prefs.cs", false);
}
//------------------------------------------------------------------------------




//==============================================================================
function GLab::savePrefs( %this ) {
	%file = filePath(LabGuiManager.getfilename())@"/prefs.cs";
	export("$GLab::*", %file, false);
}
//------------------------------------------------------------------------------
