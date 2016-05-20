//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$LabGameMap.bindCmd(keyboard, "ctrl 0", "TLabGameGui.toggleMe();");
$LabGameMap.bindCmd(keyboard, "ctrl i", "TLabGameGui.toggleCursor();");
$LabGameMap.bindCmd(keyboard, "ctrl m", "Lab.toggleGameDlg(\"SceneEditorDialogs\",\"AmbientManager\");");

//==============================================================================
function EMaterialSkinner::onWake( %this ) {
	logc("EMaterialSkinner::onWake");
}

//------------------------------------------------------------------------------
//==============================================================================
function EMaterialSkinner::onSleep( %this ) {
	logc("EMaterialSkinner::onSleep");
}

//------------------------------------------------------------------------------
//==============================================================================
function EMaterialSkinner::onShow( %this ) {
	logc("EMaterialSkinner::onShow");
}

//------------------------------------------------------------------------------
//==============================================================================
function EMaterialSkinner::onHide( %this ) {
	logc("EMaterialSkinner::onHide");
}

//------------------------------------------------------------------------------


//==============================================================================
function EMaterialSkinner::createSkin( %this ) {
	logc("EMaterialSkinner::createSkin");
	%material = MaterialEditorTools.currentMaterial;
	%mappedTo = %material.mapTo;
	EMaterialSkinner.isBase = true;

	if (getSubStr(%mappedTo,0,4) $= "base") {
		info("The map is using default base system:",%mappedTo);
		EMaterialSkinner.mapToEnd = getSubStr(%mappedTo,4);
	} else {
		info("The map is NOT using default base system, but we will figure something out",%mappedTo);
		//return;
		MaterialSkinner.isBase = false;
	}

	ETools.showTool("MaterialSkinner");
	%this-->matSourceName.setText(%material.getName());
	%this-->matSourceMap.setText(%mappedTo);
	MaterialSkinner_SkinNameEdit.setText("");
	MaterialSkinner_MatNameEdit.active = true;
	MaterialSkinner_MatNameEdit.setText("[Enter skin first]");
	MaterialSkinner_MatNameEdit.active = false;
	MaterialSkinner_SaveButton.active = false;
	MaterialSkinner_SaveEditButton.active = false;
	MaterialSkinner_GenerateButton.active = false;
	EMaterialSkinner.matName = "";
	EMaterialSkinner.skinName = "";
}

//------------------------------------------------------------------------------
//==============================================================================
function EMaterialSkinner::generateName( %this ) {

	if (MaterialSkinner.isBase)
		%mapToEnd = getSubStr(MaterialEditorTools.currentMaterial.mapTo,4);
	else
		%mapToEnd = MaterialEditorTools.currentMaterial.mapTo;

	%name = "Mat_"@EMaterialSkinner.skinName@"_"@%mapToEnd;
	%unique = getUniqueName(%name);
	MaterialSkinner_MatNameEdit.setText(%unique);
	MaterialSkinner_MatNameEdit.onValidate();
}

//------------------------------------------------------------------------------
//==============================================================================
function EMaterialSkinner::cancelClose( %this ) {
	logc("EMaterialSkinner::cancelClose");
	ETools.hideTool("MaterialSkinner");
}

//------------------------------------------------------------------------------
//==============================================================================
function EMaterialSkinner::save( %this,%edit ) {
	logc("EMaterialSkinner::save(%edit )",%edit);
	%this.createNewMaterial(%edit);
	ETools.hideTool("MaterialSkinner");
}
//==============================================================================
// Clone selected Material
function EMaterialSkinner::createNewMaterial( %this,%edit ) {
	%srcMat = MaterialEditorTools.currentMaterial;

	if (!isObject(%srcMat))
		return;

	if (EMaterialSkinner.matName $= ""||EMaterialSkinner.skinName $= ""||EMaterialSkinner.mapToEnd $= "") {
		warnLog("Something went wrong, can't generate material:",EMaterialSkinner);
		return;
	}
EMaterialSkinner.matName = getUniqueName(EMaterialSkinner.matName);
	%newMat = %srcMat.deepClone();
	%newMat.setName(EMaterialSkinner.matName);
	%newMat.setFilename(%srcMat.getFilename());
	%newMat.mapTo = EMaterialSkinner.skinName @ EMaterialSkinner.mapToEnd;
	LabObj.save(%newMat,true);

	if (!%edit)
		return;

	MaterialEditorTools.currentObject = "";
	MaterialEditorTools.setMode();
	MaterialEditorTools.prepareActiveMaterial( %newMat.getId(), true );
}
//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
//==============================================================================
function MaterialSkinner_SkinNameEdit::onValidate( %this ) {
	%skin = %this.getText();

	if (%skin $="")
		return;

	EMaterialSkinner.skinName = %skin;
	MaterialSkinner_MatNameEdit.active = true;
	MaterialSkinner_GenerateButton.active = true;

	if ($MaterialSkinner_AutomaticNameGeneration)
		EMaterialSkinner.generateName();
}

//------------------------------------------------------------------------------
//==============================================================================
function MaterialSkinner_MatNameEdit::onValidate( %this ) {
	%name = %this.getText();

	if (%name $="")
		return;

	EMaterialSkinner.matName = getUniqueName(%name);
	MaterialSkinner_SaveButton.active = true;
	MaterialSkinner_SaveEditButton.active = true;
}

//------------------------------------------------------------------------------

