//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
function NavEditorConsoleDlg::onWake(%this) {
	%this.forceInsideCtrl(NavEditorGui);
}
function NavEditorConsoleDlg::init(%this) {
	%this.AlignCtrlToParent("top");
	%this.AlignCtrlToParent("right");
}
//==============================================================================
// Plugin Object Params - Used set default settings and build plugins options GUI
//==============================================================================

if (!isObject(NavEditorConsoleListener)) {
	new ScriptMsgListener(NavEditorConsoleListener);
	getNavMeshEventManager().subscribe(NavEditorConsoleListener, "NavMeshCreated");
	getNavMeshEventManager().subscribe(NavEditorConsoleListener, "NavMeshRemoved");
	getNavMeshEventManager().subscribe(NavEditorConsoleListener, "NavMeshStartUpdate");
	getNavMeshEventManager().subscribe(NavEditorConsoleListener, "NavMeshUpdate");
	getNavMeshEventManager().subscribe(NavEditorConsoleListener, "NavMeshTileUpdate");
}

function NavEditorConsoleListener::onNavMeshCreated(%this, %data) {
}

function NavEditorConsoleListener::onNavMeshRemoved(%this, %data) {
}

function NavEditorConsoleListener::onNavMeshStartUpdate(%this, %data) {
	NavEditorConsoleDlg-->Output.clearItems();
	NavEditorConsoleDlg-->Output.addItem("Build starting for NavMesh" SPC %data, "0 0.6 0");
	NavEditorConsoleDlg-->OutputScroll.scrollToBottom();
}

function NavEditorConsoleListener::onNavMeshUpdate(%this, %data) {
	%message = "";

	if(getWordCount(%data) == 2) {
		%seconds = getWord(%data, 1);
		%minutes = mFloor(%seconds / 60);
		%seconds -= %minutes * 60;
		%message = "Built NavMesh" SPC getWord(%data, 0) SPC "in" SPC %minutes @ "m" SPC mRound(%seconds) @ "s";

		if(NavEditorGui.playSoundWhenDone) {
			sfxPlayOnce(Audio2D, "tlab/navEditor/assets/done.wav");
		}
	} else {
		%message = "Loaded NavMesh" SPC %data;
	}

	NavEditorConsoleDlg-->Output.addItem(%message, "0 0.6 0");
	NavEditorConsoleDlg-->OutputScroll.scrollToBottom();
	NavEditorConsoleDlg->StatusLeft.setText("");
}

function NavEditorConsoleListener::onNavMeshTileUpdate(%this, %data) {
	%mesh = getWord(%data, 0);
	%index = getWord(%data, 1);
	%total = getWord(%data, 2);
	%tile = getWords(%data, 3, 4);
	%success = getWord(%data, 5) == "1";

	if(!%success) {
		%message = "NavMesh" SPC %mesh SPC "tile" SPC %tile SPC "build failed!";
		NavEditorConsoleDlg-->Output.addItem(%message, "1 0 0");
		NavEditorConsoleDlg-->OutputScroll.scrollToBottom();
	}

	%percent = %index / %total * 100;
	NavEditorConsoleDlg->StatusLeft.setText("Build progress:" SPC mRound(%percent) @ "%");
}
