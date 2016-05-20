//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// TorqueLab Editor Initialization Function
//==============================================================================
$TorqueLabLoaded = false;
$TorqueLabShowTimer = false;



function initTorqueLab(%launchEditor,%guiEditorOnly) {
	if (!isObject(LabGroup)) {
		$LabGroup = new SimGroup(LabGroup);
		RootGroup.add(LabGroup);
	}

	

	%instantGroup = $instantGroup;

	$instantGroup = LabGroup;
if (!isObject(LabCfg))
		new ScriptObject(LabCfg) {
		internalName = "LabConfig";
	};
	if (!$TorqueLabLoaded) {
		timerStepStart("initTorqueLab","TL");
		$Lab = new ScriptObject(Lab);
		if (!isObject(Scene))
			newScriptObject("Scene");

		Lab.defaultPlugin = "SceneEditorPlugin";

		//Load the Editor profile before loading the game onStart (Nooo...)
		if (!$LabThemeLoaded)
			exec("tlab/themes/initTheme.cs");

		exec( "tlab/EditorLab/gui/core/cursors.ed.cs" );
		//Start by loading the TorqueLab Loading Progress GUI to use it now
		exec("tlab/EditorLab/gui/core/EditorLoadingGui.gui");
		exec("tlab/EditorLab/gui/core/EditorLoadingGui.cs");
		EditorLoadingGui.startInit();
		//loadTorqueLabProfiles();
		/*	new Settings(EditorSettings)
			{
				file = "tlab/settings.xml";
			};
			EditorSettings.read();*/
		info( "Initializing TorqueLab" );
			
			
		exec("tlab/core/execScripts.cs");
		tlabExecList("InitList");
	
		Lab.ExecMenuBarSystem();
		
		// Common GUI stuff.
		Lab.initLabEditor();
		tlabExecList("MainList");
		$LabGuiExeced = true;
		//$TorqueLabLoaded = true;
	
	} else
		EditorLoadingGui.startInit();

	//Load The GuiEditor
	if (!isObject(GuiEditor))
		Lab.loadGuiEditor();

	//Load only GuiEditor removed until init process cleaned
	if (!%guiEditorOnly)
		Lab.loadWorldEditor();
	
	
	if (!$TorqueLabLoaded)
		Lab.schedule(500,"delayedEditorInit");

	//Lab.BuildMenus();
	EditorIconRegistry::loadFromPath( "tlab/art/icons/object_class/" );

	if (%launchEditor $= "Gui")
		ToggleGuiEdit();
	else if (%launchEditor $= "World")
		toggleEditor( true,true );
	else
		EditorLoadingGui.endInit();

	$instantGroup = %instantGroup;
	$TorqueLabLoaded = true;
}

function Lab::loadWorldEditor(%this) {
	
	
	// Default file path when saving from the editor (such as prefabs)
	if ($Pref::WorldEditor::LastPath $= "") {
		$Pref::WorldEditor::LastPath = getMainDotCsDir();
	}


	//%toggle = $Scripts::ignoreDSOs;
	//$Scripts::ignoreDSOs = true;
	$ignoredDatablockSet = new SimSet();
	// fill the list of editors
	$editors[count] = getWordCount( $Lab::loadFirst );

	for ( %i = 0; %i < $editors[count]; %i++ ) {
		$editors[%i] = getWord( $Lab::loadFirst, %i );
	}

	%pattern = $Lab::resourcePath @ "/*/main.cs";
	%list = getDirectoryList("tlab/",0);

	for(%i=0; %i<getFieldCount(%list); %i++) {
		%folder = getField(%list,%i);

		if (!isFile("tlab/"@%folder@"/main.cs")&& !isFile("tlab/"@%folder@"/main.cs.dso"))
			continue;

		// Yes, this sucks and should be done better
		if ( strstr( $Lab::loadFirst, %folder ) == -1 ) {
			$editors[$editors[count]] = %folder;
			$editors[count]++;
		}
	}

	// initialize every editor
	%count = $editors[count];

	//  exec( "./worldEditor/main.cs" );
	foreach$(%tmpFolder in $LabIgnoreEnableFolderList)
		$ToolFolder[%tmpFolder] = "1";


	for ( %i = 0; %i < %count; %i++ ) {
		eval("%enabledEd = $pref::WorldEditor::"@$editors[%i]@"::Enabled;");

		if ((!%enabledEd && !$ToolFolder[%tmpFolder]) || $LabEditorLoaded[$editors[%i]]) {
			continue;
		}

		if (strFind($Lab::loadLast,$editors[%i])) {
			%finalLoadList = strAddWord(%finalLoadList,$editors[%i]);
			continue;
		}

		

		if (isFile("./" @ $editors[%i] @ "/main.cs" ))
			exec( "./" @ $editors[%i] @ "/main.cs" );

		$LabEditorLoaded[$editors[%i]] = true;
		%initFunction = "init" @ $editors[%i];
		%initializeFunction = "initialize" @ $editors[%i];
		if( isFunction( %initFunction ) )
		{
			
			%name = $TLab_PluginName_[$editors[%i]];
			%type = "Plugin";
			if ($TLab_PluginType_[$editors[%i]] $= "Module")
			{
				%type = "Module";
				Lab.createModule($editors[%i],%name);
			}
			else
				Lab.createPlugin($editors[%i],%name);				
			info(%type@":",$editors[%i],"created with name",%name);
			
		
		}
		
		else if( isFunction( %initializeFunction ) )
		{
			call( %initializeFunction );
		}

		

	}

	foreach$(%editor in %finalLoadList) {
		exec( "./" @ %editor @ "/main.cs" );
		%initializeFunction = "initialize" @ %editor;

		if( isFunction( %initializeFunction ) )
			call( %initializeFunction );
	}

	// Popuplate the default SimObject icons that
	// are used by the various editors.
	//$Scripts::ignoreDSOs = %toggle;
	Lab.pluginInitCompleted();
	Lab.BuildWorldMenu();
	GlobalActionMap.bind(keyboard, "f11", toggleEditor);
	$WorldEditorLoaded = true;
}
function Lab::loadGuiEditor(%this) {
	exec("tlab/guiEditor/main.cs");
	initializeGuiEditor();
	Lab.BuildGuiMenu();
	GlobalActionMap.bindCmd( keyboard, "f10", "toggleGuiEdit(true);","" );
}
function dumpTorqueLabInit() {
	for(%i=0; %i<getRecordCount($TLabInitTimeLog); %i++) {
		%record = getRecord($TLabInitTimeLog,%i);
		%editor = getField(  %record,0);
		%initTime = getField(  %record,2);
		info(%editor, "initialisation time was:",%initTime);
	}
}

