//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//=============================================================================================
//    GuiEditorGui.
//=============================================================================================

//---------------------------------------------------------------------------------------------

function GuiEditorGui::onWake( %this ) {
	
	Lab.onGuiEditorWake();
	GuiEdToggle.setStateOn( 1 );

	if( !isObject( %this->SelectControlsDlg ) ) {
		%this.add( GuiEditorSelectDlg );
		GuiEditorSelectDlg.setVisible( false );
	}

	// Attach our menus.

	if( isObject( %this.menuGroup ) )
		for( %i = 0; %i < %this.menuGroup.getCount(); %i ++ )
			%this.menuGroup.getObject( %i ).attachToMenuBar();

	// Read settings.
	%this.initSettings();
	%this.readSettings();

	// Initialize toolbox.

	if( !GuiEditorToolbox.isInitialized )
		GuiEditorToolbox.initialize();

	if (isObject(GuiEditCanvas.menuBar)) {
		// Set up initial menu toggle states.
		GuiEditCanvas.menuBar->SnapMenu.checkItem( $GUI_EDITOR_MENU_EDGESNAP_INDEX, GuiEditor.snapToEdges );
		GuiEditCanvas.menuBar->SnapMenu.checkItem( $GUI_EDITOR_MENU_CENTERSNAP_INDEX, GuiEditor.snapToCenters );
		GuiEditCanvas.menuBar->SnapMenu.checkItem( $GUI_EDITOR_MENU_GUIDESNAP_INDEX, GuiEditor.snapToGuides );
		GuiEditCanvas.menuBar->SnapMenu.checkItem( $GUI_EDITOR_MENU_CONTROLSNAP_INDEX, GuiEditor.snapToControls );
		GuiEditCanvas.menuBar->SnapMenu.checkItem( $GUI_EDITOR_MENU_CANVASSNAP_INDEX, GuiEditor.snapToCanvas );
		GuiEditCanvas.menuBar->SnapMenu.checkItem( $GUI_EDITOR_MENU_GRIDSNAP_INDEX, GuiEditor.snap2Grid );
		GuiEditCanvas.menuBar->SnapMenu.checkItem( $GUI_EDITOR_MENU_DRAWGUIDES_INDEX, GuiEditor.drawGuides );
		GuiEditCanvas.menuBar->EditMenu.checkItem( $GUI_EDITOR_MENU_FULLBOXSELECT_INDEX, GuiEditor.fullBoxSelection );
	}

	// Sync toolbar buttons.
	GuiEditorSnapCheckBox.setStateOn( GuiEditor.snap2Grid );
	GuiEditorEdgeSnapping_btn.setStateOn( GuiEditor.snapToEdges );
	GuiEditorCenterSnapping_btn.setStateOn( GuiEditor.snapToCenters );
	
	//
}

//---------------------------------------------------------------------------------------------

function GuiEditorGui::onSleep( %this) {
	Lab.onGuiEditorSleep();
	// If we are editing a control, store its guide state.
	%content = GuiEditor.getContentControl();

	if( isObject( %content ) )
		GuiEditor.writeGuides( %content );

	// Remove our menus.

	if( isObject( %this.menuGroup ) )
		for( %i = 0; %i < %this.menuGroup.getCount(); %i ++ )
			%this.menuGroup.getObject( %i ).removeFromMenuBar();

	// Store our preferences.
	%this.writeSettings();
	//Lab.onGuiEditorSleep();
}

//---------------------------------------------------------------------------------------------

