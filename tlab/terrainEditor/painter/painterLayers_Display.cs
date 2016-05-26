//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$EPainter_DisplayMode = 0;
$EPainter_AutoCollapse = false;
$EPainter_StartAsExtended = false;
$EPainter_CollapseSiblings = true;
//==============================================================================
// Painter Layers Display Modes
//==============================================================================

//==============================================================================
function EPainter::setDisplayModes( %this ) {
	EPainter.updateLayers();
	show(EPainterStack);
	hide(EPainter_LayerCompactSrc);

/*	EPainter_DisplayMode.clear();
	EPainter_DisplayMode.add("Mixed listing",0);
	EPainter_DisplayMode.add("Compact listing",1);
	//EPainter_DisplayMode.add("Detailled only",2);
	//EPainter_DisplayMode.add("Extended only",3);
	EPainter_DisplayMode.setSelected($EPainter_DisplayMode,true);*/
}
//------------------------------------------------------------------------------
/*
//==============================================================================
function EPainter_DisplayMode::onSelect( %this,%id,%text ) {
	//if ($EPainter_DisplayMode $= %id)
	//return;
	$EPainter_DisplayMode = %id;
	EPainter-->optionMode0.visible = 0;
	EPainter.updateLayers();
	eval("EPainter-->optionMode"@$EPainter_DisplayMode@".visible = true;");
}
//------------------------------------------------------------------------------
*/
//==============================================================================
// PAINTER UPDATE LAYERS LISTING
//==============================================================================
$PainterMatFields = "detailMap detailSize detailStrength detailDistance macroMap macroSize macroStrength macroDistance diffuseSize diffuseMap parallaxScale";
$Fixed = false;
function EPainter::toggleSplatMap( %this,%checkBox ) {
	toggleSplatMapMode();
	%checkBox.setStateOn($SplatMapModeActivated);
}

