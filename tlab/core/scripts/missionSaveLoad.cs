//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// Mission File Saving System Functions
//==============================================================================

//==============================================================================
// Menu Save and SaveAs function
function Lab::SaveCurrentMission(%this,%saveAs) {
	%file = MissionGroup.getFilename();
	if (!isFile(%file))
	   %saveAs = true;
   else if (getSubStr(%file,0,4) $= "tlab")
      %saveAs = true;
      
   /* Old check system to avoid saving into unwanted directory, seemed not flexible
   // for generic projects, simply check if level file is not in tlab/ folder instead
	%start = getSubStr(%file,0,6);
   %levelRoot = $Lab::LevelRoot $= "" ? "levels" : $Lab::LevelRoot;
  %startRoot = getSubStr(%levelRoot,0,6);
	if (%start !$= %startRoot)
		%saveAs = true;
		*/

	if(!$Pref::disableSaving) {
		if(EditorGui.saveAs || %saveAs)
			%this.SaveMissionAs();
		else
			%this.SaveMission();
	} else {
		%this.SaveMissionDisableWarning();
	}
}
//------------------------------------------------------------------------------
//==============================================================================
//Generic Save Mission Call, make sure it's possible to save
function Lab::SaveMission(%this,%backup) {
	// just save the mission without renaming it
	%missionFile = MissionGroup.getFileName();	
	
		
	// first check for dirty and read-only files:
	if((EWorldEditor.isDirty || ETerrainEditor.isMissionDirty) && !isWriteableFileName(%missionFile)) {
		LabMsgOkCancel("Error", "Mission file \""@ %missionFile @ "\" is read-only.  Continue?", "Ok", "Stop");
		return false;
	}

	if(ETerrainEditor.isDirty) {
		// Find all of the terrain files
		initContainerTypeSearch($TypeMasks::TerrainObjectType);

		while ((%terrainObject = containerSearchNext()) != 0) {
			if (!isWriteableFileName(%terrainObject.terrainFile)) {
				if (LabMsgOkCancel("Error", "Terrain file \""@ %terrainObject.terrainFile @ "\" is read-only.  Continue?", "Ok", "Stop") == $MROk)
					continue;
				else
					return false;
			}
		}
	}

	// now write the terrain and mission files out:
	Lab.LoadActionProgress("Saving the mission","","The mission is saving, it won't take long","500");
	
	
	if(EWorldEditor.isDirty || ETerrainEditor.isMissionDirty)
		MissionGroup.save(%missionFile);
   else
      info("Nothing dirty to be saved for mission:",fileBase(%missionFile));
	
	//Save the dirty terrains (.ter)
	if(ETerrainEditor.isDirty) {
		// Find all of the terrain files
		initContainerTypeSearch($TypeMasks::TerrainObjectType);

		while ((%terrainObject = containerSearchNext()) != 0)
			%terrainObject.save(%terrainObject.terrainFile);
	}
	if (isObject(ETerrainPersistMan))
		ETerrainPersistMan.saveDirty();

	// Give EditorPlugins a chance to save.
	for ( %i = 0; %i < EditorPluginSet.getCount(); %i++ ) {
		%obj = EditorPluginSet.getObject(%i);
		//if ( %obj.isDirty() )
		%obj.onSaveMission( $Server::MissionFile );
	}

	EditorClearDirty();
	EditorGui.saveAs = false;
	info("Mission saved to:",%missionFile);
	return true;
}
//------------------------------------------------------------------------------
//==============================================================================
//Generic Save Mission Call, make sure it's possible to save
function Lab::SaveMissionAs(%this) {
	if(!$Pref::disableSaving ) {
		// If we didn't get passed a new mission name then
		// prompt the user for one.
		%path = $Cfg_Common_General_levelsDirectory;
		%file = "";
		if (isFile(MissionGroup.getFileName()))
		{
		   %path =filePath(  MissionGroup.getFileName());
		   %file = MissionGroup.getFileName();
		}
		%path =filePath(  MissionGroup.getFileName());
		if (!isDirectory(%path))
		   %path = $Cfg_Common_General_levelsDirectory;
		if ( %missionName $= "" ) {
			%dlg = new SaveFileDialog() {
				Filters        = $Pref::WorldEditor::FileSpec;
				DefaultPath    = %path;
				DefaultFile = %file;
				ChangePath     = true;
				OverwritePrompt   = true;
			};
			%ret = %dlg.Execute();

			if(%ret) {
				// Immediately override/set the levelsDirectory
				$Cfg_Common_General_levelsDirectory = collapseFilename(filePath( %dlg.FileName ));				
				%missionName = %dlg.FileName;
			}

			%dlg.delete();

			if(! %ret)
				return;
		}

		if( fileExt( %missionName ) !$= ".mis" )
			%missionName = %missionName @ ".mis";

		EWorldEditor.isDirty = true;
		%saveMissionFile = $Server::MissionFile;
		$Server::MissionFile = %missionName;
		
//-------------------------------------------------------------------
// Only in SaveAs, seem to backup terrain files in case of failure to restore
// them. Need to examine if needed or not and make both method work the same
		%copyTerrainsFailed = false;
		// Rename all the terrain files.  Save all previous names so we can
		// reset them if saving fails.
		%newMissionName = fileBase(%missionName);
		%oldMissionName = fileBase(%saveMissionFile);
		initContainerTypeSearch( $TypeMasks::TerrainObjectType );
		%savedTerrNames = new ScriptObject();

		for( %i = 0;; %i ++ ) {
			%terrainObject = containerSearchNext();

			if( !%terrainObject )
				break;

			%savedTerrNames.array[ %i ] = %terrainObject.terrainFile;
			%terrainFilePath = makeRelativePath( filePath( %terrainObject.terrainFile ), getMainDotCsDir() );
			%terrainFileName = fileName( %terrainObject.terrainFile );

			// Workaround to have terrains created in an unsaved "New Level..." mission
			// moved to the correct place.

			if( EditorGui.saveAs && %terrainFilePath $= "tools/art/terrains" )
				%terrainFilePath = "art/terrains";

			// Try and follow the existing naming convention.
			// If we can't, use systematic terrain file names.
			if( strstr( %terrainFileName, %oldMissionName ) >= 0 )
				%terrainFileName = strreplace( %terrainFileName, %oldMissionName, %newMissionName );
			else
				%terrainFileName = %newMissionName @ "_" @ %i @ ".ter";

			%newTerrainFile = %terrainFilePath @ "/" @ %terrainFileName;

			if (!isWriteableFileName(%newTerrainFile)) {
				if (MessageBox("Error", "Terrain file \""@ %newTerrainFile @ "\" is read-only.  Continue?", "Ok", "Stop") == $MROk)
					continue;
				else {
					%copyTerrainsFailed = true;
					break;
				}
			}

			if( !%terrainObject.save( %newTerrainFile ) ) {
				error( "Failed to save '" @ %newTerrainFile @ "'" );
				%copyTerrainsFailed = true;
				break;
			}

			%terrainObject.terrainFile = %newTerrainFile;
		}

		ETerrainEditor.isDirty = false;
//-------------------------------------------------------------------
// If copy terrain failed or saveMission failed, restored original terrains
		// Save the mission.
		if(%copyTerrainsFailed || !%this.SaveMission()) {
			// It failed, so restore the mission and terrain filenames.
			$Server::MissionFile = %saveMissionFile;
			initContainerTypeSearch( $TypeMasks::TerrainObjectType );

			for( %i = 0;; %i ++ ) {
				%terrainObject = containerSearchNext();

				if( !%terrainObject )
					break;

				%terrainObject.terrainFile = %savedTerrNames.array[ %i ];
			}
		} else {
			%fixPath = makeRelativePath($Server::MissionFile);
			MissionGroup.setFilename(%fixPath);
		}

		%savedTerrNames.delete();
	} else {
		%this.SaveMissionDisableWarning();
	}
}
//------------------------------------------------------------------------------
//==============================================================================
//Generic Save Mission Call, make sure it's possible to save
function Lab::SaveMissionDisableWarning(%this) {
	GenericPromptDialog-->GenericPromptWindow.text = "Warning";
	GenericPromptDialog-->GenericPromptText.setText("Saving disabled in demo mode.");
	Canvas.pushDialog( GenericPromptDialog );
}
//------------------------------------------------------------------------------

