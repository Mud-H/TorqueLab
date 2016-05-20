//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// SceneEditorTools Frame Set Scripts
//==============================================================================
//==============================================================================
// Handle a selection in the MissionGroup shape selector
function SLE_AnimOptionsBook::onTabSelected( %this, %text,%id ) {
	logd("SLE_AnimOptionsBook::onTabSelected( %this, %text,%id )", %this,%text,%id );
	ShapeLab.currentAnimOptionsPage = %id;
}
//------------------------------------------------------------------------------

function ShapeLabPlugin::toggleAdvancedOptions(%this) {
	ShapeLabDialogs.toggleDlg("Advanced","",true);
	ShapeLabToolbar-->showAdvanced.setStateOn(ShapeLabDialogs-->Advanced.isVisible());
}

function ShapeLabPlugin::initStatusBar(%this) {
	EditorGuiStatusBar.setInfo("Shape editor ( Shift Click ) to speed up camera.");
	EditorGuiStatusBar.setSelection( ShapeLab.shape.baseShape );
}

