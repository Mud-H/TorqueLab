//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

function DecalEditorGui::rebuildInstanceTree( %this ) {
	// Initialize the instance tree when the tab is selected
	DecalEditorTreeView.removeItem(0);
	%rootId = DecalEditorTreeView.insertItem(0, "<root>", 0, "");
	%count = DecalEditorGui.getDecalCount();

	for (%i = 0; %i < %count; %i++) {
		%name = DecalEditorGui.getDecalLookupName(%i);

		if( %name $= "invalid" )
			continue;

		DecalEditorTreeView.addNodeTree(%i, %name);
	}
}


function DecalEditorTreeView::onDefineIcons() {
	%icons = "tlab/art/icons/default/common/default:" @
				"tlab/art/icons/object_class/decal:" @
				"tlab/art/icons/object_class/decalNode:";
	DecalEditorTreeView.buildIconTable( %icons );
}

function DecalEditorTreeView::onUnSelect(%this, %id) {
	hide(DecalEd_DeleteSelBtn);
}
function DecalEditorTreeView::onSelect(%this, %id) {
	%instanceTag = getWord( DecalEditorTreeView.getItemText(%id), 1 );

	if( !isObject( %instanceTag ) ){
	   %data = DecalEditorTreeView.getItemText(%id);
	   if (isObject(%data)){
	   DecalEditorGui.selectData(%data);
	   DecalEditorGui.showInstancePreview(false);
	   }
	   return;
	}
		

	if( %instanceTag.getClassName() !$= "DecalData" ){	
	   return;
	}
		
   DecalEditorGui.showInstancePreview(true);
	// Grab the id from the tree view
	%decalId = getWord( DecalEditorTreeView.getItemText(%id), 0 );

	if( DecalEditorGui.selDecalInstanceId == %decalId )
		return;

	// Set the curent decalinstances id
	DecalEditorGui.selDecalInstanceId = %decalId;
	DecalEditorGui.selectDecal(%decalId);
	DecalEditorGui.syncNodeDetails(%id);
}

// Creating per node in the instance tree
function DecalEditorTreeView::addNodeTree(%this, %nodeName, %parentName) {
	// If my template isnt there...put it there
	if ( %this.findItemByName(%parentName) == 0 ) {
		%rootId = %this.findItemByName("<root>");
		%this.insertItem( %rootId, %parentName, 0, "", 1, 1);
	}

	%nodeName = %nodeName SPC %parentName;
	%parentId = %this.findItemByName(%parentName);
	%id = %this.insertItem(%parentId, %nodeName, 0, "", 2);
}
