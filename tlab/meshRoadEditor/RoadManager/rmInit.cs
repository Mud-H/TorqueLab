//==============================================================================
// TorqueLab -> MeshRoadEditor - Road Manager Init
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$MREP_UpdateLinkedNodes = true;
//==============================================================================
// TerrainObject Functions
//==============================================================================
function MRoadManager::Init(%this) {
	MREP_NodeListModeMenu.clear();
	MREP_NodeListModeMenu.add("Link only",0);
	MREP_NodeListModeMenu.add("Width & Depth",1);
	MREP_NodeListModeMenu.add("Full edit",2);
	MREP_NodeListModeMenu.setSelected(0);
	MRoadManager.autoCollapsePill = true;
	MRoadManager.updateRoadData();
	hide(REP_NodePillSampleStack);
}

function MREP_NodeListModeMenu::onSelect(%this,%id,%text) {
	MRoadManager.setNodeListModeId(%id);
}


function MRoadManager::setNodeListModeId(%this,%id) {
	MRoadManager.nodeListModeId = %id;

	if (isObject(MRoadManager.currentRoad))
		%this.updateRoadData();
}


function MRoadManager::linkAll(%this,%linked) {
	if (!%linked)
		MRoadManager.linkedList = "";

	foreach(%pill in MREP_NodePillStack) {
		%pill.linkCheck.setStateOn(%linked);

		if (%linked)
			MRoadManager.linkedList = strAddWord(MRoadManager.linkedList,%pill.nodeId,true);
	}
}

function MRoadManager::linkInvert(%this) {
	%startList = MRoadManager.linkedList;

	foreach(%pill in MREP_NodePillStack) {
		%linked = %pill.linkCheck.isStateOn();
		%pill.linkCheck.setStateOn(!%linked);

		if (!%linked)
			MRoadManager.linkedList = strAddWord(MRoadManager.linkedList,%pill.nodeId,true);
		else
			MRoadManager.linkedList = strRemoveWord(MRoadManager.linkedList,%pill.nodeId);
	}
}

function MREP_LinkNodeCheck::onClick(%this) {
	%linked = %this.isStateOn();
	//%this.setStateOn(!%linked);
	%pill = %this.pill;

	if (%linked)
		MRoadManager.linkedList = strAddWord(MRoadManager.linkedList,%pill.nodeId,true);
	else
		MRoadManager.linkedList = strRemoveWord(MRoadManager.linkedList,%pill.nodeId);
}

function MRoadManager::updateLinkList(%this) {
	MRoadManager.linkedList = "";

	foreach(%pill in MREP_NodePillStack) {
		if (%pill.linkCheck.isStateOn())
			MRoadManager.linkedList = strAddWord(MRoadManager.linkedList,%pill.nodeId,true);
	}
}
