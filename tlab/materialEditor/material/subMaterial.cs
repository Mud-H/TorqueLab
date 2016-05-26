//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// SubMaterial(Material Target) -- Supports different ways to grab the
// material from the dropdown list. We're here either because-
// 1. We have switched over from another editor with an object locked in the
//    $Lab::materialEditorList variable
// 2. We have selected an object using the Object Editor via the Material Editor
//==============================================================================
function SubMaterialSelector::onSelect( %this ) {
	%material = "";

	if( MaterialEditorTools.currentMeshMode $= "Model" )
		%material = getMapEntry( %this.getText() );
	else
		%material = MaterialEditorTools.currentObject.getFieldValue( %this.getText() );

	%origMat = %material;

	if(%material$="")
		%origMat = %material = %this.getText();

	// if there is no material attached to that objects material field or the
	// object does not have a valid method to grab a material
	if( !isObject( %material ) ) {
		// look for a newMaterial name to grab
		// addiitonally, convert "." to "_" in case we have something like: "base.texname" as a material name
		// at the end we will have generated material name: "base_texname_mat"
		%material = getUniqueName( strreplace(%material, ".", "_") @ "_mat" );
		new Material(%material) {
			diffuseMap[0] = %origMat;
			mapTo = %origMat;
			parentGroup = RootGroup;
		};
		%material.setFileName(MaterialEditorTools-->selMaterialFile.getText());
		eval( "MaterialEditorTools.currentObject." @ strreplace(%this.getText(),".","_") @ " = " @ %material @ ";");

		if( MaterialEditorTools.currentObject.isMethod("postApply") )
			MaterialEditorTools.currentObject.postApply();
	}

	MaterialEditorTools.prepareActiveMaterial( %material.getId() );
}
//------------------------------------------------------------------------------
