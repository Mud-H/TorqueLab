//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================


//=============================================================================================
//    Activation.
//=============================================================================================


//==============================================================================
function Lab::toggleAutoLoadLastGui( %content ) {
	$pref::Editor::AutoLoadLastGui = !$pref::Editor::AutoLoadLastGui;
	info("Auto load last gui is set to:",$pref::Editor::AutoLoadLastGui);
}
//------------------------------------------------------------------------------


//==============================================================================
//==============================================================================
//    Methods.
//==============================================================================
//==============================================================================
package GuiEditor_BlockDialogs {
	function GuiCanvas::pushDialog() {}
	function GuiCanvas::popDialog() {}
};
//------------------------------------------------------------------------------


//==============================================================================
//---------------------------------------------------------------------------------------------

function GuiEditor::switchToWorldEditor( %this ) {
	%editingWorldEditor = false;

	if( GuiEditorContent.getObject( 0 ) == EditorGui.getId() )
		%editingWorldEditor = true;

	ToggleGuiEdit();

	if( !$missionRunning )
		EditorNewLevel();
	else if( !%editingWorldEditor )
		toggleEditor( true );
}

//---------------------------------------------------------------------------------------------

function GuiEditor::enableMenuItems(%this, %val) {
	if( !isObject( %this.menuBar ) )
		return;

	%menu = GuiEditCanvas.menuBar->EditMenu.getID();
	%menu.enableItem( 3, %val ); // cut
	%menu.enableItem( 4, %val ); // copy
	%menu.enableItem( 5, %val ); // paste
	//%menu.enableItem( 7, %val ); // selectall
	//%menu.enableItem( 8, %val ); // deselectall
	%menu.enableItem( 9, %val ); // selectparents
	%menu.enableItem( 10, %val ); // selectchildren
	%menu.enableItem( 11, %val ); // addselectparents
	%menu.enableItem( 12, %val ); // addselectchildren
	%menu.enableItem( 15, %val ); // lock
	%menu.enableItem( 16, %val ); // hide
	%menu.enableItem( 18, %val ); // group
	%menu.enableItem( 19, %val ); // ungroup
	GuiEditCanvas.menuBar->LayoutMenu.enableAllItems( %val );
	GuiEditCanvas.menuBar->MoveMenu.enableAllItems( %val );
}

//---------------------------------------------------------------------------------------------

function GuiEditor::showPrefsDialog(%this) {
	Canvas.pushDialog(GuiEditorPrefsDlg);
}

//---------------------------------------------------------------------------------------------

function GuiEditor::getUndoManager( %this ) {
	if( !isObject( GuiEditorUndoManager ) )
		new UndoManager( GuiEditorUndoManager );

	return GuiEditorUndoManager;
}

//---------------------------------------------------------------------------------------------

function GuiEditor::undo(%this) {
	%action = %this.getUndoManager().getNextUndoName();
	%this.getUndoManager().undo();
	%this.updateUndoMenu();
	//%this.clearSelection();
	GuiEditorStatusBar.print( "Undid '" @ %action @ "'" );
}

//---------------------------------------------------------------------------------------------

function GuiEditor::redo(%this) {
	%action = %this.getUndoManager().getNextRedoName();
	%this.getUndoManager().redo();
	%this.updateUndoMenu();
	//%this.clearSelection();
	GuiEditorStatusBar.print( "Redid '" @ %action @ "'" );
}

//---------------------------------------------------------------------------------------------

function GuiEditor::updateUndoMenu(%this) {
	%uman = %this.getUndoManager();
	%nextUndo = %uman.getNextUndoName();
	%nextRedo = %uman.getNextRedoName();

	if (!isObject(GuiEditCanvas.menuBar))
		return;

	%editMenu = GuiEditCanvas.menuBar->editMenu;
	%editMenu.setItemName( 0, "Undo " @ %nextUndo );
	%editMenu.setItemName( 1, "Redo " @ %nextRedo );
	%editMenu.enableItem( 0, %nextUndo !$= "" );
	%editMenu.enableItem( 1, %nextRedo !$= "" );
}

//---------------------------------------------------------------------------------------------

