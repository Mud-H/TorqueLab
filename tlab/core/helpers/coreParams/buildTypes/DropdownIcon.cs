//==============================================================================
// Castle Blasters ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// RPE_DatablockEditor.buildInterface();
function buildParamDropdownIcon(%pData)
{
	%pData.pill-->field.text = %pData.Title;
	//Dropdown ctrl updare
	%menu = %pData.pill-->dropdown;
	%menu.command = %pData.Command;
	%menu.altCommand = %pData.AltCommand;
	%menu.internalName = %pData.InternalName;
	%menu.variable = %pData.Variable;

	if ( %pData.Option[%pData.Setting,"guiGroup"] !$="")
	{
		%menu.guiGroup =  %pData.Option[%pData.Setting,"guiGroup"];
		UI.setCtrlGuiGroup(%menu);
	}

	//Update dropdown data
	%menu.clear();
	%menuData =  %pData.Option[%pData.Setting,"menuData"];

	if (%menuData!$="")
	{
		%updType = getWord(%menuData,0);
		%updValue = getWords(%menuData,1);

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
				%menu.add(%obj,%menuId);
				%menuId++;
			}
		}
	}

	//Dropdown ctrl updare
	%icon = %pData.pill-->icon;
	%icon.command =  %pData.Option[%pData.Setting,"iconCommand"];
	%icon.iconBitmap =   %pData.Option[%pData.Setting,"icon"];
}
//------------------------------------------------------------------------------
