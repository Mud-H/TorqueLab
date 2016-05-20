//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
function ETools::initTools(%this) {
	newSimSet("EToolsGuiSet");
	//Lab.addGui( EToolOverlayGui ,"Overlay");
	Lab.addGui( ETools ,"Dialog");
	
	%pattern = "tlab/EditorLab/gui/editorTools/*.gui";
	//call exec() on all files found matching the pattern
	for(%file = findFirstFile(%pattern); %file !$= ""; %file = findNextFile(%pattern))
	{
		%found = strFind(%file,"selfload");
		if (fileBase(%file) $= "ETools" ||fileBase(%file) $= "EToolOverlayGui" || %found)
			continue;		
		exec(%file);	
			
		ETools.addGui(filebase(%file));	
	}
	
	if (!LabDialogGuiSet.isMember(ETools))	
		Lab.addGui( ETools ,"Dialog");
	foreach(%gui in %this) {
		hide(%gui);
		if (%gui.isMethod("initTool"))
			%gui.initTool();
	}


}
//------------------------------------------------------------------------------

function LabToolsDlg::onAdd(%this) {
		ETools.addGui(%this);		
}

function ETools::addGui(%this,%gui) {
		ETools.add(%gui);
		EToolsGuiSet.add(%gui);
		
}
function ETools::onPreEditorSave(%this) {
	foreach(%gui in ETools)
		GuiGroup.add(%gui);
		
}
function ETools::onPostEditorSave(%this) {	
	foreach(%gui in EToolsGuiSet)
		ETools.add(%gui);
}
//==============================================================================
function ETools::toggleTool(%this,%tool) {
	if (isObject(%tool))
		%dlg = %tool;
	else
		%dlg = %this.findObjectByInternalName(%tool,true);

	if (!isObject(%dlg)) {
		warnLog("Trying to toggle invalid tool:",%tool);
		return;
	}

	%this.fitIntoParents();
	ETools.visible = true;

	if (%dlg.visible) {
		%this.hideTool(%tool);
		//%dlg.setVisible(false);
		//%position = getRealCursorPos();
		//%dlg.position = %position;
		//%dlg.position.x -= %dlg.extent.x/2;
		//%dlg.position.y -= EditorGui-->AreaMenuBar.extent.y;
	} else {
		%this.showTool(%tool);
		//%dlg.setVisible(true);
	}

	return;
	%hideMe = true;

	foreach(%gui in %this)
		if (%gui.visible)
			%hideMe = false;

	if (%hideMe)
		%this.visible = 0;

	if (isObject(%dlg.linkedButton))
		%dlg.linkedButton.setStateOn(%dlg.visible);
}
//------------------------------------------------------------------------------

//==============================================================================
function ETools::showTool(%this,%tool) {
	if (isObject(%tool))
		%dlg = %tool;
	else
		%dlg = %this.findObjectByInternalName(%tool,true);

	if(!isObject(%dlg)) {
		warnLog("Trying to show invalid tools dialog for tool:",%tool,"Dlg",%dlg);
		return;
	}

	%this.fitIntoParents();
	%toggler = EditorGuiToolbarStack.findObjectByInternalName(%tool@"Toggle",true);

	if (isObject(%toggler))
		%toggler.setStateOn(true);

	ETools.visible = true;
	%dlg.setVisible(true);

	if (isObject(%dlg.linkedButton))
		%dlg.linkedButton.setStateOn(true);

	if (%dlg.isMethod("onShow"))
		%dlg.onShow();
}
//------------------------------------------------------------------------------

//==============================================================================
function ETools::hideTool(%this,%tool) {
	if (isObject(%tool))
		%dlg = %tool;
	else
		%dlg = %this.findObjectByInternalName(%tool,true);

	%toggler = EditorGuiToolbarStack.findObjectByInternalName(%tool@"Toggle",true);

	if (isObject(%toggler))
		%toggler.setStateOn(false);

	%dlg.setVisible(false);
	%hideMe = true;

	foreach(%gui in %this)
		if (%gui.visible)
			%hideMe = false;

	if (%hideMe)
		%this.visible = 0;

	if (isObject(%dlg.linkedButton))
		%dlg.linkedButton.setStateOn(false);

	if (%dlg.isMethod("onHide"))
		%dlg.onHide();
}
//------------------------------------------------------------------------------
