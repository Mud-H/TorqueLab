//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$TLab_AutoExecPlugin = false;
$TLab_InitLabEditorCompleted = false;
//==============================================================================
function Lab::initLabEditor( %this ) {
	
	if( !isObject( "Lab_PM" ) )
		new PersistenceManager( Lab_PM );

	$LabObj = newScriptObject("LabObj");
	new SimGroup(ToolLabGuiGroup);
	$LabPluginGroup = newSimSet("LabPluginGroup");
	$LabModuleGroup = newSimSet("LabPluginModGroup");
	newSimSet( ToolGuiSet );
	newSimSet( EditorPluginSet );
	//Create a group to keep track of all objects set
	newSimGroup( LabSceneObjectGroups );
	//Create the ScriptObject for the Plugin
	new ScriptObject( WEditorPlugin ) {
		superClass = "EditorPlugin"; //Default to EditorPlugin class
		editorGui = EWorldEditor; //Default to EWorldEditor
		isHidden = true;
	};
	//Prepare the Settings
	%this.initEditorGui();
	//%this.initMenubar();
	%this.initParamsSystem();
	
  
}
//------------------------------------------------------------------------------



//==============================================================================
// All the plugins scripts have been loaded
function Lab::pluginInitCompleted( %this ) {
	
	Lab.initAllModule();
	//%this.prepareAllPluginsGui();
	//ETools.initTools();
	//Prepare the Settings
	Lab.BuildMenus();
	Lab.initConfigSystem();	
	 Lab.activateEventManager();	
}

//------------------------------------------------------------------------------

