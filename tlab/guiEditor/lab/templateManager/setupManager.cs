//==============================================================================
// TorqueLab -> GuiEditor Template Manager - Initialize
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
function Lab::initTemplateManager(%this) {
	$TplManager = newScriptObject("TplManager");
	$TplManagerGroup = newSimGroup("TplManagerGroup");
}
//------------------------------------------------------------------------------
//==============================================================================
function TplManager::checkSave(%this,%obj) {
	if (LabObj.isDirty(TplManager.tplRootGui)) {
		info("Saving changes to GUI");
		%this.saveTemplates();
	}
}
//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
function TplManager::saveTemplates(%this) {
	//GuiEditCanvas.save( false, true );
	GuiEdTemplateGroup.setFilename("tlab/guiEditor/gui/GuiEdTemplateGroup.gui");
	GuiEditCanvas.save( GuiEdTemplateGroup, true );
	geTM_SaveTplButton.active = 0;
	TplManager.isDirty = false;
}
//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
function TplManager::setCtrlFieldValue(%this,%ctrl,%field,%value) {
	%ctrlIsDirty = LabObj.update(	%ctrl,%field,%value);

	if (%ctrlIsDirty) {
		LabObj.setDirty(TplManager.tplRootGui,true);
		TplManager.isDirty = true;
		geTM_SaveTplButton.active = 1;
	}
}
//------------------------------------------------------------------------------

//==============================================================================
function TplManager::initBrowser(%this) {
	TplManagerGroup.clear();

	foreach(%gui in GuiEdTemplateGroup) {
		%tplObj = %this.getTemplateObj(%gui.internalName);
		%gui.tplObj = %tplObj;
		%gui.tplGui = %gui;
		%tplObj.tplGui = %gui;
		TplManagerGroup.add(%tplObj);
	}

	%this.updateTemplates();
}
//------------------------------------------------------------------------------

//==============================================================================
//TplManager.toggleBrowser();
function TplManager::toggleBrowser(%this,%state) {
	GuiEdTemplateBrowser.setExtent(253,725);
	GuiEdTemplateBrowser.position = "8 100";

	if (!GuiEdTemplateEditor.isAwake()) {
		GuiEditor.preTplContent = GuiEditor.lastContent;
		GuiEditContent(GuiEdTemplateEditor);
		//pushDlg(GuiEdTemplateManager);
	} else {
		if (isObject(TplManager.previousGui))
			TplManager.previousGui = GuiEditor.initialContent;

		GuiEditContent(GuiEditor.preTplContent);
	}

	//toggleDlg(GuiEdTemplateManager);
}
//------------------------------------------------------------------------------
//==============================================================================
//TplManager.toggleBrowser();
function TplManager::showBrowser(%this) {
	GuiEdTemplateBrowser.setExtent(594,725);
	GuiEdTemplateBrowser.position = "8 100";
	TplManager.initBrowser();
	pushEdDlg(GuiEdTemplateManager);
	//toggleDlg(GuiEdTemplateManager);
}
//------------------------------------------------------------------------------
//==============================================================================
//TplManager.toggleBrowser();
function TplManager::hideBrowser(%this) {
	GuiEdTemplateBrowser.setExtent(253,725);
	GuiEdTemplateBrowser.position = "8 100";
	popDlg(GuiEdTemplateManager);
}
//------------------------------------------------------------------------------
