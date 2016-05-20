//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// TorqueLab Plugin Tools Container (Side settings area)
//==============================================================================


//==============================================================================
function EditorMainFWC::initFrameWork( %this ) {	
	devLog("EditorMainFWC::initFrameWork");
	%worldTool = EditorGuiMain-->WorldAndTools;
	%worldTool.fitIntoParents();
}
//------------------------------------------------------------------------------

//==============================================================================
function EditorMainFWC::activateFrameWork( %this ) {	
	devLog("EditorMainFWC::activateFrameWork");
	%worldTool = EditorGuiMain-->WorldAndTools;
	%worldTool.fitIntoParents();
}
//------------------------------------------------------------------------------