//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// Road Object Selected/UnSelected
//==============================================================================
//==============================================================================
function RoadEditorGui::onRoadSelected( %this, %road ) {
	%this.road = %road;
	RoadManager.currentRoad = %road;

	// Update the materialEditorList
	if(isObject( %road )) {
		$Lab::materialEditorList = %road.getId();
		RoadEditorPlugin.selectedRoad = %road;
		RoadEditorPlugin.selectedMaterial = %road.Material;
		$REP_CurrentRoad = RoadEditorPlugin.selectedRoad;
		
		RoadEditorToolbar-->changeActiveMaterialBtn.active = 1;
		
		//Lab.getDecalRoadNodes();
	} else
		%this.noRoadSelected();

	
	RoadInspector.inspect( %road );
	RoadTreeView.buildVisibleTree(true);

	if( RoadTreeView.getSelectedObject() != %road ) {
		RoadTreeView.clearSelection();
		%treeId = RoadTreeView.findItemByObjectId( %road );
		RoadTreeView.selectItem( %treeId );
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function RoadEditorGui::noRoadSelected( %this ) {
	RoadEditorPlugin.selectedRoad = "";
	RoadEditorPlugin.selectedMaterial = "No road selected";
	RoadEditorToolbar-->changeActiveMaterialBtn.active = 0;
	RoadManager.updateRoadData();
}
//------------------------------------------------------------------------------
//==============================================================================
// Selected Road Material Functions
//==============================================================================
//==============================================================================
function RoadEditorGui::changeActiveMaterial( %this, %toMaterial ) {
	if (!isObject(RoadEditorPlugin.selectedRoad)) {
		LabMsgOk("No active road","You need to have a road selected to change the material");
		return;
	}

	MaterialSelector.showDialog("RoadEditorGui.changeActiveMaterialCallback");
}
//------------------------------------------------------------------------------
//==============================================================================
function RoadEditorGui::changeActiveMaterialCallback( %this, %toMaterial ) {
	if (!isObject(RoadEditorPlugin.selectedRoad))
		return;

	%roadObj = RoadEditorPlugin.selectedRoad;
	%fromMaterial = %roadObj.Material;
	RoadInspector.setObjectField( "Material", %toMaterial.getName());
	RoadEditorPlugin.selectedMaterial = %toMaterial.getName();
}
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// RoadObject Node Manipulation
//==============================================================================
//==============================================================================
function RoadEditorGui::onNodeSelected( %this, %nodeIdx ) {
	if ($REP_CustomNodeSelection) {
		warnLog("OnNodeSelected is called from script and no need to update GUI");
		return;
	}

	if ( %nodeIdx == -1 ) {
		RoadEd_TabPageInspect-->position.setActive( false );
		RoadEd_TabPageInspect-->position.setValue( "" );
		RoadEd_TabPageInspect-->width.setActive( false );
		RoadEd_TabPageInspect-->width.setValue( "" );
	} else {
		RoadEd_TabPageInspect-->position.setActive( true );
		RoadEd_TabPageInspect-->position.setValue( %this.getNodePosition() );
		RoadEd_TabPageInspect-->width.setActive( true );
		RoadEd_TabPageInspect-->width.setValue( %this.getNodeWidth() );
	}

	RoadManager.onNodeSelected(%nodeIdx);
}
//------------------------------------------------------------------------------
//==============================================================================
function RoadEditorGui::onNodeModified( %this, %nodeIdx ) {
	RoadEditorProperties-->position.setValue( %this.getNodePosition() );
	RoadEditorProperties-->width.setValue( %this.getNodeWidth() );
}
//------------------------------------------------------------------------------
//==============================================================================
function RoadEditorGui::editNodeDetails( %this ) {
	%this.setNodePosition( RoadEditorProperties-->position.getText() );
	%this.setNodeWidth( RoadEditorProperties-->width.getText() );
}
//------------------------------------------------------------------------------
//==============================================================================
// Road Object Input Events
//==============================================================================
//==============================================================================
function RoadEditorGui::onDeleteKey( %this ) {
	%road = %this.getSelectedRoad();
	%node = %this.getSelectedNode();

	if ( !isObject( %road ) )
		return;

	if ( %node != -1 ) {
		%this.deleteNode();
	} else {
		LabMsgOkCancel( "Notice", "Delete selected DecalRoad?", "RoadEditorGui.deleteRoad();", "" );
	}
}
//------------------------------------------------------------------------------

//==============================================================================
function REP_PropertyEdit::onValidate( %this ) {
	%road = RoadEditorGui.getSelectedRoad();
	if ( !isObject( %road ) )
		return;
	
	LabObj.set(%road,%this.internalName,%this.getText());
	%this.updateFriends();
}
//------------------------------------------------------------------------------
//==============================================================================
function REP_PropertySlider::onMouseDragged( %this ) {
	%road = RoadEditorGui.getSelectedRoad();
	if ( !isObject( %road ) )
		return;
	%fields = strreplace(%this.internalName,"_"," ");
	%field = getWord(%fields,0);
	LabObj.set(%road,%field,%this.getValue());
	%this.updateFriends();
}
//------------------------------------------------------------------------------
