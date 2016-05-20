//==============================================================================
// Castle Blasters ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// RPE_DatablockEditor.buildInterface();
function buildParamDropdownEdit( %pData )
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
	//GuiGroup Check
	%guiGroup = %pData.Option[%pData.Setting,"guiGroup"];

	if (%guiGroup !$= "")
	{
		%menu.guiGroup = %guiGroup;
		addCtrlToGuiGroup(%menu);
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

	%menuId = 1;

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

	%textEdit = %pData.pill-->edit;
	%textEdit.setting = %pData.Setting;
	%textEdit.command = %pData.Command;
	%textEdit.altCommand = %pData.AltCommand;
	%textEdit.internalName = %pData.InternalName@"__edit";
	return;
	//Update dropdown data
	%menu.clear();
	%selectedId = 0;
	%menuData =  %pData.Option[%pData.Setting,"menuData"];
	%defaultData =  %pData.Option[%pData.Setting,"default"];

	if (%menuData!$="")
	{
		%updType = getWord(%menuData,0);
		%updValue = getWords(%menuData,1);
		%menu.guiGroup = %updValue;

		if (%updType $="group")
		{
			%menuId = 1;

			foreach(%obj in %updValue)
			{
				%menu.add(%obj.getName(),%menuId);
				%menuId++;
			}
		}
		else if (%updType $="list")
		{
			eval("%datalist = $"@%updValue@";");
			%menuId = 1;

			foreach$(%obj in %datalist)
			{
				%menu.add(%obj.getName(),%menuId);
				%menuId++;
			}
		}
		else if (%updType $="strlist")
		{
			eval("%datalist = $"@%updValue@";");
			%menuId = 0;

			foreach$(%obj in %datalist)
			{
				%menu.add(%obj,%menuId);

				if (%obj $= %defaultData)
					%selectedId = %menuId;

				%menuId++;
			}
		}
	}

	%menu.setSelected(%selectedId,false);
}
//------------------------------------------------------------------------------
