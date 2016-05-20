//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================


//==============================================================================
// Build the preview control for the Material DynamicArray
function MatBrowser::buildPreviewArray( %this, %material ) {
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
	//%previewBorder.Command = "MatBrowser.updateSelection( $ThisControl.getParent().getObject(1).internalName, $ThisControl.getParent().getObject(1).bitmap );";
	%previewBorder.Command = "MatBrowser.updateSelection( \""@ %matName@"\", $ThisControl.sourceObj.bitmap );";
	%previewBorder.internalName = %matName@"Border";
	// add to the gui control array
	MatBrowser-->materialSelection.add(%container);
	// add to the array object for reference later
	MatEdPreviewArray.add( %previewButton, %previewImage );
}
//------------------------------------------------------------------------------
//==============================================================================
function MatBrowser::loadImages( %this, %materialNum ) {
	// this will save us from spinning our wheels in case we don't exist
	if( !MatBrowser.visible || !isObject(MatEdPreviewArray) )
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

function MatBrowser::firstPage(%this) {
	MatBrowser.currentPreviewPage = 0;
	MatBrowser.LoadFilter( MatBrowser.currentFilter, MatBrowser.currentStaticFilter );
}
//------------------------------------------------------------------------------
//==============================================================================
function MatBrowser::previousPage(%this) {
	MatBrowser.currentPreviewPage--;

	if( MatBrowser.currentPreviewPage < 0)
		MatBrowser.currentPreviewPage = 0;

	MatBrowser.LoadFilter( MatBrowser.currentFilter, MatBrowser.currentStaticFilter );
}
//------------------------------------------------------------------------------
//==============================================================================
function MatBrowser::nextPage(%this) {
	MatBrowser.currentPreviewPage++;

	if( MatBrowser.currentPreviewPage >= MatBrowser.totalPages)
		MatBrowser.currentPreviewPage = MatBrowser.totalPages - 1;

	if( MatBrowser.currentPreviewPage < 0)
		MatBrowser.currentPreviewPage = 0;

	MatBrowser.LoadFilter( MatBrowser.currentFilter, MatBrowser.currentStaticFilter );
}
//------------------------------------------------------------------------------
//==============================================================================
function MatBrowser::lastPage(%this) {
	MatBrowser.currentPreviewPage = MatBrowser.totalPages - 1;

	if( MatBrowser.currentPreviewPage < 0)
		MatBrowser.currentPreviewPage = 0;

	MatBrowser.LoadFilter( MatBrowser.currentFilter, MatBrowser.currentStaticFilter );
}
//------------------------------------------------------------------------------
//==============================================================================
function MatBrowser::selectPage(%this, %page) {
	MatBrowser.currentPreviewPage = %page;
	MatBrowser.LoadFilter( MatBrowser.currentFilter, MatBrowser.currentStaticFilter );
}
//------------------------------------------------------------------------------
//==============================================================================
function MatBrowser::thumbnailCountUpdate(%this) {
	if ($Pref::MatBrowser::ThumbnailCountIndex $= MatBrowser-->materialPreviewCountPopup.getSelected())
		return;

	$Pref::MatBrowser::ThumbnailCountIndex = MatBrowser-->materialPreviewCountPopup.getSelected();
	MatBrowser.LoadFilter( MatBrowser.currentFilter, MatBrowser.currentStaticFilter );
	MaterialEditorPlugin.setCfg("ThumbnailCountIndex",$Pref::MatBrowser::ThumbnailCountIndex);
}
//------------------------------------------------------------------------------
//==============================================================================
function MatBrowser::thumbnailSizeUpdate(%this,%ctrl) {
	

	if (isObject(%ctrl))
		%index = %ctrl.getValue();
	else
		%index = MatBrowser-->materialPreviewSizePopup.getSelected();

	if ($Pref::MatBrowser::ThumbnailSizeIndex $= %index)
		return;

	$Pref::MatBrowser::ThumbnailSizeIndex = %index;
	%size = $MatBrowser_ThumbSize[$Pref::MatBrowser::ThumbnailSizeIndex];

	if (%size > 1 ) {
		MatSelector_MaterialsContainer.colSize = %size + 12; //Add outbounds
		MatSelector_MaterialsContainer.rowSize = %size + 21; //Add outbounds
		MatSelector_MaterialsContainer.refresh();
		MaterialEditorPlugin.setCfg("ThumbnailCountIndex",$Pref::MatBrowser::ThumbnailSizeIndex);
	}
}
//------------------------------------------------------------------------------
//==============================================================================
// Load the filtered materials (called also when thumbnail count change)
function MatBrowser::buildPages( %this, %dataArray ) {
  %totalData = %dataArray.count();
   %previewsPerPage = MatBrowser-->materialPreviewCountPopup.getTextById( MatBrowser-->materialPreviewCountPopup.getSelected() );

	if (%previewsPerPage $= "All")
		%previewsPerPage = "999999";
		
		MatBrowser.totalPages = mCeil( %totalData / %previewsPerPage );

		//Can we maintain the current preview page, or should we go to page 1?
		if( (MatBrowser.currentPreviewPage * %previewsPerPage) >= %totalData )
			MatBrowser.currentPreviewPage = 0;

    
		// Build out the pages buttons
		MatBrowser.buildPagesButtons( MatBrowser.currentPreviewPage, MatBrowser.totalPages );
		%previewCount = %previewsPerPage;
		%possiblePreviewCount = %totalData - MatBrowser.currentPreviewPage * %previewsPerPage;

		if( %possiblePreviewCount < %previewCount )
			%previewCount = %possiblePreviewCount;

		%start = MatBrowser.currentPreviewPage * %previewsPerPage;

		for( %i = %start; %i < %start + %previewCount; %i++ ) {
			%mat = %dataArray.getValue(%i);

			if (strFind(strlwr(%mat.getName()),strlwr(MatBrowser.filterText),true))
				MatBrowser.buildPreviewArray( %mat );
		}
   }
//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
//==============================================================================
function MatBrowser::buildPagesButtons(%this, %currentPage, %totalPages) {
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
		MatBrowser-->materialPreviewPagesStack.add( %control );
	}

	for( %i = %start; %i < %end; %i++ ) {
		if( %i != %currentPage ) {
			%control = cloneObject(MatSelector_PageButtonSample);
			%control.text = %i+1;
		} else {
			%control = cloneObject(MatSelector_PageTextSample);
			%control.text = %i+1;
		}

		MatBrowser-->materialPreviewPagesStack.add( %control );
	}

	if( %endbracket ) {
		%control = cloneObject(MatSelector_PageTextSample);
		%control.text = "...";
		MatBrowser-->materialPreviewPagesStack.add( %control );
	}
}
//------------------------------------------------------------------------------
