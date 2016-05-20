//==============================================================================
// TorqueLab -> Procedural Terrain Painter GUI script
// Copyright NordikLab Studio, 2014
//==============================================================================

//==============================================================================
// Terrain Import Gui Base Functions
//==============================================================================
//------------------------------------------------------------------------------

//==============================================================================
function TerrainImportGui::toggle(%this) {
	TerrainImportGui.visible = 1;
}
//------------------------------------------------------------------------------

//==============================================================================
function TerrainImportGui::onWake( %this ) {
	hide(TIG_TextureFilePill);
	hide(TIG_TextureMapPill);
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainImportGui::importTerrain( %this ) {
	// Gather all the import settings.
	%heightMapPng = %this-->HeightfieldFilename.getText();
	%metersPerPixel = %this-->MetersPerPixel.getText();
	%heightScale = %this-->HeightScale.getText();
	%flipYAxis = %this-->FlipYAxis.isStateOn();
	// Grab and validate terrain object name.
	%terrainName = %this-->TerrainName.getText();

	if( !( isObject( %terrainName ) && %terrainName.isMemberOfClass( "TerrainBlock" ) ) &&
			!Lab::validateObjectName( %terrainName ) )
		return;

	%opacityNames = "";
	%materialNames = "";
	%opacityList = %this-->OpacityLayerTextList;

	foreach(%fileStack in TIG_TextureMapStack) {
		foreach(%pill in %fileStack) {
			if (isObject(%pill-->MapFile))
				continue;

			%opacityNames = strAddRecord(%opacityNames,%pill.file TAB %pill.channel);
			devLog("Mat name = ",%pill.matId.internalName);
			%materialNamesA = strAddRecord(%materialNamesA,%pill.matId.internalName);

			if (%materialNames $= "")
				%materialNames = %pill.matId.internalName;
			else
				%materialNames = %materialNames TAB %pill.matId.internalName;
		}
	}

	%updated = nameToID( %terrainName );
	// This will update an existing terrain with the name %terrainName,
	// or create a new one if %terrainName isn't a TerrainBlock
	devLog("Importing   opacity:",%opacityNames);
	devLog("Importing  material:",%materialNames);
	devLog("Importing materialA:",%materialNamesA);
	%obj = TerrainBlock::import(   %terrainName,
											 %heightMapPng,
											 %metersPerPixel,
											 %heightScale,
											 %opacityNames,
											 %materialNamesA,
											 %flipYAxis );
	%obj.terrainHeight = %heightScale;

	//Canvas.popDialog( %this );

	if ( isObject( %obj ) ) {
		if( %obj != %updated ) {
			// created a new TerrainBlock
			// Submit an undo action.
			MECreateUndoAction::submit(%obj);
		}

		assert( isObject( EWorldEditor ),
				  "ObjectBuilderGui::processNewObject - EWorldEditor is missing!" );
		// Select it in the editor.
		EWorldEditor.clearSelection();
		EWorldEditor.selectObject(%obj);
		// When we drop the selection don't store undo
		// state for it... the creation deals with it.
		EWorldEditor.dropSelection( true );
		ETerrainEditor.isDirty = true;
		EPainter.updateLayers();
	} else {
		MessageBox( "Import Terrain",
						"Terrain import failed! Check console for error messages.",
						"Ok", "Error" );
	}
}
//------------------------------------------------------------------------------



//==============================================================================
// Load Heightfield file
//==============================================================================
$TerrainImportGui::HeightFieldFilter = "Heightfield Files (*.png, *.bmp, *.jpg, *.gif)|*.png;*.bmp;*.jpg;*.gif|All Files (*.*)|*.*|";
//------------------------------------------------------------------------------

//==============================================================================
function TerrainImportGui::browseForHeightfield( %this ) {
	%this.doOpenDialog( $TerrainImportGui::HeightFieldFilter, "TerrainImportGui_SetHeightfield", "heightmap");
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainImportGui::doOpenDialog( %this, %filter, %callback,%subFolder ) {
	%currentFile = MissionGroup.getFilename();
	%dlg = new OpenFileDialog() {
		Filters = %filter;
		DefaultFile = %currentFile;
		ChangePath = false;
		MustExist = true;
		MultipleFiles = false;
	};
	%terrainPath = filePath( %currentFile )@"/terrain";

	if (%subFolder !$="")
		%terrainPath = %terrainPath @"/"@%subFolder;

	if(filePath( %currentFile ) $= "")
		%dlg.DefaultPath = getMainDotCSDir();
	else if(%terrainPath !$= "")
		%dlg.DefaultPath = %terrainPath;
	else
		%dlg.DefaultPath = filePath(%currentFile);

	if(%dlg.Execute())
		eval(%callback @ "(\"" @ %dlg.FileName @ "\");");

	%dlg.delete();
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainImportGui_SetHeightfield( %name ) {
	%name = makeRelativePath(%name );
	TerrainImportGui-->HeightfieldFilename.setText( %name );
}
//------------------------------------------------------------------------------
//==============================================================================


//------------------------------------------------------------------------------

//==============================================================================
// Load OpacityMap file
//==============================================================================
$TerrainImportGui::OpacityMapFilter = "Opacity Map Files (*.png, *.bmp, *.jpg, *.gif)|*.png;*.bmp;*.jpg;*.gif|All Files (*.*)|*.*|";
//------------------------------------------------------------------------------

//==============================================================================
function TerrainImportGui::browseForOpacityMap( %this ) {
	TerrainImportGui.doOpenDialog( $TerrainImportGui::OpacityMapFilter, "TerrainImportGuiAddOpacityMap","overlays" );
}
//------------------------------------------------------------------------------

//==============================================================================
function TerrainImportGuiAddOpacityMap( %name,%red,%green,%blue ) {
	%txt = makeRelativePath( %name, getWorkingDirectory() );
	%channelsTxt = "R" TAB "G" TAB "B" TAB "A";
	%bitmapInfo = getBitmapinfo( %name );
	%channelCount = getWord( %bitmapInfo, 2 );

	for ( %i = 0; %i < %channelCount; %i++ ) {
		%channel = getWord( %channelsTxt, %i );
		TerrainImportGui.addMapToStack(%txt,%channel);
	}
}
//------------------------------------------------------------------------------

//==============================================================================
// OpacityMap Stack Manipulation
//==============================================================================
//------------------------------------------------------------------------------

//==============================================================================
function TerrainImportGui::addMapToStack( %this,%file,%channel,%matId ) {
	%mapName = fileBase(%file);
	%chanStack = TIG_TextureMapStack.findObjectByInternalName(%mapName);

	if (!isObject(%chanStack)) {
		%chanStack = cloneObject(TIG_TextureFilePill,"",%mapName,TIG_TextureMapStack);
		%chanStack-->MapFile.text = %file;
	}

	%pill = cloneObject(TIG_TextureMapPill,"",%channel,%chanStack);
	%pill.file = %file;
	%pill.channel = %channel;
	//%pill-->MapFile.text = %file;
	%pill-->MapMaterial.text = "Material:\c3 NoMaterial";
	%pill-->MapChannel.text = "Channel:\c3 "@%channel;
	%pill-->MapEdit.command = "TerrainImportGui.showMapMaterialDlg("@%pill.getId()@");";
	%pill-->selChannel.visible = 0;
	%pill-->selMaterial.visible = 0;
}
//------------------------------------------------------------------------------

//==============================================================================
function TerrainImportGui::removeAllMaps( %this ) {
	TIG_TextureMapStack.clear();
}
//------------------------------------------------------------------------------
//==============================================================================
function TIG_ChannelMapArea::onMouseDown( %this,%modifier,%mousePoint,%clicks ) {
	devLog(" TIG_ChannelMapArea::onMouseDown ( %this,%modifier,%mousePoint,%clicks )", %this,%modifier,%mousePoint,%clicks);
	%pill = %this.getParent();

	if ( %clicks > 1)
		TerrainImportGui.showMapMaterialDlg(%pill);
	else
		TerrainImportGui.setMapSelected(%pill);
}
//------------------------------------------------------------------------------

//==============================================================================
function TerrainImportGui::setMapSelected( %this,%pill ) {
	devLog(" TIG_ChannelMapArea::onMouseDown ( %this,%pill )", %this,%pill);

	if (isObject(TerrainImportGui.SelectedMapPill)) {
		TerrainImportGui.SelectedMapPill-->selChannel.visible = 0;
		TerrainImportGui.SelectedMapPill-->selMaterial.visible = 0;
	}

	TerrainImportGui.SelectedMapPill = %pill;
	TerrainImportGui.SelectedMapPill-->selChannel.visible = 1;
	TerrainImportGui.SelectedMapPill-->selMaterial.visible = 1;
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainImportGui::showMapMaterialDlg( %this,%pill ) {
	if (%pill $= "")
		%pill = TerrainImportGui.SelectedMapPill;

	if (!isObject(%pill)) {
		warnLog("No texture map selected");
		return;
	}

	TerrainImportGui.activeMapPill = %pill;
	%matId = %pill.matId;
	TerrainMaterialDlg.showByObjectId( %matId, TIG_TerrainMaterialEditCallback );
}
//------------------------------------------------------------------------------
//==============================================================================
// Callback from TerrainMaterialDlg returning selected material info
function TIG_TerrainMaterialEditCallback( %mat, %matIndex, %activeIdx ) {
	devLog(" TerrainImportGui_TerrainMaterialApplyCallback ( %mat, %matIndex, %activeIdx )", %mat, %matIndex, %activeIdx );

	// Skip over a bad selection.
	if ( !isObject( %mat ) )
		return;

	TerrainImportGui.activeMapPill.matId = %mat;
	%matCtrl = TerrainImportGui.activeMapPill-->MapMaterial;
	%matCtrl.text = "Material:\c3" SPC %mat.internalName;
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainImportGui::removeOpacitymap( %this ) {
	delObj(TerrainImportGui.SelectedMapPill);
}
//------------------------------------------------------------------------------
