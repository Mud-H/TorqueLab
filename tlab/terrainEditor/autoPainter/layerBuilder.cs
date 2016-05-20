//==============================================================================
// LabEditor ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$TPG_AutoStepGeneration = true;


//==============================================================================
function TerrainPaintGeneratorGui::addNewLayer(%this) {
	%layer = new ScriptObject();
	%layer.internalName = "Layer_"@%this.lastLayerId++;
	%layer.heightMin = $TPG_DefaultHeightMin;
	%layer.heightMax = $TPG_DefaultHeightMax;
	%layer.slopeMin = "0";
	%layer.slopeMax = "90";
	%layer.coverage = $TPG_DefaultCoverage;
	%matId = 0;
	%this.addLayerPill(%layer,true);
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainPaintGeneratorGui::layerSelected(%this,%menu) {
	%matInternalName = %menu.getText();
	%matIndex = %menu.getSelected();
	%mat = TerrainMaterialSet.findObjectByInternalName( %matInternalName );
	%layer = %menu.layerObj;
	%layer.matObject = %mat;
	%layer.matInternalName = %matInternalName;
	%layer.matIndex = %matIndex;
}
//------------------------------------------------------------------------------
//==============================================================================
// Terrain Paint Generator - Terrain Paint Generation Functions
//==============================================================================


//==============================================================================
function TerrainPaintGeneratorGui::addLayerPill(%this,%layer,%update) {
	if (!isObject(%layer)) return;

	%srcPill = TPG_PillSample_v2;
	hide(%srcPill);
	%layer.pill = %srcPill.deepClone();
	%layer.pill.setName("");
	%layer.pill.setVisible(true);
	%layer.pill-->menuLayers.layerObj = %layer;
	%layer.pill-->edit.layerObj = %layer;
	%layer.pill-->delete.layerObj = %layer;
	%layer.pill-->up.pill = %layer.pill;
	%layer.pill-->down.pill = %layer.pill;
	%layer.pill.layerObj = %layer;
	%layer.pill-->heightMin.layerObj = %layer;
	%layer.pill-->heightMax.layerObj = %layer;
	%layer.pill-->slopeMin.layerObj = %layer;
	%layer.pill-->slopeMax.layerObj = %layer;
	%layer.pill-->coverage.layerObj = %layer;
	%layer.pill-->heightMin_Copy.layerObj = %layer;
	%layer.pill-->heightMax_Copy.layerObj = %layer;
	%layer.pill-->slopeMin_Copy.layerObj = %layer;
	%layer.pill-->slopeMax_Copy.layerObj = %layer;
	%layer.pill-->heightMin_Paste.layerObj = %layer;
	%layer.pill-->heightMax_Paste.layerObj = %layer;
	%layer.pill-->slopeMin_Paste.layerObj = %layer;
	%layer.pill-->slopeMax_Paste.layerObj = %layer;
	%layer.pill-->coverageCopy.layerObj = %layer;
	//%layer.pill-->overlayCheck.layerObj = %layer;
	//%layer.pill-->overlayCheck.setStateOn(false);
	//%layer.pill-->overlayCheck.text = "";
	%layer.activeCtrl = %layer.pill-->checkbox;
	%layer.activeCtrl.text = "";
	%layer.activeCtrl.setStateOn(true);
	TPG_StackLayers.add(%layer.pill);

	//Set terrain layer corresponding to stack order if exist
	if (%layer.matInternalName $= "") {
		%id = TPG_StackLayers.getCount()-1;

		if (%id >= getRecordCount(ETerrainEditor.getMaterials()))
			%id = 0;

		%layer.matInternalName = getRecord(ETerrainEditor.getMaterials(),%id);
	}

	%this.updateLayerMaterialMenu(%layer);

	if (%update)
		%this.updateLayerPill(%layer);
}
//------------------------------------------------------------------------------
$CreateMissing = true;
//==============================================================================
function TerrainPaintGeneratorGui::updateLayerPill(%this,%layer) {
	if (!isObject(%layer) || !isObject(%layer.pill)) return;

	%pill = %layer.pill;
	%menu = %pill-->menuLayers;
	%matName = %layer.matInternalName;

	if (%layer.matInternalName $= "") {
		warnLog("Invalid layer found:",%layer,"Name set to Unnamed");
		%layer.matInternalName = "Unnamed";
	}

	%id = %menu.findText(%layer.matInternalName);

	if (%id >= 0) {
		%menu.setSelected(%id);
	} else {
		if (!strFind(%layer.matInternalName,"*")) {
			%layer.matInternalName = "**"@%layer.matInternalName@"**";
		}

		//The assigned material is not found anymore, put the name between stars to warn
		%menu.setText(%layer.matInternalName);
	}

	%layer.pill.layerObj = %layer;
	%layer.pill-->heightMin.setText(%layer.heightMin);
	%layer.pill-->heightMax.setText( %layer.heightMax);
	%layer.pill-->slopeMin.setText(%layer.slopeMin);
	%layer.pill-->slopeMax.setText(%layer.slopeMax);
	%layer.pill-->coverage.setText(%layer.coverage);
	TPG_LayerGroup.add(%layer);
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainPaintGeneratorGui::updateLayerMaterialMenu(%this,%layer) {
	TPG_PillSample.setVisible(false);
	%pill = %layer.pill;
	%menu = %pill-->menuLayers;

	if (!isObject(%menu)) {
		delObj(%pill);
		delObj(%layer);
		return;
	}

	%mats = ETerrainEditor.getMaterials();
	%menu.clear();

	//Add all current terrain materials to menu
	for( %i = 0; %i < getRecordCount( %mats ); %i++ ) {
		%matInternalName = getRecord( %mats, %i );
		%menu.add(%matInternalName,%i);
	}

	%id = %menu.findText(%layer.matInternalName);

	//If the matInternalName is not found in menu, set as invalid material
	if (%id $= "-1") {
		%id = 0;
		%menu.setText("**"@%layer.matInternalName@"**");
		return;
	}

	%menu.setSelected(%id);
	return %id;
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainPaintGeneratorGui::cancelLayerChanges(%this) {
	TPG_LayerWindow.setVisible(false);
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainPaintGeneratorGui::moveUp(%this, %buttonCtrl) {
	%pillIndex = TPG_StackLayers.getObjectIndex(%buttonCtrl.pill);

	if (%pillIndex <= 0) return;

	%targetPill = TPG_StackLayers.getObject(%pillIndex - 1);
	TPG_StackLayers.reorderChild(%buttonCtrl.pill, %targetPill);
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainPaintGeneratorGui::moveDown(%this, %buttonCtrl) {
	%pillIndex = TPG_StackLayers.getObjectIndex(%buttonCtrl.pill);

	if (%pillIndex > TPG_StackLayers.getCount() - 1) return;

	%targetPill = TPG_StackLayers.getObject(%pillIndex + 1);
	TPG_StackLayers.reorderChild(%targetPill, %buttonCtrl.pill);
}
//------------------------------------------------------------------------------
//==============================================================================
function TPG_PillEdit::onValidate(%this) {
	TPG.validateLayerSetting(%this.internalName,%this.layerObj);
}
//------------------------------------------------------------------------------