function GuiEditorGui::initSettings( %this ) {
   
   return;
	
   if (!isObject(EditorSettings))
   	new Settings(EditorSettings)
		{
			file = "tlab/settings.xml";
		};
   EditorSettings.read();	EditorSettings.beginGroup( "GuiEditor", true );
	EditorSettings.setDefaultValue( "lastPath", "" );
	EditorSettings.setDefaultValue( "previewResolution", "1024 768" );
	EditorSettings.beginGroup( "EngineDevelopment" );
	EditorSettings.setDefaultValue( "toggleIntoEditor", 0 );
	EditorSettings.setDefaultValue( "showEditorProfiles", 0 );
	EditorSettings.setDefaultValue( "showEditorGuis", 0 );
	EditorSettings.endGroup();
	EditorSettings.beginGroup( "Library" );
	EditorSettings.setDefaultValue( "viewType", "Categorized" );
	EditorSettings.endGroup();
	EditorSettings.beginGroup( "Snapping" );
	EditorSettings.setDefaultValue( "snapToControls", "1" );
	EditorSettings.setDefaultValue( "snapToGuides", "1" );
	EditorSettings.setDefaultValue( "snapToCanvas", "1" );
	EditorSettings.setDefaultValue( "snapToEdges", "1" );
	EditorSettings.setDefaultValue( "snapToCenters", "1" );
	EditorSettings.setDefaultValue( "sensitivity", "2" );
	EditorSettings.setDefaultValue( "snap2Grid", "0" );
	EditorSettings.setDefaultValue( "snap2GridSize", $GuiEditor::defaultGridSize );
	EditorSettings.endGroup();

	EditorSettings.beginGroup( "Selection" );
	EditorSettings.setDefaultValue( "fullBox", "0" );
	EditorSettings.endGroup();
	EditorSettings.beginGroup( "Rendering" );
	EditorSettings.setDefaultValue( "drawBorderLines", "1" );
	EditorSettings.setDefaultValue( "drawGuides", "1" );
	EditorSettings.endGroup();
	EditorSettings.beginGroup( "Help" );
	EditorSettings.setDefaultValue( "documentationURL", "http://www.garagegames.com/products/torque-3d/documentation/user" ); //RDTODO: make this point to Gui Editor docs when available
	// Create a path to the local documentation.  This is a bit of guesswork here.
	// It assumes that the project is located in a folder of the SDK root directory
	// (e.g. "Examples/" or "Demos/") and that from there the path to the game
	// folder is "<project>/game".
	EditorSettings.setDefaultValue("documentationLocal", "../../../Documentation/Official Documentation.html"  );
	EditorSettings.setDefaultValue("documentationReference", "../../../Documentation/Torque 3D - Script Manual.chm"  );
	EditorSettings.endGroup();
	EditorSettings.endGroup();
}

//---------------------------------------------------------------------------------------------

