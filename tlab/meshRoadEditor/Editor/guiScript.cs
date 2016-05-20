//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
function LabMouseToggle::onMouseDown(%this,%modifier,%mousePoint,%mouseClickCount) {
	%target = %this.internalName;
	toggleVisible(%target);
}
//------------------------------------------------------------------------------
//==============================================================================
// Initialize default plugin settings
function MeshRoadEditorGui::onWake( %this ) {
	$MeshRoad::EditorOpen = true;
	%count = EWorldEditor.getSelectionSize();

	for ( %i = 0; %i < %count; %i++ ) {
		%obj = EWorldEditor.getSelectedObject(%i);

		if ( %obj.getClassName() !$= "MeshRoad" )
			EWorldEditor.unselectObject(%obj);
		else
			%this.setSelectedRoad( %obj );
	}

	//%this-->TabBook.selectPage(0);
	%this.onNodeSelected(-1);
}

function MeshRoadEditorGui::onSleep( %this ) {
	$MeshRoad::EditorOpen = false;
}
//------------------------------------------------------------------------------
function MeshRoadEditorGui::showDefaultMaterialSaveDialog( %this, %toMaterial,%extra ) {
	devLog("showDefaultMaterialSaveDialog",%toMaterial,"Extra",%extra);
	%fromMaterial = MeshRoadEditorGui.topMaterialName;
	MeshRoadEditorGui.topMaterialName = %toMaterial.getName();
	Lab.syncConfigParamField(arMeshRoadEditorCfg.paramObj,"topMaterialName",%toMaterial.getName());
	devLog("MeshRoadEditorGui Default material changed from:",%fromMaterial,"To:",%toMaterial);
}
function MeshRoadEditorGui::paletteSync( %this, %mode ) {
	%evalShortcut = "LabPaletteArray-->" @ %mode @ ".setStateOn(1);";
	eval(%evalShortcut);
}
function MeshRoadEditorGui::onEscapePressed( %this ) {
	if( %this.getMode() $= "MeshRoadEditorAddNodeMode" ) {
		%this.prepSelectionMode();
		return true;
	}

	return false;
}