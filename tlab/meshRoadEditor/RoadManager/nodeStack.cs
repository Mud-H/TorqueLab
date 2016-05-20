//==============================================================================
// TorqueLab -> MeshRoadEditor - Road Manager - NodeData
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// TerrainObject Functions
//==============================================================================


//==============================================================================
function MRoadManager::updateNodeStack(%this) {
	MREP_NodePillSampleStack.visible = 0;
	MREP_NodePillSample.visible = 1;
	%stack = MREP_NodePillStack;
	%stack.visible = 1;
	%stack.clear();

	for(%i=0; %i<MRoadManager.nodeCount; %i++) {
		%this.addNodePillToStack(%i);
	}
}

function MRoadManager::addNodePillToStack(%this,%nodeId) {
	%fieldData = MRoadManager.nodeData[%nodeId];
	%pos = getField(%fieldData,0);
	%normal = getField(%fieldData,1);
	%width = getField(%fieldData,2);
	%depth = getField(%fieldData,3);

	switch$(MRoadManager.nodeListModeId) {
	case "0":
		%pill = cloneObject(MREP_NodePillLinkSample);

	case "1":
		%pill = cloneObject(MREP_NodePillSample);
		%pill-->Width.setText(%width);
		%pill-->Width.updateFriends();
		%pill-->Depth.setText(%depth);
		%pill-->Depth.updateFriends();
		%pill-->Depth.pill = %pill;
		%pill-->Width.pill = %pill;

	case "2":
		%pill = cloneObject(MREP_NodePillFullSample);
		//%pill-->nodeIdText.text = "Node #\c2"@%nodeId;
		%pill-->toggleNorm.command = "toggleVisible("@%pill-->normCtrl.getId()@");";
		%pill-->togglePos.command = "toggleVisible("@%pill-->posCtrl.getId()@");";
		%pill-->PosX.text = %pos.x;
		%pill-->PosY.text = %pos.y;
		%pill-->PosZ.text = %pos.z;
		%pill-->Normal.text = %normal;
		%pill-->Width.setText(%width);
		%pill-->Width.updateFriends();
		%pill-->Depth.setText(%depth);
		%pill-->Depth.updateFriends();
		%pill-->Width.pill = %pill;
		%pill-->Depth.pill = %pill;
	}

	%pill-->Linked.text = " ";
	%pill-->buttonNode.text = "Node #\c1"@%nodeId;
	%pill-->buttonNode.command = "MRoadManager.selectNode("@%nodeId@");";
	%pill-->deleteButton.command = "MRoadManager.deleteNodeId("@%nodeId@");";
	%pill-->Linked.setStateOn(false);
	%pill.superClass = "MREP_NodePill";
	%pill.internalName = "NodePill_"@%nodeId;
	%pill.nodeId = %nodeId;
	%pill-->Linked.pill = %pill;
	%pill.linkCheck = %pill-->Linked;
	MREP_NodePillStack.add(%pill);
}
//==============================================================================
// Update Node
//==============================================================================
//==============================================================================
function MREP_SingleNodeEdit::onValidate(%this) {
	%type = %this.internalName;
	MRoadManager.updateNodeSetting("",%type,%this.getText());
}
//------------------------------------------------------------------------------

