//==============================================================================
// HelpersLab -> Params System - Update the params gui controls
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//===========================================================================
$HLabParams_FriendList = "slider";
//==============================================================================
/// Default params controls syncing functions. This function is called whenever
/// a params Gui Control is changed. It will do some automated tasks and then it
/// will call the specific update function is setted.
/// %ctrl:	The GuiControl that have been changed
/// %updateFunc: Callback function to be called (Only the part before (...); is
///				needed. The args will be set as follow: (%field,%value,%ctrl,%array,%arg1,%arg2)
/// %array: The Param ArrayObject which hold all the data for the params set
/// %arg1: Optional argument #1 which can be add to the updateFunc
/// %arg2: Optional argument #2 which can be add to the updateFunc
function syncParamArrayCtrl( %ctrl, %updateFunc,%array,%isAltCommand,%arg1,%arg2)
{
	//===========================================================================
	// Special script for GuiColorPickerCtrl Params
	if (%ctrl.getClassName() $= "GuiColorPickerCtrl")
	{
		%ctrl.updateCommand = %updateFunc@"(%ctrl.internalName,%color,%ctrl,\""@%array@"\",\""@%isAltCommand@"\",\""@%arg1@"\",\""@%arg2@"\");";
		%currentColor =   %ctrl.baseColor;

		if (%ctrl $= ColorBlendSelect)
		{
			return;
		}

		if (%ctrl.noAlpha)
			%currentColor.a = "1";

		if (%ctrl.isIntColor )
		{
			%currentColor =  ColorFloatToInt(%currentColor);
			%callBack = %ctrl@".ColorPickedI";
			%updateCallback = %ctrl@".ColorUpdatedI";
			GetColorI( %currentColor, %callback, %ctrl.getRoot(), %updateCallback, %cancelCallback );
		}
		else
		{
			%callBack = %ctrl@".ColorPicked";
			%updateCallback = %ctrl@".ColorUpdated";
			GetColorF( %currentColor, %callback, %ctrl.getRoot(), %updateCallback, %cancelCallback );
		}

		return;
	}

	//---------------------------------------------------------------------------
	syncParamArrayCtrlData(%ctrl, %updateFunc,%array,%isAltCommand,%arg1,%arg2);
	return;
}
function syncParamArrayCtrlData( %ctrl, %updateFunc,%array,%isAltCommand,%arg1,%arg2)
{
	//Get the field from the internalName which is always the part before _ if exist
	//===========================================================================
	%field = getWord(strreplace(%ctrl.internalName,"__"," "),0);

	if (strFind(%ctrl.internalName,"__"))
	{
		%special = getWord(strreplace(%ctrl.internalName,"__"," "),1);

		if (!strFind($HLabParams_FriendList,%special) && %array.validateSubField)
		{

			if (%ctrl.setting !$= "")
				%field = %ctrl.setting;
			else
				%field = %ctrl.internalName;
		}
	}

	//If Special Edit set, use the getText instead of global getTypeValue
	if (%special $= "Edit")
	{
		%value = %ctrl.getText();
	}
	else
		%value = %ctrl.getTypeValue();

	//===========================================================================
	// PrefGroup autosyncing will add the field to the prefGroup and store the value inside
	//Ex: PrefGroup: $PrefGroup:: Field: myField  Value: myValue will store myValue in $PrefGroup::myField
	if (%array.autoSyncPref $= "1"||%array.autoSyncPref)
	{
		%prefGroup = %array.prefGroup;
		eval(%prefGroup@%field@" = \""@%value@"\";");
	}

	//---------------------------------------------------------------------------

	// Call the specific update function is set
	if (%updateFunc !$= "" && %array.customUpdateOnly)
	{
	   
		eval(%updateFunc@"(%field,%value,%ctrl,%array,%isAltCommand,%arg1,%arg2);");
		//return;
	}

//===========================================================================
//Check for data that need to be synced with new value
	setParamFieldValue(%array,%field,%value);

//---------------------------------------------------------------------------

	//Check if a validation type is specified
	if (%ctrl.validationType !$="")
	{
		%valType = getWord(%ctrl.validationType,0);
		%valValue = getWord(%ctrl.validationType,1);

		if (%valType $= "flen")
			%value = mFloatLength(%value,%valValue);

		%ctrl.setTypeValue(%value);
	}

	//===========================================================================
	// PrefGroup autosyncing will add the field to the prefGroup and store the value inside
	//Ex: PrefGroup: $PrefGroup:: Field: myField  Value: myValue will store myValue in $PrefGroup::myField
	if (%ctrl.linkSet !$= "")
	{
		%link = %array.container.findObjectByInternalName(%ctrl.linkSet,true);

		if (isObject(%link))
		{
			%link.setTypeValue(%value);
			%link.updateFriends();
		}
		else
		{
			warnLog("Can't find the linkSet inside the param array:",	%ctrl.linkSet);
		}
	}

	//---------------------------------------------------------------------------

	//===========================================================================
	// Call the specific update function is set
	if (%updateFunc !$= "")
		eval(%updateFunc@"(%field,%value,%ctrl,%array,%arg1,%arg2);");

	//===========================================================================
	// Unless noFriends is specified, try to update agregated friends
	if (!%ctrl.noFriends)
		%ctrl.updateFriends();

	//Param generic sync data function
	if (%array.generalSyncFunc !$="")
	{
		eval(%array.generalSyncFunc);
	}
}

