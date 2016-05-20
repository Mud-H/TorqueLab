//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

// Material Editor originally created by Dave Calabrese and Travis Vroman of Gaslight Studios

//==============================================================================
// Helper functions to help create categories and manage category lists
function MaterialEditorTools::updateAllFields(%this) {
	matEd_cubemapEd_availableCubemapList.clear();
}
//------------------------------------------------------------------------------

//==============================================================================
// Select the file in which the material will be saved
//MaterialEditorTools.selectMatFile();
function MaterialEditorTools::selectMatFile( %this ) {
	%file = %this.openFile("File",MaterialEditorTools.currentMaterial.getFileName());

	if (!isFile(%file)) {
		warnLog("Invalid file selected:",%file);
		return;
	}

	if (MaterialEditorTools.currentMaterial.getFileName() $= %file) {
		warnLog("File is the same as current:",%file,"Current:",MaterialEditorTools.currentMaterial.getFileName());
		return;
	}

	MaterialEditorTools.currentMaterial.setFilename(%file);
	MaterialEditorTools.setMaterialDirty();
	%this.setActiveMaterialFile(MaterialEditorTools.currentMaterial);
}
//------------------------------------------------------------------------------

//==============================================================================
function MaterialEditorTools::changeLayer( %this, %layer ) {
	if( MaterialEditorTools.currentLayer == getWord(%layer, 1) )
		return;

	MaterialEditorTools.currentLayer = getWord(%layer, 1);
	MaterialEditorTools.guiSync( materialEd_previewMaterial );
}
//------------------------------------------------------------------------------
//==============================================================================
function MaterialEditorTools::updateMaterialReferences( %this, %obj, %oldName, %newName ) {
	if ( %obj.isMemberOfClass( "SimSet" ) ) {
		// invoke on children
		%count = %obj.getCount();

		for ( %i = 0; %i < %count; %i++ )
			%this.updateMaterialReferences( %obj.getObject( %i ), %oldName, %newName );
	} else {
		%objChanged = false;
		// Change all material fields that use the old material name
		%count = %obj.getFieldCount();

		for( %i = 0; %i < %count; %i++ ) {
			%fieldName = %obj.getField( %i );

			if ( ( %obj.getFieldType( %fieldName ) $= "TypeMaterialName" ) && ( %obj.getFieldValue( %fieldName ) $= %oldName ) ) {
				eval( %obj @ "." @ %fieldName @ " = " @ %newName @ ";" );
				%objChanged = true;
			}
		}

		EWorldEditor.isDirty |= %objChanged;

		if ( %objChanged && %obj.isMethod( "postApply" ) )
			%obj.postApply();
	}
}
//------------------------------------------------------------------------------
//==============================================================================
// Global Material Options

function MaterialEditorTools::updateReflectionType( %this, %type ) {
	if( %type $= "None" ) {
		MaterialEditorPropertiesWindow-->matEd_cubemapEditBtn.setVisible(0);
		//Reset material reflection settings on the preview materials
		MaterialEditorTools.updateActiveMaterial( "cubeMap", "" );
		MaterialEditorTools.updateActiveMaterial( "dynamicCubemap" , false );
		MaterialEditorTools.updateActiveMaterial( "planarReflection", false );
	} else {
		if(%type $= "cubeMap") {
			devLog("ShowCubemap button");
			MaterialEditorPropertiesWindow-->matEd_cubemapEditBtn.setVisible(1);
			MaterialEditorTools.updateActiveMaterial( %type, materialEd_previewMaterial.cubemap );
		} else {
			MaterialEditorTools.updateActiveMaterial( %type, true );
		}
	}
}
//------------------------------------------------------------------------------