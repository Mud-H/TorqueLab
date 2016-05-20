//==============================================================================
// Castle Blasters ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// RPE_DatablockEditor.buildInterface();
function buildParamDropdown( %pData )
{
	%pData.pill-->field.text = %pData.Title;

	if(%pData.Variable !$= "")
		eval("%value = "@%pData.Variable@";");

	//Dropdown ctrl updare
	%menu = %pData.pill-->menu;

	if (!isObject(%menu))
	{
		//Check for old setup using dropdown
		%menu = %pData.pill-->dropdown;

		if (!isObject(%menu))
			return;
	}

	%menu.command = %pData.Command;
	%menu.altCommand = %pData.AltCommand;
	%menu.internalName = %pData.InternalName;
	%menu.variable = %pData.Variable;
	%menu.canSaveDynamicFields = true;

	if (%pData.Option[%pData.Setting,"syncId"] !$= "")
	{
		%menu.syncId = true;
	}

	if (%pData.myNameIs!$= "")
	{
		%name = %pData.myNameIs;

		if (isObject(%name))
		{
			%name = getUniqueName(%pData.myNameIs);
			warnLog("Can't name the new param control to:",%pData.myNameIs,"The unique name is set to:",%name);
		}

		%menu.setName(%name);
	}

	%menuId = 0;

	if (%pData.Option[%pData.Setting,"itemList"] !$= "")
	{
		%list = %pData.Option[%pData.Setting,"itemList"];
		eval("%items = "@%list@";");

		foreach$(%item in %items)
		{
			%menu.add(%item,%menuId);
			%menuId++;
		}
	}
	else if (%pData.Option[%pData.Setting,"fieldList"] !$= "")
	{
		%list = %pData.Option[%pData.Setting,"fieldList"];
		eval("%items = "@%list@";");

		for(%i=0; %i<getFieldCount(%items); %i++)
		{
			%name = getField(%items,%i);
			%menu.add(%name,%menuId);
			%menuId++;
		}
	}
	else if (%pData.Option[%pData.Setting,"objList"] !$= "")
	{
		%list = %pData.Option[%pData.Setting,"objList"];
		eval("%items = "@%list@";");

		foreach$(%obj in %list)
		{
			%menu.add(%obj.getName(),%menuId);
			%menuId++;
		}
	}
	else if (%pData.Option[%pData.Setting,"itemGroup"] !$= "")
	{
		%itemGroup = %pData.Option[%pData.Setting,"itemGroup"];

		if (!isObject(%itemGroup))
		{
			warnLog("Invalid menu items group supplied:",%itemGroup,"No items have been added!");
			return;
		}

		foreach(%obj in %itemGroup)
		{
			%menu.add(%obj.getName(),%menuId);
			%menuId++;
		}
	}

	return;
}
//------------------------------------------------------------------------------