//==============================================================================
// Update the active material layers list
function EPainter::updateLayers( %this, %matIndex ) {
   devLog(" EPainter::updateLayers( %this, %matIndex )",%this,%matIndex);
	// Default to whatever was selected before.
	if ( %matIndex $= "" )
	   %matIndex = $EPainter_ActiveLayerId;
	if ( %matIndex $= "" )   
		%matIndex = ETerrainEditor.paintIndex;

	// The material string is a newline seperated string of
	// TerrainMaterial internal names which we can use to find
	// the actual material data in TerrainMaterialSet.
	%mats = ETerrainEditor.getMaterials();
	
	%ctrlSrc = EPainter_LayerCollapseSrc;
	if (!isObject(%ctrlSrc))
	{
	   devLog("No EPainter_Layer Control Source",%ctrlSrc);
	   return;   
	}	
	if (!isObject(EPainterStack))
      {
         devLog("There's no Epainter");
         return;  
      }
	hide(%ctrlSrc);	
	show(EPainterStack);
	EPainterStack.clear();	

	for( %i = 0; %i < getRecordCount( %mats ); %i++ ) {
		%matInternalName = getRecord( %mats, %i );
		%mat = TerrainMaterialSet.findObjectByInternalName( %matInternalName );

		// Is there no material info for this slot?
		if ( !isObject( %mat ) )
			continue;
      
		%index = EPainterStack.getCount();
		%command = "EPainter.setPaintLayer( " @ %index @ " );";
		%altCommand = "TerrainMaterialDlg.show( " @ %index @ ", " @ %mat @ ", EPainter_TerrainMaterialUpdateCallback );";
		%ctrl = cloneObject(%ctrlSrc,"","Layer_"@%index,EPainterStack);
		%ctrl.terrainMat = %mat;
		%ctrl.layerId = %i;
		%ctrl-->iconStack.AlignCtrlToParent("right");
		%ctrl.text = "";
		//Icons Stack
		%editButton = %ctrl-->editButton;
		%editButton.command =  "TerrainMaterialDlg.show( " @ %index @ ", " @ %mat @ ", EPainter_TerrainMaterialUpdateCallback );";
		%deleteButton = %ctrl-->deleteButton;
		%deleteButton.command = "EPainter.showMaterialDeleteDlg( " @ %matInternalName @ " );";
//-------------------------------------------------------------
// Compact Details Section			
		%bitmapButton = %ctrl-->bitmapButton;
		//%bitmapButton.internalName = "EPainterMaterialButton" @ %i;
		%bitmapButton.command = %command;
		%bitmapButton.altCommand = %altCommand;
		if (isFile(%mat.diffuseMap))
		   %bitmapButton.setBitmap( %mat.diffuseMap );
		//}
		
	
		%ctrl-->matName.text = %matInternalName;
		%ctrl-->matName.internalName = %matInternalName;
		%ctrl.isActiveCtrl = %ctrl-->ctrlActive;
		%ctrl.isActiveCtrl.visible = 0;		
		
		
		%ctrl-->dropLayerCompact.layerIndex = %index;
		%ctrl-->dropLayerCompact.layerId = %i;
		%ctrl-->dropLayerCompact.visible = 0;
		%ctrl-->dropLayerCompact.superClass = "EPainterDropLayer";
		%ctrl-->dropLayerCompact.baseCtrl = %ctrl;
		%ctrl.dropLayerCompact = %ctrl-->dropLayerCompact;
		%ctrl-->dropLayerFull.layerIndex = %index;
		%ctrl-->dropLayerFull.layerId = %i;
		%ctrl-->dropLayerFull.visible = 0;
		%ctrl-->dropLayerFull.superClass = "EPainterDropLayer";
		%ctrl-->dropLayerFull.baseCtrl = %ctrl;
		
		%ctrl.dropLayerFull = %ctrl-->dropLayerFull;
		
		
		%ctrl-->detailButton.baseCtrl = %ctrl;
		%compactCtrl = %ctrl-->compactCtrl;
		%compactCtrl-->matName.text = %matInternalName;
		
		%bitmapButtonCompact = %compactCtrl-->bitmapButton;
		//	%bitmapButtonCompact.internalName = "EPainterMaterialButton" @ %i;
		%bitmapButtonCompact.command = %command;
		%bitmapButtonCompact.altCommand = %altCommand;
		%bitmapButtonCompact.internalName = "diffuseMap";
		if (isImageFile(%mat.diffuseMap))
		%bitmapButtonCompact.setBitmap( %mat.diffuseMap );
		
		%mouseEvent = %compactCtrl-->mouseEvent;
		%mouseEvent.command = %command;
		%mouseEvent.altCommand = %altCommand;
		%mouseEvent.superClass = "PainterLayerMouse";
		%mouseEvent.baseCtrl = %ctrl;
		%mouseEvent.dragClone = %compactCtrl;
		%compactCtrl-->ctrlActive.visible = 0;
		%compactCtrl-->dropLayer.visible = 0;
		%ctrl.mouseEvent = %mouseEvent;
//-------------------------------------------------------------
// Full Details Section		
		%fullCtrl = %ctrl-->fullCtrl;
		%fullCtrl.visible = 0;
		//%fullCtrl-->matName.text = %matInternalName;
		%fullCtrl-->ctrlActive.visible = 0;
		%fullCtrl-->dropLayer.visible = 0;
		
		%bitmapDiffuseFull = %fullCtrl-->bitmapDiffuse;
		%bitmapDiffuseFull.command = %command;
		%bitmapDiffuseFull.altCommand = %altCommand;
		%bitmapDiffuseFull.internalName = "diffuseMap";
		if (isImageFile(%mat.diffuseMap))
		%bitmapButtonCompact.setBitmap( %mat.diffuseMap );
		
		%bitmapDetailFull = %fullCtrl-->bitmapDetail;		
		%bitmapDetailFull.command = %command;
		%bitmapDetailFull.altCommand = "EPainter.changeMapIndex(\"detail\","@%i@");";
		%bitmapDetailFull.internalName = "detailMap";
      if (isImageFile(%mat.detailMap))
		%bitmapDetailFull.setBitmap( %mat.detailMap );		
		
		%bitmapMacroFull = %fullCtrl-->bitmapMacro;		
		%bitmapMacroFull.command = %command;
		%bitmapMacroFull.altCommand = "EPainter.changeMapIndex(\"macro\","@%i@");";
		%bitmapMacroFull.internalName = "macroMap";
		if (isImageFile(%mat.macroMap))
		%bitmapMacroFull.setBitmap( %mat.macroMap );
		
		%fullCtrl-->diffuseMapBase.text = fileBase(%mat.diffuseMap);
		%fullCtrl-->detailMapBase.text = fileBase(%mat.detailMap);
		%fullCtrl-->normalMapBase.text = fileBase(%mat.normalMap);
		%fullCtrl-->macroMapBase.text = fileBase(%mat.macroMap);
		%fullCtrl-->parallaxScaleSlider.setValue(%mat.parallaxScale);
		%fullCtrl-->parallaxScaleSlider.mat = %mat;
		%fullCtrl-->parallaxScaleSlider.nameCtrl = %ctrl-->matName;
		
		%mouseEventFull = %fullCtrl-->mouseEvent;
		%mouseEventFull.command = %command;
		%mouseEventFull.altCommand = %altCommand;
		%mouseEventFull.superClass = "PainterLayerMouse";
		%mouseEventFull.baseCtrl = %ctrl;
		%mouseEventFull.dragClone = %fullCtrl;

		foreach$(%field in $PainterMatFields) {
			%fieldCtrl = %fullCtrl.findObjectByInternalName(%field,true);

			if (!isObject(%fieldCtrl))
				continue;

			%fieldCtrl.setValue(%mat.getFieldValue(%field));
			%fieldCtrl.mat = %mat;		
			if (%fieldCtrl.getClassName() $= "GuiTextEditCtrl")
			   %fieldCtrl.superClass = "PainterLayerEdit";
			%fieldCtrl.nameCtrl = %ctrl-->matName;
		}

		

		if(%i < 9)
			%tooltip = %tooltip @ " (" @ (%i+1) @ ")";
		else if(%i == 9)
			%tooltip = %tooltip @ " (0)";

		%bitmapButton.tooltip = %tooltip;
	}

	%matCount = EPainterStack.getCount();
	// Add one more layer as the 'add new' layer.
	%ctrl = new GuiIconButtonCtrl() {
		profile = "ToolsButtonProfile";
		iconBitmap = "tlab/art/icons/default/new_layer_icon";
		iconLocation = "Left";
		textLocation = "Right";
		//extent = %listWidth SPC "46";
		textMargin = 5;
		buttonMargin = "4 4";
		buttonType = "PushButton";
		sizeIconToButton = true;
		makeIconSquare = true;
		tooltipprofile = "ToolsToolTipProfile";
		text = "New Layer";
		tooltip = "New Layer";
		command = "TerrainMaterialDlg.show( " @ %matCount @ ", 0, EPainter_TerrainMaterialAddCallback );";
	};
	EPainterStack.add( %ctrl );

	// Make sure our selection is valid and that we're
	// not selecting the 'New Layer' button.

	if( %matIndex < 0 )
		return;

	if( %matIndex >= %matCount )
		%matIndex = 0;

	%activeIndex = ETerrainMaterialSelected.selectedMatIndex;

	if (%activeIndex $= "")
		%activeIndex = 0;

	%this.setPaintLayer(%activeIndex);
	// To make things simple... click the paint material button to
	// active it and initialize other state.
	%ctrl = EPainterStack.getObject( %matIndex );

	if (isObject(%ctrl-->diffuseMap))
		%ctrl-->diffuseMap.performClick(); //FIXME something wrong here
}
//------------------------------------------------------------------------------
//==============================================================================
// Update the active material layers list
/*
function EPainter::updateSelectedLayerList( %this,%ctrl) {
	//Hide all selected colored containers
	foreach(%layerCtrl in EPainterStack)
		%layerCtrl.isActiveCtrl.visible = 0;

	%ctrl.isActiveCtrl.visible = 1;

	//If AutoCollapse true and display mode = mixes, collapse all layers
	if ($EPainter_DisplayMode $= "0" && $EPainter_AutoCollapse) {
		foreach(%layerCtrl in EPainterStack) {
			if (%layerCtrl.text $= "New layer")
				continue;

			%isCompact = true;

			if(%layerCtrl $= %ctrl) {
				%isCompact = false;
			}

			%isExtended = %layerCtrl.isExtendedMode;
			%this.setMixedView(%layerCtrl,%isCompact,%isExtended);
		}
	}
}*/
//------------------------------------------------------------------------------
//==============================================================================
// MIXED LISTING MODES FUNCTIONS
//==============================================================================
//==============================================================================
// Update the active material layers list
function EPainterToggleMixedButton::onClick( %this) {
	%ctrl = %this.baseCtrl;
	EPainter.toggleLayerFull(%ctrl);
	//EPainter.setMixedView(%ctrl,%ctrl-->fullCtrl.isVisible());
}
//------------------------------------------------------------------------------
//==============================================================================
// Update the active material layers list
function EPainter::toggleLayerFull( %this,%ctrl,%show,%indirect) {
   //Check if the fullCtrl is visible, if not show it.
  
   if (!isObject(%ctrl-->fullCtrl))
      return;
      
    if (%show $= "")
      %show =  !%ctrl-->fullCtrl.visible;
      
   %ctrl-->fullCtrl.visible = %show;
   %ctrl-->iconStack.AlignCtrlToParent("right");
   devLog("FullCtrl Shoen:",%show);
  
   if (%indirect)
      return;
   if ($EPainter_CollapseSiblings && %show){
			foreach(%colCtrl in EPainterStack){
				
				if (%colCtrl != %ctrl )
					%colCtrl-->fullCtrl.visible = 0;
			}
		}
}
//------------------------------------------------------------------------------

