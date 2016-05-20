//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SceneEd::genAutoLightSelection( %this ) {
	%count = EWorldEditor.getSelectionSize();
	if (%count < 1) {
		warnLog("There's no selected objects to generate lights!");
		return;
	}
	%group = %this.getActiveSimGroup();	
	for( %j=0; %j<%count; %j++) {
		%obj = EWorldEditor.getSelectedObject( %j );
		addLights(%obj);
	}	
}
//------------------------------------------------------------------------------

//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SceneEd::updateAutoLightTree( %this ) {
	SEP_AutoLightSetTree.open(AutoLightsSet);
	SEP_AutoLightSetTree.buildVisibleTree();
}
//------------------------------------------------------------------------------


//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SceneEd::scanAutoLights( %this ) {
	//%lightShapes = getMissionObjectClassList("TSStatic","refLight NotEmpty");
	//foreach$(%shape in %lightShapes)
		//AutoLightsSet.add(%shape);
		
	%this.updateAutoLightTree();
}
//------------------------------------------------------------------------------
