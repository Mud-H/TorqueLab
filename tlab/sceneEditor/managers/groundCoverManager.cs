//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$SEP_GroundCoverDefault_["Material"] = "grass1";

$SEP_GroundCoverDefault_["radius"] = "400";
$SEP_GroundCoverDefault_["dissolveRadius"] = "200";
$SEP_GroundCoverDefault_["shapeCullRadius"] = "400";
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_GroundCover::onWake( %this ) {
	hide(SEP_GroundCoverInspectorScroll);
	hide(SEP_GroundCoverLayerPill);
	SEP_GroundCoverSaveButton.active = SEP_GroundCover.isDirty;
	SEP_GroundCover.getMissionGroundCover();
	SEP_GroundCoverCommonSettings.selectPage(0);
	
	if (SEP_GroundCoverLayerArray.getCount() $= "0")
		SEP_GroundCover.rebuildSettings();
	
	if (isObject(SEP_GroundCover.selectedGroundCover))
		%text = SEP_GroundCover.selectedGroundCover.getName() SPC "- Layers settings";
	else
		%text = "No GroundCover object selected";
	
	SEP_GroundCoverDataTitle.text = %text;
}
//------------------------------------------------------------------------------
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_GroundCover::onSleep( %this ) {
}
//------------------------------------------------------------------------------
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_GroundCover::createGroundCover( %this ) {
	if (!isObject($SEP_GroundCoverDefault_["Material"])) {
		warnLog("Invalid default material for GroundCover:",$SEP_GroundCoverDefault_["Material"]);
		return;
	}

	%name = getUniqueName("envGroundCover");
	%groundCover = new GroundCover(%name) {
		Material = $SEP_GroundCoverDefault_["Material"];
		radius = $SEP_GroundCoverDefault_["radius"];
		dissolveRadius = $SEP_GroundCoverDefault_["dissolveRadius"];
		shapeCullRadius = $SEP_GroundCoverDefault_["shapeCullRadius"];
	};
	%group = Scene.getActiveSimGroup();

	if (!isObject(%group))
		%group = MissionGroup;

	%groundCover.setFileName(%group.getFilename());
	%group.add(%groundCover);
	SEP_GroundCover.onGroundCoverSelected(%groundCover);
	//SEP_GroundCover.selectedGroundCover = %groundCover;
	SEP_GroundCover.getMissionGroundCover();
}
//------------------------------------------------------------------------------
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_GroundCover::toggleInspector( %this ) {
	SEP_GroundCoverInspectorScroll.visible = !SEP_GroundCoverInspectorScroll.visible;
}
//------------------------------------------------------------------------------
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_GroundCover::updateFieldValue( %this,%field,%value,%layerId ) {
	%obj = SEP_GroundCover.selectedGroundCover;

	if (!isObject(%obj)) {
		warnLog("Can't update ground cover value because none is selected. Tried wth:",%obj);
		return;
	}

	%currentValue = %obj.getFieldValue(%field,%layerId);

	if (%currentValue $= %value) {
		return;
	}

	GroundCoverInspector.apply();
	//eval("%obj."@%checkField@" = %value;");
	%obj.setFieldValue(%field,%value,%layerId);
	EWorldEditor.isDirty = true;
	%this.setDirty();
}
//------------------------------------------------------------------------------
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_GroundCover::setDirty( %this ) {
	SEP_GroundCover.isDirty = true;
	SEP_GroundCoverSaveButton.active = true;
}
//------------------------------------------------------------------------------
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_GroundCover::setNotDirty( %this ) {
	SEP_GroundCover.isDirty = false;
	SEP_GroundCoverSaveButton.active = false;
}
//------------------------------------------------------------------------------
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_GroundCover::saveData( %this ) {
	%obj = SEP_GroundCover.selectedGroundCover;

	if (!isObject(%obj)) {
		warnLog("Can't update ground cover value because none is selected. Tried wth:",%obj);
		return;
	}

	Lab_PM.setDirty(%obj);
	Lab_PM.saveDirty();
	%this.setNotDirty();
}
//------------------------------------------------------------------------------
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_GroundCover::deleteObj( %this ) {
	%obj = SEP_GroundCover.selectedGroundCover;
	LabMsgOkCancel("Delete selected GroundCover","You are about to delete the current GroundCover:" SPC %obj.getName() @ ". Proceed with GroundCover deletion?","delObj("@%obj.getId()@");");
}
//------------------------------------------------------------------------------
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_GroundCover::rebuildSettings( %this ) {
	SEP_GroundCover.buildLayerSettingGui();
}
//------------------------------------------------------------------------------

