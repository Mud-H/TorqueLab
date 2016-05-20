//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$InGuiEditor = false;
$MLAAFxGuiEditorTemp = false;

//==============================================================================
// Toggle the GuiEditor( Close if open and open if closed)
function ToggleGuiEdit( %fromGame,%loadLast ) {
	logd("ToggleGuiEdit %fromGame,%loadLast",%fromGame,%loadLast);
	if (Canvas.isFullscreen() && $Cfg_UI_Menu_UseNativeMenu) {
		LabMsgOK("Windowed Mode Required", "Please switch to windowed mode to access the GUI Editor.");
		return;
	}

	GuiEditor.forceContent = "";

	//If called from a loaded game or WorldEditor
	if (%fromGame) {
		if( EditorIsActive() && !GuiEditor.toggleIntoEditorGui ) {
			if (EditorGui.isAwake()) {
				GuiEditor.forceContent = EditorGui;
			}

			toggleEditor( true,true );
		}

		// Cancel the scheduled event to prevent the level from cycling after it's duration has elapsed.
		cancel($Game::Schedule);
	}

	if (!$InGuiEditor) {
		GuiEd.launchEditor(%loadLast);
	} else {
		GuiEd.closeEditor();
	}
}
//------------------------------------------------------------------------------


GlobalActionMap.bindCmd( keyboard, "f10", "toggleGuiEdit(true);","" );
GlobalActionMap.bindCmd( keyboard, "ctrl f10", "toggleGuiEdit(true,true);","" );




//------------------------------------------------------------------------------


//==============================================================================
function GuiEd::launchEditor( %this,%loadLast ) {
   logd("GuiEd::launchEditor");
   // AlterVerse Script Modification (MAR) - Remove toggleCursor() bind in editor >>>
  // globalActionMap.unbind( mouse, button1);
  // unbindChatCommands();
  Lab.onGuiEditorOpen();
   // AlterVerse Script Modification (MAR) - Remove toggleCursor() bind in editor <<<
   $LabEditorLaunch = "Gui";
	//Store current Content to return to it
//	%initialContent = Canvas.getContent();

	
	if ( isObject(GuiEditor.forceContent))
		%initialContent = GuiEditor.forceContent;
	else if (!isObject(%initialContent))
		%initialContent = Canvas.getContent();
		
	if (($pref::Editor::AutoLoadLastGui || %loadLast) && isObject(GuiEd.lastGuiLoaded))
		%initialGui = GuiEd.lastGuiLoaded;	
	else
		%initialGui = %initialContent;

	GuiEditor.initialGui = %initialGui;
	GuiEditor.initialContent =%initialContent;
	//GuiEditor.initialContent =%initialContent;

	GuiEditContent(%initialGui);

	//Temp fix to disable MLAA when in GUI editor
	if( isObject(MLAAFx) && MLAAFx.isEnabled==true ) {
		MLAAFx.isEnabled = false;
		$MLAAFxGuiEditorTemp = true;
	}

	//GuiEdMap.push();
	//Make sure the TorqueLab loading progress is stopped
	EditorLoadingGui.endInit();
	
}
//------------------------------------------------------------------------------

//==============================================================================
function GuiEd::closeEditor( %this ) {
	logd("GuiEd::closeEditor");
   // AlterVerse Script Modification (MAR) - Remove toggleCursor() bind in editor >>>
  // globalActionMap.bind( mouse, button1, toggleCursor );
  // bindChatCommands();
   // AlterVerse Script Modification (MAR) - Remove toggleCursor() bind in editor <<<
   Lab.onGuiEditorClose();
	//deactivatePackage( TorqueLabPackage );
	Lab.lastGuiEditSource = GuiEditor.lastContent;
	GuiEditCanvas.quit();
	
}
//------------------------------------------------------------------------------

//==============================================================================
function GuiEd::toggleAutoLoadLastGui( %this ) {
	$pref::Editor::AutoLoadLastGui = !$pref::Editor::AutoLoadLastGui;
	info("Auto load last gui is set to:",$pref::Editor::AutoLoadLastGui);
}
//------------------------------------------------------------------------------

