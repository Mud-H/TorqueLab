//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$TPG_FileFilter = "Lab Painter Layers|*.painter.cs";
$TPG_SaveDialogMode = false;
$TPG_LayerPath = "tlab/terrainEditor/painterLayers";
//==============================================================================
// Set the layer group file folder base (Editor or Level)
function TPG::setLayersFolderBase(%this,%menu) {
	TPG.folderBase = %menu.getText();
}
//------------------------------------------------------------------------------

//==============================================================================
// Save painter layers set
//==============================================================================
//==============================================================================
// Save all the layers to a file
function TPG::quickSaveLayerGroup(%this,%saveLoadedGroup) {
	%layersPath = "tlab/terrainEditor/painterLayers/";
	%fileName = TPG_Window-->groupName.getText();
	%folders = TPG_Window-->groupFolders.getText();

	if (%fileName $= "")
		%fileName = "Default";

	%saveTmp = %layersPath@%folders@"/"@%fileName@".painter.cs";
	%saveTo = strreplace(%saveTmp,"//","/");
	%this.saveLayerGroupHandler(%saveTo);
}
//------------------------------------------------------------------------------
//==============================================================================
// Save all the layers to a file
function TPG::saveLayerGroup(%this) {
	%baseFile = $TPG_LayerPath @"/default.painter.cs";
	getSaveFilename($TPG_FileFilter, "TPG.saveLayerGroupHandler", %baseFile);
}
//------------------------------------------------------------------------------
//==============================================================================
// Callback for the saved file
function TPG::saveLayerGroupHandler( %this,%filename ) {
	%filename = makeRelativePath( %filename, getMainDotCsDir() );

	if(strStr(%filename, ".") == -1)
		%filename = %filename @ ".painter.cs";

	%clone = TPG_LayerGroup.deepClone();
	delObj("TPG_LayerGroup_Saved");
	%clone.setName("TPG_LayerGroup_Saved");
	// Create a file object for writing
	%fileWrite = new FileObject();
	%fileWrite.OpenForWrite(%filename);
	%fileWrite.writeObject(%clone);
	%fileWrite.close();
	%fileWrite.delete();
	info("Terrain painter layers save to file:",%filename);

	if (TPG_Explorer.isAwake())
		TPG.getSavedGroupsData();
}
//------------------------------------------------------------------------------

//==============================================================================
// Load painter layer set
//==============================================================================

//==============================================================================
// Load all the layers saved in a file
function TPG::loadLayerGroup(%this) {
	%baseFile = $TPG_LayerPath @"/default.painter.cs";
	getLoadFilename($TPG_FilerFilter, "TPG.loadLayerGroupHandler",%baseFile);
}
//------------------------------------------------------------------------------
//==============================================================================
// Callback for file loader
function TPG::loadLayerGroupHandler(%this,%file) {
	if ( !isScriptFile( %file ) ) {
		warnLog("loadLayerGroupHandler got invalid file:",%file);
		return;
	}

	delObj(TPG_LayerGroup_Saved);
	%file = expandFilename(%file);
	exec(%file);

	if (!isObject(TPG_LayerGroup_Saved)) {
		warnLog("Invalid layer group file. Can't find the object TPG_LayerGroup_Saved");
		return;
	}

	%file = makeRelativePath(%file);
	%fileName = strreplace(fileBase(%file),".painter","");
	%path = filePath(%file);
	%folders = strreplace(%path,$TPG_LayerPath,"");

	if (getSubStr(%folders,0,1) $= "/")
		%folders = getSubStr(%folders,1);

	TPG_Window-->groupFolders.setText(%folders);
	TPG_Window-->groupName.setText(%fileName);
	TPG.loadedGroupFile = %file;
	TPG_Window-->saveGroupButton.active = 1;

	//if (!isObject(TPG_LayerGroup))
	//new SimGroup( TPG_LayerGroup );
	//TerrainPaintGeneratorGui.deleteLayerGroup();
	if ( !$TPG_AppendLoadedLayers) {
		//TerrainPaintGeneratorGui.deleteLayerGroup();
		TPG_LayerGroup.deleteAllObjects();
	} else {
		TPG_Window-->saveGroupButton.active = false;
	}

	foreach(%layer in TPG_LayerGroup_Saved) {
		%addLayers = strAddWord(%addLayers,%layer.getId());
	}

	foreach$(%layer in %addLayers) {
		TPG_LayerGroup.add(%layer);
	}

	TPG_LayerGroup.dumpData();
	delObj(TPG_LayerGroup_Saved);
	%mats = ETerrainEditor.getMaterials();
	TPG_StackLayers.clear();

	foreach(%layer in TPG_LayerGroup) {
		%layer.matInternalName = strreplace(%layer.matInternalName,"*","");

		if ($CreateMissing) {
			if (recordFind(%mats,%layer.matInternalName) $= "-1") {
				%result = TPG.addTerrainMaterialLayer(%layer.matInternalName);
				%mats = ETerrainEditor.getMaterials();
			}
		}

		%layer.pill = "";
		TerrainPaintGeneratorGui.addLayerPill(%layer,true);
	}
}
//------------------------------------------------------------------------------
