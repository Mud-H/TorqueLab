//==============================================================================
// Lab GuiManager -> Init Lab GUI Manager System
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

function GLab_ProfileInspector::onFieldSelected( %this, %fieldName, %fieldTypeStr, %fieldDoc ) {
	loga("GLab_ProfileInspector::onFieldSelected(%fieldName, %fieldTypeStr, %fieldDoc)",%fieldName, %fieldTypeStr, %fieldDoc);
	GuiEditorProfileFieldInfo.setText( "<font:ArialBold:14>" @ %fieldName @ "<font:ArialItalic:14> (" @ %fieldTypeStr @ ") " NL "<font:Arial:14>" @ %fieldDoc );
}

//---------------------------------------------------------------------------------------------

function GLab_ProfileInspector::onFieldAdded( %this, %object, %fieldName ) {
	loga("GLab_ProfileInspector::onFieldAdded(%object, %fieldName)",%object, %fieldName);
	GLab.setProfileDirty( %object, true );
	// GuiEditor.setProfileDirty( %object, true );
}

//---------------------------------------------------------------------------------------------

function GLab_ProfileInspector::onFieldRemoved( %this, %object, %fieldName ) {
	loga("GLab_ProfileInspector::onFieldRemoved(%object, %fieldName)",%object, %fieldName);
	GLab.setProfileDirty( %object, true );
	//GuiEditor.setProfileDirty( %object, true );
}

//---------------------------------------------------------------------------------------------

function GLab_ProfileInspector::onFieldRenamed( %this, %object, %oldFieldName, %newFieldName ) {
	logd("GLab_ProfileInspector::onFieldRenamed(%object, %oldFieldName, %newFieldName)",%object, %oldFieldName, %newFieldName);
	GLab.setProfileDirty( %object, true );
	//GuiEditor.setProfileDirty( %object, true );
}


function GLab_ProfileInspector::onInspectorFieldModified( %this, %object, %fieldName, %arrayIndex, %oldValue, %newValue ) {
	devLog("GLab_ProfileInspector::onInspectorFieldModified(%object, %fieldName, %arrayIndex, %oldValue, %newValue)",%object, %fieldName, %arrayIndex, %oldValue, %newValue);
	GLab.setProfileDirty( %object, true );

	if (strFind(%arrayIndex,"null"))
		%arrayIndex = "";

	GLab.updateProfileChildsField( %object,%fieldName SPC %arrayIndex,%newValue);
	//GuiEditor.setProfileDirty( %object, true );
}

//---------------------------------------------------------------------------------------------

function GLab_ProfileInspector::onInspectorPreFieldModification( %this, %fieldName, %arrayIndex ) {
	logd("GLab_ProfileInspector::onInspectorPreFieldModification(%fieldName, %arrayIndex)",%fieldName, %arrayIndex);
	GLab.setProfileDirty( $GLab_SelectedObject, true );
}

//---------------------------------------------------------------------------------------------

function GLab_ProfileInspector::onInspectorPostFieldModification( %this ) {
	logd("GLab_ProfileInspector::onInspectorPostFieldModification()");
}

//---------------------------------------------------------------------------------------------

function GLab_ProfileInspector::onInspectorDiscardFieldModification( %this ) {
	logd("GLab_ProfileInspector::onInspectorDiscardFieldModification()");
}