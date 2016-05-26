//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================


//==============================================================================
// Build the preview control for the Material DynamicArray
function MaterialSelector::buildPreviewArray( %this, %material ) {
	%matName = "";

	// CustomMaterials are not available for selection
	if ( !isObject( %material ) || %material.isMemberOfClass( "CustomMaterial" ) )
		return;

	if( %material.isMemberOfClass("TerrainMaterial") ) {
		%matName = %material.getInternalName();

		if( %material.diffuseMap $= "")
			%previewImage = "tlab/materialEditor/assets/warnMat";
		else
			%previewImage = %material.diffuseMap;
	} else if( %material.toneMap[0] $= "" && %material.diffuseMap[0] $= "" && !isObject(%material.cubemap) ) {
		%matName = %material.name;
		%previewImage = "tlab/materialEditor/assets/warnMat";
	} else {
		%matName = %material.name;

		if( %material.toneMap[0] !$= "" )
			%previewImage = %material.toneMap[0];
		else if( %material.diffuseMap[0] !$= "" )
			%previewImage = %material.diffuseMap[0];
		else if( %material.cubemap.cubeFace[0] !$= "" )
			%previewImage = %material.cubemap.cubeFace[0];

		//%previewImage = searchForTexture( %material,  %previewImage );
		// were going to use a couple of string commands in order to properly
		// find out what the img src path is
		// **NEW** this needs to be updated with the above, but has some timing issues
		%materialDiffuse =  %previewImage;
		%materialPath = %material.getFilename();

		if( strchr( %materialDiffuse, "/") $= "" ) {
			%k = 0;

			while( strpos( %materialPath, "/", %k ) != -1 ) {
				%foo = strpos( %materialPath, "/", %k );
				%k = %foo + 1;
			}

			%foobar = getSubStr( %materialPath , %k , 99 );
			%previewImage =  strreplace( %materialPath, %foobar, %previewImage );
		} else
			%previewImage =  strreplace( %materialPath, %materialPath, %previewImage );
	}

	%container = cloneObject(MatSelector_MaterialPreviewSample);
	%container-->text.text = %matName;
	%previewBorder = %container-->button;
	%previewButton = %container-->bitmapButton;
	%previewButton.internalName = %matName;
	%previewBorder.tooltip = %matName;
	%previewBorder.sourceObj = %previewButton;
	//%previewBorder.Command = "MaterialSelector.updateSelection( $ThisControl.getParent().getObject(1).internalName, $ThisControl.getParent().getObject(1).bitmap );";
	%previewBorder.Command = "MaterialSelector.updateSelection( \""@ %matName@"\", $ThisControl.sourceObj.bitmap );";
	%previewBorder.internalName = %matName@"Border";
	// add to the gui control array
	MaterialSelector-->materialSelection.add(%container);
	// add to the array object for reference later
	MatEdPreviewArray.add( %previewButton, %previewImage );
}
//------------------------------------------------------------------------------
//==============================================================================
function MaterialSelector::loadImages( %this, %materialNum ) {
	// this will save us from spinning our wheels in case we don't exist
	if( !MaterialSelector.visible || !isObject(MatEdPreviewArray) )
		return;

	// this schedule is here to dynamically load images
	%previewButton = MatEdPreviewArray.getKey(%materialNum);
	%previewImage = MatEdPreviewArray.getValue(%materialNum);

	if( !isObject(%previewButton )) {	
		return;
	}

	%isFile = isImageFile(%previewImage);

	if (%isFile)
		%previewButton.setBitmap(%previewImage);

	%previewButton.setText("");
	%materialNum++;

	if( %materialNum < MatEdPreviewArray.count() ) {
		%tempSchedule = %this.schedule(64, "loadImages", %materialNum);
		MatEdScheduleArray.add( %tempSchedule, %materialNum );
	}
}


//------------------------------------------------------------------------------
//==============================================================================
// Preview Page Navigation

