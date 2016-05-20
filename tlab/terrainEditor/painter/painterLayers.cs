//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
function EPainterSplatMapWin::onCollapse( %this ) {
	devLog("Index changed from:",%this.lockIndex,"To",%index);
	//EPainterPreview.reorderChild(EPainterSplatMapWin,EPainter);
	
}
function EPainterSplatMapWin::onRestore( %this ) {
	devLog("Index changed from:",%this.lockIndex,"To",%index);
	//EPainterPreview.reorderChild(EPainterSplatMapWin,EPainter);
	
}


//==============================================================================
// TerrainMaterial Preview
//==============================================================================
function EPainter::toggle4MatPreview( %this ) {
	%frame = EPainter_4MatPreview;
}
//==============================================================================
// TerrainPainterPlugin
//==============================================================================

function EPainter::onActivated( %this ) {
	%this.setDisplayModes();
	// Update the layer listing.
	//%this.updateLayers( %matIndex );
	// Automagically put us into material paint mode.
	ETerrainEditor.currentMode = "paint";
	ETerrainEditor.selectionHidden = true;
	ETerrainEditor.currentAction = paintMaterial;
	ETerrainEditor.currentActionDesc = "Paint material on terrain";
	ETerrainEditor.setAction( ETerrainEditor.currentAction );
	EditorGuiStatusBar.setInfo(ETerrainEditor.currentActionDesc);
	ETerrainEditor.renderVertexSelection = true;
	EPainter-->saveDirtyMaterials.active = 0;
}


//==============================================================================
// Painter Active Layers Functions
//==============================================================================


//==============================================================================
// Update the active material layers list
function EPainter::setPaintLayer( %this, %matIndex) {
	%ctrl = EPainterStack.findObjectByInternalName("Layer_"@%matIndex,true);
	%terrainMat = %ctrl.terrainMat;
	%pill = EPainterStack.getObject(%matIndex);
	%this.updateSelectedLayerList(%ctrl);

	foreach(%ctrl in EPainterStack)
		%ctrl.isActiveCtrl.visible = 0;

	%pill.isActiveCtrl.visible = 1;

	if (!isObject( %terrainMat )) {
		warnLog("Index",%matIndex,"Set Paint Layer to invalid material. From Button:",%ctrl,%ctrl.terrainMat);
		return;
	}

	EPainter.activeMat = %terrainMat;
	EPainter.activePill = EPainterStack.findObjectByInternalName("Layer_"@%matIndex,true);
	EPainter.activeMatIndex = %matIndex;
	//assert( isObject( %terrainMat ), "TerrainEditor::setPaintMaterial - Got bad material!" );
	ETerrainEditor.paintIndex = %matIndex;
	ETerrainMaterialSelected.selectedMatIndex = %matIndex;
	ETerrainMaterialSelected.selectedMat = %terrainMat;
	foreach$(%map in $TerMat_BitmapFields){
		if (isImageFile(%terrainMat.getFieldValue(%map))){
			%ctrl = EPainter_4MatPreview.findObjectByInternalName(%map,true);
			%ctrl.setBitmap(%terrainMat.getFieldValue(%map));
		}
	}
	//ETerrainMaterialSelected.bitmap = %terrainMat.diffuseMap;
	//ETerrainMaterialSelected_N.bitmap = %terrainMat.normalMap;
	//ETerrainMaterialSelected_M.bitmap = %terrainMat.macroMap;
	//ETerrainMaterialSelectedEdit.Visible = isObject(%terrainMat);
	TerrainTextureText.text = %terrainMat.getInternalName();
	ProceduralTerrainPainterDescription.text = "Generate "@ %terrainMat.getInternalName() @" layer";
}

//------------------------------------------------------------------------------

