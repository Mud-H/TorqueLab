//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
function ObjectCreatorViewMenu::onSelect( %this,%id,%text ) {
	$ObjectCreator_ViewId = %id;
	ObjectCreator.setViewId($ObjectCreator_ViewId);
}
//==============================================================================
//ObjectCreator.initFiles
function ObjectCreator::setViewId(%this,%id,%force) {
	if (%id $= "")
		%id = ObjectCreator.currentViewId;
	if (%id $= "")
		return;	

	%text = getField($ObjectCreator_View[%i],%id);

	switch$(%id) {
	case "0":
		%width = (ObjectCreatorArray.extent.x - 10);

	case "1":
		%width = (ObjectCreatorArray.extent.x - 10) /2;

	case "2":
		%width = (ObjectCreatorArray.extent.x - 10) /3;
	}

	if (%width !$= "") {
		ObjectCreatorArray.colSize = %width;
		ObjectCreatorArray.refresh();
		ObjectCreator.currentViewId = %id;
		ObjectCreatorViewMenu.setText(getField($ObjectCreator_View[%id],0));
	}
}
//------------------------------------------------------------------------------
function ObjectCreator::onResized(%this) {
   
	//ObjectCreator.setViewId();
}

//==============================================================================

//==============================================================================
function ObjectCreator::iconFolderAlt(%this,%icon) {	
	ObjectCreator.schedule(0,"navigateDown",%icon.text);
}
//------------------------------------------------------------------------------
//==============================================================================
function ObjectCreator::iconMeshAlt(%this,%icon) {
	if (Lab.currentEditor.isMethod("addObjectCreatorMesh"))
		Lab.currentEditor.addObjectCreatorMesh(%icon.fullPath,%icon.createCmd);
	else if (EWorldEditor.visible)
		ColladaImportDlg.showDialog(%icon.fullPath,%icon.createCmd);
}
//------------------------------------------------------------------------------
//==============================================================================
function ObjectCreator::iconImageAlt(%this,%icon) {
	
}
//------------------------------------------------------------------------------
//==============================================================================
function ObjectCreator::iconUnknownAlt(%this,%icon) {
	info("Unknown file clicked! TorqueLab doesn't compute this file...",%icon.fullPath);
}
//------------------------------------------------------------------------------