//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$TLab_PluginName_["DecalEditor"] = "Decal Editor";

function initConvexEditor() {
	info( "TorqueLab","->","Initializing Sketch Tool");
	exec( "./gui/convexEditorGui.gui" );
	exec( "./gui/ConvexEditorTools.gui" );
	exec( "./gui/convexEditorToolbar.gui" );
	exec( "./gui/convexEditorPalette.gui" );
	exec( "./convexEditorGui.cs" );
	exec( "tlab/convexEditor/ConvexEditorPlugin.cs" );
	exec( "tlab/convexEditor/ConvexEditorParams.cs" );
	//Lab.createPlugin("ConvexEditor");
	Lab.addPluginEditor("ConvexEditor",ConvexEditorGui);
	Lab.addPluginGui("ConvexEditor",    ConvexEditorTools);
	//Lab.addPluginGui("ConvexEditor",   ConvexEditorOptionsWindow);
	//Lab.addPluginGui("ConvexEditor",   ConvexEditorTreeWindow);
	Lab.addPluginToolbar("ConvexEditor",ConvexEditorToolbar);
	Lab.addPluginPalette("ConvexEditor",   ConvexEditorPalette);
	ConvexEditorPlugin.editorGui = ConvexEditorGui;
	// Note that we use the WorldEditor's Toolbar.
	%map = new ActionMap();
	%map.bindCmd( keyboard, "1", "ConvexEditorNoneModeBtn.performClick();", "" );  // Select
	%map.bindCmd( keyboard, "2", "ConvexEditorMoveModeBtn.performClick();", "" );  // Move
	%map.bindCmd( keyboard, "3", "ConvexEditorRotateModeBtn.performClick();", "" );// Rotate
	%map.bindCmd( keyboard, "4", "ConvexEditorScaleModeBtn.performClick();", "" ); // Scale
	ConvexEditorPlugin.map = %map;
	// ConvexEditorPlugin.initSettings();
}
