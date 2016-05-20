//==============================================================================
// GameLab -> Editor Binds
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// Create the Editor specific ActionMap
delObj(EditorMap);
new ActionMap(EditorMap);

//==============================================================================
// Editor General bind functions
//==============================================================================

function EditorGlobalDelete() {
	if ( isObject( Lab.currentEditor ) )
		Lab.currentEditor.handleDelete();
}

EditorMap.bind(keyboard,"delete",EditorGlobalDelete);
EditorMap.bind(keyboard,"ctrl p",screenshotBind);
//==============================================================================
// Editor mouse movement functions
//==============================================================================

//==============================================================================
function getEditorMouseAdjustAmount(%val) {
	%adjust = $Camera::MouseMoveMultiplier;

	if (%adjust $= "")
		%adjust = 1;

	// based on a default camera FOV of 90'
	return(%val * ($cameraFov / 90) * 0.01) * %adjust;
}
//------------------------------------------------------------------------------
//==============================================================================
function getEditorMouseScrollAdjustAmount(%val) {
	%adjust = $Camera::MouseScrollMultiplier;

	if (%adjust $= "")
		%adjust = 1;

	// based on a default camera FOV of 90'
	return(%val * ($cameraFov / 90) * 0.01) * 22;
}
//------------------------------------------------------------------------------
//==============================================================================
function mouseWheelScroll( %val ) {
	%rollAdj = getEditorMouseScrollAdjustAmount(%val);
	%rollAdj = mClamp(%rollAdj, -mPi()+0.01, mPi()-0.01);
	$mvRoll += %rollAdj; //Maxed at pi in code
}
//------------------------------------------------------------------------------
//==============================================================================
function editorYaw(%val) {
	%yawAdj = getEditorMouseAdjustAmount(%val);

	if(ServerConnection.isControlObjectRotDampedCamera() || EWorldEditor.isMiddleMouseDown()) {
		// Clamp and scale
		%yawAdj = mClamp(%yawAdj, -m2Pi()+0.01, m2Pi()-0.01);
		%yawAdj *= 0.5;
	}

	if($Cfg_Common_Camera_invertXAxis )
		%yawAdj *= -1;

	$mvYaw += %yawAdj;
}
//------------------------------------------------------------------------------
//==============================================================================
function editorPitch(%val) {
	%pitchAdj = getEditorMouseAdjustAmount(%val);

	if(ServerConnection.isControlObjectRotDampedCamera() || EWorldEditor.isMiddleMouseDown()) {
		// Clamp and scale
		%pitchAdj = mClamp(%pitchAdj, -m2Pi()+0.01, m2Pi()-0.01);
		%pitchAdj *= 0.5;
	}

	if( Lab.invertYAxis )
		%pitchAdj *= -1;

	$mvPitch += %pitchAdj;
}
//------------------------------------------------------------------------------
//==============================================================================
function editorWheelFadeScroll( %val ) {
	EWorldEditor.fadeIconsDist += %val * 0.1;

	if( EWorldEditor.fadeIconsDist < 0 )
		EWorldEditor.fadeIconsDist = 0;
}
//------------------------------------------------------------------------------

//==============================================================================
function pressButton0( %val ) {
	$Button0Pressed = %val;
	if (%val &&  Lab.currentControlClass $= "Player")
	   toggleFirstPerson(1);
	
}
//------------------------------------------------------------------------------
//==============================================================================
// Default Camera movement binds

EditorMap.bind( mouse, xaxis, editorYaw );
EditorMap.bind( mouse, yaxis, editorPitch );
EditorMap.bind( mouse, zaxis, mouseWheelScroll );
EditorMap.bind( keyboard, "tab", pressButton0 );

EditorMap.bind( keyboard, "w", moveForward );
EditorMap.bind( keyboard, "s", moveBackward );
EditorMap.bind( keyboard, "a", moveleft );
EditorMap.bind( keyboard, "d", moveright );

EditorMap.bind( mouse, "alt zaxis", editorWheelFadeScroll );
EditorMap.bindCmd( keyboard, "ctrl o", "toggleDlg(LabSettingsDlg);","" );
EditorMap.bindCmd(keyboard, "ctrl z", "Editor.getUndoManager().undo();", "");
EditorMap.bindCmd(keyboard, "ctrl y", "Editor.getUndoManager().redo();", "");
//------------------------------------------------------------------------------

//==============================================================================
// Special Editor Camera binds
//==============================================================================

//==============================================================================
function dropCameraAtPlayer(%val) {
	if (%val)
		cmdServer("Lab.dropCameraAtPlayer");
	//commandToServer('dropCameraAtPlayer');
}
//------------------------------------------------------------------------------
//==============================================================================
function dropPlayerAtCamera(%val) {
	if (%val)
		cmdServer("Lab.dropPlayerAtCamera");
	//commandToServer('DropPlayerAtCamera');
}
//------------------------------------------------------------------------------
//==============================================================================
EditorMap.bind(keyboard, "F8", dropCameraAtPlayer);
EditorMap.bind(keyboard, "F7", dropPlayerAtCamera);
//------------------------------------------------------------------------------
