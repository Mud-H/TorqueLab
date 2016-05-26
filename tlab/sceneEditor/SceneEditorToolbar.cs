//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
function SceneEditorPlugin::initToolBar( %this ) {
	%menu = SceneEditorToolbar-->DropTypeMenu;
	%menu.clear();
	%selected = 0;

	foreach$(%type in $Cfg_TLab_Object_DropTypes) {
		%menu.add(%type,%id++);

		if (EWorldEditor.dropType $= %type)
			%selected = %id;
	}

	%menu.setSelected(%selected,false);
}

//==============================================================================
function Lab::toggleObjectCenter( %this,%updateOnly ) {
	if (!%updateOnly)
		EWorldEditor.objectsUseBoxCenter = !EWorldEditor.objectsUseBoxCenter;

	if( EWorldEditor.objectsUseBoxCenter ) {
		SceneEditorToolbar-->centerObject.setBitmap("tlab/art/icons/toolbar/SelObjectCenter.png");
	} else {
		SceneEditorToolbar-->centerObject.setBitmap("tlab/art/icons/toolbar/SelObjectBounds.png");
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::toggleObjectTransform( %this ) {
	if (!%updateOnly) {
		if( GlobalGizmoProfile.getFieldValue(alignment) $= "Object" )
			GlobalGizmoProfile.setFieldValue(alignment, World);
		else
			GlobalGizmoProfile.setFieldValue(alignment, Object);
	}

	if( GlobalGizmoProfile.getFieldValue(alignment) $= "Object" ) {
		SceneEditorToolbar-->objectTransform.setBitmap("tlab/art/icons/toolbar/TransformObject.png");
		SEP_CreatorTools-->objectTransform.setBitmap("tlab/art/icons/toolbar/TransformObject.png");
	} else {
		SceneEditorToolbar-->objectTransform.setBitmap("tlab/art/icons/toolbar/TransformWorld");
		SEP_CreatorTools-->objectTransform.setBitmap("tlab/art/icons/toolbar/TransformWorld");
	}
}
//------------------------------------------------------------------------------

//==============================================================================
function Lab::setSelectionLock( %this,%locked ) {
	EWorldEditor.lockSelection(%locked);
	EWorldEditor.syncGui();
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::setSelectionVisible( %this,%visible ) {
	EWorldEditor.hideSelection(!%visible);
	EWorldEditor.syncGui();
}
//------------------------------------------------------------------------------
//==============================================================================
function SceneEditorPlugin::toggleSnapSlider( %this,%sourceObj ) {
//Canvas.pushDialog(softSnapSizeSliderCtrlContainer);
	%srcPos = %sourceObj.getRealPosition();
	%srcPos.y += %sourceObj.extent.y;
	EOverlay.toggleSlider("2",%srcPos,"range \t 0.1 10 \n altCommand \t SceneEditorPlugin.onSnapSizeSliderChanged($ThisControl);");
}
//------------------------------------------------------------------------------
function SceneEditorPlugin::onSnapSizeSliderChanged(%this,%slider) {
	%value = mFloatLength(%slider.value,1);
	EWorldEditor.setSoftSnapSize(%value );
	EWorldEditor.syncGui();
}
//------------------------------------------------------------------------------------
//==============================================================================
function SceneEditorPlugin::toggleGridSizeSlider( %this,%sourceObj ) {
//Canvas.pushDialog(softSnapSizeSliderCtrlContainer);
	%srcPos = %sourceObj.getRealPosition();
	%srcPos.y += %sourceObj.extent.y;
	%step = $WEditor::gridStep;

	if (%step $= "")
		%step = "0.1";

	%range = %step SPC "10";
	%ticks = getTicksFromRange(%range,%step);
	EOverlay.toggleSlider("2",%srcPos,"range \t "@%range@" \n ticks \t "@%ticks@"\n altCommand \t SceneEditorPlugin.onGridSizeSliderChanged($ThisControl);");
}
//------------------------------------------------------------------------------
function SceneEditorPlugin::onGridSizeSliderChanged(%this,%slider) {
	%value = mFloatLength(%slider.value,1);
	LabToolbarStack-->WorldEditorGridSizeEdit.setText(%value);
	Lab.setGridSize(%value );
	EWorldEditor.syncGui();
}
//------------------------------------------------------------------------------------

