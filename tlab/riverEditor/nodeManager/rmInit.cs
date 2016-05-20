//==============================================================================
// TorqueLab -> MeshRiverEditor - River Manager Init
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$RiverEd_UpdateLinkedNodes = true;
//==============================================================================
// TerrainObject Functions
//==============================================================================
function RiverManager::Init(%this) {
}


//==============================================================================
// Node linking functions
//==============================================================================
function RiverManager::linkAll(%this,%linked) {
	if (!%linked)
		RiverManager.linkedList = "";

	foreach(%pill in RiverEd_NodePillStack) {
		%pill.linkCheck.setStateOn(%linked);

		if (%linked)
			RiverManager.linkedList = strAddWord(RiverManager.linkedList,%pill.nodeId,true);
	}
}

function RiverManager::linkInvert(%this) {
	%startList = RiverManager.linkedList;

	foreach(%pill in RiverEd_NodePillStack) {
		%linked = %pill.linkCheck.isStateOn();
		%pill.linkCheck.setStateOn(!%linked);

		if (!%linked)
			RiverManager.linkedList = strAddWord(RiverManager.linkedList,%pill.nodeId,true);
		else
			RiverManager.linkedList = strRemoveWord(RiverManager.linkedList,%pill.nodeId);
	}
}

function RiverEd_LinkNodeCheck::onClick(%this) {
	%linked = %this.isStateOn();
	//%this.setStateOn(!%linked);
	%pill = %this.pill;

	if (%linked)
		RiverManager.linkedList = strAddWord(RiverManager.linkedList,%pill.nodeId,true);
	else
		RiverManager.linkedList = strRemoveWord(RiverManager.linkedList,%pill.nodeId);
}

function RiverManager::updateLinkList(%this) {
	RiverManager.linkedList = "";

	foreach(%pill in RiverEd_NodePillStack) {
		if (%pill.linkCheck.isStateOn())
			RiverManager.linkedList = strAddWord(RiverManager.linkedList,%pill.nodeId,true);
	}
}
