//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$sepVM_MainBook_PageId = 0;
$sepVM_ApplyChangeToDatablock = false;
$sepVMDebug = false;
$sepVM_DataInitDone = false;

//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_VehicleManager::initManager( %this ) {
	devLog("SEP_VehicleManager::initManager");
sepVM_MainBook.selectPage($sepVM_MainBook_PageId);
	if ($sepVMDebug)
		return;

	if (!$sepVM_DataInitDone) {
		sepVM.initWheeledPage();
		sepVM.buildWheeledParams();
		sepVM.initPresetsData();
	}

//	if (!isObject(sepVM.selectedDatablock)){
	if (isObject(LocalClientConnection.vehicle)) {
		%vehicle = LocalClientConnection.vehicle;
		devLog("Auto selecting Client vehicle",%vehicle);
	} else if (EWorldEditor.getSelectionSize() > 0) {
		if (EWorldEditor.getSelectedObject(0).isMemberOfClass("Vehicle")) {
			%vehicle = EWorldEditor.getSelectedObject(0);
			devLog("Auto selecting selected scene vehicle",	%vehicle);
		}
	}

	if (isObject(%vehicle))
		sepVM.selectWheeledData(%vehicle.getDatablock(),"Vehicle");

///	}
	$sepVM_DataInitDone = false;
}
//------------------------------------------------------------------------------

//==============================================================================
// initDialog and OnActivated Callbacks
//==============================================================================
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_VehicleManager::checkState( %this ) {
	logd("SEP_VehicleManager::checkState");
	if (!$SEP_VehicleManager_Loaded)	
		%this.initDialog();	
	
	return $SEP_VehicleManager_Loaded;
}
//------------------------------------------------------------------------------

//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_VehicleManager::initDialog( %this ) {
	$SEP_VehicleManager_Loaded = true;
}
//------------------------------------------------------------------------------
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_VehicleManager::onActivated( %this ) {
	logd("SEP_VehicleManager::onActivated(%this)");
	return; //WIP Fast - Only load whe needed
	sepVM_MainBook.selectPage($sepVM_MainBook_PageId);
}
//------------------------------------------------------------------------------




//==============================================================================
// OnWake and OnSleep Callbacks
//==============================================================================
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_VehicleManager::onWake( %this ) {
	devLog("SEP_VehicleManager::onWake");

	if ($sepVMDebug)
		return;

	%this.initManager();
}
//------------------------------------------------------------------------------
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_VehicleManager::onSleep( %this ) {
	logd("SEP_VehicleManager::onSleep( %this )");
}
//------------------------------------------------------------------------------

//==============================================================================
// OnShow and OnHide Callbacks
//==============================================================================
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_VehicleManager::onShow( %this ) {
	devLog("SEP_VehicleManager::onShow");
	%this.initManager();
}
//------------------------------------------------------------------------------
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_VehicleManager::onHide( %this ) {
}
//------------------------------------------------------------------------------

//==============================================================================
// onPreEditorSave and onPostEditorSave Callbacks
//==============================================================================
//==============================================================================
function SEP_VehicleManager::onPreEditorSave(%this) {
	logd("SEP_VehicleManager::onPreEditorSave");
}
//------------------------------------------------------------------------------
//==============================================================================
function SEP_VehicleManager::onPostEditorSave(%this) {
	logd("SEP_VehicleManager::onPostEditorSave");
}
//------------------------------------------------------------------------------


//==============================================================================
// Main GUI Functions
//==============================================================================
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function seVM_MainBook::onTabSelected( %this,%text,%index ) {
	logd("seVM_MainBook::onTabSelected( %this,%text,%index )");
	$seVM_MainBook_PageId = %index;
}
//------------------------------------------------------------------------------
