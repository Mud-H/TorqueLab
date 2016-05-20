//==============================================================================
// TorqueLab -> MeshRoadEditor - Road Manager Init
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$REP_UpdateLinkedNodes = true;
//==============================================================================
// TerrainObject Functions
//==============================================================================
function RoadManager::Init(%this) {
}


//==============================================================================
// Node linking functions
//==============================================================================
function RoadManager::linkAll(%this,%linked) {
	if (!%linked)
		RoadManager.linkedList = "";

	foreach(%pill in REP_NodePillStack) {
		%pill.linkCheck.setStateOn(%linked);

		if (%linked)
			RoadManager.linkedList = strAddWord(RoadManager.linkedList,%pill.nodeId,true);
	}
}

function RoadManager::linkInvert(%this) {
	%startList = RoadManager.linkedList;

	foreach(%pill in REP_NodePillStack) {
		%linked = %pill.linkCheck.isStateOn();
		%pill.linkCheck.setStateOn(!%linked);

		if (!%linked)
			RoadManager.linkedList = strAddWord(RoadManager.linkedList,%pill.nodeId,true);
		else
			RoadManager.linkedList = strRemoveWord(RoadManager.linkedList,%pill.nodeId);
	}
}

function REP_LinkNodeCheck::onClick(%this) {
	%linked = %this.isStateOn();
	//%this.setStateOn(!%linked);
	%pill = %this.pill;

	if (%linked)
		RoadManager.linkedList = strAddWord(RoadManager.linkedList,%pill.nodeId,true);
	else
		RoadManager.linkedList = strRemoveWord(RoadManager.linkedList,%pill.nodeId);
}

function RoadManager::updateLinkList(%this) {
	RoadManager.linkedList = "";

	foreach(%pill in REP_NodePillStack) {
		if (%pill.linkCheck.isStateOn())
			RoadManager.linkedList = strAddWord(RoadManager.linkedList,%pill.nodeId,true);
	}
}
