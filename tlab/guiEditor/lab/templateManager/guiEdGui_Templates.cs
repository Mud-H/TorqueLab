//==============================================================================
// TorqueLab -> GuiEditor Toggle Functions (open and close)
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//GuiEd.initGuiTemplates();
$GuiEd_TemplateListFilterEmpty = "\c2Filter...";
//==============================================================================
function GuiEd::initGuiTemplates(%this) {
	hide(GuiEd_TplPreview);

	foreach(%gui in GuiEd_TplPreviewContainer)
		GuiEdTemplateGroup.add(%gui);

	GuiEditorTemplateList.clear();
	hide(GuiEdGui_TemplatePillSrc);

	if (!isObject(GuiEdTemplateGroup)) {
		warnLog("No template defined");
		return;
	}

	foreach(%gui in GuiEdTemplateGroup) {
		%this.addTemplatePill(%gui);
	}

	GuiEditorTemplateFilter.text = $GuiEd_TemplateListFilterEmpty;
	GuiEd.tplFilterText = "";
	GuiEd.tplFilterCategory = "";
	GuiEd.filterTemplateList();
	%catMenu = GuiEdGui_TplCatMenu;
	%catMenu.clear();
	%catMenu.add("None",0);
	%catMenu.add("Windows",1);
	%catMenu.add("Containers",2);
	%catMenu.add("TabBooks",3);
	%catMenu.setSelected(0,false);
}
//------------------------------------------------------------------------------

//==============================================================================
function GuiEd::addTemplatePill(%this,%tplGui) {
	%srcPill = GuiEdGui_TemplatePillSrc;
	%pill = cloneObject(GuiEdGui_TemplatePillSrc,"",%tplGui.internalName);
	%pill-->name.text = %tplGui.internalName;
	%pill-->category.text = "\c1Undefined";
	%pill-->selected.visible = "0";
	%pill-->mouse.tplGui = %tplGui;
	%pill-->mouse.pill = %pill;
	%pill-->mouse.superClass = "GuiEdTemplatePillMouse";
	%pill-->mouse.canSaveDynamicFields = "1";
	%pill.tplGui = %tplGui;
	GuiEditorTemplateList.add(%pill);
}
//------------------------------------------------------------------------------

