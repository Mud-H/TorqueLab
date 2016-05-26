//==============================================================================
// TorqueLab -> Editor Gui Open and Closing States
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
if (!isObject(LabMat))
   new ScriptObject(LabMat);
//==============================================================================
// Initial Editor launch call from EditorManager
function LabMaterialEditor::onWake(%this) {
   
	%selObj = EWorldEditor.getSelectedObject(0);
	if (isObject(%selObj))
	   LabMat.currentObject = %selObj;

   LabMat.updateObjectMaterials();
   LabMat.open();
   LabMatOptions.expanded = 0;
}
//-----------------------------------------------------------------------------

//==============================================================================
function LabMaterialEditor::toggleMapCtrl(%this,%ctrl) {	
  
	%map = %ctrl.internalName;
	%visible = %ctrl.isStateOn();
	%this.toggleMap(%map,%visible);
	

}
//------------------------------------------------------------------------------
//==============================================================================
function LabMaterialEditor::toggleMap(%this,%map,%visible) {	
   
  $LabMat_ShowMap[%map] = %visible;
	%cont = LabMat_MapsStack.findObjectByInternalName(%map@"Container",true);
	%cont.setVisible(%visible);
}
//------------------------------------------------------------------------------


//==============================================================================
// LabMat.activatePBR();
function LabMat::togglePBR(%this ) {
	%this.activatePBR(!LabMat.PBRenabled);
	$LabMat_PbrEnabled = LabMat.PBRenabled;
}
//------------------------------------------------------------------------------
//==============================================================================
// LabMat.activatePBR();
function LabMat::activatePBR(%this,%activate ) {
	if (%activate $= "")
		%activate = true;

	LabMat.PBRenabled = %activate;
	LabMat_PBRSettings.visible = %activate;
	LabMat_MapsStack-->MaterialDamageContainer.visible = %activate;	
	LabMat_MapsStack-->AccumulationContainer.visible = %activate;
	LabMatOptions-->PBR.visible = %activate;
	//MEP_SpecularContainer.visible = !%activate;
	LabMat_MapsStack-->specularContainer.visible = !%activate;
}
//------------------------------------------------------------------------------
