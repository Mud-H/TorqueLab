//==============================================================================
// TorqueLab -> GuiEditor Toggle Functions (open and close)
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$InGuiEditor = false;
$MLAAFxGuiEditorTemp = false;
$GuiEdGui_BottomTabPage = 0;
$GuiEd_InitCompleted = false;
//==============================================================================
// Toggle, Open amd Close
//==============================================================================
//==============================================================================
// Toggle the GuiEditor( Close if open and open if closed)
function GuiEd::InitGuiEditor( %this,%force ) {
	if (!$GuiEd_InitCompleted || %force) {
		//Nothing here for now...
	}

	if ($GuiEd_InitCompleted ) {
		return;
	}

	// Init the Common stuff
	GuiEd.initCommonInspector();
	// Init the GUI Page stuff
	GuiEd.initGuiTreeView();
	GuiEdGui_BottomTabBook.selectPage($GuiEdGui_BottomTabPage);
	//GuiEd.initGuiTemplates();
	// Init the Profile Page stuff
	GuiEd.initProfileTreeView();
	// Init the Library Page stuff
	GuiEd.initLibraryContent();
	$GuiEd_InitCompleted = true;
}
//------------------------------------------------------------------------------
