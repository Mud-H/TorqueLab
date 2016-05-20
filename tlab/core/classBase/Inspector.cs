//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
/*
virtual Script 	inspect ((string this, string obj))
virtual Script 	onBeginCompoundEdit ((string this))
virtual Script 	onCancelCompoundEdit ((string this))
virtual Script 	onEndCompoundEdit ((string this))
virtual Script 	onFieldSelected ((string this, string fieldName, string fieldTypeStr, string fieldDoc))
virtual Script 	onInspectorDiscardFieldModification ((string this))
virtual Script 	onInspectorFieldModified ((string this, string object, string fieldName, string arrayIndex, string oldValue, string newValue))
virtual Script 	onInspectorPostFieldModification ((string this))
virtual Script 	onInspectorPreFieldModification ((string this, string fieldName, string arrayIndex))
*/
//==============================================================================

$Cfg_TLab_Class_Inspector_LogLevel = 0;
function GuiInspector::clog( %this, %a1,%a2,%a3, %a4,%a5,%a6, %a7,%a8,%a9, %a10,%a11,%a12 ) {
	if($Cfg_TLab_Class_Inspector_LogLevel > 0)
		devLog(%a1,%a2,%a3, %a4,%a5,%a6, %a7,%a8,%a9, %a10,%a11,%a12);
}

function Inspector::inspect( %this, %object ) {
	%this.cLog("GuiInspector::inspect","%this" SPC %this,"%object" SPC %object);
}

function Inspector::onBeginCompoundEdit( %this ) {
	%this.cLog("GuiInspector::onBeginCompoundEdit","%this" SPC %this);
}

function Inspector::onCancelCompoundEdit( %this ) {
	%this.cLog("GuiInspector::onCancelCompoundEdit","%this" SPC %this);
}

function Inspector::onEndCompoundEdit( %this ) {
	%this.cLog("GuiInspector::onEndCompoundEdit","%this" SPC %this);
}

function Inspector::onInspectorDiscardFieldModification( %this ) {
	%this.cLog("GuiInspector::onInspectorDiscardFieldModification","%this" SPC %this);
}

function Inspector::onInspectorFieldModified( %this, %object, %fieldName, %arrayIndex, %oldValue, %newValue ) {
	%this.cLog("GuiInspector::onInspectorFieldModified","%this" SPC %this,"%object" SPC %object,"%fieldName" SPC %fieldName,"%arrayIndex" SPC %arrayIndex,"%oldValue" SPC %oldValue,"%newValue" SPC %newValue);
}


function Inspector::onInspectorPostFieldModification( %this ) {
	%this.cLog("GuiInspector::onInspectorPostFieldModification","%this" SPC %this);
}


function Inspector::onInspectorPreFieldModification( %this, %fieldName, %arrayIndex ) {
	%this.cLog("GuiInspector::onInspectorPreFieldModification","%this" SPC %this,"%fieldName" SPC %fieldName,"%arrayIndex" SPC %arrayIndex);
}


