//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
// Allow to manage different GUI Styles without conflict
//==============================================================================
singleton GizmoProfile( LabGizmoProfile ) {
	// This isnt a GuiControlProfile but fits in well here.
	// Don't really have to initialize this now because that will be done later
	// based on the saved editor prefs.
	screenLength = 100;
	category = "Editor";
	gridColor = "0 156 0 80";
};
//------------------------------------------------------------------------------
singleton GizmoProfile( GlobalGizmoProfile ) {
	// This isnt a GuiControlProfile but fits in well here.
	// Don't really have to initialize this now because that will be done later
	// based on the saved editor prefs.
	screenLength = 100;
	category = "Editor";
};