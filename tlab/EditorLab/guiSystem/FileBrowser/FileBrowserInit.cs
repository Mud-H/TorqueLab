//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

$FileBrowser_View[0] = "1 Column" TAB "Col 1";
$FileBrowser_View[1] = "2 Column" TAB "Col 2";
$FileBrowser_View[2] = "3 Column" TAB "Col 3";
$FileBrowser_ViewId = 0;

$FileBrowser_DefaultFolder = "art";
$FileBrowser_FavRow = "600";
//SceneBrowserTree.rebuild
function Lab::initFileBrowser( %this ) {
	
	FileBrowser.getFavorites();
	
  FileBrowser.initFiles();
	
	FileBrowser.currentViewId = "";
	FileBrowserViewMenu.clear();
	%i = 0;

	while($FileBrowser_View[%i] !$= "") {
		%text = getField($FileBrowser_View[%i],0);
		FileBrowserViewMenu.add(%text,%i);
		%i++;
	}

	FileBrowser.setViewId($FileBrowser_ViewId);
	//FileBrowserViewMenu.setSelected($FileBrowser_ViewId);
	$FileBrowserInit = true;
	
	FileBrowser.arrayCtrl = FileBrowserArray;
	hide(FileBrowserOptionCtrl);
}


function FileBrowser::initFiles( %this ) {
	FileBrowser.getAllFolders();
	 %lastAddress = FileBrowser.lastAddress;
	if (%lastAddress $= "")
		%lastAddress = $FileBrowser_DefaultFolder;
	FileBrowser.goToAddress( %lastAddress );
	
}

//FileBrowser.initFiles
function FileBrowser::onResized(%this) {
	FileBrowser.setViewId();
}
//------------------------------------------------------------------------------
