//==============================================================================
// TorqueLab -> Initialize editor plugins
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
//GuiEditCanvas.createLabMenu();
function Lab::InitMenuBarSystem(%this) {
	if( $platform $= "macos" ) {
		$LabCmd = "Cmd";
		$LabMenuCmd = "Cmd";
		$LabQuit = "Cmd Q";
		$LabRedo = "Cmd-Shift Z";
	} else {
		$LabCmd = "Ctrl";
		$LabMenuCmd = "Alt";
		$LabQuit = "Alt F4";
		$LabRedo = "Ctrl Y";
	}
}
//------------------------------------------------------------------------------
//==============================================================================
//GuiEditCanvas.createLabMenu();
function Lab::ExecMenuBarSystem(%this) {
	execpattern("tlab/core/menubar/defineMenu/*.cs");
	exec("tlab/core/menubar/menuBuilder.cs");
	exec("tlab/core/menubar/menuCallbacks.cs");
	exec("tlab/core/menubar/menuFunctions.cs");
	exec("tlab/core/menubar/menuHandlers.cs");
	exec("tlab/core/menubar/manageMenu.cs");
}
//------------------------------------------------------------------------------
//==============================================================================
//GuiEditCanvas.createLabMenu();
function Lab::BuildMenus(%this,%execFirst) {
	if (%execFirst)
		%this.ExecMenuBarSystem();

	Lab.BuildGuiMenu();
	Lab.BuildWorldMenu();
	
}
//------------------------------------------------------------------------------
//==============================================================================
//GuiEditCanvas.createLabMenu();
function Lab::BuildGuiMenu(%this,%execFirst) {
	if (%execFirst)
		%this.ExecMenuBarSystem();

	if (isObject(GuiEdMap)){
		$LabMenuBindMap["Gui"] = GuiEdMap;
		$LabMenuContainer["Gui"] = GuiEditorGui-->AreaMenuBar;
		Lab.initMenu("Gui");
	}		
}
//------------------------------------------------------------------------------
//==============================================================================
//GuiEditCanvas.createLabMenu();
function Lab::BuildWorldMenu(%this,%execFirst) {
	if (%execFirst)
		%this.ExecMenuBarSystem();

	if (isObject(EditorMap)){
		$LabMenuBindMap["World"] = EditorMap;
		$LabMenuContainer["World"] = EditorGui-->AreaMenuBar;
		Lab.initMenu("World");
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::initMenu(%this,%menuType,%execScripts) {
	if (%execScripts)
		%this.ExecMenuBarSystem();
		
   

   $EditorFullscreenAllowed = !$Cfg_UI_Menu_UseNativeMenu;
	//===========================================================================
	// Custom Menu System using GuiMenuBar Control
	if (!$Cfg_UI_Menu_UseNativeMenu) {
		
		eval("%menuObj = "@%menuType@"EdMenu;");
		
		//$LabMenuContainer[%menuType].add(%menuObj);
		%menuObj.initData();
		Lab.BuildMenuBar(%menuObj);

		//Check for natives menu and delete them
		if (isObject(Lab.menuBar))
			Lab.menuBar.removeFromCanvas();

		if (isObject(GuiEditCanvas.menuBar))
			GuiEditCanvas.menuBar.removeFromCanvas();

		return;
	}

	//===========================================================================
	// Native Menu System using coded MenuBar Object
	Lab.clearMenus();

	if(!isObject(%this.menuBar))
		%this.buildMenus();

	%this.attachMenus();
}
//==============================================================================
//GuiEditCanvas.createLabMenu();
function Lab::BuildMenuBar(%this,%menuObj) {
	Lab.clearMenus(%menuObj);
	%type = %menuObj.internalName;
	%menuId = 0;
	$LabMenuList[%type] = "";

	while($LabMenu[%type,%menuId] !$= "") {
		Lab.addMenu(%menuObj,$LabMenu[%type,%menuId],%menuId);
		%menuItemId = 0;

		while($LabMenuItem[%type,%menuId,%menuItemId] !$= "") {
			%itemData = $LabMenuItem[%type,%menuId,%menuItemId];
			Lab.addMenuItemData(%menuObj,%menuId,%menuItemId,%itemData);
			//GuiEditCanvas.addMenuItem( %menuId, getField(%item,0),%menuItemId,getField(%item,1),%checkId,%item);
			%subMenuItemId = 0;

			while($LabSubMenuItem[%type,%menuId,%menuItemId,%subMenuItemId] !$= "") {
				%subItemData = $LabSubMenuItem[%type,%menuId,%menuItemId,%subMenuItemId] ;
				Lab.addSubMenuItemData( %menuObj,%menuId,%menuItemId,%subMenuItemId,%subItemData);
				%subMenuItemId++;
			}

			%menuItemId++;
		}

		%menuId++;
	}

	Lab.setCheckmarkBitmapIndex(%menuObj,2);
	Lab.setMenuMargins(%menuObj,"4","0","0");
}
//------------------------------------------------------------------------------

//==============================================================================
function Lab::addMenuItemData(%this,%menuObj,%menuId,%menuItemId,%data) {
	%type = %menuObj.internalName;
	%itemText = getField(%data,0);
	%itemCode = %itemText;

	if (strFind(%itemText,">>")) {
		%fields = strreplace(%itemText,">>","/t");
		%itemText = getField(%fields,0);
		%itemCode = getField(%fields,1);
	}

	%accelerator = getField(%data,1);
	%callback = getField(%data,2);
	%toggler = getField(%data,3);
	%checkGroup = getField(%data,4);

	if (%checkGroup $= "")
		%checkGroup = "-1";

	$LabMenuCallback[%type,%menuId,%menuItemId,%itemText] = %callback;
	$LabMenuIsSubMenu[%type,%menuId,%menuItemId,%itemText] = false;
	$LabMenuCodeIds[%type,%itemCode] = %itemText TAB %menuId TAB %menuItemId;

	if (%accelerator !$="") {
		%this.addMenuBind(%type,%accelerator,%callBack);
	}

	Lab.addMenuItem(%menuObj,%menuId,%itemText,%menuItemId,%accelerator,%checkGroup);

	if ( %toggler !$= "") {
		$LabMenuToggler[%type,%menuId,%menuItemId,%itemText] = %toggler;
		eval("%checked = "@%toggler);
		Lab.setMenuItemChecked(%menuObj,%menuId,%menuItemId,%checked);
	}
}
//------------------------------------------------------------------------------

//==============================================================================
function Lab::addSubMenuItemData(%this,%menuObj,%menuId,%menuItemId,%submenuItemId,%data) {
	%type = %menuObj.internalName;
	%subItemText = getField(%data,0);
	%itemCode = %subItemText;

	if (strFind(%subItemText,">>")) {
		%fields = strreplace(%subItemText,">>","/t");
		%subItemText = getField(%fields,0);
		%itemCode = getField(%fields,1);
	}

	%accelerator = getField(%data,1);
	%callback = getField(%data,2);
	%toggler = getField(%data,3);
	%checkGroup = getField(%data,4);

	if (%checkGroup $= "")
		%checkGroup = "-1";

	$LabMenuCallback[%type,%menuId,%submenuItemId,%subItemText] = %callback;
	$LabMenuIsSubMenu[%type,%menuId,%submenuItemId,%subItemText] = true;
	$LabSubMenuItemId[%type,%menuId,%submenuItemId,%subItemText] = %menuItemId;
	$LabMenuCodeIds[%type,%itemCode] = %subItemText TAB %menuId TAB %menuItemId TAB %submenuItemId;

	if (%accelerator !$="") {
		%this.addMenuBind(%type,%accelerator,%callBack);
	}

	Lab.addSubmenuItem(%menuObj,%menuId,%menuItemId,%subItemText,%submenuItemId,%accelerator,%checkGroup);
	Lab.setMenuItemBitmap(%menuObj,%menuId,%menuItemId,3);

	if ( %toggler !$= "") {
		$LabMenuToggler[%type,%menuId,%submenuItemId,%subItemText] = %toggler;
		eval("%checked = "@%toggler);
		Lab.setSubmenuItemChecked(%menuObj,%menuId,%menuItemId,%subItemText,%checked);
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::addMenuBind(%this,%type,%bind,%command) {
	%map = $LabMenuBindMap[%type];

	if (!isObject(%map))
		return;

	%map.bindCmd(keyboard, %bind, %command, "");
}
//------------------------------------------------------------------------------
