//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//-
//- Returns Projects API's EventManager Singleton
//-
function Projects::GetEventManager() {
	if( !isObject( $_Lab::ProjectEventManager ) )
		$_Lab::ProjectEventManager = new EventManager() {
		queue = "ProjectEventManager";
	};

	return $_Lab::ProjectEventManager;
}
exec("tlab/core/eventManager/projectBaseEvents.cs");
exec("tlab/core/eventManager/projectInternalInterface.cs");

