//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// TorqueLab Plugin Tools Container (Side settings area)
//==============================================================================


//==============================================================================
function EditorMainFWA::initFrameWork( %this ) {	
	devLog("EditorMainFWA::initFrameWork");
	%worldTool = EditorGuiMain-->WorldAndTools;
	%worldTool.fitIntoParents();
}
//------------------------------------------------------------------------------

//==============================================================================
function EditorMainFWA::activateFrameWork( %this ) {	
	devLog("EditorMainFWA::activateFrameWork");
	%worldTool = EditorGuiMain-->WorldAndTools;
	%worldTool.fitIntoParents();
}
//------------------------------------------------------------------------------