//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$TLab_PluginName_["PhysicsTools"] = "Physics Tools";
$TLab_PluginType_["PhysicsTools"] = "Module";
function initPhysicsTools() {
	info( "TorqueLab","->","Initializing Physics Tools" );

	if ( !physicsPluginPresent() ) {
		echo( "No physics plugin exists." );
		return;
	}

	execPhysTools(true);
	$PT = newScriptObject("PT");
	//Lab.createModule("PhysicsTools","Physics Tools");
	Lab.addPluginToolbar("PhysicsTools",PhysicsToolsToolbar);
	globalactionmap.bindCmd( keyboard, "alt t", "PT.physicsToggleSimulation();", "" );
	globalactionmap.bindCmd( keyboard, "alt r", "PT.physicsRestoreState();", "" );
}
function execPhysTools(%loadGui) {
	if (%loadGui) {
		exec("tlab/physicsTools/gui/PhysicsToolsToolbar.gui");
	}

	exec("tlab/physicsTools/PhysicsToolsPlugin.cs");
	exec("tlab/physicsTools/physicsTools.cs");
}
function destroyPhysicsTools() {
}
