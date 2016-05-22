//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
function LabProgressActionGui::onWake(%this) {
}
//------------------------------------------------------------------------------
//==============================================================================
function LabProgressActionGui::onSleep(%this) {
	LabProgressActionText.setText("");
	LabProgressActionNote.setText("");
	LabProgressActionProgress.setValue(0);
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::LoadActionProgress(%this,%text,%progressVar,%note,%delayMS) {
	Lab.doingAction = true;
	pushDlg(LabProgressActionGui);
	LabProgressActionText.setText(%text);
	LabProgressActionNote.setText(%note);

	if (%progressVar $= "") {
		LabProgressActionProgress.visible = 0;
	} else {
		LabProgressActionProgress.visible = 1;
		LabProgressActionProgress.setValue(0);
		LabProgressActionProgress.variable = %progressVar;
	}

	if(%delayMS !$= "") {
		%this.schedule(%delayMS,"ExitActionProgress");
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::ExitActionProgress(%this) {
	Lab.doingAction = false;
	popDlg(LabProgressActionGui);
}
//------------------------------------------------------------------------------
