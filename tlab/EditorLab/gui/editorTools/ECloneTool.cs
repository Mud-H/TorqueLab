//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$ECloneToolContainers = "ECloneTool";
//==============================================================================
// Profile FONTS manipulation functions
//==============================================================================

//==============================================================================
//FONTS -> Change the font to all profile or only those specified in the list
function ECloneTool::toggleVisibility( %this ) {
	ETools.toggleTool("CloneTool");

	if ( %this.visible  ) {
		//%this.selectWindow();
		%this.setCollapseGroup(false);
		//%this.onShow();
	}
}
//------------------------------------------------------------------------------
//==============================================================================
//FONTS -> Change the font to all profile or only those specified in the list
function ECloneTool::onWake( %this ) {
	%this-->copyCount.text = "1";
	%this-->copyOffsetX.text = "0";
	%this-->copyOffsetY.text = "0";
	%this-->copyOffsetZ.text = "0";
}
//------------------------------------------------------------------------------
//==============================================================================
//FONTS -> Change the font to all profile or only those specified in the list
function ECloneTool::doCopy( %this ) {
	%copyCount = %this-->copyCount.getValue();
	%offsetX = %this-->copyOffsetX.getValue();
	%offsetY = %this-->copyOffsetY.getValue();
	%offsetZ = %this-->copyOffsetZ.getValue();
	%count = EWorldEditor.getSelectionSize();

	if (%count < 1) {
		warnLog("There's no selected objects to copy!");
		return;
	}

	for (%i=1; %i<=%copyCount; %i++) {
		for( %j=0; %j<%count; %j++) {
			%obj = EWorldEditor.getSelectedObject( %j );

			if( !%obj.isMemberOfClass("SceneObject") ) continue;

			%clone = %obj.clone();
			%clone.position.x += %offsetX * %i;
			%clone.position.y += %offsetY * %i;
			%clone.position.z += %offsetZ * %i;
			MissionGroup.add(%clone);
		}
	}
}
//------------------------------------------------------------------------------
//==============================================================================
//FONTS -> Change the font to all profile or only those specified in the list
function ECloneTool::setCurrentOffset( %this,%axis ) {
	%obj = EWorldEditor.getSelectedObject(0);
	%size = %obj.getObjectBox();
	%scale = %obj.getScale();
	%sizex = (getWord(%size, 3) - getWord(%size, 0)) * getWord(%scale, 0);
	%sizey = (getWord(%size, 4) - getWord(%size, 1)) * getWord(%scale, 0);
	%sizez = (getWord(%size, 5) - getWord(%size, 2)) * getWord(%scale, 0);

	foreach$(%container in $ECloneToolContainers) {
		%textEdit = %container.findObjectByInternalName("copyOffset"@%axis,true);

		if (!isObject(%textEdit))
			continue;

		%textEdit.setValue(%size[%axis]);
	}
}
//------------------------------------------------------------------------------
//==============================================================================
//FONTS -> Change the font to all profile or only those specified in the list
function ECloneTool::resetCurrentOffset( %this,%axis ) {
	%obj = EWorldEditor.getSelectedObject(0);
	%size = %obj.getObjectBox();
	%scale = %obj.getScale();
	%sizex = "0";
	%sizey = "0";
	%sizez = "0";

	foreach$(%container in $ECloneToolContainers) {
		%textEdit = %container.findObjectByInternalName("copyOffset"@%axis,true);

		if (!isObject(%textEdit))
			continue;

		%textEdit.setValue(%size[%axis]);
	}
}
//------------------------------------------------------------------------------
//==============================================================================
//FONTS -> Change the font to all profile or only those specified in the list
function ECloneEdit::onValidate( %this ) {
	%value = %this.getText();
	%field = %this.internalName;

	if (%field $= "copyCount")
		%value = ECloneTool.validateCopieCount(%value);

	foreach$(%container in $ECloneToolContainers) {
		%textEdit = %container.findObjectByInternalName(%field,true);

		if (!isObject(%textEdit))
			continue;

		%textEdit.setValue(%value);
	}
}
//------------------------------------------------------------------------------
//==============================================================================
//FONTS -> Change the font to all profile or only those specified in the list
function ECloneTool::validateCopieCount( %this,%value ) {
	if (!strIsNumeric(%value)) {
		warnLog("Clone copie count must be a numeric value!");
		%value = "0";
	} else if (%value < 0) {
		%value = "0";
	}

	return %value;
}
//------------------------------------------------------------------------------

//==============================================================================
//ETransformTool.resetAll Field TextEditValue Changed
function ECloneTool::cloneToCtrl( %this,%ctrl ) {
	%ctrl.add(ECloneTool-->cloneTools.deepClone());
	$ECloneToolContainers = strAddWord($ECloneToolContainers,%ctrl.getId(),true);
}
//------------------------------------------------------------------------------
