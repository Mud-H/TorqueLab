//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$ESnapOptions_Initialized = false;
//==============================================================================
function ESnapOptions::ToggleVisibility(%this) {
	ETools.toggleTool("SnapOptions");
	SnapToBar-->snappingSettingsBtn.setStateOn(%this.visible);

	if ( %this.visible  ) {
		//%this.selectWindow();
		%this.setCollapseGroup(false);
		%this.onShow();
	}
}
//------------------------------------------------------------------------------

function ESnapOptions::onShow( %this ) {
	if(!$ESnapOptions_Initialized) {
		ESnapOptions_GridSystemMenu.clear();
		ESnapOptions_GridSystemMenu.add("Metric",0);
		ESnapOptions_GridSystemMenu.add("Power of 2",1);
		%selected = 0;

		if ($WEditor::gridSystem $= "Power of 2")
			%selected = 1;

		ESnapOptions_GridSystemMenu.setSelected(%selected,false);
		//%this.position = %this-->snappingSettingsBtn.position;
		$ESnapOptions_Initialized = true;
	}

	%this-->TabPage_Terrain-->NoAlignment.setStateOn(1);
	%this-->TabPage_Soft-->NoAlignment.setStateOn(1);
	%this-->TabPage_Soft-->RenderSnapBounds.setStateOn(1);
	%this-->TabPage_Soft-->SnapBackfaceTolerance.setText(EWorldEditor.getSoftSnapBackfaceTolerance());
}

function ESnapOptions::hideDialog( %this ) {
	%this.setVisible(false);
	SnapToBar-->snappingSettingsBtn.setStateOn(false);
}


//==============================================================================
// Grid Book Page Functions
//==============================================================================


//==============================================================================
function ESnapOptions_GridSystemMenu::onSelect(%this,%id,%text) {
}
//------------------------------------------------------------------------------
//==============================================================================
function ESnapOptions_GridStepEdit::onValidate(%this) {
	%step = %this.getText();

	if (!strIsNumeric(%step))
		return;

	$WEditor::GridStep = %step;
}
//------------------------------------------------------------------------------

function ESnapOptions::setTerrainSnapAlignment( %this, %val ) {
	EWorldEditor.setTerrainSnapAlignment(%val);
}

function ESnapOptions::setSoftSnapAlignment( %this, %val ) {
	EWorldEditor.setSoftSnapAlignment(%val);
}

function ESnapOptions::setSoftSnapSize( %this ) {
	%val = ESnapOptions-->SnapSize.getText();
	EWorldEditor.setSoftSnapSize(%val);

	if (EditorIsActive())
		EWorldEditor.syncGui();
}



function ESnapOptions::toggleRenderSnapBounds( %this ) {
	EWorldEditor.softSnapRender( ESnapOptionsTabSoft-->RenderSnapBounds.getValue() );
}

function ESnapOptions::toggleRenderSnappedTriangle( %this ) {
	EWorldEditor.softSnapRenderTriangle( ESnapOptionsTabSoft-->RenderSnappedTriangle.getValue() );
}

function ESnapOptions::getSoftSnapBackfaceTolerance( %this ) {
	%val = ESnapOptions-->SnapBackfaceTolerance.getText();
	EWorldEditor.setSoftSnapBackfaceTolerance(%val);
}
