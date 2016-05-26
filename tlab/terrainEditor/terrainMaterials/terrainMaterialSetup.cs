//==============================================================================
// TorqueLab -> Fonts Setup
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
function TerrainMatDlgActiveNameEdit::onValidate( %this ) {
	devLog("TerrainMatDlgActiveNameEdit::onValidate",%this.getText());
	TerrainMaterialDlg.setMaterialName(%this.getText());
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainMatDlgActiveFileEdit::onValidate( %this ) {
	devLog("TerrainMatDlgActiveFileEdit::onValidate",%this.getText());
	TerrainMaterialDlg.setFileName(%this.getText());
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainMaterialDlg::nameAltCommand( %this, %newName ) {
	devLog("TerrainMaterialDlg::nameAltCommand",%newName);
}
//------------------------------------------------------------------------------
//==============================================================================
function TMD_MaterialEdit::onValidate( %this ) {
	//if (!isObject(%this.activeMat)) return;
	//%this.mat.setFieldValue(%this.internalName,%this.getText());
	//EPainter.setMaterialDirty( %this.mat,%this.nameCtrl );
	TerrainMaterialDlg.dirtyMat[TerrainMaterialDlg.activeMat] = true;
	TerrainMaterialDlg-->saveMatButton.setActive(true);
	TerrainMaterialDlg.setMatDirty(TerrainMaterialDlg.activeMat);
	if ($TerrainMatDlg_LiveChanges)
		TerrainMaterialDlg.activeMat.setFieldValue(%this.internalName,%this.getText());
}
//------------------------------------------------------------------------------

//==============================================================================
function TerrainMaterialDlg::setMaterialName( %this, %newName ) {
	%mat = %this.activeMat;

	if( %mat.internalName !$= %newName ) {
		%existingMat = TerrainMaterialSet.findObjectByInternalName( %newName );

		if( isObject( %existingMat ) ) {
			LabMsgOK( "Error",
						 "There already is a terrain material called '" @ %newName @ "'.", "", "" );
		} else {
			%mat.setInternalName( %newName );
			%this-->matLibTree.buildVisibleTree( false );
		}
	}
}
//------------------------------------------------------------------------------

//==============================================================================
function TerrainMaterialDlg::setFileName( %this, %newFile ) {
	%mat = %this.activeMat;

	if( %mat.getFileName() !$= %newFile ) {
		%mat.setFileName( %newFile );
		%this-->matLibTree.buildVisibleTree( false );
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainMaterialDlg::changeMap( %this,%type ) {
	
	%ctrl = %this.findObjectByInternalName(%type@"MapCtrl",true);
	%curFile = %ctrl.bitmap;

	if( getSubStr( %curFile, 0 , 6 ) $= "tlab/" )
		%curFile = "";

	%file = getFile( $TerrainMatDlg_MapFilter,%curFile,"art/terrain/",true,true );
	
	if (!isImageFile(%file)){
		warnLog(%type,"The file selected is not a valid image:",%file);
		return;
	}	
	%file = makeRelativePath( %file, getMainDotCsDir() );
	%ctrl.setBitmap( %file );
	if (%curFile !$= %file)
		
	
	%nameCtrl = %this.findObjectByInternalName(%type@"MapFile",true);
	%nameCtrl.setText( fileBase(%file) );
	
	
}
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------

function TerrainMaterialDlg::selectMatId( %this,%matId ) {
	%item = %matLibTree.findItemByObjectId( %matId );
	%matLibTree.selectItem( %item );
}

//==============================================================================
function TerrainMaterialDlg::activateMaterialCtrls( %this, %active ) {
	%parent = %this-->matSettingsParent;
	%count = %parent.getCount();

	for ( %i = 0; %i < %count; %i++ )
		%parent.getObject( %i ).setActive( %active );
}
//------------------------------------------------------------------------------

//==============================================================================
function TerrainMaterialDlg::setActiveMaterial( %this, %mat ) {
	if (  isObject( %mat ) && %mat.isMemberOfClass( TerrainMaterial ) ) {
		if(  isObject( %mat.matSource ) &&
				%mat.matSource.isMemberOfClass( TerrainMaterial ) ) {
			warnLog("The material loaded is linked to another material:",%mat.matSource.getName());
		}

		%this.canSaveDirty = true;
		%this.activeMat = %mat;
		%this-->internalNameCtrl.setText( %mat.internalName );
		%this-->matFileCtrl.setText( %mat.getFileName() );

		foreach$(%bmpField in $TerMat_BitmapFields){
			%bmpCtrl = %this.findObjectByInternalName(%bmpField@"Ctrl",true);
			if (%mat.getFieldValue(%bmpField) $= "" || !isImageFile(%mat.getFieldValue(%bmpField)))
				%bmpCtrl.setBitmap( "tlab/materialEditor/assets/unknownImage" );
			else
				%bmpCtrl.setBitmap( %mat.getFieldValue(%bmpField) );
				
			%bmpFile = %this.findObjectByInternalName(%bmpField@"File",true);
			%bmpFile.setText(%mat.getFieldValue(%bmpField));
		
		}
		
		foreach$(%field in $TerMat_GuiFields){
			%ctrl = %this.findObjectByInternalName(%field@"Ctrl",true);
			if (%ctrl.isMemberOfClass("GuiCheckboxCtrl"))
				%ctrl.setStateOn(%mat.getFieldValue(%field));
			else
				%ctrl.setText(%mat.getFieldValue(%field));
		}
	
		%this.activateMaterialCtrls( true );
	} 
	else {
		%this.activeMat = 0;
		%this.activateMaterialCtrls( false );
	}
	%this.updateMaterialMapping(%this.activeMat);
}
//------------------------------------------------------------------------------
