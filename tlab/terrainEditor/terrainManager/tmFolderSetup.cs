//==============================================================================
// TorqueLab -> TerrainMaterialManager
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
function TMG::getActiveFolders(%this) {
	%dataFolder = TMG.activeTerrain.dataFolder;

	if (%dataFolder $= "")
		%dataFolder = MissionGroup.dataFolder;

	if (%dataFolder $= "")
		%dataFolder = filePath(TMG.activeTerrain.terrainFile);

	TMG.setFolder("data",validatePath(%dataFolder));
	%sourceFolder = TMG.activeTerrain.sourceFolder;

	if (%sourceFolder $= "")
		%sourceFolder = MissionGroup.sourceFolder;

	if (%sourceFolder $= "")
		%sourceFolder = %dataFolder@"/Source";

	TMG.setFolder("source",validatePath(%sourceFolder));
	%targetFolder = TMG.activeTerrain.targetFolder;

	if (%targetFolder $= "")
		%targetFolder = MissionGroup.targetFolder;

	if (%targetFolder $= "")
		%targetFolder = %dataFolder@"/Target";

	TMG.setFolder("target",validatePath(%targetFolder));
	%terrainFolder = validatePath(TMG.dataFolder@"/terData/"@TMG.activeTerrain.getName(),true);
	TMG.terrainFolder = %terrainFolder;
}
//------------------------------------------------------------------------------
//==============================================================================
function TMG::setFolder(%this,%type,%folder,%relativeToData,%onlyTMG) {
	if (%folder $= "")
		return;

	if (%relativeToData) {
		%subFolder = %folder;
		%folder = TMG.dataFolder@"/"@%subFolder;
		%folder = strreplace(%folder,"//","/");
		%subField = %type@"SubFolder";
		eval("TMG."@%subField@" = %subFolder;");
	}

	%folder = validatePath(%folder,true);
	%field = %type@"Folder";
	eval("%currentFolder = TMG."@%field@";");

	if (%currentFolder !$= %folder)
		%folderChanged = true;

	eval("TMG."@%field@" = %folder;");

	if (%type !$= "data") {
		%subText = %folder;

		if (strFind(%folder,TMG.dataFolder)) {
			%relativeFolder = strreplace(%folder,TMG.dataFolder,"");

			if (getSubStr(%relativeFolder,0,1) $= "/")
				%relativeFolder = getSubStr(%relativeFolder,1);

			%subText = %relativeFolder;
		}

		eval("%editCtrl = TerrainManagerGui-->"@%type@"_sideFolder;");
		%editCtrl.setText(%subText);
		eval("%subEdit = TerrainManagerGui-->"@%type@"_FolderEdit;");
		%subEdit.setText(%subText);
		eval("%genEdit = TMG_GeneralFolderSetup-->"@%type@"_FolderEdit;");
		%genEdit.setText(%subText);
	} else {
		TerrainManagerGui-->dataFolder.setText(TMG.dataFolder);
		TerrainManagerGui-->sideDataFolderText.text = TMG.dataFolder;
		TMG_GeneralFolderSetup-->dataFolder.setText(TMG.dataFolder);
		%this.setFolder("source",TMG.sourceSubFolder,true);
		%this.setFolder("target",TMG.targetSubFolder,true);
	}

	if (!%onlyTMG) {
		MissionGroup.setFieldValue(%field,%folder);
	}

	if (isObject(TMG.activeTerrain)) {
		if (TMG.activeTerrain.storeFolders)
			TMG.activeTerrain.setFieldValue(%field,%folder);
	}

	//Update map layers data if source changed
	if (%type $= "Source" && %folderChanged)
		TMG.updateAllMaterialLayersMenu();
}
//------------------------------------------------------------------------------
//==============================================================================
// Select TerrainManager Data folder
//==============================================================================
//==============================================================================
function TMG::selectDataFolder( %this,%type) {
	if (%type $= "")
		%type = "data";

	eval("%currentFolder = TMG."@%type@"Folder;");

	if (!isDirectory(%currentFolder))
		%currentFolder = MissionGroup.getFilename();

	devLog("Current Dir:",%currentFolder);
	getFolderName("*.*","TMG.setDataFolder",%currentFolder,"Select Export Folder",%type);
}
//------------------------------------------------------------------------------
//==============================================================================
function TMG::setDataFolder( %this, %path,%type ) {
	%terObj = %this.activeTerrain;

	if (!isObject(%terObj)) {
		warnlog("Not active terrain detected. Please select one before setting data folder:",%terObj);
		return;
	}

	%path =  makeRelativePath( %path, getMainDotCsDir() );
	//TerrainManagerGui-->dataFolder.setText(%path);
	%this.setFolder(%type,%path);
}
//------------------------------------------------------------------------------

//==============================================================================
// Data/Source/Target TextEdit validations
//==============================================================================

//==============================================================================
function RelativeFolderEdit::onValidate(%this) {
	%data = strreplace(%this.internalName,"_"," ");
	%type = getWord(%data,0);
	%path = TMG.fixEditPath(%this.getText());
	TMG.setFolder(%type,%path,true);
}
//------------------------------------------------------------------------------

//==============================================================================
function TMG_RelativeSourceFolderEdit::onValidate( %this ) {
	%path = TMG.fixEditPath(%this.getText());
	TMG.setFolder("source",%path,true);
}
//------------------------------------------------------------------------------
//==============================================================================
function TMG_RelativeTargetFolderEdit::onValidate( %this ) {
	%path = TMG.fixEditPath(%this.getText());
	TMG.setFolder("target",%path,true);
}
//------------------------------------------------------------------------------
//==============================================================================
function TMG::fixEditPath( %this,%path ) {
	%fixPath = %path;
	return %fixPath;
}
//------------------------------------------------------------------------------