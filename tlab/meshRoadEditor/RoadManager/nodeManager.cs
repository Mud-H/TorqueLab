//==============================================================================
// TorqueLab -> MeshRoadEditor - Road Manager - NodeData
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//MRoadManager.updateRoadData();
function MRoadManager::updateRoadData(%this) {
	%road = MRoadManager.currentRoad;

	if (!isObject(%road))
		return;

	%this.noNodeUpdate = true;

	if (MRoadManager.nodeListModeId $= "")
		MRoadManager.nodeListModeId = "0";

	%id = 0;

	while( true ) {
		MeshRoadEditorGui.setSelectedNode(%id);
		%nodeWidth = MeshRoadEditorGui.getNodeWidth();

		if (%nodeWidth $= "" || %nodeWidth $= "-1")
			break;

		%nodePos = MeshRoadEditorGui.getNodePosition();
		%nodeDepth = MeshRoadEditorGui.getNodeDepth();
		%nodeNormal = MeshRoadEditorGui.getNodeNormal();
		MRoadManager.nodeData[%id] = %nodePos TAB %nodeNormal TAB %nodeWidth TAB %nodeDepth;
		%id++;
	}

	MRoadManager.nodeCount = %id;
	%this.updateNodeStack();
	%this.noNodeUpdate = false;
	%node = MeshRoadEditorGui.getSelectedNode();

	if (%node $= "-1" || %node $= "" || %node >= MRoadManager.nodeCount)
		MRoadManager.selectNode(0);
	else
		MRoadManager.onNodeSelected(%node);

	MeshRoadEditorOptionsWindow-->topMaterial.setText(%road.topMaterial);
	MeshRoadEditorOptionsWindow-->bottomMaterial.setText(%road.bottomMaterial);
	MeshRoadEditorOptionsWindow-->sideMaterial.setText(%road.sideMaterial);
}
//==============================================================================
// Select Node
//==============================================================================

//------------------------------------------------------------------------------
$NoNodeUpd = true;
function MRoadManager::selectNode(%this,%nodeId,%noUpdate) {
	if (!isObject(MeshRoadEditorGui.road))
		return;

	%this.noNodeUpdate = %noUpdate;
	MeshRoadEditorGui.setSelectedNode(%nodeId);
}
//------------------------------------------------------------------------------

//==============================================================================
function MRoadManager::onNodeSelected(%this,%nodeId) {
	if (%this.noNodeUpdate) {
		return;
	}

	if (%nodeId $= "-1" || %nodeId $= "") {
		MRoadManager.selectNode(0);
		return;
	}

	foreach(%ctrl in MREP_NodePillStack) {
		%active = false;

		if (%ctrl.nodeId $= %nodeId)
			%active = true;

		%ctrl-->buttonNode.setStateOn(%active);

		if (!isObject(%ctrl-->NodeDataStack))
			continue;

		if (MRoadManager.autoCollapsePill)
			%ctrl-->NodeDataStack.visible = %active;
		else
			%ctrl-->NodeDataStack.visible = 1;
	}
}
//------------------------------------------------------------------------------