function GuiEditorGui::readSettings( %this ) {
   
   if ($Cfg_GuiEditor_Snapping_snap2GridSize <= "2")
   	$Cfg_GuiEditor_Snapping_snap2GridSize = "2";    
				GuiEditor.snapToGuides = $Cfg_GuiEditor_Snapping_snapToGuides;
	GuiEditor.snapToControls = $Cfg_GuiEditor_Snapping_snapToControls;
	GuiEditor.snapToCanvas = $Cfg_GuiEditor_Snapping_snapToCanvas;
	GuiEditor.snapToEdges = $Cfg_GuiEditor_Snapping_snapToEdges;
	GuiEditor.snapToCenters = $Cfg_GuiEditor_Snapping_snapToCenters;
	GuiEditor.snapSensitivity = $Cfg_GuiEditor_Snapping_sensitivity;
	GuiEditor.snap2Grid = $Cfg_GuiEditor_Snapping_snap2Grid;
	GuiEditor.snap2GridSize = $Cfg_GuiEditor_Snapping_snap2GridSize;
	GuiEditor.lastPath = $Cfg_GuiEditor_Editor_lastPath;
	GuiEditor.previewResolution = $Cfg_GuiEditor_Editor_previewResolution;
	GuiEditor.toggleIntoEditor = $Cfg_GuiEditor_EngineDevelopment_toggleIntoEditor;
	GuiEditor.showEditorProfiles = $Cfg_GuiEditor_EngineDevelopment_showEditorProfiles;
	GuiEditor.showEditorGuis = $Cfg_GuiEditor_EngineDevelopment_showEditorGuis;
	GuiEditorToolbox.currentViewType = $Cfg_GuiEditor_Library_viewType;
	GuiEditor.fullBoxSelection = $Cfg_GuiEditor_Selection_fullBox;
	GuiEditor.drawBorderLines = $Cfg_GuiEditor_Rendering_drawBorderLines;
	GuiEditor.drawGuides = $Cfg_GuiEditor_Rendering_drawGuides;
	GuiEditor.documentationURL = $Cfg_GuiEditor_Help_documentationURL;
	GuiEditor.documentationLocal = $Cfg_GuiEditor_Help_documentationLocal;
	GuiEditor.documentationReference = $Cfg_GuiEditor_Help_documentationReference;
	
if( GuiEditor.snap2Grid )
		GuiEditor.setSnapToGrid( GuiEditor.snap2GridSize );
	
	return;
	EditorSettings.read();
	EditorSettings.beginGroup( "GuiEditor", true );
		GuiEditor.lastPath = EditorSettings.value( "lastPath" );
	GuiEditor.previewResolution = EditorSettings.value( "previewResolution" );
	EditorSettings.beginGroup( "EngineDevelopment" );
	GuiEditor.toggleIntoEditor = EditorSettings.value( "toggleIntoEditor" );
	GuiEditor.showEditorProfiles = EditorSettings.value( "showEditorProfiles" );
	GuiEditor.showEditorGuis = EditorSettings.value( "showEditorGuis" );
	EditorSettings.endGroup();
	EditorSettings.beginGroup( "Library" );
	GuiEditorToolbox.currentViewType = EditorSettings.value( "viewType" );
	EditorSettings.endGroup();
	EditorSettings.beginGroup( "Snapping" );
	GuiEditor.snapToGuides = EditorSettings.value( "snapToGuides" );
	GuiEditor.snapToControls = EditorSettings.value( "snapToControls" );
	GuiEditor.snapToCanvas = EditorSettings.value( "snapToCanvas" );
	GuiEditor.snapToEdges = EditorSettings.value( "snapToEdges" );
	GuiEditor.snapToCenters = EditorSettings.value( "snapToCenters" );
	GuiEditor.snapSensitivity = EditorSettings.value( "sensitivity" );
	GuiEditor.snap2Grid = EditorSettings.value( "snap2Grid" );
	GuiEditor.snap2GridSize = EditorSettings.value( "snap2GridSize" );
	EditorSettings.endGroup();
	EditorSettings.beginGroup( "Selection" );
	GuiEditor.fullBoxSelection = EditorSettings.value( "fullBox" );
	EditorSettings.endGroup();
	EditorSettings.beginGroup( "Rendering" );
	GuiEditor.drawBorderLines = EditorSettings.value( "drawBorderLines" );
	GuiEditor.drawGuides = EditorSettings.value( "drawGuides" );
	EditorSettings.endGroup();
	EditorSettings.beginGroup( "Help" );
	GuiEditor.documentationURL = EditorSettings.value( "documentationURL" );
	GuiEditor.documentationLocal = EditorSettings.value( "documentationLocal" );
	GuiEditor.documentationReference = EditorSettings.value( "documentationReference" );
	EditorSettings.endGroup();
	EditorSettings.endGroup();

	if( GuiEditor.snap2Grid )
		GuiEditor.setSnapToGrid( GuiEditor.snap2GridSize );
}

//---------------------------------------------------------------------------------------------