//==============================================================================
// SEP_GroundCover.getMissionGroundCover();
function SEP_GroundCover::getMissionGroundCover( %this ) {
	SEP_GroundCover.groundCoverList = Lab.getMissionObjectClassList("GroundCover");
	SEP_GroundCoverMenu.clear();
	SEP_GroundCoverMenu.add("None selected",0);
	%selected = 0;

	foreach$(%obj in SEP_GroundCover.groundCoverList) {
		SEP_GroundCoverMenu.add(%obj.getName(),%obj.getId());

		if (SEP_GroundCover.selectedGroundCover.getId() $= %obj.getId())
			%selected = %obj.getId();
	}

	SEP_GroundCoverMenu.setSelected(%selected,false);
}
//------------------------------------------------------------------------------

//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_GroundCoverMenu::onSelect( %this,%id,%text ) {
	logd("SEP_GroundCoverMenu::onSelect( %this,%id,%text )", %this,%id,%text);

	if (isObject(%id))
		SEP_GroundCover.onGroundCoverSelected(%id);
}
//------------------------------------------------------------------------------
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_GroundCover::onGroundCoverSelected( %this,%obj ) {
	logd("SEP_GroundCover know about the selected groundcover:",%obj.getName());
	SceneEditorToolbar-->groundCoverToolbar.visible = 1;
	SEP_GroundCoverDeleteButton.active = true;

	%this.updateGroundCoverLayers(%obj);
	

	%text = %obj.getName() SPC "- Layers settings";		
	SEP_GroundCoverDataTitle.text = %text;
}
//------------------------------------------------------------------------------
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_GroundCover::onGroundCoverUnselected( %this,%obj ) {
	logd("SEP_GroundCover know about the unselected groundcover:",%obj.getName());
	SceneEditorToolbar-->groundCoverToolbar.visible = 0;
	hide(SEP_GroundCoverLayerArray);
	hide(SEP_GroundCoverCommonSettings);
}
//------------------------------------------------------------------------------

