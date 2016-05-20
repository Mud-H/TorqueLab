//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
// Allow to manage different GUI Styles without conflict
//==============================================================================

$SceneEd_ModePage = 0;
$SceneEd_TreePage = 0;
function SceneEditorModeTab::onTabSelected( %this,%text,%index ) {
	$SceneEd_ModePage = %index;
}
function SceneEditorTreeTabBook::onTabSelected( %this,%text,%index ) {
	$SceneEd_TreePage = %index;
}


function SceneAddSimGroupButton::onClick( %this ) {
	devLog("SceneAddSimGroupButton::onClick??");
	EWorldEditor.addSimGroup();
}
function SceneAddSimGroupButton::onDefaultClick( %this ) {
	devLog("SceneAddSimGroupButton::onDefaultClick??");
	EWorldEditor.addSimGroup();
}

function SceneAddSimGroupButton::onCtrlClick( %this ) {
	devLog("SceneAddSimGroupButton::onCtrlClick??");
	EWorldEditor.addSimGroup( true );
}


function SceneEditorPlugin::toggleBuildMode( %this ) {
	%rowCount = getWordCount(SceneEditorTools.rows);

	if (%rowCount > 1) {
		SceneEditorTools.lastRows = SceneEditorTools.rows;
		SceneEditorTools.rows = "0";
		SceneEditorTools.updateSizes();
	} else {
		if (getWordCount(SceneEditorTools.lastRows) <= 1)
			SceneEditorTools.lastRows = "0 200";

		SceneEditorTools.rows = SceneEditorTools.lastRows;
		SceneEditorTools.updateSizes();
	}
}

function SceneEd_BuilderSelectableClassButton::onClick( %this ) {
	devLog("SceneEd_BuilderSelectableClassButton::onClick??");

	if (EVisibilityLayers.currentPresetFile $= "visBuilder") {
		EVisibilityLayers.loadPresetFile("default");
		%this.text = "Set TSStatic-Only Selectable";
		return;
	}

	EVisibilityLayers.loadPresetFile("visBuilder");
	%this.text = "Set back default selectable class";
}

