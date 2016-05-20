//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================


function DecalEditorGui::editNodeDetails( %this ) {
	%decalId = DecalEditorGui.selDecalInstanceId;

	if( %decalId == -1 )
		return;

	%nodeDetails = DecalEditorDetailContainer-->nodePosition.getText();
	%nodeDetails = %nodeDetails @ " " @ DecalEditorDetailContainer-->nodeTangent.getText();
	%nodeDetails = %nodeDetails @ " " @ DecalEditorDetailContainer-->nodeSize.getText();

	if( getWordCount(%nodeDetails) == 7 )
		DecalEditorGui.doEditNodeDetails( %decalId, %nodeDetails, false );
}

//------------------------------------------------------------------------------
// Edit node
function DecalEditorGui::doEditNodeDetails(%this, %instanceId, %transformData, %gizmo) {
	%action = %this.createAction(ActionEditNodeDetails, "Edit Decal Transform");
	%action.instanceId = %instanceId;

	if (%action.newTransformData !$= %transformData)
		DecalEd_SaveAllInstanceButton.active = true;

	%action.newTransformData = %transformData;

	if( %gizmo )
		%action.oldTransformData = %this.gizmoDetails;
	else
		%action.oldTransformData = %this.getDecalTransform(%instanceId);

	%this.doAction(%action);
	
	show(DecalEd_SaveAllInstanceButton);
}


function DecalEditorGui::syncNodeDetails( %this ) {
	%decalId = DecalEditorGui.selDecalInstanceId;

	if( %decalId == -1 ){
	   hide(DecalEd_DeleteSelBtn);
		return;
	}
   
   show(DecalEd_DeleteSelBtn);
	%lookupName = DecalEditorGui.getDecalLookupName( %decalId );
	DecalEditorGui.updateInstancePreview( %lookupName.material );
	DecalEd_InstanceProperties-->instanceId.setText(%decalId @ " " @ %lookupName);
	%transformData = DecalEditorGui.getDecalTransform(%decalId);
	DecalEd_InstanceProperties-->nodePosition.setText(getWords(%transformData, 0, 2));
	DecalEd_InstanceProperties-->nodeTangent.setText(getWords(%transformData, 3, 5));
	DecalEd_InstanceProperties-->nodeSize.setText(getWord(%transformData, 6));
}

//DecalEditorGui.saveAllInstances();

function DecalEditorGui::deleteSelInstances( %this ) {
	warnLog("Not saving yet");
}
