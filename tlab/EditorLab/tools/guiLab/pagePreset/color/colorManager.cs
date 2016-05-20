//==============================================================================
// Lab GuiManager -> Profile Color Management System
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$GLab::DefaultColorsFile = "tlab/EditorLab/gui/editorDialogs/guiLab/colorPresets/default.cs";

$GLab_ColorGroups = "colorFont colorFill";
//==============================================================================
// Initialized the Color Manager by loading default colors and updating the colors
function GLab::initColorManager( %this,%force ) {
	if ($GLab::ColorManager::Loaded && !%force)
		return;

	exec($GLab::DefaultColorsFile);
	//Scan and initialize the loaded color groups
	%this.updateColors();
	//Scan the presets and update the menu
	GLab.initColorPresetMenu();
	%this.updateProfilesColors();
	$GLab::ColorManager::Loaded = true;
}
//------------------------------------------------------------------------------



//==============================================================================
function GLab::saveColors( %this,%saveDefault ) {
	%file = $GLab::DefaultColorsFile;

	if (!%saveDefault) {
		%name = $GLab_PresetName;

		if(%name $= "default" || %name $= "[default]") {
			msgBoxOk("Can't name a preset \"Default\"","You are trying to save a preset named default, this name is reserved for the default color set. Please change the name and try again","");
			return;
		}

		%file = strreplace(%file,"default",%name@".set");
		%id = DIG_ColorPresetMenu.lastId;

		if (!isFile(%file)) {
			GLab.addColorPresetFile(%file);
			GLab_ColorPresetMenu.setSelected(GLab_ColorPresetMenu.lastId,false);
		}
	}

	GuiColor_Group.save(%file,false,"delObj(GuiColor_Group); \n");
}
//------------------------------------------------------------------------------

//==============================================================================
// Manage color presets menu
//==============================================================================
//==============================================================================
function GLab::initColorPresetMenu( %this ) {
	GLab_ColorPresetMenu.clear();
	%filePathScript = "tlab/EditorLab/gui/editorDialogs/guiLab/colorPresets/*.set.cs";
	%pid = 0;
	GLab_ColorPresetMenu.lastId = -1;
	GLab.addColorPresetFile($GLab::DefaultColorsFile);
	//GLab_ColorPresetMenu.add("Default",%pid);

	for(%file = findFirstFile(%filePathScript); %file !$= ""; %file = findNextFile(%filePathScript)) {
		GLab.addColorPresetFile(%file);
	}

	GLab_ColorPresetMenu.setSelected(0);
}
//------------------------------------------------------------------------------
//==============================================================================
function GLab::addColorPresetFile( %this,%file ) {
	%fileName = strreplace(fileBase(%file),".","\t");
	%fileName = getField(%fileName,0);
	GLab_ColorPresetMenu.add(%fileName, GLab_ColorPresetMenu.lastId++);
}
//------------------------------------------------------------------------------
//==============================================================================
function GLab_ColorPresetMenu::onSelectPreset( %this ) {
	%name = %this.getText();
	%file = $GLab::DefaultColorsFile;

	if (%name !$= "Default") {
		%file = strreplace(%file,"default",%name@".set");
	}

	if(%name $= "default")
		%name = "[default]";

	GLab_ColorPresetName.setText(%name);
	exec(%file);
	GLab.updateColors();
	GLab.updateProfilesColors();
}
//------------------------------------------------------------------------------


//==============================================================================
// Color Picker callbacks for Gui Manager color selection
//==============================================================================

//==============================================================================
// Empty Editor Gui
function GuiColorDefaultPicker::ColorRefresh(%this) {
	loga("GuiColorDefaultPicker::ColorPicked(%this)",%this,%color);
	%srcObj = %this.sourceObject;
	%srcField = %this.sourceField;
	%alpha = mCeil(getWord(%color,3));
	%color = setWord(%color,3,%alpha);
	%this.baseColor = ColorIntToFloat(%color);
	%srcObj.setFieldValue(%srcField,%color);
}
//------------------------------------------------------------------------------
//==============================================================================
// Empty Editor Gui
function GuiColorDefaultPicker::ColorPicked(%this,%color) {
	loga("GuiColorDefaultPicker::ColorPicked(%this,%color)",%this,%color);
	%srcObj = %this.sourceObject;
	%srcField = %this.sourceField;
	%alpha = mCeil(getWord(%color,3));
	%color = setWord(%color,3,%alpha);
	%this.baseColor = ColorIntToFloat(%color);
	%srcObj.setFieldValue(%srcField,%color);

	if (isObject(%this.alphaSlider)) {
		devLog("Color Picked, updating Slider to:",%alpha);
		%this.alphaSlider.setValue(%alpha);
	}
}
//------------------------------------------------------------------------------
//==============================================================================
// Empty Editor Gui
function GuiColorDefaultPicker::ColorUpdated(%this,%color) {
	loga("GuiColorDefaultPicker::ColorUpdated(%this,%color)",%this,%color);
	%alpha = mCeil(getWord(%color,3));
	%this.baseColor = ColorIntToFloat(%color);
	// %this.sourceArray.setValue(%this.internalName,%color);
	%this.updateColor();

	if (isObject(%this.alphaSlider)) {
		devLog("Color Updated, updating Slider to:",%alpha);
		%this.alphaSlider.setValue(%alpha);
	}
}
//------------------------------------------------------------------------------

//==============================================================================
// Empty Editor Gui
function GLabSliderAlpha::update(%this) {
	loga("GLabSliderAlpha::update(%this)",%this,%this.getValue());

	if (!isObject(%this.colorPicker)) {
		warnLog("Invalid color picker referenced!");
		return;
	}

	%this.colorPicker.baseColor.a = %this.getValue();
	%current = $GLab_SelectedObject.getFieldValue(%this.fieldSource);
	%currentAlpha = %current.a;
	%intAlpha = mCeil(%this.getValue() * 255);
	%new = setWord(%current,3,%intAlpha);
	devLog("Update Alpha field:",%this.fieldSource,"From",%current,"To",%new);
	%this.colorPicker.evalUpdate();
}
//------------------------------------------------------------------------------
//==============================================================================
// Empty Editor Gui
function GLabSliderAlpha::altUpdate(%this) {
	loga("GLabSliderAlpha::altUpdate(%this)",%this,%this.getValue());

	if (!isObject(%this.colorPicker)) {
		warnLog("Invalid color picker referenced!");
		return;
	}

	%this.colorPicker.baseColor.a = %this.getValue();
}
//------------------------------------------------------------------------------
