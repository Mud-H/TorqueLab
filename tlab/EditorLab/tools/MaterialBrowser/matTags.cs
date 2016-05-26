//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//------------------------------------------------------------------------------
function MaterialSelector::updateMaterialTags( %this, %material, %tag, %tagValue ) {
	if( %tagValue == 1 ) {
		MaterialFilterAllArray.add( %tag, %material );

		if( %material.mapTo $= "" || %material.mapTo $= "unmapped_mat" )
			%secondStaticFilter = MaterialFilterUnmappedArray;
		else
			%secondStaticFilter = MaterialFilterMappedArray;

		%secondStaticFilter.add( %tag, %material );
		%createdTag = 0;

		for( %i = 0; %createdTag == 0; %i++ ) {
			%materialTag = %material.getFieldValue("materialTag" @ %i);

			if( %materialTag $= "" ) {
				eval( %material @ ".materialTag" @ %i @ "=" @ %tag @ ";" );
				%createdTag = 1;

				for( %j = MaterialSelector.staticFilterObjCount; %j < MaterialSelector-->tagFilters.getCount() ; %j++ ) {
					if( %tag $= MaterialSelector-->tagFilters.getObject(%j).getObject(0).filter ) {
						%count = getWord( MaterialSelector-->tagFilters.getObject(%j).getObject(0).getText(), 2 );
						%count++;
						MaterialSelector-->tagFilters.getObject(%j).getObject(0).setText( %tag @ " ( "@ %count @ " )");
					}
				}

				break;
			}
		}
	} else {
		// Remove the material from the "all" category
		for( %i = 0; %i < MaterialFilterAllArray.count(); %i++ ) {
			if( MaterialFilterAllArray.getKey(%i) $= %tag ) {
				if( MaterialFilterAllArray.getValue(%i) $= %material ) {
					MaterialFilterAllArray.erase(%i);
					break;
				}
			}
		}

		// Figure out what the material's other category is
		if( %material.mapTo $= "" || %material.mapTo $= "unmapped_mat" )
			%secondStaticFilter = MaterialFilterUnmappedArray;
		else
			%secondStaticFilter = MaterialFilterMappedArray;

		// Remove the material from its other category
		for( %i = 0; %i < %secondStaticFilter.count(); %i++ ) {
			if( %secondStaticFilter.getKey(%i) $= %tag ) {
				if( %secondStaticFilter.getValue(%i) $= %material ) {
					%secondStaticFilter.erase( %i );
					break;
				}
			}
		}

		MaterialSelector.updateFilterCount( %tag, false );
		%tagField = MaterialSelector.getTagField( %material, %tag );
		%lastTagField = MaterialSelector.getLastTagField( %material );
		%lastValidTagField = MaterialSelector.getLastValidTagField( %material, %tag );

		if( %tagField $= %lastValidTagField || %lastValidTagField $= "" ) {
			MaterialSelectorPerMan.removeField( %material, %tagField );
		} else {
			// Replace the current tagFieldValue with the last tagFieldValue
			%lastValidTag = %material.getFieldValue( %lastValidTagField );
			%material.setFieldValue( %tagField, %lastValidTag );
			// Remove the last tagFieldValue
			MaterialSelectorPerMan.removeField( %material, %lastTagField );
		}
	}

	// so were not going to save materials that dont current exist...
	// technically all the data is stored in dynamic fields if the user feels like saving
	// their auto-generated or new material
	if( %material.getFilename() !$= "" &&
											  %material.getFilename() !$= "tlab/gui/oldmatSelector.ed.gui" &&
													  %material.getFilename() !$= "tlab/materialEditor/scripts/materialEditor.ed.cs"  ) {
		MaterialSelectorPerMan.setDirty( %material );
		MaterialSelectorPerMan.saveDirty();
		MaterialSelectorPerMan.removeDirty( %material );

		if(!%tagValue)
			%material.setFieldValue( %lastTagField, "" );
	}
}


//------------------------------------------------------------------------------
// Tagging Functionality

function MaterialSelector::getTagField( %this, %material, %tag ) {
	for( %i = 0; %material.getFieldValue("materialTag" @ %i) !$= ""; %i++ ) {
		%loopTag = %material.getFieldValue("materialTag" @ %i);

		if( %tag $= %loopTag ) {
			%tagField = "materialTag" @ %i;
			break;
		}
	}

	return %tagField;
}
//------------------------------------------------------------------------------
function MaterialSelector::getLastTagField( %this, %material ) {
	for( %i = 0; %material.getFieldValue("materialTag" @ %i) !$= ""; %i++ ) {
		%tagField = "materialTag" @ %i;
	}

	return %tagField;
}
//------------------------------------------------------------------------------
function MaterialSelector::getLastValidTagField( %this, %material, %invalidTag ) {
	for( %i = 0; %material.getFieldValue("materialTag" @ %i) !$= ""; %i++ ) {
		%tag = %material.getFieldValue("materialTag" @ %i);

		// Can't equal our invalid tag
		if( %tag $= %invalidTag )
			continue;

		// Set our last found tag
		%tagField = "materialTag" @ %i;
	}

	return %tagField;
}
