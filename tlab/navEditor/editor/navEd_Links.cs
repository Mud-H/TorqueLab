//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// Plugin Object Params - Used set default settings and build plugins options GUI
//==============================================================================

function NavMeshLinkFlagButton::onClick(%this) {
	NavEditorGui.updateLinkFlags();
}

function NavEditorGui::buildLinks(%this) {
	if(isObject(%this.getMesh())) {
		%this.getMesh().buildLinks();
		%this.isDirty = true;
	}
}

function updateLinkData(%control, %flags) {
	%control->LinkWalkFlag.setActive(true);
	%control->LinkJumpFlag.setActive(true);
	%control->LinkDropFlag.setActive(true);
	%control->LinkLedgeFlag.setActive(true);
	%control->LinkClimbFlag.setActive(true);
	%control->LinkTeleportFlag.setActive(true);
	%control->LinkWalkFlag.setStateOn(%flags & $Nav::WalkFlag);
	%control->LinkJumpFlag.setStateOn(%flags & $Nav::JumpFlag);
	%control->LinkDropFlag.setStateOn(%flags & $Nav::DropFlag);
	%control->LinkLedgeFlag.setStateOn(%flags & $Nav::LedgeFlag);
	%control->LinkClimbFlag.setStateOn(%flags & $Nav::ClimbFlag);
	%control->LinkTeleportFlag.setStateOn(%flags & $Nav::TeleportFlag);
}

function getLinkFlags(%control) {
	return (%control->LinkWalkFlag.isStateOn() ? $Nav::WalkFlag : 0) |
			 (%control->LinkJumpFlag.isStateOn() ? $Nav::JumpFlag : 0) |
			 (%control->LinkDropFlag.isStateOn() ? $Nav::DropFlag : 0) |
			 (%control->LinkLedgeFlag.isStateOn() ? $Nav::LedgeFlag : 0) |
			 (%control->LinkClimbFlag.isStateOn() ? $Nav::ClimbFlag : 0) |
			 (%control->LinkTeleportFlag.isStateOn() ? $Nav::TeleportFlag : 0);
}

function disableLinkData(%control) {
	%control->LinkWalkFlag.setActive(false);
	%control->LinkJumpFlag.setActive(false);
	%control->LinkDropFlag.setActive(false);
	%control->LinkLedgeFlag.setActive(false);
	%control->LinkClimbFlag.setActive(false);
	%control->LinkTeleportFlag.setActive(false);
}

function NavEditorGui::onLinkSelected(%this, %flags) {
	updateLinkData(NavEditorOptionsWindow-->LinkProperties, %flags);
}

function NavEditorGui::onPlayerSelected(%this, %flags) {
	updateLinkData(NavEditorOptionsWindow-->TestProperties, %flags);
}

function NavEditorGui::updateLinkFlags(%this) {
	if(isObject(%this.getMesh())) {
		%properties = NavEditorOptionsWindow-->LinkProperties;
		%this.setLinkFlags(getLinkFlags(%properties));
		%this.isDirty = true;
	}
}



function NavEditorGui::onLinkDeselected(%this) {
	disableLinkData(NavEditorOptionsWindow-->LinkProperties);
}

function NavEditorGui::onPlayerDeselected(%this) {
	disableLinkData(NavEditorOptionsWindow-->TestProperties);
}