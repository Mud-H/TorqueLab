//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================


function DecalEditorGui::onSelectInstance( %this, %decalId, %lookupName ) {
	if( DecalEditorGui.selDecalInstanceId == %decalId )
		return;

	// Lets remember the new Id
	DecalEditorGui.selDecalInstanceId = %decalId;
	DecalEditorTreeView.clearSelection();
	%name = %decalId SPC %lookupName;
	%item = DecalEditorTreeView.findItemByName( %name );
	DecalEditorTreeView.selectItem( %item );
	DecalEditorGui.syncNodeDetails();
	
}

function DecalEditorGui::onCreateInstance( %this, %decalId, %lookupName ) {
	// Lets remember the new Id
	DecalEditorGui.selDecalInstanceId = %decalId;
	// Add the new instance to the node tree
	DecalEditorTreeView.addNodeTree( %decalId, %lookupName );
	DecalEditorTreeView.clearSelection();
	%name = %decalId SPC %lookupName;
	%item = DecalEditorTreeView.findItemByName( %name );
	DecalEditorTreeView.selectItem( %item );
	DecalEditorGui.syncNodeDetails();
}

function DecalEditorGui::onDeleteInstance( %this, %decalId, %lookupName ) {
	if( %decalId == DecalEditorGui.selDecalInstanceId )
		DecalEditorGui.selDecalInstanceId = -1;

	%id = DecalEditorTreeView.findItemByName( %decalId SPC %lookupName );
	DecalEditorTreeView.removeItem(%id);
}