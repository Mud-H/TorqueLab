//==============================================================================
// GameLab -> Interface Development Gui
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$GLab_ProfileBook_ActivePageId = 0;

//==============================================================================
function GLab::initStylePage( %this ) {
	%this.initProfileStyles();
	GLab_StyleUpdatedInfo.visible = 0;
}
//------------------------------------------------------------------------------

//==============================================================================
function GLab::reloadCurrentGUI( %this ) {
	%currentGui = Canvas.getContent();
	Canvas.setContent(StartupGui);
	%this.schedule(200,"resetCurrentGUI",%currentGui);
}
//------------------------------------------------------------------------------
//==============================================================================
function GLab::resetCurrentGUI( %this,%content ) {
	Canvas.setContent(%content);
	pushDlg(LabGuiManager);
	GLab_StyleUpdatedInfo.visible = 0;
}
//------------------------------------------------------------------------------