//==============================================================================
function GuiEd::onChangedGuiDone(%this  ) {		
	GuiEdMap.push();
	
}
//------------------------------------------------------------------------------
//==============================================================================
//    Activation.
//==============================================================================

//==============================================================================
function GuiEditContent( %content ) {
	logd("GuiEditContent %content",%content);
	if( !isObject( GuiEditCanvas ) )
		new GuiControl( GuiEditCanvas, EditorGuiGroup );

	GuiEdMap.push();
	$InGuiEditor = true;
	GuiEditor.openForEditing( %content );
}
//------------------------------------------------------------------------------
//==============================================================================
function GuiEditor::openForEditing( %this, %content ) {
	logd("GuiEditor::openForEditing %content",%content);
	if (!isObject(%content))
		return;
	//activatePackage( TorqueLabPackage );
	Canvas.setContent( GuiEditorGui );
	GuiEd.initGuiEditor();

	while( GuiEditorContent.getCount() )
		GuiGroup.add( GuiEditorContent.getObject( 0 ) ); // get rid of anything being edited

	// Clear the current guide set and add the guides
	// from the control.
	//Mud-H Quick hack to prevent a start script error
	%this.clearGuides();
	%this.readGuides( %content );
	// Enumerate GUIs and put them into the content list.
	GuiEditorContentList.init();
	GuiEditorScroll.scrollToTop();
	activatePackage( GuiEditor_BlockDialogs );
	GuiEditorContent.add( %content );
	deactivatePackage( GuiEditor_BlockDialogs );
	GuiEditorContentList.sort();

	if(%content.getName() $= "")
		%name = "(unnamed) - " @ %content;
	else
		%name = %content.getName() @ " - " @ %content;

	GuiEditorContentList.setText(%name);
	%this.setContentControl(%content);
	// Initialize the preview resolution list and select the current
	// preview resolution.
	GuiEditorResList.init();
	%res = %this.previewResolution;

	if( %res $= "" )
		%res = "1024 768";

	GuiEditorResList.selectFormat( %res );
	// Initialize the treeview and expand the first level.
	GuiEditorTreeView.init();
	GuiEditorTreeView.open( %content );
	GuiEditorTreeView.expandItem( 1 );

	// Initialize profiles tree.

	if( !GuiEditorProfilesTree.isInitialized ) {
		GuiEditorProfilesTree.init();
		GuiEditorProfilesTree.isInitialized = true;
	}

	// Create profile change manager if we haven't already.
	if( !isObject( GuiEditorProfileChangeManager ) )
		new SimGroup( GuiEditorProfileChangeManager );

	// clear the undo manager if we're switching controls.
	if( %this.lastContent != %content )
		GuiEditor.getUndoManager().clearAll();

	GuiEditor.setFirstResponder();
	%this.updateUndoMenu();
	%this.lastContent = %content;
	GuiEd.lastGuiLoaded = %content;
	
	GuiEd.schedule(200,"onChangedGuiDone");
}
//------------------------------------------------------------------------------

//==============================================================================
//    Deactivation.
//==============================================================================
function GuiEditCanvas::close( %this ) {
	logd("GuiEditCanvas::close");
}

//---------------------------------------------------------------------------------------------

function GuiEditCanvas::quit( %this ) {
	logd("GuiEditCanvas::quit");
	%this.close();
	GuiGroup.add(GuiEditorGui);
	// we must not delete a window while in its event handler, or we foul the event dispatch mechanism
	%this.schedule(10, delete);
	$InGuiEditor = false;

	if (!isObject(GuiEditor.initialContent)) {
		warnLog("Closing the GuiEditor and there's no valid content to load:",GuiEditor.initialContent,"Using default MainMenuGui");
		GuiEditor.initialContent = $Cfg_TLab_defaultGui;
	}
	
	
	Canvas.setContent(GuiEditor.initialContent);

	// Canvas.setContent(GuiEditor.lastContent);

	//Temp fix to disable MLAA when in GUI editor
	if( isObject(MLAAFx) && $MLAAFxGuiEditorTemp==true ) {
		MLAAFx.isEnabled = true;
		$MLAAFxGuiEditorTemp = false;
	}
}
