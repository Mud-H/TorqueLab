//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// Road Object Functions
//==============================================================================
//==============================================================================
function MeshRoadEditorGui::onRoadSelected( %this, %road ) {
	%this.road = %road;
	devLog("MeshRoadEditorGuionRoadSelected");
	MRoadManager.currentRoad = %road;

	// Update the materialEditorList
	if( isObject( %road ) )
		$Lab::materialEditorList = %road.getId();

	MeshRoadInspector.inspect( %road );
	MeshRoadTreeView.buildVisibleTree(true);

	if( MeshRoadTreeView.getSelectedObject() != %road ) {
		MeshRoadTreeView.clearSelection();
		%treeId = MeshRoadTreeView.findItemByObjectId( %road );
		MeshRoadTreeView.selectItem( %treeId );
	}

	MRoadManager.updateRoadData();
}
//------------------------------------------------------------------------------
//==============================================================================
function MeshRoadEditorGui::onNodeSelected( %this, %nodeIdx ) {
	if ( %nodeIdx == -1 ) {
		MeshRoadEditorOptionsWindow-->position.setActive( false );
		MeshRoadEditorOptionsWindow-->position.setValue( "" );
		MeshRoadEditorOptionsWindow-->normal.setActive( false );
		MeshRoadEditorOptionsWindow-->normal.setValue( "" );
		MeshRoadEditorOptionsWindow-->width.setActive( false );
		MeshRoadEditorOptionsWindow-->width.setValue( "" );
		MeshRoadEditorOptionsWindow-->depth.setActive( false );
		MeshRoadEditorOptionsWindow-->depth.setValue( "" );
	} else {
		MeshRoadEditorOptionsWindow-->position.setActive( true );
		MeshRoadEditorOptionsWindow-->position.setValue( %this.getNodePosition() );
		MeshRoadEditorOptionsWindow-->normal.setActive( true );
		MeshRoadEditorOptionsWindow-->normal.setValue( %this.getNodeNormal() );
		MeshRoadEditorOptionsWindow-->width.setActive( true );
		MeshRoadEditorOptionsWindow-->width.setValue( %this.getNodeWidth() );
		MeshRoadEditorOptionsWindow-->depth.setActive( true );
		MeshRoadEditorOptionsWindow-->depth.setValue( %this.getNodeDepth() );
	}

	MRoadManager.onNodeSelected(%nodeIdx);
}
//------------------------------------------------------------------------------
//==============================================================================
function MeshRoadEditorGui::onNodeModified( %this, %nodeIdx ) {
	MeshRoadEditorOptionsWindow-->position.setValue( %this.getNodePosition() );
	MeshRoadEditorOptionsWindow-->normal.setValue( %this.getNodeNormal() );
	MeshRoadEditorOptionsWindow-->width.setValue( %this.getNodeWidth() );
	MeshRoadEditorOptionsWindow-->depth.setValue( %this.getNodeDepth() );
}
//------------------------------------------------------------------------------
//==============================================================================
function MeshRoadEditorGui::editNodeDetails( %this ) {
	%this.setNodePosition( MeshRoadEditorOptionsWindow-->position.getText() );
	%this.setNodeNormal( MeshRoadEditorOptionsWindow-->normal.getText() );
	%this.setNodeWidth( MeshRoadEditorOptionsWindow-->width.getText() );
	%this.setNodeDepth( MeshRoadEditorOptionsWindow-->depth.getText() );
}
//------------------------------------------------------------------------------
//==============================================================================
// Road Materials Selection
//==============================================================================
//==============================================================================
function MeshRoadEditorGui::onBrowseClicked( %this ) {
	//%filename = RETextureFileCtrl.getText();
	%dlg = new OpenFileDialog() {
		Filters        = "All Files (*.*)|*.*|";
		DefaultPath    = MeshRoadEditorGui.lastPath;
		DefaultFile    = %filename;
		ChangePath     = false;
		MustExist      = true;
	};
	%ret = %dlg.Execute();

	if(%ret) {
		MeshRoadEditorGui.lastPath = filePath( %dlg.FileName );
		%filename = %dlg.FileName;
		MeshRoadEditorGui.setTextureFile( %filename );
		MeshRoadEditorTextureFileCtrl.setText( %filename );
	}

	%dlg.delete();
}