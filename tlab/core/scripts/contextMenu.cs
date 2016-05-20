//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$LabCfg_ContextMenuProfile = "ToolsDropdownProfile";

//==============================================================================
// MouseEventControl - GuiControl AutoToggler
//==============================================================================


//==============================================================================
// Context Menu Helpers
//==============================================================================
function Lab::createContextMenu(%this,%itemRecords,%show) {
	logc("Lab::createContextMenu(%this,%itemRecords,%show,%menu)",%this,%itemRecords,%show,%menu);

	if (!isObject(%menu)) {
		%menu = new PopupMenu() {
			superClass = "ContextMenu";
			isPopup = true;
			object = -1;
			profile = $LabCfg_ContextMenuProfile;
		};
	}

	return %menu;
}
//------------------------------------------------------------------------------
// Adds one item to the menu.
// if %item is skipped or "", we will use %item[#], which was set when the menu was created.
// if %item is provided, then we update %item[#].
function ContextMenu::addItem(%this, %pos, %item) {
	if(%item $= "")
		%item = %this.item[%pos];

	if(%item !$= %this.item[%pos])
		%this.item[%pos] = %item;

	%name = getField(%item, 0);
	%accel = getField(%item, 1);
	%cmd = getField(%item, 2);
	// We replace the [this] token with our object ID
	%cmd = strreplace( %cmd, "[this]", %this );
	%this.item[%pos] = setField( %item, 2, %cmd );

	if(isObject(%accel)) {
		// If %accel is an object, we want to add a sub menu
		%this.insertSubmenu(%pos, %name, %accel);
	} else {
		%this.insertItem(%pos, %name !$= "-" ? %name : "", %accel);
	}
}

function ContextMenu::appendItem(%this, %item) {
	%this.addItem(%this.getItemCount(), %item);
}

function ContextMenu::onSelectItem(%this, %id, %text) {
	%cmd = getField(%this.item[%id], 2);

	if(%cmd !$= "") {
		eval( %cmd );
		return true;
	}

	return false;
}

//- Sets a new name on an existing menu item.
function ContextMenu::setItemName( %this, %id, %name ) {
	%item = %this.item[%id];
	%accel = getField(%item, 1);
	%this.setItem( %id, %name, %accel );
}

//- Sets a new command on an existing menu item.
function ContextMenu::setItemCommand( %this, %id, %command ) {
	%this.item[%id] = setField( %this.item[%id], 2, %command );
}
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
