//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// Manage Plugins Dialogs
//==============================================================================





//==============================================================================
function scanZones( %this, %type ) {
	
	%zoneList = getMissionObjectClassList("Zone Portal OcculsionVolume");
	devLog("Zones:",%zoneList);
	foreach$(%zone in %zoneList){
		if (!isObject(%zone))
			continue;	
			devLog("Details:",getZoneDetail(%zone));
		%class = %zone.getClassName();
		%zoneClassList = strAddWord(%zoneClassList,%class,1);
		%zoneList[%class] = strAddWord(%zoneList[%class],%zone,1);
	}
	foreach$(%class in %zoneClassList){
		devLog("//========================================================");
		devLog("Found",getWordCount(%zoneList[%class]),"object of class:",%class);
		devLog("List",%zoneList[%class]);
		
	}
}
//------------------------------------------------------------------------------//==============================================================================
function getZoneDetail(  %object ) {
	%detail = %object.getId() @ "\t\c1 "@  %object.getClassName() @ "\t\c2 "@ %object.hidden @ "\t\c3 "@ %object.position @ "\t\c4 "@ %object.scale;
	return %detail;
	
}
//------------------------------------------------------------------------------