//==============================================================================
function MRoadManager::updateNodeSetting(%this,%node,%field,%value,%isLink) {
	%this.noNodeUpdate = true;

	if (%node $= "")
		%node = MeshRoadEditorGui.getSelectedNode();
	else
		MeshRoadEditorGui.setSelectedNode(%node);

	switch$(%field) {
	case "width":
		MeshRoadEditorGui.setNodeWidth(%value);

	case "depth":
		MeshRoadEditorGui.setNodeDepth(%value);

	case "normal":
		MeshRoadEditorGui.setNodeNormal(%value);

	case "PosX":
		%posDiff = %position.x - %value;
		%position = MeshRoadEditorGui.getNodePosition();

		if (%isLink)
			%position.x = %position.x + %value;
		else
			%position.x = %value;

		MeshRoadEditorGui.setNodePosition(%position);

	case "PosY":
		%posDiff = %position.y - %value;
		%position = MeshRoadEditorGui.getNodePosition();

		if (%isLink)
			%position.y = %position.y + %value;
		else
			%position.y = %value;

		MeshRoadEditorGui.setNodePosition(%position);

	case "PosZ":
		%posDiff = %position.z - %value;
		%position = MeshRoadEditorGui.getNodePosition();

		if (%isLink)
			%position.z = %position.z + %value;
		else
			%position.z = %value;

		MeshRoadEditorGui.setNodePosition(%position);

	case "position":
		if (%isLink) {
			%position = MeshRoadEditorGui.getNodePosition();
			%value = VectorAdd(%position,%value);
		} else {
			%position = MeshRoadEditorGui.getNodePosition();
			%posDiff = VectorSub(%value,%position);
		}

		MeshRoadEditorGui.setNodePosition(%value);
	}

	if ($MREP_UpdateLinkedNodes && !%isLink) {
		foreach$(%nodeLink in MRoadManager.linkedList) {
			if (%nodeLink $= %node)
				continue;

			if (%posDiff !$= "") {
				%value = %posDiff;
			}

			%this.updateNodeSetting(%nodeLink,%field,%value,true);
		}

		MeshRoadEditorGui.setSelectedNode(%node);
	}

	%this.noNodeUpdate = false;
}
//------------------------------------------------------------------------------
//==============================================================================
function MRoadManager::updateNodeCtrlSetting(%this,%ctrl) {
	%pill = MRoadManager.getParentPill(%ctrl);
	%node = %pill.nodeId;
	%fields = strreplace(%ctrl.internalName,"_"," ");
	%field = getWord(%fields,0);
	%value = %ctrl.getTypeValue();
	%ctrl.updateFriends();
	%this.updateNodeSetting(%node,%field,%value);
}
//------------------------------------------------------------------------------
//==============================================================================
function MRoadManager::getParentPill(%this,%ctrl) {
	%attempt = 0;

	while(%attempt < 10) {
		%parent = %ctrl.parentGroup;

		if (%parent.superClass $= "MREP_NodePill")
			return %parent;

		%ctrl = %parent;
		%attempt++;
	}

	return "";
}
//------------------------------------------------------------------------------

//==============================================================================
function MREP_NodeSlider::onMouseDragged(%this) {
	%value = mFloatLength(%this.getValue(),3);
	%this.setValue(%value);
	MRoadManager.updateNodeCtrlSetting(%this);
}
//------------------------------------------------------------------------------
//==============================================================================
function MREP_NodeEdit::onValidate(%this) {
	MRoadManager.updateNodeCtrlSetting(%this);
}
//------------------------------------------------------------------------------
//==============================================================================
function MREP_MaterialSelect::setMaterial(%this,%materialName,%a1,%a2) {
	devLog("MREP_MaterialSelect::setMaterial(%this,%materialName,%a1,%a2)",%this,%materialName,%a1,%a2);
	%type = %this.internalName;
	%road = MRoadManager.currentRoad;

	if (!isObject(%road))
		return;

	MeshRoadInspector.inspect(%road);
	%road.setFieldValue(%type@"Material",%materialName);
	MeshRoadInspector.refresh();
	MeshRoadInspector.apply();
	%textEdit = MeshRoadEditorOptionsWindow.findObjectByInternalName(%type@"Material",true);
	%textEdit.setText(%materialName);
	devLog("Select Material for:",%type,"Is:",%materialName);
}
//------------------------------------------------------------------------------
//==============================================================================
function MREP_MaterialEdit::onValidate(%this) {
	%type = %this.internalName;
	devLog("Select Material for:",%type,"Is:",%this.getText());
}
//------------------------------------------------------------------------------

