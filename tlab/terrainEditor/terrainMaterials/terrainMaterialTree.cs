//==============================================================================
// TorqueLab -> Fonts Setup
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//"art/terrains/*.cs";
$TerrainMaterialFolders="art/Terrains/";

//TerrainMaterialDlg.refreshMaterialTree();
//TerrainMaterialDlg.setFilteredMaterialsSet();
//==============================================================================
function TerrainMaterialDlg::refreshMaterialTree( %this,%selected ) {
	// Refresh the material list.
	%matLibTree = %this-->matLibTree;
	%matLibTree.clear();
	%matLibTree.open( FilteredTerrainMaterialsSet, false );
	%matLibTree.buildVisibleTree( true );

	if (%selected !$= "") {
		%item = %matLibTree.findItemByObjectId( %newMat );
	}

	if (%item $= "-1" || %item $= "")
		%item = %matLibTree.getFirstRootItem();

	%matLibTree.selectItem( %item );
	%matLibTree.expandItem( %item );
	%this.activateMaterialCtrls( true );
}
//------------------------------------------------------------------------------

//==============================================================================
function TerrainMaterialDlg::selectObjectInTree( %this,%matObjectId,%selectFirstIfInvalid ) {
	%matLibTree = %this-->matLibTree;
	%matLibTree.clearSelection();
	%item = %matLibTree.findItemByObjectId( %matObjectId );

	if ( %item != -1 ) {
		%matLibTree.selectItem( %item );
		%matLibTree.scrollVisible( %item );
	} else if (%selectFirstIfInvalid) {
		for( %i = 1; %i < %matLibTree.getItemCount(); %i++ ) {
			%terrMat = TerrainMaterialDlg-->matLibTree.getItemValue(%i);

			if( %terrMat.getClassName() $= "TerrainMaterial" ) {
				%matLibTree.selectItem( %i, true );
				%matLibTree.scrollVisible( %i );
				break;
			}
		}
	}
}
//------------------------------------------------------------------------------

