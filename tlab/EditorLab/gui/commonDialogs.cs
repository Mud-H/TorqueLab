//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================


//==============================================================================
// Activate the interface for a plugin
//==============================================================================
//==============================================================================
//Set a plugin as active (Selected Editor Plugin)
function Lab::toggleToolbarDialog(%this,%guiCtrl,%icon) {
	%dialog = %guiCtrl;

	if (%guiCtrl.isMemberOfClass("GuiMouseEventCtrl"))
		%dialog = %guiCtrl.getObject(0);

	%guiCtrl.fitIntoParents();

	if ( %guiCtrl.visible  ) {
		%guiCtrl.setVisible(false);
		%icon.setStateOn(0);
	} else {
		%guiCtrl.setVisible(true);
		%icon.setStateOn(1);
		%dialog.position = %icon.position;
	}
}
//------------------------------------------------------------------------------




//==============================================================================
function EToolDlgDecoyArea::onMouseLeave() {
	EToolDlgCameraTypesToggle();
}
//------------------------------------------------------------------------------