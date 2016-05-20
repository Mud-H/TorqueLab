//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
function FileBrowserIcon::onMouseDragged( %this,%a1,%a2,%a3 ) {
	if (!isObject(%this)) {
		devLog("FileBrowserIcon class is corrupted! Fix it!");
		return;
	}

	startDragAndDropCtrl(%this,"FileBrowserIcon","FileBrowser.onIconDropped");
	hide(%this);
}
//------------------------------------------------------------------------------
//==============================================================================
function FileBrowserFav::onMouseDragged( %this,%a1,%a2,%a3 ) {
	if (!isObject(%this)) {
		devLog("FileBrowserFav class is corrupted! Fix it!");
		return;
	}

	FileBrowserFavRemoveBin.visible = 1;
	startDragAndDropCtrl(%this,"FileBrowserFav","FileBrowser.onIconDropped");
	hide(%this);
}
//------------------------------------------------------------------------------
//==============================================================================
// Dropping a file icon
//==============================================================================


//==============================================================================
function FileBrowser::onIconDropped( %this,%droppedOn,%icon,%dropPoint ) {
	%originalIcon = %icon.dragSourceControl;
	show(%originalIcon);
	delObj(%icon);
	devLog("Icon dropped on ctrl:",%droppedOn,%droppedOn.getName());
	
	if (%originalIcon.class $= "FileBrowserFav")
		FileBrowserFavRemoveBin.visible =0;

	if (FileBrowser.isMethod("checkIconDrop"@%originalIcon.type))
		eval("FileBrowser.checkIconDrop"@%originalIcon.type@"(%originalIcon,%droppedOn,%dropPoint);");
}
//------------------------------------------------------------------------------

//==============================================================================
function FileBrowser::checkIconDropMesh( %this,%icon,%droppedOn,%dropPoint) {
	devLog(%icon,"Mesh Icon dropped on ctrl:",%droppedOn,%droppedOn.getName());

	if (%droppedOn.getName() $= "EWorldEditor") {
		//%worldPos = screenToWorld(%dropPoint);
		//devLog("Drop point",%dropPoint,"World",%worldPos);
		//$SceneEditor_DropSinglePosition = %worldPos;
		ColladaImportDlg.showDialog(%icon.fullPath,%icon.createCmd);
	} else if (%droppedOn.getClassName() $= "GuiShapeLabPreview") {
		//%worldPos = screenToWorld(%dropPoint);
		//devLog("Drop point",%dropPoint,"World",%worldPos);
		//$SceneEditor_DropSinglePosition = %worldPos;
		ShapeLab.selectFilePath(%icon.fullPath);
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function FileBrowser::checkIconDropPrefab( %this,%icon,%droppedOn,%dropPoint) {
	devLog(%icon,"Prefab Icon dropped on ctrl:",%droppedOn,%droppedOn.getName());

	if (%droppedOn.getName() $= "EWorldEditor") {
		//%worldPos = screenToWorld(%dropPoint);
		//devLog("Drop point",%dropPoint,"World",%worldPos);
		//$SceneEditor_DropSinglePosition = %worldPos;
		eval(%icon.createCmd);
	}

	/*else if (%droppedOn.getClassName() $= "GuiShapeLabPreview"){

		//%worldPos = screenToWorld(%dropPoint);
		//devLog("Drop point",%dropPoint,"World",%worldPos);
		//$SceneEditor_DropSinglePosition = %worldPos;
		ShapeLab.selectFilePath(%icon.fullPath);

	}*/
}
//------------------------------------------------------------------------------
//==============================================================================
function FileBrowser::checkIconDropImage( %this,%icon,%droppedOn,%dropPoint) {
	devLog("Image Icon dropped on ctrl:",%droppedOn,"At Pos",%dropPoint);

	if (%droppedOn.class $= "MaterialMapCtrl") {
		%type = %droppedOn.internalName;

		if (MaterialEditorTools.isMethod("update"@%type@"Map")) {
			devLog("Material map found, update called for map:",%type,"File",%icon.fullPath);
			eval("MaterialEditorTools.update"@%type@"Map(true,%icon.fullPath,false);");
		}
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function FileBrowser::checkIconDropFav( %this,%icon,%droppedOn,%dropPoint) {
	devLog("Favorite Icon dropped on ctrl:",%droppedOn,"At Pos",%dropPoint,"superC",%droppedOn.superclass);

	if (%droppedOn.dropType $= "FileBrowserFavButton") {
		%type = %droppedOn.internalName;
		FileBrowserFavArray.reorderChild(%icon,%droppedOn);
		FileBrowserFavArray.refresh();
		
	} else if (%droppedOn.superclass $= "FileBrowserFavRemover") {
		FileBrowser.removeFavoriteIcon(%icon);
		
	}
}
//------------------------------------------------------------------------------
