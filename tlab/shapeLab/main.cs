//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$TLab_PluginName_["ShapeLab"] = "Shape Lab";
//------------------------------------------------------------------------------
// Shape Lab
//------------------------------------------------------------------------------

function initShapeLab() {
	info( "TorqueLab","->","Initializing Shape Lab");
	newScriptObject("ShapeLab");
	
	execShapeLab(true);
	//Lab.createPlugin("ShapeLab","Shape Lab");
	Lab.addPluginGui("ShapeLab",ShapeLabTools);
	Lab.addPluginEditor("ShapeLab",ShapeLabPreviewGui);
	//Lab.addPluginEditor("ShapeLab",ShapeLabPreview,true);
	Lab.addPluginToolbar("ShapeLab",ShapeLabToolbar);
	Lab.addPluginPalette("ShapeLab",   ShapeLabPalette);
	Lab.addPluginDlg("ShapeLab",   ShapeLabDialogs);
	
	ShapeLabPlugin.editorGui = ShapeLabShapeView;
	// Add windows to editor gui
	%map = new ActionMap();
	%map.bindCmd( keyboard, "escape", "LabPluginArray->SceneEditorPalette.performClick();", "" );
	%map.bindCmd( keyboard, "1", "ShapeLabNoneModeBtn.performClick();", "" );
	%map.bindCmd( keyboard, "2", "ShapeLabMoveModeBtn.performClick();", "" );
	%map.bindCmd( keyboard, "3", "ShapeLabRotateModeBtn.performClick();", "" );
	//%map.bindCmd( keyboard, "4", "ShapeLabScaleModeBtn.performClick();", "" ); // not needed for the shape editor
	%map.bindCmd( keyboard, "n", "ShapeLabToolbar->showNodes.performClick();", "" );
	%map.bindCmd( keyboard, "t", "ShapeLabToolbar->ghostMode.performClick();", "" );
	%map.bindCmd( keyboard, "r", "ShapeLabToolbar->wireframeMode.performClick();", "" );
	%map.bindCmd( keyboard, "f", "ShapeLabToolbar->fitToShapeBtn.performClick();", "" );
	%map.bindCmd( keyboard, "g", "ShapeLabToolbar->showGridBtn.performClick();", "" );
	%map.bindCmd( keyboard, "h", "ShapeLabPropWindow->tabBook.selectPage( 2 );", "" ); // Load help tab
	%map.bindCmd( keyboard, "l", "ShapeLabPropWindow->tabBook.selectPage( 1 );", "" ); // load Library Tab
	%map.bindCmd( keyboard, "j", "ShapeLabPropWindow->tabBook.selectPage( 0 );", "" ); // load scene object Tab
	%map.bindCmd( keyboard, "SPACE", "ShapeLabPreview.togglePause();", "" );
	%map.bindCmd( keyboard, "i", "ShapeLabSequences.onEditSeqInOut(\"in\", ShapeLabSeqSlider.getValue());", "" );
	%map.bindCmd( keyboard, "o", "ShapeLabSequences.onEditSeqInOut(\"out\", ShapeLabSeqSlider.getValue());", "" );
	%map.bindCmd( keyboard, "shift -", "ShapeLabSeqSlider.setValue(ShapeLabPreview-->seqIn.getText());", "" );
	%map.bindCmd( keyboard, "shift =", "ShapeLabSeqSlider.setValue(ShapeLabPreview-->seqOut.getText());", "" );
	%map.bindCmd( keyboard, "=", "ShapeLabPreview-->stepFwdBtn.performClick();", "" );
	%map.bindCmd( keyboard, "-", "ShapeLabPreview-->stepBkwdBtn.performClick();", "" );
	ShapeLabPlugin.map = %map;
	
	
	//ShapeLabPlugin.initSettings();
}
//==============================================================================
// Load the Scene Editor Plugin scripts, load Guis if %loadgui = true
function execShapeLab(%loadGui) {
	if (%loadGui) {
		exec("./gui/Profiles.cs");
		exec("tlab/shapeLab/gui/shapeLabPreviewWindow.gui");
		exec("tlab/shapeLab/gui/ShapeLabDialogs.gui");
		exec("tlab/shapeLab/gui/shapeLabToolbar.gui");
		exec("tlab/shapeLab/gui/shapeLabPalette.gui");
		exec("tlab/shapeLab/gui/ShapeLabTools.gui");
	}

	exec("./scripts/shapeLab.cs");
	exec("./scripts/shapeLabHints.cs");
	exec("./scripts/shapeLabActions.cs");
	exec("./scripts/shapeLabUtility.cs");
	exec("tlab/shapeLab/ShapeLabPlugin.cs");
	exec("tlab/shapeLab/ShapeLabTools.cs");

	execPattern("tlab/shapeLab/collision/*.cs");
	execPattern("tlab/shapeLab/editor/*.cs");
	execPattern("tlab/shapeLab/shape/*.cs");
	execPattern("tlab/shapeLab/sequence/*.cs");
	execPattern("tlab/shapeLab/node/*.cs");
	execPattern("tlab/shapeLab/detail/*.cs");

}
//------------------------------------------------------------------------------

function destroyShapeLab() {
}

function ShapeLabToggleButtonValue(%ctrl, %value) {
	if ( %ctrl.getValue() != %value )
		%ctrl.performClick();
}

function shapeLabWireframeMode() {
	devLog("shapeLabWireframeMode Ignored test");
	return;
	$gfx::wireframe = !$gfx::wireframe;
	ShapeLabToolbar-->wireframeMode.setStateOn($gfx::wireframe);
}
