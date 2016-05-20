//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// TorqueLab Plugin Tools Container (Side settings area)
//==============================================================================


//==============================================================================
function EditorMainFWB::initFrameWork( %this ) {	
	devLog("EditorMainFWB::initFrameWork");
	%worldTool = EditorGuiMain-->WorldAndTools;
	%worldTool.fitIntoParents();
}
//------------------------------------------------------------------------------

//==============================================================================
function EditorMainFWB::activateFrameWork( %this ) {	
	devLog("EditorMainFWB::activateFrameWork");
	%worldTool = EditorGuiMain-->WorldAndTools;
	%worldTool.fitIntoParents();
}
//------------------------------------------------------------------------------