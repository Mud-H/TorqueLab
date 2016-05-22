//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
// ETools is a special system to handle common Editor Tools Dialogs. It include
// a bunch of Dialogs that the ETools object manage the display. Use it make sure
// all Tools dialogs are set correctly and make it easy to adapt all behaviors
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
      %gui = filebase(%file);
      $EToolsUseOwnGui[%gui.getName()] = true;
		ETools.addGui(%gui);	
		
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
//==============================================================================
function LabToolsDlg::onAdd(%this) {
		ETools.addGui(%this);
		EToolsGuiSet.add(%this);		
}
//------------------------------------------------------------------------------
//==============================================================================
function ETools::addGui(%this,%gui) {
		ETools.add(%gui);
		EToolsGuiSet.add(%gui);
		
}
//------------------------------------------------------------------------------
//==============================================================================
// ETools Pre/Post save - ETools have some Dialog included but some are store extern
//==============================================================================

//==============================================================================
function ETools::onPreEditorSave(%this) {
	foreach(%gui in ETools)
	{
	   //If the %gui use it's own GUI, add to temp group and save it to it's file.
	   %ownGui = $EToolsUseOwnGui[%gui.getName()];
	   if ( !%ownGui )
	      continue;
	      
		GuiGroup.add(%gui);
		%gui.save(%gui.getFileName()@"tmp");
	}
		
}
//------------------------------------------------------------------------------
//==============================================================================
function ETools::onPostEditorSave(%this) {	
	foreach(%gui in EToolsGuiSet)
		ETools.add(%gui);
}
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// Toggle Tool Dialog Display - Toggle/Show/Hide
//==============================================================================
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
