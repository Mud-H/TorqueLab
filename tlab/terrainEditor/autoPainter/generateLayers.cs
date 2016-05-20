//==============================================================================
// LabEditor ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$TPG_AutoStepGeneration = "1";
$TPG_StepGenerationMode = "1";
//==============================================================================
// Terrain Paint Generator - Terrain Paint Generation Functions
//==============================================================================

//==============================================================================
// Generate all the layers in the layer group
function TPG::generateLayerGroup(%this) {
	//First make sure layers data is right
	TPG.checkLayersStackGroup();

	foreach(%layer in TPG_LayerGroup) {
		%layer.inactive = false;
		%layer.failedSettings = "";

		if ( !%layer.activeCtrl.isStateOn()) {
			%layer.inactive = true;
			continue;
		}

		%layer.fieldsValidated = false;
		%validated = %this.validateLayerObjSetting(%layer,true);

		if (strFind(%layer.matInternalName,"*"))
			%layer.failedSettings = "Invalid material assigned to layer, please select a valid from the menu";

		if (%layer.failedSettings !$= "") {
			%validationgFailed = true;
			%badLayers = strAddRecord(%badLayers,%layer.internalName SPC "Bad fields:" SPC %layer.failedSettings);
		}
	}

	if (%validationgFailed) {
		if (%badLayers !$="" ) {
			LabMsgOk("Generation aborted!","Some layers have fail the validation. Here's the list:\c2" SPC  %badLayers @ ". Please fix them before attempting generation again.");
		}

		return;
	}

	show(TPG_GenerateProgressWindow);
	TPG_GenerateLayerStack.clear();

	foreach(%ctrl in TPG_StackLayers) {
		%layer = %ctrl.layerObj;

		if (%layer.inactive)
			continue;

		%heightMin = %layer.getFieldValue("heightMin");
		%heightMax = %layer.getFieldValue("heightMax");
		%slopeMin = %layer.getFieldValue("slopeMin");
		%slopeMax = %layer.getFieldValue("slopeMax");
		%coverage = %layer.getFieldValue("coverage");
		%pill = cloneObject(TPG_GenerateInfoPill_v2,"",%layer.internalName,TPG_GenerateLayerStack);
		%pill.internalName = %layer.internalName;
		%pill-->id.text = "#"@%layerId++ @ "-";
		%pill-->material.text = %layer.matInternalName;
		%pill-->slopeMinMax.setText(%slopeMin SPC "/" SPC %slopeMax);
		%pill-->heightMinMax.text = %heightMin SPC "/" SPC %heightMax;
		%pill-->coverage.text = %coverage;
		%pill-->duration.text = "pending";
	}

	if (%layerId < 1) {
		LabMsgOk("No active layers","There's no active layers to generate terrain materials, operation aborted");
		hide(TPG_GenerateProgressWindow);
		return;
	}

	TPG_GenerateProgressWindow-->cancelButton.visible = 1;
	TPG_GenerateProgressWindow-->reportButton.text = "Start process";
	TPG_GenerateProgressWindow-->reportButton.active = 1;
	$TPG_GenerationStatus = "Pending";
	TPG_GenerateProgressWindow-->reportText.text = %layerId SPC "layers  are ready to be processed";
	//LabMsgYesNo("Early development feature","The terrain pain generator is still in early development and can cause the engine to freeze. "@
//					"We recommend you to save your work before proceeding with automated painting. Are you sure you want to start the painting process?","TPG.schedule(1000,\"startGeneration\");");
	//%this.schedule(1000,"startGeneration");
}
function TPG::toggleStepMode(%this,%checkbox) {
	return;

	if (%checkbox.isStateOn())
		$TPG_AutoStepGeneration = "0";
	else
		$TPG_AutoStepGeneration = "1";
}

//------------------------------------------------------------------------------
function TPG_ReportButton::onClick(%this) {
	devLog("OnCLick");

	switch$($TPG_GenerationStatus) {
	case "Pending":
		%this.text = "Processing";
		%this.active = false;
		TPG_GenerateProgressWindow-->cancelButton.visible = 0;
		TPG.startGeneration();

	case "Completed":
		hide(TPG_GenerateProgressWindow);
	}
}