/*
//==============================================================================
// Update the active material layers list

function EPainter::setMixedView( %this,%ctrl,%isCompact,%noCheck) {
	%compactCtrl = %ctrl-->compactCtrl;
	%compactCtrl.visible = %isCompact;
	%compactCtrl.visible = true; //Stay as full title
	
	%fullCtrl = %ctrl-->fullCtrl;
	
	%fullCtrl.visible = !%isCompact;
	%compactCtrl-->dropLayer.visible = 0;
	%fullCtrl-->dropLayer.visible = 0;

	if (%isCompact) {
		%ctrl.extent = %ctrl-->compactCtrl.extent;
		%ctrl.isActiveCtrl = %compactCtrl-->ctrlActive;
		%ctrl.dropLayer = %compactCtrl-->dropLayer;
	} else {
		if ($EPainter_CollapseSiblings && !%noCheck){
			foreach(%colCtrl in EPainterStack){
				
				if (%colCtrl != %ctrl )
					%this.setMixedView(%colCtrl,true,true);
			}
		}
			
		%ctrl.isActiveCtrl = %fullCtrl-->ctrlActive;
		%ctrl.extent = %fullCtrl.extent;
		%ctrl.dropLayer = %fullCtrl-->dropLayer;
	}
  
	%ctrl-->iconStack.AlignCtrlToParent("right");
	%fullCtrl.setExtent("313 191");
	EPainterStack.updateStack();
	
}
//------------------------------------------------------------------------------
*/