//==============================================================================
function GuiEd::filterTemplateList(%this) {
	%filterText = GuiEd.tplFilterText;
	%filterCat = GuiEd.tplFilterCategory;

	foreach(%pill in GuiEditorTemplateList) {
		%tplGui = %pill.tplGui;

		if (%filterCat !$= "" && %filterCat !$= %tplGui.tplCategory)
			%pill.visible = 0;
		else if (!strFind(strlwr(%tplGui.internalName),strlwr(%filterText),true))
			%pill.visible = 0;
		else
			%pill.visible = 1;
	}

	GuiEditorTemplateList.refresh();
}
//------------------------------------------------------------------------------
//==============================================================================
function GuiEd::addTemplateToGUI(%this) {
	if (!isObject(GuiEd.activeTemplateGui)) {
		GuiEd.setActiveTemplate();
		return;
	}

	TplManager.addTemplateToGui(GuiEd.activeTemplateGui);
}
//------------------------------------------------------------------------------
//==============================================================================
function GuiEd::applyTemplateOnSelection(%this) {
	if (!isObject(GuiEd.activeTemplateGui)) {
		GuiEd.setActiveTemplate();
		return;
	}

	%selectionSet = GuiEditor.getSelection();

	foreach(%ctrl in %selectionSet) {
		devLog(%id++,"Applying template on GuiControl:",%ctrl.getId(),%ctrl.getName());
		TplManager.applyTemplateOnControl(GuiEd.activeTemplateGui,%ctrl);
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function GuiEd::setSelectionAsTemplateSource(%this) {
	if (!isObject(GuiEd.activeTemplateGui)) {
		GuiEd.setActiveTemplate();
		return;
	}

	if (GuiEditor.getNumSelected() > 1) {
		warnLog("You have to select on GuiControl for template source");
		return;
	}

	if (GuiEditor.getNumSelected() < 1 || GuiEditor.getNumSelected() $= "") {
		warnLog("There's no GuiControl selected to be set as template source");
		return;
	}

	%tplSrc = %selectionSet.getObject(0);
	%name = %selectionSet.getObject(0).getName();

	if (%name $= "")
		%name = %selectionSet.getObject(0);

	devLog("New template source control for template:",GuiEd.activeTemplate,"Source Control:",%name);
}
//------------------------------------------------------------------------------
//==============================================================================
// Control select callbacks
//==============================================================================
//==============================================================================
function GuiEd::setActiveTemplate(%this,%tplGui) {
	%applyButton = GuiEdGui_BottomPageTemplate-->tplApplyButton;
	%setSrcButton = GuiEdGui_BottomPageTemplate-->tplSetSourceButton;
	%title = GuiEdGui_BottomPageTemplate-->tplTemplateInfo;
	%catCtrl = GuiEdGui_BottomPageTemplate-->tplInfoCategory;
	%setSrcButton.active = 0;

	if (!isObject(%tplGui)) {
		%applyButton.active = 0;
		GuiEd.activeTemplateGui = "";
		GuiEd.activeTemplate = "";
		%title.text = "\c3None";
		%catCtrl.text = "\c3N/A";
		hide(GuiEdGui_TplInfoCtrl);
		return;
	}

	%applyButton.active = 1;

	if (GuiEditor.getNumSelected() >= 1)
		%setSrcButton.active = 1;

	GuiEd.activeTemplateGui = %tplGui;
	GuiEd.activeTemplate = %tplGui.internalName;

	if (GuiEd_TplPreview.visible)
		GuiEd.updateTplPreview();

	%title.text = "\c1"@GuiEd.activeTemplate;
	%catCtrl.text = "Category\c1" SPC %tplGui.tplCategory;
	return;

	if (GuiEd.showTemplateInfo)
		show(GuiEdGui_TplInfoCtrl);
	else
		hide(GuiEdGui_TplInfoCtrl);
}
//------------------------------------------------------------------------------
//==============================================================================
function GuiEdTemplatePillMouse::onMouseDown(%this,%modifier,%mousePoint,%mouseClickCount) {
	%tplGui = %this.tplGui;

	foreach(%pill in GuiEditorTemplateList) {
		%pill-->selected.visible = "0";
	}

	%this.pill-->selected.visible = "1";
	warnLog("Clicked on Template:",%tplGui.internalName);
	GuiEd.setActiveTemplate(%tplGui);
}
//------------------------------------------------------------------------------


//==============================================================================
// Control select callbacks
//==============================================================================

//==============================================================================
function GuiEd::templateTargetChanged(%this) {
	%selectionSet = GuiEditor.getSelection();	

	%setSrcButton = GuiEdGui_BottomPageTemplate-->tplSetSourceButton;
	%applyButton = GuiEdGui_BottomPageTemplate-->tplApplyButton;

	if (GuiEditor.getNumSelected() >= 1 && isObject(GuiEd.activeTemplateGui)) {
		if (GuiEditor.getNumSelected() > 1)
			%setSrcButton.active = 0;
		else
			%setSrcButton.active = 1;

		%applyButton.active = 1;
	} else {
		%setSrcButton.active = 0;
		%applyButton.active = 0;
	}

	if (GuiEditor.getNumSelected() == 1) {
		%name = %selectionSet.getObject(0).getName();

		if (%name $= "")
			%name = %selectionSet.getObject(0);

		%text = "\c1" SPC %name;
	} else if (GuiEditor.getNumSelected() > 1) {
		%text = "\c1"@GuiEditor.getNumSelected() SPC "\c0 targets selected";
	} else {
		%text = "No GuiControl selected";
	}

	%title = GuiEdGui_BottomPageTemplate-->tplTargetInfo;
	%title.text = %text;
}
//------------------------------------------------------------------------------

//==============================================================================
// Templates Filtering and category menu
//==============================================================================
function GuiEdGui_TplCatMenu::onSelect(%this,%id,%text) {
	if (%id == 0)
		%text = "";

	GuiEd.tplFilterCategory = %text;
	GuiEd.filterTemplateList();
}
//------------------------------------------------------------------------------
//==============================================================================
function GuiEditorTemplateFilter::onValidate(%this) {
	%text = %this.getText();

	if (%text $= "")
		%this.text = $GuiEd_TemplateListFilterEmpty;
	else if (%text $= $GuiEd_TemplateListFilterEmpty)
		%text = "";

	GuiEd.tplFilterText = %text;
	GuiEd.filterTemplateList();
}
//------------------------------------------------------------------------------
//==============================================================================
function GuiEditorTemplateFilter::reset(%this) {
	%this.text = $GuiEd_TemplateListFilterEmpty;
	GuiEd.tplFilterText = "";
	GuiEd.filterTemplateList();
}
//------------------------------------------------------------------------------
//==============================================================================
function GuiEd::toggleTemplateInfo(%this) {
	GuiEdGui_TplInfoCtrl.visible = !GuiEdGui_TplInfoCtrl.visible;
	GuiEd.showTemplateInfo = GuiEdGui_TplInfoCtrl.visible;
}
//------------------------------------------------------------------------------

//==============================================================================
//GuiEd.previewTemplate();
function GuiEd::previewTemplate(%this) {
	if (!isObject(GuiEd.activeTemplateGui)) {
		GuiEd.setActiveTemplate();
		return;
	}

	if (GuiEd_TplPreview.isVisible()) {
		hide(GuiEd_TplPreview);

		foreach(%gui in GuiEd_TplPreviewContainer)
			GuiEdTemplateGroup.add(%gui);

		GuiEd.previewIsActive = true;
		return;
	}

	show(GuiEd_TplPreview);
	GuiEd_TplPreviewContainer.add(GuiEd.activeTemplateGui);
}
//------------------------------------------------------------------------------
//==============================================================================
//GuiEd.previewTemplate();
function GuiEd::updateTplPreview(%this) {
	if (!isObject(GuiEd.activeTemplateGui)) {
		GuiEd.setActiveTemplate();
		return;
	}

	if (!GuiEd_TplPreview.isVisible())
		return;

	foreach(%gui in GuiEd_TplPreviewContainer)
		GuiEdTemplateGroup.add(%gui);

	show(GuiEd.activeTemplateGui);
	GuiEd_TplPreviewContainer.add(GuiEd.activeTemplateGui);
}
//------------------------------------------------------------------------------
//==============================================================================
//GuiEd.previewTemplate();
function GuiEd::closeTplPreview(%this) {
	foreach(%gui in GuiEd_TplPreviewContainer)
		GuiEdTemplateGroup.add(%gui);

	hide(GuiEd_TplPreview);
}
//------------------------------------------------------------------------------