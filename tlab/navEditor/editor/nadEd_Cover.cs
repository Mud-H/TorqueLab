//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// Plugin Object Params - Used set default settings and build plugins options GUI
//==============================================================================


function NavEditorGui::createCoverPoints(%this) {
	if(isObject(%this.getMesh())) {
		%this.getMesh().createCoverPoints();
		%this.isDirty = true;
	}
}

function NavEditorGui::deleteCoverPoints(%this) {
	if(isObject(%this.getMesh())) {
		NavEditorGui.getMesh().deleteCoverPoints();
		%this.isDirty = true;
	}
}

function NavEditorGui::findCover(%this) {
	if(%this.getMode() $= "TestMode" && isObject(%this.getPlayer())) {
		%pos = LocalClientConnection.getControlObject().getPosition();
		%text = NavEditorOptionsWindow-->TestProperties->CoverPosition.getText();

		if(%text !$= "")
			%pos = eval(%text);

		%this.getPlayer().findCover(%pos, NavEditorOptionsWindow-->TestProperties->CoverRadius.getText());
	}
}