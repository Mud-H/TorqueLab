//==============================================================================
// TorqueLab -> ShapeLab -> Node Editing
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
// This file implements all of the actions that can be applied by the GUI. Each
// action has 3 methods:
//
//    doit: called the first time the action is performed
//    undo: called to undo the action
//    redo: called to redo the action (usually the same as doit)
//
// In each case, the appropriate change is made to the shape, and the GUI updated.
//
// TSShapeConstructor keeps track of all the changes made and provides a simple
// way to save the modifications back out to a script file.
//==============================================================================

//==============================================================================
// DETAIL ACTION
//==============================================================================
//------------------------------------------------------------------------------
// Rename detail
function ShapeLab::doRenameDetail( %this, %oldName, %newName ) {
	%action = %this.createAction( ActionRenameDetail, "Rename detail" );
	%action.oldName = %oldName;
	%action.newName = %newName;
	%this.doAction( %action );
}

function ActionRenameDetail::doit( %this ) {
	if ( ShapeLab.shape.renameDetailLevel( %this.oldName, %this.newName ) ) {
		ShapeLab.onDetailRenamed( %this.oldName, %this.newName );
		return true;
	}

	return false;
}

function ActionRenameDetail::undo( %this ) {
	Parent::undo( %this );

	if ( ShapeLab.shape.renameDetailLevel( %this.newName, %this.oldName ) )
		ShapeLab.onDetailRenamed( %this.newName, %this.oldName );
}

//------------------------------------------------------------------------------
// Edit detail size
function ShapeLab::doEditDetailSize( %this, %oldSize, %newSize ) {
	devLog("ShapeLab::doEditDetailSize %oldSize, %newSize,",%oldSize, %newSize);
	%action = %this.createAction( ActionEditDetailSize, "Edit detail size" );
	%action.oldSize = %oldSize;
	%action.newSize = %newSize;
	%this.doAction( %action );
}

function ActionEditDetailSize::doit( %this ) {
	%dl = ShapeLab.shape.setDetailLevelSize( %this.oldSize, %this.newSize );

	if ( %dl != -1 ) {
		ShapeLab.onDetailSizeChanged( %this.oldSize, %this.newSize );
		return true;
	}

	return false;
}

function ActionEditDetailSize::undo( %this ) {
	Parent::undo( %this );
	%dl = ShapeLab.shape.setDetailLevelSize( %this.newSize, %this.oldSize );

	if ( %dl != -1 )
		ShapeLab.onDetailSizeChanged( %this.newSize, %this.oldSize );
}

//------------------------------------------------------------------------------
//==============================================================================
// Rename object
function ShapeLab::doRenameObject( %this, %oldName, %newName ) {
	%action = %this.createAction( ActionRenameObject, "Rename object" );
	%action.oldName = %oldName;
	%action.newName = %newName;
	%this.doAction( %action );
}

function ActionRenameObject::doit( %this ) {
	if ( ShapeLab.shape.renameObject( %this.oldName, %this.newName ) ) {
		ShapeLab.onObjectRenamed( %this.oldName, %this.newName );
		return true;
	}

	return false;
}

function ActionRenameObject::undo( %this ) {
	Parent::undo( %this );

	if ( ShapeLab.shape.renameObject( %this.newName, %this.oldName ) )
		ShapeLab.onObjectRenamed( %this.newName, %this.oldName );
}

//------------------------------------------------------------------------------
//==============================================================================
// Edit object node
function ShapeLab::doSetObjectNode( %this, %objName, %node ) {
	%action = %this.createAction( ActionSetObjectNode, "Set object node" );
	%action.objName = %objName;
	%action.oldNode = %this.shape.getObjectNode( %objName );
	%action.newNode = %node;
	%this.doAction( %action );
}

function ActionSetObjectNode::doit( %this ) {
	if ( ShapeLab.shape.setObjectNode( %this.objName, %this.newNode ) ) {
		ShapeLab.onObjectNodeChanged( %this.objName );
		return true;
	}

	return false;
}

function ActionSetObjectNode::undo( %this ) {
	Parent::undo( %this );

	if ( ShapeLab.shape.setObjectNode( %this.objName, %this.oldNode ) )
		ShapeLab.onObjectNodeChanged( %this.objName );
}

//------------------------------------------------------------------------------
//==============================================================================
// Remove Detail

function ShapeLab::doRemoveDetail( %this, %size ) {
	%action = %this.createAction( ActionRemoveDetail, "Remove detail level" );
	%action.size = %size;
	%this.doAction( %action );
}

function ActionRemoveDetail::doit( %this ) {
	%meshList = ShapeLab.getDetailMeshList( %this.size );

	if ( ShapeLab.shape.removeDetailLevel( %this.size ) ) {
		%meshCount = getFieldCount( %meshList );

		for ( %i = 0; %i < %meshCount; %i++ )
			SShapeLab.onDetailRemoved( getField( %meshList, %i ) );

		return true;
	}

	return false;
}

function ActionRemoveDetail::undo( %this ) {
	Parent::undo( %this );
}
//==============================================================================
// MESHED ACTION
//==============================================================================

