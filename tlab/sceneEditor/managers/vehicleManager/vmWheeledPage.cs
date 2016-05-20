//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$sepVM_WheeledBook_PageId = 0;
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function sepVM::initWheeledPage( %this ) {
	if (!$sepVM_DataInitDone) {
		%this.updateWheeledDatablocks();
		%this.updateWheeledSystems();
	}

	sepVM_WheeledBook.selectPage($sepVM_WheeledBook_PageId);
}
//------------------------------------------------------------------------------
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function sepVM_WheeledBook::onTabSelected(  %this,%text,%index ) {
	$sepVM_WheeledBook_PageId = %index;
}
//------------------------------------------------------------------------------
//==============================================================================
// WheeledVehicleData (+ TireData + SpringSata) Setup
//==============================================================================
$sepVM_DataClassMenu["Wheeled_Vehicle_vm_clone"] = seVM_WheeledDataMenu;
$sepVM_DataClassMenu["Wheeled_Tire1_vm_clone"] = seVM_WheeledDataMenu_Tire1;
$sepVM_DataClassMenu["Wheeled_Tire2_vm_clone"] = seVM_WheeledDataMenu_Tire2;
$sepVM_DataClassMenu["Wheeled_Spring1_vm_clone"] = seVM_WheeledDataMenu_Spring1;
$sepVM_DataClassMenu["Wheeled_Spring2_vm_clone"] = seVM_WheeledDataMenu_Spring2;
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function sepVM::updateWheeledDatablocks( %this ) {
	seVM_WheeledDataMenu.clear();
	seVM_WheeledDataMenu_Tire1.clear();
	seVM_WheeledDataMenu_Tire2.clear();
	seVM_WheeledDataMenu_Spring1.clear();
	seVM_WheeledDataMenu_Spring2.clear();
	seVM_CloneMenu_Wheeled_Vehicle.clear();
	seVM_CloneMenu_Wheeled_Vehicle.setText("Select datablock to clone from");

	foreach( %data in DataBlockGroup ) {
		%class = %data.getClassName();

		switch$(%class) {
		case "WheeledVehicleData":
			seVM_WheeledDataMenu.add(%data.getName(),%data.getId());
			seVM_CloneMenu_Wheeled_Vehicle.add(%data.getName(),%data.getId());

		case "WheeledVehicleTire":
			seVM_WheeledDataMenu_Tire1.add(%data.getName(),%data.getId());
			seVM_WheeledDataMenu_Tire2.add(%data.getName(),%data.getId());

		case "WheeledVehicleSpring":
			seVM_WheeledDataMenu_Spring1.add(%data.getName(),%data.getId());
			seVM_WheeledDataMenu_Spring2.add(%data.getName(),%data.getId());
		}
	}

	seVM_WheeledDataMenu.setSelected(seVM_WheeledDataMenu.findText(sepVM.WheeledVehicle),true);
	seVM_WheeledDataMenu_Tire1.setSelected(seVM_WheeledDataMenu_Tire1.findText(sepVM.WheeledTire1),true);
	seVM_WheeledDataMenu_Tire2.setSelected(seVM_WheeledDataMenu_Tire2.findText(sepVM.WheeledTire2),true);
	seVM_WheeledDataMenu_Spring1.setSelected(seVM_WheeledDataMenu_Spring1.findText(sepVM.WheeledSpring1),true);
	seVM_WheeledDataMenu_Spring2.setSelected(seVM_WheeledDataMenu_Spring2.findText(sepVM.WheeledSpring2),true);
}
//------------------------------------------------------------------------------
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function sepVM_WheeledDataMenu::onSelect( %this,%id,%text ) {
	%type = getWord(%this.internalName,0);
	%typeId =  getWord(%this.internalName,1);
	%data = %id.getName();
	sepVM.selectWheeledData(%data,%type,%typeId);
}
//------------------------------------------------------------------------------
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function sepVM::selectWheeledData( %this,%data,%type,%typeId ) {
	devLog("sepVM selectWheeledData",%id,%data,"Type",%type,"TypeId",%typeId);
//	eval("sepVM.Wheeled"@%field@" = %data;");
	%field = %type@%typeId;
	sepVM.cloneWheeledDatablock("Wheeled",%field,%data);
	sepVM.selectedDatablock = %data;

	if (%type $= "Vehicle") {
		//syncParamArray(sepVM.wheeledParamVehicle);
		%useFrontAndRear = false;
		%system = 1;
		%posId["Front"] = 1;
		%posId["Rear"] = 2;
		devLog("Looking for tire:",%data.tireData, "Sprng:",%data.springData);

		foreach$(%dataType in "tire spring") {
			eval("%checkData = %data."@%dataType@"Data;");
			eval("%checkData = %data."@%dataType@"Data;");
			devLog("Test data:",%dataType,"CheckObj",%checkData);

			if (!isObject(%checkData)) {
				foreach$(%pos in "Front Rear") {
					eval("%checkData1 = %data."@%dataType@"Data"@%pos@";");
					//eval("%checkData1 = "@%checkData@%pos@";");
					devLog("Test data:",%dataType,"Pos",%pos,"CheckObj",%checkData1);

					if (!isObject(%checkData1)) {
						eval("%checkData1 = %data."@%dataType@"Data"@%posId[%pos]@";");
						devLog("Test data:",%dataType,"PosID",%posId[%pos],"CheckObj",%checkData1);
					}

					if (!isObject(%checkData1)) {
						%fail = strAddWord(%fail,%dataType@"_"@%pos);
					} else {
						%id = %posId[%pos];
						sepVM.cloneWheeledDatablock("Wheeled",%dataType@%id,%checkData1);
						//eval("sepVM.Wheeled"@%dataType@%id@" = %checkData1;");
						eval("seVM_WheeledDataMenu_"@%dataType@%id@".setText(%checkData1);");
						devlog("sepVM.Wheeled"@%dataType@%id,"Set as:",%checkData1);
						%useFrontAndRear = true;
					}
				}
			} else {
				sepVM.cloneWheeledDatablock("Wheeled",%dataType@"1",%checkData);
				//	eval("sepVM.Wheeled"@%dataType@"1 = %checkData;");
				devlog("sepVM.Wheeled"@%dataType@"1","Set as:",%checkData);
			}
		}

		//syncParamArray(sepVM.wheeledParamWheel1);
		if (%useFrontAndRear) {
			%system = 0;
			//	syncParamArray(sepVM.wheeledParamWheel2);
		}

		seVM_WheeledSystemMenu.setSelected(%system);
	}

	/*else if (%typeId $= "1")
		sepVM.cloneDatablock("Wheeled",%dataType@"1",%checkData);
	else if (%typeId $= "2")
		syncParamArray(sepVM.wheeledParamWheel2);*/
}
//------------------------------------------------------------------------------

//==============================================================================
// WheeledData System Menu
//==============================================================================
$sepVM_WheeledSystem_["rows",0] = 2;
$sepVM_WheeledSystem_["rows",1] = 1;
$sepVM_WheeledSystem = 0;
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function sepVM::updateWheeledSystems( %this ) {
	seVM_WheeledSystemMenu.clear();
	seVM_WheeledSystemMenu.add("Front and rear data",0);
	seVM_WheeledSystemMenu.add("All wheels same data",1);
	seVM_WheeledSystemMenu.setSelected($sepVM_WheeledSystem);
}
//------------------------------------------------------------------------------

//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function seVM_WheeledSystemMenu::onSelect( %this,%id,%text ) {
	$sepVM_WheeledSystem = %id;
	%rows = $sepVM_WheeledSystem_["rows",%id];

	for(%i=1; %i<3; %i++) {
		%show = false;

		if (%rows >= %i)
			%show = true;

		eval("sepVM_WheeledPage-->WheelAndSpring"@%i@".visible = %show;");
	}
}
//------------------------------------------------------------------------------
