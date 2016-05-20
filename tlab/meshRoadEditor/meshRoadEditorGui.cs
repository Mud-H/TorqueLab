//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
$MeshRoad::wireframe = true;
$MeshRoad::showSpline = true;
$MeshRoad::showReflectPlane = false;
$MeshRoad::showRoad = true;
$MeshRoad::breakAngle = 3.0;


//==============================================================================
function MeshRoadEditorGui::prepSelectionMode( %this ) {
	%mode = %this.getMode();

	if ( %mode $= "MeshRoadEditorAddNodeMode"  ) {
		if ( isObject( %this.getSelectedRoad() ) )
			%this.deleteNode();
	}

	%this.setMode( "MeshRoadEditorSelectMode" );
	LabPaletteArray-->MeshRoadEditorSelectMode.setStateOn(1);
}
//------------------------------------------------------------------------------
//==============================================================================
function EMeshRoadEditorSelectModeBtn::onClick(%this) {
	EditorGuiStatusBar.setInfo(%this.ToolTip);
}
//------------------------------------------------------------------------------
//==============================================================================
function EMeshRoadEditorAddModeBtn::onClick(%this) {
	EditorGuiStatusBar.setInfo(%this.ToolTip);
}
//------------------------------------------------------------------------------
//==============================================================================
function EMeshRoadEditorMoveModeBtn::onClick(%this) {
	EditorGuiStatusBar.setInfo(%this.ToolTip);
}
//------------------------------------------------------------------------------
//==============================================================================
function EMeshRoadEditorRotateModeBtn::onClick(%this) {
	EditorGuiStatusBar.setInfo(%this.ToolTip);
}
//------------------------------------------------------------------------------
//==============================================================================
function EMeshRoadEditorScaleModeBtn::onClick(%this) {
	EditorGuiStatusBar.setInfo(%this.ToolTip);
}
//------------------------------------------------------------------------------
//==============================================================================
function EMeshRoadEditorInsertModeBtn::onClick(%this) {
	EditorGuiStatusBar.setInfo(%this.ToolTip);
}
//------------------------------------------------------------------------------
//==============================================================================
function EMeshRoadEditorRemoveModeBtn::onClick(%this) {
	EditorGuiStatusBar.setInfo(%this.ToolTip);
}
//------------------------------------------------------------------------------
//==============================================================================
function MeshRoadDefaultWidthSliderCtrlContainer::onWake(%this) {
	MeshRoadDefaultWidthSliderCtrlContainer-->slider.setValue(MeshRoadDefaultWidthTextEditContainer-->textEdit.getText());
}
//------------------------------------------------------------------------------
//==============================================================================
function MeshRoadDefaultDepthSliderCtrlContainer::onWake(%this) {
	MeshRoadDefaultDepthSliderCtrlContainer-->slider.setValue(MeshRoadDefaultDepthTextEditContainer-->textEdit.getText());
}
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
function MeshRoadEditorGui::setDefaultTopMaterial(%this,%matName) {
	MeshRoadEditorGui.setFieldValue("topMaterialName",%matName);
}
//------------------------------------------------------------------------------
//==============================================================================
function MeshRoadEditorGui::setDefaultBottomMaterial(%this,%matName) {
	MeshRoadEditorGui.setFieldValue("bottomMaterialName",%matName);
}
//------------------------------------------------------------------------------
//==============================================================================
function MeshRoadEditorGui::setDefaultSideMaterial(%this,%matName) {
	MeshRoadEditorGui.setFieldValue("sideMaterialName",%matName);
}
//------------------------------------------------------------------------------