function GuiEditor::isFilteredClass( %this, %className ) {
	// Filter out all the internal GuiInspector classes.
	if( startsWith( %className, "GuiInspector" ) && %className !$= "GuiInspector" )
		return true;

	// Filter out GuiEditor classes.

	if( startsWith( %className, "GuiEditor" ) )
		return true;

	// Filter out specific classes.

	switch$( %className ) {
	case "GuiCanvas":
		return true;

	case "GuiAviBitmapCtrl":
		return true; // For now.  Probably removed altogether.

	case "GuiArrayCtrl":
		return true; // Abstract base class really.

	case "GuiScintillaTextCtrl":
		return true; // Internal class.

	case "GuiNoMouseCtrl":
		return true; // Too odd.

	case "GuiEditCtrl":
		return true;

	case "GuiBackgroundCtrl":
		return true; // Just plain useless.

	case "GuiTSCtrl":
		return true; // Abstract base class.

	case "GuiTickCtrl":
		return true; // Abstract base class.

	case "GuiWindowCollapseCtrl":
		return true; // Legacy.
	}

	return false;
}

//---------------------------------------------------------------------------------------------

function GuiEditor::editProfile( %this, %profile ) {
	GuiEditorTabBook->profilesPage.select();
	GuiEditorProfilesTree.setSelectedProfile( %profile );
}

//---------------------------------------------------------------------------------------------

function GuiEditor::createControl( %this, %className ) {
	%ctrl = eval( "return new " @ %className @ "();" );

	if( !isObject( %ctrl ) )
		return;

	// Add the control.
	%this.addNewCtrl( %ctrl );
}

//---------------------------------------------------------------------------------------------

/// Group all GuiControls in the currenct selection set under a new GuiControl.
function GuiEditor::groupSelected( %this ) {
	%selection = %this.getSelection();

	if( %selection.getCount() < 2 )
		return;

	// Create action.
	%action = GuiEditorGroupAction::create( %selection, GuiEditor.getContentControl() );
	%action.groupControls();
	// Update editor tree.
	%this.clearSelection();
	%this.addSelection( %action.group[ 0 ].groupObject );
	GuiEditorTreeView.update();
	// Update undo state.
	%action.addtoManager( %this.getUndoManager() );
	%this.updateUndoMenu();
}

//---------------------------------------------------------------------------------------------

/// Take all direct GuiControl instances in the selection set and reparent their child controls
/// to each of the group's parents.  The GuiControl group objects are deleted.
function GuiEditor::ungroupSelected( %this ) {
	%action = GuiEditorUngroupAction::create( %this.getSelection() );
	%action.ungroupControls();
	// Update editor tree.
	%this.clearSelection();
	GuiEditorTreeView.update();
	// Update undo state.
	%action.addToManager( %this.getUndoManager() );
	%this.updateUndoMenu();
}

//---------------------------------------------------------------------------------------------

function GuiEditor::deleteControl( %this, %ctrl ) {
	// Unselect.
	GuiEditor.removeSelection( %ctrl );
	// Record undo.
	%set = new SimSet() {
		parentGroup = RootGroup;
	};
	%set.add( %ctrl );
	%action = UndoActionDeleteObject::create( %set, %this.getTrash(), GuiEditorTreeView );
	%action.addToManager( %this.getUndoManager() );
	%this.updateUndoMenu();
	GuiEditorTreeView.update();
	%set.delete();
	// Remove.
	%this.getTrash().add( %ctrl );
}

//---------------------------------------------------------------------------------------------

function GuiEditor::setPreviewResolution( %this, %width, %height ) {
	//Mud-H Quick hack to prevent a start script error
	if (isObject(GuiEditorRegion))
		GuiEditorRegion.resize( 0, 0, %width, %height );

	if (isObject(GuiEditorContent))
		GuiEditorContent.getObject( 0 ).resize( 0, 0, %width, %height );

	GuiEditor.previewResolution = %width SPC %height;
}

//---------------------------------------------------------------------------------------------

function GuiEditor::toggleEdgeSnap( %this ) {
	%this.snapToEdges = !%this.snapToEdges;
	Lab.checkMenuCodeItem("Gui","snapToEdges",%this.snapToEdges);
	//GuiEditCanvas.menuBar->SnapMenu.checkItem( $GUI_EDITOR_MENU_EDGESNAP_INDEX, %this.snapToEdges );
	GuiEditorEdgeSnapping_btn.setStateOn( %this.snapToEdges );
}

