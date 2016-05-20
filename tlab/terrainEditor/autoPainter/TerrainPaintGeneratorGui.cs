//==============================================================================
// TorqueLab -> Procedural Terrain Painter GUI script
// Copyright NordikLab Studio, 2014
//==============================================================================
$TPG_DefaultHeightMin = 0;
$TPG_DefaultHeightMax = 1000;
$TPG_DefaultSlopeMin = 0;
$TPG_DefaultSlopeMax = 90;
$TPG_DefaultCoverage = 100;
$Lab::TerrainPainter::ValidateHeight = "0";
$TPG_Coverage = 100;

$TPG_CreateMissingMaterials = false;

$TPG_AppendLoadedLayers = false;
$TerrainPaintGeneratorGui_Initialized = false;
//==============================================================================
// Multi material automatic terrain painter
//==============================================================================

//==============================================================================
function TerrainPaintGeneratorGui::onWake(%this) {
	if (!$TerrainPaintGeneratorGui_Initialized)
		TPG.init();

	TPG_Window-->saveGroupButton.visible = 0;
	TPG_GenerateProgressWindow.setVisible(false);
	TPG_PillSample.setVisible(false);
	TPG.checkLayersStackGroup();

	foreach(%layer in TPG_LayerGroup) {
		%this.updateLayerPill(%layer);
		%this.updateLayerMaterialMenu(%layer);
	}

	TPG.map.push();
	Lab.hidePluginTools();
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainPaintGeneratorGui::onSleep(%this) {
	TPG.map.pop();
	Lab.showPluginTools();
}
//------------------------------------------------------------------------------
//==============================================================================
function TPG::toggleOptions(%this) {
	TPG_Options.visible = !TPG_Options.visible;
	TerrainPaintGeneratorGui-->optionsIcon.setStateOn(TPG_Options.visible);
}
//------------------------------------------------------------------------------
//==============================================================================
function TPG::addTerrainMaterialLayer(%this,%matInternalName) {
	%mat = TerrainMaterialSet.findObjectByInternalName( %matInternalName );

	if (!isObject(%mat))
		return false;

	ETerrainEditor.addMaterial(%matInternalName);
	EPainter.updateLayers();
	ETerrainEditor.updateMaterial(  EPainterStack.getCount(), %mat.getInternalName() );
	return true;
}
//------------------------------------------------------------------------------


//==============================================================================
function TerrainPaintGeneratorGui::editLayer(%this, %buttonCtrl) {
	%this.openLayerSettings(%buttonCtrl.layerObj);
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainPaintGeneratorGui::deleteLayer(%this, %buttonCtrl) {
	%layer = %buttonCtrl.layerObj;
	TPG_LayerGroup.remove(%layer);
	%buttonCtrl.layerObj.pill.delete();
	%layer.delete();
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainPaintGeneratorGui::deleteLayerGroup(%this) {
	TPG_LayerGroup.clear();
	TPG_StackLayers.clear();
	TPG_Window-->saveGroupButton.active = false;
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainPaintGeneratorGui::deleteInvalidLayers(%this) {
	foreach(%pill in TPG_StackLayers) {
		%layer = %pill.layerObj;

		if (strFind(%layer.matInternalName,"*"))
			%removeList = strAddWord(%removeList,%layer.getId());
	}

	foreach$(%layer in %removeList) {
		delObj(%layer.pill);
		delObj(%layer);
	}
}
//------------------------------------------------------------------------------
//==============================================================================
// Copy/Paste layer parameters system
//==============================================================================
//==============================================================================
function TPG_FieldCopyButton::onClick(%this) {
	%layer = %this.layerObj;
	%pill = %layer.pill;
	%wordsData = strreplace(%this.internalName,"_"," ");
	%field = getWord(%wordsData,0);
	%action = getWord(%wordsData,1);

	if (%action $= "Copy") {
		TPG.clipBoard = %layer.getFieldValue(%field);
	} else if (%action $= "Paste") {
		if (TPG.clipBoard $= "")
			return;

		%layer.setFieldValue(%field,TPG.clipBoard);
		%ctrl = %pill.findObjectByInternalName(%field,true);
		%ctrl.setText(TPG.clipBoard);
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function TPG::getCurrentHeight(%this,%id) {
	%avgHeight = ETerrainEditor.lastAverageHeight;
	%avgHeight = mFloatLength(%avgHeight,2);
	TPG.clipBoard = %avgHeight;

	if (%id $= "")
		return;

	TPG.height[%id] = %avgHeight;
	$TPG_ValueStore[%id] = %avgHeight;
	eval("TPG_StoredValues-->v"@%id@".setText(\""@%avgHeight@"\");");
}
//------------------------------------------------------------------------------
//==============================================================================
function TPG::setCurrentHeight(%this,%id) {
	if (TPG.height[%id] $= "")
		return;

	TPG.clipBoard = TPG.height[%id];
}
//------------------------------------------------------------------------------
//==============================================================================
function TPG::pasteCurrentHeight(%this,%id) {
	if (TPG.clipBoard $= "")
		return;

	TPG.height[%id] = TPG.clipBoard;
	$TPG_ValueStore[%id] = TPG.clipBoard;
	eval("TPG_StoredValues-->v"@%id@".setText(\""@TPG.clipBoard@"\");");
}
//------------------------------------------------------------------------------
