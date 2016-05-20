//==============================================================================
// TorqueLab -> DecalEditor Action Manager
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

function DecalEditorGui::createAction(%this, %class, %desc) {
	pushInstantGroup();
	%action = new UndoScriptAction() {
		class = %class;
		superClass = BaseDecalEdAction;
		actionName = %desc;
		tree = DecalEditorTreeView;
	};
	popInstantGroup();
	return %action;
}

function DecalEditorGui::doAction(%this, %action) {
	if (%action.doit())
		%action.addToManager(Editor.getUndoManager());
}

function BaseDecalEdAction::redo(%this) {
	// Default redo action is the same as the doit action
	%this.doit();
}

function BaseDecalEdAction::undo(%this) {
}


function ActionEditNodeDetails::doit(%this) {
	%count = getWordCount(%this.newTransformData);

	if(%this.instanceId !$= "" && %count == 7) {
		%newPos = getWords(%this.newTransformData,0,2);
		%newTangent = getWords(%this.newTransformData,3,5);
		%newSize = getWord(%this.newTransformData,6);
		DecalEditorGui.editDecalDetails( %this.instanceId, %newPos,%newTangent,%newSize );
		//DecalEditorGui.editDecalDetails( %this.instanceId, %this.newTransformData );
		DecalEditorGui.syncNodeDetails();
		DecalEditorGui.selectDecal( %this.instanceId );
		return true;
	}

	return false;
}

function ActionEditNodeDetails::undo(%this) {
	%count = getWordCount(%this.oldTransformData);

	if(%this.instanceId !$= "" && %count == 7) {
		%oldPos = getWords(%this.oldTransformData,0,2);
		%oldTangent = getWords(%this.oldTransformData,3,5);
		%oldSize = getWord(%this.oldTransformData,6);
		DecalEditorGui.editDecalDetails( %this.instanceId, %oldPos,%oldTangent,%oldSize );
		DecalEditorGui.syncNodeDetails();
		DecalEditorGui.selectDecal( %this.instanceId );
	}
}
