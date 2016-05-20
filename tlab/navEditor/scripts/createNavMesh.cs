//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// Plugin Object Params - Used set default settings and build plugins options GUI
//==============================================================================

function CreateNewNavMeshDlg::onWake(%this) {
	%this-->MeshName.setText("Nav");
	%this-->MeshPosition.setText("0 0 0");
	%this-->MeshScale.setText("50 50 20");
	MeshMissionBounds.setStateOn(false);
	MeshTerrainBounds.setStateOn(true);
	// show(CreateNewNavMeshWindow);
}

function MissionBoundsExtents(%group) {
	%box = "0 0 0 0 0 0";

	foreach(%obj in %group) {
		%cls = %obj.getClassName();

		if(%cls $= "SimGroup" || %cls $= "SimSet" || %cls $= "Path") {
			// Need to recursively check grouped objects.
			%wbox = MissionBoundsExtents(%obj);
		} else {
			// Skip objects that are too big and shouldn't really be considered
			// part of the scene, or are global bounds and we therefore can't get
			// any sensible information out of them.
			if(%cls $= "LevelInfo")
				continue;

			if(!MeshTerrainBounds.isStateOn() && %cls $= "TerrainBlock")
				continue;

			if(!(%obj.getType() & $TypeMasks::StaticObjectType) ||
					%obj.getType() & $TypeMasks::EnvironmentObjectType)
				continue;

			if(%obj.isGlobalBounds())
				continue;

			%wbox = %obj.getWorldBox();
		}

		// Update min point.
		for(%j = 0; %j < 3; %j++) {
			if(GetWord(%box, %j) > GetWord(%wbox, %j))
				%box = SetWord(%box, %j, GetWord(%wbox, %j));
		}

		// Update max point.
		for(%j = 3; %j < 6; %j++) {
			if(GetWord(%box, %j) < GetWord(%wbox, %j))
				%box = SetWord(%box, %j, GetWord(%wbox, %j));
		}
	}

	return %box;
}

function CreateNewNavMeshDlg::create(%this) {
	%name = %this-->MeshName.getText();

	if(%name $= "" || nameToID(%name) != -1) {
		MessageBoxOk("Error", "A NavMesh must have a unique name!");
		return;
	}

	%mesh = 0;

	if(MeshMissionBounds.isStateOn()) {
		if(!isObject(MissionGroup)) {
			MessageBoxOk("Error", "You must have a MissionGroup to use the mission bounds function.");
			return;
		}

		// Get maximum extents of all objects.
		%box = MissionBoundsExtents(MissionGroup);
		%pos = GetBoxCenter(%box);
		%scale = (GetWord(%box, 3) - GetWord(%box, 0)) / 2 + 5
					SPC (GetWord(%box, 4) - GetWord(%box, 1)) / 2 + 5
					SPC (GetWord(%box, 5) - GetWord(%box, 2)) / 2 + 5;
		%mesh = new NavMesh(%name) {
			position = %pos;
			scale = %scale;
		};
	} else {
		%mesh = new NavMesh(%name) {
			position = %this-->MeshPosition.getText();
			scale = %this-->MeshScale.getText();
		};
	}

	MissionGroup.add(%mesh);
	NavEditorGui.selectObject(%mesh);
	Canvas.popDialog(CreateNewNavMeshDlg);
}

function MeshMissionBounds::onClick(%this) {
	MeshTerrainBounds.setActive(%this.isStateOn());
}
