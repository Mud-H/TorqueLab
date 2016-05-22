//==============================================================================
// TorqueLab -> Editor Gui Open and Closing States
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// Initial Editor launch call from EditorManager
function Editor::open(%this) {
	logb("Editor::open( %this )");

	// prevent the mission editor from opening while the GuiEditor is open.
	if(Canvas.getContent() == GuiEditorGui.getId())
		return;
  
//Store current content and set EditorGui as content
	EditorGui.previousGui = Canvas.getContent();
	Canvas.setContent(EditorGui);
	Lab.onWorldEditorOpen();
	$EClient = LocalClientConnection;

	if( !LabEditor.isInitialized ) {
		Lab.onInitialEditorLaunch();
	}

	//EditManager call to set the Editor Enabled
	%this.editorEnabled();
	// Push the ActionMaps in the order that we want to have them
	// before activating an editor plugin, so that if the plugin
	// installs an ActionMap, it will be highest on the stack.
	//MoveMap.push();
	//EditorMap.push();
	//if (!$TLabLimited)
	if (Lab.currentEditor $= "" || !isObject(Lab.currentEditor)) 
		Lab.currentEditor = Lab.defaultPlugin;
	Lab.setEditor( Lab.currentEditor, true );

	//ResetGFX();//WIP Fast
	//Canvas.setContent(EditorGui);

	//The default menu seem to be create somewhere between setCOntent and here
	if(!$Cfg_UI_Menu_UseNativeMenu && isObject(Lab.menuBar)) {
		flog("Editor::open removeMenu");
		Lab.menuBar.removeFromCanvas();
	}

	Lab.EditorLaunchGuiSetup();
	SceneBrowserTree.currentSet = "";

	if (EditorGui-->SideBarContainer.isOpen)
		SideBarMainBook.onTabSelected("test",$SideBarMainBook_CurrentPage); //Hack to force reload

	Lab.SetAutoSave();
}
//------------------------------------------------------------------------------
//==============================================================================
// EditorGui OnWake -> When the EditorGui is rendered
function EditorGui::onWake( %this ) {
	logb("EditorGui::onWake( %this )");
	Lab.onWorldEditorWake();
	activatePackage( TorqueLabPackage );
	Lab.setInitialCamera();
	//EHWorldEditor.setStateOn( 1 );
	startFileChangeNotifications();
	// Notify the editor plugins that the editor has started.
	//if(Canvas.getContent() == GuiEditorGui.getId())
	//return;
	// Active the current editor plugin.
	//if( !Lab.currentEditor.isActivated )
	%slashPos = 0;

	while( strpos( $Server::MissionFile, "/", %slashPos ) != -1 )
		%slashPos = strpos( $Server::MissionFile, "/", %slashPos ) + 1;

	%levelName = getSubStr( $Server::MissionFile , %slashPos , 99 );

	if( %levelName !$= Lab.levelName )
		%this.onNewLevelLoaded( %levelName );

	$TLabWorldReady = true;
	//if ($Cfg_EditorLab_ShowSideBarOnWake)
	Lab.schedule(0,"launchGuiSystem","true");
	
	skipPostFx(false);
}
//------------------------------------------------------------------------------
//==============================================================================
// Called when we have been set as the content and onWake has been called
function EditorGui::onSetContent(%this, %oldContent) {
	logb("EditorGui::onSetContent");
	Lab.attachMenus();
	//Lab.schedule(500,"openSidebar","true");
}
//------------------------------------------------------------------------------
//==============================================================================
//EditManager Functions
//==============================================================================


//==============================================================================
function Lab::onInitialEditorLaunch( %this ) {
	logc("Lab::onInitialEditorLaunch( %this )",%this);

	%this.InitialGuiSetup();
	EWorldEditor.isDirty = false;

	//Get up-to-date config
	if (!$TLabLimited)
		Lab.readAllConfigArray(true);

	if( LabEditor.isInitialized )
		return;

	$SelectedOperation = -1;
	$NextOperationId   = 1;
	$HeightfieldDirtyRow = -1;
	//-----------------------------------------------------
	EWorldEditor.init();
	EWorldEditor.setDisplayType($EditTsCtrl::DisplayTypePerspective);
	//ETerrainEditor.init();
	//Creator.init();
	//SEP_Creator.init();
	Lab.setMenuDefaultState();
	
	Lab.initEditorCamera();
	// sync camera gui
	Lab.syncCameraGui();
	// dropdowns out so that they display correctly in editor gui
	// make sure to show the default world editor guis
	EditorGui.bringToFront( EWorldEditor );
	//EWorldEditor.setVisible( false );

	if (!$TLabLimited) {
		

		Lab.AddSelectionCallback("ETransformBox.updateSource","Transform");
		EVisibilityLayers.init();
	}

	LabEditor.isInitialized = true;
	//EditorFrameContent.columnsDefault = EditorFrameContent.columns;
	//WIP Fast - Called from EditorFrameContent
	//Lab.initSideBar();
	//Lab.schedule(5000,"delayedEditorInit");
}
//------------------------------------------------------------------------------


//-----------------------------------------------------------------------------


//==============================================================================
function Lab::delayedEditorInit( %this,%part ) {
	if (%part $= "")
		%part = "1";

	switch$(%part) {
		case "1":
			tlabExecList("LastList",!$LastGuiExeced);
			$LastGuiExeced = true;
			
			//WIP Fast - Call it from here since everything has been loaded
			FW.checkLaunchedLayout();
			
			Scene.setNewObjectGroup(MissionGroup);
			//Lab.initToolbarPluginTrash();
			
			Scene.setDropType($Cfg_WorldEditor_DropType);
			%this.schedule(500,"delayedEditorInit","2");
			//Scene.SceneTrees = "GlobalSceneTree SceneEditorTree";

		case "2":
			//Initialize GuiLab now
			timerStart("initGuiLab");
			initGuiLab(true);
			timerStop();
			//WIP Fast - moved to delayed init
			//ESnapOptions-->TabBook.selectPage(0); //Should be in gui onWake
			//EVisibilityLayers-->TabBook.selectPage(0); //Should be in gui onWake
			//Reset the TLabGameGui to default state
			TLabGameGui.reset();			
	}

	
}
//-----------------------------------------------------------------------------


//------------------------------------------------------------------------------

