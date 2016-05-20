//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//EWorldEditor.transformSelection(doPosition, point, doRelativePos, doRotate,rotation, doRelativeRot, doRotLocal, scaleType, scale, isRelative,  isLocal );

$ETransformToolContainers = "ETransformTool";
$ETransformToolEditFields = "PosX PosY PosZ TransX TransY TransZ RotX RotY RotZ RotH RotP RotB ScaleX ScaleY ScaleZ";
$ETransformTool::Active::Position = false;
$ETransformTool::Active::Rotation = false;
$ETransformTool::Active::Scale = false;

$ETransformTool::Relative::Position = true;
$ETransformTool::Relative::Rotation = true;
$ETransformTool::Relative::Scale = true;

$ETransformTool::LocalCenter::Rotation = true;
$ETransformTool::LocalCenter::Scale = true;

$ETransformTool::Proportional::Scale = false;
//==============================================================================
//ETransformTool.resetAll Field TextEditValue Changed
function ETransformTool::resetAll( %this,%ctrl ) {
	foreach$(%field in $ETransformToolEditFields) {
		%ctrl = %this.findObjectByInternalName(%field,true);
		%ctrl.setText("0");
		%this.updateTextEditField(%ctrl);
	}
}
//------------------------------------------------------------------------------

//==============================================================================
//ETransformTool Field TextEditValue Changed
function ETransformTool::editCommand( %this,%ctrl ) {
	logd("ETransformTool::editCommand",%ctrl);
	%this.updateTextEditField(%ctrl);
}
//------------------------------------------------------------------------------
//==============================================================================
//ETransformTool Field TextEditValue Changed
function ETransformTool::updateTextEditField( %this,%ctrl ) {
	logd("ETransformTool::updateTextEditField",%ctrl);
	%value = %ctrl.getText();
	%field = %ctrl.internalName;

	if (!strIsNumeric(%value)) {
		warnLog("Invalid value submitted for:",%field, "Value resetted to 0");
		%value = "0";
	}

	foreach$(%container in $ETransformToolContainers) {
		%textEdit = %container.findObjectByInternalName(%field,true);

		if (!isObject(%textEdit))
			continue;

		%textEdit.setValue(%value);
	}
}
//------------------------------------------------------------------------------

//==============================================================================
//ETransform tool text edit onValidate
function ETransformEdit::onValidate( %this ) {
	ETransformTool.updateTextEditField(%this);
}
//------------------------------------------------------------------------------

//==============================================================================
//ETransform tool text edit onValidate
function ETransformCheck::onClick( %this ) {
	%value = %this.isStateOn();
	%field = %this.internalName;

	foreach$(%container in $ETransformToolContainers) {
		%check = %container.findObjectByInternalName(%field,true);
		%check.setStateOn(%value);
	}
}
//------------------------------------------------------------------------------
function ETransformTool::setFieldValueContainers( %this,%field,%value ) {
	foreach$(%container in $ETransformToolContainers) {
		%textEdit = %container.findObjectByInternalName(%field,true);

		if (!isObject(%textEdit))
			continue;

		%textEdit.setValue(%value);
	}
}

//==============================================================================
function ETransformTool::getSelectionObj( %this ) {
	%obj = EWorldEditor.getSelectedObject( 0 );

	if( !isObject(%obj)) {
		warnLog("No selected object found");
		return "";
	}

	return %obj;
}
//------------------------------------------------------------------------------
//==============================================================================
//ETransformTool.resetAll Field TextEditValue Changed
function ETransformTool::cloneToCtrl( %this,%ctrl ) {
	%ctrl.add(ETransformTool-->toolsStack.deepClone());
	$ETransformToolContainers = strAddWord($ETransformToolContainers,%ctrl.getId(),true);
}
//------------------------------------------------------------------------------



