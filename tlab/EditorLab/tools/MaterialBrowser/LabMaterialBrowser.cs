//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
// FILTERING:
// - Base ArrayObject containing data:
//    new ArrayObject(MaterialFilterAllArray);
// 	new ArrayObject(MaterialFilterMappedArray);
//	   new ArrayObject(MaterialFilterUnmappedArray);
// - Tags
//==============================================================================

$Pref::MaterialSelector::DefaultMaterialFile = "art/textures/customMaterials.cs";
//==============================================================================
$Pref::MaterialSelector::CurrentStaticFilter = "MaterialFilterAllArray";
$Pref::MaterialSelector::CurrentFilter = ""; //ALL
$Pref::MaterialSelector::ThumbnailCountIndex = 3;
$Pref::MaterialSelector::ThumbnailCustomCount = "";
$MaterialSelector_ThumbPerPage[0] = "20";
$MaterialSelector_ThumbPerPage[1] = "40";
$MaterialSelector_ThumbPerPage[2] = "75";
$MaterialSelector_ThumbPerPage[3] = "100";
$MaterialSelector_ThumbPerPage[4] = "150";
$MaterialSelector_ThumbPerPage[5] = "200";
$MaterialSelector_ThumbPerPage[6] = "All";

$MaterialSelector_ThumbSize[0] = "32";
$MaterialSelector_ThumbSize[1] = "48";
$MaterialSelector_ThumbSize[2] = "64";
$MaterialSelector_ThumbSize[3] = "80";
$MaterialSelector_ThumbSize[4] = "96";
$MaterialSelector_ThumbSize[5] = "128";
$MaterialSelector_ThumbSize[6] = "160";
//------------------------------------------------------------------------------

//==============================================================================
if (!isObject(MaterialSelectorPerMan))
	new PersistenceManager(MaterialSelectorPerMan);

if (!isObject(MaterialSelector))
	newScriptObject("MaterialSelector");

//------------------------------------------------------------------------------
if (!isObject(UnlistedMaterials)) {
	new ArrayObject(UnlistedMaterials);
	UnlistedMaterials.add( "unlistedMaterials", WarningMaterial );
	UnlistedMaterials.add( "unlistedMaterials", materialEd_previewMaterial );
	UnlistedMaterials.add( "unlistedMaterials", notDirtyMaterial );
	UnlistedMaterials.add( "unlistedMaterials", materialEd_cubemapEd_cubeMapPreview );
	UnlistedMaterials.add( "unlistedMaterials", matEdCubeMapPreviewMat );
	UnlistedMaterials.add( "unlistedMaterials", materialEd_justAlphaMaterial );
	UnlistedMaterials.add( "unlistedMaterials", materialEd_justAlphaShader );
}

//------------------------------------------------------------------------------
function materialSelector::showDialog( %this) {
	MaterialSelector.setVisible(1);
}
function materialSelector::showDialog( %this) {
	MaterialSelector.setVisible(1);
}

//------------------------------------------------------------------------------
function LabMaterialBrowser::onWake( %this) {
	MaterialSelector.setVisible(1);
}

