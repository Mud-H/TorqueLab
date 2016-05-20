//==============================================================================
// TorqueLab -> Initialize editor plugins
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// Menu Functions Duplicating MenuBar Functions for additional checks if needed
//==============================================================================

///==============================================================================
/// menuText 	Text to display for the new menu item.
/// menuId 	ID for the new menu item.
function Lab::addMenu(%this,%menuObj,%menuText,%menuId) {
	%type = %menuObj.internalName;
	$LabMenuList[%type] = strAddField($LabMenuList[%type],%menuId SPC %menuText,true);
	%menuObj.addMenu(%menuText,%menuId);
}
//------------------------------------------------------------------------------
//==============================================================================
/// %menuId 	Menu name or menu Id to add the new item to.
/// %menuItemText 	Text for the new menu item.
/// %menuItemId 	Id for the new menu item.
/// %accelerator 	Accelerator key for the new menu item.
/// %checkGroup 	Check group to include this menu item in.
function Lab::addMenuItem(%this,%menuObj,%menuId,%menuItemText,%menuItemId,%accelerator,%checkGroup) {
	%type = %menuObj.internalName;
	logd("Lab::addMenuItem(%this,%menuObj,%menuId,%menuItemText,%menuItemId,%accelerator,%checkGroup)",%menuObj,%menuId,%menuItemText,%menuItemId,%accelerator,%checkGroup);
	$LabMenuItemList[%type,%menuId] = strAddField($LabMenuItemList[%type,%menuId],%menuItemId SPC %menuItemText,true);
	%menuObj.addMenuItem(%menuId,%menuItemText,%menuItemId,%accelerator,%checkGroup);
}
//------------------------------------------------------------------------------

//==============================================================================
/// %menuObj MenuBar object
/// %menuId 	Menu to affect a submenu in
/// %menuItemId 	Menu item to affect
/// %submenuItemText 	Text to show for the new submenu
/// %submenuItemId 	Id for the new submenu
/// %accelerator 	Accelerator key for the new submenu
/// %checkGroup 	Which check group the new submenu should be in, or -1 for none.
function Lab::addSubmenuItem(%this,%menuObj,%menuId,%menuItemId,%submenuItemText,%submenuItemId,%accelerator,%checkGroup) {
	%type = %menuObj.internalName;
	$LabMenuSubItemList[%type,%menuId,%menuItemId] = strAddField($LabMenuSubItemList[%type,%menuId,%menuItemId],%submenuItemId SPC %submenuItemText,true);
	%menuObj.addSubmenuItem(%menuId,%menuItemId,%submenuItemText,%submenuItemId,%accelerator,%checkGroup);
}
//------------------------------------------------------------------------------

//==============================================================================
function Lab::ClearMenus(%this,%menuObj) {
	%menuObj.clearMenus("","");
}
//------------------------------------------------------------------------------

//==============================================================================
function Lab::ClearMenusItems(%this,%menuObj,%menu) {
	%menuObj.ClearMenusItems(%menu);
}
//------------------------------------------------------------------------------
//==============================================================================
/// Removes all the menu items from the specified submenu.
function Lab::clearSubmenuItems(%this,%menuObj,%menuId,%menuItemId) {
	%menuObj.clearSubmenuItems(%menuId,%menuItemId);
}
//------------------------------------------------------------------------------