//---------------------------------------------------------------------------------------------

function GuiEditor::toggleCenterSnap( %this ) {
	%this.snapToCenters = !%this.snapToCenters;
	Lab.checkMenuCodeItem("Gui","snapToCenters",%this.snapToCenters);
	//GuiEditCanvas.menuBar->SnapMenu.checkItem( $GUI_EDITOR_MENU_CENTERSNAP_INDEX, %this.snapToCenters );
	GuiEditorCenterSnapping_btn.setStateOn( %this.snapToCenters );
}

//---------------------------------------------------------------------------------------------

function GuiEditor::toggleFullBoxSelection( %this ) {
	%this.fullBoxSelection = !%this.fullBoxSelection;
	Lab.checkMenuCodeItem("Gui","fullBoxSelection",%this.fullBoxSelection);
	//GuiEditCanvas.menuBar->EditMenu.checkItem( $GUI_EDITOR_MENU_FULLBOXSELECT_INDEX, %this.fullBoxSelection );
}

//---------------------------------------------------------------------------------------------

function GuiEditor::toggleDrawGuides( %this ) {
	%this.drawGuides= !%this.drawGuides;
	Lab.checkMenuCodeItem("Gui","drawGuides",%this.drawGuides);
	//GuiEditCanvas.menuBar->SnapMenu.checkItem( $GUI_EDITOR_MENU_DRAWGUIDES_INDEX, %this.drawGuides );
}

//---------------------------------------------------------------------------------------------

function GuiEditor::toggleGuideSnap( %this ) {
	%this.snapToGuides = !%this.snapToGuides;
	Lab.checkMenuCodeItem("Gui","snapToGuides",%this.snapToGuides);
	//GuiEditCanvas.menuBar->SnapMenu.checkItem( $GUI_EDITOR_MENU_GUIDESNAP_INDEX, %this.snapToGuides );
}

//---------------------------------------------------------------------------------------------

function GuiEditor::toggleControlSnap( %this ) {
	%this.snapToControls = !%this.snapToControls;
	Lab.checkMenuCodeItem("Gui","snapToGuides",%this.snapToControls);
	//GuiEditCanvas.menuBar->SnapMenu.checkItem( $GUI_EDITOR_MENU_CONTROLSNAP_INDEX, %this.snapToControls );
}

//---------------------------------------------------------------------------------------------

function GuiEditor::toggleCanvasSnap( %this ) {
	%this.snapToCanvas = !%this.snapToCanvas;
	Lab.checkMenuCodeItem("Gui","snapToCanvas",%this.snapToCanvas);
	//GuiEditCanvas.menuBar->SnapMenu.checkItem( $GUI_EDITOR_MENU_CANVASSNAP_INDEX, %this.snapToCanvas );
}

//---------------------------------------------------------------------------------------------

function GuiEditor::toggleGridSnap( %this ) {
	%this.snap2Grid = !%this.snap2Grid;

	if( !%this.snap2Grid )
		%this.setSnapToGrid( 0 );
	else
		%this.setSnapToGrid( %this.snap2GridSize );

	Lab.checkMenuCodeItem("Gui","snap2Grid",%this.snap2Grid);
	//GuiEditCanvas.menuBar->SnapMenu.checkItem( $GUI_EDITOR_MENU_GRIDSNAP_INDEX, %this.snap2Grid );
	GuiEditorSnapCheckBox.setStateOn( %this.snap2Grid );
}

//---------------------------------------------------------------------------------------------

function GuiEditor::toggleLockSelection( %this ) {
	%this.toggleFlagInAllSelectedObjects( "locked" );
}

//---------------------------------------------------------------------------------------------

function GuiEditor::toggleHideSelection( %this ) {
	%this.toggleFlagInAllSelectedObjects( "hidden" );
}

//---------------------------------------------------------------------------------------------

