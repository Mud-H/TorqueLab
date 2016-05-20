//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function sepVM::initPresetsData( %this ) {
	sepVM_ExportPresetName.setText("[Preset file name]");
	%this.getPresetFiles();
}
//------------------------------------------------------------------------------
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function sepVM::exportData( %this,%dataType ) {
	%dataMain = Wheeled_Vehicle_vm_clone;
	%name = sepVM_ExportPresetName.getText();

	if (%name $= "[Preset file name]") {
		%name = "Unnamed";
		sepVM_ExportPresetName.setText(%name);
	}

	%fileWrite = getFileWriteObj("tlab/sceneEditor/managers/vehicleManager/presets/"@%name@".preset.txt");

	for(%i = 0; %i<arsepVM_WheeledVehicleParam.count(); %i++) {
		%field = arsepVM_WheeledVehicleParam.getKey(%i);

		if (%field $= "") {
			warnLog("Trying to export empty field for array:",arsepVM_WheeledVehicleParam.getName(),arsepVM_WheeledVehicleParam.getId(),"Field index=",%i);
			continue;
		}

		%value = %dataMain.getFieldValue(%field);
		//Change superClass name to not make preset Obj call superclass functions
		%fileWrite.writeline(%field TAB %value);
	}

	closeFileObj(%fileWrite);
	%this.getPresetFiles();
}
//------------------------------------------------------------------------------
//sepVM_WheeledPresetFileTree.filter = "*.preset.txt";
//sepVM_WheeledPresetFileTree.setSelectedPath("tlab/sceneEditor/vehicleManager/presets/");
//sepVM_WheeledPresetFileTree.reload();
//==============================================================================
// sepVM.importData
function sepVM::importData( %this,%dataType ) {
	%dataMain = Wheeled_Vehicle_vm_clone;
	%file = sepVM.selectedPresetFile;
	sepVM_ExportPresetName.setText(sepVM.selectedPresetName);

	if (!isFile(%file)) {
		warnLog("Invalid preset file to import data from:",%file);
		return;
	}

	%fileRead = getFileReadObj(%file);

	while( !%fileRead.isEOF() ) {
		%line = %fileRead.readLine();
		%field = getField(%line,0);
		%value = getField(%line,1);
		devLog("Importing field,",%field,"With Value",%value,"Current is",%dataMain.getFieldValue(%field));
		%dataMain.setFieldValue(%field,%value);
	}

	closeFileObj(%fileRead);
}
//------------------------------------------------------------------------------
//sepVM.getPresetFiles
//==============================================================================
function sepVM::getPresetFiles(%this) {
	%basePath = "tlab/sceneEditor/managers/vehicleManager/presets";
	%searchFolder = %basePath @ "/*.preset.txt";
	%folderColor = "\c2";
	sepVM_PresetTree.clear();
	sepVM.selectedPresetFile = "";

	//Now go through each files again to add a brush with latest items
	for(%file = findFirstFile(%searchFolder); %file !$= ""; %file = findNextFile(%searchFolder)) {
		%fileName = fileBase(fileBase(%file));
		%path = filePath(%file);
		%relPath = strreplace(%path,%basePath@"/","");
		%relPath = strreplace(%relPath,%basePath,"");
		%folderOrder = strreplace(%relPath,"/","\t");
		%targetId = 0;

		for(%i = 0; %i < getFieldCount(%folderOrder); %i++) {
			%folder = trim(getField(%folderOrder,%i));
			%childId = sepVM_PresetTree.findChildItemByName(%targetId,%folderColor@%folder);

			if (%childId <= 0) {
				%childId = sepVM_PresetTree.insertItem(%targetId,%folderColor@%folder,"Folder","",0,0);
			}

			%targetId = %childId;
		}

		%itemId = sepVM_PresetTree.insertItem(%targetId,%fileName,%file,"",0,0);
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function sepVM_PresetTree::onSelect(%this,%itemId) {
	%text = %this.getItemText(%itemId);
	%value = %this.getItemValue(%itemId);

	if (%value $= "Folder")
		return;

	sepVM.selectedPresetFile = %value;
	sepVM.selectedPresetName = %text;
	devLog("Selected file = ",%text,"OR",%value);
}
//------------------------------------------------------------------------------
//==============================================================================
function sepVM_PresetTree::onUnselect(%this,%itemId) {
}
//------------------------------------------------------------------------------