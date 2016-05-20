//==============================================================================
// TorqueLab -> MeshRiverEditor - River Manager - NodeData
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// TerrainObject Functions
//==============================================================================

$RiverEd_WidthRange = "0 200";
$RiverEd_DepthRange = "0 100";
//==============================================================================
//RiverManager.updateNodeStack();
function RiverManager::updateNodeStack(%this) {
	RiverEd_NodePillSampleStack.visible = 0;
	RiverEd_NodePillStack.visible = 1;
	%stack = RiverEd_NodePillStack;
	%stack.visible = 1;
	%stack.clear();

	for(%i=0; %i<RiverManager.nodeCount; %i++) {
		%this.addNodePillToStack(%i);
	}
}

function RiverManager::addNodePillToStack(%this,%nodeId) {
	%fieldData = RiverManager.nodeData[%nodeId];
	%pos = getField(%fieldData,0);
	%width = getField(%fieldData,1);
	%depth = getField(%fieldData,2);
	
	RiverManager.nodeListModeId = 1;

	switch$(RiverManager.nodeListModeId) {
	case "0":
		%pill = cloneObject(RiverEd_NodePillLinkSample);

	case "1":
		%pill = cloneObject(RiverEd_NodePillSample);
		%pill-->Width.setText(%width);
		%pill-->Width.updateFriends();
		%pill-->Width.pill = %pill;
		%pill-->Width_slider.pill = %pill;
		%pill-->Depth.setText(%width);
		%pill-->Depth.updateFriends();
		%pill-->Depth.pill = %pill;
		%pill-->Depth_slider.pill = %pill;
		%pill-->Width_slider.range = $RiverEd_WidthRange;
		%pill-->Depth_slider.range = $RiverEd_DepthRange;
		%pill-->PosX.text = %pos.x;
		%pill-->PosY.text = %pos.y;
		%pill-->PosZ.text = %pos.z;
		%pill-->PosX.pill = %pill;
		%pill-->PosY.pill = %pill;
		%pill-->PosZ.pill = %pill;
		%pill-->PosX.altCommand = "$ThisControl.onAltCommand();";
		%pill-->PosY.altCommand = "$ThisControl.onAltCommand();";
		%pill-->PosZ.altCommand = "$ThisControl.onAltCommand();";
		%pill-->positionCtrl.visible = RiverManager.showPositionEdit;

	case "2":
		%pill = cloneObject(RiverEd_NodePillSample);
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
	%pill-->buttonNode.command = "RiverManager.selectNode("@%nodeId@");";
	%pill-->deleteButton.command = "RiverManager.deleteNodeId("@%nodeId@");";
	%pill-->Linked.setStateOn(false);
	%pill.superClass = "RiverEd_NodePill";
	%pill.internalName = "NodePill_"@%nodeId;
	%pill.nodeId = %nodeId;
	%pill-->Linked.pill = %pill;
	%pill.linkCheck = %pill-->Linked;
	RiverEd_NodePillStack.add(%pill);
}
//==============================================================================
// Update Node
//==============================================================================
//==============================================================================
function RiverEd_SingleNodeEdit::onValidate(%this) {
	%type = %this.internalName;
	RiverManager.updateNodeSetting("",%type,%this.getText());
}
//------------------------------------------------------------------------------

//==============================================================================
function RiverManager::updateNodeSetting(%this,%node,%field,%value,%isLink) {
	%this.noNodeUpdate = true;

	if (%node $= "")
		%node = RiverEditorGui.getSelectedNode();
	else
		RiverEditorGui.setSelectedNode(%node);

	switch$(%field) {
	case "width":
		%width = RiverEditorGui.getNodeWidth();

		if (!%isLink) {
			%widthDiff = %value - %width;
		}

		if (%isLink && RiverManager.linkRelative) {
			%newValue = %width + %value;
			%value = %newValue;
		}

		RiverEditorGui.setNodeWidth(%value);

	case "depth":
		%depth = RiverEditorGui.getNodeDepth();

		if (!%isLink) {
			%depthDiff = %value - %depth;
		}

		if (%isLink && RiverManager.linkRelative) {
			%newValue = %depth + %value;
			%value = %newValue;
		}

		RiverEditorGui.setNodeDepth(%value);

	case "PosX":
		%position = RiverEditorGui.getNodePosition();

		if (!%isLink)
			%posDiff = %value - %position.x;

		if (%isLink && RiverManager.linkRelative)
			%position.x = %position.x + %value;
		else
			%position.x = %value;

		RiverEditorGui.setNodePosition(%position);

	case "PosY":
		%position = RiverEditorGui.getNodePosition();

		if (!%isLink)
			%posDiff =%value - %position.x;

		if (%isLink && RiverManager.linkRelative)
			%position.y = %position.y + %value;
		else
			%position.y = %value;

		RiverEditorGui.setNodePosition(%position);

	case "PosZ":
		%position = RiverEditorGui.getNodePosition();

		if (!%isLink)
			%posDiff = %value - %position.z;

		if (%isLink && RiverManager.linkRelative)
			%position.z = %position.z + %value;
		else
			%position.z = %value;

		RiverEditorGui.setNodePosition(%position);

	case "position":
		if (%isLink && RiverManager.linkRelative) {
			%position = RiverEditorGui.getNodePosition();
			%value = VectorAdd(%position,%value);
		} else {
			%position = RiverEditorGui.getNodePosition();
			%posDiff = VectorSub(%value,%position);
		}

		RiverEditorGui.setNodePosition(%value);
	}

	if ($RiverEd_UpdateLinkedNodes && !%isLink) {
		foreach$(%nodeLink in RiverManager.linkedList) {
			if (%nodeLink $= %node)
				continue;

			if (RiverManager.linkRelative) {
				if (%posDiff !$= "") {
					%value = %posDiff;
				} else if (%widthDiff !$= "") {
					%value = %widthDiff;
				} else if (%depthDiff !$= "") {
					%value = %depthDiff;
				}
			}

			%this.updateNodeSetting(%nodeLink,%field,%value,true);
		}

		RiverEditorGui.setSelectedNode(%node);
	}

	%this.noNodeUpdate = false;
	RiverManager.onNodeModified(%node,"Pill");
}
//------------------------------------------------------------------------------
//==============================================================================
function RiverManager::updateNodeCtrlSetting(%this,%ctrl) {
	//%pill = RiverManager.getParentPill(%ctrl);
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
function RiverManager::getParentPill(%this,%ctrl) {
	%attempt = 0;

	while(%attempt < 10) {
		%parent = %ctrl.parentGroup;

		if (%parent.superClass $= "RiverEd_NodePill")
			return %parent;

		%ctrl = %parent;
		%attempt++;
	}

	return "";
}
//------------------------------------------------------------------------------

//==============================================================================
function RiverEd_NodeSlider::onMouseDragged(%this) {
	%value = mFloatLength(%this.getValue(),3);
	%this.setValue(%value);
	RiverManager.updateNodeCtrlSetting(%this);
}
//------------------------------------------------------------------------------
//==============================================================================
function RiverEd_NodeEdit::onValidate(%this) {
	//RiverManager.updateNodeCtrlSetting(%this);
}
//------------------------------------------------------------------------------
//==============================================================================
function RiverEd_NodeEdit::onCommand(%this) {
	//RiverManager.updateNodeCtrlSetting(%this);
}
//------------------------------------------------------------------------------
//==============================================================================
function RiverEd_NodeEdit::onAltCommand(%this) {
	RiverManager.updateNodeCtrlSetting(%this);
}
//------------------------------------------------------------------------------
//==============================================================================
function RiverEd_NodeListCheck::onClick(%this) {
	RiverManager.onNodeSelected();
}
//------------------------------------------------------------------------------


//==============================================================================
function RiverEd_MaterialSelect::setMaterial(%this,%materialName,%a1,%a2) {
	devLog("RiverEd_MaterialSelect::setMaterial(%this,%materialName,%a1,%a2)",%this,%materialName,%a1,%a2);
	%type = %this.internalName;
	%River = RiverManager.currentRiver;

	if (!isObject(%River))
		return;

	MeshRiverInspector.inspect(%River);
	%River.setFieldValue(%type@"Material",%materialName);
	MeshRiverInspector.refresh();
	MeshRiverInspector.apply();
	%textEdit = MeshRiverEditorOptionsWindow.findObjectByInternalName(%type@"Material",true);
	%textEdit.setText(%materialName);
	devLog("Select Material for:",%type,"Is:",%materialName);
}
//------------------------------------------------------------------------------
//==============================================================================
function RiverEd_MaterialEdit::onValidate(%this) {
	%type = %this.internalName;
	devLog("Select Material for:",%type,"Is:",%this.getText());
}
//------------------------------------------------------------------------------