function GuiEditor::selectAllControlsInSet( %this, %set, %deselect ) {
	if( !isObject( %set ) )
		return;

	foreach( %obj in %set ) {
		if( !%obj.isMemberOfClass( "GuiControl" ) )
			continue;

		if( !%deselect )
			%this.addSelection( %obj );
		else
			%this.removeSelection( %obj );
	}
}

//---------------------------------------------------------------------------------------------

function GuiEditor::toggleFlagInAllSelectedObjects( %this, %flagFieldName ) {
	// Use the inspector's code here to record undo information
	// for the field edits.
	GuiEditorInspectFields.onInspectorPreFieldModification( %flagFieldName );
	%selected = %this.getSelection();

	foreach( %object in %selected )
		%object.setFieldValue( %flagFieldName, !%object.getFieldValue( %flagFieldName ) );

	GuiEditorInspectFields.onInspectorPostFieldModification();
	GuiEditorInspectFields.refresh();
}

//=============================================================================================
//    Resolution List.
//=============================================================================================

//---------------------------------------------------------------------------------------------

function GuiEditorResList::init( %this ) {
	%this.clear();
	// Non-widescreen formats.
	%this.add( "640x480 (VGA, 4:3)", 640 );
	%this.add( "800x600 (SVGA, 4:3)", 800 );
	%this.add( "1024x768 (XGA, 4:3)", 1024 );
	%this.add( "1280x1024 (SXGA, 4:3)", 1280 );
	%this.add( "1600x1200 (UXGA, 4:3)", 1600 );
	// Widescreen formats.
	%this.add( "1280x720 (WXGA, 16:9)", 720 );
	%this.add( "1600x900 (16:9)", 900 );
	%this.add( "1920x1080 (16:9)", 1080 );
	%this.add( "1440x900 (WXGA+, 16:10)", 900 );
	%this.add( "1680x1050 (WSXGA+, 16:10)", 1050 );
	%this.add( "1920x1200 (WUXGA, 16:10)", 1200 );
}

//---------------------------------------------------------------------------------------------

function GuiEditorResList::selectFormat( %this, %format ) {
	%width = getWord( %format, 0 );
	%height = getWord( %format, 1 );

	switch( %height ) {
	case 720:
		%this.setSelected( 720 );

	case 900:
		%this.setSelected( 900 );

	case 1050:
		%this.setSelected( 1050 );

	case 1080:
		%this.setSelected( 1080 );

	default:
		switch( %width ) {
		case 640:
			%this.setSelected( 640 );

		case 800:
			%this.setSelected( 800 );

		case 1024:
			%this.setSelected( 1024 );

		case 1280:
			%this.setSelected( 1280 );

		case 1600:
			%this.setSelected( 1600 );

		default:
			%this.setSelected( 1200 );
		}
	}
}

//---------------------------------------------------------------------------------------------

function GuiEditorResList::onSelect( %this, %id ) {
	switch( %id ) {
	case 640:
		GuiEditor.setPreviewResolution( 640, 480 );

	case 800:
		GuiEditor.setPreviewResolution( 800, 600 );

	case 1024:
		GuiEditor.setPreviewResolution( 1024, 768 );

	case 1280:
		GuiEditor.setPreviewResolution( 1280, 1024 );

	case 1600:
		GuiEditor.setPreviewResolution( 1600, 1200 );

	case 720:
		GuiEditor.setPreviewResolution( 1280, 720 );

	case 900:
		GuiEditor.setPreviewResolution( 1440, 900 );

	case 1050:
		GuiEditor.setPreviewResolution( 1680, 1050 );

	case 1080:
		GuiEditor.setPreviewResolution( 1920, 1080 );

	case 1200:
		GuiEditor.setPreviewResolution( 1920, 1200 );
	}
}

//=============================================================================================
//    Sidebar.
//=============================================================================================

//---------------------------------------------------------------------------------------------

function GuiEditorTabBook::onWake( %this ) {
	if( !isObject( "GuiEditorTabBookLibraryPopup" ) )
		new PopupMenu( GuiEditorTabBookLibraryPopup ) {
		superClass = "MenuBuilder";
		isPopup = true;
		item[ 0 ] = "Alphabetical View" TAB "" TAB "GuiEditorToolbox.setViewType( \"Alphabetical\" );";
		item[ 1 ] = "Categorized View" TAB "" TAB "GuiEditorToolbox.setViewType( \"Categorized\" );";
	};
}

