//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================


//==============================================================================
function Lab::initParamsSystem( %this ) {
	$LabParams = newScriptObject("LabParams");
	$LabParamsGroup = newSimGroup("LabParamsGroup");
}
//------------------------------------------------------------------------------


//==============================================================================
function LabParams::syncArray( %this,%paramArray,%syncTarget ) {
	for( ; %i < %paramArray.count() ; %i++) {
		%field = %paramArray.getKey(%i);
		%data = %paramArray.getValue(%i);
		%this.syncParamField(%paramArray,%field,%data,%syncTarget);
	}
}
//------------------------------------------------------------------------------


//==============================================================================
// Sync the current profile values into the params objects
function LabParams::syncParamField( %this,%paramArray,%field,%data,%syncTarget ) {
	%cfgObj = %paramArray.cfgObject;

	if (%paramArray.noSyncField[%field] )
		return;

	if (!%paramArray.prefModeOnly && isObject(%cfgObj)) {
		%value = %cfgObj.getCfg(%field);
	}

	if ( %value $= "") {
		%value = getParamValue(%paramArray,%field,true);
	}

	if ( %value $= "") {
		paramLog(%paramArray.getName(),"Cfg Value for field:",%field,"Is Blank!");
		return;
	}

	if (%syncTarget)
		%this.updateParamSyncData(%field,%value,%paramArray);

	%pill = %paramArray.pill[%field];

	if (isObject(%pill)) {
		%pillHolder = %pill.findObjectByInternalName(%field,true);

		if (isObject(%pillHolder))
			%pillHolder.setTypeValue(%value,true);
		else
			paramLog("No gui control found assigned to the field:",%field,"ParamObj:",%paramArray);
	} else {
		paramLog(%paramArray.getName(),"Invalid control holder for field:",%field,"Couldn't sync the param!");
	}
}
//------------------------------------------------------------------------------
//==============================================================================
// Sync the current profile values into the params objects
function LabParams::setParamPillValue( %this,%field,%value,%paramArray ) {
	%pill = %paramArray.pill[%field];

	if (isObject(%pill)) {
		%pillHolder = %pill.findObjectByInternalName(%field,true);
		%pillHolder.setTypeValue(%value,true);
	} else {
		paramLog(%paramArray.getName(),"Invalid control holder for field:",%field,"Couldn't sync the param!");
	}
}
//------------------------------------------------------------------------------

//==============================================================================
function LabParams::updateParamSyncData( %this,%field,%value,%paramArray ) {
}

//---------------------------------------------------------------------------

//------------------------------------------------------------------------------

//==============================================================================
// Update the Params after control value changed in ParamsDlg
//==============================================================================

//==============================================================================
function LabParams::updateParamArrayCtrl( %this,%field,%value,%ctrl,%paramArray,%arg1,%arg2 ) {
   paramLog("LabParams::updateParamArrayCtrl(", %this,%field,%value,%ctrl,%paramArray,%arg1,%arg2 );
    
   if (%paramArray.cfgData !$= ""){
      $Cfg_[%paramArray.cfgData,%field] = %value;          
	}
	
	if (%paramArray.prefModeOnly)
		return;

	%cfgObj = %paramArray.cfgObject;

	if (isObject(%cfgObj)) {
		%cfgObj.setCfg(%field,%value);
	} else {
		return false;
	}
}
//------------------------------------------------------------------------------
function LabParams::updateParamFromCtrl( %this,%ctrl,%field,%value,%paramArray ) {
	if (!%paramArray.prefModeOnly) {
		%cfgObj = %paramArray.cfgObject;

		if (isObject(%cfgObj)) {
			%cfgObj.setCfg(%field,%value);
		} else {
			return false;
		}
	}

	%ctrl.updateFriends();
	%this.updateParamSyncData(%field,%value,%paramArray);
	%this.setParamPillValue(%field,%value,%paramArray);
}
//------------------------------------------------------------------------------
