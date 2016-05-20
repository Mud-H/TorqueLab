//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$MatEdRollOutList = "TextureMaps PBR Rendering MaterialDamage Accumulation lighting animation advanced";
$MatEd_TextureMaps = "Diffuse Normal Detail Specular DetailNormal Overlay Light Tone Environment";


$MatEd_RolloutExpanded["TextureMaps"] = true;

$Cfg_MaterialEditor_PropShowGroup_["TextureMaps"] = 1;
$Cfg_MaterialEditor_PropShowMap_["Diffuse"] = 1;

//==============================================================================
// Helper functions to help load and update the preview and active material
//------------------------------------------------------------------------------
//==============================================================================
// Finds the selected line in the material list, then makes it active in the editor.
function MaterialEditorTools::initGui(%this) {
	foreach(%ctrl in MatEd_Rollouts){
	   if (!%ctrl.isMemberOfClass("GuiRolloutCtrl"))
	      continue;	
		
		%ctrl.expanded = $MatEd_RolloutExpanded[%ctrl.internalName];
		%ctrl.visible = $Cfg_MaterialEditor_PropShowGroup_[%ctrl.internalName];
	}
	foreach$(%map in $MatEd_TextureMaps){
	   %ctrl = MEP_TextureMapStack.findObjectByInternalName(%map);
	   if (!isObject(%ctrl))
	      continue;
	      
		%ctrl.visible = $Cfg_MaterialEditor_PropShowMap_[%map];
	}	
}
//------------------------------------------------------------------------------

//==============================================================================
// Activate GUIs - called when Material Editor is activated
function MaterialEditorTools::activateGui(%this ) {
	//advancedTextureMapsRollout.Expanded = false;
	//materialAnimationPropertiesRollout.Expanded = false;
	//materialAdvancedPropertiesRollout.Expanded = false;
	
	MatEd.pbrMapMode(MatEd.MapModePBR);
	%radio = MatEd_PBRmodeRadios.findObjectByInternalName(MatEd.MapModePBR);
	if (isObject(%radio))
		%radio.setStateOn(true);
	MEP_MatOptionRollout.expanded = 0;
}
//------------------------------------------------------------------------------
/*
//==============================================================================
function MaterialEditorPlugin::initPropertySetting(%this) {
	devLog("MaterialEditorPlugin::initPropertySetting(%this)",%this,%ctrl,"Name",%ctrl.internalName);
	%textureMapStack = MaterialEditorTools-->textureMapsOptionsStack;

	foreach(%ctrl in %textureMapStack) {
		%map = %ctrl.internalName;
		%ctrl.variable = "$Cfg_MaterialEditor_PropShowMap_"@%map;
	}
}
//------------------------------------------------------------------------------
*/
//==============================================================================
// Material Properties Display Options Callback
//==============================================================================
//==============================================================================
function MaterialEditorTools::toggleMap(%this,%ctrl) {	
	%map = %ctrl.internalName;
	%visible = %ctrl.isStateOn();
	$MEP_ShowMap[%map] = %visible;
	%cont = MEP_TextureMapStack.findObjectByInternalName(%map,true);
	%cont.setVisible(%visible);
	MaterialEditorPlugin.setCfg("PropShowMap_"@%map,%visible);
}
//------------------------------------------------------------------------------
//==============================================================================
function MaterialEditorTools::toggleGroup(%this,%ctrl) {
	
	%group = %ctrl.internalName;
	%visible = %ctrl.isStateOn();

	  %groupCtrl = MatEd_Rollouts.findObjectByInternalName(%group);

	if (!isObject(%groupCtrl))
		return;

	%groupCtrl.setVisible(%visible);
	MaterialEditorPlugin.setCfg("PropShowGroup_"@%group,%visible);
}
//------------------------------------------------------------------------------


//==============================================================================
// Image thumbnail right-clicks.

//==============================================================================
// Set Material GUI Mode (Mesh or Standard)
//==============================================================================
// Set GUI depending of if we have a standard Material or a Mesh Material
function MaterialEditorTools::setMode( %this ) {
	MatEdMaterialMode.setVisible(0);
	MatEdTargetMode.setVisible(0);

	if( isObject(MaterialEditorTools.currentObject) ) {
		MaterialEditorTools.currentMode = "Mesh";
		MatEdTargetMode.setVisible(1);
	} else {
		MaterialEditorTools.currentMode = "Material";
		MatEdMaterialMode.setVisible(1);
		EWorldEditor.clearSelection();
	}
}
//------------------------------------------------------------------------------
