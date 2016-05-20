//==============================================================================
// TorqueLab -> MeshRoadEditor - Road Manager - NodeData
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// Custom PhysicsTools Functions
//==============================================================================

//==============================================================================
function Lab::togglePhysicToolbar(%this,%val) {
	if (%val $="")
		%val = !PhysicsToolsToolbar.visible;
	PhysicsToolsToolbar.visible = %val;	
}
//------------------------------------------------------------------------------
//==============================================================================
function togglePhysicToolbar(%val) {
	Lab.togglePhysicToolbar(%val);
	
}
//------------------------------------------------------------------------------
//==============================================================================
// Custom PhysicsTools Functions
//==============================================================================
function PT::physicsToggleSimulation(%this) {
	%isEnabled = physicsSimulationEnabled();

	if ( %isEnabled ) {
		physicsStateText.setText( "Simulation is paused." );
		PT.physicsStopSimulation( "client" );
		PT.physicsStopSimulation( "server" );
	} else {
		physicsStateText.setText( "Simulation is unpaused." );
		PT.physicsStartSimulation( "client" );
		PT.physicsStartSimulation( "server" );
	}
}
//------------------------------------------------------------------------------

//==============================================================================
// Custom PhysicsTools Functions
//==============================================================================
function PT::setInitialPhysicsState(%this,%startDelay) {
	%isEnabled = physicsSimulationEnabled();

	if ( %isEnabled ) {
		//	physicsStateText.setText( "Simulation is paused." );
		PT.physicsStopSimulation( "client" );
		PT.physicsStopSimulation( "server" );
	}

	PT.physicsRestoreState();

	if (%startDelay $= "")
		return;

	%this.schedule(%startDelay,"doAction","start");
}
//------------------------------------------------------------------------------

//==============================================================================
// Custom PhysicsTools Functions
//==============================================================================
function PT::doAction(%this,%action) {
	%isEnabled = physicsSimulationEnabled();

	switch$(%action) {
	case "start":
		PT.physicsSetTimeScale(1);

		if (%isEnabled)
			return;

		PT.physicsStartSimulation( "client" );
		PT.physicsStartSimulation( "server" );

	case "pause":
		PT.physicsSetTimeScale(0);

	case "stop":
		if (!%isEnabled)
			return;

		PT.physicsStopSimulation( "client" );
		PT.physicsStopSimulation( "server" );

	case "restart":
		PT.setInitialPhysicsState(500);

	case "store":
		PT.physicsStoreState();

	case "refresh":
		PT.setInitialPhysicsState();
	}
}
//------------------------------------------------------------------------------
function PhysicsToolsIcon::onClick(%this) {
	%action = %this.internalName;
	PT.doAction(%action);
}
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
function PhysicsToolsSlider::doCommand(%this) {
	%action = %this.internalName;

	switch$(%action) {
	case "timeScale":
		%scale = mFloatLength(%this.getValue(),1);
		PT.physicsSetTimeScale(%scale);
	}
}
//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
function PhysicsToolsSlider::doAltCommand(%this) {
	%action = %this.internalName;

	switch$(%action) {
	case "timeScale":
		%scale = mFloatLength(%this.getValue(),1);
		PT.physicsSetTimeScale(%scale);
	}
}
//------------------------------------------------------------------------------
//==============================================================================
// Physics Plugin Functions
//==============================================================================
//==============================================================================
//Returns true if a physics plugin exists and is initialized
function PT::physicsPluginPresent(%this) {
	return physicsPluginPresent();
}
//------------------------------------------------------------------------------
//==============================================================================
//Returns true if a physics plugin exists and is initialized
function PT::physicsInit(%this,%library) {
	return physicsInit(%library);
}
//------------------------------------------------------------------------------
//==============================================================================
//Returns true if a physics plugin exists and is initialized
function PT::physicsDestroy(%this) {
	physicsDestroy();
}
//------------------------------------------------------------------------------

//==============================================================================
// Physics World Functions
//==============================================================================

//==============================================================================
//Returns true if a physics plugin exists and is initialized
function PT::physicsInitWorld(%this,%worldName) {
	return physicsInitWorld(%worldName);
}
//------------------------------------------------------------------------------
//==============================================================================
//Returns true if a physics plugin exists and is initialized
function PT::physicsDestroyWorld(%this,%worldName) {
	physicsDestroyWorld(%worldName);
}
//------------------------------------------------------------------------------

//==============================================================================
// Physics Simulation Functions
//==============================================================================

//==============================================================================
//Returns true if a physics plugin exists and is initialized
function PT::physicsStartSimulation(%this,%worldName) {
	physicsStartSimulation(%worldName);
}
//------------------------------------------------------------------------------
//==============================================================================
//Returns true if a physics plugin exists and is initialized
function PT::physicsStopSimulation(%this,%worldName) {
	physicsStopSimulation(%worldName);
}
//------------------------------------------------------------------------------
//==============================================================================
//Returns true if a physics plugin exists and is initialized
function PT::physicsSimulationEnabled(%this,%worldName) {
	return physicsSimulationEnabled(%worldName);
}
//------------------------------------------------------------------------------

//==============================================================================
// Physics TimeScale Functions
//==============================================================================
//==============================================================================
// Used for slowing down time on the physics simulation, and for pausing/restarting the simulation.
function PT::physicsSetTimeScale(%this,%scale) {
	logd("PT::physicsSetTimeScale(%this,%scale)",%scale);
	physicsSetTimeScale(%scale);
}
//------------------------------------------------------------------------------

//==============================================================================
// Get the currently set time scale.
function PT::physicsGetTimeScale(%this,%scale) {
	return physicsGetTimeScale();
}
//------------------------------------------------------------------------------

//==============================================================================
// Physics State Functions
//==============================================================================

//==============================================================================
// Used to send a signal to objects in the
// physics simulation that they should store
// their current state for later restoration,
// such as when the editor is closed.
function PT::physicsStoreState(%this) {
	physicsStoreState();
}
//------------------------------------------------------------------------------
//==============================================================================
// Used to send a signal to objects in the
// physics simulation that they should restore
// their saved state, such as when the editor is opened.
function PT::physicsRestoreState(%this) {
	physicsRestoreState();
}
//------------------------------------------------------------------------------

//==============================================================================
// Physics Debug Functions
//==============================================================================
//==============================================================================
// Get the currently set time scale.
function PT::physicsDebugDraw(%this,%enable) {
	physicsDebugDraw(%enable);
}
//------------------------------------------------------------------------------

