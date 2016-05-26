//==============================================================================
// TorqueLab -> Initialize editor plugins
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$TLab::DefaultPlugins = "SceneEditor";

//==============================================================================
// Make sure all GUIs are fine once the editor is launched
function Lab::EditorLaunchGuiSetup(%this) {
	//Lab.attachAllEditorGuis();

	
	Lab.activateFrameWork();
	
	//Lab.updateActivePlugins(); //WIP Fast - Shouldnt be needed
	
	LabToolbarStack.bringToFront(LabToolbarStack-->FirstToolbarGroup);
	LabToolbarStack.pushToBack(LabToolbarStack-->LastToolbarGroup);
	
	
}
//------------------------------------------------------------------------------

//==============================================================================
// Make sure all GUIs are fine once the editor is launched
function Lab::InitialGuiSetup(%this,%pluginName,%displayName,%alwaysEnable) {
	
	//First check the EditorCore integrity and add any missing default GUI
	FW.checkEditorCore();
	Lab.updatePluginsBar();
	Lab.sortPluginsBar(true);
	//ETools.initTools(); //WIP Fast - moved to delayed init
	
	//Store some dimension for future session update
	Lab.toolbarHeight = EditorGuiToolbar.y;
	Lab.pluginBarHeight = LabPluginBar.y;
	Lab.paletteBarWidth = LabPaletteBar.x;
	
	//ESnapOptions-->TabBook.selectPage(0); //WIP Fast - moved to delayed init
	//EVisibilityLayers-->TabBook.selectPage(0); //WIP Fast - moved to delayed init
	//===========================================================================
	// Add the TorqueLab Universal GUIs to the editor
	
	//Lab.addGui( ESceneManager ,"Dialog");
	//Lab.addGui( EManageBookmarks ,"Dialog");
	//Lab.addGui( EManageSFXParameters ,"Dialog");
	//Lab.addGui( ESelectObjects ,"Dialog");
	//Lab.addGui( EPostFxManager ,"Dialog");
	Lab.addGui( StackStartToolbar ,"Toolbar",true);
	Lab.addGui( StackEndToolbar ,"Toolbar",true);
	Lab.addGui( LabSideBar ,"SideBar",true);
	Lab.addGui( EWorldEditor ,"FullEditor");
	//Lab.addGui( EditorGuiStatusBar ,"Root",true);	
	$TLGuiCmd["EToolOverlayGui"] = "parentGroup.pushToBack(EToolOverlayGui);";
	//---------------------------------------------------------------------------

	//---------------------------------------------------------------------------
	EWorldEditorAlignPopup.clear();
	EWorldEditorAlignPopup.add("World",0);
	EWorldEditorAlignPopup.add("Object",1);
	EWorldEditorAlignPopup.setSelected(0);
	//---------------------------------------------------------------------------
	%dropId = 0;
	%selDrop = 0;
	EWorldEditorDropMenu.clear();

	foreach$(%dropType in $Scene_AllDropTypes) {
		%text = $Scene_DropTypeDisplay[%dropType];

		if (Scene.dropMode $= %dropType)
			%selDrop = %dropId;

		if (%text $= "")
			continue;

		EWorldEditorDropMenu.typeId[%dropId] = %dropType;
		EWorldEditorDropMenu.add("Drop> "@%text,%dropId);
		%dropId++;
	}

	EWorldEditorDropMenu.setSelected(%selDrop);
	Scene.dropTypeMenus = strAddWord(Scene.dropTypeMenus,EWorldEditorDropMenu.getId(),true);
	//---------------------------------------------------------------------------
	// this will brind EToolDlgCameraTypes to front so that it goes over the menubar
	EditorGui.pushToBack(EToolDlgCameraTypes);
	EditorGui.pushToBack(VisibilityDropdown);
	
	Lab.resizeEditorGui();
	//Lab.initObjectConfigArray(EWorldEditor,"WorldEditor","General");
	
	//WIP Fast - Moved to a delayed init function, nothing urgent here
	//Lab.initAllToolbarGroups();
	//Lab.initToolbarTrash();
}
//------------------------------------------------------------------------------
//==============================================================================


//==============================================================================
// Clear the editors menu (for dev purpose only as now)
function Lab::resizeEditorGui( %this ) {
	//-----------------------------------------------------
	// ToolsToolbar
	
	LabPluginBar.position = "0 0";// SPC EditorGuiToolbar.extent.y;
	LabPluginBar.extent  = "43 33" ;
	LabPluginBar-->resizeArrow.position = getWord(LabPluginBar.Extent, 0) - 7 SPC "0";
	Lab.expandPluginBar();
	LabPaletteBar.position.y =  LabPluginBar.extent.y;// + EditorGuiToolbar.extent.y;
	LabPaletteBar.position.x = "0";
	//-----------------------------------------------------
	// VisibilityLayerContainer
	EVisibility.Position = getWord(visibilityToggleBtn.position, 0) SPC getWord(EditorGuiToolbar.extent, 1);

	//-----------------------------------------------------
	// CameraSpeedDropdownCtrlContainer
	/*	CameraSpeedDropdownCtrlContainerA.position = firstWord(CameraSpeedDropdownContainer.position) + firstWord(EditorGuiToolbar.position) + -6 SPC
				(getWord(CameraSpeedDropdownContainer, 1)) + 31;
		softSnapSizeSliderCtrlContainer-->slider.position = firstWord(SceneEditorToolbar-->softSnapSizeTextEdit.getGlobalPosition()) - 12 SPC
				(getWord(SceneEditorToolbar-->softSnapSizeTextEdit.getGlobalPosition(), 1)) + 18;
	*/
	foreach(%gui in EditorGui-->EditorContainer)
		%gui.fitIntoParents();

	foreach(%gui in LabEditorGuiSet) {
		%gui.fitIntoParents();
	}

	foreach(%gui in LabPaletteGuiSet) {
		%gui.extent.x = LabPaletteBar.extent.x;
	}
}
//------------------------------------------------------------------------------
