//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$DATABLOCK_EDITOR_DEFAULT_FILENAME = "art/datablocks/managedDatablocks.cs";
return;
//---------------------------------------------------------------------------------------------
$TLab_PluginName_["DatablockEditor"] = "Datablock Editor";
function initDatablockEditor() {
	info( "TorqueLab","->","Initializing Datablock Editor" );
	$DbEd = newScriptObject("DbEd");
	execDBEd(true);
	// Add ourselves to EditorGui, where all the other tools reside
	//Lab.createPlugin("DatablockEditor");
	Lab.addPluginGui("DatablockEditor",DatablockEditorTools);
	DatablockEditorPlugin.superClass = "WEditorPlugin";
	DatablockEditorPlugin.customPalette = "SceneEditorPalette";
	new SimSet( UnlistedDatablocks );
	// create our persistence manager
	DatablockEditorPlugin.PM = new PersistenceManager();
	%map = new ActionMap();
	%map.bindCmd( keyboard, "backspace", "DatablockEditorPlugin.onDeleteKey();", "" );
	%map.bindCmd( keyboard, "delete", "DatablockEditorPlugin.onDeleteKey();", "" );
	DatablockEditorPlugin.map = %map;
	// DatablockEditorPlugin.initSettings();
}


function execDBEd(%loadGui) {
	if (%loadGui) {
		exec("tlab/datablockEditor/gui/DatablockEditorTools.gui");
		exec("tlab/datablockEditor/gui/DatablockEditorCreatePrompt.gui");
	}

	exec( "tlab/datablockEditor/DatablockEditorPlugin.cs" );
	exec( "tlab/datablockEditor/DatablockEditorParams.cs" );
	execPattern("tlab/datablockEditor/scripts/*.cs" );
	execPattern("tlab/datablockEditor/editor/*.cs" );
}

//---------------------------------------------------------------------------------------------

function destroyDatablockEditor() {
}
