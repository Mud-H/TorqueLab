function TerrainExportGui::findAllTerrains( %this ) {
	TerrainSelectListBox.clearItems();

	if ( isObject( MegaTerrain ) )
		TerrainSelectListBox.addItem( "MegaTerrain" );

	// Find all of the terrain files
	initContainerTypeSearch( $TypeMasks::TerrainObjectType );

	while ( (%terrainObject = containerSearchNext()) != 0 ) {
		%terrainId = %terrainObject.getId();
		%terrainName = %terrainObject.getName();

		if ( %terrainName $= "" )
			%terrainName = "Unnamed (" @ %terrainId @ ")";

		TerrainSelectListBox.addItem( %terrainName, %terrainId );
	}
}

function TerrainExportGui::init( %this ) {
	%this.findAllTerrains();
}

function TerrainExportGui::export( %this,%isRoot ) {
	%itemId = TerrainSelectListBox.getSelectedItem();
	%terrainObj = TerrainSelectListBox.getItemObject( %itemId );

	if ( !isObject( %terrainObj ) ) {
		LabMsgOK( "Export failed", "Could not find the selected TerrainBlock!" );
		return;
	}

	if (%isRoot)
		%filePath = $TEG_ExportFolderRoot@"/"@$TEG_ExportFolderName;
	else
		%filePath = SelectFolderTextEdit.getText();

	%terrainName = %terrainObj.getName();

	if ( %terrainName $= "" )
		%terrainName = "Unnamed";

	%fileName = %terrainName @ "_heightmap.png";
	%filePrefix = %terrainName @ "_layerMap";
	%ret = %terrainObj.exportHeightMap( %filePath @ "/" @ %fileName, "png" );

	if ( %ret )
		%ret = %terrainObj.exportLayerMaps( %filePath @ "/" @ %filePrefix, "png" );

	if ( %ret )
		%this.close();
}

function TerrainExportGui::onWake( %this ) {
	if (MissionGroup.terrainExportRoot $= "")
		$TEG_ExportFolderRoot = filePath(MissionGroup.getFilename())@"/terrainExport";
	else
		$TEG_ExportFolderRoot = MissionGroup.terrainExportRoot;

	$TEG_ExportFolderName = "Default";
	%this-->rootFolder.text = $TEG_ExportFolderRoot;
	%this-->folderName.text = $TEG_ExportFolderName;
	TerrainExportGui.init();
}

function TerrainExportGui::close( %this ) {
	Canvas.popDialog( %this );
}

function TerrainExportGui::showExportDialog( %this ) {
	%this.findAllTerrains();
	Canvas.pushDialog( %this );
}

function TerrainExportGui::openFolderCallback( %this, %path ) {
	SelectFolderTextEdit.setText( %path );
}

function TerrainExportGui::openRootFolderCallback( %this, %path ) {
	%path = makeRelativePath(%path );
	$TEG_ExportFolderRoot = %path;

	if(theLevelInfo.terrainExportRoot $= %path)
		return;

	theLevelInfo.terrainExportRoot = %path;
	EWorldEditor.isDirty = true;
}

function TerrainExportGui::selectFolder( %this,%isRoot ) {
	if (%isRoot)
		%this.doOpenDialog( "", %this @ ".openRootFolderCallback" );
	else
		%this.doOpenDialog( "", %this @ ".openFolderCallback" );
}

function TerrainExportGui::doOpenDialog( %this, %filter, %callback ) {
	%currentFile = MissionGroup.getFilename();
	%dlg = new OpenFolderDialog() {
		Title = "Select Export Folder";
		Filters = %filter;
		DefaultFile = %currentFile;
		ChangePath = false;
		MustExist = true;
		MultipleFiles = false;
	};

	if(filePath( %currentFile ) !$= "")
		%dlg.DefaultPath = filePath(%currentFile);
	else
		%dlg.DefaultPath = getMainDotCSDir();

	if(%dlg.Execute())
		eval(%callback @ "(\"" @ %dlg.FileName @ "\");");

	%dlg.delete();
}