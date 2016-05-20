//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
function ArrayIconCtrl::onMouseDragged( %this,%a1,%a2,%a3 ) {
	if (!isObject(%this)) {
		devLog("ArrayIconCtrl class is corrupted! Fix it!");
		return;
	}

	startDragAndDropCtrl(%this,"ArrayIconCtrl","ObjectCreator.onIconDropped");
	hide(%this);
}
//------------------------------------------------------------------------------

//==============================================================================
// Dropping a file icon
//==============================================================================


//==============================================================================
function ObjectCreator::onIconDropped( %this,%droppedOn,%icon,%dropPoint ) {
	%originalIcon = %icon.dragSourceControl;
	show(%originalIcon);
	delObj(%icon);
	devLog(%icon.type,"Icon dropped on ctrl:",%droppedOn,%droppedOn.getName());

	if (ObjectCreator.isMethod("checkIconDrop"@%originalIcon.type))
		eval("ObjectCreator.checkIconDrop"@%originalIcon.type@"(%originalIcon,%droppedOn,%dropPoint);");
}
//------------------------------------------------------------------------------

//==============================================================================
function ObjectCreator::checkIconDropMissionObject( %this,%icon,%droppedOn,%dropPoint) {
	devLog(%icon,"MissionObject Icon dropped on ctrl:",%droppedOn,%droppedOn.getName());

	if (%droppedOn.getName() $= "EWorldEditor") {
		devLog("Evalthis:",%icon.altCommand);
		eval(%icon.altCommand);
		//%worldPos = screenToWorld(%dropPoint);
		//devLog("Drop point",%dropPoint,"World",%worldPos);
		//$SceneEditor_DropSinglePosition = %worldPos;
		//ColladaImportDlg.showDialog(%icon.fullPath,%icon.createCmd);
	} else if (%droppedOn.getClassName() $= "GuiShapeLabPreview") {
		//%worldPos = screenToWorld(%dropPoint);
		//devLog("Drop point",%dropPoint,"World",%worldPos);
		//$SceneEditor_DropSinglePosition = %worldPos;
		ShapeLab.selectFilePath(%icon.fullPath);
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function ObjectCreator::checkIconDropImage( %this,%icon,%droppedOn,%dropPoint) {
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
