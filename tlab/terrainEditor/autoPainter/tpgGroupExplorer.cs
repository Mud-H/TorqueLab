//==============================================================================
// TorqueLab -> Terrain Paint Generator System
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
function TPG_Explorer::onWake(%this) {
	TPG.getSavedGroupsData();
}
//------------------------------------------------------------------------------

//==============================================================================
function TPG_Explorer::loadSelectedLayerGroup(%this) {
	%file = TPG_Explorer.selectedFile;

	if (!isFile(%file)) {
		warnLog("Invalid file selected:",%file);
		return;
	}

	TPG.loadLayerGroupHandler(%file);
	TPG_ExplorerInfo-->groupName.text = filebase(%file);
}
//------------------------------------------------------------------------------
//==============================================================================
function TPG_Explorer::toggleGroupInfo(%this,%checkbox) {
	%showInfo = %checkbox.isStateOn();
	TPG_ExplorerInfo.visible = %showInfo;
}
//------------------------------------------------------------------------------

//==============================================================================
function TPG::getSavedGroupsData(%this) {
	%basePath = "tlab/terrainEditor/painterLayers";
	%searchFolder = %basePath @ "/*.cs";
	%folderColor = "\c2";
	TPG_SavedGroupTree.clear();
	TPG_Explorer.selectedFile = "";

	//Now go through each files again to add a brush with latest items
	for(%file = findFirstFile(%searchFolder); %file !$= ""; %file = findNextFile(%searchFolder)) {
		%fileName = fileBase(%file);
		%fileName = strreplace(%fileName,".painter","");
		%path = filePath(%file);
		%relPath = strreplace(%path,%basePath@"/","");
		%relPath = strreplace(%relPath,%basePath,"");
		%folderOrder = strreplace(%relPath,"/","\t");
		%targetId = 0;

		for(%i = 0; %i < getFieldCount(%folderOrder); %i++) {
			%folder = trim(getField(%folderOrder,%i));
			%childId = TPG_SavedGroupTree.findChildItemByName(%targetId,%folderColor@%folder);

			if (%childId <= 0) {
				%childId = TPG_SavedGroupTree.insertItem(%targetId,%folderColor@%folder,"Folder","",0,0);
			}

			%targetId = %childId;
		}

		%itemId = TPG_SavedGroupTree.insertItem(%targetId,%fileName,%file,"",0,0);
	}
}
//------------------------------------------------------------------------------


//==============================================================================
function TPG::selectSavedGroupFile(%this,%file) {
	TPG_Explorer.selectedFile = %file;
	delObj(TPG_LayerGroup_Saved);
	%file = expandFilename(%file);
	exec(%file);

	if (!isObject(TPG_LayerGroup_Saved)) {
		warnLog("Invalid layer group file. Can't find the object TPG_LayerGroup_Saved");
		return;
	}

	%count = TPG_LayerGroup_Saved.getCount();
	TPG_ExplorerInfo-->groupName.text = filebase(%file)@" (\c2"@%count@"\c0)";
	%textList = TPG_ExplorerInfo-->selectedLayers;
	%textList.clear();

	foreach(%layer in TPG_LayerGroup_Saved) {
		%heightMax = %layer.heightMax;

		if (%heightMax >= $TPG_DefaultHeightMax)
			%heightMax = $TPG_DefaultHeightMax;

		%heightTab = mCeil(%layer.heightMin) @"/"@mCeil(%heightMax);
		%slopeTab = mCeil(%layer.slopeMin) @"/"@mCeil(%layer.slopeMax);
		%textList.addRow(%id++,%layer.matInternalName TAB "\c2"@%heightTab TAB "\c3"@%slopeTab);
	}
}
//------------------------------------------------------------------------------
//==============================================================================
// Terrain Paint Generator - Layer Validation Functions
//==============================================================================

//==============================================================================
function TPG_SavedGroupTree::onSelect(%this,%itemId) {
	%text = TPG_SavedGroupTree.getItemText(%itemId);
	%value = %this.getItemValue(%itemId);
	%sibling = %this.getNextSibling(%itemId);

	if (%value $= "Folder")
		return;

	if (!isFile(%value))
		return;

	TPG.selectSavedGroupFile(%value);
}
//------------------------------------------------------------------------------

//==============================================================================
function TPG_SavedGroupTree::onMouseUp(%this,%itemId,%clicks) {
	if (%clicks > 1)
		TPG_Explorer.loadSelectedLayerGroup();
}
//------------------------------------------------------------------------------
