//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// Plugin Object Params - Used set default settings and build plugins options GUI
//==============================================================================

function NavMeshTestFlagButton::onClick(%this) {
	NavEditorGui.updateTestFlags();
}

function NavEditorGui::updateTestFlags(%this) {
	if(isObject(%this.getPlayer())) {
		%properties = NavEditorOptionsWindow-->TestProperties;
		%player = %this.getPlayer();
		$NavBot = %player;
		%player.allowWwalk = %properties->LinkWalkFlag.isStateOn();
		%player.allowJump = %properties->LinkJumpFlag.isStateOn();
		%player.allowDrop = %properties->LinkDropFlag.isStateOn();
		%player.allowLedge = %properties->LinkLedgeFlag.isStateOn();
		%player.allowClimb = %properties->LinkClimbFlag.isStateOn();
		%player.allowTeleport = %properties->LinkTeleportFlag.isStateOn();
		%this.isDirty = true;
	}
}

function NavEditorGui::followObject(%this) {
	if(%this.getMode() $= "TestMode" && isObject(%this.getPlayer())) {
		%obj = LocalClientConnection.player;
		%text = NavEditorOptionsWindow-->TestProperties->FollowObject.getText();

		if(%text !$= "") {
			eval("%obj = " @ %text);

			if(!isObject(%obj))
				MessageBoxOk("Error", "Cannot find object" SPC %text);
		}

		if(isObject(%obj))
			%this.getPlayer().followObject(%obj, NavEditorOptionsWindow-->TestProperties->FollowRadius.getText());
	}
}