//==============================================================================
// Generic updateRenderer method
function setParamFieldValue( %array, %field,%value)
{
	%syncData = %array.syncData[%field];
	%updCommand = %array.updCmd[%field];
	%fieldData = %array.getVal(%field);
	
	

	%dataType =  getField(%fieldData,2);
	if (strFind(%dataType,"Color")){
	   if (getWordCount(%value) < 3){
	      return;
	   }
	}
	

	//if no syncData, try with the old system
	if (%syncData $= "")
	{
		%data = %array.getVal(%field);
		%syncData = getField(%data,4);

		if (%syncData $= "")
			paramLog(%array.getName(),"Param have no SyncData ! Field",%field,"SyncData",%syncData);
		else
			paramLog(%array.getName(),"Use old syncData system! Field",%field,"SyncData",%syncData);
	}

	if (%updCommand !$="")
	{
		if (strFind(%updCommand,"*val*"))
		{
			%command = strreplace(%updCommand,"*val*","\""@%value@"\"");			
			eval(%command);
		}
		//Replace ** occurance with value (Old way)
		else if (strFind(%updCommand,"**"))
		{
			%command = strreplace(%updCommand,"**","\""@%value@"\"");
			eval(%command);
		}
	}

	if (%array.noDirectSync || %syncData $= "")
		return false;

	//Check for a standard global starting with $

	if (isObject(%syncData))
	{
		%syncData.setFieldValue(%field,%value);
		return;
	}
	else if (getSubStr(%syncData,0,1) $= "$")
	{
		%lastChar = getSubStr(%syncData,strlen(%syncData)-1,1);

		if (%lastChar $= ":" || %lastChar $= "_")
		{
			//Incomplete global, need to add the field
			eval(%syncData@%field@" = %value;");
			return;
		}

		eval(%syncData @" = %value;");
		return;
	}
	else if (strFind(%syncData,"::"))
	{
		eval(%paramArray.prefGroup@%syncData@" = %value;");
		return;
	}
	//Check for a function template by searching for );
	else if (strFind(%syncData,");"))
	{
		//Replace *val* occurance with value
		if (strFind(%syncData,"*val*"))
		{
			%command = strreplace(%syncData,"*val*","\""@%value@"\"");
			eval(%command);
			return;
		}
		//Replace ** occurance with value (Old way)
		else if (strFind(%syncData,"**"))
		{
			%command = strreplace(%syncData,"**","\""@%value@"\"");
			eval(%command);
			return;
		}
	}
	else if (strFind(%syncData,"."))
	{
		eval("%testObj = "@%syncData@";");

		if (isObject(%testObj))
		{
			%testObj.setFieldValue(%field,%value);
		}
		else {
		   eval(%syncData@" = \""@%value@"\";");
		}
		return;
	}

}


//------------------------------------------------------------------------------
function setParamCtrlValue(%ctrl,%value,%field,%paramArray)
{
	if (!isObject(%ctrl))
	{
		warnLog("Couln't find a valid GuiControl holding the value for field:",%field);
		return;
	}

	//===========================================================================
	// Special script for GuiColorPickerCtrl Params
	if (%ctrl.getClassName() $= "GuiColorPickerCtrl" && false)
	{
		%ctrl.updateCommand = %updateFunc@"(%ctrl.internalName,%color,%ctrl,\""@%array@"\",\""@%arg1@"\",\""@%arg2@"\");";
		return;
	}

	//---------------------------------------------------------------------------
	%ctrl.setTypeValue(%value);
	%ctrl.updateFriends();

	foreach$(%syncCtrl in %ctrl.syncCtrls)
		%syncCtrl.setTypeValue(%value);
}
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
function getParamValue(%paramArray,%field,%fromData)
{
	%syncData = %paramArray.syncData[%field];

	//if no syncData, try with the old system
	if (%syncData $= "")
	{
		%data = %paramArray.getVal(%field);
		%syncData = getField(%data,4);
	}

	//If fromData true, we want to get the value using syncdata only
	if (!%fromData)
	{
		%container = %paramArray.container;
		%ctrl = %container.findObjectByInternalName(%field,true);

		if (isObject(%ctrl))
		{
			%value = %ctrl.getTypeValue();
			return %value;
		}
	}

	//Skip Direct Sync if specified
	if (%syncData $="")
	{
		paramLog("getParamValue failed with empty SyncData! Param:",%paramArray.getName(),"Field",%field);
		return;
	}

	if (isObject(%syncData))
	{
		%value = %syncData.getFieldValue(%field);
		return %value;
	}

	if (%paramArray.autoSyncPref)
	{
		eval("%value = "@%paramArray.prefGroup@%field@";");

		if (%value !$= "")
			return %value;
	}

	//Check for a standard global starting with $
	if (getSubStr(%syncData,0,1) $= "$")
	{
		%lastChar = getSubStr(%syncData,strlen(%syncData)-1,1);

		if (%lastChar $= ":" || %lastChar $= "_")
		{
			//Incomplete global, need to add the field
			eval("%value = "@%syncData@%field@";");
			return %value;
		}

		eval("%value = "@%syncData@";");
		return %value;
	}

	if (strFind(%syncData,"::"))
	{
		eval("%value = "@%paramArray.prefGroup@%syncData@";");
		return %value;
	}

	if (strFind(%syncData,".") && !strFind(%syncData,";"))
	{
		//Check for a standard global starting with $
		eval("%testObj = "@%syncData@";");

		if (isObject(%testObj))
		{
			%value = %testObj.getFieldValue(%field);
			return %value;
		}
		else if (%testObj !$="")
		{
			return %testObj;
		}
	}

	paramLog("Blank value for sync:",%syncData);
}
//------------------------------------------------------------------------------