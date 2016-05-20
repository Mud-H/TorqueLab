//==============================================================================
// Castle Blasters ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// RPE_DatablockEditor.buildInterface();
function buildParamListBox( %pData )
{
	%pData.pill-->field.text = %pData.Title;

	if(%pData.Variable !$= "")
		eval("%value = "@%pData.Variable@";");

	//Dropdown ctrl updare
	%list = %pData.pill-->list;

	if (!isObject(%list))
	{
		warnLog("Invalid ListBox param source for", %pData.Title);
		return;
	}

	%list.command = %pData.Command;
	%list.altCommand = %pData.AltCommand;
	%list.internalName = %pData.InternalName;
	%listId = 0;

	if (%pData.Option[%pData.Setting,"itemList"] !$= "")
	{
		%dataList = %pData.Option[%pData.Setting,"itemList"];
		eval("%items = "@%dataList@";");

		foreach$(%item in %items)
		{
			%list.insertItem(%item,%list.getItemCount());
			%listId++;
		}
	}
	else if (%pData.Option[%pData.Setting,"fieldList"] !$= "")
	{
		%dataList = %pData.Option[%pData.Setting,"fieldList"];
		eval("%items = "@%dataList@";");

		for(%i=0; %i<getFieldCount(%items); %i++)
		{
			%name = getField(%items,%i);
			%list.insertItem(%name,%list.getItemCount());
			%listId++;
		}
	}
	else if (%pData.Option[%pData.Setting,"objList"] !$= "")
	{
		%dataList = %pData.Option[%pData.Setting,"objList"];

		foreach$(%obj in %dataList)
		{
			%list.insertItem(%obj.getName(),%list.getItemCount());
			%listId++;
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
			%list.insertItem(%obj.getName(),%list.getItemCount());
			%listId++;
		}
	}

	return;
}
//------------------------------------------------------------------------------
