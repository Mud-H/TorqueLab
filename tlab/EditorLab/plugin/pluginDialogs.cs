//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// Manage Plugins Dialogs
//==============================================================================





//==============================================================================
function PluginDlg::toggleDlg( %this, %dlg,%contentId,%alwaysOn ) {
	%this.fitIntoParents();
	%dlgCtrl = %this.findObjectByInternalName(%dlg);
	
	if (!isObject(%dlgCtrl))
		return;

	if (%dlgCtrl.isVisible())
		%this.hideDlg(%dlg);
	else
		%this.showDlg(%dlg,%contentId,%alwaysOn);
}
//------------------------------------------------------------------------------
//==============================================================================
function PluginDlg::showDlg( %this, %dlg,%contentId,%alwaysOn ) {
	%this.fitIntoParents();
	%dlgCtrl = %this.findObjectByInternalName(%dlg);
	%dlgCtrl.alwaysOn = %alwaysOn;
	if (%contentId !$= "" || %this.contentId !$= "")
		%this.contentId = %contentId;
	if (!isObject(%dlgCtrl))
		return;
	
	//Check if the dialog have a checkState method which make sure it's ready for open
	if(%dlgCtrl.isMethod("checkState"))
	{
		%success = %dlgCtrl.checkState();
		info(%dlg,"Dialog state has been checked, the result is:",%success);
	}

	if (!%dlgCtrl.isVisible())
		show(%dlgCtrl);

	show(%this);

	if(%dlgCtrl.isMethod("onShow"))
		%dlgCtrl.onShow();
}
//------------------------------------------------------------------------------
//==============================================================================
function PluginDlg::hideDlg( %this, %dlg,%contentId ) {
	%this.fitIntoParents();
	%dlgCtrl = %this.findObjectByInternalName(%dlg);
	%this.hideDlgCtrl(%dlgCtrl);
}
//------------------------------------------------------------------------------
//==============================================================================
function PluginDlg::closeSelf( %this,%dlgCtrl ) {
	//Check is the dlg is used as TLabGameGui
	if (%dlgCtrl.parentGroup $= TLabGameGui) {
		TLabGameGui.closeAll();
	}

	%this.hideDlgCtrl(%dlgCtrl);
}
//------------------------------------------------------------------------------
//==============================================================================
function PluginDlg::hideDlgCtrl( %this, %dlgCtrl ) {
	if (!isObject(%dlgCtrl))
		return;

	%dlgCtrl.alwaysOn = false;
	//if (!%dlgCtrl.isVisible())
	//return;
	hide(%dlgCtrl);

	if(%dlgCtrl.isMethod("onHide"))
		%dlgCtrl.onHide();

	%this.checkState();
}
//------------------------------------------------------------------------------
//==============================================================================
function PluginDlg::checkState( %this ) {
	%visibleCount = 0;
	%hiddenCount = 0;

	foreach(%ctrl in %this) {
		if (%ctrl.visible)
			%visibleCount++;
		else
			%hiddenCount++;
	}

	//If all are hidden, hide the dialog container
	if (%visibleCount == 0 && %this.visible)
		hide(%this);
	else if (!%this.visible)
		show(%this);
}
//------------------------------------------------------------------------------

//==============================================================================
function PluginDlg::onSleep( %this ) {
	foreach(%ctrl in %this) {
		if (%ctrl.closeOnActivate)
			hide(%ctrl);
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function PluginDlg::onPreEditorSave(%this) {
	devLog("PluginDlg::onPreEditorSave",%this);

	//Check if a onPreEditorSave method is found in sub dialogs
	foreach(%ctrl in %this) {
		if (%ctrl.isMethod("onPreEditorSave"))
			%ctrl.onPreEditorSave();
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function PluginDlg::onPostEditorSave(%this) {
	devLog("PluginDlg::onPostEditorSave",%this);

	//Check if a onPreEditorSave method is found in sub dialogs
	foreach(%ctrl in %this) {
		if (%ctrl.isMethod("onPostEditorSave"))
			%ctrl.onPostEditorSave();
	}
}
//------------------------------------------------------------------------------
