//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
function LabMat::updatePreviewObject(%this) {
	%newModel = LabMat_quickPreview_Popup.getValue();

	switch$(%newModel) {
	case "sphere":
		matEd_quickPreview_Popup.selected = %newModel;
		matEd_previewObjectView.setModel("tlab/materialEditor/assets/spherePreview.dts");
		matEd_previewObjectView.setOrbitDistance(4);

	case "cube":
		matEd_quickPreview_Popup.selected = %newModel;
		matEd_previewObjectView.setModel("tlab/materialEditor/assets/cubePreview.dts");
		matEd_previewObjectView.setOrbitDistance(5);

	case "pyramid":
		matEd_quickPreview_Popup.selected = %newModel;
		matEd_previewObjectView.setModel("tlab/materialEditor/assets/pyramidPreview.dts");
		matEd_previewObjectView.setOrbitDistance(5);

	case "cylinder":
		matEd_quickPreview_Popup.selected = %newModel;
		matEd_previewObjectView.setModel("tlab/materialEditor/assets/cylinderPreview.dts");
		matEd_previewObjectView.setOrbitDistance(4.2);

	case "torus":
		matEd_quickPreview_Popup.selected = %newModel;
		matEd_previewObjectView.setModel("tlab/materialEditor/assets/torusPreview.dts");
		matEd_previewObjectView.setOrbitDistance(4.2);

	case "knot":
		matEd_quickPreview_Popup.selected = %newModel;
		matEd_previewObjectView.setModel("tlab/materialEditor/assets/torusknotPreview.dts");
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function LabMat::updateLivePreview(%this,%preview) {
	// When checkbox is selected, preview the material in real time, if not; then don't
	if( %preview )
		LabMat.copyMaterials( materialEd_previewMaterial, LabMat.currentMaterial );
	else
		LabMat.copyMaterials( notDirtyMaterial, LabMat.currentMaterial );

	LabMat.currentMaterial.flush();
	LabMat.currentMaterial.reload();
}
//------------------------------------------------------------------------------
