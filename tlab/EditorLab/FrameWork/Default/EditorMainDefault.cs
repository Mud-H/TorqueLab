//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// TorqueLab Plugin Tools Container (Side settings area)
//==============================================================================


//==============================================================================
function EditorMainDefault::initFrameWork( %this ) {	
	devLog("EditorMainDefault::initFrameWork");
	%worldTool = EditorGuiMain-->WorldAndTools;
	%worldTool.fitIntoParents();
}
//------------------------------------------------------------------------------

//==============================================================================
function EditorMainDefault::activateFrameWork( %this ) {	
	devLog("EditorMainDefault::activateFrameWork");
	%worldTool = EditorGuiMain-->WorldAndTools;
	%worldTool.fitIntoParents();
}
//------------------------------------------------------------------------------