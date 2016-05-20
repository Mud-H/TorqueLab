//==============================================================================
// TorqueLab -> ShapeLab -> Material Editing
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// Edit the selected object material
//==============================================================================
function ShapeLabMaterials::editSelectedMaterial( %this ) {
	if ( isObject( %this.selectedMaterial ) ) {
		%this.updateSelectedMaterial( false );
		%this.editMaterial(%this.selectedMaterial);
	}
}
function ShapeLabMaterials::editMaterial( %this,%material ) {
	if ( !isObject(%material ))
		return;

	// Remove the highlight effect from the selected material, then switch
	// to the Material Editor
	// Create a temporary TSStatic so the MaterialEditor can query the model's
	// materials.
	pushInstantGroup();
	%this.tempShape = new TSStatic() {
		shapeName = ShapeLab.shape.baseShape;
		collisionType = "None";
	};
	popInstantGroup();
	MaterialEditorTools.currentMaterial = %material;
	MaterialEditorTools.currentObject = $Lab::materialEditorList = %this.tempShape;
	Lab.setEditor(MaterialEditorPlugin);
	show(MEP_CallbackArea);
	MEP_CallbackArea-->callbackButton.text = "Return to ShapeLab";
	MEP_CallbackArea-->callbackButton.command = "ShapeLabMaterials.editMaterialEnd();";
	//ShapeLabSelectWindow.setVisible( false );
	//ShapeLabPropWindow.setVisible( false );
	//EditorGui-->MatEdPropertiesWindow.setVisible( true );
	//EditorGui-->MatEdPreviewWindow.setVisible( true );
	//MatEd_phoBreadcrumb.setVisible( true );
	//MatEd_phoBreadcrumb.command = "ShapeLabMaterials.editSelectedMaterialEnd();";
	//advancedTextureMapsRollout.Expanded = false;
	//materialAnimationPropertiesRollout.Expanded = false;
	//materialAdvancedPropertiesRollout.Expanded = false;
	//MaterialEditorTools.open();
	//MaterialEditorTools.setActiveMaterial( %this.selectedMaterial );
	%id = SubMatBrowser.findText( %this.selectedMapTo );

	if( %id != -1 )
		SubMatBrowser.setSelected( %id );

	Lab.setEditor(MaterialEditorPlugin);
}

function ShapeLabMaterials::editMaterialEnd( %this, %closeEditor ) {
	hide(MEP_CallbackArea);
	Lab.setEditor(ShapeLabPlugin);
	//MaterialEditorTools.quit();
	//EditorGui-->MatEdPropertiesWindow.setVisible( false );
	//EditorGui-->MatEdPreviewWindow.setVisible( false );
	// Delete the temporary TSStatic
	%this.tempShape.delete();

	if( !%closeEditor ) {		
		ShapeLabPropWindow.setVisible( true );
	}
}
//==============================================================================
// ShapeLab -> Material Editing
//==============================================================================


function ShapeLab::updateMaterialList( %this ) {
	ShapeLab_MatPillStack.clear();
	// --- MATERIALS TAB ---
	/*ShapeLabMaterialList.clear();
	ShapeLabMaterialList.addRow( -2, "Name" TAB "Mapped" );
	ShapeLabMaterialList.setRowActive( -2, false );
	ShapeLabMaterialList.addRow( -1, "<none>" );*/
	%count = ShapeLab.shape.getTargetCount();

	for ( %i = 0; %i < %count; %i++ ) {
		%matName = ShapeLab.shape.getTargetName( %i );
		%mapped = getMaterialMapping( %matName );
		
		if (strFind(%matName,"ColorEffectR") && $ShapeLab_HideColorMaterial)
				continue;
		/*	if ( %mapped $= "" )
				ShapeLabMaterialList.addRow( WarningMaterial.getID(), %matName TAB "unmapped" );
			else
				ShapeLabMaterialList.addRow( %mapped.getID(), %matName TAB %mapped );*/
		%this.addMaterialPill(%matName,%mapped);
	}

	//ShapeLabMaterials-->materialListHeader.setExtent( getWord( ShapeLabMaterialList.extent, 0 ) SPC "19" );
}

function ShapeLab::addMaterialPill( %this,%matName,%mapped ) {
	if ( !isObject(%mapped)) {
		%mapped = "Unmapped";
		%mapId =  WarningMaterial.getID();
	} else
		%mapId =  %mapped.getID();

	
				
	hide(ShapeLab_MatPillSource);
	%pill = cloneObject(ShapeLab_MatPillSource,"",%matName,ShapeLab_MatPillStack);
	%pill-->materialName.setText(%matName);
	%pill-->mapName.setText(%mapped);
	%pill-->MouseArea.pill = %pill;
	%pill-->selectedCtrl.visible = 0;
	%pill-->editButton.command = "ShapeLabMaterials.editMaterial("@%mapId@");";
	%pill-->highlightCheck.visible = 0;
	%pill.mapID = %mapId;
	%pill.mat = %matName;
}

function ShapeLabMaterials::updateSelectedMaterial( %this, %highlight ) {
	// Remove the highlight effect from the old selection
	if ( isObject( %this.selectedMaterial ) ) {
		%this.selectedMaterial.diffuseMap[1] = %this.savedMap;
		%this.selectedMaterial.reload();
	}

	// Apply the highlight effect to the new selected material
	%this.selectedMapTo = ShapeLabMaterials.pendingMap;
	%this.selectedMaterial = ShapeLabMaterials.pendingMaterial;
	%this.savedMap = %this.selectedMaterial.diffuseMap[1];

	if ( %highlight && isObject( %this.selectedMaterial ) ) {
		%this.selectedMaterial.diffuseMap[1] = "tlab/shapeLab/images/highlight_material";
		%this.selectedMaterial.reload();
	}
}

function ShapeLab_MatPillMouse::onMouseDown( %this, %mod,%point,%clicks ) {
	devLog("ShapeLab_MatPillMouse PILL:",%this.pill,"THIS",%this);

	foreach(%pill in ShapeLab_MatPillStack)
		%pill-->selectedCtrl.visible = 0;

	%this.pill-->selectedCtrl.visible = 1;
	ShapeLabMaterials.pendingMaterial = %this.pill.mapID;
	ShapeLabMaterials.pendingMap = %this.pill-->mapName.text;
	ShapeLabMaterials.updateSelectedMaterial(ShapeLabMaterials-->highlightMaterial.getValue());
}