//==============================================================================
// GroundCover Layers GUI generation
//==============================================================================
$SEP_GroundCoverLayerCount = 8;
%fields = "billboardUVs invertLayer probability shapeFilename shapeFilename_button layer layer_menu windScale";
%fieldsA = "sizeMin sizeMax sizeExponent minSlope maxSlope minElevation maxElevation minClumpCount maxClumpCount clumpExponent clumpRadius";
$SEP_GroundCoverLayerFields = %fields SPC %fieldsA;
%cFields = "Material radius dissolveRadius reflectScale gridSize zOffset seed maxElements maxBillboardTiltAngle shapeCullRadius shapesCastShadows";
%cFieldsA = "windDirection windGustLength windGustFrequency windGustStrength windTurbulenceFrequency windTurbulenceStrength lockFrustum renderCells noBillboards noShapes";
$SEP_GroundCoverCommonFields = %cFields SPC %cFieldsA;
//==============================================================================
// The layers settings Gui is generated from a source Gui structure
function SEP_GroundCover::buildLayerSettingGui( %this ) {
	show(SEP_GroundCoverLayerArray);
	SEP_GroundCoverLayerArray.clear();
	%pillSrc = SEP_GroundCoverLayerPill;

	for(%i=0; %i< $SEP_GroundCoverLayerCount; %i++) {
		%pill = cloneObject(	%pillSrc,"","layer_"@%i,SEP_GroundCoverLayerArray);
		%pill.layerId = %i;

		foreach$(%field in $SEP_GroundCoverLayerFields) {
			eval("%pill-->"@%field@".layerId = %i;");
		}
	}

	hide(SEP_GroundCoverLayerArray);
}
//------------------------------------------------------------------------------
//==============================================================================
// The layers settings Gui is generated from a source Gui structure SEP_GroundCover.updateGroundCoverLayers(newGroundCover)
function SEP_GroundCover::updateGroundCoverLayers( %this,%groundCover ) {
	GroundCoverInspector.inspect(%groundCover);
	SEP_GroundCover.selectedGroundCover = %groundCover.getId();
	SEP_GroundCoverMenu.setText(%groundCover.getName());
	show(SEP_GroundCoverLayerArray);
	show(SEP_GroundCoverCommonSettings);

	foreach$(%field in $SEP_GroundCoverCommonFields) {
		%fieldCtrl = SEP_GroundCoverCommonSettings.findObjectByInternalName(%field,true);

		if (!isObject(%fieldCtrl)) {
			warnLog("Trying to update an invalid control for GroundCover common field:",%field);
			continue;
		}

		%fieldOnly = getWord(strreplace(%field,"_"," "),0);
		eval("%value = %groundCover."@%fieldOnly@";");
		%fieldCtrl.setTypeValue(%value);
	}

	foreach(%obj in SEP_GroundCoverLayerArray) {
		%layerId = %obj.layerId;

		if (%layerId $= "") {
			warnLog("Trying to update an invalid LayerId for GroundCover:",%groundCover.getName());
			continue;
		}

		foreach$(%field in $SEP_GroundCoverLayerFields) {
			%fieldCtrl = %obj.findObjectByInternalName(%field,true);

			if (!isObject(%fieldCtrl)) {
				warnLog("Trying to update an invalid control for GroundCover layer:",%layerId,"Field",%field);
				continue;
			}

			%fieldOnly = getWord(strreplace(%field,"_"," "),0);
			eval("%value = %groundCover."@%fieldOnly@"["@%layerId@"];");
			%fieldCtrl.setTypeValue(%value);
		}
	}
}
//------------------------------------------------------------------------------
//==============================================================================
// GroundCover Settings functions
//==============================================================================
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_GroundCover::selectLayer( %this,%menu ) {
	SEP_GroundCover.currentTerrainLayerId = %menu.layerId;
	MaterialSelector.showTerrainDialog("SEP_GroundCover.applyLayer", "name");
}
//------------------------------------------------------------------------------
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_GroundCover::applyLayer( %this,%layerName ) {
	%layerId = SEP_GroundCover.currentTerrainLayerId;
	eval("%container = SEP_GroundCoverLayerArray-->layer_"@%layerId@";");
	%fieldCtrl = %container.findObjectByInternalName("layer",true);
	%fieldCtrl.setText(%layerName);
	SEP_GroundCover.updateFieldValue("layer",%layerName,%layerId);
}
//------------------------------------------------------------------------------
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_GroundCover::selectShapeFile( %this,%button ) {
	%current = SEP_GroundCover.selectedGroundCover.getFieldValue("shapeFilename",%button.layerId);
	SEP_GroundCover.currentShapeFileLayerId = %button.layerId;
	getLoadFilename("*.*|*.*", "SEP_GroundCover.applyShapeFile", %current);
}
//------------------------------------------------------------------------------
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_GroundCover::applyShapeFile( %this,%file ) {
	%file = makeRelativePath(%file );
	%layerId = %this.currentShapeFileLayerId;
	eval("%container = SEP_GroundCoverLayerArray-->layer_"@%layerId@";");
	%fieldCtrl = %container.findObjectByInternalName("shapeFilename",true);
	%fieldCtrl.setText(%file);
	SEP_GroundCover.updateFieldValue("shapeFilename",%file,%layerId);
}
//------------------------------------------------------------------------------
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_GroundCover::selectCommonMaterial( %this,%button ) {
	MaterialSelector.showDialog("SEP_GroundCover.applyCommonMaterial", "name");
}
//------------------------------------------------------------------------------
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_GroundCover::applyCommonMaterial( %this,%materialName ) {
	logd("SEP_GroundCover::applyCommonMaterial( %this,%materialName )",%this,%materialName);
	%fieldCtrl = SEP_GroundCoverCommonSettings.findObjectByInternalName("Material",true);
	%fieldCtrl.setText(%materialName);
	SEP_GroundCover.updateFieldValue("Material",%materialName);
}
//------------------------------------------------------------------------------
//fieldValue = %object.getFieldValue( %fieldName, %arrayIndex );
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_GroundCoverEdit::onValidate( %this ) {
	logd("SEP_GroundCoverCheck::onClick( %this )",%this.internalName,"LayerId=",%this.layerId);
	%fieldOnly = getWord(strreplace(%this.internalName,"_"," "),0);
	SEP_GroundCover.updateFieldValue(%fieldOnly,%this.getValue(),%this.layerId);
}
//------------------------------------------------------------------------------
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_GroundCoverCheck::onClick( %this ) {
	logd("SEP_GroundCoverCheck::onClick( %this )",%this.internalName);
}
//------------------------------------------------------------------------------