//==============================================================================
//==============================================================================
function EPainter_4MatPreview::changeMap( %this,%type ) {			
	if (!isObject(EPainter.activeMat))
		return;
	%ctrl = %this.findObjectByInternalName(%type@"Map",true);
	%curFile = %ctrl.bitmap;
	
	%file = EPainter.getMap(%type,%curFile);
	
		if (!isImageFile(%file)){
		warnLog("The file selected is not a valid image:",%file);
		return;
	}	
	%mat.setFieldValue(%type@"Map",%file);
	TerrainMaterialDlg.setMatDirty( %mat );
	
	%previewMap = EPainter_4MatPreview.findObjectByInternalName(%type@"Map",true);
	%previewMap.setBitmap( %file );	
}
//------------------------------------------------------------------------------
//==============================================================================
function EPainter::changeMapIndex( %this,%type,%index ) {			
	
	%pill = EPainterStack.findObjectByInternalName("Layer_"@%index,true);
	if (!isObject(%pill) || !isObject(%pill.terrainMat))
		return;
		
	
	%previewPill = %pill.findObjectByInternalName(%type@"Map",true);
		%curFile = %previewPill.bitmap;
	%file = EPainter.getMap(%type,%curFile);
	
	if (!isImageFile(%file)){
		warnLog("The file selected is not a valid image:",%file);
		return;
	}	
	%pill.terrainMat.setFieldValue(%type@"Map",%file);
	TerrainMaterialDlg.setMatDirty( %pill.terrainMat );
	

	%previewPill.setBitmap( %file );	
	if (%pill.terrainMat.getId() $= EPainter.activeMat.getId()){
		%previewMap = EPainter_4MatPreview.findObjectByInternalName(%type@"Map",true);
		%previewMap.setBitmap( %file );
	}
	
	
}
//------------------------------------------------------------------------------

//==============================================================================
function EPainter::getMap( %this,%type,%curFile ) {	
	if (%mat $= "")
		%mat = EPainter.activeMat;		
	if (!isObject(%mat))
		return;
	
	if( getSubStr( %curFile, 0 , 6 ) $= "tlab/" )
		%curFile = "";

	%file = getFile( $TerrainMatDlg_MapFilter,%curFile,"art/terrain/",true,true );
	
	if (!isImageFile(%file)){
		warnLog("The file selected is not a valid image:",%file);
		return;
	}	
	%file = makeRelativePath( %file, getMainDotCsDir() );		
	
	return %file;
}
//------------------------------------------------------------------------------
//==============================================================================
// Update the active material layers list
function PainterLayerEdit::onValidate( %this) {
	devLog("PainterLayerEdit onValidate:: FIELD:",%this.internalName,"Mat:",%this.mat);
	%this.mat.setFieldValue(%this.internalName,%this.getText());
	TerrainMaterialDlg.setMatDirty( %this.mat );
	%this.updateFriends();
}
//------------------------------------------------------------------------------
//==============================================================================
// Update the active material layers list
function PainterLayerSlider::onMouseDragged( %this) {
	%field = strreplace(%this.internalName,"Slider","");
	%this.mat.setFieldValue(%field,%this.getValue());
	TerrainMaterialDlg.setMatDirty( %this.mat );
	%this.updateFriends();
}
//------------------------------------------------------------------------------
//==============================================================================
// Update the active material layers list
function PainterLayerMouse::onMouseDown( %this,%modifier,%pos,%clicks) {
	logd("PainterLayerMouse onMouseDown::",%modifier,%pos,%clicks);

	if (%clicks > 1) {
		eval(%this.altCommand);
	} else {
		eval(%this.command);
	}
}
//------------------------------------------------------------------------------
//==============================================================================
// Update the active material layers list
function PainterLayerMouse::onRightMouseDown( %this,%modifier,%pos,%clicks) {
	devLog("PainterLayerMouse onRightMouseDown::",%this.baseCtrl,%pos,%clicks);
	%ctrl = %this.baseCtrl;
	//%ctrl.isExtendedMode = !%ctrl.isExtendedMode;
	//%this.setStateOn(%ctrl.isExtendedMode);
	EPainter.setMixedView(%ctrl,!%ctrl-->compactCtrl.isVisible());
}
//------------------------------------------------------------------------------

