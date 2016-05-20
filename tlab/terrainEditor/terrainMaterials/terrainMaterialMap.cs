//==============================================================================
// TorqueLab -> Fonts Setup
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
function TerrainMaterialDlg::updateMaterialMapping( %this, %terrMat ) {
	if (%terrMat $= "")
		%terrMat = %this.activeMat;

	%diffuse = %terrMat.diffuseMap;
	%texName = fileBase(%diffuse);
	%mapMat = getMaterialMapping(%texName);

	if (isObject(%mapMat)) {
		TerrainMatDlg_MaterialInfo-->matName.text = %mapMat;
		TerrainMatDlg_MappedInspector.inspect(%mapMat);
		TerrainMatDlg_MaterialInfo-->unmappedCont.visible = false;
		TerrainMatDlg_MaterialInfo-->inspectorCont.visible = true;
		TerrainMatDlg_MaterialInfo-->inspectorCont.extent = TerrainMatDlg_MappedInspector.extent;
		TerrainMaterialDlg.mappedMat = %mapMat;
	} else {
		TerrainMatDlg_MaterialInfo-->matName.text = "";
		TerrainMatDlg_MappedInspector.inspect("");
		TerrainMatDlg_MaterialInfo-->inspectorCont.visible = false;
		TerrainMatDlg_MaterialInfo-->unmappedCont.visible = true;
		TerrainMaterialDlg.mappedMat = "";
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainMaterialDlg::saveMappedMat( %this, %terrMat ) {
	%mapMat = %this.mappedMat;

	if (!isObject(%mapMat))
		return;

	TerrainMatDlg_MappedInspector.apply();
	devLog(%mapMat.getName(),"Material is dirty:",Lab_PM.isDirty(%mapMat));

	if (!TerrainMatDlg_MappedInspector.isDirty) {
		devLog("Nothing to save");
		return;
	}

	Lab_PM.saveDirtyObject(%mapMat);
}
//------------------------------------------------------------------------------

//==============================================================================
function TerrainMaterialDlg::deleteMappedMat( %this, %terrMat ) {
	%mapMat = %this.mappedMat;

	if (!isObject(%mapMat))
		return;

	//%mapMat.delete();
	%this.removeMappedMaterial(%mapMat);
}
//------------------------------------------------------------------------------
function TerrainMatDlg_MappedInspector::onInspectorFieldModified( %this, %object, %fieldName, %oldValue, %newValue ) {
	Lab_PM.setDirty( %object );
	TerrainMatDlg_MappedInspector.isDirty = true;
}
//------------------------------------------------------------------------------
function TerrainMatDlg_MappedInspector::inspect( %this, %object, %fieldName, %oldValue, %newValue ) {
	%this.isDirty = false;
	Parent::inspect( %this, %object );
}

//==============================================================================
//TerrainMaterialDlg.createMappedMaterial();
function TerrainMaterialDlg::createMappedMaterial( %this ) {
	%terrMat = %this.activeMat;
	%file = %terrMat.getFilename();

	if (!isFile(%file)) {
		warnLog("The current material doesn't have a valid file:",%file," A valid file is needed to create mapped material.");
		return;
	}

	%diffuse = %terrMat.diffuseMap;
	%texName = fileBase(%diffuse);
	%mapMat = getMaterialMapping(%texName);

	if (isObject(%mapMat)) {
		warnLog("There's already a valid mapped material linked to this TerrainMaterial:",%mapMat);
		return;
	}

	%newMapMat = singleton Material("GMat_"@%texName) {
		mapTo = %texName;
		footstepSoundId = 0;
		terrainMaterials = "1";
		ShowDust = "1";
		showFootprints = "1";
		materialTag0 = "Terrain";
		impactSoundId = 0;
	};
	%startAtLine = 1; //Start writing obj at this line
	%newMapMat.setFilename(%file);
	%fileObj = getFileReadObj(%file);

	while( !%fileObj.isEOF() ) {
		%line = %fileObj.readLine();
		%lines[%lineId++] = %line;

		if (strFind(%line,"//Maps")) {
			%startAtLine = %lineId+1;
		}
	}

	closeFileObj(%fileObj);
	%writeFile = %file;//strReplace(%file,".cs","_test.cs");
	%fileWrite = getFileWriteObj(%writeFile);
	//%fileWrite.writeLine("//==============================================================================");
	//%fileWrite.writeLine("// Generated from TerrainMaterialDlg");
	//%fileWrite.writeLine("//------------------------------------------------------------------------------");
	%newline = 1;

	for(%i = 1; %i<=%lineId; %i++) {
		if (%startAtLine $= %i) {
			%fileWrite.writeLine("singleton Material(GMat_"@%texName@")");
			%fileWrite.writeLine("{");
			%fileWrite.writeLine("	mapTo = \""@%texName@"\";");
			%fileWrite.writeLine("	footstepSoundId = 0;");
			%fileWrite.writeLine("	terrainMaterials = \"1\";");
			%fileWrite.writeLine("	ShowDust = \"1\";");
			%fileWrite.writeLine("	showFootprints = \"1\";");
			%fileWrite.writeLine("	materialTag0 = \"Terrain\";");
			%fileWrite.writeLine("	impactSoundId = 0;");
			%fileWrite.writeLine("};");
			//%fileWrite.writeLine("");
		}

		%fileWrite.writeLine(%lines[%i]);
	}

	closeFileObj(%fileWrite);
	%this.updateMaterialMapping();
}
//------------------------------------------------------------------------------
//==============================================================================
//TerrainMaterialDlg.removeMappedMaterial("GMat_gvSandGrain01_c");
function TerrainMaterialDlg::removeMappedMaterial( %this,%mapMat ) {
	if (!isObject(%mapMat)) {
		warnLog("Invalid mapped material to remove:",%mapMat);
		return;
	}

	%file = %mapMat.getFilename();

	if (!isFile(%file)) {
		warnLog("The current material doesn't have a valid file:",%file," A valid file is needed to create mapped material.");
		return;
	}

	%file = %mapMat.getFilename();
	%name = %mapMat.getName();
	%fileObj = getFileReadObj(%file);

	while( !%fileObj.isEOF() ) {
		%line = %fileObj.readLine();

		if (strFind(%line,%name)) {
			%removeMode = true;
		}

		if (!%removeMode) {
			%lines[%lineId++] = %line;
		}

		if (%removeMode && strFind(%line,"};"))
			%removeMode =  false;
	}

	closeFileObj(%fileObj);
	%writeFile = %file;//strReplace(%file,".cs","_test.cs");
	//%writeFile = strReplace(%file,".cs","_del.cs");
	%fileWrite = getFileWriteObj(%writeFile);

	for(%i = 1; %i<=%lineId; %i++) {
		%fileWrite.writeLine(%lines[%i]);
	}

	closeFileObj(%fileWrite);
	delObj(%mapMat);
	%this.updateMaterialMapping();
}
//------------------------------------------------------------------------------
