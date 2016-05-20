//==============================================================================
// TorqueLab Editor ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$TplManager_ShowChildTemplateList = false;
$TplManager_SaveOnTemplateUnselected = true;
//==============================================================================
function TplManagerCreatorToggle::onExpanded(%this) {
	show(tplManager_Creator);
}
//------------------------------------------------------------------------------
//==============================================================================
function TplManagerCreatorToggle::onCollapsed(%this) {
	hide(tplManager_Creator);
}
//------------------------------------------------------------------------------


//==============================================================================
function TplManager::createNewTemplate(%this) {
	%name = tplManager_Creator-->newTemplateName.getText();
	%tplGui = %this.getTemplateGui(%name);
	%tplGui.isTpl = true;
	%tplGui.isGui = true;
	%tplGui.tplGui = %tplGui; //Used for tree selection
	%tplObj = %this.getTemplateObj(%name);
	%tplObj.tplGui = %tplGui;
	%tplGui.tplObj = %tplObj;
	%this.updateTemplates();
}
//------------------------------------------------------------------------------

//==============================================================================
function TplManager::getTemplateGui(%this,%name) {
	%tplGui = "tplGui_"@%name;

	if (isObject(%tplGui)) {
		GuiEdTemplateGroup.add(%tplGui);
		return %tplGui;
	}

	%tpl = cloneObject(GuiEdTemplateContainerSrc,%tplGui,%name,GuiEdTemplateGroup);
	hide(%tpl);
	%tpl.canSaveDynamicFields = true;
	return %tpl;
}
//------------------------------------------------------------------------------
//==============================================================================
function TplManager::getTemplateObj(%this,%name) {
	%tplName = "tpl_"@%name;

	if (isObject(%tplName))
		return %tplName;

	%tplObj = newScriptObject(%tplName,TplManagerGroup,"GuiTemplate");
	%tplObj.isTpl = true;
	return %tplObj;
}
//------------------------------------------------------------------------------