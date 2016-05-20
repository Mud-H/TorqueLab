//==============================================================================
// TorqueLab -> Core Menubar handlers
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

$Pref::WorldEditor::FileSpec = "Torque Mission Files (*.mis)|*.mis|All Files (*.*)|*.*|";


//////////////////////////////////////////////////////////////////////////
// File Menu Handlers
//////////////////////////////////////////////////////////////////////////

function EditorFileMenu::onMenuSelect(%this) {
	// don't do this since it won't exist if this is a "demo"
	if(!isWebDemo())
		%this.enableItem(2, EditorIsDirty());
}

//////////////////////////////////////////////////////////////////////////

// Package that gets temporarily activated to toggle editor after mission loading.
// Deactivates itself.
package BootEditor {

	function GameConnection::initialControlSet( %this ) {
		Parent::initialControlSet( %this );
		toggleEditor( true );
		deactivatePackage( "BootEditor" );
	}

};

//////////////////////////////////////////////////////////////////////////






//////////////////////////////////////////////////////////////////////////
// View Menu Handlers
//////////////////////////////////////////////////////////////////////////

function EditorViewMenu::onMenuSelect( %this ) {
	%this.checkItem( 1, EWorldEditor.renderOrthoGrid );
}

//////////////////////////////////////////////////////////////////////////
// Edit Menu Handlers
//////////////////////////////////////////////////////////////////////////

function EditorEditMenu::onMenuSelect( %this ) {
	// UndoManager is in charge of enabling or disabling the undo/redo items.
	Editor.getUndoManager().updateUndoMenu(  );

	// SICKHEAD: It a perfect world we would abstract
	// cut/copy/paste with a generic selection object
	// which would know how to process itself.

	// Give the active editor a chance at fixing up
	// the state of the edit menu.
	// Do we really need this check here?
	if ( isObject( Lab.currentEditor ) )
		Lab.currentEditor.onEditMenuSelect( %this );
}

//////////////////////////////////////////////////////////////////////////

function EditorMenuEditDelete() {
	if ( isObject( Lab.currentEditor ) )
		Lab.currentEditor.handleDelete();
}

function EditorMenuEditDeselect() {
	if ( isObject( Lab.currentEditor ) )
		Lab.currentEditor.handleDeselect();
}

function EditorMenuEditCut() {
	if ( isObject( Lab.currentEditor ) )
		Lab.currentEditor.handleCut();
}

function EditorMenuEditCopy() {
	if ( isObject( Lab.currentEditor ) )
		Lab.currentEditor.handleCopy();
}

function EditorMenuEditPaste() {
	if ( isObject( Lab.currentEditor ) )
		Lab.currentEditor.handlePaste();
}



//////////////////////////////////////////////////////////////////////////
// Window Menu Handler
//////////////////////////////////////////////////////////////////////////

function EditorToolsMenu::onSelectItem(%this, %id) {
	%toolName = getField( %this.item[%id], 2 );
	EditorGui.setEditor(%toolName, %paletteName  );
	%this.checkRadioItem(0, %this.getItemCount(), %id);
	return true;
}

function EditorToolsMenu::setupDefaultState(%this) {
	Parent::setupDefaultState(%this);
}

//////////////////////////////////////////////////////////////////////////
// Camera Menu Handler
//////////////////////////////////////////////////////////////////////////

function EditorCameraMenu::onSelectItem(%this, %id, %text) {
	if(%id == 0 || %id == 1) {
		// Handle the Free Camera/Orbit Camera toggle
		%this.checkRadioItem(0, 1, %id);
	}

	return Parent::onSelectItem(%this, %id, %text);
}

function EditorCameraMenu::setupDefaultState(%this) {
	// Set the Free Camera/Orbit Camera check marks
	%this.checkRadioItem(0, 1, 0);
	Parent::setupDefaultState(%this);
}

function EditorFreeCameraTypeMenu::onSelectItem(%this, %id, %text) {
	// Handle the camera type radio
	%this.checkRadioItem(0, 2, %id);
	return Parent::onSelectItem(%this, %id, %text);
}

function EditorFreeCameraTypeMenu::setupDefaultState(%this) {
	// Set the camera type check marks
	%this.checkRadioItem(0, 2, 0);
	Parent::setupDefaultState(%this);
}

function EditorCameraSpeedMenu::onSelectItem(%this, %id, %text) {
	// Grab and set speed
	%speed = getField( %this.item[%id], 2 );
	$Camera::movementSpeed = %speed;
	// Update Editor
	%this.checkRadioItem(0, 6, %id);
	// Update Toolbar TextEdit
	EWorldEditorCameraSpeed.setText( $Camera::movementSpeed );
	// Update Toolbar Slider
	CameraSpeedDropdownCtrlContainer-->Slider.setValue( $Camera::movementSpeed );
	return true;
}
function EditorCameraSpeedMenu::setupDefaultState(%this) {
	// Setup camera speed gui's. Both menu and editorgui
	%this.setupGuiControls();
	//Grab and set speed
	%defaultSpeed =  Lab.levelsDirectory @ Lab.levelName @ "/cameraSpeed";

	if( %defaultSpeed $= "" ) {
		// Update Editor with default speed
		%defaultSpeed = 25;
	}

	$Camera::movementSpeed = %defaultSpeed;
	// Update Toolbar TextEdit
	EWorldEditorCameraSpeed.setText( %defaultSpeed );
	// Update Toolbar Slider
	CameraSpeedDropdownCtrlContainer-->Slider.setValue( %defaultSpeed );
	Parent::setupDefaultState(%this);
}

