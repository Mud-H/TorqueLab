//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
function EAutoLightManager::initParams( %this ) {
	%arCfg = Lab.createBaseParamsArray("EAutoLightParams",EAutoLight_LightParamsGroup);
	%arCfg.updateFunc = "EAutoLightManager.updateParam";
	%arCfg.style = "StyleA";
	%arCfg.useNewSystem = true;
	%arCfg.noDirectSync = true;
	%arCfg.group[%gid++] = "Lighting" TAB "Stack StackA";
	%arCfg.setVal("brightness",       "" TAB "Brightness" TAB "SliderEdit" TAB "range>>0 50;;tickAt>>0.1" TAB "EAutoLightManager.activeLight" TAB %gid);
	%arCfg.setVal("radius",       "" TAB "Radius" TAB "SliderEdit" TAB "range>>0 50;;tickAt>>0.1" TAB "EAutoLightManager.activeLight" TAB %gid);
	%arCfg.setVal("color",       "" TAB "Color" TAB "ColorSliderEdit" TAB "mode>>float;;flen>>2" TAB "EAutoLightManager.activeLight" TAB %gid);
	
	buildParamsArray(%arCfg,false);
	EAutoLightManager.paramArray = %arCfg;
}
//------------------------------------------------------------------------------


//==============================================================================
function EAutoLightManager::updateFieldValue(%this,%field,%value,%obj) {
	logc("EAutoLightManager::updateFieldValue(%this,%field,%value,%obj)",%this,%field,%value,%obj);

	
	%count = EWorldEditor.getSelectionSize();
	for( %i=0; %i<%count; %i++) {
		%obj = EWorldEditor.getSelectedObject( %i );
		if (%obj.isMemberOfClass("PointLight"))
			%lightObjList = strAddWord(%lightObjList,%obj.getId(),1);
	}

	if (%lightObjList $= ""){
		warnLog("No active PointLight found");
		return;
	}
	
	foreach$(%lightObj in %lightObjList){
		if (!isObject(%lightObj))
			continue;			
		LabObj.set(%lightObj,%field,%value);
	//%this.setDirtyObject(%lightObj);
	}
}
//------------------------------------------------------------------------------

//==============================================================================
function EAutoLightManager::updateParam(%this,%field,%value,%ctrl,%array,%arg1,%arg2) {
	logc("EAutoLightManager::updateParam(%this,%field,%value,%ctrl,%array,%arg1,%arg2)",%this,%field,%value,%ctrl,%array,%arg1,%arg2);
	%arrayValue = %array.getVal(%field);
	%obj = getField(%arrayValue,4);
	%this.updateFieldValue(%field,%value,%obj);
}
//------------------------------------------------------------------------------
//==============================================================================
function EAutoLightManager::setDirtyObject(%this,%obj,%isDirty) {
	logd("EAutoLightManager::setDirtyObject(%this,%obj,%isDirty)",%this,%obj,%isDirty);
	
	if (!isObject(%obj))
		return "-1";

	if (%isDirty !$= "") {
		LabObj.setDirty(%obj,%isDirty);
	}

	%objIsDirty = LabObj.isDirty(%obj);

	if (%objIsDirty) {
		EWorldEditor.isDirty = true;
		return "1";
	}

	return "0";
	//SEP_ScatterSkySaveButton.active = %objIsDirty;
}
//------------------------------------------------------------------------------
