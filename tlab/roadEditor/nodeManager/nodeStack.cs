//==============================================================================
// TorqueLab -> MeshRoadEditor - Road Manager - NodeData
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// TerrainObject Functions
//==============================================================================


//==============================================================================
//RoadManager.updateNodeStack();
function RoadManager::updateNodeStack(%this) {
	REP_NodePillSampleStack.visible = 0;
	REP_NodePillStack.visible = 1;
	%stack = REP_NodePillStack;
	%stack.visible = 1;
	%stack.clear();

	for(%i=0; %i<RoadManager.nodeCount; %i++) {
		%this.addNodePillToStack(%i);
	}
}

function RoadManager::addNodePillToStack(%this,%nodeId) {
	%fieldData = RoadManager.nodeData[%nodeId];
	%pos = getField(%fieldData,0);
	%width = getField(%fieldData,1);
	RoadManager.nodeListModeId = 1;

	switch$(RoadManager.nodeListModeId) {
	case "0":
		%pill = cloneObject(REP_NodePillLinkSample);

	case "1":
		%pill = cloneObject(REP_NodePillSample);
		%pill-->Width.setText(%width);
		%pill-->Width.updateFriends();
		%pill-->Width.pill = %pill;
		%pill-->Width_slider.pill = %pill;
		%pill-->PosX.text = %pos.x;
		%pill-->PosY.text = %pos.y;
		%pill-->PosZ.text = %pos.z;
		%pill-->PosX.pill = %pill;
		%pill-->PosY.pill = %pill;
		%pill-->PosZ.pill = %pill;
	}

	%pill-->Linked.text = " ";
	%pill-->buttonNode.text = "Node #\c1"@%nodeId;
	%pill-->buttonNode.command = "RoadManager.selectNode("@%nodeId@");";
	%pill-->deleteButton.command = "RoadManager.deleteNodeId("@%nodeId@");";
	%pill-->Linked.setStateOn(false);
	%pill.superClass = "REP_NodePill";
	%pill.internalName = "NodePill_"@%nodeId;
	%pill.nodeId = %nodeId;
	%pill-->Linked.pill = %pill;
	%pill.linkCheck = %pill-->Linked;
	REP_NodePillStack.add(%pill);
	RoadEd_TabPageNode-->roadNodePageTitle.setText("Road have \c1"@RoadManager.nodeCount@"\c0 Nodes");
}
//==============================================================================
// Update Node
//==============================================================================
//==============================================================================
function REP_SingleNodeEdit::onValidate(%this) {
	%type = %this.internalName;
	RoadManager.updateNodeSetting("",%type,%this.getText());
}
//------------------------------------------------------------------------------

//==============================================================================
function RoadManager::updateNodeSetting(%this,%node,%field,%value,%isLink) {
	%this.noNodeUpdate = true;

	if (%node $= "")
		%node = RoadEditorGui.getSelectedNode();
	else
		RoadEditorGui.setSelectedNode(%node);

	switch$(%field) {
	case "width":
		RoadEditorGui.setNodeWidth(%value);

	case "PosX":
		%posDiff = %position.x - %value;
		%position = RoadEditorGui.getNodePosition();

		if (%isLink)
			%position.x = %position.x + %value;
		else
			%position.x = %value;

		RoadEditorGui.setNodePosition(%position);

	case "PosY":
		%posDiff = %position.y - %value;
		%position = RoadEditorGui.getNodePosition();

		if (%isLink)
			%position.y = %position.y + %value;
		else
			%position.y = %value;

		RoadEditorGui.setNodePosition(%position);

	case "PosZ":
		%posDiff = %position.z - %value;
		%position = RoadEditorGui.getNodePosition();

		if (%isLink)
			%position.z = %position.z + %value;
		else
			%position.z = %value;

		RoadEditorGui.setNodePosition(%position);

	case "position":
		if (%isLink) {
			%position = RoadEditorGui.getNodePosition();
			%value = VectorAdd(%position,%value);
		} else {
			%position = RoadEditorGui.getNodePosition();
			%posDiff = VectorSub(%value,%position);
		}

		RoadEditorGui.setNodePosition(%value);
	}

	if ($REP_UpdateLinkedNodes && !%isLink) {
		foreach$(%nodeLink in RoadManager.linkedList) {
			if (%nodeLink $= %node)
				continue;

			if (%posDiff !$= "") {
				%value = %posDiff;
			}

			%this.updateNodeSetting(%nodeLink,%field,%value,true);
		}

		RoadEditorGui.setSelectedNode(%node);
	}

	%this.noNodeUpdate = false;
}
//------------------------------------------------------------------------------
//==============================================================================
function RoadManager::updateNodeCtrlSetting(%this,%ctrl) {
	//%pill = RoadManager.getParentPill(%ctrl);
	%pill = %ctrl.pill;
	%node = %pill.nodeId;
	%fields = strreplace(%ctrl.internalName,"_"," ");
	%field = getWord(%fields,0);
	%value = %ctrl.getTypeValue();
	%ctrl.updateFriends();
	%this.updateNodeSetting(%node,%field,%value);
}
//------------------------------------------------------------------------------
//==============================================================================
function RoadManager::getParentPill(%this,%ctrl) {
	%attempt = 0;

	while(%attempt < 10) {
		%parent = %ctrl.parentGroup;

		if (%parent.superClass $= "REP_NodePill")
			return %parent;

		%ctrl = %parent;
		%attempt++;
	}

	return "";
}
//------------------------------------------------------------------------------

//==============================================================================
function REP_NodeSlider::onMouseDragged(%this) {
	%value = mFloatLength(%this.getValue(),3);
	%this.setValue(%value);
	RoadManager.updateNodeCtrlSetting(%this);
}
//------------------------------------------------------------------------------
//==============================================================================
function REP_NodeEdit::onValidate(%this) {
	RoadManager.updateNodeCtrlSetting(%this);
}
//------------------------------------------------------------------------------
//==============================================================================
function REP_NodeListCheck::onClick(%this) {
	RoadManager.onNodeSelected();
}
//------------------------------------------------------------------------------


//==============================================================================
function REP_MaterialSelect::setMaterial(%this,%materialName,%a1,%a2) {
	devLog("REP_MaterialSelect::setMaterial(%this,%materialName,%a1,%a2)",%this,%materialName,%a1,%a2);
	%type = %this.internalName;
	%road = RoadManager.currentRoad;

	if (!isObject(%road))
		return;

	MeshRoadInspector.inspect(%road);
	%road.setFieldValue(%type@"Material",%materialName);
	MeshRoadInspector.refresh();
	MeshRoadInspector.apply();
	%textEdit = MeshRoadEditorOptionsWindow.findObjectByInternalName(%type@"Material",true);
	%textEdit.setText(%materialName);
}
//------------------------------------------------------------------------------
//==============================================================================
function REP_MaterialEdit::onValidate(%this) {
	%type = %this.internalName;
}
//------------------------------------------------------------------------------

