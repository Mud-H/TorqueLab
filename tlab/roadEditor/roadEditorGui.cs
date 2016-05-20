//==============================================================================
// TorqueLab -> RoadEditorGui Editor GUI
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
function RoadEditorGui::onWake( %this ) {
	$DecalRoad::EditorOpen = true;
	%count = EWorldEditor.getSelectionSize();

	for ( %i = 0; %i < %count; %i++ ) {
		%obj = EWorldEditor.getSelectedObject(%i);

		if (!isObject(%obj)) {
			warnLog("EWorldEditor contain an invalid object in selection index:",%i,"Obj:",%obj);
			continue;
		}

		if ( %obj.getClassName() !$= "DecalRoad" )
			EWorldEditor.unselectObject(%obj);
		else
			%this.setSelectedRoad( %obj );
	}

	%this.onNodeSelected(-1);
}
//------------------------------------------------------------------------------
//==============================================================================
function RoadEditorGui::showDefaultMaterialSaveDialog( %this, %toMaterial ) {
	%fromMaterial = RoadEditorGui.materialName;
	RoadEditorGui.materialName = %toMaterial.getName();
	Lab.syncConfigParamField(arRoadEditorCfg.paramObj,"materialName",%toMaterial.getName());
	devLog("RoadEditorGui Default material changed from:",%fromMaterial,"To:",%toMaterial);
}
//------------------------------------------------------------------------------
//==============================================================================
function RoadEditorGui::onSleep( %this ) {
	$DecalRoad::EditorOpen = false;
}
//------------------------------------------------------------------------------
//==============================================================================
function RoadEditorGui::onEscapePressed( %this ) {
	if( %this.getMode() $= "RoadEditorAddNodeMode" ) {
		%this.prepSelectionMode();
		return true;
	}

	return false;
}
//------------------------------------------------------------------------------
//==============================================================================
function RoadEditorGui::onRoadCreation( %this ) {
}
//------------------------------------------------------------------------------
//==============================================================================
function RoadEditorGui::onBrowseClicked( %this ) {
	//%filename = RETextureFileCtrl.getText();
	%dlg = new OpenFileDialog() {
		Filters        = "All Files (*.*)|*.*|";
		DefaultPath    = RoadEditorGui.lastPath;
		DefaultFile    = %filename;
		ChangePath     = false;
		MustExist      = true;
	};
	%ret = %dlg.Execute();

	if(%ret) {
		RoadEditorGui.lastPath = filePath( %dlg.FileName );
		%filename = %dlg.FileName;
		RoadEditorGui.setTextureFile( %filename );
		RETextureFileCtrl.setText( %filename );
	}

	%dlg.delete();
}
//------------------------------------------------------------------------------
//==============================================================================
function RoadTreeView::onSelect(%this, %obj) {
	RoadEditorGui.road = %obj;
	RoadInspector.inspect( %obj );

	if(%obj != RoadEditorGui.getSelectedRoad()) {
		RoadEditorGui.setSelectedRoad( %obj );
	}
	RoadManager.updateRoadData();
}
//------------------------------------------------------------------------------
//==============================================================================
function RoadDefaultWidthSliderCtrlContainer::onWake(%this) {
	RoadDefaultWidthSliderCtrlContainer-->slider.setValue(RoadDefaultWidthTextEditContainer-->textEdit.getText());
}
//------------------------------------------------------------------------------
//==============================================================================

function RoadEditorGui::setDefaultMaterial(%this,%matName) {
	RoadEditorGui.setFieldValue("materialName",%matName);
}
//------------------------------------------------------------------------------
//==============================================================================

function RoadEditorGui::setActiveMaterial(%this,%matName) {
	if (!isObject(RoadEditorGui.road))
		return;

	RoadEditorGui.road.setFieldValue("Material",%matName);
	RoadInspector.refresh();
}
//------------------------------------------------------------------------------