//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
function EPainterSplatMapWin::onCollapse( %this )
{
    devLog("Index changed from:",%this.lockIndex,"To",%index);
    //EPainterPreview.reorderChild(EPainterSplatMapWin,EPainter);
}
function EPainterSplatMapWin::onRestore( %this )
{
    devLog("Index changed from:",%this.lockIndex,"To",%index);
    //EPainterPreview.reorderChild(EPainterSplatMapWin,EPainter);
}


//==============================================================================
// TerrainMaterial Preview
//==============================================================================
function EPainter::toggle4MatPreview( %this )
{
    %frame = EPainter_4MatPreview;
}
//==============================================================================
// TerrainPainterPlugin
//==============================================================================

function EPainter::onActivated( %this )
{
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
function EPainter::setPaintLayer( %this, %matIndex)
{
    %ctrl = EPainterStack.findObjectByInternalName("Layer_"@%matIndex,true);
    %terrainMat = %ctrl.terrainMat;
    %pill = EPainterStack.getObject(%matIndex);
    //%this.updateSelectedLayerList(%ctrl);
    foreach(%ctrl in EPainterStack)
        %ctrl.isActiveCtrl.visible = 0;
    %pill.isActiveCtrl.visible = 1;
    if (!isObject( %terrainMat ))
    {
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
    foreach$(%map in $TerMat_BitmapFields)
    {
        if (isImageFile(%terrainMat.getFieldValue(%map)))
        {
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
function EPainter_4MatPreview::changeMap( %this,%type )
{
    if (!isObject(EPainter.activeMat))
        return;
    %ctrl = %this.findObjectByInternalName(%type@"Map",true);
    %curFile = %ctrl.bitmap;
    %file = EPainter.getMap(%type,%curFile);
    if (!isImageFile(%file))
    {
        warnLog(%type,"The file selected is not a valid image:",%file);
        return;
    }
    %mat.setFieldValue(%type@"Map",%file);
    TerrainMaterialDlg.setMatDirty( %mat );
    %previewMap = EPainter_4MatPreview.findObjectByInternalName(%type@"Map",true);
    %previewMap.setBitmap( %file );
}
//------------------------------------------------------------------------------
//==============================================================================
function EPainter::changeMapIndex( %this,%type,%index )
{
    %pill = EPainterStack.findObjectByInternalName("Layer_"@%index,true);
    if (!isObject(%pill) || !isObject(%pill.terrainMat))
        return;
    %previewPill = %pill.findObjectByInternalName(%type@"Map",true);
    %curFile = %previewPill.bitmap;
    %file = EPainter.getMap(%type,%curFile);
    if (!isImageFile(%file))
    {
        warnLog(%type,"The file selected is not a valid image:",%file);
        return;
    }
    %pill.terrainMat.setFieldValue(%type@"Map",%file);
    TerrainMaterialDlg.setMatDirty( %pill.terrainMat );
    %previewPill.setBitmap( %file );
    if (%pill.terrainMat.getId() $= EPainter.activeMat.getId())
    {
        %previewMap = EPainter_4MatPreview.findObjectByInternalName(%type@"Map",true);
        %previewMap.setBitmap( %file );
    }
}
//------------------------------------------------------------------------------

//==============================================================================
function EPainter::getMap( %this,%type,%curFile )
{
    if (%mat $= "")
        %mat = EPainter.activeMat;
    if (!isObject(%mat))
        return;
    if( getSubStr( %curFile, 0 , 6 ) $= "tlab/" )
        %curFile = "";
    %file = getFile( $TerrainMatDlg_MapFilter,%curFile,"art/terrain/",true,true );
    if (!isImageFile(%file))
    {
        warnLog(%type,"The file selected is not a valid image:",%file);
        return;
    }
    %file = makeRelativePath( %file, getMainDotCsDir() );
    return %file;
}
//------------------------------------------------------------------------------
//==============================================================================
// Update the active material layers list
function PainterLayerEdit::onValidate( %this)
{
    devLog("PainterLayerEdit onValidate:: FIELD:",%this.internalName,"Mat:",%this.mat);
    %this.mat.setFieldValue(%this.internalName,%this.getText());
    TerrainMaterialDlg.setMatDirty( %this.mat );
    %this.updateFriends();
}
//------------------------------------------------------------------------------
//==============================================================================
// Update the active material layers list
function PainterLayerSlider::onMouseDragged( %this)
{
    %field = strreplace(%this.internalName,"Slider","");
    %this.mat.setFieldValue(%field,%this.getValue());
    TerrainMaterialDlg.setMatDirty( %this.mat );
    %this.updateFriends();
}
//------------------------------------------------------------------------------
//==============================================================================
// Update the active material layers list
function PainterLayerMouse::onMouseDown( %this,%modifier,%pos,%clicks)
{
    logd("PainterLayerMouse onMouseDown::",%modifier,%pos,%clicks);
    if (%clicks > 1)
    {
        eval(%this.altCommand);
    }
    else
    {
        eval(%this.command);
    }
}
//------------------------------------------------------------------------------
//==============================================================================
// Update the active material layers list
function PainterLayerMouse::onRightMouseDown( %this,%modifier,%pos,%clicks)
{
    devLog("PainterLayerMouse onRightMouseDown::",%this.baseCtrl,%pos,%clicks);
    %ctrl = %this.baseCtrl;
    //%ctrl.isExtendedMode = !%ctrl.isExtendedMode;
    //%this.setStateOn(%ctrl.isExtendedMode);
    EPainter.setMixedView(%ctrl,!%ctrl-->compactCtrl.isVisible());
}
//------------------------------------------------------------------------------

//==============================================================================
function EPainter::showMaterialDeleteDlg( %this, %matInternalName )
{
    LabMsgYesNo( "Confirmation",
                 "Really remove material '" @ %matInternalName @ "' from the terrain?",
                 %this @ ".removeMaterial( " @ %matInternalName @ " );", "" );
}
//------------------------------------------------------------------------------
//==============================================================================
function EPainter::removeMaterial( %this, %matInternalName )
{
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
// LAYER DRAG AND DROP SYSTEM
//==============================================================================
//==============================================================================
// Update the active material layers list
function EPainter::setLayerDropZones( %this,%visible)
{
    foreach(%ctrl in EPainterStack)
    {
        %ctrl-->dropLayerCompact.visible = %visible;
        %ctrl-->dropLayerFull.visible = %visible;
    }
}
//------------------------------------------------------------------------------
//==============================================================================
// Update the active material layers list
function PainterLayerMouse::onMouseDragged( %this,%modifier,%pos,%clicks)
{
    %dragClone = %this.baseCtrl;
    if (isObject(%this.dragClone))
        %dragClone = %this.dragClone;
    %dragClone.layerId = %this.baseCtrl.layerId;
    EPainter.setLayerDropZones(1);
//startDragAndDropCtrl(%this,"FileBrowserFav","FileBrowser.onIconDropped");
    startDragAndDropCtrl(%dragClone,"TerrainLayer","EPainter.layerDropped","EPainter.layerDragFailed");
}
//------------------------------------------------------------------------------
function EPainter::layerDropped(%this, %droppedOn, %droppedCtrl,%pos)
{
    devLog("DragSuccess!",%droppedOn, %droppedCtrl,%pos);
    if (isObject(%droppedOn.baseCtrl))
        %layerCtrl = %droppedOn.baseCtrl;
    else if (isObject(%droppedOn.getParent().baseCtrl))
        %layerCtrl = %droppedOn.baseCtrl;
     
     devLog("DragSuccess! BaseLayer",%layerCtrl,%droppedCtrl.layerId, %droppedOn.layerId );   
    EPainter.setLayerDropZones(0);
    %draggedLayer = %droppedCtrl.dragSourceControl;
    delObj(%droppedCtrl);
    if (isObject(%layerCtrl) && (%draggedLayer.layerId >=0 && %droppedOn.layerId >=0))
    {    
      $EPainter_ActiveLayerId = %droppedOn.layerId;
      ETerrainEditor.reorderMaterial( %draggedLayer.layerId, %droppedOn.layerId );   
      
    }
   
}
function EPainter::clickLayerId(%this, %id)
{
   %clickOn = EPainterStack.findObjectByInternalName("layer_"@%id);
   if (isObject(%clickOn-->diffuseMap))
      %clickOn-->diffuseMap.performClick();
}


//==============================================================================
// Update the active material layers list
function EPainter::layerDragFailed( %this,%modifier,%pos,%clicks)
{
    EPainter.setLayerDropZones(0);
}
//------------------------------------------------------------------------------
//==============================================================================
