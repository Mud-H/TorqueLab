//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
// Allow to manage different GUI Styles without conflict
//==============================================================================
//------------------------------------------------------------------------------
// WEditorPlugin
//------------------------------------------------------------------------------



function WEditorPlugin::onActivated( %this ) {
	EditorGui.bringToFront( EWorldEditor );
	EWorldEditor.setVisible(true);
	Lab.insertDynamicMenu(Lab.worldMenu);
	EWorldEditor.makeFirstResponder(true);
	SceneEditorTree.open(MissionGroup,true);
	EWorldEditor.syncGui();
	EditorGuiStatusBar.setSelectionObjectsByCount(EWorldEditor.getSelectionSize());

	// Should the Transform Selection window open?
	if( EWorldEditor.ETransformSelectionDisplayed ) {
		ETransformSelection.setVisible(true);
	}

	Parent::onActivated(%this);
}
function testMe( ) {
	devLog("Test");
}

function WEditorPlugin::onDeactivated( %this ) {
	// Hide the Transform Selection window from other editors
	//ETransformSelection.setVisible(false);
	EWorldEditor.setVisible( false );
	Lab.removeDynamicMenu(Lab.worldMenu);
	Parent::onDeactivated(%this);
}

