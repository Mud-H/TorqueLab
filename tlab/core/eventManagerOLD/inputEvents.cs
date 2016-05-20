//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//-
//- Returns Projects API's EventManager Singleton
//-
function Input::GetEventManager() {
	if( !isObject( $_Lab::InputEventManager ) )
		$_Lab::InputEventManager = new EventManager() {
		queue = "InputEventManager";
	};

	return $_Lab::InputEventManager;
}
exec("tlab/core/eventManager/dragDropEvents.cs");
exec("tlab/core/eventManager/applicationEvents.cs");