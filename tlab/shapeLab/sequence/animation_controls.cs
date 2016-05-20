//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// Scene Editor Params - Used set default settings and build plugins options GUI
//==============================================================================
function ShapeLabPreview::onShow( %this ) {
	ShapeLabPreview.fitIntoParents("width");
	ShapeLabPreview.AlignCtrlToParent("bottom");
}


function ShapeLabPlugin::toggleAnimBar(%this) {
	ShapeLabDialogs.toggleDlg("AnimBar","",true);
	ShapeLabToolbar-->showAnimBar.setStateOn(ShapeLabDialogs-->AnimBar.isVisible());
}
//ShapeLabPlugin.updateAnimBar();
function ShapeLabPlugin::updateAnimBar(%this) {
	%stateOn = ShapeLabPreview.isVisible();
	//FIXME Hack : hide and show to fix container rendering issue of unknown cause
	hide(ShapeLabPreview);

	if (%stateOn)
		show(ShapeLabPreview);

	ShapeLabToolbar-->showAnimBar.setStateOn(%stateOn);
}
