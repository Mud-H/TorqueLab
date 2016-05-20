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
	hide(EPainter_LayerSrc);
	show(EPainterStack);
	hide(EPainter_LayerCompactSrc);
	hide(EPainter_LayerMixedSrc);
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
	// Default to whatever was selected before.
	if ( %matIndex $= "" )
		%matIndex = ETerrainEditor.paintIndex;

	// The material string is a newline seperated string of
	// TerrainMaterial internal names which we can use to find
	// the actual material data in TerrainMaterialSet.
	%mats = ETerrainEditor.getMaterials();
	%ctrlSrc = EPainter_LayerCollapseSrc;
	

	// %listWidth = getWord( EPainterStack.getExtent(), 0 );
	hide(EPainter_LayerCompactSrc);
	hide(EPainter_LayerCollapseSrc);
	//hide(EPainter_LayerExtendedSrc);
	show(EPainterStack);
	EPainterStack.clear();

	if (!$Fixed && $EPainter_DisplayMode > 1/*&& %ctrlSrc.getId() $= EPainter_LayerMixedSrc.getId()*/) {
		devLog("PaintMode = ",$EPainter_DisplayMode,"ExtendedExist",%ctrlSrc-->extendedCtrl);
		$EPainter_DisplayMode = "1";
		%ctrlSrc = EPainter_LayerCollapseSrc;
	}

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
		
		%bitmapButton = %ctrl-->bitmapButton;
		//%bitmapButton.internalName = "EPainterMaterialButton" @ %i;
		%bitmapButton.command = %command;
		%bitmapButton.altCommand = %altCommand;
		%bitmapButton.setBitmap( %mat.diffuseMap );
		//}
		%editButton = %ctrl-->editButton;
		%editButton.command =  "TerrainMaterialDlg.show( " @ %index @ ", " @ %mat @ ", EPainter_TerrainMaterialUpdateCallback );";
		%deleteButton = %ctrl-->deleteButton;
		%deleteButton.command = "EPainter.showMaterialDeleteDlg( " @ %matInternalName @ " );";
	
		%ctrl-->matName.text = %matInternalName;
		%ctrl-->matName.internalName = %matInternalName;
		%ctrl.isActiveCtrl = %ctrl-->ctrlActive;
		%ctrl.isActiveCtrl.visible = 0;
		%mouseEvent = %ctrl-->mouseEvent;
		%mouseEvent.command = %command;
		%mouseEvent.altCommand = %altCommand;
		%mouseEvent.superClass = "PainterLayerMouse";
		%mouseEvent.baseCtrl = %ctrl;
		%ctrl.mouseEvent = %ctrl-->mouseEvent;
		%ctrl-->dropLayer.layerIndex = %index;
		%ctrl-->dropLayer.layerId = %i;
		%ctrl-->dropLayer.visible = 0;
		%ctrl.dropLayer = %ctrl-->dropLayer;
		%ctrl-->detailButton.baseCtrl = %ctrl;
		%compactCtrl = %ctrl-->compactCtrl;
		%compactCtrl-->matName.text = %matInternalName;
		%bitmapButtonCompact = %compactCtrl-->bitmapButton;
		//	%bitmapButtonCompact.internalName = "EPainterMaterialButton" @ %i;
		%bitmapButtonCompact.command = %command;
		%bitmapButtonCompact.altCommand = %altCommand;
		%bitmapButtonCompact.internalName = "diffuseMap";
		//%bitmapButtonCompact.setBitmap( %mat.diffuseMap );
		%mouseEvent = %compactCtrl-->mouseEvent;
		%mouseEvent.command = %command;
		%mouseEvent.altCommand = %altCommand;
		%mouseEvent.superClass = "PainterLayerMouse";
		%mouseEvent.baseCtrl = %ctrl;
		%mouseEvent.dragClone = %compactCtrl;
		%compactCtrl-->ctrlActive.visible = 0;
		%compactCtrl-->dropLayer.visible = 0;
		%fullCtrl = %ctrl-->fullCtrl;
		//%fullCtrl-->matName.text = %matInternalName;
		%fullCtrl-->ctrlActive.visible = 0;
		%fullCtrl-->dropLayer.visible = 0;
		%bitmapButtonFull = %fullCtrl-->bitmapButton;
		%bitmapButtonFull.command = %command;
		%bitmapButtonFull.altCommand = %altCommand;
		
		%bitmapDetailFull = %fullCtrl-->bitmapDetail;		
		%bitmapDetailFull.command = %command;
		%bitmapDetailFull.altCommand = "EPainter.changeMapIndex(\"detail\","@%i@");";
		%bitmapDetailFull.internalName = "detailMap";
		
		%bitmapMacroFull = %fullCtrl-->bitmapMacro;		
		%bitmapMacroFull.command = %command;
		%bitmapMacroFull.altCommand = "EPainter.changeMapIndex(\"macro\","@%i@");";
		%bitmapMacroFull.internalName = "macroMap";
		
		%fullCtrl-->diffuseMapBase.text = fileBase(%mat.diffuseMap);
		%fullCtrl-->detailMapBase.text = fileBase(%mat.detailMap);
		%fullCtrl-->normalMapBase.text = fileBase(%mat.normalMap);
		%fullCtrl-->macroMapBase.text = fileBase(%mat.macroMap);
		%fullCtrl-->parallaxScaleSlider.setValue(%mat.parallaxScale);
		%fullCtrl-->parallaxScaleSlider.mat = %mat;
		%fullCtrl-->parallaxScaleSlider.nameCtrl = %ctrl-->matName;
		%mouseEventExt = %fullCtrl-->mouseEvent;
		%mouseEventExt.command = %command;
		%mouseEventExt.altCommand = %altCommand;
		%mouseEventExt.superClass = "PainterLayerMouse";
		%mouseEventExt.baseCtrl = %ctrl;
		%mouseEventExt.dragClone = %fullCtrl;

		foreach$(%field in $PainterMatFields) {
			%fieldCtrl = %fullCtrl.findObjectByInternalName(%field,true);

			if (!isObject(%fieldCtrl))
				continue;

			%fieldCtrl.setValue(%mat.getFieldValue(%field));
			%fieldCtrl.mat = %mat;
			%fieldCtrl.superClass = "PainterLayerEdit";
			%fieldCtrl.nameCtrl = %ctrl-->matName;
		}

		%this.setMixedView(%ctrl,true,$EPainter_StartAsExtended);

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

	if (isObject(%ctrl))
		%ctrl-->bitmapButton.performClick(); //FIXME something wrong here
}
//------------------------------------------------------------------------------
//==============================================================================
// Update the active material layers list
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
}
//------------------------------------------------------------------------------
//==============================================================================
// MIXED LISTING MODES FUNCTIONS
//==============================================================================
//==============================================================================
// Update the active material layers list
function EPainterToggleMixedButton::onClick( %this) {
	%ctrl = %this.baseCtrl;
	EPainter.setMixedView(%ctrl,%ctrl-->fullCtrl.isVisible());
}
//------------------------------------------------------------------------------

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
