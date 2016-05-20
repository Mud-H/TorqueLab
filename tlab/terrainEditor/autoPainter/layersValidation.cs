//==============================================================================
// TorqueLab -> Terrain Paint Generator System
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// Terrain Paint Generator - Layer Validation Functions
//==============================================================================
function TPG::reportValidationErrors(%this) {
	if ($TPG_LayerValidationErrors !$="")
		LabMsgOk(%layer.internalName SPC "validation failed",$TPG_LayerValidationErrors);

	$TPG_LayerValidationErrors = "";
}
//==============================================================================
// Validate all setting for a single layer
function TPG::validateLayerObjSetting(%this,%layer,%doPostValidation) {
	%validated = false;
	%layer.failedSettings = "";
	$TPG_LayerValidationErrors = "";
	%this.validateLayerSetting("heightMin",%layer,true);
	%this.validateLayerSetting("heightMax",%layer,true);
	%this.validateLayerSetting("slopeMin",%layer,true);
	%this.validateLayerSetting("slopeMax",%layer,true);
	%this.validateLayerSetting("coverage",%layer,true);
	%this.reportValidationErrors();

	if (%doPostValidation && %layer.failedSettings $= "")
		%validated = %this.postLayerValidation(%layer);
}
//------------------------------------------------------------------------------
//==============================================================================
// Validate a single setting for a layer
function TPG::validateLayerSetting(%this,%setting,%layer,%fullValidation) {
	if (!isObject(%layer)) {
		%error = "Invalid layer submitted for validation. There's no such object:\c2" SPC %layer;
		warnLog(%error,"Setting:",%setting);
		return;
		$TPG_LayerValidationErrors = strAddRecord($TPG_LayerValidationErrors,%error);
		%failed = true;
	}

	%layer.isValidated = false;
	%layer.setFieldValue(%setting,"");
	eval("%ctrl = %layer.pill-->"@%setting@";");

	if (!isObject(%ctrl)) {
		%error = %layer.internalName SPC "layer doesn't have a valid field for:" SPC %setting @ ". Please delete it and report the issue.";
		$TPG_LayerValidationErrors = strAddRecord($TPG_LayerValidationErrors,%error);
		%failed = true;
	} else {
		%value = %ctrl.getValue();

		if (!strIsNumeric(%value)) {
			%error = %setting SPC " is not a numeric value, please change it before being able to generate the layers";
			$TPG_LayerValidationErrors = strAddRecord($TPG_LayerValidationErrors,%error);
			%failed = true;
		}

		if (%value $= "") {
			%error = %setting SPC " is not a numeric value, please change it before being able to generate the layers";
			$TPG_LayerValidationErrors = strAddRecord($TPG_LayerValidationErrors,%error);
			%failed = true;
		}
	}

	if (%failed) {
		%layer.failedSettings = strAddWord(%layer.failedSettings,%setting);

		if (!%fullValidation)
			%this.reportValidationErrors();

		return;
	}

	switch$(%setting) {
	case "heightMin":
		if ($Lab::TerrainPainter::ValidateHeight) {
			if (%value < $TPG_DefaultHeightMin)
				%value = $TPG_DefaultHeightMin;
		}

	case "heightMax":
		if ($Lab::TerrainPainter::ValidateHeight) {
			if (%value < $TPG_DefaultHeightMax)
				%value = $TPG_DefaultHeightMax;
		}

	case "slopeMin":
		if (%value < 0)
			%value = "0";

	case "slopeMax":
		if (%value > 90)
			%value = "90";

	case "coverage":
		%value = mClamp(%value,"0","99");
	}

	%ctrl.setValue(%value);
	%layer.setFieldValue(%setting,%value);
}
//------------------------------------------------------------------------------
//==============================================================================
// Post validate the settings of a layer globally
function TPG::postLayerValidation(%this,%layer) {
	foreach$(%field in $TPG_ValidateFields) {
		if (%layer.getFieldValue(%field) $="") {
			%error = %setting SPC "is not validated";
			%errors = strAddRecord(%errors,%error);
		}
	}

	if (%errors !$="") {
		LabMsgOk("Layer post validation failed",%errors);
		return false;
	}

	if (%layer.getFieldValue("heightMin") > %layer.getFieldValue("heightMax")) {
		%layer.setFieldValue("heightMin",%layer.getFieldValue("heightMax"));
		%report = "Height min was higher than Height max. The height minimum is changed to fit the maximum.";
		%reports = strAddRecord(%reports,%report);
	}

	if (%reports !$="") {
		LabMsgOk("Layer post validation succeed with warnings",%reports);
	}

	%layer.fieldsValidated = true;
	%layer.isValidated = true;
	return true;
}
//------------------------------------------------------------------------------