//==============================================================================
function EPainter::showMaterialDeleteDlg( %this, %matInternalName ) {
	LabMsgYesNo( "Confirmation",
					 "Really remove material '" @ %matInternalName @ "' from the terrain?",
					 %this @ ".removeMaterial( " @ %matInternalName @ " );", "" );
}
//------------------------------------------------------------------------------
//==============================================================================
function EPainter::removeMaterial( %this, %matInternalName ) {
	%selIndex = ETerrainEditor.paintIndex - 1;
	// Remove the material from the terrain.
	%index = ETerrainEditor.getMaterialIndex( %matInternalName );

	if( %index != -1 )
		ETerrainEditor.removeMaterial( %index );

	// Update the material list.
	%this.updateLayers( %selIndex );
}
//------------------------------------------------------------------------------
//==============================================================================
// EPainter Set Dirty / Save materials
//==============================================================================
//==============================================================================
/*function EPainter::setMaterialDirty( %this,%mat,%nameCtrl ) {
	if (isObject(%nameCtrl)) {
		%nameCtrl.setText("*" SPC %mat.internalName);
		$PainterDirtyNames = strAddWord($PainterDirtyNames,%nameCtrl,true);
	}

	// Mark the material as dirty and needing saving.
	%fileName = %mat.getFileName();

	if( %fileName $= "" )
		%fileName = "art/terrains/materials.cs";

	ETerrainMaterialPersistMan.setDirty( %mat, %fileName );
	EPainter-->saveDirtyMaterials.active = 1;
}*/
//------------------------------------------------------------------------------
//==============================================================================
/*function EPainter::saveDirtyMaterials( %this ) {
	ETerrainMaterialPersistMan.saveDirty();

	foreach$(%nameCtrl in $PainterDirtyNames) {
		%text= strreplace(%nameCtrl.text,"* ","");
		%nameCtrl.text = %text;
	}

	EPainter-->saveDirtyMaterials.active = 0;
}*/
//------------------------------------------------------------------------------
//==============================================================================
// LAYER DRAG AND DROP SYSTEM
//==============================================================================
//==============================================================================
// Update the active material layers list
function PainterLayerMouse::onMouseDragged( %this,%modifier,%pos,%clicks) {
	%dragClone = %this.baseCtrl;

	if (isObject(%this.dragClone))
		%dragClone = %this.dragClone;

	%dragClone.layerId = %this.baseCtrl.layerId;

	foreach(%ctrl in EPainterStack)
		%ctrl.dropLayer.visible = 1;

	startDragAndDropCtrl(%dragClone,"TerrainLayer","","EPainter.layerDragFailed();");
}
//------------------------------------------------------------------------------

//==============================================================================
// Update the active material layers list
function LayerDropClass::onControlDropped(%this, %control, %dropPoint) {
	%droppedOnLayer = %this.layerID;
	%droppedLayer = %control.dragSourceControl.layerID;
	ETerrainEditor.reorderMaterial(%droppedLayer,%droppedOnLayer);
	EPainter.setPaintLayer(%droppedOnLayer);
	EPainter.updateLayers();

	foreach(%ctrl in EPainterStack)
		%ctrl.dropLayer.visible = 0;
}
//==============================================================================
// Update the active material layers list
function EPainter::layerDragFailed( %this,%modifier,%pos,%clicks) {
	foreach(%ctrl in EPainterStack)
		%ctrl.dropLayer.visible = 0;
}
//------------------------------------------------------------------------------

function EPainterIconBtn::onMouseDragged( %this ) {
	%payload = new GuiControl() {
		profile = ToolsButtonArray;
		position = "0 0";
		extent = %this.extent.x SPC "5";
		dragSourceControl = %this;
	};
	%xOffset = getWord( %payload.extent, 0 ) / 2;
	%yOffset = getWord( %payload.extent, 1 ) / 2;
	%cursorpos = Canvas.getCursorPos();
	%xPos = getWord( %cursorpos, 0 ) - %xOffset;
	%yPos = getWord( %cursorpos, 1 ) - %yOffset;
	// Create the drag control.
	%ctrl = new GuiDragAndDropControl() {
		canSaveDynamicFields    = "0";
		Profile                 = ToolsDragDropProfile;
		HorizSizing             = "right";
		VertSizing              = "bottom";
		Position                = %xPos SPC %yPos;
		extent                  = %payload.extent;
		MinExtent               = "4 4";
		canSave                 = "1";
		Visible                 = "1";
		hovertime               = "1000";
		deleteOnMouseUp         = true;
	};
	%ctrl.add( %payload );
	Canvas.getContent().add( %ctrl );
	%ctrl.startDragging( %xOffset, %yOffset );
}

function EPainterIconBtn::onControlDragged( %this, %payload ) {
	%payload.getParent().position = %this.getGlobalPosition();
}

function EPainterIconBtn::onControlDropped( %this, %payload ) {
	%srcBtn = %payload.dragSourceControl;
	%dstBtn = %this;
	%stack = %this.getParent();

	// Not dropped on a valid Button.
	// Really this shouldnt happen since we are in a callback on our specialized
	// EPainterIconBtn namespace.
	if ( %stack != %dstBtn.getParent() || %stack != EPainterStack.getId() ) {
		echo( "Not dropped on valid control" );
		return;
	}

	// Dropped on the original control, no order change.
	// Simulate a click on the control, instead of a drag/drop.
	if ( %srcBtn == %dstBtn ) {
		%dstBtn.performClick();
		return;
	}

	%dstIndex = %stack.getObjectIndex( %dstBtn );
	ETerrainEditor.reorderMaterial( %stack.getObjectIndex( %srcBtn ), %dstIndex );
	// select the button/material we just reordered.
	%stack.getObject( %dstIndex ).performClick();
}