//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$sepVM_FrontRearDataAllowed = true;
//==============================================================================
//%field,%value,%ctrl,%array,%arg1,%arg2
function sepVM::cloneWheeledDatablock( %this,%class,%type,%obj ) {
	logd("sepVM::cloneWheeledDatablock( %this,%class,%type,%obj )",%class,%type,%obj);
	%name = %class@"_"@%type@"_vm_clone";
	delObj(%name);
	%newObj = %obj.deepClone();
	%newObj.setName(%name);
	%newObj.internalName = %obj.getName();
	//eval("sepVM."@%class@%type@" = %newObj;");
	%menu = $sepVM_DataClassMenu[%name];
	%menu.setText(%obj.getName());

	switch$(%type) {
	case "Vehicle":
		sepVM_WheeledCloneCtrl-->cloneTextVehicle.text = "Target:\c2" SPC %obj.getName();
		syncParamArray(arsepVM_WheeledVehicleParam);

	case "tire1":
		Wheeled_Vehicle_vm_clone.tireData1 = %obj.getName();
		sepVM.setDirty(Wheeled_Vehicle_vm_clone);
		syncParamArray(arsepVM_WheeledWheels1Param);

	case "spring1":
		Wheeled_Vehicle_vm_clone.springData1 = %obj.getName();
		sepVM.setDirty(Wheeled_Vehicle_vm_clone);
		syncParamArray(arsepVM_WheeledWheels1Param);

	case "tire2":
		Wheeled_Vehicle_vm_clone.tireData2 = %obj.getName();
		sepVM.setDirty(Wheeled_Vehicle_vm_clone);
		syncParamArray(arsepVM_WheeledWheels2Param);

	case "spring2":
		Wheeled_Vehicle_vm_clone.springData2 = %obj.getName();
		sepVM.setDirty(Wheeled_Vehicle_vm_clone);
		syncParamArray(arsepVM_WheeledWheels2Param);
	}

	$sepVM_DataClassMenu[%name].setText(%obj.getName());
	%newObj.setFileName("art/datablocks/vehicles/devonly/tmpData.cs");
	sepVM_TempDatablocks.add(%newObj);
}
//------------------------------------------------------------------------------

//==============================================================================
//%field,%value,%ctrl,%array,%arg1,%arg2
function sepVM::cleanWheeledData( %this,%obj ) {
	logd("sepVM::cleanWheeledData( %this,%obj )",%obj);
	Lab_PM.removeField(%obj,"tireData");
	Lab_PM.removeField(%obj,"springData");
	Lab_PM.removeField(%obj,"tireDataFront");
	Lab_PM.removeField(%obj,"tireDataRear");
	Lab_PM.removeField(%obj,"springDataFront");
	Lab_PM.removeField(%obj,"springDataRear");
}
//------------------------------------------------------------------------------