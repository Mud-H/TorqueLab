//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
function Lab::destroyMenu(%this,%type) {
	if ($Cfg_UI_Menu_UseNativeMenu) {
		return;
	}

	eval("%menuObj = "@%type@"EdMenu;");
	Lab.ClearMenus(%menuObj);
}
//------------------------------------------------------------------------------

//==============================================================================
function Lab::checkMenuCodeItem(%this,%type,%code,%checked) {
	%data = $LabMenuCodeIds[%type,%code];
	logc("Type",%type,"Code",%code,"checkMenuCodeItem Data",%data,"Checked",%checked);
}
//------------------------------------------------------------------------------

//==============================================================================
// Functions to manage various Menu States
//==============================================================================
//------------------------------------------------------------------------------

//==============================================================================
function Lab::setNativeMenuSystem(%this,%useNative) {
	if ($Cfg_UI_Menu_UseNativeMenu == %useNative)
		return;

	$Cfg_UI_Menu_UseNativeMenu = %useNative;
	%this.initMenubar();
}
//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
function Lab::checkMenuItem(%this,%menu,%firstPos,%lastPos,%checkPos) {
	if ($Cfg_UI_Menu_UseNativeMenu)
		eval("Lab."@%menu@"Menu.checkRadioItem( "@%firstpos@", "@%lastPos@", "@%checkPos@" );");
}
function Lab::checkFindItem(%this,%findMenu,%firstPos,%lastPos,%checkPos) {
	if ($Cfg_UI_Menu_UseNativeMenu) {
		%menu = Lab.findMenu( %findMenu );
		eval("Lab."@%menu@"Menu.checkRadioItem( "@%firstpos@", "@%lastPos@", "@%checkPos@" );");
	}
}
function Lab::setMenuDefaultState(%this,%menu) {
	if ($Cfg_UI_Menu_UseNativeMenu) {
		%menu.setupDefaultState();
	}
}

//==============================================================================
//Editor Initialization callbacks
//==============================================================================
//==============================================================================
function Lab::insertDynamicMenu(%this,%menu) {
	if ($Cfg_UI_Menu_UseNativeMenu)
		Lab.menuBar.insert( %menu, EditorGui.menuBar.dynamicItemInsertPos );
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::removeDynamicMenu(%this,%menu) {
	if ($Cfg_UI_Menu_UseNativeMenu)
		Lab.menuBar.remove( %menu );
}
//------------------------------------------------------------------------------
function Lab::attachMenus(%this) {
	if ($Cfg_UI_Menu_UseNativeMenu)
		%this.menuBar.attachToCanvas(Canvas, 0);
}


function Lab::detachMenus(%this) {
	if ($Cfg_UI_Menu_UseNativeMenu)
		%this.menuBar.removeFromCanvas();
}

function Lab::setMenuDefaultState(%this) {
	if(! isObject(%this.menuBar) || !$Cfg_UI_Menu_UseNativeMenu)
		return 0;

	for(%i = 0; %i < %this.menuBar.getCount(); %i++) {
		%menu = %this.menuBar.getObject(%i);
		%menu.setupDefaultState();
	}

	%this.worldMenu.setupDefaultState();
}

function Lab::updateUndoMenu(%this) {
	if ($Cfg_UI_Menu_UseNativeMenu) {
		%editMenu =  Lab.menuBar-->EditMenu;
		%undoName = %this.getNextUndoName();
		%redoName = %this.getNextRedoName();
		%editMenu.setItemName( 0, "Undo " @ %undoName );
		%editMenu.setItemName( 1, "Redo " @ %redoName );
		%editMenu.enableItem( 0, %undoName !$= "" );
		%editMenu.enableItem( 1, %redoName !$= "" );
	}
}