//==============================================================================
function TerrainMaterialTreeCtrl::onSelect( %this, %item ) {
	TerrainMaterialDlg.setActiveMaterial( %item );
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainMaterialTreeCtrl::onUnSelect( %this, %item ) {
	if ($TerrainMatDlg_SaveWhenUnselected)
		TerrainMaterialDlg.checkMaterialDirty( %item );
	else
		warnLog("Saving when unselect material is disabled");

	TerrainMaterialDlg.setActiveMaterial( 0 );
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainMaterialDlg::initFiltersData( %this ) {
	%folderMenu = %this-->menuFolders;
	%folderMenu.clear();
	%folderMenu.add("All",0);
	%menuId = 1;

	for(%j=0; %j<getFieldCount($TerrainMaterialFolders); %j++) {
		%pathBase = getField($TerrainMaterialFolders,%j);
		%filePathScript = %pathBase@"*.cs";

		for(%file = findFirstFile(%filePathScript); %file !$= ""; %file = findNextFile(%filePathScript)) {
			//get folder
			%folder = filePath(%file);
			%folderStr = strreplace(%folder,"/","\t");
			%folderCount = getFieldCount(%folderStr);
			%themeId = "";

			for(%i = 0; %i<%folderCount; %i++) {
				%currentFolder = getField(%folderStr,%i);

				if (%currentFolder $= "terrains") {
					%themeId = %i+1;
					%themeFolder = getField(%folderStr,%themeId);
				}

				if (!%added[%themeFolder] && %themeFolder !$= "") {
					%folderMenu.add( %themeFolder,%menuId);
					%added[%themeFolder] = true;
					%menuId++;
				}
			}
		}
	}

	%folderMenu.setSelected(0,false);
	%surfaceMenu = %this-->menuSurfaces;
	%surfaceMenu.clear();
	%surfaceMenu.add("All",0);
	%surfaceMenu.add("Grass",1);
	%surfaceMenu.add("Ground",2);
	%surfaceMenu.add("Rock",3);
	%surfaceMenu.setSelected(0,false);
}
//------------------------------------------------------------------------------

//==============================================================================
function TerrainMaterialDlg::changeFolderFilter( %this ) {
	%this.canSaveDirty = false;
	%folderMenu = %this-->menuFolders;
	%id = %folderMenu.getSelected();
	%folder = %folderMenu.getTextById(%id);
	TerrainMaterialDlg.folderFilter = %folder;
	%this.setFilteredMaterialsSet();
	%matLibTree = %this-->matLibTree;
	%matLibTree.clearSelection();
	%item = %matLibTree.findItemByObjectId( %this.activeMat );

	if ( %item $= "-1" ) {
		%item = FilteredTerrainMaterialsSet.getObject(0);
		%matLibTree.selectItem( %item );
		%matLibTree.scrollVisible( %item );
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainMaterialDlg::changeSurfaceFilter( %this ) {
	%surfaceMenu = %this-->menuSurfaces;
	%id = %surfaceMenu.getSelected();
	%surface = %surfaceMenu.getTextById(%id);
	TerrainMaterialDlg.surfaceFilter = %surface;
	%this.setFilteredMaterialsSet();
}
//------------------------------------------------------------------------------

//==============================================================================
function TerrainMaterialDlg::applyMaterialFilters( %this ) {
	%matLibTree = %this-->matLibTree;
	%matLibTree.clear();
	%matLibTree.open( TerrainMaterialSet, false );
	%currentSelection = %matLibTree.getSelectedItemList();
	%matLibTree.clearSelection();
	%count = %matLibTree.getItemCount();
	%hideMe = false;

	foreach(%mat in TerrainMaterialSet) {
		%item = %matLibTree.findItemByObjectId(%mat.getId());

		if (%this.folderFilter !$= "All") {
			%folderFound = strstr(%mat.getFilename(),%this.folderFilter);

			if (%folderFound $= "-1") {
				//Hide this material
				%hideMe = true;
			}
		}

		if (%this.surfaceFilter !$= "All") {
			%folderFound = strstr(%mat.getFilename(),%this.surfaceFilter);
			%nameFound = strstr(%mat.getName(),%this.surfaceFilter);
			%intnameFound = strstr(%mat.internalName,%this.surfaceFilter);

			if (%folderFound $= "-1" && %nameFound $= "-1" && %intnameFound $= "-1") {
				//Hide this material
				%hideMe = true;
			}
		}

		if (!$Cfg_MaterialEditor_ShowGroundCoverMaterial && (%mat.isGroundCoverMat || %mat.isGroundCoverMat $= "1") )
			%hideMe = true;

		if (%hideMe) {
			%matLibTree.removeItem(%item);
			%matLibTree.hideSelection();
			%matLibTree.clearSelection();
		}
	}

	%matLibTree.clearSelection();

	foreach$(%itemId in %currentSelection)
		%matLibTree.selectItem(%itemId);

	%matLibTree.setFilterText(%this-->materialFilter.getValue());
	%matLibTree.buildVisibleTree( true );
}
//------------------------------------------------------------------------------
//==============================================================================

function TerrainMaterialDlg::setFilteredMaterialsSet( %this,%reset,%selectMat ) {
	FilteredTerrainMaterialsSet.clear();

	if (%reset) {
		%this.folderFilter = "All";
		%this.surfaceFilter = "All";
	}

	%folderFilter = %this.folderFilter;
	%surfaceFilter = %this.surfaceFilter;

	foreach(%mat in TerrainMaterialSet) {
		%hideMe = false;

		if (%this.folderFilter !$= "All") {
			%folderFound = strstr(%mat.getFilename(),%folderFilter);

			if (%folderFound $= "-1") {
				//Hide this material
				%hideMe = true;
			}
		}

		if (%this.surfaceFilter !$= "All") {
			%folderFound = strstr(%mat.getFilename(),%surfaceFilter);
			%nameFound = strstr(%mat.getName(),%surfaceFilter);
			%intnameFound = strstr(%mat.internalName,%surfaceFilter);

			if (%folderFound $= "-1" && %nameFound $= "-1" && %intnameFound $= "-1") {
				//Hide this material
				%hideMe = true;
			}
		}

		if (!$Cfg_MaterialEditor_ShowGroundCoverMaterial && (%mat.isGroundCoverMat || %mat.isGroundCoverMat $="1") )
			%hideMe = true;

		if (!%hideMe) {
			FilteredTerrainMaterialsSet.add(%mat);
		}
	}

	%this.refreshMaterialTree(%selectMat);
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainMaterialDlg::updateFilterText( %this ) {
	%matLibTree = %this-->matLibTree;
	%matLibTree.setFilterText(%this-->materialFilter.getValue());
	%this.refreshMaterialTree();
}