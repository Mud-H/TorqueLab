//==============================================================================
// TorqueLab -> TerrainMaterial Creator
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// TerrainMaterial Creator
//==============================================================================
//==============================================================================
function TerrainMaterialDlg::createNewMaterial( %this,%clone ) {
	if (%clone){
		%src = %this.activeMat;
		%internalName = %src.internalName;
		if (isObject(%src)) {		
			TerMatDlg_CreateSourceMenu.setSelected(%src.getId());	
			TerrainMaterialDlg-->cloneFileName.setText(%src.getFileName());
		}
		else
			TerMatDlg_CreateSourceMenu.setSelected(0);
	}
	else {
		TerMatDlg_CreateSourceMenu.setSelected(0);
		TerrainMaterialDlg-->cloneFileName.setText("art/terrain/materials.cs");
	}	
	
	TerrainMatDlg_Cloning.expanded = true;
	if (%internalName $= "")
		%internalName = "NewMaterial";		
		
	TerrainMaterialDlg-->cloneInternalName.setText(%internalName);
}
function TerMatDlg_CreateSourceMenu::onSelect( %this,%sourceId,%internalName) {
	if (!isObject(%sourceId)){
		%file = "art/terrain/materials.cs";
		%name = "NewMaterial";
	}
	else {
		%file = %sourceId.getFileName();
		%name = getUniqueInternalName( %sourceId.internalName , TerrainMaterialSet, true );
	}
	TerrainMaterialDlg-->cloneFileName.setText(%file);
	TerrainMaterialDlg-->cloneInternalName.setText(%name);
}

//==============================================================================
function TerrainMaterialDlg::doMaterialCreate( %this,%sourceId,%internalName) {
	TerrainMatDlg_Cloning.expanded = false;
	if (%sourceId $= "")
		%sourceId = TerMatDlg_CreateSourceMenu.getSelected();
	
	if (%sourceId <= 0)
		%isBlankMat = true;
		
	if (%internalName $= "")
		%internalName = TerrainMaterialDlg-->cloneInternalName.getText();
	
	%matName = getUniqueInternalName( %internalName, TerrainMaterialSet, true );
	
	%fileName = TerrainMaterialDlg-->cloneFileName.getText();
	// Create the new material.
	%newMat = new TerrainMaterial() {
		internalName = %matName;
		parentGroup = TerrainMaterialDlgNewGroup;
	};
	
	
	if (isObject(%sourceId) && %sourceId.isMemberOfClass("TerrainMaterial")) 
		%newMat.assignFieldsFrom(%sourceId);
	%newMat.setInternalName( %matName );
	%newMat.setFileName( %fileName );
	
	
	
	// Mark it as dirty and to be saved in the default location.
	ETerrainMaterialPersistMan.setDirty( %newMat, %fileName );
	%this.setFilteredMaterialsSet(true,%newMat);
	
	TerrainMaterialDlg.schedule(200,"selectObjectInTree",%newMat);
}
//==============================================================================
// TerrainMaterial OLD Creator
//==============================================================================
//==============================================================================


//==============================================================================
function TerrainMatNameEdit::onValidate( %this ) {
	if (isObject(TerrainMaterialSet.findObjectByInternalName(%this.GetText(),true))){
		if ($TerrainMatDlg_AutoFixNewName){
			%matName = getUniqueInternalName( %this.getText(), TerrainMaterialSet, true );
			%this.setText(%matName);
			TerMatDlg_CreateMatButton.active = 1;
			TerMatDlg_CreateMatButton.text = "Create and select new material";
			return;
		}
		TerMatDlg_CreateMatButton.active = 0;
		TerMatDlg_CreateMatButton.text = "Material already exist";
		return;
	}
	TerMatDlg_CreateMatButton.active = 1;
	TerMatDlg_CreateMatButton.text = "Create and select new material";
	
	//	%matName = getUniqueInternalName( %this.getText(), TerrainMaterialSet, true );
	//	%this.setText(%matName);
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainMatFileEdit::onValidate( %this ) {
	//%matName = getUniqueInternalName( %this.getText(), TerrainMaterialSet, true );
	//%this.setText(%matName);
}
//------------------------------------------------------------------------------
