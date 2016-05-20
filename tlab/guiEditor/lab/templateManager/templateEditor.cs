//==============================================================================
// TorqueLab -> GuiEditor Template Manager - Editor
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
function GuiEdTemplateEditor::onWake(%this) {
	devLog("GuiEdTemplateEditor::onWake(%this)");

	if (!isObject(GuiEdTemplateGroup))
		exec( "tlab/guiEditor/gui/GuiEdTemplateGroup.gui" );

	GuiEdTemplateContainer.add(GuiEdTemplateGroup);
	GuiEdTemplateContainer.bringToFront(GuiEdTemplateGroup);

	if (GuiEditor.lastContent.getName() $= "GuiEdTemplateEditor") {
		devLog("Not in editor");
		return;
	}

	if (!GuiEdTemplateManager.isAwake())
		TplManager.schedule(200,"showBrowser");
}
//==============================================================================
//==============================================================================
function GuiEdTemplateEditor::onPreEditorSave(%this) {
	GuiGroup.add(GuiEdTemplateGroup);
}
//------------------------------------------------------------------------------
//==============================================================================
function GuiEdTemplateEditor::onPostEditorSave(%this) {
	GuiEdTemplateContainer.add(GuiEdTemplateGroup);
	GuiEdTemplateContainer.bringToFront(GuiEdTemplateGroup);
}
//------------------------------------------------------------------------------
//==============================================================================

//------------------------------------------------------------------------------
//==============================================================================
function GuiEdTemplateEditor::onSleep(%this) {
	devLog("GuiEdTemplateEditor::onSleep(%this)");

	if (GuiEdTemplateManager.isAwake())
		TplManager.hideBrowser();
}
//==============================================================================

//==============================================================================
function TplManager::initEditor(%this) {
}
//==============================================================================

