//==============================================================================
// TorqueLab -> Initialize editor plugins
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// LabMenu Callbacks
//==============================================================================

//==============================================================================
/// LabMenu::onMenuItemSelect Called whenever an item in a menu is selected.
/// Parameters:
/// %menuId 	Index id of the menu which contains the selected menu item
/// %menuText 	Text of the menu which contains the selected menu item
/// %menuItemId 	Index id of the selected menu item
/// %menuItemText 	Text of the selected menu item
function LabMenu::onMenuItemSelect(%this,%menuId,%menuText,%menuItemId,%menuItemText) {
	logd("LabMenu::onMenuItemSelect(%this,%menuId,%menuText,%menuItemId,%menuItemText)",%this,%menuId,%menuText,%menuItemId,%menuItemText);
	%type = %this.internalName;
	%callBack =  $LabMenuCallback[%type,%menuId,%menuItemId,%menuItemText];
	eval(%callBack);
	%isSubmenu = $LabMenuIsSubMenu[%type,%menuId,%menuItemId,%menuItemText];

	if (%isSubmenu)
		devLog("This a submenu of",%this.SubMenuText);

	%toggler = $LabMenuToggler[%type,%menuId,%menuItemId,%menuItemText];

	if (%toggler !$= "") {
		eval("%checked = "@%toggler);

		//Lab.setMenuItemBitmap(%menuId,%menuItemId,3);
		if (%isSubmenu) {
			%newMenuItemId = $LabSubMenuItemId[%type,%menuId,%menuItemId,%menuItemText];
			Lab.setSubMenuItemChecked(%this,%menuId,%newMenuItemId,%menuItemText,%checked);
			return;
		}

		Lab.setMenuItemChecked(%this,%menuId,%menuItemId,%checked);

		if (!%checked)
			Lab.setMenuItemBitmap(%this,%menuId,%menuItemId,1);
		else
			Lab.setMenuItemBitmap(%this,%menuId,%menuItemId,2);
	}
}
//------------------------------------------------------------------------------
//Lab.ActiveMenu.setSubMenuItemChecked(6,0,"DatablockEditor",true);
//==============================================================================
/// LabMenu::onMenuSelect Called whenever a menu is selected.
/// Parameters:
/// menuId 	Index id of the clicked menu
/// menuText 	Text of the clicked menu
function LabMenu::onMenuSelect(%this,%menuId,%menuText) {
	logd("LabMenu::onMenuSelect(%this,%menuId,%menuText)",%this,%menuId,%menuText);
	%this.SubMenuText = "";
}
//------------------------------------------------------------------------------
//==============================================================================
function LabMenu::onMouseInMenu(%this,%isInMenu) {
	/* Called whenever the mouse enters, or persists is in the menu.

	Parameters:
	isInMenu 	True if the mouse has entered the menu, otherwise is false.
	Note:
	To receive this callback, call setProcessTicks(true) on the menu bar.

	*/
}
//------------------------------------------------------------------------------

//==============================================================================
/// LabMenu::onSubmenuSelect Called whenever a submenu is selected.
/// Parameters:
/// submenuId 	Index id of the clicked submenu
/// submenuText 	Text of the clicked submenu
function LabMenu::onSubmenuSelect(%this,%submenuId,%submenuText) {
	logd("LabMenu::onSubmenuSelect(%this,%submenuId,%submenuText)",%this,%submenuId,%submenuText);
	%this.SubMenuText = %submenuText;
}
//------------------------------------------------------------------------------