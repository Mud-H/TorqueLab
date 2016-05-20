//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$FileBrowserNewSystem = true;



//==============================================================================
// New navigation system that only scan current folder for files
//==============================================================================
//==============================================================================
// Set the folder from which to show the files and folders
function FileBrowser::refreshCurrentFolder( %this ) {		
	
	%this.goToFolder(FileBrowser.currentFolder);
}
//==============================================================================
// Set the folder from which to show the files and folders
function FileBrowser::goToAddress( %this, %address ) {		
	logd("FileBrowser::goToAddress->",%address);
		%path = strReplace(trim(%address)," ","/"); 
	   %path = %path@"/";
	%this.goToFolder(%path);
}
//==============================================================================
// Set the folder from which to show the files and folders
function FileBrowser::goToFolder( %this, %folder ) {		
	logd("FileBrowser::goToFolder->",%folder);	

	if (!IsDirectory(%folder)){
	   warnLog("Invalid folder, must leave");
	   return;
	}
	if ($FileBrowser_CurrentFolder $= %folder){
	   warnLog("Folder already loaded:",%folder);
	   return;
	}
	
	
	//Check for double dash
	%folder = strreplace(%folder,"//","/");
	FileBrowser.address = strreplace(%folder,"/"," ");
	FileBrowser.currentFolder = %folder;
	$FileBrowser_CurrentFolder = %folder;
	FileBrowserArray.clear();
	FileBrowser.addFolderUpIcon();
	//Start by adding the folders
	%dirs = getDirectoryList(%folder,0);
	for(%i = 0;%i < getFieldCount(%dirs);%i++){
	   %dir = getField(%dirs,%i);
	   %fullDir = %folder@"/"@%dir;	    
	   %this.addFolderItem( %dir,%fullDir );
	   //Check if this is a new folder
	   %r = FileBrowserMenu.findText( %dir );
		if ( %r == -1 ) {
			FileBrowserMenu.add( %dir );
			%sortMenu = true;
		}
	}
	
	//Add the files after the folder
	%exts = "dts dae png dds tga prefab";
	%files = getMultiExtensionFileList(%folder,%exts,true);
	for(%i = 0;%i < getRecordCount(%files);%i++){
	   %file = getRecord(%files,%i);
	    %fullPath = %folder@"/"@%file;	   
	    %this.addFileItem( %fullPath );
	}	
	
	
	%this.setViewId($FileBrowser_ViewId);
	FileBrowserArray.refresh();
	if (%sortMenu)
	   FileBrowserMenu.sort();
	FileBrowserMenu.setText(%folder);
}

//==============================================================================
function FileBrowser::goToCurrentFolder(%this) {
	%sel = EWorldEditor.getSelectedObject(0);

	if (!isObject(%sel))
		return;

	%path = "No path found";

	if (isFile(%sel.shapeName)) {
		%file = %sel.shapeName;
		%path = filePath(%file);
	} else if (isFile(%sel.fileName)) {
		%file = %sel.fileName;
		%path = filePath(%file);
	}

	//%address = strReplace( %path,"/"," ");
	%this.goToFolder(%path);
}
//------------------------------------------------------------------------------
//==============================================================================
function FileBrowser::navigateDown( %this, %folder ) {
	if ( %this.address $= "" )
		%address = %folder;
	else
		%address = %this.address SPC %folder;		
	
	// Wait after function to make sure icon is deleted
   %this.schedule( 1, "goToAddress", %address );
  
}
//------------------------------------------------------------------------------
//==============================================================================
//FileBrowser.navigateUp
function FileBrowser::navigateUp( %this ) {
	%count = getWordCount( %this.address );

	if ( %count == 0 )
		return;

	if ( %count == 1 )
		%address = "";
	else
		%address = getWords( %this.address, 0, %count - 2 );
		
   // Wait after function to make sure icon is deleted	
   %this.schedule( 1, "goToAddress", %address );
 
}
//------------------------------------------------------------------------------

//==============================================================================
// Set the folder from which to show the files and folders
function FileBrowser::getAllFolders( %this ) {	
   //Start by adding the folders
   FileBrowserMenu.clear( );
	%dirs = getDirectoryList($FileBrowser_DefaultFolder,100);
	for(%i = 0;%i < getFieldCount(%dirs);%i++){
	   %dir = getField(%dirs,%i);	   
	   FileBrowserMenu.add( $FileBrowser_DefaultFolder@"/"@%dir,%i++ );
	}
   FileBrowserMenu.sort();
}
//------------------------------------------------------------------------------
//==============================================================================
function FileBrowserMenu::onSelect( %this, %id, %text ) {
	%split = strreplace( trim(%text), "//", "/" );	
	FileBrowser.goToFolder( %split );
}
//------------------------------------------------------------------------------