//==============================================================================
// Create new mission functions
//==============================================================================
//==============================================================================
function Lab::CreateNewMission( %forceSave,%confirmed ) {
	logc("Lab::CreateNewMission( %forceSave,%confirmed )",%forceSave,%confirmed );

	if(isWebDemo())
		return;

	if ( EditorIsDirty() && !%confirmed) {
		error(knob);
		LabMsgYesNoCancel("Mission Modified", "Would you like to save changes to the current mission \"" @
								$Server::MissionFile @ "\" before creating a new mission?", "Lab.CreateNewMission(true,true);","Lab.CreateNewMission(false,true);");
		return;
	}

	if(%saveFirst)
		Lab.SaveMission();

	// Clear dirty flags first to avoid duplicate dialog box from EditorOpenMission()
	if( isObject( Editor ) ) {
		EditorClearDirty();
		Editor.getUndoManager().clearAll();
	}

	if( %file $= "" )
		%file = Lab.newLevelFile;

	if( !$missionRunning ) {
		activatePackage( "BootEditor" );
		StartLevel( %file );
	} else
		EditorOpenMission(%file);

	//EWorldEditor.isDirty = true;
	//ETerrainEditor.isDirty = true;
	EditorGui.saveAs = true;
}
//------------------------------------------------------------------------------
//==============================================================================
function EditorNewLevel( %file,%forceSave,%confirmed ) {
	logd("EditorNewLevel( %file,%forceSave,%confirmed )",%file,%forceSave,%confirmed );

	if(isWebDemo())
		return;

	%saveFirst = false;

	if (!%forceSave && %forceSave !$= "") {
		%saveFirst = false;
		%noConfirm =  true;
	} else if (%forceSave) {
		%saveFirst = true;
		%noConfirm =  true;
	}

	if ( EditorIsDirty() && !%confirmed) {
		error(knob);
		LabMsgYesNoCancel("Mission Modified", "Would you like to save changes to the current mission \"" @
								$Server::MissionFile @ "\" before creating a new mission?", "EditorNewLevel("@%file@",true,true)","EditorNewLevel("@%file@",false,true)");
		return;
	}

	if(%saveFirst)
		Lab.SaveMission();

	// Clear dirty flags first to avoid duplicate dialog box from EditorOpenMission()
	if( isObject( Editor ) ) {
		EditorClearDirty();
		Editor.getUndoManager().clearAll();
	}

	if( %file $= "" )
		%file = Lab.newLevelFile;

	if( !$missionRunning ) {
		activatePackage( "BootEditor" );
		StartLevel( %file );
	} else
		EditorOpenMission(%file);

	//EWorldEditor.isDirty = true;
	//ETerrainEditor.isDirty = true;
	EditorGui.saveAs = true;
}
//------------------------------------------------------------------------------

