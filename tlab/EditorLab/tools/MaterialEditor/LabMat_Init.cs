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