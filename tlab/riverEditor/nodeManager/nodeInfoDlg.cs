//==============================================================================
// TorqueLab -> MeshRiverEditor - River Manager - NodeData
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
function RiverManager::updateNodeInfoDlg(%this,%nodeId,%fromPill) {
	%pos = RiverEditorGui.getNodePosition();
	%normal = RiverEditorGui.getNodeNormal();
	%width = RiverEditorGui.getNodeWidth();
	%depth = RiverEditorGui.getNodeDepth();
	%infoDlg = RiverEd_NodeInfoDialog;

	if (RiverEd_NodeInfoDialog.isAwake()) {
		%infoDlg-->Width.setText(%width);
		%infoDlg-->Width.updateFriends();
		%infoDlg-->Depth.setText(%depth);
		%infoDlg-->Depth.updateFriends();
		%infoDlg-->position.setText(%pos);
		%infoDlg-->normal.setText(%normal);
	}
}
//------------------------------------------------------------------------------
function RiverEd_InfoNodeEdit::onValidate( %this ) {
	%field = %this.internalName;
	RiverEditorGui.setNodeData(%field,%this.getText());
	%this.updateFriends();
}
function RiverEd_InfoNodeSlider::onMouseDragged( %this ) {
	%fieldData = %this.internalName;
	%field = getWord(strreplace(%fieldData,"_"," "),0);
	RiverEditorGui.setNodeData(%field,%this.getValue());
	%this.updateFriends();
}

function RiverEditorGui::setNodeData( %this,%type,%value ) {
	devLog("setNodeData,",%type,%value);
	%evalStr = "RiverEditorGui.setNode"@%type@"("@%value@");";
	eval(%evalStr);
	RiverManager.onNodeModified(RiverEditorGui.getSelectedNode(),"InfoDlg");
}