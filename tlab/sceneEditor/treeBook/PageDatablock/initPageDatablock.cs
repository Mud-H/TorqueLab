//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
function SceneEd::initPageDatablock( %this ) {
   if (!isObject(SceneEd_DBPM))
      new PersistenceManager(LabPM_Datablock);
   SceneEd.DBPM = SceneEd_DBPM;
   
   SEP_DatablockPage-->SaveDirtyDatablock.active = 0;
}
//------------------------------------------------------------------------------

//==============================================================================
function DbEd::setFilters( %this,%id ) {
	switch$(%id) {
	case "":
		DatablockEditorInspector.groupFilters = "";

	case "0":
		DatablockEditorInspector.groupFilters = "+Scripting -Object";

	case "1":
		DatablockEditorInspector.groupFilters = "-Scripting +Object";

	case "2":
		DatablockEditorInspector.groupFilters = "-Scripting +Object";
	}

	DatablockEditorInspector.refresh();
}
//------------------------------------------------------------------------------
