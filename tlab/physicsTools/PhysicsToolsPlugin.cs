//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

function PhysicsEditorPlugin::onPluginLoaded( %this ) {	
	if ($Cfg_UI_Menu_UseNativeMenu) {
		new PopupMenu( PhysicsToolsMenu ) {
			superClass = "MenuBuilder";
			//class = "PhysXToolsMenu";
			barTitle = "Physics";
			item[0] = "Start Simulation" TAB "Ctrl-Alt P" TAB "physicsStartSimulation( \"client\" );physicsStartSimulation( \"server\" );";
			//item[1] = "Stop Simulation" TAB "" TAB "physicsSetTimeScale( 0 );";
			item[1] = "-";
			item[2] = "Speed 25%" TAB "" TAB "physicsSetTimeScale( 0.25 );";
			item[3] = "Speed 50%" TAB "" TAB "physicsSetTimeScale( 0.5 );";
			item[4] = "Speed 100%" TAB "" TAB "physicsSetTimeScale( 1.0 );";
			item[5] = "-";
			item[6] = "Reload NXBs" TAB "" TAB "";
		};
		// Add our menu.
		Lab.menuBar.insert( PhysicsToolsMenu, Lab.menuBar.dynamicItemInsertPos );
	}

	// Add ourselves to the window menu.
	//EditorGui.addToWindowMenu( "Road and Path Editor", "", "RoadEditor" );
}

function PhysicsToolsMenu::onMenuSelect(%this) {
	%isEnabled = physicsSimulationEnabled();
	%itemText = !%isEnabled ? "Start Simulation" : "Pause Simulation";
	%itemCommand = !%isEnabled ? "physicsStartSimulation( \"client\" );physicsStartSimulation( \"server\" );" : "physicsStopSimulation( \"client\" );physicsStopSimulation( \"server\" );";
	%this.setItemName( 0, %itemText );
	%this.setItemCommand( 0, %itemCommand );
}

function PhysicsEditorPlugin::onEditorWake( %this ) {
	// Disable physics when entering
	// the editor.  Will be re-enabled
	// when the editor is closed.
	physicsStopSimulation( "client" );
	physicsStopSimulation( "server" );
	physicsRestoreState();
}

function PhysicsEditorPlugin::onEditorSleep( %this ) {
	physicsStoreState();
	%currentTimeScale = physicsGetTimeScale();

	if ( %currentTimeScale == 0.0 )
		physicsSetTimeScale( 1.0 );

	Game.physicsStartSimulation( "client" );
	Game.physicsStartSimulation( "server" );
}