function GuiEditorGui::writeSettings( %this ) {
   
   $Cfg_GuiEditor_Snapping_snapToGuides = GuiEditor.snapToGuides;
	 $Cfg_GuiEditor_Snapping_snapToControls = GuiEditor.snapToControls;
	 $Cfg_GuiEditor_Snapping_snapToCanvas = GuiEditor.snapToCanvas;
	 $Cfg_GuiEditor_Snapping_snapToEdges = GuiEditor.snapToEdges;
	 $Cfg_GuiEditor_Snapping_snapToCenters = GuiEditor.snapToCenters;
	 $Cfg_GuiEditor_Snapping_sensitivity = GuiEditor.snapSensitivity;
	$Cfg_GuiEditor_Snapping_snap2Grid = GuiEditor.snap2Grid;
	 $Cfg_GuiEditor_Snapping_snap2GridSize = GuiEditor.snap2GridSize;
	 $Cfg_GuiEditor_Editor_lastPath = GuiEditor.lastPath;
	 $Cfg_GuiEditor_Editor_previewResolution = GuiEditor.previewResolution;
	 $Cfg_GuiEditor_EngineDevelopment_toggleIntoEditor = GuiEditor.toggleIntoEditor;
	 $Cfg_GuiEditor_EngineDevelopment_showEditorProfiles = GuiEditor.showEditorProfiles;
	$Cfg_GuiEditor_EngineDevelopment_showEditorGuis = GuiEditor.showEditorGuis; 
	 $Cfg_GuiEditor_Library_viewType = GuiEditorToolbox.currentViewType;
	 $Cfg_GuiEditor_Selection_fullBox = GuiEditor.fullBoxSelection;
	 $Cfg_GuiEditor_Rendering_drawBorderLines = GuiEditor.drawBorderLines;
	 $Cfg_GuiEditor_Rendering_drawGuides = GuiEditor.drawGuides;
	 $Cfg_GuiEditor_Help_documentationURL = GuiEditor.documentationURL;
	 $Cfg_GuiEditor_Help_documentationLocal = GuiEditor.documentationLocal;
	 $Cfg_GuiEditor_Help_documentationReference = GuiEditor.documentationReference;
	 
	 tlabSave();
	return;
	
	EditorSettings.beginGroup( "GuiEditor", true );
	EditorSettings.setValue( "lastPath", GuiEditor.lastPath );
	EditorSettings.setValue( "previewResolution", GuiEditor.previewResolution );
	EditorSettings.beginGroup( "EngineDevelopment" );
	EditorSettings.setValue( "toggleIntoEditor", GuiEditor.toggleIntoEditor );
	EditorSettings.setValue( "showEditorProfiles", GuiEditor.showEditorProfiles );
	EditorSettings.setValue( "showEditorGuis", GuiEditor.showEditorGuis );
	EditorSettings.endGroup();
	EditorSettings.beginGroup( "Library" );
	EditorSettings.setValue( "viewType", GuiEditorToolbox.currentViewType );
	EditorSettings.endGroup();
	EditorSettings.beginGroup( "Snapping" );
	EditorSettings.setValue( "snapToControls", GuiEditor.snapToControls );
	EditorSettings.setValue( "snapToGuides", GuiEditor.snapToGuides );
	EditorSettings.setValue( "snapToCanvas", GuiEditor.snapToCanvas );
	EditorSettings.setValue( "snapToEdges", GuiEditor.snapToEdges );
	EditorSettings.setValue( "snapToCenters", GuiEditor.snapToCenters );
	EditorSettings.setValue( "sensitivity", GuiEditor.snapSensitivity );
	EditorSettings.setValue( "snap2Grid", GuiEditor.snap2Grid );
	EditorSettings.setValue( "snap2GridSize", GuiEditor.snap2GridSize );
	EditorSettings.endGroup();
	EditorSettings.beginGroup( "Selection" );
	EditorSettings.setValue( "fullBox", GuiEditor.fullBoxSelection );
	EditorSettings.endGroup();
	EditorSettings.beginGroup( "Rendering" );
	EditorSettings.setValue( "drawBorderLines", GuiEditor.drawBorderLines );
	EditorSettings.setValue( "drawGuides", GuiEditor.drawGuides );
	EditorSettings.endGroup();
	EditorSettings.beginGroup( "Help" );
	EditorSettings.setValue( "documentationURL", GuiEditor.documentationURL );
	EditorSettings.setValue( "documentationLocal", GuiEditor.documentationLocal );
	EditorSettings.setValue( "documentationReference", GuiEditor.documentationReference );
	EditorSettings.endGroup();
	EditorSettings.endGroup();
	//EditorSettings.write();
	//EditorSettings.write();
}