//==============================================================================
// Start the generation process now that everything is validated
function TPG::startGeneration(%this) {
	if ($TPG_Generating) return;

	TPG.generationStartTime = $Sim::Time;
	$TPG_Generating = true;
	TPG.generatorSteps = "";

	foreach(%ctrl in TPG_StackLayers) {
		%layer = %ctrl.layerObj;

		if (%layer.inactive)
			continue;

		TPG.generatorSteps = strAddWord(TPG.generatorSteps,%layer.getId());
	}

	$TPG_Generating = false;
	//TPG_GenerateProgressWindow.setVisible(false);
	%this.doGenerateLayerStep(200);
}
//------------------------------------------------------------------------------

function TPG::doGenerateLayerStep(%this,%delay) {
	%layer = getWord(TPG.generatorSteps,0);

	if (%layer $= "")
		return;

	%pill = TPG_GenerateLayerStack.findObjectByInternalName(%layer.internalName,true);
	%pill-->duration.text = "Processing";
	TPG_GenerateProgressWindow-->reportText.text ="Processing layer:" SPC %layer.matInternalName @"." SPC getWordCount(TPG.generatorSteps) SPC "left to process.";
	TPG.generatorSteps = removeWord(TPG.generatorSteps,0);
	$TPG_LayerStartTime = $Sim::Time;
	%this.schedule(%delay,"generateLayer",%layer,true);
}
//==============================================================================
// Tell the engine to generate a layer with approved settings
function TPG::generateLayer(%this,%layer,%stepMode) {
	ETerrainEditor.paintIndex = %layer.matIndex;
	%heightMin = %layer.getFieldValue("heightMin");
	%heightMax = %layer.getFieldValue("heightMax");
	%slopeMin = %layer.getFieldValue("slopeMin");
	%slopeMax = %layer.getFieldValue("slopeMax");
	%coverage = %layer.getFieldValue("coverage");
	%terrain = ETerrainEditor.getActiveTerrain();
	%heightMin_w = getWord(%terrain.position,2) + %heightMin;
	%heightMax_w = getWord(%terrain.position,2) + %heightMax;

	if (%stepMode)
		info("Step Painting terrain with Mat Index",%layer.matIndex,"Name",%layer.matInternalName,"Height and Slope",%heightMin_w@"("@%heightMin@")", %heightMax_w@"("@%heightMax@")", %slopeMin, %slopeMax,"Coverage",%coverage);
	else
		info("Painting terrain with Mat Index",%layer.matIndex,"Name",%layer.matInternalName,"Height and Slope",%heightMin, %heightMax, %slopeMin, %slopeMax,"Coverage",%coverage);

	ETerrainEditor.autoMaterialLayer(%heightMin_w, %heightMax_w, %slopeMin, %slopeMax,%coverage);
	%this.generateLayerCompleted(%layer);
}
//------------------------------------------------------------------------------

//==============================================================================
// Tell the engine to generate a layer with approved settings
function TPG::generateLayerCompleted(%this,%layer) {
	TPG.generationTotalTime = $Sim::Time - TPG.generationStartTime;
	%layerTime = $Sim::Time - $TPG_LayerStartTime;
	%pill = TPG_GenerateLayerStack.findObjectByInternalName(%layer.internalName,true);
	%pill-->duration.text = mFloatLength(%layerTime,2) SPC "sec";
	//Get next layer step, if empty, process is completed
	%nextStep = getWord(TPG.generatorSteps,0);

	if (%nextStep !$= "") {
		//Call next step now if we are in Auto Step Generation mode, else wait for next step confirmation
		if (!TPG_StepModeCheckbox.isStateOn())
			TPG.doGenerateLayerStep(200);
		else
			LabMsgYesNo(%layer.matInternalName SPC "step completed","Do you want to proceed with next step:" SPC getWord(TPG.generatorSteps,0).matInternalName SPC "?","TPG.doGenerateLayerStep(500);","TPG.generateProcessCompleted();");
	} else {
		%this.generateProcessCompleted();
	}
}
//------------------------------------------------------------------------------

//==============================================================================
// Tell the engine to generate a layer with approved settings
function TPG::generateProcessCompleted(%this) {
	if (TPG.generatorSteps !$= "")
		%result = "Process cancelled. ("@getWordCount(TPG.generatorSteps)@" unprocessed).";
	else
		%result = "All layers have been processed.";

	TPG.generatorSteps = "";
	$TPG_GenerationStatus = "Completed";
	TPG_GenerateProgressWindow-->reportText.text = %result SPC "Process time:\c1 " @ TPG.generationTotalTime @ " sec";
	TPG_GenerateProgressWindow-->reportButton.text = "Close report";
	TPG_GenerateProgressWindow-->reportButton.active = 1;
}
//------------------------------------------------------------------------------