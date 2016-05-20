//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
function DecalEditorGui::updateDecalPreview( %this, %material ) {
   %this.updateInstancePreview(%material);
   return;
	if( isObject( %material ) )
		DecalPreviewWindow-->decalPreview.setBitmap( searchForTexture( %material.getId(), %material.diffuseMap[0]) );
	else
		DecalPreviewWindow-->decalPreview.setBitmap("tlab/materialEditor/assets/unknownImage");
}

function DecalEditorGui::updateInstancePreview( %this, %material ) {
	if( isObject( %material ) )
		DecalEd_InstancePreview-->instancePreview.setBitmap( searchForTexture( %material.getId(), %material.diffuseMap[0]) );
	else
		DecalEd_InstancePreview-->instancePreview.setBitmap("tlab/materialEditor/assets/unknownImage");
}

function DecalEditorTools::updatePreviewBackground( %this,%color ) {
   DecalPreviewBackground.color = %color;
	DecalPreviewBackgroundPicker.color = %color;
}


function DecalEditorGui::onWake( %this ) {
}
function DecalEditorGui::onSleep( %this ) {
}


// Stores the information when the gizmo is first used
function DecalEditorGui::prepGizmoTransform( %this, %decalId, %nodeDetails ) {
	DecalEditorGui.gizmoDetails = %nodeDetails;
}

// Activated in onMouseUp while gizmo is dirty
function DecalEditorGui::completeGizmoTransform( %this, %decalId, %nodeDetails ) {
	DecalEditorGui.doEditNodeDetails( %decalId, %nodeDetails, true );
}



function DecalEditorGui::paletteSync( %this, %mode ) {
	%evalShortcut = "LabPaletteArray-->" @ %mode @ ".setStateOn(1);";
	eval(%evalShortcut);
}



function DecalEditorTabBook::onTabSelected( %this, %text, %idx ) {
	if( %idx == 0)
		%showInstance = true;
	else
		%showInstance = false;

	if( %idx == 0) {
		DecalPreviewWindow.text = "Instance Properties";
		DeleteDecalButton.tabSelected = %idx;
	} else {
		DecalPreviewWindow.text = "Template Properties";
		DeleteDecalButton.tabSelected = %idx;
	}
   DecalEditorGui.showInstancePreview(%showInstance);
	
}
function DecalEditorGui::showInstancePreview( %this, %showInstance ) {
	DecalEditorTools-->TemplateProperties.setVisible(!%showInstance);
	DecalEditorTools-->TemplatePreview.setVisible(false);
	DecalEditorWindow-->libraryStack.visible = !%showInstance;
	DecalEditorTools-->InstanceProperties.setVisible(%showInstance);
	DecalEditorTools-->InstancePreview.setVisible(true);
	DecalEditorWindow-->instanceStack.visible = %showInstance;
}






