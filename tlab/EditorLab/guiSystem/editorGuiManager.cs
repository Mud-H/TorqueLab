//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
function Lab::initEditorGui( %this ) {
	newScriptObject("LabEditor");
	newSimGroup("LabDetachedGuiGroup");
	newSimSet("LabGuiSet");
	newSimSet("LabPluginGuiSet");
	newSimSet("LabDefaultGuiSet");
	newSimSet("LabActiveFrameSet");//empty
	newSimSet("LabEditorGuiSet");//empty
	newSimSet("LabFullEditorSet");//empty
	newSimSet("LabToolbarGuiSet");
	newSimSet("LabSideBarGuiSet");
	newSimSet("LabGeneratedSet");//empty
	newSimSet("LabToolbarStartGuiSet");//empty
	newSimSet("LabPaletteGuiSet");
	newSimSet("LabDialogGuiSet");
	newSimSet("LabToolGuiSet");
	newSimSet("LabMainFrameSet");//empty
	newSimSet("EditorDetachedGuis");
	
	newSimSet("LabPaletteItemSet");

}
//------------------------------------------------------------------------------


//==============================================================================
// Add the various TorqueLab GUIs to the container and set they belong
function Lab::addGui(%this,%gui,%type,%noHide) {
   
   if (!isObject(LabGuiSet))
      Lab.initEditorGui();
	%parent = %gui.parentGroup;
	if (%noHide)
		%gui.noHide = true;

	switch$(%type) {
		case "Root":
			%container = "EditorGui";
			LabDefaultGuiSet.add(%gui);
			
		case "Tool":
			%container = "ToolsContainer";
			LabToolGuiSet.add(%gui);
			LabMainFrameSet.add(%gui);

		case "EditorGui":
			%container = "EditorContainer";
			LabEditorGuiSet.add(%gui);
			LabMainFrameSet.add(%gui);

		case "FullEditor":
			%container = "FullEditorContainer";
			LabFullEditorSet.add(%gui);
			LabMainFrameSet.add(%gui);

		case "Toolbar":
			%container = "ToolbarContainer";
			LabToolbarGuiSet.add(%gui);

		case "SideBar":		
			%container = "SideBarContainer";
			LabSideBarGuiSet.add(%gui);
			LabMainFrameSet.add(%gui);
		case "Dialog":
			%container = "DialogContainer";
			LabDialogGuiSet.add(%gui);
			LabMainFrameSet.add(%gui);

		case "Palette":
			LabPaletteGuiSet.add(%gui);
			%container = "Custom";		
		
		case "":
			warnLog("Adding a Gui with no type:",%gui,%gui.getName());
			%container = "ToolsContainer";
			LabDialogGuiSet.add(%gui);
		}
		
		%gui.containerType = %type;
		%gui.container = %container;
		%gui.defaultParent = %gui.parentGroup;
		LabGuiSet.add(%gui); // Simset Holding all Editor Guis	
	
		if (%gui.container !$= "")
			Lab.attachEditorGui(%gui);
		return;
		
		
}
//------------------------------------------------------------------------------
function LabDialog::onAdd( %this ) {
	Lab.addGui(%this ,"Dialog");	
}
function Lab::resetGuis( %this ) {
	foreach(%obj in LabGuiSet)
	{
		%cont = EditorGui.findObjectByInternalName(%obj.container,true);
		%cont.add(%obj);
	}
	
}
function Toolbar::onAdd( %this ) {
	Lab.addGui( %this.getName() ,"Toolbar");	
}
//==============================================================================
// Detach the GUIs not saved with EditorGui (For safely save EditorGui)
//==============================================================================
//==============================================================================
function Lab::detachAllEditorGuis(%this) {
	%this.editorGuisDetached = true;	
		
	foreach(%gui in LabGeneratedSet)
		%this.detachEditorGui(%gui);

	foreach(%gui in LabGuiSet)
		%this.detachEditorGui(%gui);

	foreach(%gui in LabPaletteItemSet)
		%this.detachEditorGui(%gui,true);

	foreach(%obj in LabPaletteArray)
		%paletteList = strAddWord(%paletteList,%obj.getId());

	foreach$(%id in %paletteList)
		%this.detachEditorGui(%id,true);

	//foreach(%gui in LabSideBarGuiSet)
		//Lab.detachEditorGui(%gui);

	%this.resetPluginsBar();
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::detachEditorGui(%this,%gui,%dontShow) {
	if (%gui.isCore)
		return;
	if(%gui.container !$= "Custom" && %gui.container !$= "")
		EditorDetachedGuis.add(%gui);
	//%gui.editorParent = %gui.parentGroup;
	%parent = %gui.defaultParent;

	if (!isObject(%parent))
		%parent = GuiGroup;

	%parent.add(%gui);

	if (%dontShow)
		return;

	show(%gui);
}
//------------------------------------------------------------------------------
//==============================================================================
// Reattach the GUIs not saved with EditorGui (For safely save EditorGui)
//==============================================================================
//==============================================================================
function Lab::attachAllEditorGuis(%this) {
	//if (!Lab.editorGuisDetached)
		//return;	
	Lab.editorGuisDetached = false;

	foreach(%gui in EditorDetachedGuis)	
		Lab.attachEditorGui(%gui);

	foreach(%item in LabPaletteItemSet) {
		EditorGui-->ToolsPaletteArray.add(%item);
	}

	Lab.updatePluginsBar();
	//Now make sure everything fit together
	%this.resizeEditorGui();
	Lab.postGuiReattached();
}
//------------------------------------------------------------------------------

//==============================================================================
function Lab::attachEditorGui(%this,%gui) {
	//Ignore custome or empty container
	if(%gui.container $= "Custom" || %gui.container $= "")
		return;
	%myContainer = EditorGui.findObjectByInternalName(%gui.container,true);
	//If no container found, it might be the container directly
	if (!isObject(%myContainer)){
		%myContainer = %gui.container;
		if (!isObject(%myContainer))
		{
			warnLog("Trying to attach a Gui to the editor but can't find a valid container:",%gui);
			return;
		}
	}		
	%gui.parentGroup = %myContainer;
	if (%gui.isCommon || %gui.noHide)
		show(%gui);
	else
		hide(%gui);
		
	
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::checkGuiConfig( %this,%gui ) {	
	%cmds = $TLGuiCmd[%gui.getName()];
	if (%cmds !$= ""){		
		for(%i=0;%i<getFieldCount(%cmds);%i++){
			eval("%gui."@getField(%cmds,%i));			
		}
	}	
}
//------------------------------------------------------------------------------
// TO BE REMOVED
//==============================================================================
function Lab::postGuiReattached( %this ) {	
		//$LabToolsPaletteContainer = EditorGui-->ToolsPaletteContainer;
	//$LabToolsPaletteArray = EditorGui-->ToolsPaletteArray;	
	$LabPluginBar = EditorGui-->PluginsToolbar;
	$LabPluginBarDecoy = $LabContPluginBar-->decoy;
	$LabPaletteBar = EditorGui-->ToolsPaletteWindow;
	$LabEditor= EditorGui-->EditorContainer;
	$LabSideBar = EditorGui-->SideBarContainer;
	
	
	foreach(%gui in EditorDetachedGuis)	
		Lab.checkGuiConfig(%gui);
}
//==============================================================================
// TO BE REMOVED
//==============================================================================
function Lab::setDefaultGuis( %this ) {		
	return;
	
}
//------------------------------------------------------------------------------