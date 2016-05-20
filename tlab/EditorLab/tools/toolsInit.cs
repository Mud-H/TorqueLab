//==============================================================================
// TorqueLab -> Editor Gui Open and Closing States
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// Initial Editor launch call from EditorManager
function initLabTools() {Lab.initEditorTools();}
function Lab::initEditorTools(%this) {
	if (!%this.editorToolsLoaded)
		execPattern("tlab/EditorLab/tools/*.gui");
	execPattern("tlab/EditorLab/tools/*.cs");	
	%this.editorToolsLoaded = true;
	
	LabMat.init();
}
//-----------------------------------------------------------------------------


//------------------------------------------------------------------------------