//==============================================================================
function MaterialSelector::showDialog( %this, %selectCallback, %returnType) {
	MatSelector_FilterSamples.visible = false;
	MatSelector_PageTextSample.visible = false;
	MatSelector_PageButtonSample.visible = false;
	MatSelector_MaterialPreviewSample.visible = false;
	MaterialSelector_Creator.visible = false;
	MaterialSelector.setListFilterText("");
	hide(MatSel_SetAsActiveContainer);

	if (MaterialEditorTools.isAwake())
		show(MatSel_SetAsActiveContainer);

	//FIXME Commented because with update it was staying visible inside hidden container and that was causing an issue
	//if( MaterialSelector.isVisible() )
	//return;
	%this.showDialogBase(%selectCallback, %returnType, false);
}
//------------------------------------------------------------------------------
function MaterialSelector::showTerrainDialog( %this, %selectCallback, %returnType) {
	%this.showDialogBase(%selectCallback, %returnType, true);
}
//------------------------------------------------------------------------------
function MaterialSelector::showDialogBase( %this, %selectCallback, %returnType, %useTerrainMaterials) {
	// Set the select callback
	MaterialSelector.selectCallback = %selectCallback;
	MaterialSelector.returnType = %returnType;
	MaterialSelector.currentStaticFilter = $Pref::MaterialSelector::CurrentStaticFilter;
	MaterialSelector.currentFilter = $Pref::MaterialSelector::CurrentFilter;
	MaterialSelector.terrainMaterials = %useTerrainMaterials;
	MaterialSelector-->materialPreviewCountPopup.clear();
	%i = 0;

	while($MaterialSelector_ThumbPerPage[%i] !$="") {
		MaterialSelector-->materialPreviewCountPopup.add( $MaterialSelector_ThumbPerPage[%i], %i );
		%i++;
	}

	
	%selected = $Pref::MaterialSelector::ThumbnailCountIndex;

	if ($Pref::MaterialSelector::ThumbnailCustomCount !$="") {
		MaterialSelector-->materialPreviewCountPopup.add( $Pref::MaterialSelector::ThumbnailCustomCount, %i );
		%selected = %i;
	}

	MaterialSelector-->materialPreviewCountPopup.setSelected( %selected );
	%i = 0;

	while($MaterialSelector_ThumbSize[%i] !$="") {
		MaterialSelector-->materialPreviewSizePopup.add( $MaterialSelector_ThumbSize[%i], %i );
		%i++;
	}

	
	pushDlg(LabMaterialBrowser);
	MatSelector_MaterialsContainer.clear();
	MaterialSelector.setVisible(1);
	MaterialSelector.buildStaticFilters();
	MaterialSelector.selectedMaterial = "";
	MaterialSelector.loadMaterialFilters();
}
//------------------------------------------------------------------------------
function MaterialSelector::hideDialog( %this ) {
	MaterialSelector.breakdown();
	//oldmatSelector.setVisible(0);
	Canvas.popDialog(LabMaterialBrowser);
}
//------------------------------------------------------------------------------
function MaterialSelector::breakdown( %this ) {
	$Pref::MaterialSelector::CurrentStaticFilter = MaterialSelector.currentStaticFilter;
	$Pref::MaterialSelector::CurrentFilter = MaterialSelector.currentFilter;
	%this.clearFilterArray();
	MaterialSelector-->materialSelection.deleteAllObjects();
	MatEdPreviewArray.delete();
	MaterialSelector-->materialCategories.deleteAllObjects();
	MaterialFilterAllArray.delete();
	MaterialFilterMappedArray.delete();
	MaterialFilterUnmappedArray.delete();
}
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
// this should create a new material pretty nicely
function MaterialSelector::createNewMaterial( %this ) {
	// look for a newMaterial name to grab
	%material = getUniqueName( "newMaterial" );
	new Material(%material) {
		diffuseMap[0] = "art/textures/core/warnMat";
		mapTo = "unmapped_mat";
		parentGroup = RootGroup;
	};
	// add one to All filter
	MaterialFilterAllArray.add( "", %material.name );
	MaterialFilterAllArrayCheckbox.setText("All ( " @ MaterialFilterAllArray.count() + 1 @ " ) ");
	MaterialFilterUnmappedArray.add( "", %material.name );
	MaterialFilterUnmappedArrayCheckbox.setText("Unmapped ( " @ MaterialFilterUnmappedArray.count() + 1 @ " ) ");

	if( MaterialSelector.currentStaticFilter !$= "MaterialFilterMappedArray" ) {
		// create the new material gui
		%container = new GuiControl() {
			profile = "ToolsDefaultProfile";
			Position = "0 0";
			Extent = "74 85";
			HorizSizing = "right";
			VertSizing = "bottom";
			isContainer = "1";
			new GuiTextCtrl() {
				position = "10 70";
				profile = "ToolsGuiTextCenterProfile";
				extent = "64 16";
				text = %material.name;
			};
		};
		%previewButton = new GuiBitmapButtonCtrl() {
			internalName = %material.name;
			HorizSizing = "right";
			VertSizing = "bottom";
			profile = "ToolsButtonProfile";
			position = "7 4";
			extent = "64 64";
			buttonType = "PushButton";
			bitmap = "art/textures/core/warnMat";
			Command = "";
			text = "Loading...";
			useStates = false;
			new GuiBitmapButtonCtrl() {
				HorizSizing = "right";
				VertSizing = "bottom";
				profile = "ToolsButtonProfile";
				position = "0 0";
				extent = "64 64";
				Variable = "";
				buttonType = "toggleButton";
				bitmap = "tlab/materialEditor/assets/cubemapBtnBorder";
				groupNum = "0";
				text = "";
			};
		};
		%previewBorder = new GuiButtonCtrl() {
			internalName = %material.name@"Border";
			HorizSizing = "right";
			VertSizing = "bottom";
			profile = "ToolsButtonHighlight";
			position = "3 0";
			extent = "72 88";
			Variable = "";
			buttonType = "toggleButton";
			tooltip = %material.name;
			Command = "MaterialSelector.updateSelection( $ThisControl.getParent().getObject(1).internalName, $ThisControl.getParent().getObject(1).bitmap );";
			groupNum = "0";
			text = "";
		};
		%container.add(%previewButton);
		%container.add(%previewBorder);
		// add to the gui control array
		MaterialSelector-->materialSelection.add(%container);
	}

	%material.setFilename($Pref::MaterialSelector::DefaultMaterialFile);
	// select me
	MaterialSelector.updateSelection( %material, "art/textures/core/warnMat.png" );
}
//------------------------------------------------------------------------------
//needs to be deleted with the persistence manager and needs to be blanked out of the matmanager
//also need to update instances... i guess which is the tricky part....
function MaterialSelector::showDeleteDialog( %this ) {
	%material = MaterialSelector.selectedMaterial;
	%secondFilter = "MaterialFilterMappedArray";
	%secondFilterName = "Mapped";

	for( %i = 0; %i < MaterialFilterUnmappedArray.count(); %i++ ) {
		if( MaterialFilterUnmappedArray.getValue(%i) $= %material ) {
			%secondFilter = "MaterialFilterUnmappedArray";
			%secondFilterName = "Unmapped";
			break;
		}
	}

	if( isObject( %material ) ) {
		MessageBoxYesNoCancel("Delete Material?",
									 "Are you sure you want to delete<br><br>" @ %material.getName() @ "<br><br> Material deletion won't take affect until the engine is quit.",
									 "MaterialSelector.deleteMaterial( " @ %material @ ", " @ %secondFilter @ ", " @ %secondFilterName @" );",
									 "",
									 "" );
	}
}
//------------------------------------------------------------------------------
function MaterialSelector::deleteMaterial( %this, %materialName, %secondFilter, %secondFilterName ) {
	if( !isObject( %materialName ) )
		return;

	for( %i = 0; %i <= MaterialFilterAllArray.countValue( %materialName ); %i++) {
		%index = MaterialFilterAllArray.getIndexFromValue( %materialName );
		MaterialFilterAllArray.erase( %index );
	}

	MaterialFilterAllArrayCheckbox.setText("All ( " @ MaterialFilterAllArray.count() - 1 @ " ) ");
	%checkbox = %secondFilter @ "Checkbox";

	for( %k = 0; %k <= %secondFilter.countValue( %materialName ); %k++) {
		%index = %secondFilter.getIndexFromValue( %materialName );
		%secondFilter.erase( %index );
	}

	%checkbox.setText( %secondFilterName @ " ( " @ %secondFilter.count() - 1 @ " ) ");

	for( %i = 0; %materialName.getFieldValue("materialTag" @ %i) !$= ""; %i++ ) {
		%materialTag = %materialName.getFieldValue("materialTag" @ %i);

		for( %j = MaterialSelector.staticFilterObjCount; %j < MaterialSelector-->tagFilters.getCount() ; %j++ ) {
			if( %materialTag $= MaterialSelector-->tagFilters.getObject(%j).getObject(0).filter ) {
				%count = getWord( MaterialSelector-->tagFilters.getObject(%j).getObject(0).getText(), 2 );
				%count--;
				MaterialSelector-->tagFilters.getObject(%j).getObject(0).setText( %materialTag @ " ( "@ %count @ " )");
			}
		}
	}

	UnlistedMaterials.add( "unlistedMaterials", %materialName );

	if( %materialName.getFilename() !$= "" &&
													%materialName.getFilename() !$= "tlab/gui/oldmatSelector.ed.gui" &&
															%materialName.getFilename() !$= "tlab/materialEditor/scripts/materialEditor.ed.cs" ) {
		MaterialSelectorPerMan.removeObjectFromFile(%materialName);
		MaterialSelectorPerMan.saveDirty();
	}

	MaterialSelector.preloadFilter();
	//oldmatSelector.selectMaterial( "WarningMaterial" );
}


