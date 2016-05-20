//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$FileBrowser_LockUp = true;

//==============================================================================
//FileBrowser.initFiles
function FileBrowserViewMenu::onSelect( %this,%id,%text ) {
	$FileBrowser_ViewId = %id;
	FileBrowser.setViewId($FileBrowser_ViewId);
}
//------------------------------------------------------------------------------

//==============================================================================
//FileBrowser.setViewId
function FileBrowser::setViewId(%this,%id,%force) {
	//if (FileBrowser.currentViewId $= %id && !%force)
		//return;
	if (%id $= "")
		%id = FileBrowser.currentViewId;
	if (%id $= "")
		return;	
	%text = getField($FileBrowser_View[%i],%id);
	
	FileBrowserArray.extent.x = FileBrowserArray.getParent().extent.x;
	switch$(%id) {
	case "0":
		%width = (FileBrowserArray.extent.x - 10);

	case "1":
		%width = (FileBrowserArray.extent.x - 10) /2;

	case "2":
		%width = (FileBrowserArray.extent.x - 10) /3;
	}

	if (%width !$= "") {
		FileBrowserArray.colSize = %width;
		FileBrowserArray.refresh();
		FileBrowser.currentViewId = %id;
		FileBrowserViewMenu.setText(getField($FileBrowser_View[%id],0));
	}
}
//------------------------------------------------------------------------------
/*if ( %this.isList ) {
	%width = FileBrowserArray.extent.x - 6;
	FileBrowserArray.colSize = %width;
	%extent = %width SPC "20";
	//%ctrl.extent = %extent;
	//%ctrl.autoSize = true;
} else {
	%width = (FileBrowserArray.extent.x - 10) /2;
	//%extent = %width SPC "20";
	//%ctrl.extent = %extent;
	FileBrowserArray.colSize = %width;
}*/
//==============================================================================



//==============================================================================
function FileBrowser::createImageIcon( %this ) {
	
		%ctrl = cloneObject(FileBrowserIconImgSrc);
		%ctrl.profile = "ToolsButtonArray";
		%ctrl.buttonType = "radioButton";
		%ctrl.groupNum = "-1";
	
		
	
	return %ctrl;
}
//------------------------------------------------------------------------------
//==============================================================================
function FileBrowser::createIcon( %this ) {
	
		%ctrl = cloneObject(FileBrowserIconSrc);
		%ctrl.profile = "ToolsButtonArray";
		%ctrl.buttonType = "radioButton";
		%ctrl.groupNum = "-1";	
	
	return %ctrl;
}
//------------------------------------------------------------------------------

//==============================================================================
function FileBrowser::addFileItem( %this, %fullPath ) {

	%ext = fileExt( %fullPath );
	%stockExt = strreplace(%ext,".","");
%file = fileBase( %fullPath );
	if (!$Cfg_FileBrowser_ShowCachedDts)
		if (strFind(%fullPath,".cached.dts"))
			return;
		
	if (strFindWords(strlwr(%ext),"dae dts")) {
		%createCmd = "Scene.createMesh( \"" @ %fullPath @ "\" );";
		%type = "Mesh";
		
	} else if (%stockExt $= "prefab") {
		%createCmd = "Scene.createPrefab( \"" @ %fullPath @ "\" );";
		%type = "Prefab";
	
	} else if (strFindWords(strlwr(%ext),"png tga jpg bmp dds")) {
		%type = "Image";
		if ($FileBrowserOnlyDiffuse){
			%end = getSubStr(%file,strLen(%file)-4);
			devLog("End is:",%end);
			if (!strFind(%end,"_d"))
				return;
		}
			
	} else	{
		%type = "Unknown";
	}

	
	%fileLong = %file @ %ext;
	%tip = %fileLong NL
			 "Size: " @ fileSize( %fullPath ) / 1000.0 SPC "KB" NL
			 "Date Created: " @ fileCreatedTime( %fullPath ) NL
			 "Last Modified: " @ fileModifiedTime( %fullPath );
	if (%type $= "image" && isImageFile(%fullPath) && $FileBrowserShowImageIcon){
		%ctrl = %this.createImageIcon();
		%iconBmp = %fullPath;
	}
	else{	
		%ctrl = Lab.createArrayIcon(FileBrowserIconSrc);
		%iconBmp = "tlab/art/icons/default/files/"@%stockExt@".png";
	}

	if (!isFile(%iconBmp))
		%iconBmp = "tlab/art/icons/default/files/default.png";

	if (%createCmd !$= "")
		%ctrl.createCmd = %createCmd;
	%ctrl.iconBitmap = %iconBmp;
	%ctrl.altCommand = "FileBrowser.icon"@%type@"Alt($ThisControl);";
	//%ctrl.iconBitmap = ( ( %ext $= ".dts" ) ? EditorIconRegistry::findIconByClassName( "TSStatic" ) : "tlab/art/icons/default/iconCollada" );
	%ctrl.text = %file;
	%ctrl.type = %type;
	%ctrl.class = "FileBrowserIcon";
	//%ctrl.superClass = "FileBrowserIcon"@%type;
	%ctrl.tooltip = %tip;
	%ctrl.buttonType = "radioButton";
	%ctrl.groupNum = "-1";
	%ctrl.fullPath = %fullPath;
	FileBrowserArray.addGuiControl( %ctrl );
}

