//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// MaterialEditorTools.activatePBR();
function MatED::enablePBRMode(%this ) {
	%enabled = $Cfg_MaterialEditor_PropShowGroup_PBR;
	devLog("MatEd PBR enabled?",$Cfg_MaterialEditor_PropShowGroup_PBR);

	MatEd.PBRenabled = %enabled;
	MEP_PBRSettings.visible = %enabled;	
}
//------------------------------------------------------------------------------

//==============================================================================
// MaterialEditorTools.activatePBR();
function MaterialEditorTools::togglePBR(%this ) {
	%this.activatePBR(!MatEd.PBRenabled);
	$MatEd_PbrEnabled = MatEd.PBRenabled;
}
//------------------------------------------------------------------------------
//==============================================================================
// MaterialEditorTools.activatePBR();
function MaterialEditorTools::activatePBR(%this,%activate ) {
	if (%activate $= "")
		%activate = true;

	MatEd.PBRenabled = %activate;
	MEP_PBRSettings.visible = %activate;
	pbr_materialDamageProperties.visible = %activate;
	pbr_lightingProperties.visible = %activate;
	pbr_accumulationProperties.visible = %activate;
	MEP_MatOptionRollout-->PBR.visible = %activate;
	//MEP_SpecularContainer.visible = !%activate;
	MEP_TextureMapStack-->specular.visible = !%activate;
}
//------------------------------------------------------------------------------
//==============================================================================
// Mode : 0 = all maps >> 1 = Comp only >> 2 = No COmp
function MatEd::pbrMapMode(%this,%mode ) {
	switch$(%mode) {
	case "0":
		MEP_PBRSettings-->compMap.visible = 1;
		MEP_PBRSettings-->smoothMap.visible = 1;
		MEP_PBRSettings-->aoMap.visible = 1;
		MEP_PBRSettings-->metalMap.visible = 1;

	case "1":
		MEP_PBRSettings-->compMap.visible = 1;
		MEP_PBRSettings-->smoothMap.visible = 0;
		MEP_PBRSettings-->aoMap.visible = 0;
		MEP_PBRSettings-->metalMap.visible = 0;

	case "2":
		MEP_PBRSettings-->compMap.visible = 0;
		MEP_PBRSettings-->smoothMap.visible = 1;
		MEP_PBRSettings-->aoMap.visible = 1;
		MEP_PBRSettings-->metalMap.visible = 1;
	}
}
//------------------------------------------------------------------------------