function MaterialSelector::firstPage(%this) {
	MaterialSelector.currentPreviewPage = 0;
	MaterialSelector.LoadFilter( MaterialSelector.currentFilter, MaterialSelector.currentStaticFilter );
}
//------------------------------------------------------------------------------
//==============================================================================
function MaterialSelector::previousPage(%this) {
	MaterialSelector.currentPreviewPage--;

	if( MaterialSelector.currentPreviewPage < 0)
		MaterialSelector.currentPreviewPage = 0;

	MaterialSelector.LoadFilter( MaterialSelector.currentFilter, MaterialSelector.currentStaticFilter );
}
//------------------------------------------------------------------------------
//==============================================================================
function MaterialSelector::nextPage(%this) {
	MaterialSelector.currentPreviewPage++;

	if( MaterialSelector.currentPreviewPage >= MaterialSelector.totalPages)
		MaterialSelector.currentPreviewPage = MaterialSelector.totalPages - 1;

	if( MaterialSelector.currentPreviewPage < 0)
		MaterialSelector.currentPreviewPage = 0;

	MaterialSelector.LoadFilter( MaterialSelector.currentFilter, MaterialSelector.currentStaticFilter );
}
//------------------------------------------------------------------------------
//==============================================================================
function MaterialSelector::lastPage(%this) {
	MaterialSelector.currentPreviewPage = MaterialSelector.totalPages - 1;

	if( MaterialSelector.currentPreviewPage < 0)
		MaterialSelector.currentPreviewPage = 0;

	MaterialSelector.LoadFilter( MaterialSelector.currentFilter, MaterialSelector.currentStaticFilter );
}
//------------------------------------------------------------------------------
//==============================================================================
function MaterialSelector::selectPage(%this, %page) {
	MaterialSelector.currentPreviewPage = %page;
	MaterialSelector.LoadFilter( MaterialSelector.currentFilter, MaterialSelector.currentStaticFilter );
}
//------------------------------------------------------------------------------
//==============================================================================
function MaterialSelector::thumbnailCountUpdate(%this) {
	if ($Pref::MaterialSelector::ThumbnailCountIndex $= MaterialSelector-->materialPreviewCountPopup.getSelected())
		return;

	$Pref::MaterialSelector::ThumbnailCountIndex = MaterialSelector-->materialPreviewCountPopup.getSelected();
	MaterialSelector.LoadFilter( MaterialSelector.currentFilter, MaterialSelector.currentStaticFilter );
	MaterialEditorPlugin.setCfg("ThumbnailCountIndex",$Pref::MaterialSelector::ThumbnailCountIndex);
}
//------------------------------------------------------------------------------
//==============================================================================
function MaterialSelector::thumbnailSizeUpdate(%this,%ctrl) {
	

	if (isObject(%ctrl))
		%index = %ctrl.getValue();
	else
		%index = MaterialSelector-->materialPreviewSizePopup.getSelected();

	if ($Pref::MaterialSelector::ThumbnailSizeIndex $= %index)
		return;

	$Pref::MaterialSelector::ThumbnailSizeIndex = %index;
	%size = $MaterialSelector_ThumbSize[$Pref::MaterialSelector::ThumbnailSizeIndex];

	if (%size > 1 ) {
		MatSelector_MaterialsContainer.colSize = %size + 12; //Add outbounds
		MatSelector_MaterialsContainer.rowSize = %size + 21; //Add outbounds
		MatSelector_MaterialsContainer.refresh();
		MaterialEditorPlugin.setCfg("ThumbnailCountIndex",$Pref::MaterialSelector::ThumbnailSizeIndex);
	}
}
//------------------------------------------------------------------------------
//==============================================================================
// Load the filtered materials (called also when thumbnail count change)
function MaterialSelector::buildPages( %this, %dataArray ) {
  %totalData = %dataArray.count();
   %previewsPerPage = MaterialSelector-->materialPreviewCountPopup.getTextById( MaterialSelector-->materialPreviewCountPopup.getSelected() );

	if (%previewsPerPage $= "All")
		%previewsPerPage = "999999";
		
		MaterialSelector.totalPages = mCeil( %totalData / %previewsPerPage );

		//Can we maintain the current preview page, or should we go to page 1?
		if( (MaterialSelector.currentPreviewPage * %previewsPerPage) >= %totalData )
			MaterialSelector.currentPreviewPage = 0;

    
		// Build out the pages buttons
		MaterialSelector.buildPagesButtons( MaterialSelector.currentPreviewPage, MaterialSelector.totalPages );
		%previewCount = %previewsPerPage;
		%possiblePreviewCount = %totalData - MaterialSelector.currentPreviewPage * %previewsPerPage;

		if( %possiblePreviewCount < %previewCount )
			%previewCount = %possiblePreviewCount;

		%start = MaterialSelector.currentPreviewPage * %previewsPerPage;

		for( %i = %start; %i < %start + %previewCount; %i++ ) {
			%mat = %dataArray.getValue(%i);

			if (strFind(strlwr(%mat.getName()),strlwr(MaterialSelector.filterText),true))
				MaterialSelector.buildPreviewArray( %mat );
		}
   }
//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
//==============================================================================
function MaterialSelector::buildPagesButtons(%this, %currentPage, %totalPages) {
	// We don't want any more than 8 pages at a time.
	if( %totalPages > 8 ) {
		// We attempt to display up to 2 pages before the current page
		%start = %currentPage - 2;

		if( %start <= 0 ) {
			%start = 0;
			%startbracket = false;
		} else {
			%startbracket = true;
		}

		if( (%totalPages - %start) < 8 ) {
			// Move %start closer to the beginning to maintain 8 pages
			%start = %totalPages - 8;
		}

		%end = %start + 8;

		if( %end >= %totalPages ) {
			%end = %totalPages;
			%endbracket = false;
		} else {
			%endbracket = true;
		}
	} else {
		%start = 0;
		%end = %totalPages;
		%startbracket = false;
		%endbracket = false;
	}

	if( %startbracket ) {
		%control = cloneObject(MatSelector_PageTextSample);
		%control.text = "...";
		MaterialSelector-->materialPreviewPagesStack.add( %control );
	}

	for( %i = %start; %i < %end; %i++ ) {
		if( %i != %currentPage ) {
			%control = cloneObject(MatSelector_PageButtonSample);
			%control.text = %i+1;
		} else {
			%control = cloneObject(MatSelector_PageTextSample);
			%control.text = %i+1;
		}

		MaterialSelector-->materialPreviewPagesStack.add( %control );
	}

	if( %endbracket ) {
		%control = cloneObject(MatSelector_PageTextSample);
		%control.text = "...";
		MaterialSelector-->materialPreviewPagesStack.add( %control );
	}
}
//------------------------------------------------------------------------------