//==============================================================================
function Lab::removeMenu(%this,%menuObj,%menu) {
	%menuObj.removeMenu(%menu);
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::removeMenuItem(%this,%menuObj,%menu,%item) {
	%menuObj.removeMenuItem(%menu,%item);
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::setCheckmarkBitmapIndex(%this,%menuObj,%index) {
	%menuObj.setCheckmarkBitmapIndex(%index);
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::setMenuBitmapIndex(%this,%menuObj,%menuTarget,%bitmapindex,%bitmaponly,%drawborder) {
	//--------------------------------------------
	// Sets the bitmap index for the menu and toggles rendering only the bitmap.
	// %menuTarget 	Menu to affect
	// %bitmapindex 	Bitmap index to set for the menu
	// %bitmaponly 	If true, only the bitmap will be rendered
	// %drawborder 	If true, a border will be drawn around the menu.*/
	%menuObj.setMenuBitmapIndex(%menuTarget,%bitmapindex,%bitmaponly,%drawborder);
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::setMenuItemBitmap(%this,%menuObj,%menuTarget,%menuItem,%bitmapIndex) {
	//--------------------------------------------
	// Sets the specified menu item bitmap index in the bitmap array. Setting the item's index to -1 will remove any bitmap.
	// %menuTarget 	Menu to affect the menuItem in
	// %menuItem 	Menu item to affect
	// %bitmapIndex 	Bitmap index to set the menu item to
	%menuObj.setMenuItemBitmap(%menuTarget,%menuItem,%bitmapIndex);
}
//------------------------------------------------------------------------------

//==============================================================================
function Lab::setMenuItemChecked(%this,%menuObj,%menuTarget,%menuItem,%checked) {
	//--------------------------------------------
	// Sets the menu item bitmap to a check mark, which by default is the first element in the bitmap array (although this may be changed with setCheckmarkBitmapIndex()). Any other menu items in the menu with the same check group become unchecked if they are checked.
	// %menuTarget 	Menu to work in
	// %menuItem 	Menu item to affect
	// %checked 	Whether we are setting it to checked or not
	%menuObj.setMenuItemChecked(%menuTarget,%menuItem,%checked);
}
//------------------------------------------------------------------------------

//==============================================================================
function Lab::setMenuItemEnable(%this,%menuObj,%menuTarget,%menuItemTarget,%enabled) {
	//--------------------------------------------
	// sets the menu item to enabled or disabled based on the enable parameter. The specified menu and menu item can either be text or ids.
	// %menuTarget 	Menu to work in
	// %menuItemTarget 	The menu item inside of the menu to enable or disable
	// %enabled 	Boolean enable / disable value.
	%menuObj.setMenuItemEnable(%menuTarget,%menuItemTarget,%enabled);
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::setMenuItemSubmenuState(%this,%menuObj,%menuTarget,%menuItem,%isSubmenu) {
	//--------------------------------------------
	// Sets the given menu item to be a submenu.
	// %menuTarget 	Menu to affect a submenu in
	// %menuItem 	Menu item to affect
	// %isSubmenu 	Whether or not the menuItem will become a subMenu or not
	// %menuObj.setMenuItemSubmenuState(0,2,true);
	%menuObj.setMenuItemSubmenuState(%menuTarget,%menuItem,%isSubmenu);
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::setMenuItemText(%this,%menuObj,%menuTarget,%menuItem,%newMenuItemText) {
	//--------------------------------------------
	// Sets the text of the specified menu item to the new string.
	// %menuTarget 	Menu to affect
	// %menuItem 	Menu item in the menu to change the text at
	// %newMenuItemText 	New menu text
	%menuObj.setMenuItemText(%menuTarget,%menuItem,%newMenuItemText);
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::setMenuItemVisible(%this,%menuTarget,%menuObj,%menuItem,%isVisible) {
	//--------------------------------------------
	// menuTarget 	Menu to affect the menu item in
	// menuItem 	Menu item to affect
	// %isVisible 	Visible state to set the menu item to.
	%menuObj.setMenuItemVisible(%menuTarget,%menuItem,%isVisible);
}
//------------------------------------------------------------------------------


//==============================================================================
function Lab::setMenuMargins(%this,%menuObj,%horizontalMargin,%verticalMargin,%bitmapToTextSpacing) {
	//--------------------------------------------
	// Sets the menu rendering margins: horizontal, vertical, bitmap spacing.
	// %horizontalMargin 	Number of pixels on the left and right side of a menu's text.
	// %verticalMargin 	Number of pixels on the top and bottom of a menu's text.
	// %bitmapToTextSpacing 	Number of pixels between a menu's bitmap and text.
	%menuObj.setMenuMargins(%horizontalMargin,%verticalMargin,%bitmapToTextSpacing);
}
//------------------------------------------------------------------------------

//==============================================================================
function Lab::setMenuText(%this,%menuObj,%menuTarget,%newMenuText) {
	//--------------------------------------------
	// Sets the text of the specified menu to the new string.
	// menuTarget 	Menu to affect
	// %newMenuText 	New menu text
	%menuObj.setMenuText(%menuTarget,%newMenuText);
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::setMenuVisible(%this,%menuObj,%menuTarget,%visible) {
	//--------------------------------------------
	// Sets the whether or not to display the specified menu.
	// menuTarget 	Menu item to affect
	// %visible 	Whether the menu item will be visible or not
	%menuObj.setMenuVisible(%menuTarget,%visible);
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::setSubmenuItemChecked(%this,%menuObj,%menuTarget,%menuItem,%submenuItemText,%checked) {
	//--------------------------------------------
	// Sets the menu item bitmap to a check mark, which by default is the first element in the bitmap array (although this may be changed with setCheckmarkBitmapIndex()). Any other menu items in the menu with the same check group become unchecked if they are checked.
	// %menuTarget 	Menu to affect a submenu in
	// %menuItem 	Menu item to affect
	// %submenuItemText 	Text to show for submenu
	// %checked 	Whether or not this submenu item will be checked.
	%menuObj.setSubmenuItemChecked(%menuTarget,%menuItem,%submenuItemText,%checked);
}
//------------------------------------------------------------------------------



