//==============================================================================
// TorqueLab Editor ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================


//==============================================================================
function GuiEdMenu::initData(%this) {
	//set up $LabCmd variable so that it matches OS standards
	if( $platform $= "macos" ) {
		$LabCmd = "Cmd";
		$LabMenuCmd = "Cmd";
		$LabQuit = "Cmd Q";
		$LabRedo = "Cmd-Shift Z";
	} else {
		$LabCmd = "Ctrl";
		$LabMenuCmd = "Alt";
		$LabQuit = "Alt F4";
		$LabRedo = "Ctrl Y";
	}

	%id = -1;
	%itemId = -1;
	$LabMenuGui_[%id++] = "File";
	$LabMenuItemGui_[%id,%itemId++] = "New Gui..." TAB $LabCmd SPC "N" TAB "GuiEditCanvas.create();";
	$LabMenuItemGui_[%id,%itemId++] = "Open..." TAB $LabCmd SPC "O" TAB "GuiEditCanvas.open();";
	$LabMenuItemGui_[%id,%itemId++] = "Save" TAB $LabCmd SPC "S" TAB "GuiEditCanvas.save( false, true );";
	$LabMenuItemGui_[%id,%itemId++] = "Save As..." TAB $LabCmd @ "-Shift S" TAB "GuiEditCanvas.save( false );";
	$LabMenuItemGui_[%id,%itemId++] = "Save Selected As..." TAB $LabCmd @ "-Alt S" TAB "GuiEditCanvas.save( true );";
	$LabMenuItemGui_[%id,%itemId++] = "-";
	$LabMenuItemGui_[%id,%itemId++] = "Revert Gui" TAB "" TAB "GuiEditCanvas.revert();";
	$LabMenuItemGui_[%id,%itemId++] = "Add Gui From File..." TAB "" TAB "GuiEditCanvas.append();";
	$LabMenuItemGui_[%id,%itemId++] = "-";
	$LabMenuItemGui_[%id,%itemId++] = "Open Gui File in Torsion" TAB "" TAB "GuiEditCanvas.openInTorsion();";
	$LabMenuItemGui_[%id,%itemId++] = "-";
	$LabMenuItemGui_[%id,%itemId++] = "Close Editor" TAB "F10" TAB "GuiEditCanvas.quit();";
	$LabMenuItemGui_[%id,%itemId++] = "Quit" TAB $LabCmd SPC "Q" TAB "quit();";
	%itemId = -1;
	$LabMenuGui_[%id++] = "Edit";
	$LabMenuItemGui_[%id,%itemId++] = "Undo" TAB $LabCmd SPC "Z" TAB "GuiEditor.undo();";
	$LabMenuItemGui_[%id,%itemId++] = "Redo" TAB $LabRedo TAB "GuiEditor.redo();";
	$LabMenuItemGui_[%id,%itemId++] = "-";
	$LabMenuItemGui_[%id,%itemId++] = "Cut" TAB $LabCmd SPC "X" TAB "GuiEditor.saveSelection(); GuiEditor.deleteSelection();";
	$LabMenuItemGui_[%id,%itemId++] = "Copy" TAB $LabCmd SPC "C" TAB "GuiEditor.saveSelection();";
	$LabMenuItemGui_[%id,%itemId++] = "Paste" TAB $LabCmd SPC "V" TAB "GuiEditor.loadSelection();";
	$LabMenuItemGui_[%id,%itemId++] = "-";
	$LabMenuItemGui_[%id,%itemId++] = "Select All" TAB $LabCmd SPC "A" TAB "GuiEditor.selectAll();";
	$LabMenuItemGui_[%id,%itemId++] = "Deselect All" TAB $LabCmd SPC "D" TAB "GuiEditor.clearSelection();";
	$LabMenuItemGui_[%id,%itemId++] = "Select Parent(s)" TAB $LabCmd @ "-Alt Up" TAB "GuiEditor.selectParents();";
	$LabMenuItemGui_[%id,%itemId++] = "Select Children" TAB $LabCmd @ "-Alt Down" TAB "GuiEditor.selectChildren();";
	$LabMenuItemGui_[%id,%itemId++] = "Add Parent(s) to Selection" TAB $LabCmd @ "-Alt-Shift Up" TAB "GuiEditor.selectParents( true );";
	$LabMenuItemGui_[%id,%itemId++] = "Add Children to Selection" TAB $LabCmd @ "-Alt-Shift Down" TAB "GuiEditor.selectChildren( true );";
	$LabMenuItemGui_[%id,%itemId++] = "Select..." TAB "" TAB "GuiEditorSelectDlg.toggleVisibility();";
	$LabMenuItemGui_[%id,%itemId++] = "-";
	$LabMenuItemGui_[%id,%itemId++] = "Lock/Unlock Selection" TAB $LabCmd SPC "L" TAB "GuiEditor.toggleLockSelection();";
	$LabMenuItemGui_[%id,%itemId++] = "Hide/Unhide Selection" TAB $LabCmd SPC "H" TAB "GuiEditor.toggleHideSelection();";
	$LabMenuItemGui_[%id,%itemId++] = "-";
	$LabMenuItemGui_[%id,%itemId++] = "Group Selection" TAB $LabCmd SPC "G" TAB "GuiEditor.groupSelected();";
	$LabMenuItemGui_[%id,%itemId++] = "Ungroup Selection" TAB $LabCmd @ "-Shift G" TAB "GuiEditor.ungroupSelected();";
	$LabMenuItemGui_[%id,%itemId++] = "-";
	$LabMenuItemGui_[%id,%itemId++] = "Full Box Selection" TAB "" TAB "GuiEditor.toggleFullBoxSelection();";
	$LabMenuItemGui_[%id,%itemId++] = "-";
	$LabMenuItemGui_[%id,%itemId++] = "Grid Size" TAB $LabCmd SPC "," TAB "GuiEditor.showPrefsDialog();";
	%itemId = -1;
	$LabMenuGui_[%id++] = "Layout";
	$LabMenuItemGui_[%id,%itemId++] = "Align Left" TAB $LabCmd SPC "Left" TAB "GuiEditor.Justify(0);";
	%subId=-1;
	$LabSubMenuItemGui_[%id,%itemId,%subId++] = "SlowestA" TAB $LabCmd @ "-Shift 1" TAB "5" TAB "$ToggleMe = !$ToggleMe;";
	$LabSubMenuItemGui_[%id,%itemId,%subId++] = "SlowA" TAB $LabCmd @ "-Shift 2" TAB "35" TAB "$ToggleMe = !$ToggleMe;";
	$LabSubMenuItemGui_[%id,%itemId,%subId++] = "SlowerA" TAB $LabCmd @ "-Shift 3" TAB "70" TAB "$ToggleMe = !$ToggleMe;";
	$LabMenuItemGui_[%id,%itemId++] = "Center Horizontally" TAB "" TAB "GuiEditor.Justify(1);" TAB "$ToggleMe = !$ToggleMe;";
	%subId=-1;
	$LabSubMenuItemGui_[%id,%itemId,%subId++] = "SlowestA" TAB $LabCmd @ "-Shift 1" TAB "5" TAB "$ToggleMe = !$ToggleMe;";
	$LabSubMenuItemGui_[%id,%itemId,%subId++] = "SlowA" TAB $LabCmd @ "-Shift 2" TAB "35" TAB "$ToggleMe = !$ToggleMe;";
	$LabSubMenuItemGui_[%id,%itemId,%subId++] = "SlowerA" TAB $LabCmd @ "-Shift 3" TAB "70" TAB "$ToggleMe = !$ToggleMe;";
	$LabMenuItemGui_[%id,%itemId++] = "Align Right" TAB $LabCmd SPC "Right" TAB "GuiEditor.Justify(2);";
	$LabMenuItemGui_[%id,%itemId++] = "-";
	$LabMenuItemGui_[%id,%itemId++] = "Align Top" TAB $LabCmd SPC "Up" TAB "GuiEditor.Justify(3);";
	$LabMenuItemGui_[%id,%itemId++] = "Center Vertically" TAB "" TAB "GuiEditor.Justify(7);";
	$LabMenuItemGui_[%id,%itemId++] = "Align Bottom" TAB $LabCmd SPC "Down" TAB "GuiEditor.Justify(4);";
	$LabMenuItemGui_[%id,%itemId++] = "-";
	$LabMenuItemGui_[%id,%itemId++] = "Space Vertically" TAB "" TAB "GuiEditor.Justify(5);";
	$LabMenuItemGui_[%id,%itemId++] = "Space Horizontally" TAB "" TAB "GuiEditor.Justify(6);";
	$LabMenuItemGui_[%id,%itemId++] = "-";
	$LabMenuItemGui_[%id,%itemId++] = "Fit into Parent(s)" TAB "Alt f" TAB "GuiEditor.fitIntoParents();";
	$LabMenuItemGui_[%id,%itemId++] = "Fit Width to Parent(s)" TAB "Alt w" TAB "GuiEditor.fitIntoParents( true, false );";
	$LabMenuItemGui_[%id,%itemId++] = "Fit Height to Parent(s)" TAB "Alt h" TAB "GuiEditor.fitIntoParents( false, true );";
	$LabMenuItemGui_[%id,%itemId++] = "Force into Parent-" TAB "Alt g" TAB "Lab.forceCtrlInsideParent( );";
	$LabMenuItemGui_[%id,%itemId++] = "-";
	$LabMenuItemGui_[%id,%itemId++] = "Bring to Front" TAB "" TAB "GuiEditor.BringToFront();";
	$LabMenuItemGui_[%id,%itemId++] = "Send to Back" TAB "" TAB "GuiEditor.PushToBack();";
	$LabMenuItemGui_[%id,%itemId++] = "-";
	$LabMenuGui_[%id++] = "LabLayout";
	$LabMenuItemGui_[%id,%itemId = 0] = "Align Left" TAB $LabCmd SPC "Left" TAB "GuiEd.AlignCtrlToParent(\"left\");";
	$LabMenuItemGui_[%id,%itemId++] = "Align Right" TAB $LabCmd SPC "Left" TAB "GuiEd.AlignCtrlToParent(\"right\");";
	$LabMenuItemGui_[%id,%itemId++] = "Align Top" TAB $LabCmd SPC "Left" TAB "GuiEd.AlignCtrlToParent(\"top\");";
	$LabMenuItemGui_[%id,%itemId++] = "Align Bottom" TAB $LabCmd SPC "Left" TAB "GuiEd.AlignCtrlToParent(\"bottom\");";
	$LabMenuItemGui_[%id,%itemId++] = "-";
	$LabMenuItemGui_[%id,%itemId++] = "Force into Parent-" TAB "Alt g" TAB "Lab.forceCtrlInsideParent( );";
	$LabMenuItemGui_[%id,%itemId++] = "-";
	$LabMenuItemGui_[%id,%itemId++] = "Set control as reference-" TAB "Alt r" TAB "Lab.setSelectedControlAsReference( );";
	$LabMenuItemGui_[%id,%itemId++] = "Set profile from reference-" TAB "Shift 1" TAB "Lab.setControlReferenceField(\" Profile \");";
	$LabMenuItemGui_[%id,%itemId++] = "Set position from reference-" TAB "Shift 2" TAB "Lab.setControlReferenceField(\" position \");";
	$LabMenuItemGui_[%id,%itemId++] = "Set extent from reference-" TAB "Shift 3" TAB "Lab.setControlReferenceField(\" extent \");";
	$LabMenuItemGui_[%id,%itemId++] = "Set empty name tp selection-" TAB "Shift n" TAB "Lab.setControlReferenceField(\" name \");";
	$LabMenuItemGui_[%id,%itemId++] = "-";
	$LabMenuItemGui_[%id,%itemId++] = "Toggle auto load last GUI" TAB "" TAB "Lab.toggleAutoLoadLastGui();" TAB "$pref::Editor::AutoLoadLastGui;";
	%itemId = -1;
	$LabMenuGui_[%id++] = "Move";
	$LabMenuItemGui_[%id,%itemId++] = "Nudge Left" TAB "Left" TAB "GuiEditor.move( -1, 0);";
	$LabMenuItemGui_[%id,%itemId++] = "Nudge Right" TAB "Right" TAB "GuiEditor.move( 1, 0);";
	$LabMenuItemGui_[%id,%itemId++] = "Nudge Up" TAB "Up" TAB "GuiEditor.move( 0, -1);";
	$LabMenuItemGui_[%id,%itemId++] = "Nudge Down" TAB "Down" TAB "GuiEditor.move( 0, 1 );";
	$LabMenuItemGui_[%id,%itemId++] = "-";
	$LabMenuItemGui_[%id,%itemId++] = "Big Nudge Left" TAB "Shift Left" TAB "GuiEditor.move( - $Cfg_GuiEditor_Snapping_snap2GridSize, 0 );";
	$LabMenuItemGui_[%id,%itemId++] = "Big Nudge Right" TAB "Shift Right" TAB "GuiEditor.move( $Cfg_GuiEditor_Snapping_snap2GridSize, 0 );";
	$LabMenuItemGui_[%id,%itemId++] = "Big Nudge Up" TAB "Shift Up" TAB "GuiEditor.move( 0, - $Cfg_GuiEditor_Snapping_snap2GridSize );";
	$LabMenuItemGui_[%id,%itemId++] = "Big Nudge Down" TAB "Shift Down" TAB "GuiEditor.move( 0, $Cfg_GuiEditor_Snapping_snap2GridSize );";
	%itemId = -1;
	$LabMenuGui_[%id++] = "Snap";
	$LabMenuItemGui_[%id,%itemId++] = "Snap Edges>>snapToEdges" TAB "Alt-Shift E" TAB "GuiEditor.toggleEdgeSnap();" TAB "GuiEditor.snapToEdges;";
	$LabMenuItemGui_[%id,%itemId++] = "Snap Centers>>snapToCenters" TAB "Alt-Shift C" TAB "GuiEditor.toggleCenterSnap();" TAB "GuiEditor.snapToCenters;";
	$LabMenuItemGui_[%id,%itemId++] = "-";
	$LabMenuItemGui_[%id,%itemId++] = "Snap to Guides>>snapToGuides" TAB "Alt-Shift G" TAB "GuiEditor.toggleGuideSnap();" TAB "GuiEditor.snapToGuides;";
	$LabMenuItemGui_[%id,%itemId++] = "Snap to Controls>>snapToControls" TAB "Alt-Shift T" TAB "GuiEditor.toggleControlSnap();" TAB "GuiEditor.snapToControls;";
	$LabMenuItemGui_[%id,%itemId++] = "Snap to Canvas>>snapToCanvas" TAB "" TAB "GuiEditor.toggleCanvasSnap();" TAB "GuiEditor.snapToCanvas;";
	$LabMenuItemGui_[%id,%itemId++] = "Snap to Grid>>snap2Grid" TAB "" TAB "GuiEditor.toggleGridSnap();" TAB "GuiEditor.snap2Grid;";
	$LabMenuItemGui_[%id,%itemId++] = "-";
	$LabMenuItemGui_[%id,%itemId++] = "Show Guides>>drawGuides" TAB "" TAB "GuiEditor.toggleDrawGuides();" TAB "GuiEditor.drawGuides;";
	$LabMenuItemGui_[%id,%itemId++] = "Full box selection>>fullBoxSelection" TAB "" TAB "GuiEditor.toggleFullBoxSelection();" TAB "GuiEditor.fullBoxSelection;";
	$LabMenuItemGui_[%id,%itemId++] = "Clear Guides" TAB "" TAB "GuiEditor.clearGuides();";
	%itemId = -1;
	$LabMenuGui_[%id++] = "Lab Menu";
	$LabMenuItemGui_[%id,%itemId++] = "-> General Editors GUIs <-";
	$LabMenuItemGui_[%id,%itemId++] = "Toggle Editors GUI listing" TAB "" TAB "Lab.toggleEditorGuiListing();";
	$LabMenuItemGui_[%id,%itemId++] = "Detach the Editor GUIs" TAB "" TAB "Lab.detachAllEditorGuis();";
	$LabMenuItemGui_[%id,%itemId++] = "Attach the Editor GUIs" TAB "" TAB "Lab.attachAllEditorGuis();";
	$LabMenuItemGui_[%id,%itemId++] = "Toggle Lab Editor Settings" TAB "" TAB "toggleDlg(LabEditorSettings);";
	$LabMenuItemGui_[%id,%itemId++] = "-> Special Editors GUIs <-";
	$LabMenuItemGui_[%id,%itemId++] = "Toggle Field Duplicator" TAB  "Alt-Shift D" TAB "Lab.toggleDuplicator();";
	$LabMenuItemGui_[%id,%itemId++] = "-> Special TorqueLab functions <-";
	$LabMenuItemGui_[%id,%itemId++] = "Save all toolbars" TAB  "" TAB "Lab.saveToolbar();";
	$LabMenuItemGui_[%id,%itemId++] = "-> Editing the GuiEditor GUI <-";
	$LabMenuItemGui_[%id,%itemId++] = "Clone GuiEditorGui" TAB  "" TAB "Lab.cloneGuiEditor();";
	$LabMenuItemGui_[%id,%itemId++] = "Apply GuiEditorGui Clone" TAB  "" TAB "Lab.convertClonedGuiEditor();";
	$LabMenuItemGui_[%id,%itemId++] = "Dump GUI TextIds" TAB  "" TAB "getGuiTextIds(GuiEd.lastGuiLoaded);";
	%itemId = -1;
	$LabMenuGui_[%id++] = "Help";
	$LabMenuItemGui_[%id,%itemId++] = "Online Documentation..." TAB "Alt F1" TAB "gotoWebPage( GuiEditor.documentationURL );";
	$LabMenuItemGui_[%id,%itemId++] = "Offline User Guid..." TAB "" TAB "gotoWebPage( GuiEditor.documentationLocal );";
	$LabMenuItemGui_[%id,%itemId++] = "Offline Reference Guide..." TAB "" TAB "shellExecute( GuiEditor.documentationReference );";
	$LabMenuItemGui_[%id,%itemId++] = "-";
	$LabMenuItemGui_[%id,%itemId++] = "Torque 3D Public Forums..." TAB "" TAB "gotoWebPage( \"http://www.garagegames.com/community/forums/73\" );";
	$LabMenuItemGui_[%id,%itemId++] = "Torque 3D Private Forums..." TAB "" TAB "gotoWebPage( \"http://www.garagegames.com/community/forums/63\" );";
}
//------------------------------------------------------------------------------
