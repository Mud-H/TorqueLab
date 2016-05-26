//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================


//==============================================================================
function Lab::setCameraMoveSpeed(%this, %speed) {
	// Update Toolbar TextEdit
	LabToolbarStack-->CameraSpeedEdit.setText(%speed);
	$Camera::movementSpeed = %speed;

}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::toggleToolbarCamSpeedSlider(%this, %sourceObj) {
	%srcPos = %sourceObj.getRealPosition();
	%srcPos.y += %sourceObj.extent.y;
	%range = "1 1000";
	%ticks = getTicksFromRange(%range,"1");
	EOverlay.toggleSlider("2",%srcPos,"range \t "@%range@" \n ticks \t "@%ticks@"\n altCommand \t Lab.setCameraMoveSpeed($ThisControl.getTypeValue());",$Camera::movementSpeed);
}
//------------------------------------------------------------------------------

//==============================================================================
function WorldEditor::dropCameraToSelection(%this) {
	if(%this.getSelectionSize() == 0)
		return;

	%pos = %this.getSelectionCentroid();
	%cam = LocalClientConnection.camera.getTransform();
	// set the pnt
	%cam = setWord(%cam, 0, getWord(%pos, 0));
	%cam = setWord(%cam, 1, getWord(%pos, 1));
	%cam = setWord(%cam, 2, getWord(%pos, 2));
	LocalClientConnection.camera.setTransform(%cam);
}
//------------------------------------------------------------------------------
