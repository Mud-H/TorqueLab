//==============================================================================
// TorqueLab -> GuiEditor Template Manager - Broswer
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================


//==============================================================================
function GuiEdTemplateManager::onWake(%this) {
	hide(GuiEdTemplateContainerSrc);
	geTM_SaveTplButton.active = TplManager.isDirty;
}
//------------------------------------------------------------------------------


//==============================================================================
function TplManager::updateTemplates(%this) {
	if (!$TplManager_ShowChildTemplateList)
		TplManager.tplListObj = TplManagerGroup;
	else
		TplManager.tplListObj = GuiEdTemplateGroup;

	if (!isObject(TplManager.tplTreeObj))
		TplManager.tplTreeObj = TplManager.tplListObj.getObject(0);

	GuiEdTemplateTree.open(TplManager.tplListObj);
	GuiEdTemplateTree.buildVisibleTree();

	if (!isObject(TplManager.tplTreeObj))
		return;

	%id = GuiEdTemplateTree.findItemByObjectId(TplManager.tplTreeObj);

	if (%id !$= "-1")
		GuiEdTemplateTree.selectItem(%id);
}
//------------------------------------------------------------------------------



//==============================================================================
// Template Tree & Selection
//==============================================================================
$LockExpand = true;
//==============================================================================
function GuiEdTemplateTree::onSelect(%this,%obj) {
	if (!isObject(%obj))
		return;

	if (%obj.isTpl) {
		%tplGui = %obj.tplGui;
		TplManager.tplTreeObj = %obj;
		%id = GuiEdTemplateTree.findItemByObjectId(%obj);
		TplManager.tplTreeItem = %id;
		TplManager.setActiveTemplate(%tplGui);
	} else {
		if ($LockExpand)
			GuiEdTemplateTree.expandItem(TplManager.tplTreeItem,false);

		TplManager.setActiveSubControl(%obj);
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function GuiEdTemplateTree::onUnselect(%this,%obj) {
	if (%obj.isTpl) {
		%tplGui = %obj.tplGui;
		TplManager.checkSave(%tplGui.rootGui);
		hide(%tplGui);

		if (%obj.isGui) {
			%id = GuiEdTemplateTree.findItemByObjectId(%obj);

			if (%id !$= "-1")
				GuiEdTemplateTree.expandItem(%id,false);
		}
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function TplManager::setActiveTemplate(%this,%obj) {
	foreach(%gui in GuiEdTemplateGroup)
		hide(%gui);

	show(%obj);
	GuiEdTemplateEditor-->templateEditorWindow.text = "\c1"@%obj.internalName@"\c0 template UI editing and setup";
	%obj.tplGui-->templateSourceWindow.text = "\c1"@%obj.internalName@"\c0 GuiControls editor";
	%mainTplGui = %obj.getObject(0);
	%name = %mainTplGui.getName();

	if (%name $="") {
		%name = getUniqueName(%obj.internalName@"_Root");
		warnLog("Main GuiControl of a template require a name for saving. A name have been generated:",%name);
		%mainTplGui.setName(%name);
		LabObj.setDirty(%mainTplGui,true);
	}

	if (%mainTplGui.getFilename $="")
		%mainTplGui.setFilename(GuiEdTemplateEditor.getFilename());

	%this.tplRootGui = %mainTplGui;
	%obj.rootGui = %mainTplGui;
	GuiEdTplStructureTree.open(%obj);
	GuiEdTplStructureTree.buildVisibleTree();
	TplManager.setActiveSubControl(%obj);
}
//------------------------------------------------------------------------------
//==============================================================================
// Active Template Structure Tree & Sub GuiCtrl Selection
//==============================================================================
//==============================================================================
function GuiEdTplStructureTree::onSelect(%this,%obj) {
	if (!isObject(%obj))
		return;

	TplManager.setActiveSubControl(%obj);
}
//------------------------------------------------------------------------------
//==============================================================================
function GuiEdTplStructureTree::onUnselect(%this,%obj) {
	if (!isObject(%obj))
		return;
}
//------------------------------------------------------------------------------

//==============================================================================
function TplManager::setActiveSubControl(%this,%obj) {
	if (!isObject(%obj)) {
		GuiEdTplEditor_GuiCtrlSettings-->activeSubCtrlInfo.text = "No Template GuiControl Selected";
		return;
	}

	TplManager.tplSubControl = %obj;
	%className = %obj.getClassName();
	%name = %obj.getName();
	
	GuiEdTplSelectCtrlInspector.inspect(%obj);

	if (%name $= "")
		%name = %obj.internalName;

	if (%name $= "")
		%name = %obj.getId();

	GuiEdTplEditor_GuiCtrlSettings-->activeSubCtrlInfo.text = "Class:\c1" SPC %className SPC "\c0-Name:\c2" SPC %name;
	%this.updateSubControlFieldsStacks();
}
//------------------------------------------------------------------------------

//==============================================================================
function TplManager::updateSubControlFieldsStacks(%this,%obj) {
	%subCtrl = TplManager.tplSubControl;

	foreach$(%stack in "geTM_CtrlBasicFieldsStack geTM_CtrlSpecialFieldsStack") {
		foreach(%ctrl in %stack) {
			%check = %ctrl.getObject(0);
			%check.variable = "";
			%check.superClass = "geTM_SharedFieldCheckbox";
			%field = %check.internalName;
			%shared = strFind(%subCtrl.shareFields,%field);
			%check.setStateOn(%shared);
		}
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function geTM_SharedFieldCheckbox::onClick(%this) {
	%subCtrl = TplManager.tplSubControl;
	%shareFields = %subCtrl.shareFields;
	%shared = %this.isStateOn();
	%field = %this.internalName;

	if (%shared) {
		%shareFields = strAddWord(%shareFields,%field,true);
	} else {
		%shareFields = strRemoveWord(%shareFields,%field);
	}

	TplManager.setCtrlFieldValue(%subCtrl,"shareFields",%shareFields);
}
//------------------------------------------------------------------------------
