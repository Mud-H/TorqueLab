//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// TorqueLab Plugin Tools Container (Side settings area)
//==============================================================================

devLog("Execed");
//==============================================================================
function EditorMainStock::initFrameWork( %this ) {	
	devLog("EditorMainStock::initFrameWork");
	%worldTool = EditorGuiMain-->WorldAndTools;
	%worldTool.fitIntoParents();
	
	EditorGui-->ToolsContainer.forceInsideCtrl(EditorGuiMain);
	EditorGui-->SidebarContainer.forceInsideCtrl(EditorGuiMain);
	hide(EditorGui-->ToolsContainer-->ToolsToggle);
}
//------------------------------------------------------------------------------
//==============================================================================
function EditorMainStock::activateFrameWork( %this ) {	
	devLog("EditorMainStock::activateFrameWork");
	%worldTool = EditorGuiMain-->WorldAndTools;
	%worldTool.fitIntoParents();
	

}
//------------------------------------------------------------------------------


