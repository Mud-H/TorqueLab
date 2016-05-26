//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//MaterialEditorTools.prepareActiveObject
//==============================================================================
function MaterialEditorTools::prepareActiveObject( %this, %override ) {
	%obj = $Lab::materialEditorList;
	$MEP_BaseObjectPath = "";
	$MEP_BaseObject = "";
   
	if (isObject(MaterialEditorTools.currentObject)) {
	   %obj = MaterialEditorTools.currentObject;
		$MEP_BaseObject = MaterialEditorTools.currentObject;
		$MEP_BaseObjectPath  = MaterialEditorTools.currentObject.shapeName;
	}

	if( MaterialEditorTools.currentObject == %obj && !%override)
		return;

	// TSStatics and ShapeBase objects should have getModelFile methods
	if( %obj.isMethod( "getModelFile" ) ) {
		MaterialEditorTools.currentObject = %obj;
		SubMaterialSelector.clear();
		MaterialEditorTools.currentMeshMode = "Model";
		MaterialEditorTools.setMode();

		for(%j = 0; %j < MaterialEditorTools.currentObject.getTargetCount(); %j++) {
			%target = MaterialEditorTools.currentObject.getTargetName(%j);
			%count = SubMaterialSelector.getCount();
			if (strFind(%target,"ColorEffectR") && $MatEd_HideColorMaterial)
				continue;
				
			SubMaterialSelector.add(%target);
		}
	} else { // Other classes that support materials if possible
		%canSupportMaterial = false;

		for( %i = 0; %i < %obj.getFieldCount(); %i++ ) {
			%fieldName = %obj.getField(%i);

			if( %obj.getFieldType(%fieldName) !$= "TypeMaterialName" )
				continue;

			if( !%canSupportMaterial ) {
				MaterialEditorTools.currentObject = %obj;
				SubMaterialSelector.clear();
				SubMaterialSelector.add(%fieldName, 0);
			} else {
				%count = SubMaterialSelector.getCount();
				SubMaterialSelector.add(%fieldName, %count);
			}

			%canSupportMaterial = true;
		}

		if( !%canSupportMaterial ) // Non-relevant classes get returned
			return;

		MaterialEditorTools.currentMeshMode = "EditorShape";
		MaterialEditorTools.setMode();
	}

	%id = SubMaterialSelector.findText( MaterialEditorTools.currentMaterial.mapTo );

	if( %id != -1 )
		SubMaterialSelector.setSelected( %id );
	else
		SubMaterialSelector.setSelected(0);
}
//------------------------------------------------------------------------------
