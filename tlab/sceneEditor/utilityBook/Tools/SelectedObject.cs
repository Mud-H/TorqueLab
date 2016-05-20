//==============================================================================
// TorqueLab -> SceneEditor Inspector script
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
// Allow to manage different GUI Styles without conflict
//==============================================================================

//==============================================================================
// Hack to force LevelInfo update after Cubemap change...
//==============================================================================
//==============================================================================

function SETools_NameEdit::onValidate( %this ) {
	Parent::onValidate( %this );
	return;
	if (!isObject(SETools_NameEdit.currentObj))
		return;

	SETools_NameEdit.currentObj.internalName = %this.getText();
}

function SceneEd::setActiveObject( %this,%obj ) {	
	if (!isObject(%obj)){
		%this.setNoActiveObject();
		return;
	}
	SceneEd.activeObject = %obj;
	if (!isObject(SETools_NameEdit))
	   return;
	SETools_NameEdit.currentObj = %obj;
	SETools_NameEdit.setText(%obj.internalName);
	
	if (%obj.isMemberOfClass("SimSet") || !%obj.isMethod("getTransform")){
		SETools_RotationEdit.setText("N/A");
		SETools_PositionEdit.setText("N/A");
		SETools_ScaleEdit.setText("N/A");
		SETools_SkinEdit.setText("N/A");
		return;
	}
   
	%trans = %obj.getTransform();
	%pos = getWords(%trans,0,2);
	%rot = getWords(%trans,3,6);

	SETools_RotationEdit.setText(%rot);
	SETools_PositionEdit.setText(%pos);
	SETools_ScaleEdit.setText(%obj.scale);	
	SETools_SkinEdit.setText(%obj.skin);

}
function SceneEd::setNoActiveObject( %this ) {	
	
	SceneEd.activeObject = "";
	
	SETools_NameEdit.currentObj = "";
	SETools_NameEdit.setText("");
	SETools_RotationEdit.setText("");
	SETools_PositionEdit.setText("");
	SETools_ScaleEdit.setText("");
	SETools_SkinEdit.setText("");
}
function SEP_SelectedObjectEdit::onValidate( %this ) {
	devLog("SEP_SelectedObjectEdit::onValidate",%this,%this.internalName);
	if (!isObject(SceneEd.activeObject)){
		SceneEd.setNoActiveObject();
		return;
	}
	%field = %this.internalName;
	%value = %this.getText();
	switch$(%field){
		case "scale":
			//If only one word found, it's an uniform scaling so repeat it
			if (getWordCount(	%value) < 3){
				%uniScale = getWord(%value,0);
				if (!strIsNumeric(%uniScale))
					return;
				%value = %uniScale SPC %uniScale SPC %uniScale;
			}
	}
	%this.setText(%value);
	SceneEd.activeObject.setFieldValue(%field,%value);

}

