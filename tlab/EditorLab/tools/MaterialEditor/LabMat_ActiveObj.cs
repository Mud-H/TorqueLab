//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================


//==============================================================================
//Close editor call
function LabMat::setCurrentObject(%this, %obj) {
	devlog("LabMat::setCurrentObject",%obj);
	
	LabMat.currentObject = %obj;
	%this.updateObjectMaterials();
}
//------------------------------------------------------------------------------
//==============================================================================
//Close editor call
function LabMat::updateObjectMaterials(%this) {
	devlog("LabMat::updateObjectMaterials");
	
	%obj = LabMat.currentObject;
	%this.setMode();
	LabMatTarget_MatMenu.clear();
	for(%j = 0; %j < %obj.getTargetCount(); %j++) {
			%target = %obj.getTargetName(%j);
			%count = LabMatTarget_MatMenu.getCount();
			
				
			LabMatTarget_MatMenu.add(%target,%j);
   }
   LabMatTarget_MatMenu.setSelected(0);
}
//------------------------------------------------------------------------------
//==============================================================================
// Set Material GUI Mode (Mesh or Standard)
//==============================================================================
// Set GUI depending of if we have a standard Material or a Mesh Material
function LabMat::setMode( %this ) {
	LabMatModeMaterial.setVisible(0);
	LabMatModeTarget.setVisible(0);

	if( isObject(LabMat.currentObject) ) {
	   %sourcePath = LabMat.currentObject.getModelFile();
	   %sourceName = fileName(%sourcePath);
		LabMat.currentMode = "Target";
		LabMatModeTarget.setVisible(1);
		LabMatModeTarget-->selMaterialMapTo.text = %sourceName;
	} else {
		LabMat.currentMode = "Material";
		LabMatModeMaterial.setVisible(1);
		LabMat.currentObject = "";		
	}
}
//------------------------------------------------------------------------------

//==============================================================================
// SubMaterial(Material Target) -- Supports different ways to grab the
// material from the dropdown list. We're here either because-
// 1. We have switched over from another editor with an object locked in the
//    $Lab::materialEditorList variable
// 2. We have selected an object using the Object Editor via the Material Editor
//==============================================================================
function LabMatTarget_MatMenu::onSelect( %this ) {
	

	
   %material = getMapEntry( %this.getText() );

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
		%material.setFileName(LabMatModeTarget-->selMaterialFile.getText());
		eval( "LabMat.currentObject." @ strreplace(%this.getText(),".","_") @ " = " @ %material @ ";");

		if( LabMat.currentObject.isMethod("postApply") )
			LabMat.currentObject.postApply();
	}

	LabMat.prepareActiveMaterial( %material.getId() );
}
//------------------------------------------------------------------------------
