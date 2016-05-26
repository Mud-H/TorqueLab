//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================


//==============================================================================
function MaterialSelector::selectMaterial( %this, %material ) {
	%name = "";

	if( MaterialSelector.terrainMaterials ) {
		%name = %material;
		%material = TerrainMaterialSet.findObjectByInternalName( %material );
	} else {
		%name = %material.getName();
	}

	// The callback function should be ready to intake the returned material
	//eval("materialEd_previewMaterial." @ %propertyField @ " = " @ %value @ ";");
	if( MaterialSelector.returnType $= "name" )
		eval( "" @ MaterialSelector.selectCallback @ "(" @ %name  @ ");");
	else if( MaterialSelector.returnType $= "index" ) {
		%index = -1;

		if( MaterialSelector.terrainMaterials ) {
			// Obtain the index into the terrain's material list
			%mats = ETerrainEditor.getMaterials();

			for(%i = 0; %i < getRecordCount( %mats ); %i++) {
				%matInternalName = getRecord( %mats, %i );

				if( %matInternalName $= %name ) {
					%index = %i;
					break;
				}
			}
		} else {
			// Obtain the index into the material set
			for(%i = 0; %i < materialSet.getCount(); %i++) {
				%obj = materialSet.getObject(%i);

				if( %obj.getName() $= %name ) {
					%index = %i;
					break;
				}
			}
		}

		eval( "" @ MaterialSelector.selectCallback @ "(" @ %index  @ ");");
	} else
		eval( "" @ MaterialSelector.selectCallback @ "(" @ %material.getId()  @ ");");

	MaterialSelector.hideDialog();
}
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
function MaterialSelector::updateSelection( %this, %material, %previewImagePath ) {
	logd("MaterialSelector::updateSelection( %this, %material, %previewImagePath )",%this, %material, %previewImagePath );
	// the material selector will visually update per material information
	// after we move away from the material. eg: if we remove a field from the material,
	// the empty checkbox will still be there until you move fro and to the material again
	%isMaterialBorder = 0;
	eval("%isMaterialBorder = isObject(MaterialSelector-->"@%material@"Border);");

	if( %isMaterialBorder ) {
		eval( "MaterialSelector-->"@%material@"Border.setStateOn(1);");
	}

	%isMaterialBorderPrevious = 0;
	eval("%isMaterialBorderPrevious = isObject(MaterialSelector-->"@$prevSelectedMaterialHL@"Border);");

	if( %isMaterialBorderPrevious ) {
		eval( "MaterialSelector-->"@$prevSelectedMaterialHL@"Border.setStateOn(0);");
	}

	MaterialSelector-->materialCategories.deleteAllObjects();
	MaterialSelector.selectedMaterial = %material;
	MaterialSelector.selectedPreviewImagePath = %previewImagePath;
	MaterialSelector-->previewSelectionText.setText( %material );
	MaterialSelector-->previewSelection.setBitmap( %previewImagePath );

	// running through the existing list of categorynames in the left, so yes
	// some might exist on the left only temporary if not given a home
	for( %i = MaterialSelector.staticFilterObjCount; %i < MaterialSelector-->tagFilters.getCount() ; %i++ ) {
		%filter = MaterialSelector-->tagFilters.getObject(%i).getObject(0).filter;
		%checkbox = new GuiCheckBoxCtrl() {
			materialName = %material.name;
			Profile = "ToolsCheckBoxProfile";
			position = "5 2";
			Extent = "118 18";
			Command = "MaterialSelector.updateMaterialTags( $ThisControl.materialName, $ThisControl.getText(), $ThisControl.getValue() );";
			text = %filter;
		};
		MaterialSelector-->materialCategories.add( %checkbox );
		// crawl through material for categories in order to check or not
		%filterFound = 0;

		for( %j = 0; %material.getFieldValue("materialTag" @ %j) !$= ""; %j++ ) {
			%tag = %material.getFieldValue("materialTag" @ %j);

			if( %tag  $= %filter ) {
				%filterFound = 1;
				break;
			}
		}

		if( %filterFound  )
			%checkbox.setStateOn(1);
		else
			%checkbox.setStateOn(0);
	}

	$prevSelectedMaterialHL = %material;
}
