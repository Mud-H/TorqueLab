//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================


//==============================================================================
// Scene Editor Params - Used set default settings and build plugins options GUI
//==============================================================================
//==============================================================================
//%field,%value,%ctrl,%array,%arg1,%arg2
function DbEd::updateParam( %this,%field,%value,%ctrl,%array,%arg1,%arg2 ) {
	logd("DbEd::updateParam( %this,%field,%value,%ctrl,%array,%arg1,%arg2 )",%field,%value,%ctrl,%array,%arg1,%arg2);

	if (!isObject(%this.activeDatablock)) {
		%this.setSelectedDatablock();
		return;
	}

	DatablockEditorInspector.setObjectField( %field,%value );
}
//------------------------------------------------------------------------------

//==============================================================================
//%field,%value,%ctrl,%array,%arg1,%arg2
function DbEd::syncData( %this ) {
	logd("DbEd::syncData( %this )");
	syncParamArray(DbEd.currentParam);
}
//------------------------------------------------------------------------------
//==============================================================================
//%field,%value,%ctrl,%array,%arg1,%arg2
function DbEd::refreshData( %this ) {
	logd("DbEd::refreshData( %this )");
	DbEd.activeDatablock.reloadOnLocalClient();
}
//------------------------------------------------------------------------------