//==============================================================================
// Load a Mission in Editor (Not tested)
//==============================================================================
//==============================================================================
function EditorOpenMission(%filename) {
	if( EditorIsDirty() && !isWebDemo() ) {
		// "EditorSaveBeforeLoad();", "getLoadFilename(\"*.mis\", \"EditorDoLoadMission\");"
		if(LabMsgOkCancel("Mission Modified", "Would you like to save changes to the current mission \"" @
								$Server::MissionFile @ "\" before opening a new mission?", SaveDontSave, Question) == $MROk) {
			if(! Lab.SaveMission())
				return;
		}
	}

	if(%filename $= "") {
		%dlg = new OpenFileDialog() {
			Filters        = $Pref::WorldEditor::FileSpec;
			DefaultPath    = Lab.levelsDirectory;
			ChangePath     = false;
			MustExist      = true;
		};
		%ret = %dlg.Execute();

		if(%ret) {
			// Immediately override/set the levelsDirectory
			Lab.levelsDirectory = collapseFilename(filePath( %dlg.FileName ));
			%filename = %dlg.FileName;
		}

		%dlg.delete();

		if(! %ret)
			return;
	}

	// close the current editor, it will get cleaned up by MissionCleanup
	if( isObject( "Editor" ) )
		Editor.close( DlgLoadingLevel );

	EditorClearDirty();

	// If we haven't yet connnected, create a server now.
	// Otherwise just load the mission.

	if( !$missionRunning ) {
		activatePackage( "BootEditor" );
		StartLevel( %filename );
	} else {
		Game.loadMission( %filename, true ) ;
		pushInstantGroup();
		// recreate and open the editor
		Editor::create();
		MissionCleanup.add( Editor );
		MissionCleanup.add( Editor.getUndoManager() );
		EditorGui.loadingMission = true;
		Editor.open();
		popInstantGroup();
	}
}
//------------------------------------------------------------------------------

//==============================================================================
// Mission Dirty Check and Update Functions
//==============================================================================

//==============================================================================
/// Checks the various dirty flags and returns true if the
/// mission or other related resources need to be saved.
function EditorIsDirty() {
	// We kept a hard coded test here, but we could break these
	// into the registered tools if we wanted to.
	%isDirty =  ( isObject( "ETerrainEditor" ) && ( ETerrainEditor.isMissionDirty || ETerrainEditor.isDirty ) )
					|| ( isObject( "EWorldEditor" ) && EWorldEditor.isDirty )
					|| ( isObject( "ETerrainPersistMan" ) && ETerrainPersistMan.hasDirty() );

	// Give the editor plugins a chance to set the dirty flag.
	for ( %i = 0; %i < EditorPluginSet.getCount(); %i++ ) {
		%obj = EditorPluginSet.getObject(%i);
		%isDirty |= %obj.isDirty();
	}

	return %isDirty;
}

//------------------------------------------------------------------------------
//==============================================================================
/// Clears all the dirty state without saving.
function EditorClearDirty() {
	EWorldEditor.isDirty = false;
	ETerrainEditor.isDirty = false;
	ETerrainEditor.isMissionDirty = false;
	if (isObject(ETerrainPersistMan))
		ETerrainPersistMan.clearAll();

	for ( %i = 0; %i < EditorPluginSet.getCount(); %i++ ) {
		%obj = EditorPluginSet.getObject(%i);
		%obj.clearDirty();
	}
}
//------------------------------------------------------------------------------





