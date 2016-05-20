//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
/*
%cfgArray.setVal("FIELD",       "DEFAULT" TAB "TEXT" TAB "TextEdit" TAB "" TAB "");
*/
//==============================================================================
//==============================================================================
function TerrainMaterialDlg::checkDirty( %this ) {
	
	%butActive = ETerrainMaterialPersistMan.getDirtyObjectCount() > 0 ? "1" : "0";
	TerMatDlg_RevertBut.active = %butActive;
	//TerMatDlg_ApplyBut.active = %butActive;
	
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainMaterialDlg::setMatDirty( %this,%mat ) {
	%file = %mat.getFileName();
	if (%file $= "")
		%file = "art/terrain/materials.cs";
	ETerrainMaterialPersistMan.setDirty( %mat, %file );
	
	
	%layerCtrl = EPainterStack.findObjectByInternalName(%mat.internalName,true);
	if (isObject(%layerCtrl)){
		%layerCtrl.text = %mat.internalName SPC "*";
		EPainter-->saveDirtyMaterials.active = 1;
	}
	%this.checkDirty();

}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainMaterialDlg::setMatNotDirty( %this,%mat ) {
	
	ETerrainMaterialPersistMan.removeDirty( %mat );
	
	%layerCtrl = EPainterStack.findObjectByInternalName(%mat.internalName,true);
	if (isObject(%layerCtrl))
		%layerCtrl.text = %mat.internalName;
	
	%this.checkDirty();
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainMaterialDlg::saveAllDirty( %this ) {
	%count = ETerrainMaterialPersistMan.getDirtyObjectCount();
	for(%i = 0;%i < %count ; %i++){
		%mat = ETerrainMaterialPersistMan.getDirtyObject(%i);
		%this.setMatNotDirty(%mat);
	}
	ETerrainMaterialPersistMan.saveDirty();
	EPainter-->saveDirtyMaterials.active = 0;
	
	
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainMaterialDlg::saveDirtyMaterial( %this,%mat ) {
	
		
	if (ETerrainMaterialPersistMan.isDirty(%mat))
	{
		ETerrainMaterialPersistMan.saveDirtyObject(%mat);
		%this.setMatNotDirty(%mat);
	}
	
}
//------------------------------------------------------------------------------

//==============================================================================
function TerrainMaterialDlg::saveActiveDirty( %this ) {
	TerrainMaterialDlg.checkMaterialDirty( TerrainMaterialDlg.activeMat,true );
	
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainMaterialDlg::checkMaterialDirty( %this, %mat,%saveIfDirty ) {
	// Skip over obviously bad cases.
	if (  !isObject( %mat ) ||
			!%mat.isMemberOfClass( TerrainMaterial ) )
		return;

	if (!%this.canSaveDirty) {
		warnLog("Save dirty is disabled. Material skipped:",%mat.internalName);
		return;
	}
	
	// Read out properties from the dialog.
	%newInternalName = %this-->internalNameCtrl.getText();
	%newFile = %this-->matFileCtrl.getText();
	
	foreach$(%bmpField in $TerMat_BitmapFields){
		%bmpCtrl = %this.findObjectByInternalName(%bmpField@"Ctrl",true);
		%new[%bmpField] = %bmpCtrl.bitmap $= "tlab/materialEditor/assets/unknownImage" ? "" : %bmpCtrl.bitmap;
	}	
	
	foreach$(%field in $TerMat_GuiFields){
		%ctrl = %this.findObjectByInternalName(%field@"Ctrl",true);
		if (%ctrl.isMemberOfClass("GuiCheckboxCtrl"))
			%new[%field] = %ctrl.isStateOn();
		else
			%new[%field] = %ctrl.getText();
		//eval("%"@%field@" = %ctrl.getText();");		
	}
	
	// If no properties of this materials have changed,
	// return.
	foreach$(%field in $TerMat_AllFields){
		%matVal = %mat.getFieldValue(%field);
		if (%matVal !$= %new[%field]){
			devLog("Mat change detected for field:",%field,"Current = ",%matVal,"New =",%new[%field]);
			%matChanged = true;
		}
	}	
	if (%mat.getFileName() !$= %newFile){
		devLog("Mat change file detected",%mat.getFileName(),"New",%newFile);
		%matFileChanged = true;
	}
	if (!%matChanged && !%matFileChanged)
		return;		

	// Make sure the material name is unique.
	if (%matFileChanged && isFile(%newFile)){
		%mat.setFileName(%newFile);
	}
	else if (%matFileChanged){
		devLog("New file don't exist, let's try to save anyway:",%newFile);
		%mat.setFileName(%newFile);
	}
	
	if( %mat.internalName !$= %newInternalName ) {
		%existingMat = TerrainMaterialSet.findObjectByInternalName( %newInternalName );

		if( isObject( %existingMat ) ) {
			LabMsgOK( "Error",
						 "There already is a terrain material called '" @ %newName @ "'.", "", "" );
			// Get a unique internalName
			%newInternalName = getUniqueInternalName(%newInternalName,TerrainMaterialSet,true);
			
			%this-->internalNameCtrl.setText( %newInternalName );
		} 
		
		%mat.setInternalName( %newInternalName );
	}
	foreach$(%field in $TerMat_AllFields){		
		%mat.setFieldValue(%field,%new[%field]);		
	}	
	
	if (%mat.diffuseMap !$= %newDiffuse)
		%mat.diffuseMapDefault = %newDiffuse;
	if (%mat.diffuseSize !$= %diffuseSize)
		%mat.diffuseSizeDefault = %diffuseSize;
	
	%this.setMatDirty(%mat);
	if (%saveIfDirty)
		%this.saveDirtyMaterial(%mat);

		
}
//------------------------------------------------------------------------------

//==============================================================================
//Call when TerrainMaterialDlg is open and create a copy of all materials
function TerrainMaterialDlg::snapshotMaterials( %this ) {
	if( !isObject( TerrainMaterialDlgSnapshot ) )
		new SimGroup( TerrainMaterialDlgSnapshot );

	%group = TerrainMaterialDlgSnapshot;
	%group.clear();
	%matCount = TerrainMaterialSet.getCount();
	
	//Fill the mat source menu at same time
	TerMatDlg_CreateSourceMenu.clear();
	TerMatDlg_CreateSourceMenu.add("Blank Material",0);
	for( %i = 0; %i < %matCount; %i ++ ) {
		%mat = TerrainMaterialSet.getObject( %i );

		if( !isMemberOfClass( %mat.getClassName(), "TerrainMaterial" ) )
			continue;
		if (%mat.internalName !$= "")
			TerMatDlg_CreateSourceMenu.add(%mat.internalName,%mat.getId());

		%snapshot = new ScriptObject() {
			parentGroup = %group;
			material = %mat;
		};
		foreach$(%field in $TerMat_AllFields SPC $TerMat_DefaultFields){
			%snapshot.setFieldValue(%field,%mat.getFieldValue(%field));
		}		
	}
	TerMatDlg_CreateSourceMenu.setSelected(0);
	
	
}
//------------------------------------------------------------------------------
//==============================================================================
// Restore the materials to the state of when the TerrainMaterialDlg was open
function TerrainMaterialDlg::restoreMaterials( %this ) {
	if( !isObject( TerrainMaterialDlgSnapshot ) ) {
		error( "TerrainMaterial::restoreMaterials - no snapshot present" );
		return;
	}

	%count = TerrainMaterialDlgSnapshot.getCount();

	for( %i = 0; %i < %count; %i ++ ) {
		%obj = TerrainMaterialDlgSnapshot.getObject( %i );
		%mat = %obj.material;
		%mat.setInternalName( %obj.internalName );
			foreach$(%field in $TerMat_AllFields SPC $TerMat_DefaultFields){
			%mat.setFieldValue(%field,%obj.getFieldValue(%field));
		}
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainMaterialDlg::deleteMat( %this ) {
	if( !isObject( %this.activeMat ) )
		return;

	// Cannot delete this material if it is the only one left on the Terrain
	if ( ( ETerrainEditor.getMaterialCount() == 1 ) &&
			( ETerrainEditor.getMaterialIndex( %this.activeMat.internalName ) != -1 ) ) {
		LabMsgOK( "Error", "Cannot delete this Material, it is the only " @
					 "Material still in use by the active Terrain." );
		return;
	}
	%removeIndex = TerrainMaterialSet.getObjectIndex(%this.activeMat);
	
	TerrainMaterialSet.remove( %this.activeMat );
	TerrainMaterialDlgDeleteGroup.add( %this.activeMat );
	%matLibTree = %this-->matLibTree;
	%matLibTree.open( TerrainMaterialSet, false );
	//%matLibTree.selectItem( 1 );
	
	%removeIndex--;
	if (%removeIndex < 0)
		%removeIndex = 0;
	
	%selectMat = TerrainMaterialSet.getObject(%removeIndex);
	TerrainMaterialDlg.schedule(200,"selectObjectInTree",%selectMat);

}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainMaterialDlg::trashImageMap( %this,%mapType ) {
	%ctrl = %this.findObjectByInternalName(%mapType@"Ctrl",true);
	if (!isObject(%ctrl))
		return;
	
	%ctrl.setBitmap("tlab/materialEditor/assets/unknownImage");
}
//------------------------------------------------------------------------------