function EditorCameraSpeedMenu::setupGuiControls(%this) {
	// Default levelInfo params
	%minSpeed = 5;
	%maxSpeed = 200;
	%speedA =  Lab.levelsDirectory @ Lab.levelName @ "/cameraSpeedMin";
	%speedB =  Lab.levelsDirectory @ Lab.levelName @ "/cameraSpeedMax";

	if( %speedA < %speedB ) {
		if( %speedA == 0 ) {
			if( %speedB > 1 )
				%minSpeed = 1;
			else
				%minSpeed = 0.1;
		} else {
			%minSpeed = %speedA;
		}

		%maxSpeed = %speedB;
	}

	// Set up the camera speed items
	%inc = ( (%maxSpeed - %minSpeed) / (%this.getItemCount() - 1) );

	for( %i = 0; %i < %this.getItemCount(); %i++)
		%this.item[%i] = setField( %this.item[%i], 2, (%minSpeed + (%inc * %i)));

	// Set up min/max camera slider range
	eval("CameraSpeedDropdownCtrlContainer-->Slider.range = \"" @ %minSpeed @ " " @ %maxSpeed @ "\";");
}
//////////////////////////////////////////////////////////////////////////
// World Menu Handler Object Menu
//////////////////////////////////////////////////////////////////////////

function EditorWorldMenu::onMenuSelect(%this) {
	%selSize = EWorldEditor.getSelectionSize();
	%lockCount = EWorldEditor.getSelectionLockCount();
	%hideCount = EWorldEditor.getSelectionHiddenCount();
	%this.enableItem(0, %lockCount < %selSize);  // Lock Selection
	%this.enableItem(1, %lockCount > 0);  // Unlock Selection
	%this.enableItem(3, %hideCount < %selSize);  // Hide Selection
	%this.enableItem(4, %hideCount > 0);  // Show Selection
	%this.enableItem(6, %selSize > 1 && %lockCount == 0);  // Align bounds
	%this.enableItem(7, %selSize > 1 && %lockCount == 0);  // Align center
	%this.enableItem(9, %selSize > 0 && %lockCount == 0);  // Reset Transforms
	%this.enableItem(10, %selSize > 0 && %lockCount == 0);  // Reset Selected Rotation
	%this.enableItem(11, %selSize > 0 && %lockCount == 0);  // Reset Selected Scale
	%this.enableItem(12, %selSize > 0 && %lockCount == 0);  // Transform Selection
	%this.enableItem(14, %selSize > 0 && %lockCount == 0);  // Drop Selection
	%this.enableItem(17, %selSize > 0); // Make Prefab
	%this.enableItem(18, %selSize > 0); // Explode Prefab
	%this.enableItem(20, %selSize > 1); // Mount
	%this.enableItem(21, %selSize > 0); // Unmount
}

//////////////////////////////////////////////////////////////////////////

function EditorDropTypeMenu::onSelectItem(%this, %id, %text) {
	// This sets up which drop script function to use when
	// a drop type is selected in the menu.
	EWorldEditor.dropType = getField(%this.item[%id], 2);
	%this.checkRadioItem(0, (%this.getItemCount() - 1), %id);
	return true;
}

function EditorDropTypeMenu::setupDefaultState(%this) {
	// Check the radio item for the currently set drop type.
	%numItems = %this.getItemCount();
	%dropTypeIndex = 0;

	for( ; %dropTypeIndex < %numItems; %dropTypeIndex ++ )
		if( getField( %this.item[ %dropTypeIndex ], 2 ) $= EWorldEditor.dropType )
			break;

	// Default to screenCenter if we didn't match anything.
	if( %dropTypeIndex > (%numItems - 1) )
		%dropTypeIndex = 4;

	%this.checkRadioItem( 0, (%numItems - 1), %dropTypeIndex );
	Parent::setupDefaultState(%this);
}

//////////////////////////////////////////////////////////////////////////

function EditorAlignBoundsMenu::onSelectItem(%this, %id, %text) {
	// Have the editor align all selected objects by the selected bounds.
	EWorldEditor.alignByBounds(getField(%this.item[%id], 2));
	return true;
}

function EditorAlignBoundsMenu::setupDefaultState(%this) {
	// Allow the parent to set the menu's default state
	Parent::setupDefaultState(%this);
}

//////////////////////////////////////////////////////////////////////////

function EditorAlignCenterMenu::onSelectItem(%this, %id, %text) {
	// Have the editor align all selected objects by the selected axis.
	EWorldEditor.alignByAxis(getField(%this.item[%id], 2));
	return true;
}

function EditorAlignCenterMenu::setupDefaultState(%this) {
	// Allow the parent to set the menu's default state
	Parent::setupDefaultState(%this);
}
