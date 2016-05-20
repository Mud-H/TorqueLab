//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
//CREATE PREFAB FROM SELECTION
//==============================================================================
//==============================================================================
// New Create Prefab System using Lab
function Lab::CreatePrefab(%this,%file) {
	// Should this be protected or not?
	%autoMode = $SceneEd::AutoCreatePrefab;

	if (%file !$= "") {
		%saveFile = %file;
	} else if (%autoMode) {
		%saveFile = %this.GetAutoPrefabFile();
	} else if ( !$Pref::disableSaving && !isWebDemo() ) {
		%saveFile = %this.GetPrefabFile();
	}

	if (%saveFile $= "")
		return;

	EWorldEditor.makeSelectionPrefab( %saveFile );

	//Add new prefab to active group if exist (Must get previous id)
	if (isObject(SceneEd.ActiveGroup))
		SceneEd.ActiveGroup.add($ThisPrefab.getId()-1);

	SceneEditorTree.buildVisibleTree( true );
}
//------------------------------------------------------------------------------
//==============================================================================
// New Create Prefab System using Lab
function Lab::GetAutoPrefabFile() {
	%missionFolder = filePath(MissionGroup.getFilename());
	%firstObj = EWorldEditor.getSelectedObject(0);
	%mode = $SceneEd::AutoPrefabMode;

	if (%mode $= "")
		%mode = "Level";

	if (%firstObj.isMemberOfClass(TSStatic)) {
		%name = fileBase(%firstObj.shapeName);
	}

	if (%name $= "") {
		if (%firstObj.getName() !$="")
			%name = %firstObj.getName();
		else if (%firstObj.internalName !$="")
			%name = %firstObj.internalName;
		else
			%name = %firstObj;
	}

	switch$(%mode) {
	case "Level":
		%prefabPath = %missionFolder @"/prefabs/";
		%fileTmp = %prefabPath@"/"@%name@".prefab";

	case "Folder":
		%prefabPath = $SceneEd::AutoPrefabFolder;
		%fileTmp = %prefabPath@"/"@%name@".prefab";

	case "Object":
		%prefabPath = filePath(%firstObj.shapeName);
		%fileTmp = %prefabPath@"/"@%name@".prefab";
	}

	%file = strreplace(%fileTmp,"//","/");

	while (isFile(%file)) {
		%uniqueName = %name@"_"@%inc++;
		%fileTmp = %prefabPath@"/"@%uniqueName@".prefab";
		%file = strreplace(%fileTmp,"//","/");
	}

	devLog("Prefab stored to:",%file,"Using mode:",%mode);
	return %file;
}
//------------------------------------------------------------------------------
//==============================================================================
// New Create Prefab System using Lab
function Lab::GetPrefabFile() {
	%dlg = new SaveFileDialog() {
		Filters        = "Prefab Files (*.prefab)|*.prefab|";
		DefaultPath    = $Pref::WorldEditor::LastPath;
		DefaultFile    = "";
		ChangePath     = false;
		OverwritePrompt   = true;
	};
	%ret = %dlg.Execute();

	if ( %ret ) {
		$Pref::WorldEditor::LastPath = filePath( %dlg.FileName );
		%saveFile = %dlg.FileName;
	}

	if( fileExt( %saveFile ) !$= ".prefab" )
		%saveFile = %saveFile @ ".prefab";

	%dlg.delete();

	if ( !%ret )
		return "";

	return %saveFile;
}
//------------------------------------------------------------------------------

//==============================================================================
//EXPLODED SELECTED PREFAB
//==============================================================================
//==============================================================================
// New Create Prefab System using Lab
function Lab::ExplodePrefab(%this) {
	//echo( "EditorExplodePrefab()" );
	EWorldEditor.explodeSelectedPrefab();
	SceneEditorTree.buildVisibleTree( true );
}
//------------------------------------------------------------------------------


//==============================================================================
// Default T3D Prefab function for backward compatibility
//==============================================================================
//==============================================================================
function EditorMakePrefab() {
	Lab.CreatePrefab();
}

function EditorExplodePrefab() {
	Lab.ExplodePrefab();
}
//------------------------------------------------------------------------------
