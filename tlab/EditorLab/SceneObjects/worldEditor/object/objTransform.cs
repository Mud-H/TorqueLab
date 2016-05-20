//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// resets the scale and rotation on the selection set
function WorldEditor::resetTransforms(%this) {
	%this.addUndoState();

	for(%i = 0; %i < %this.getSelectionSize(); %i++) {
		%obj = %this.getSelectedObject(%i);
		%transform = %obj.getTransform();
		%transform = setWord(%transform, 3, "0");
		%transform = setWord(%transform, 4, "0");
		%transform = setWord(%transform, 5, "1");
		%transform = setWord(%transform, 6, "0");
		//
		%obj.setTransform(%transform);
		%obj.setScale("1 1 1");
	}
}
//------------------------------------------------------------------------------
//==============================================================================
// Synchronize WorldEditor GUI parameters with current plugin
function Lab::rotateSelection( %this,%rotation,%notLocal ) {
	//Transform to radiant
	%applyRot = %rotation;
	%applyRot.x =mDegToRad(%rotation.x);
	%applyRot.y = mDegToRad(%rotation.y);
	%applyRot.z = mDegToRad(%rotation.z);
	EWorldEditor.transformSelection(false,"",false,true,%applyRot,!%notLocal,false,   "","",false,false);
	
	Lab.rotateSnapSelection();
}
//------------------------------------------------------------------------------
//==============================================================================
// Synchronize WorldEditor GUI parameters with current plugin
function Lab::rotateSnapSelection( %this,%precision,%obj ) {
	if (%precision $= "")
		%precision = 1;

	for(%i = 0; %i < EWorldEditor.getSelectionSize(); %i++) {
		%obj = EWorldEditor.getSelectedObject(%i);
		%trans = %obj.getTransform();
		%trans = %obj.rotation;
		%rot = getWord(%trans,3);
		%rot = mRound(%rot);
		%newTrans = setWord(%trans,3,%rot);
		%obj.rotation = %newTrans;
	
	}
}
//------------------------------------------------------------------------------