//------------------------------------------------------------------------------
//==============================================================================
// Edit mesh size
function ShapeLab::doEditMeshSize( %this, %meshName, %size ) {
	%action = %this.createAction( ActionEditMeshSize, "Edit mesh size" );
	%action.meshName = stripTrailingNumber( %meshName );
	%action.oldSize = getTrailingNumber( %meshName );
	%action.newSize = %size;
	%this.doAction( %action );
}

function ActionEditMeshSize::doit( %this ) {
	if ( ShapeLab.shape.setMeshSize( %this.meshName SPC %this.oldSize, %this.newSize ) ) {
		ShapeLab.onMeshSizeChanged( %this.meshName, %this.oldSize, %this.newSize );
		return true;
	}

	return false;
}

function ActionEditMeshSize::undo( %this ) {
	Parent::undo( %this );

	if ( ShapeLab.shape.setMeshSize( %this.meshName SPC %this.newSize, %this.oldSize ) )
		ShapeLab.onMeshSizeChanged( %this.meshName, %this.oldSize, %this.oldSize );
}


//------------------------------------------------------------------------------
//==============================================================================
// Remove mesh
function ShapeLab::doRemoveMesh( %this, %meshName ) {
	%action = %this.createAction( ActionRemoveMesh, "Remove mesh" );
	%action.meshName = %meshName;
	%this.doAction( %action );
}

function ActionRemoveMesh::doit( %this ) {
	if ( ShapeLab.shape.removeMesh( %this.meshName ) ) {
		ShapeLab.onMeshRemoved( %this.meshName );
		return true;
	}

	return false;
}

function ActionRemoveMesh::undo( %this ) {
	Parent::undo( %this );
}

//------------------------------------------------------------------------------
//==============================================================================
// Add meshes from file
function ShapeLab::doAddMeshFromFile( %this, %filename, %size ) {
	%action = %this.createAction( ActionAddMeshFromFile, "Add mesh from file" );
	%action.filename = %filename;
	%action.size = %size;
	%this.doAction( %action );
}

function ActionAddMeshFromFile::doit( %this ) {
	%this.meshList = ShapeLab.addLODFromFile( ShapeLab.shape, %this.filename, %this.size, 1 );

	if ( %this.meshList !$= "" ) {
		%count = getFieldCount( %this.meshList );

		for ( %i = 0; %i < %count; %i++ )
			ShapeLab.onMeshAdded( getField( %this.meshList, %i ) );

		ShapeLab.updateMaterialList();
		return true;
	}

	return false;
}

function ActionAddMeshFromFile::undo( %this ) {
	// Remove all the meshes we added
	%count = getFieldCount( %this.meshList );

	for ( %i = 0; %i < %count; %i ++ ) {
		%name = getField( %this.meshList, %i );
		ShapeLab.shape.removeMesh( %name );
		ShapeLab.onMeshRemoved( %name );
	}

	ShapeLab.updateMaterialList();
}


//==============================================================================
// IMPOSTER And BILLBOARD ACTION
//==============================================================================
//------------------------------------------------------------------------------
// Add/edit imposter
function ShapeLab::doEditImposter( %this, %dl, %detailSize, %bbEquatorSteps, %bbPolarSteps,
												  %bbDetailLevel, %bbDimension, %bbIncludePoles, %bbPolarAngle ) {
	%action = %this.createAction( ActionEditImposter, "Edit imposter" );
	%action.oldDL = %dl;

	if ( %action.oldDL != -1 ) {
		%action.oldSize = ShapeLab.shape.getDetailLevelSize( %dl );
		%action.oldImposter = ShapeLab.shape.getImposterSettings( %dl );
	}

	%action.newSize = %detailSize;
	%action.newImposter = "1" TAB %bbEquatorSteps TAB %bbPolarSteps TAB %bbDetailLevel TAB
								 %bbDimension TAB %bbIncludePoles TAB %bbPolarAngle;
	%this.doAction( %action );
}

function ActionEditImposter::doit( %this ) {
	// Unpack new imposter settings
	for ( %i = 0; %i < 7; %i++ )
		%val[%i] = getField( %this.newImposter, %i );

	return ShapeLab.onImposterAdded(%this.newSize);
	/*
	ShapeLabWaitGui.show( "Generating imposter bitmaps..." );
	// Need to de-highlight the current material, or the imposter will have the
	// highlight effect baked in!
	ShapeLabMaterials.updateSelectedMaterial( false );
	%dl = ShapeLab.shape.addImposter( %this.newSize, %val[1], %val[2], %val[3], %val[4], %val[5], %val[6] );
	ShapeLabWaitGui.hide();
	// Restore highlight effect
	ShapeLabMaterials.updateSelectedMaterial( ShapeLabMaterials-->highlightMaterial.getValue() );
	devLog("ActionEditImposter::DoIt: dl =",%dl);
	if ( %dl != -1 ) {
		ShapeLabShapeView.refreshShape();
		ShapeLabShapeView.currentDL = %dl;
		ShapeLabAdv_Details-->detailSize.setText( %this.newSize );
		ShapeLabDetails-->meshSize.setText( %this.newSize );
		ShapeLab.updateDetail();
		return true;
	}

	return false;*/
}