//---------------------------------------------------------------------------------------------

function GuiEditorTabBook::onTabSelected( %this, %text, %index ) {
	%sidebar = GuiEditorSidebar;

	if (!isObject(%sidebar))
		return;

	%name = %this.getObject( %index ).getInternalName();

	switch$( %name ) {
	case "guiPage":
		%sidebar-->button1.setVisible( false );
		%sidebar-->button2.setVisible( false );
		%sidebar-->button3.setVisible( true );
		%sidebar-->button4.setVisible( true );
		%sidebar-->button4.setBitmap( "tlab/art/buttons/default/delete" );
		%sidebar-->button4.command = "GuiEditor.deleteSelection();";
		%sidebar-->button4.tooltip = "Delete Selected Control(s)";
		%sidebar-->button3.setBitmap( "tlab/art/buttons/default/visible" );
		%sidebar-->button3.command = "GuiEditor.toggleHideSelection();";
		%sidebar-->button3.tooltip = "Hide Selected Control(s)";

	case "profilesPage":
		%sidebar-->button1.setVisible( true );
		%sidebar-->button2.setVisible( true );
		%sidebar-->button3.setVisible( true );
		%sidebar-->button4.setVisible( true );
		%sidebar-->button4.setBitmap( "tlab/art/buttons/default/delete" );
		%sidebar-->button4.command = "GuiEditor.showDeleteProfileDialog( GuiEditorProfilesTree.getSelectedProfile() );";
		%sidebar-->button4.tooltip = "Delete Selected Profile";
		%sidebar-->button3.setBitmap( "tlab/art/buttons/default/new" );
		%sidebar-->button3.command = "GuiEditor.createNewProfile( \"Unnamed\" );";
		%sidebar-->button3.tooltip = "Create New Profile with Default Values";
		%sidebar-->button2.setBitmap( "tlab/art/buttons/default/copy-btn" );
		%sidebar-->button2.command = "GuiEditor.createNewProfile( GuiEditorProfilesTree.getSelectedProfile().getName(), GuiEditorProfilesTree.getSelectedProfile() );";
		%sidebar-->button2.tooltip = "Create New Profile by Copying the Selected Profile";
		%sidebar-->button1.setBitmap( "tlab/art/buttons/default/reset-icon" );
		%sidebar-->button1.command = "GuiEditor.revertProfile( GuiEditorProfilesTree.getSelectedProfile() );";
		%sidebar-->button1.tooltip = "Revert Changes to the Selected Profile";

	case "toolboxPage":
		//TODO
		%sidebar-->button1.setVisible( false );
		%sidebar-->button2.setVisible( false );
		%sidebar-->button3.setVisible( false );
		%sidebar-->button4.setVisible( false );
	}
}

//---------------------------------------------------------------------------------------------

function GuiEditorTabBook::onTabRightClick( %this, %text, %index ) {
	%name = %this.getObject( %index ).getInternalName();

	switch$( %name ) {
	case "toolboxPage":
		// Open toolbox popup.
		%popup = GuiEditorTabBookLibraryPopup;
		%currentViewType = GuiEditorToolbox.getViewType();

		switch$( %currentViewType ) {
		case "Alphabetical":
			%popup.checkRadioItem( 0, 1, 0 );

		case "Categorized":
			%popup.checkRadioItem( 0, 1, 1 );
		}

		%popup.showPopup( Canvas );
	}
}

//=============================================================================================
//    Toolbar.
//=============================================================================================

//---------------------------------------------------------------------------------------------

function GuiEditorSnapCheckBox::onWake(%this) {
	%snap = GuiEditor.snap2grid * GuiEditor.snap2gridsize;
	%this.setValue( %snap );
	GuiEditor.setSnapToGrid( %snap );
}

//---------------------------------------------------------------------------------------------

function GuiEditorSnapCheckBox::onAction(%this) {
	%snap = GuiEditor.snap2gridsize * %this.getValue();
	GuiEditor.snap2grid = %this.getValue();
	GuiEditor.setSnapToGrid(%snap);
}

//=============================================================================================
//    GuiEditorGui.
//=============================================================================================
