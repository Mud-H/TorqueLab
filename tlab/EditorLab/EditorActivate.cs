//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================


//==============================================================================
function Lab::setEditor( %this, %newEditor, %dontActivate,%callAfterExec ) {
	if (!%newEditor.isEnabled)
		return;



	//First make sure the new editor is valid
	if (%newEditor $= "" || !isObject(%newEditor)) {
		%newEditor = Lab.defaultPlugin;

		if( !isObject( %newEditor )) {
			warnLog("Set editor called for invalid editor plugin:",%newEditor);
			return;
		}
	}

	if (!%newEditor.initialized) {
		Lab.execPlugin(%newEditor);
		%this.schedule(0,"setEditor",%newEditor,%dontActivate,true);
		return;
	}

	// If we have a special set editor function, run that instead
	if( %newEditor.isMethod( "setEditorFunction" ) ) {
		%pluginActivated= %newEditor.setEditorFunction();

		if (!%pluginActivated) {
			warnLog("Plugin failed to activate, keeping the current plugin active");
			return;
		}
	}

	//Make sure currentEditor exist for the next checks
	if ( isObject( %this.currentEditor ) ) {
		//Desactivated current editor if activated and not same as new
		if( %this.currentEditor.getId() != %newEditor.getId() && %this.currentEditor.isActivated)
			%this.currentEditor.onDeactivated(%newEditor);

		//Store current OrthoFOV view to set same in new editor
		if( isObject( %this.currentEditor.editorGui ) )
			%this.orthoFOV = %this.currentEditor.editorGui.getOrthoFOV();
	}

	%this.syncEditor( %newEditor );
	%this.currentEditor = %newEditor;
	//if (!%this.currentEditor.isActivated)
	%this.currentEditor.onActivated();
	Lab.activePluginName = %this.currentEditor.displayName;
	//Lab.activatePlugin(%this.currentEditor);
}
//------------------------------------------------------------------------------
//==============================================================================
// Synchronize tht various UI relative to current Editor
function Lab::syncEditor( %this,%newEditor ) {
	if (%newEditor $= "")
		%newEditor = %this.currentEditor;

	if (!isObject(%newEditor)) {
		warnLog("Trying to sync Toolbar for invalid editor:",%newEditor);
		return;
	}

	%editorName = %newEditor.getName();

	if ($Cfg_UI_Menu_UseNativeMenu) {
		// Sync with menu bar
		%menu = Lab.findMenu( "Editors" );
		%count = %menu.getItemCount();

		for ( %i = 0; %i < %count; %i++ ) {
			%pluginObj = getField( %menu.item[%i], 2 );

			if ( %pluginObj $= %newEditor ) {
				%menu.checkRadioItem( 0, %count, %i );
				break;
			}
		}
	}

	%icon = LabPluginArray.findObjectByInternalName(%newEditor.plugin);

	if (isObject(%icon))
		%icon.setStateOn(1); //Grouped radio button => Other will be turned off automatically

	%paletteName = strreplace(%newEditor, "Plugin", "Palette");

	if (%newEditor.customPalette !$= "") {
		%paletteName = %newEditor.customPalette;
	}

	FW.onresized();
	Lab.togglePluginPalette(%paletteName);
	//LabPaletteBar.togglePalette(%paletteName);
}
//------------------------------------------------------------------------------