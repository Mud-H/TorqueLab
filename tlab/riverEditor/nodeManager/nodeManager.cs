//==============================================================================
// TorqueLab -> MeshRiverEditor - River Manager - NodeData
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//RiverManager.updateRiverData();
function RiverManager::updateRiverData(%this,%nodeId) {
	%River = RiverManager.currentRiver;

	if (!isObject(%River)) {
		RiverEd_NodePillStack.clear();
		return;
	}

	%startNode = RiverEditorGui.getSelectedNode();
	%this.noNodeUpdate = true;

	if (RiverManager.nodeListModeId $= "")
		RiverManager.nodeListModeId = "0";

	%id = 0;

	while( true ) {
		RiverEditorGui.setSelectedNode(%id);
		%nodeWidth = RiverEditorGui.getNodeWidth();

		if (%nodeWidth $= "" || %nodeWidth $= "-1")
			break;

		%nodePos = RiverEditorGui.getNodePosition();
		%nodeDepth = RiverEditorGui.getNodeDepth();
		RiverManager.nodeData[%id] = %nodePos TAB %nodeWidth TAB %nodeDepth;
		%id++;
	}

	RiverManager.nodeCount = %id;
	%this.updateNodeStack();
	%this.noNodeUpdate = false;

	//If an active node is specified, simply update the active node pill
	if (%nodeId !$= "") {
		if (%nodeId >= RiverManager.nodeCount)
			%nodeId = RiverManager.nodeCount-1;

		RiverManager.setNodePillActive(%nodeId);
		return;
	}

	if (%startNode < 0)
		%startNode = 0;

	if (%startNode >= RiverManager.nodeCount)
		%startNode = RiverManager.nodeCount-1;

	RiverManager.selectNode(%startNode);
}
//==============================================================================
// Select Node
//==============================================================================

//------------------------------------------------------------------------------
$NoNodeUpd = true;
function RiverManager::selectNode(%this,%nodeId,%noUpdate) {
	if (strFind("Insert Remove",RiverManager.toolMode)) {
		RiverManager.updateRiverData(%nodeId);
		return;
	}

	if (!isObject(RiverEditorGui.River))
		return;

	%this.noNodeUpdate = %noUpdate;

	if (%nodeId !$= RiverEditorGui.getSelectedNode()) {
		RiverManager.fromScript = true;
		RiverEditorGui.setSelectedNode(%nodeId);
	} else
		warnLog("Node is already selected");
}
//------------------------------------------------------------------------------

//==============================================================================
function RiverManager::onNodeSelected(%this,%nodeId,%fromCode) {
	if (%this.noNodeUpdate) {
		return;
	}

	if (strFind("Insert Remove",RiverManager.toolMode) && %nodeId !$= "" && %fromCode) {
		if (!RiverManager.fromScript) {
			RiverManager.updateRiverData(%nodeId);
			return;
		}
	}

	if (%nodeId $= "") {
		%nodeId = RiverEditorGui.getSelectedNode();
	}

	//if (%nodeId $= "-1"){
	//RiverManager.selectNode(0);
	//return;
	//}
	%this.updateNodeInfoDlg(%nodeId);
	%this.setNodePillActive(%nodeId);
	RiverManager.fromScript = false;
}
//------------------------------------------------------------------------------
//==============================================================================
function RiverManager::setNodePillActive(%this,%nodeId) {
	foreach(%ctrl in RiverEd_NodePillStack) {
		%active = false;

		if (%ctrl.nodeId $= %nodeId)
			%active = true;

		%ctrl-->buttonNode.setStateOn(%active);

		if (!isObject(%ctrl-->NodeDataStack))
			continue;

		if (RiverManager.autoCollapsePill)
			%ctrl-->NodeDataStack.visible = %active;
		else
			%ctrl-->NodeDataStack.visible = 1;

		%ctrl-->positionCtrl.visible = RiverManager.showPositionEdit;
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function RiverManager::onNodeModified(%this,%nodeId,%source) {
	%pos = RiverEditorGui.getNodePosition();
	%normal = RiverEditorGui.getNodeNormal();
	%width = RiverEditorGui.getNodeWidth();
	%depth = RiverEditorGui.getNodeDepth();
	%pill = RiverEd_NodePillStack.findObjectByInternalName("NodePill_"@%nodeId,true);

	if (isObject(%pill) && %source !$= "Pill") {
		%pill-->Width.setText(%width);
		%pill-->Width.updateFriends();
		%pill-->Depth.setText(%depth);
		%pill-->Depth.updateFriends();
		%pill-->PosX.text = %pos.x;
		%pill-->PosY.text = %pos.y;
		%pill-->PosZ.text = %pos.z;
	}

	if ( %source !$= "InfoDlg")
		%this.updateNodeInfoDlg(%nodeId);
}
//------------------------------------------------------------------------------

//==============================================================================
function RiverManager::deleteNodeId(%this,%nodeId) {
	RiverEditorGui.setSelectedNode(%nodeId);
	RiverEditorGui.deleteNode();
	%this.updateRiverData();
}
//------------------------------------------------------------------------------