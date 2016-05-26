//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$TLabLimited = true;
$TLabTraceInitialLaunch = false;
%helpersLab = "tlab/core/helpers/initHelpers.cs";

if (isFile(%helpersLab) && !$HelperLabLoaded)
	exec(%helpersLab);

//exec("tlab/avTools.cs");
//==============================================================================
//Do TorqueLab should be initialized after main onStart?
$TorqueLabInitMode = "0"; 
//0 = Wait for toggle bind (F10, F11)
//1 = At end of main onSTart function
//2 = After StartupGui On Wake
//Anything Else = wait for a initTorqueLab(); call place anywhere
//If $TorqueLabInitOnStart is set to false, you need to initialize it manually
//There's 3 options available for post start initialization
//1- It will initialized when you press F10 or F11 (Editors toggle binds)
//2- Add a initTorqueLab(); call anywhere in your scripts
//3- Use the custom function overide package at bottom (At end of Lab Package)
//   You simply set the function from which you want TorqueLab to be initialized.
//------------------------------------------------------------------------------

//==============================================================================
// TorqueLab Core Global Settings
//==============================================================================
// Path to the folder that contains the editors we will load.
$Lab::OldConfigSystem = false;
$Lab::resourcePath = "tlab/";
$Cfg_TLab_Theme = "Laborean";
if (isObject($pref::UI::defaultGui))
	$Cfg_TLab_defaultGui = $pref::UI::defaultGui;
else
	$Cfg_TLab_defaultGui = "MainMenuGui";

$LabGameMap = "moveMap";
// Global holding material list for active simobject
$Lab::materialEditorList = "";

// These must be loaded first, in this order, before anything else is loaded
$Lab::loadFirst = "sceneEditor";
$Lab::loadLast = "materialLab";
// These folders must be skipped for initial load
$LabIgnoreEnableFolderList = "debugger forestEditor levels guiEditor";

$Lab::LevelRoot = "levels";
//Load the TorqueLab Main Initialization script.
exec("tlab/initTorqueLab.cs");
function lateToolStart() {	
	   initTorqueLab();	
	}
//==============================================================================
// Lab Package contain overide function that deal with TorqueLab
//==============================================================================
package Lab {
	//==============================================================================
	// onStart() - Called when the application is launched by the engine
	//------------------------------------------------------------------------------
	// If $TorqueLabInitOnStart is true, TorqueLab will be loaded at end of onStart
	// Else, it will bind the editor toggle keys to the init function and TorqueLab
	// will be initialized when any editor is toggled. It will launch the editor once
	// initialization is completed. If TLab is loaded manually from elsewhere, those
	// binds will be overiden with the normal toggle binds.
	//------------------------------------------------------------------------------
	function onStart() {
		if ($TorqueLabInitMode $= "1") {
			Parent::onStart();
			initTorqueLab();			
			return;
		}
		else if ($TorqueLabInitMode $= "0") {			
			GlobalActionMap.bindCmd(keyboard,"F10","initTorqueLab(\"Gui\",true);","");
			GlobalActionMap.bindCmd(keyboard,"F11","initTorqueLab(\"World\");","");
		}
		Parent::onStart();
		
	}
	
	//------------------------------------------------------------------------------

	//==============================================================================
	// onExit() - Call just before the application is shutted down
	//------------------------------------------------------------------------------
	// This will terminated some editor process and make sure everything is deleted
	//------------------------------------------------------------------------------
	function onExit() {
		if( LabEditor.isInitialized )
			EditorGui.shutdown();

		// Free all the icon images in the registry.
		EditorIconRegistry::clear();

		//Call destroy function of all editors
		for (%i = 0; %i < $editors[count]; %i++) {
			%destroyFunction = "destroy" @ $editors[%i];

			if( isFunction( %destroyFunction ) )
				call( %destroyFunction );
		}

		// Call Parent.
		Parent::onExit();
	}
	//------------------------------------------------------------------------------
	//==============================================================================
	//Custom Overide Function TorqueLab initialization (Example)
	//------------------------------------------------------------------------------
	// This is another way to initialize TorqueLab from anywhere, just set the function
	// information from which you want TLab to be loaded. If you want it to load at start
	// of the function, simply place initTorqueLab(); before the Parent:: call. The example
	// will make TorqueLab to load at the end of the StartupGui onWake function.
	//------------------------------------------------------------------------------
	// Uncomment and set on which function call you want to load TorqueLab
	function StartupGui::onWake(%this) {
		if (isFunction("Parent::onWake"))
			Parent::onWake(%this);
		//initTorqueLab();
		if ($TorqueLabInitMode $= "2")
			schedule(500,0,"initTorqueLab");
	}
	//------------------------------------------------------------------------------
	//==============================================================================
   // All the plugins scripts have been loaded
   function Editor::checkActiveLoadDone( %this ) {
      if (!isObject(EditorGui))
         return false;
      if (EditorGui.isAwake())
         return true;
      return false;
   }
   //------------------------------------------------------------------------------

};
//------------------------------------------------------------------------------
//==============================================================================
// Activate Package Lab
activatePackage(Lab);
//------------------------------------------------------------------------------