function ActionEditImposter::undo( %this ) {
	Parent::undo( %this );

	// If this was a new imposter, just remove it. Otherwise restore the old settings
	if ( %this.oldDL < 0 ) {
		if ( ShapeLab.shape.removeImposter() ) {
			ShapeLabShapeView.refreshShape();
			ShapeLabShapeView.currentDL = 0;
			ShapeLab.updateDetail();
		}
	} else {
		// Unpack old imposter settings
		for ( %i = 0; %i < 7; %i++ )
			%val[%i] = getField( %this.oldImposter, %i );
			
	ShapeLab.onImposterAdded(%this.oldSize);
	return;
		ShapeLabWaitGui.show( "Generating imposter bitmaps..." );
		// Need to de-highlight the current material, or the imposter will have the
		// highlight effect baked in!
		ShapeLabMaterials.updateSelectedMaterial( false );
		%dl = ShapeLab.shape.addImposter( %this.oldSize, %val[1], %val[2], %val[3], %val[4], %val[5], %val[6] );
		ShapeLabWaitGui.hide();
		// Restore highlight effect
		ShapeLabMaterials.updateSelectedMaterial( ShapeLabMaterials-->highlightMaterial.getValue() );

		if ( %dl != -1 ) {
			ShapeLabShapeView.refreshShape();
			ShapeLabShapeView.currentDL = %dl;
			ShapeLabAdv_Details-->detailSize.setText( %this.oldSize );
			ShapeLabDetails-->meshSize.setText( %this.oldSize );
		}
	}
}

//------------------------------------------------------------------------------
// Remove imposter
function ShapeLab::doRemoveImposter( %this ) {
	%action = %this.createAction( ActionRemoveImposter, "Remove imposter" );
	%dl = ShapeLab.shape.getImposterDetailLevel();

	if ( %dl != -1 ) {
		%action.oldSize = ShapeLab.shape.getDetailLevelSize( %dl );
		%action.oldImposter = ShapeLab.shape.getImposterSettings( %dl );
		%this.doAction( %action );
	}
}

function ActionRemoveImposter::doit( %this ) {
	if ( ShapeLab.shape.removeImposter() ) {
		ShapeLab.onImposterRemoved();
		
		return true;
	}

	return false;
}

function ActionRemoveImposter::undo( %this ) {
	Parent::undo( %this );

	// Unpack the old imposter settings
	for ( %i = 0; %i < 7; %i++ )
		%val[%i] = getField( %this.oldImposter, %i );

	ShapeLab.onImposterAdded(%this.oldSize);
	return;
	/*
	ShapeLabWaitGui.show( "Generating imposter bitmaps..." );
	%dl = ShapeLab.shape.addImposter( %this.oldSize, %val[1], %val[2], %val[3], %val[4], %val[5], %val[6] );
	ShapeLabWaitGui.hide();

	if ( %dl != -1 ) {
		ShapeLabShapeView.refreshShape();
		ShapeLabShapeView.currentDL = %dl;
		ShapeLabAdv_Details-->detailSize.setText( %this.oldSize );
		ShapeLabDetails-->meshSize.setText( %this.oldSize );
		ShapeLab.updateDetail();
	}
	*/
}

//------------------------------------------------------------------------------
// Edit billboard type
function ShapeLab::doEditMeshBillboard( %this, %meshName, %type ) {
	%action = %this.createAction( ActionEditMeshBillboard, "Edit mesh billboard" );
	%action.meshName = %meshName;
	%action.oldType = %this.shape.getMeshType( %meshName );
	%action.newType = %type;
	%this.doAction( %action );
}

function ActionEditMeshBillboard::doit( %this ) {
	if ( ShapeLab.shape.setMeshType( %this.meshName, %this.newType ) ) {
		switch$ ( ShapeLab.shape.getMeshType( %this.meshName ) ) {
		case "normal":
			ShapeLabDetails-->bbType.setSelected( 0, false );

		case "billboard":
			ShapeLabDetails-->bbType.setSelected( 1, false );

		case "billboardzaxis":
			ShapeLabDetails-->bbType.setSelected( 2, false );
		}

		return true;
	}

	return false;
}

function ActionEditMeshBillboard::undo( %this ) {
	Parent::undo( %this );

	if ( ShapeLab.shape.setMeshType( %this.meshName, %this.oldType ) ) {
		%id = ShapeLab_DetailTree.getSelectedItem();

		if ( ( %id > 1 ) && ( ShapeLab_DetailTree.getItemText( %id ) $= %this.meshName ) ) {
			switch$ ( ShapeLab.shape.getMeshType( %this.meshName ) ) {
			case "normal":
				ShapeLabDetails-->bbType.setSelected( 0, false );

			case "billboard":
				ShapeLabDetails-->bbType.setSelected( 1, false );

			case "billboardzaxis":
				ShapeLabDetails-->bbType.setSelected( 2, false );
			}
		}
	}
}