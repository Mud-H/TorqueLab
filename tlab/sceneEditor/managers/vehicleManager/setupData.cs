//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
//%field,%value,%ctrl,%array,%arg1,%arg2
function sepVM::updateParam( %this,%field,%value,%ctrl,%array,%arg1,%arg2 ) {
	logd("sepVM::updateParam( %this,%field,%value,%ctrl,%array,%arg1,%arg2 )",%field,%value,%ctrl,%array,%arg1,%arg2);
	%data = %array.getVal(%field);
	%updObj = getField(%data,%array.syncObjsField);
	%isDirty = LabObj.set(%updObj,%field,%value);

	if ($sepVM_ApplyChangeToDatablock) {
		%updData = %updObj.internalName;
		%isDirty = LabObj.set(%updData,%field,%value);
	}

	if (%isDirty) {
		%menu = $sepVM_DataClassMenu[%updObj.getName()];
		%menu.setText(%updObj.internalName @ "*");
		sepVM_WheeledApplyButton.active = 1;
	}
}
//------------------------------------------------------------------------------


//==============================================================================
//sepVM.rebuildAllParams
function sepVM::rebuildAllParams( %this ) {
	logd("sepVM::rebuildAllParams( %this)",%this);
	exec("tlab/sceneEditor/vehicleManager/paramsWheeledVehicle.cs");
	%this.buildWheeledParams();
}
//------------------------------------------------------------------------------

//==============================================================================
//sepVM.applyChanges
function sepVM::applyChanges( %this ) {
	%dirtyCount = 0;

	foreach(%obj in sepVM_TempDatablocks) {
		%isDirty = Lab_PM.isDirty(%obj);

		if (!%isDirty)
			continue;

		%dirtyCount++;
		%srcData = %obj.internalName;
		%srcData.assignFieldsFrom(%obj);
		%this.cleanWheeledData(%srcData);
		Lab_PM.removeField(%srcData,"internalName");
		LabObj.save(%srcData,true);
		Lab_PM.removeDirty(%obj);
		%menu = $sepVM_DataClassMenu[%obj.getName()];
		%menu.setText(%obj.internalName);
	}

	sepVM_WheeledApplyButton.active = 0;
	info("Changes applied for wheeled datablocks!",%dirtyCount,"Object saved");
}
//------------------------------------------------------------------------------

//==============================================================================
//sepVM.applyChanges
function sepVM::cloneData( %this,%type ) {
	%target = %type@"_vm_clone";
	%menu = "seVM_CloneMenu_"@%type;
	%source = %menu.getText();

	if (!isObject(%source)) {
		warnLog("Trying to clone invalid source:",%source);
		return;
	}

	if (!isObject(%target)) {
		warnLog("Trying to clone invalid target:",%target);
		return;
	}

	%target.assignFieldsFrom(%source);
	Lab_PM.setDirty(%target);
	sepVM_WheeledApplyButton.active = 1;
	info(%source.getName(),"have been cloned into:",%target.internalName);
}
//------------------------------------------------------------------------------

//==============================================================================
//sepVM.applyChanges
function sepVM::newDataFromSource( %this,%type ) {
	%textEdit = "seVM_NewName_"@%type;
	%name = %textEdit.getText();
	%unique = getUniqueName(%name);
	%menu = "seVM_CloneMenu_"@%type;
	%source = %menu.getText();

	if (!isObject(%source)) {
		warnLog("Trying to clone invalid source:",%source);
		return;
	}

	%newObj = %source.deepClone();
	%newObj.setName(%unique);
	%newObj.setFilename(%source.getFilename());
	DataBlockGroup.add(%newObj);
	Lab_PM.setDirty(%newObj);
	sepVM_WheeledApplyButton.active = 1;
	%field = getWord(strreplace(%type,"_"," "),1);
	%this.selectWheeledData(%newObj,%field);
}
//------------------------------------------------------------------------------
//==============================================================================
//sepVM.applyChanges
function sepVM::setDirty( %this,%obj,%notDirty ) {
	if (%notDirty) {
		Lab_PM.removeDirty(%obj);
		return;
	}

	Lab_PM.setDirty(%obj);
	sepVM_WheeledApplyButton.active = 1;
}
//------------------------------------------------------------------------------