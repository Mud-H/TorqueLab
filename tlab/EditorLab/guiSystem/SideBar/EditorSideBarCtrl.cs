//==============================================================================
// TorqueLab -> WorldEditor Grid and Snapping System
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================


//==============================================================================
// EditorSideBarCtrl Functions
//==============================================================================
function EditorSideBarCtrl::onWake(%this) {	

	//WIP Fast - Called from EditorFrameContent
	//Lab.schedule(100,"initSideBar");
}


function EditorSideBarCtrl::onPreEditorSave(%this) {
	
	FileBrowserArray.clear();
	ObjectCreatorArray.clear();
	return;
SideBarVIS-->theVisOptionsList.clear();
SideBarVIS-->theClassVisList.clear();
	SideBarVIS-->theClassVisArray.clear();
	SideBarVIS-->theClassSelList.clear();
	SideBarVIS-->theClassSelArray.clear();
}
function EditorSideBarCtrl::onPostEditorSave(%this) {
	
	//SideBarVIS.init();
	
	FileBrowser.initFiles();
	ObjectCreator.initFiles();
	
}
//==============================================================================
// SideBar Tab Book
//==============================================================================

//==============================================================================
//Set a plugin as active (Selected Editor Plugin)
function SideBarMainBook::onTabSelected(%this,%text,%id) {
	logd("SideBarMainBook::onTabSelected",%text,%id);
	$SideBarMainBook_CurrentPage = %id;

	switch$(%id) {
	case "0":
		if (!$FileBrowserInit && isObject(FileBrowser))
			Lab.initFileBrowser();

	case "1":	
		Lab.schedule(200,"initSceneBrowser");
	}
}
//------------------------------------------------------------------------------
