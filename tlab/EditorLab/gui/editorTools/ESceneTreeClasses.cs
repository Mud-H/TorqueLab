//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$ESceneTreeClassesLoaded = 0;
//==============================================================================
//FileBrowserTree.initFiles

function ESceneTreeClasses::onShow( %this ) {
	if ( !isObject( ar_ESceneTreeClasses ) ) {
		%this.init();
	}

	%this.updatePresetMenu();

	if(!$ESceneTreeClassesLoaded) {
		//EVisibilityLayers.position = visibilityToggleBtn.position;
		%this.initOptions();
		%this.addClassOptions();
		$EVisibilityLayers_Initialized = true;
	}
}
function ESceneTreeClasses::init( %this ) {
	if ( !isObject( ar_ESceneTreeClasses ) )
		%classArray = newArrayObject("ar_ESceneTreeClasses");

	%this.classArray = ar_ESceneTreeClasses;
	%stack = ESceneTreeClasses-->theClassList;
	// First clear the stack control.
	%stack.clear();
	%classList = enumerateConsoleClasses( "SceneObject" );
	%classCount = getFieldCount( %classList );

	for ( %i = 0; %i < %classCount; %i++ ) {
		%className = getField( %classList, %i );
		%this.classArray.push_back( %className );
	}

	// Remove duplicates and sort by key.
	%this.classArray.uniqueKey();
	%this.classArray.sortkd();

	// Go through all the
	// parameters in our array and
	// create a check box for each.
	for ( %i = 0; %i < %this.classArray.count(); %i++ ) {
		%class = %this.classArray.getKey( %i );
		%classVar = "$" @ %class @ "::isShownInTree";
		%textLength = strlen( %class );
		%text = "  " @ %class;
		// Add visibility toggle.
		%classCheckBox = new GuiCheckBoxCtrl() {
			canSaveDynamicFields = "0";
			isContainer = "0";
			Profile = "ToolsCheckBoxProfile";
			HorizSizing = "right";
			VertSizing = "bottom";
			Position = "0 0";
			Extent = (%textLength * 4) @ " 18";
			MinExtent = "8 2";
			canSave = "1";
			Visible = "1";
			Variable = %classVar;
			command = "EVisibilityLayers.toggleRenderable(\""@%class@"\");";
			tooltipprofile = "ToolsToolTipProfile";
			hovertime = "1000";
			tooltip = "Show/hide all " @ %class @ " objects.";
			text = %text;
			groupNum = "-1";
			buttonType = "ToggleButton";
			useMouseEvents = "0";
			useInactiveState = "0";
		};
		//Variable = %visVar;
		%stack.addGuiControl( %classCheckBox );
	}

	$ESceneTreeClassesLoaded = true;
}

function ESceneTreeClasses::updatePresetMenu( %this ) {
	%searchFolder = "tlab/EditorLab/gui/editorTools/visibilityLayers/presets/*.layers.ucs";
	//Now go through each files again to add a brush with latest items
	ESceneTreeClasses_PresetMenu.clear();
	%selected = 0;
	ESceneTreeClasses_PresetMenu.add("Select a preset",0);

	for(%presetFile = findFirstFile(%searchFolder); %presetFile !$= ""; %presetFile = findNextFile(%searchFolder)) {
		%presetName = strreplace(fileBase(%presetFile),".layers","");
		ESceneTreeClasses_PresetMenu.add(%presetName,%pid++);
	}

	ESceneTreeClasses_PresetMenu.setSelected(%selected);
}


//EVisibilityLayers.exportPresetSample
function ESceneTreeClasses::exportPresetSample( %this ) {
	%layerExampleObj = newScriptObject("ESceneTreeClassesPresetExample");

	for ( %i = 0; %i < %this.classArray.count(); %i++ ) {
		%class = %this.classArray.getKey( %i );
		eval("%shown = $" @ %class @ "::isShownInTree;");
		%layerExampleObj.shown[%class] = %shown;
	}

	%layerExampleObj.save("tlab/EditorLab/gui/editorTools/sceneTreeClasses/presetExample.classes.ucs",0,"%presetClasses = ");
}



//EVisibilityLayers.loadPresetFile("visBuilder");
function ESceneTreeClasses::loadPresetFile( %this,%filename ) {
	%file = "tlab/EditorLab/gui/editorTools/sceneTreeClasses/presets/"@%filename@".layers.ucs";

	if (!isFile(%file))
		return;

	exec(%file);

	for ( %i = 0; %i < %this.classArray.count(); %i++ ) {
		%class = %this.classArray.getKey( %i );
		%shown = %presetClasses.shown[%class];

		if (%selectable !$= "") {
			eval("$" @ %class @ "::isShownInTree = \""@%shown@"\";");
			info("Class:",%class,"isShownInTree set to:",%shown);
		}
	}

	ESceneTreeClasses.currentPresetFile = %filename;
}

function ESceneTreeClasses_Preset::onSelect( %this,%id,%text ) {
	logd("ESceneTreeClasses_Preset onSelect",%id,%text);

	if (	%id $= "0")
		return;

	ESceneTreeClasses.loadPresetFile(%text);
}

