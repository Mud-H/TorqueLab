//==============================================================================
// Boost! -> Helper functions for common settings GUI needs
// Copyright NordikLab Studio, 2013
//==============================================================================

//==============================================================================
function autoUpdateParam( %field,%value,%paramObj,%callback )
{
	//devLog("autoUpdateParam( %field,%value,%paramObj,%callback ) ", %field,%value,%paramObj,%callback ) ;
	if (%callback !$="")
		eval(%callback);

	%syncObjs = %paramObj.syncObjs[%field];

	//If no object to sync, there might be a prefGroup to use
	if (%syncObjs $= "" && %paramObj.prefGroup !$= "" )
	{
		eval(%paramObj.prefGroup@%field@" = %value;");
	}

	for( ; %i< getFieldCount(%syncObjs); %i++)
	{
		%data = getField(%syncObjs ,%i);

		//devLog("SyncObjs for field:", %field,"Is:", %data);
		if (isObject(%data))
		{
			%data.setFieldValue(%field,%value);
		}
		else if (strstr(%data,");") !$= "-1")
		{
			//Seem to be a function so replace ** with the value
			%function = strreplace(%data,"**",%value);
			//devLog("SyncObjs is a function:", %function);
			eval(%function);
		}
		else if (getSubStr(%data,0,1) $= "$")
		{
			//devLog("updateSettingsParams Global:",%data,"Field",%field,"Value",%value);
			//Seem to be a function so replace ** with the value
			%data = strreplace(%data,"\"","");
			eval(%data@" = %value;");
		}
		// Check for special object field name EX: EWorldEditor.stickToGround
		else if (strstr(%data,".") !$= "-1")
		{
			//devLog("updateSettingsParams SpecialObjField:",%data,"Field",%field,"Value",%value);
			//Seem to be a function so replace ** with the value
			eval(%data@" = %value;");
		}
	}
}

//==============================================================================
function syncParamObj(%paramObj)
{
	foreach$(%field in %paramObj.fieldList)
	{
		if (%paramObj.skipField[%field])
			continue;

		%value = "";
		%syncObjs = %paramObj.syncObjs[%field];
		//If no object to sync, there might be a prefGroup to use
		%firstObj = getWord(%syncObjs,0);

		if (%syncObjs $= "" && %paramObj.prefGroup !$= "" )
		{
			eval("%value = "@%paramObj.prefGroup@%field@";");
		}
		else if (isObject(%firstObj))
		{
			%value = %firstObj.getFieldValue(%field);
		}
		else if (getSubStr(%firstObj,0,1) $= "$")
		{
			//devLog("updateSettingsParams Global:",%data,"Field",%field,"Value",%value);
			//Seem to be a function so replace ** with the value
			%fix = strreplace(%firstObj,"\"","");
			eval("%value = "@%fix@";");
		}
		// Check for special object field name EX: EWorldEditor.stickToGround
		else if (strstr(%firstObj,".") !$= "-1")
		{
			//devLog("updateSettingsParams SpecialObjField:",%data,"Field",%field,"Value",%value);
			//Seem to be a function so replace ** with the value
			eval("%value = "@%firstObj@";");
		}

		//devLog("Value for field:",%field,"Is:",%value);

		if (%value $= "") continue;

		%ctrl = %paramObj.baseGuiControl.findObjectByInternalName(%field,true);
		%ctrl.setTypeValue(%value);
	}
}
//------------------------------------------------------------------------------

