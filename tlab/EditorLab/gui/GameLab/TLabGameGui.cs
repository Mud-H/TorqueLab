//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$LabGameMap.bindCmd(keyboard, "ctrl 0", "TLabGameGui.toggleMe();");
$LabGameMap.bindCmd(keyboard, "ctrl i", "TLabGameGui.toggleCursor();");
$LabGameMap.bindCmd(keyboard, "ctrl m", "Lab.toggleGameDlg(\"SceneEditorDialogs\",\"AmbientManager\");");
//==============================================================================
function Lab::initGameLabDialogs( %this ) {
	TLabGameGui.reset();
	%mainGui = TLabGameGui;
	%menu = %mainGui-->dialogMenu;
	%menu.clear();
	%menu.add("Select a dialog" ,0);

	foreach(%gui in LabDialogGuiSet) {
		foreach(%dlg in %gui) {
			if (%dlg.gameName $="")
				continue;

			%menu.add(%dlg.gameName ,%dlg.getId());
		}
	}

	%menu.setSelected(0);
}
//------------------------------------------------------------------------------

//==============================================================================
function TLabGameGui::onWake( %this ) {
	%menu = TLabGameGui-->dialogMenu;

	if (isObject(Lab.currentGameDlg))
		%menu.setSelected(Lab.currentGameDlg.getId(),false);
	else
		%menu.setSelected(0,false);
}

//------------------------------------------------------------------------------
//==============================================================================
function TLabGameGui::onSleep( %this ) {
	hideCursor();
	$LabGameMap.push();
	//Canvas.schedule(300,"hideCursor");
	//Canvas.schedule(300,"cursorOff");
}

//------------------------------------------------------------------------------


//==============================================================================
function TLabGameGui::toggleMe( %this ) {
	toggleDlg(TLabGameGui);
	//if (%this.isVisible())
	//	TLabGameGui.schedule(300,"toggleCursor",true);
}

//------------------------------------------------------------------------------

//==============================================================================
function TLabGameGuiDialogMenu::onSelect( %this,%id,%text ) {
	if (%id == 0) {
		TLabGameGui.reset();
		return;
	}

	TLabGameGui.showDlg(%id);
}
//------------------------------------------------------------------------------
//==============================================================================
// TLabGameGui -> Show Dialogs in game
//==============================================================================

//==============================================================================
function TLabGameGui::getDlgObject( %this,%dlg,%child ) {
	%dlgCtrl = %dlg;

	if (%child !$= "" && isObject(%dlg)) {
		%dlgCtrl = %dlg.findObjectByInternalName(%child);

		//If not to default parent try with TLabGameGui
		if (!isObject(%dlgCtrl))
			%dlgCtrl = %this.findObjectByInternalName(%child);
	}

	return %dlgCtrl;
}
//------------------------------------------------------------------------------
//==============================================================================
//Set a plugin as active (Selected Editor Plugin)
function Lab::toggleGameDlg(%this,%dlg,%child) {
	%dlgCtrl = TLabGameGui.getDlgObject(%dlg,%child );

	if (!isObject(%dlgCtrl))
		return;

	if (Lab.currentGameDlg $= %dlgCtrl)
		TLabGameGui.hideDlg(%dlg,%child);
	else
		TLabGameGui.showDlg(%dlg,%child);

	
}
//------------------------------------------------------------------------------


//==============================================================================
function TLabGameGui::showDlg( %this,%dlg,%child ) {
	%dlgCtrl = %this.getDlgObject(%dlg,%child );

	//If not to default parent try with TLabGameGui
	if (!isObject(%dlgCtrl)) {
		warnLog("Trying to show invalid GameLab Dialog:",%dlg,%child);
		return;
	}

	if (isObject(Lab.currentGameDlg)) {
		Lab.currentGameDlg.editorParent.add(Lab.currentGameDlg);
	}

	//if (%dlgCtrl.isMethod("onActivated"))
		//%dlgCtrl.onActivated();

	%dlgCtrl.editorParent = %dlgCtrl.parentGroup;
	%pluginName = %dlgCtrl.editorParent.pluginObj.displayName;
	TLabGameGui-->dialogTitle.text = %pluginName SPC "\c1->\c2" SPC %dlgCtrl.internalName;
	TLabGameGui.add(%dlgCtrl);
	Lab.currentGameDlg = %dlgCtrl;
	%dlgCtrl.visible = 1;
	%this.lastLoadedDlg = %dlg;
	pushDlg(TLabGameGui);
	//TLabGameGui.schedule(300,"toggleCursor",true);
	TLabGameGui.clearChildResponder();
}
//------------------------------------------------------------------------------
//==============================================================================
function TLabGameGui::hideDlg( %this,%dlg,%child ) {
	%dlgCtrl.editorParent.add(%dlgCtrl);
	%dlgCtrl.visible = 0;
	popDlg(TLabGameGui);
	Lab.currentGameDlg = "";
}
//------------------------------------------------------------------------------
//==============================================================================
function TLabGameGui::closeAll( %this ) {
	Lab.currentGameDlg = "";

	foreach(%ctrl in 	%this) {
		if (%ctrl.internalName $= "infoContainer")
			continue;

		if (isObject(%ctrl.editorParent)) {
			%ctrl.editorParent.add(%ctrl);
		}

		hide(%ctrl);
	}

	Canvas.cursorOff();
	popDlg(%this);
}
//------------------------------------------------------------------------------


//==============================================================================
function TLabGameGui::toggleCursor( %this,%show ) {
	%button = %this-->toggleCursorButton;
	%button.text = "Toggle cursor (ctrl + i)";
	%button.active = true;

	if (Canvas.isCursorOn() && !%show) {
		hideCursor();
	} else {
		showCursor();
	}

	if (!Canvas.isCursorOn()) {
		%this.clearChildResponder();
	}

	$HudCtrl.makeFirstResponder(!Canvas.isCursorOn());
	TLabGameGui.makeFirstResponder(Canvas.isCursorOn());
}
//------------------------------------------------------------------------------

//==============================================================================
function TLabGameGui::setNoCursor( %this,%noCursor ) {
	TLabGameGui.noCursor = %noCursor;
}
//------------------------------------------------------------------------------
//==============================================================================
function TLabGameGui::reset( %this ) {
	//Make sure all Dlg in TLabGameGui have been returned to editor
	foreach(%ctrl in TLabGameGui) {
		if (%ctrl.internalName $= "infoContainer")
			continue;

		if (isObject(%ctrl.editorParent)) {
			%ctrl.editorParent.add(%ctrl);
		}

		hide(%ctrl);
	}

	Lab.currentGameDlg = "";
}