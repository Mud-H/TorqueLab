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

$Pref::MatBrowser::DefaultMaterialFile = "art/textures/customMaterials.cs";
//==============================================================================
$Pref::MatBrowser::CurrentStaticFilter = "MaterialFilterAllArray";
$Pref::MatBrowser::CurrentFilter = ""; //ALL
$Pref::MatBrowser::ThumbnailCountIndex = 3;
$Pref::MatBrowser::ThumbnailCustomCount = "";
$MatBrowser_ThumbPerPage[0] = "20";
$MatBrowser_ThumbPerPage[1] = "40";
$MatBrowser_ThumbPerPage[2] = "75";
$MatBrowser_ThumbPerPage[3] = "100";
$MatBrowser_ThumbPerPage[4] = "150";
$MatBrowser_ThumbPerPage[5] = "200";
$MatBrowser_ThumbPerPage[6] = "All";

$MatBrowser_ThumbSize[0] = "32";
$MatBrowser_ThumbSize[1] = "48";
$MatBrowser_ThumbSize[2] = "64";
$MatBrowser_ThumbSize[3] = "80";
$MatBrowser_ThumbSize[4] = "96";
$MatBrowser_ThumbSize[5] = "128";
$MatBrowser_ThumbSize[6] = "160";
//------------------------------------------------------------------------------

//==============================================================================
if (!isObject(MatBrowserPerMan))
	new PersistenceManager(MatBrowserPerMan);

if (!isObject(MatBrowser))
	newScriptObject("MatBrowser");

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
function LabMaterialBrowser::onWake( %this) {
	MatBrowser.setVisible(1);
}

//==============================================================================
function MatBrowser::showDialog( %this, %selectCallback, %returnType) {
	MatSelector_FilterSamples.visible = false;
	MatSelector_PageTextSample.visible = false;
	MatSelector_PageButtonSample.visible = false;
	MatSelector_MaterialPreviewSample.visible = false;
	MatBrowser_Creator.visible = false;
	MatBrowser.setListFilterText("");
	hide(MatSel_SetAsActiveContainer);

	if (MaterialEditorTools.isAwake())
		show(MatSel_SetAsActiveContainer);

	//FIXME Commented because with update it was staying visible inside hidden container and that was causing an issue
	//if( MatBrowser.isVisible() )
	//return;
	%this.showDialogBase(%selectCallback, %returnType, false);
}
//------------------------------------------------------------------------------
function MatBrowser::showTerrainDialog( %this, %selectCallback, %returnType) {
	%this.showDialogBase(%selectCallback, %returnType, true);
}
//------------------------------------------------------------------------------
function MatBrowser::showDialogBase( %this, %selectCallback, %returnType, %useTerrainMaterials) {
	// Set the select callback
	MatBrowser.selectCallback = %selectCallback;
	MatBrowser.returnType = %returnType;
	MatBrowser.currentStaticFilter = $Pref::MatBrowser::CurrentStaticFilter;
	MatBrowser.currentFilter = $Pref::MatBrowser::CurrentFilter;
	MatBrowser.terrainMaterials = %useTerrainMaterials;
	MatBrowser-->materialPreviewCountPopup.clear();
	%i = 0;

	while($MatBrowser_ThumbPerPage[%i] !$="") {
		MatBrowser-->materialPreviewCountPopup.add( $MatBrowser_ThumbPerPage[%i], %i );
		%i++;
	}

	
	%selected = $Pref::MatBrowser::ThumbnailCountIndex;

	if ($Pref::MatBrowser::ThumbnailCustomCount !$="") {
		MatBrowser-->materialPreviewCountPopup.add( $Pref::MatBrowser::ThumbnailCustomCount, %i );
		%selected = %i;
	}

	MatBrowser-->materialPreviewCountPopup.setSelected( %selected );
	%i = 0;

	while($MatBrowser_ThumbSize[%i] !$="") {
		MatBrowser-->materialPreviewSizePopup.add( $MatBrowser_ThumbSize[%i], %i );
		%i++;
	}

	
	pushDlg(LabMaterialBrowser);
	MatSelector_MaterialsContainer.clear();
	MatBrowser.setVisible(1);
	MatBrowser.buildStaticFilters();
	MatBrowser.selectedMaterial = "";
	MatBrowser.loadMaterialFilters();
}
//------------------------------------------------------------------------------
function MatBrowser::hideDialog( %this ) {
	MatBrowser.breakdown();
	//oldmatSelector.setVisible(0);
	Canvas.popDialog(LabMaterialBrowser);
}
//------------------------------------------------------------------------------
function MatBrowser::breakdown( %this ) {
	$Pref::MatBrowser::CurrentStaticFilter = MatBrowser.currentStaticFilter;
	$Pref::MatBrowser::CurrentFilter = MatBrowser.currentFilter;
	%this.clearFilterArray();
	MatBrowser-->materialSelection.deleteAllObjects();
	MatEdPreviewArray.delete();
	MatBrowser-->materialCategories.deleteAllObjects();
	MaterialFilterAllArray.delete();
	MaterialFilterMappedArray.delete();
	MaterialFilterUnmappedArray.delete();
}
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
// this should create a new material pretty nicely
function MatBrowser::createNewMaterial( %this ) {
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

	if( MatBrowser.currentStaticFilter !$= "MaterialFilterMappedArray" ) {
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
			Command = "MatBrowser.updateSelection( $ThisControl.getParent().getObject(1).internalName, $ThisControl.getParent().getObject(1).bitmap );";
			groupNum = "0";
			text = "";
		};
		%container.add(%previewButton);
		%container.add(%previewBorder);
		// add to the gui control array
		MatBrowser-->materialSelection.add(%container);
	}

	%material.setFilename($Pref::MatBrowser::DefaultMaterialFile);
	// select me
	MatBrowser.updateSelection( %material, "art/textures/core/warnMat.png" );
}
//------------------------------------------------------------------------------
//needs to be deleted with the persistence manager and needs to be blanked out of the matmanager
//also need to update instances... i guess which is the tricky part....
function MatBrowser::showDeleteDialog( %this ) {
	%material = MatBrowser.selectedMaterial;
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
									 "MatBrowser.deleteMaterial( " @ %material @ ", " @ %secondFilter @ ", " @ %secondFilterName @" );",
									 "",
									 "" );
	}
}
//------------------------------------------------------------------------------
function MatBrowser::deleteMaterial( %this, %materialName, %secondFilter, %secondFilterName ) {
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

		for( %j = MatBrowser.staticFilterObjCount; %j < MatBrowser-->tagFilters.getCount() ; %j++ ) {
			if( %materialTag $= MatBrowser-->tagFilters.getObject(%j).getObject(0).filter ) {
				%count = getWord( MatBrowser-->tagFilters.getObject(%j).getObject(0).getText(), 2 );
				%count--;
				MatBrowser-->tagFilters.getObject(%j).getObject(0).setText( %materialTag @ " ( "@ %count @ " )");
			}
		}
	}

	UnlistedMaterials.add( "unlistedMaterials", %materialName );

	if( %materialName.getFilename() !$= "" &&
													%materialName.getFilename() !$= "tlab/gui/oldmatSelector.ed.gui" &&
															%materialName.getFilename() !$= "tlab/materialEditor/scripts/materialEditor.ed.cs" ) {
		MatBrowserPerMan.removeObjectFromFile(%materialName);
		MatBrowserPerMan.saveDirty();
	}

	MatBrowser.preloadFilter();
	//oldmatSelector.selectMaterial( "WarningMaterial" );
}


