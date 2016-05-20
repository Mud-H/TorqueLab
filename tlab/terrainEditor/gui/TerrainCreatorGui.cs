function CreateNewTerrainGui::onWake( %this ) {
	%this-->theName.setText( "" );
	%matList = %this-->theMaterialList;
	%matList.clear();
	%count = TerrainMaterialSet.getCount();

	for ( %i=0; %i < %count; %i++ )
		%matList.add( TerrainMaterialSet.getObject( %i ).internalName, %i );

	%matList.setSelected( 0 );
	%rezList = %this-->theRezList;
	%rezList.clear();
	%rezList.add( "256", 256 );
	%rezList.add( "512", 512 );
	%rezList.add( "1024", 1024 );
	%rezList.add( "2048", 2048 );
	//%rezList.add( "4096", 4096 );
	%rezList.setSelected( 256 );
	%this-->flatRadio.setStateOn( true );
}

function CreateNewTerrainGui::create( %this ) {
	%terrainName = %this-->theName.getText();
	%resolution = %this-->theRezList.getSelected();
	%materialName = %this-->theMaterialList.getText();
	%genNoise = %this-->noiseRadio.getValue();
	devLog("NewTerrain Name",%terrainName,"Res",%resolution,"Mat",%materialName,"Noise",%genNoise);
	%obj = TerrainBlock::createNew( %terrainName, %resolution, %materialName, %genNoise );

	if( %genNoise )
		ETerrainEditor.isDirty = true;

	if( isObject( %obj ) ) {
		// Submit an undo action.
		MECreateUndoAction::submit(%obj);
		assert( isObject( EWorldEditor ),
				  "ObjectBuilderGui::processNewObject - EWorldEditor is missing!" );
		// Select it in the editor.
		EWorldEditor.clearSelection();
		EWorldEditor.selectObject(%obj);
		// When we drop the selection don't store undo
		// state for it... the creation deals with it.
		EWorldEditor.dropSelection( true );
	}

	Canvas.popDialog( %this );
}
