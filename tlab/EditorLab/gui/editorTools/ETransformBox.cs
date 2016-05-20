//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$LabCfg_TransformBox_ShowPosition = true;
$LabCfg_TransformBox_ShowScale = false;
$LabCfg_TransformBox_ShowRotation = true;
$LabCfg_TransformBox_EulerRotation = false;
//==============================================================================
function ETransformBoxGui::initTool( %this ) {
	ETransformBoxGui.fitIntoParents();
	ETransformBox.position = "2000 0";
	hide(ETransformBoxSettings);
	ETransformBox.updateGui();
	hide(ETransformBox);
}
//------------------------------------------------------------------------------
//==============================================================================
function ETransformBox::toggleBox( %this ) {
	if (ETransformBox.notActive)
		%this.activate();
	else
		%this.deactivate();
}
//------------------------------------------------------------------------------
//==============================================================================
function ETransformBox::activate( %this ) {
	ETransformBoxGui.fitIntoParents();
	ETransformBox.notActive = false;
	show(%this);
}
//------------------------------------------------------------------------------
//==============================================================================
function ETransformBox::deactivate( %this ) {
	%this.notActive = true;
	hide(%this);
}
//------------------------------------------------------------------------------
//==============================================================================
function ETransformBox::updateGui( %this ) {
	ETransformBoxGui.fitIntoParents();
	%this-->pos.setVisible($LabCfg_TransformBox_ShowPosition);
	//%this-->scale.setVisible($LabCfg_TransformBox_ShowPosition);
	%this-->rot.setVisible(0);
	%this-->euler.setVisible(0);

	if ($LabCfg_TransformBox_ShowRotation) {
		%this-->rot.setVisible(!$LabCfg_TransformBox_EulerRotation);
		%this-->euler.setVisible($LabCfg_TransformBox_EulerRotation);
	} else {
		%this-->rot.setVisible(0);
		%this-->euler.setVisible(0);
	}

	%this.updateStack();
	ETransformBox.forceInsideCtrl(ETransformBoxGui);
	%setPos = %this.position;
	%setPos.x += %this.extent.x - ETransformBoxSettings.extent.x;
	%setPos.y += %this.extent.y;
	ETransformBoxSettings.position = %setPos;
	ETransformBoxSettings.forceInsideCtrl(ETransformBoxGui);
}
//------------------------------------------------------------------------------
//==============================================================================
function ETransformBox::updateSource( %this,%sourceObj ) {
	if (!isObject(%sourceObj) || %this.notActive) {
		hide(ETransformBox);
		return;
	}

	ETransformBoxGui.fitIntoParents();
	show(ETransformBox);
	ETransformBox.sourceObject = %sourceObj;
	%posCtrl = ETransformBox-->pos;
	%sourcePos = %sourceObj.position;

	foreach$(%axis in "x y z") {
		eval("%posCtrl"@%axis@" = %posCtrl-->"@%axis@";");
		eval("%posCtrl"@%axis@".setText(%sourcePos."@%axis@");");
	}

	%rotCtrl = ETransformBox-->rot;
	%objRot = %sourceObj.rotation;

	foreach$(%axis in "x y z a") {
		eval("%rotCtrl"@%axis@" = %rotCtrl-->"@%axis@";");
		eval("%rotCtrl"@%axis@".setText(%objRot."@%axis@");");
	}

	%eulerCtrl = ETransformBox-->euler;
	%eulerRot = rotationToEuler(%objRot);

	foreach$(%axis in "x y z") {
		eval("%eulerCtrl"@%axis@" = %eulerCtrl-->"@%axis@";");
		eval("%eulerCtrl"@%axis@".setText(%eulerRot."@%axis@");");
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function ETransformBox::updateRotation( %this,%axis,%value ) {
	if (!isObject(%this.sourceObject)) {
		hide(ETransformBox);
		return;
	}

	%sourceObj = %this.sourceObject;
	%objPos = getWords(%sourceObj.getTransform(),0,2);
	%objRot = getWords(%sourceObj.getTransform(),3,6);
	eval("%objRot."@%axis@" = %angle;");
	%newTransform = %objPos SPC %objRot;
	%sourceObj.setTransform(%newTransform);
}
//------------------------------------------------------------------------------
//==============================================================================
function ETransformBox::updateEuler( %this,%axis,%angle ) {
	if (!isObject(%this.sourceObject)) {
		hide(ETransformBox);
		return;
	}

	%sourceObj = %this.sourceObject;
	%objRot = getWords(%sourceObj.getTransform(),3,6);
	%sourceRot = rotationToEuler(%objRot);
	eval("%sourceRot."@%axis@" = %angle;");
	testEulerFromAxisAngle(%sourceRot);
	%newRot = MatrixCreateFromEuler(%sourceRot);
	%newRot = getWords(%newRot,3,6);
	%transform = %sourceObj.getTransform();
	%pos = getWords(%transform,0,2);
	%newTransform = %pos SPC %newRot;
	%sourceObj.setTransform(%newTransform);
}
//------------------------------------------------------------------------------
//==============================================================================
function ETransformBox::updatePosition( %this,%axis,%pos ) {
	if (!isObject(%this.sourceObject)) {
		hide(ETransformBox);
		return;
	}

	%sourceObj = %this.sourceObject;
	%objRot = getWords(%sourceObj.getTransform(),3,6);
	%objPos = getWords(%sourceObj.getTransform(),0,2);
	eval("%objPos."@%axis@" = %pos;");
	%newTransform = %objPos SPC %objRot;
	%sourceObj.setTransform(%newTransform);
}
//------------------------------------------------------------------------------
//==============================================================================
function ETransformBox::refresh( %this ) {
	%this.updateStack();
}
//------------------------------------------------------------------------------
//==============================================================================
function ETransformBoxRotationMode::OnClick( %this ) {
	if (%this.internalName $= "Euler")
		$LabCfg_TransformBox_EulerRotation = true;
	else
		$LabCfg_TransformBox_EulerRotation = false;

	ETransformBox.updateGui();
}
//------------------------------------------------------------------------------
//==============================================================================
function ETransformBoxRot::OnValidate( %this ) {
	ETransformBox.updateRotation(%this.internalName,%this.getText());
}
//------------------------------------------------------------------------------
//==============================================================================
function ETransformBoxEuler::OnValidate( %this ) {
	ETransformBox.updateEuler(%this.internalName,%this.getText());
}
//------------------------------------------------------------------------------
//==============================================================================
function ETransformBoxPos::OnValidate( %this ) {
	ETransformBox.updatePosition(%this.internalName,%this.getText());
}
//------------------------------------------------------------------------------
//==============================================================================
function ETransformBoxDrag::onMouseDragged( %this,%a1,%a2,%a3 ) {
	startDragAndDropCtrl(ETransformBox);
	hide(ETransformBox);
}
//------------------------------------------------------------------------------
//==============================================================================
function ETransformBoxCtrl::DragSuccess( %this,%droppedCtrl,%pos) {
	show(%this);
	%realpos = EditorGui-->WorldContainer.getRealPosition();
	%pos.x = %pos.x - %this.extent.x/2- %realpos.x;
	%pos.y = %pos.y - %this.extent.y/2 - %realpos.y;
	%this.setPosition(%pos.x,%pos.y);
	//%this.forceInsideCtrl(EditorGui-->WorldContainer);
	%this.refresh();
	%this.updateGui();
}