//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
function FileBrowserFrameSet::onResized( %this ) {
	logd("FileBrowserFrameSet::onResized");
}
function GuiFrameSetCtrl::onResized( %this ) {
	logd("GuiFrameSetCtrl::onResized",%this,%this.getName());
}
//==============================================================================
function FileBrowser::toggleFavorites( %this ) {
	

	
	if (FileBrowserFavFrame.visible){
		hide(FileBrowserFavFrame);
		FileBrowserFrameSet.reorderChild(FileBrowserFileFrame,FileBrowserFavFrame);
		FileBrowserFrameSet.defaultRows = FileBrowserFrameSet.rows;
		FileBrowserFrameSet.rows = "0";
	}
	else {
		show(FileBrowserFavFrame);
		FileBrowserFrameSet.reorderChild(FileBrowserFavFrame,FileBrowserFileFrame);
		%rows = FileBrowserFrameSet.defaultRows;
		if (getWordCount(%rows)!$= "2")
			%rows = "0 100";
		FileBrowserFrameSet.rows = %rows;	
	}
	FileBrowserFrameSet.updateSizes();
	
}

function FileBrowser::addCurrentToFavorite(%this) {
	
	%path = FileBrowser.lastAddress;
	info("Adding:",%path,"To FileBrowser favorites");
	%fileRead = getFileReadObj("tlab/EditorLab/SideBar/FileBrowser/settings/storedFavs.txt");
	%id = 0;
	while( !%fileRead.isEOF() ) {
		%line = %fileRead.readLine();
		devLog("LINE:",%line);
		if (%line $= %path)
			%exist = true;
		
			
	}
	closeFileObj(%fileRead);
	
	if (%exist){
		info("The folder is already a favorite");
		return;
	}
	%fileWrite= getFileWriteObj("tlab/EditorLab/SideBar/FileBrowser/settings/storedFavs.txt",true);
	%fileWrite.writeLine(%path);
	closeFileObj(%fileWrite);
	FileBrowser.getFavorites();
}
//------------------------------------------------------------------------------
//==============================================================================
function FileBrowser::addFavoriteIcon( %this, %text,%dropZone ) {
	%folder = strReplace(%text," ","/");
	%ctrl = Lab.createArrayIcon(FileBrowserIconSrc);
	
	%ctrl.iconBitmap = "";
	%ctrl.text = %folder;
	%ctrl.address = %text;
	%ctrl.tooltip = %text;
	if (!%dropZone){
		%ctrl.command = "FileBrowser.navigateFav($ThisControl);";
	%ctrl.altCommand = "FileBrowser.navigateFav($ThisControl);";
		%ctrl.class = "FileBrowserFav";
	}
	else {
		%ctrl.command = "";
		%ctrl.altCommand = "";
	}

	%ctrl.dropType = "FileBrowserFavButton";
	%ctrl.buttonType = "radioButton";
	%ctrl.groupNum = "-1";
	%ctrl.buttonMargin = "6 0";
	%ctrl.textMargin = "8";
	%ctrl.sizeIconToButton = true;
	%ctrl.makeIconSquare = true;
	%ctrl.extent.y = "20";
	%ctrl.type = "Fav";
	FileBrowserFavArray.addGuiControl( %ctrl );
}
//------------------------------------------------------------------------------
//==============================================================================
function FileBrowser::navigateFav( %this, %icon ) {
	
		%folder = %icon.address;
	logd("navigateFav adress:",%folder);
	// Because this is called from an IconButton::onClick command
	// we have to wait a tick before actually calling navigate, else
	// we would delete the button out from under itself.
	%this.schedule( 1, "navigate", %folder );
}
//------------------------------------------------------------------------------
//==============================================================================
//FileBrowser.getFavorites();
function FileBrowser::getFavorites(%this) {
	FileBrowserFavArray.clear();
/*	if (isObject("FileBrowserFavArray")){
		FileBrowserFavArray.empty();
		FileBrowser.favArray = FileBrowserFavArray;
	}
	else{
		FileBrowser.favArray = newArrayObject("FileBrowserFavArray");
	}*/
	%fileRead = getFileReadObj("tlab/EditorLab/SideBar/FileBrowser/settings/storedFavs.txt");
	%id = 0;
	while( !%fileRead.isEOF() ) {
		%line = %fileRead.readLine();		
		if (%line $= "")
			continue;
		
		%this.addFavoriteIcon(%line);
			
	}
	closeFileObj(%fileRead);
	%this.addFavoriteIcon("<>",true);
	FileBrowserFavArray.updateStack();
FileBrowserFavArray.getParent().computeSizes();

}
//------------------------------------------------------------------------------
//==============================================================================
//FileBrowser.getFavorites();
function FileBrowser::removeFavoriteIcon(%this,%icon) {
	%address = %icon.address;
	%newText = "";
	%fileRead = getFileReadObj("tlab/EditorLab/SideBar/FileBrowser/settings/storedFavs.txt");
	%id = 0;
	while( !%fileRead.isEOF() ) {
		%line = %fileRead.readLine();
		devLog("LINE:",%line);
		if (%line $= %address)
			continue;
		%newText = strAddRecord(%newText,%line);			
	}
	closeFileObj(%fileRead);
	
	%fileWrite= getFileWriteObj("tlab/EditorLab/SideBar/FileBrowser/settings/storedFavs.txt");
	for(%i = 0;%i < getRecordCount(%newText);%i++)
		%fileWrite.writeLine(getRecord(%newText,%i));
		
	closeFileObj(%fileWrite);
	FileBrowserFavArray.remove(%icon);
	

	FileBrowserFavArray.refresh();
FileBrowserFavArray.getParent().computeSizes();

}
//------------------------------------------------------------------------------
//==============================================================================
//FileBrowser.getFavorites();
function FileBrowser::favPanelPos(%this,%location) {
	if (%location $= "Top"){
		FileBrowserFrameSet.reorderChild(FileBrowserFavFrame,FileBrowserFileFrame);
		SideFavIconToBottom.visible = 1;
		SideFavIconToTop.visible = 0;
	}
	else if (%location $= "Bottom"){
		FileBrowserFrameSet.reorderChild(FileBrowserFileFrame,FileBrowserFavFrame);
		SideFavIconToBottom.visible = 0;
		SideFavIconToTop.visible = 1;
	}
	SideFavIconToTop.getParent().updateStack();
	FileBrowserFrameSet.updateSizes();
}
//------------------------------------------------------------------------------

