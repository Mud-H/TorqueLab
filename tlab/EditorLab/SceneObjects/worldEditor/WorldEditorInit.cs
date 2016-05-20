//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
function WorldEditor::init(%this) {
	// add objclasses which we do not want to collide with
	%this.ignoreObjClass(Sky);
	// editing modes
	WEditorPlugin.numEditModes = 3;
	WEditorPlugin.editMode[0]    = "move";
	WEditorPlugin.editMode[1]    = "rotate";
	WEditorPlugin.editMode[2]    = "scale";
	// context menu
	new GuiControl(WEContextPopupDlg, EditorGuiGroup) {
		profile = "ToolsDefaultProfile_NoModal";
		horizSizing = "width";
		vertSizing = "height";
		position = "0 0";
		extent = "640 480";
		minExtent = "8 8";
		visible = "1";
		setFirstResponder = "0";
		modal = "1";
		new GuiPopUpMenuCtrl(WEContextPopup) {
			profile = "ToolsScrollProfile";
			position = "0 0";
			extent = "0 0";
			minExtent = "0 0";
			maxPopupHeight = "200";
			command = "canvas.popDialog(WEContextPopupDlg);";
		};
	};
	WEContextPopup.setVisible(false);

	// Make sure we have an active selection set.
	if( !%this.getActiveSelection() )
		%this.setActiveSelection( new WorldEditorSelection( EWorldEditorSelection ) );
}

//------------------------------------------------------------------------------
//==============================================================================
// Synchronize WorldEditor GUI parameters with current plugin
function EWorldEditor::syncGui( %this ) {
	if(!EditorGui.isAwake()) {
		return;
	}

	%this.syncToolPalette();


	Editor.getUndoManager().updateUndoMenu( );
	EditorGuiStatusBar.setSelectionObjectsByCount( %this.getSelectionSize() );
	//SceneTreeWindow-->LockSelection.setStateOn( %this.getSelectionLockCount() > 0 );
	SceneEditorToolbar-->boundingBoxColBtn.setStateOn( EWorldEditor.boundingBoxCollision );

	if( EWorldEditor.objectsUseBoxCenter ) {
		SceneEditorToolbar-->centerObject.iconBitmap ="tlab/art/icons/toolbar/SelObjectCenter";
	} else {
		SceneEditorToolbar-->centerObject.iconBitmap ="tlab/art/icons/toolbar/SelObjectBounds";
	}

	if( GlobalGizmoProfile.getFieldValue(alignment) $= "Object" ) {
		SceneEditorToolbar-->objectTransform.iconBitmap ="tlab/art/icons/toolbar/TransformObject";
	} else {
		SceneEditorToolbar-->objectTransform.iconBitmap ="tlab/art/icons/toolbar/TransformWorld";
	}

	%gridSnap = %this.getGridSnap();
	%softSnap = %this.getSoftSnap();
	SceneEditorToolbar-->renderHandleBtn.setStateOn( EWorldEditor.renderObjHandle );
	SceneEditorToolbar-->renderTextBtn.setStateOn( EWorldEditor.renderObjText );
	SceneEditorToolbar-->objectSnapBtn.setStateOn( %softSnap);
	SceneEditorToolbar-->objectGridSnapBtn.setStateOn( %gridSnap );

	//SceneEditorToolbar-->softSnapSizeTextEdit.setText( EWorldEditor.getSoftSnapSize() );
	if (isObject(ESnapOptions)) {
		ESnapOptions-->SnapSize.setText( EWorldEditor.getSoftSnapSize() );
		ESnapOptions-->GridSize.setText( EWorldEditor.getGridSize() );
		ESnapOptions-->GridSnapButton.setStateOn( %gridSnap );
		ESnapOptions-->NoSnapButton.setStateOn( !%this.stickToGround && !%softSnap && !%gridSnap );
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function EWorldEditor::syncToolPalette( %this ) {
	switch$ ( GlobalGizmoProfile.mode ) {
	case "None":
		EWorldEditorNoneModeBtn.performClick();

	case "Move":
		EWorldEditorMoveModeBtn.performClick();

	case "Rotate":
		EWorldEditorRotateModeBtn.performClick();

	case "Scale":
		EWorldEditorScaleModeBtn.performClick();
	}
}
//------------------------------------------------------------------------------

//==============================================================================

function WorldEditor::export(%this) {
	getSaveFilename("~/editor/*.mac|mac file", %this @ ".doExport", "selection.mac");
}
//------------------------------------------------------------------------------
//==============================================================================
function WorldEditor::doExport(%this, %file) {
	missionGroup.save("~/editor/" @ %file, true);
}
//------------------------------------------------------------------------------
//==============================================================================
function WorldEditor::import(%this) {
	getLoadFilename("~/editor/*.mac|mac file", %this @ ".doImport");
}
//------------------------------------------------------------------------------
//==============================================================================
function WorldEditor::doImport(%this, %file) {
	exec("~/editor/" @ %file);
}
//------------------------------------------------------------------------------


