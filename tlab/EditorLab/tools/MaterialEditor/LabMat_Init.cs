//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
function LabMat::init(%this,%forced) {
	//Cubemap used to preview other cubemaps in the editor.
	if (!isObject(LabMat_CubeMapPreviewMat))
		singleton CubemapData( LabMat_CubeMapPreviewMat ) {
		cubeFace[0] = "tlab/materialEditor/assets/cube_xNeg";
		cubeFace[1] = "tlab/materialEditor/assets/cube_xPos";
		cubeFace[2] = "tlab/materialEditor/assets/cube_ZNeg";
		cubeFace[3] = "tlab/materialEditor/assets/cube_ZPos";
		cubeFace[4] = "tlab/materialEditor/assets/cube_YNeg";
		cubeFace[5] = "tlab/materialEditor/assets/cube_YPos";
		parentGroup = "RootGroup";
	};

	if (!isObject(LabMat_previewMaterial))
		singleton Material(LabMat_previewMaterial) {
		mapTo = "LabMat_mappedMat";
		diffuseMap[0] = "tlab/materialEditor/assets/matEd_mappedMat";
	};

	if (!isObject(LabMat_justAlphaMaterial))
		singleton CustomMaterial( LabMat_justAlphaMaterial ) {
		mapTo = "LabMat_mappedMatB";
		texture[0] = LabMat_previewMaterial.diffuseMap[0];
	};

	//Custom shader to allow the display of just the alpha channel.
	if (!isObject(LabMat_justAlphaShader))
		singleton ShaderData( LabMat_justAlphaShader ) {
		DXVertexShaderFile 	= "shaders/alphaOnlyV.hlsl";
		DXPixelShaderFile 	= "shaders/alphaOnlyP.hlsl";
		pixVersion = 1.0;
	};
	//Custom shader to allow the display of just the alpha channel.
	if (!isObject(LabMat_PM))
		new PersistenceManager(LabMat_PM);
	
}
//------------------------------------------------------------------------------
//==============================================================================
function LabMat::open(%this) {

	//Preview Models
	LabMat_PreviewModelMenu.clear();
	LabMat_PreviewModelMenu.add("Cube",0);
	LabMat_PreviewModelMenu.add("Sphere",1);
	LabMat_PreviewModelMenu.add("Pyramid",2);
	LabMat_PreviewModelMenu.add("Cylinder",3);
	LabMat_PreviewModelMenu.add("Torus",4);
	LabMat_PreviewModelMenu.add("Knot",5);
	LabMat_PreviewModelMenu.setSelected( 0, false );
	LabMat_PreviewModelMenu.selected = LabMat_PreviewModelMenu.getText();
	LabMat_ActiveCtrl-->MaterialLayerCtrl.clear();
	LabMat_ActiveCtrl-->MaterialLayerCtrl.add("Layer 0",0);
	LabMat_ActiveCtrl-->MaterialLayerCtrl.add("Layer 1",1);
	LabMat_ActiveCtrl-->MaterialLayerCtrl.add("Layer 2",2);
	LabMat_ActiveCtrl-->MaterialLayerCtrl.add("Layer 3",3);
	LabMat_ActiveCtrl-->MaterialLayerCtrl.setSelected( 0, false );
	
		LabMat_PropertiesStack-->LabMat_cubemapEditBtn.setVisible(0);
	//Setup our dropdown menu contents.
	//Blending Modes
	LabMat_PropertiesStack-->blendingTypePopUp.clear();
	LabMat_PropertiesStack-->blendingTypePopUp.add(None,0);
	LabMat_PropertiesStack-->blendingTypePopUp.add(Mul,1);
	LabMat_PropertiesStack-->blendingTypePopUp.add(Add,2);
	LabMat_PropertiesStack-->blendingTypePopUp.add(AddAlpha,3);
	LabMat_PropertiesStack-->blendingTypePopUp.add(Sub,4);
	LabMat_PropertiesStack-->blendingTypePopUp.add(LerpAlpha,5);
	LabMat_PropertiesStack-->blendingTypePopUp.setSelected( 0, false );
	//Reflection Types
	LabMat_PropertiesStack-->reflectionTypePopUp.clear();
	LabMat_PropertiesStack-->reflectionTypePopUp.add("None",0);
	LabMat_PropertiesStack-->reflectionTypePopUp.add("cubemap",1);
	LabMat_PropertiesStack-->reflectionTypePopUp.setSelected( 0, false );
	//Sounds
	LabMat_PropertiesStack-->footstepSoundPopup.clear();
	LabMat_PropertiesStack-->impactSoundPopup.clear();
	
	%sounds = "<None>" TAB "<Soft>" TAB "<Hard>" TAB "<Metal>" TAB "<Snow>";    // Default sounds
%count = getFieldCount(%sounds);

	for (%i = 0; %i < %count; %i++) {
		%name = getField(%sounds, %i);
		LabMat_PropertiesStack-->footstepSoundPopup.add(%name);
		LabMat_PropertiesStack-->impactSoundPopup.add(%name);
	}
	// Get custom sound datablocks
	foreach (%db in DataBlockSet) {
		if (%db.isMemberOfClass("SFXTrack"))
			%sounds = %sounds TAB %db.getName();
	}
	
	LabMat.updatePreviewObject();
	return;
	// We hide these specific windows here due too there non-modal nature.
	// These guis are also pushed onto Canvas, which means they shouldn't be parented
	// by editorgui
	MaterialSelector.setVisible(0);
	matEdSaveDialog.setVisible(0);

	

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
