//==============================================================================
// TorqueLab -> MeshRoadEditor - Road Manager - NodeData
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//RoadManager.updateRoadData();
function RoadManager::updateRoadData(%this) {
	%road = RoadManager.currentRoad;

	if (!isObject(%road)) {
		REP_NodePillStack.clear();
		RoadEd_TabPageNode-->roadNodePageTitle.setText("No road selected");
		return;
	}

	%this.noNodeUpdate = true;

	if (RoadManager.nodeListModeId $= "")
		RoadManager.nodeListModeId = "0";

	%id = 0;

	while( true ) {
		RoadEditorGui.setSelectedNode(%id);
		%nodeWidth = RoadEditorGui.getNodeWidth();

		if (%nodeWidth $= "" || %nodeWidth $= "-1")
			break;

		%nodePos = RoadEditorGui.getNodePosition();
		RoadManager.nodeData[%id] = %nodePos TAB %nodeWidth;
		%id++;
	}

	RoadManager.nodeCount = %id;
	%this.updateNodeStack();
	%this.noNodeUpdate = false;
	%node = RoadEditorGui.getSelectedNode();

	if (%node $= "-1" || %node $= "" || %node >= RoadManager.nodeCount)
		RoadManager.selectNode(0);
	else
		RoadManager.onNodeSelected(%node);

	MeshRoadEditorOptionsWindow-->topMaterial.setText(%road.topMaterial);
	MeshRoadEditorOptionsWindow-->bottomMaterial.setText(%road.bottomMaterial);
	MeshRoadEditorOptionsWindow-->sideMaterial.setText(%road.sideMaterial);
}
//==============================================================================
// Select Node
//==============================================================================

//------------------------------------------------------------------------------
$NoNodeUpd = true;
function RoadManager::selectNode(%this,%nodeId,%noUpdate) {
	if (!isObject(RoadEditorGui.road))
		return;

	%this.noNodeUpdate = %noUpdate;
	RoadEditorGui.setSelectedNode(%nodeId);
}
//------------------------------------------------------------------------------

//==============================================================================
function RoadManager::onNodeSelected(%this,%nodeId) {
	if (%this.noNodeUpdate) {
		return;
	}

	if (%nodeId $= "") {
		%nodeId = RoadEditorGui.getSelectedNode();
	}

	if (%nodeId $= "-1") {
		RoadManager.selectNode(0);
		return;
	}

	foreach(%ctrl in REP_NodePillStack) {
		%active = false;

		if (%ctrl.nodeId $= %nodeId)
			%active = true;

		%ctrl-->buttonNode.setStateOn(%active);

		if (!isObject(%ctrl-->NodeDataStack))
			continue;

		if (RoadManager.autoCollapsePill)
			%ctrl-->NodeDataStack.visible = %active;
		else
			%ctrl-->NodeDataStack.visible = 1;

		%ctrl-->positionCtrl.visible = RoadManager.showPositionEdit;
	}
}
//------------------------------------------------------------------------------