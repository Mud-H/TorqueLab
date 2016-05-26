//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
function LabMat::establishMaterials(%this,%forced) {
	//Cubemap used to preview other cubemaps in the editor.
	if (!isObject(matEdCubeMapPreviewMat))
		singleton CubemapData( matEdCubeMapPreviewMat ) {
		cubeFace[0] = "tlab/materialEditor/assets/cube_xNeg";
		cubeFace[1] = "tlab/materialEditor/assets/cube_xPos";
		cubeFace[2] = "tlab/materialEditor/assets/cube_ZNeg";
		cubeFace[3] = "tlab/materialEditor/assets/cube_ZPos";
		cubeFace[4] = "tlab/materialEditor/assets/cube_YNeg";
		cubeFace[5] = "tlab/materialEditor/assets/cube_YPos";
		parentGroup = "RootGroup";
	};

	//Material used to preview other materials in the editor.
	if (!isObject(materialEd_previewMaterial))
		singleton Material(materialEd_previewMaterial) {
		mapTo = "matEd_mappedMat";
		diffuseMap[0] = "tlab/materialEditor/assets/matEd_mappedMat";
	};

	if (!isObject(materialEd_justAlphaMaterial))
		singleton CustomMaterial( materialEd_justAlphaMaterial ) {
		mapTo = "matEd_mappedMatB";
		texture[0] = materialEd_previewMaterial.diffuseMap[0];
	};

	//Custom shader to allow the display of just the alpha channel.
	if (!isObject(materialEd_justAlphaShader))
		singleton ShaderData( materialEd_justAlphaShader ) {
		DXVertexShaderFile 	= "shaders/alphaOnlyV.hlsl";
		DXPixelShaderFile 	= "shaders/alphaOnlyP.hlsl";
		pixVersion = 1.0;
	};
}
//------------------------------------------------------------------------------
//==============================================================================
function LabMat::open(%this) {

	LabMat.establishMaterials();
	// We hide these specific windows here due too there non-modal nature.
	// These guis are also pushed onto Canvas, which means they shouldn't be parented
	// by editorgui
	MaterialSelector.setVisible(0);
	matEdSaveDialog.setVisible(0);
	MaterialEditorPropertiesWindow-->LabMat_cubemapEditBtn.setVisible(0);
	//Setup our dropdown menu contents.
	//Blending Modes
	MaterialEditorPropertiesWindow-->blendingTypePopUp.clear();
	MaterialEditorPropertiesWindow-->blendingTypePopUp.add(None,0);
	MaterialEditorPropertiesWindow-->blendingTypePopUp.add(Mul,1);
	MaterialEditorPropertiesWindow-->blendingTypePopUp.add(Add,2);
	MaterialEditorPropertiesWindow-->blendingTypePopUp.add(AddAlpha,3);
	MaterialEditorPropertiesWindow-->blendingTypePopUp.add(Sub,4);
	MaterialEditorPropertiesWindow-->blendingTypePopUp.add(LerpAlpha,5);
	MaterialEditorPropertiesWindow-->blendingTypePopUp.setSelected( 0, false );
	//Reflection Types
	MaterialEditorPropertiesWindow-->reflectionTypePopUp.clear();
	MaterialEditorPropertiesWindow-->reflectionTypePopUp.add("None",0);
	MaterialEditorPropertiesWindow-->reflectionTypePopUp.add("cubemap",1);
	MaterialEditorPropertiesWindow-->reflectionTypePopUp.setSelected( 0, false );
	//Sounds
	MaterialEditorPropertiesWindow-->footstepSoundPopup.clear();
	MaterialEditorPropertiesWindow-->impactSoundPopup.clear();
	%sounds = "<None>" TAB "<Soft>" TAB "<Hard>" TAB "<Metal>" TAB "<Snow>";    // Default sounds

	// Get custom sound datablocks
	foreach (%db in DataBlockSet) {
		if (%db.isMemberOfClass("SFXTrack"))
			%sounds = %sounds TAB %db.getName();
	}

	%count = getFieldCount(%sounds);

	for (%i = 0; %i < %count; %i++) {
		%name = getField(%sounds, %i);
		MaterialEditorPropertiesWindow-->footstepSoundPopup.add(%name);
		MaterialEditorPropertiesWindow-->impactSoundPopup.add(%name);
	}

	//Preview Models
	matEd_quickPreview_Popup.clear();
	matEd_quickPreview_Popup.add("Cube",0);
	matEd_quickPreview_Popup.add("Sphere",1);
	matEd_quickPreview_Popup.add("Pyramid",2);
	matEd_quickPreview_Popup.add("Cylinder",3);
	matEd_quickPreview_Popup.add("Torus",4);
	matEd_quickPreview_Popup.add("Knot",5);
	matEd_quickPreview_Popup.setSelected( 0, false );
	matEd_quickPreview_Popup.selected = matEd_quickPreview_Popup.getText();
	MaterialEditorPropertiesWindow-->MaterialLayerCtrl.clear();
	MaterialEditorPropertiesWindow-->MaterialLayerCtrl.add("Layer 0",0);
	MaterialEditorPropertiesWindow-->MaterialLayerCtrl.add("Layer 1",1);
	MaterialEditorPropertiesWindow-->MaterialLayerCtrl.add("Layer 2",2);
	MaterialEditorPropertiesWindow-->MaterialLayerCtrl.add("Layer 3",3);
	MaterialEditorPropertiesWindow-->MaterialLayerCtrl.setSelected( 0, false );
	//Sift through the RootGroup and find all loaded material items.
	LabMat_cubemapEd_availableCubemapList.clear();
	//LabMat.updateAllFields();
	LabMat.updatePreviewObject();
	// If no selected object; go to material mode. And edit the last selected material
	LabMat.setMode();
	LabMat.preventUndo = true;

	if( LabMat.currentMode $= "Mesh" )
		LabMat.prepareActiveObject( true );
	else
		LabMat.prepareActiveMaterial( "", true );

	LabMat.preventUndo = false;
}
//------------------------------------------------------------------------------