//==============================================================================
function FileBrowser::iconMeshAlt(%this,%icon) {
   %validPath = strreplace(%icon.fullPath,"//","/");
	if (Lab.currentEditor.isMethod("addFileBrowserMesh"))
		Lab.currentEditor.addFileBrowserMesh(%validPath,%icon.createCmd);
	else if (EWorldEditor.visible)
		ColladaImportDlg.showDialog(%validPath,%icon.createCmd);
}
//------------------------------------------------------------------------------
//==============================================================================
function FileBrowser::iconPrefabAlt(%this,%icon) {
	if (Lab.currentEditor.isMethod("addFileBrowserPrefab"))
		Lab.currentEditor.addFileBrowserPrefab(%icon.fullPath,%icon.createCmd);
	else if (EWorldEditor.visible)
		eval(%icon.createCmd);

	//ColladaImportDlg.showDialog(%icon.fullPath,%icon.createCmd);
}
//------------------------------------------------------------------------------
//==============================================================================
function FileBrowser::iconImageAlt(%this,%icon) {	
}
//------------------------------------------------------------------------------
//==============================================================================
function FileBrowser::iconUnknownAlt(%this,%icon) {
	info("Unknown file clicked! TorqueLab doesn't compute this file...",%icon.fullPath);
}
//------------------------------------------------------------------------------

//==============================================================================
// Browser Folder Icons Functions
//==============================================================================
//==============================================================================
function FileBrowser::addFolderItem( %this, %text,%fullDir ) {
	%ctrl = Lab.createArrayIcon(FileBrowserIconSrc);
	%ctrl.command = "FileBrowser.iconFolderAlt($ThisControl);";
	%ctrl.altCommand = "FileBrowser.iconFolderAlt($ThisControl);";
	%ctrl.iconBitmap = "tlab/art/icons/24-assets/folder_open.png";
	%ctrl.text = %text;
	%ctrl.fullDir = %fullDir;
	%ctrl.tooltip = %text;
	//%ctrl.class = "CreatorFolderIconBtn";
	%ctrl.buttonType = "radioButton";
	%ctrl.groupNum = "-1";
	%ctrl.buttonMargin = "6 0";
	%ctrl.sizeIconToButton = true;
	%ctrl.makeIconSquare = true;
	FileBrowserArray.addGuiControl( %ctrl );
}
//------------------------------------------------------------------------------

//==============================================================================
function FileBrowser::iconFolderAlt(%this,%icon) {
	
	FileBrowser.schedule(0,"goToFolder",%icon.fullDir);
}
//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
//FileBrowser.addFolderUpIcon
function FileBrowser::addFolderUpIcon( %this ) {
	%ctrl = Lab.createArrayIcon(FileBrowserIconSrc);
	%ctrl.command = "FileBrowser.navigateUp();";
	%ctrl.altCommand = "FileBrowser.navigateUp();";
	%ctrl.iconBitmap = "tlab/art/icons/24-assets/folder_up.png";
	%ctrl.text = "..";
	%ctrl.tooltip = "Go to parent folder";
	%ctrl.buttonMargin = "8 1";
	%ctrl.sizeIconToButton = true;
	%ctrl.makeIconSquare = true;
	//%ctrl.class = "CreatorFolderIconBtn";
	%ctrl.buttonType = "PushButton";
	FileBrowserArray.addGuiControl( %ctrl );
	FileBrowserArray.bringToFront(%ctrl);
}
//------------------------------------------------------------------------------

