//==============================================================================
// HelpersLab -> Build the params from array data
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
// Param Array Object Options
//------------------------------------------------------------------------------
// style          => Style of the Controls to be created (Default-> Default_230)
// container      => GuiControl that hold all the Params Stacks
// aggregateStyle => Aggregate class to use if needed
// prefGroup      => Global pref base to save all values

//==============================================================================

$ParamsArray_WidgetPrefix = "wParams_";
$ParamsArray_DefaultStyle = "Default_230";
$ParamsArray_AggregateClass = "AggregateVar";
$ParamsArray_DefaultStack = "Params_Stack";
$ParamsArray_DefaultStackType = "Rollout";
//==============================================================================
// RPE_DatablockEditor.buildInterface(); buildParamsArray(AxisGizmo_Param)
function buildParamsArray( %array,%syncAfter )
{
	//======================================================================
	// Prepare the array for the build process (Set default for unsetted settings)
	//----------------------------------------------------------------------
	%guiStyle = %array.style;

	if (%guiStyle $= "")
		%guiStyle = $ParamsArray_DefaultStyle;

	%guiSource = $ParamsArray_WidgetPrefix@%guiStyle;
	
	if (!isObject(wParams_StyleA) && %guiStyle $= "StyleA")
	   exec("tlab/EditorLab/gui/LabWidgetsGui.gui");

	if (!isObject(%guiSource))
	{
		if (%array.pillCreatedCheck)
		{
			%checkPill = %array.container.findObjectByInternalName(%pData.setting, true);
		}

		warnLog("Can't build params because the widgets source is invalid:",%guiSource);
		return;
	}

	%guiSourceOriginalExtentX = %guiSource.extent.x;
	%guiSourceOriginalExtentY = %guiSource.extent.y;
	%defaultStackType = $ParamsArray_DefaultStackType;
	%cloneFromGui = %guiSource;

	//Check for multi column param container (GuiFrameSetCtrl)
	if (%guiSource.columnCount !$="")
	{
		%multiCol = %guiSource.columnCount;
		%cloneFromGui = %guiSource-->ColData;
		%defaultStackType = "Collapse";
	}

	if ( %array.aggregateStyle !$= "")
		%aggregateClass = "Aggregate"@%array.aggregateStyle;

	//Check if a updateFunction is supplied
	if (%array.useNewSystem)
	{
		%array.common["command"] = "syncParamArrayCtrl($ThisControl,\""@%array.updateFunc@"\",\""@%array.getName()@"\",\"\",\"\");";
		%array.common["altCommand"] = "syncParamArrayCtrl($ThisControl,\""@%array.updateFunc@"\",\""@%array.getName()@"\",\"1\",\"\");";
	}
	else
	{
		%array.common["command"] = "updateParamArrayCtrl($ThisControl,\""@%array.updateFunc@"\",\""@%array.getName()@"\",\"\",\"\");";
		%array.common["altCommand"] = "updateParamArrayCtrl($ThisControl,\""@%array.updateFunc@"\",\""@%array.getName()@"\",\"1\",\"\");";
	}

	%groupFieldId = 6;

	if (%array.groupFieldId !$= "")
		%groupFieldId =%array.groupFieldId;

	//======================================================================
	// Prepare all params group data
	//----------------------------------------------------------------------
	%gid = 1;

	while(%array.group[%gid] !$="")
	{
		%groupInfo = %array.group[%gid];
		%groupTitle = getField(%groupInfo,0);
		%groupOptions = getField(%groupInfo,1);
		//Convert options to field string (;; become TAB)
		%groupOptFields = strreplace(%groupOptions,";;","\t");

		//Store group options found
		for(%gi=0; %gi<getFieldCount(%groupOptFields); %gi++)
		{
			%gData = getField(%groupOptFields,%gi);
			%gField = firstWord(%gData);
			%gFieldValue = removeWord(%gData,0);
			%groupOption[%gid,%gField] = %gFieldValue;
		}

		//Group ctrl type is the internal name of group holder in widgets source
		%groupCtrlType = %defaultStackType;

		//Check for specific group StackType in options
		if (%groupOption[%gid,"StackType"] !$= "")
			%groupCtrlType = %groupOption[%gid,"StackType"];

		//======================================================================
		//Get the BaseCtrl Source (Ctrl to add group pill to)
		//----------------------------------------------------------------------
		%baseCtrl = %array.container;

		//Check for specific Container object specified in options
		if (%groupOption[%gid,"Container"] !$= "")
			%baseCtrl = %groupOption[%gid,"Container"];
		//Check for specific Stack Internal name specified in options
		else if (%groupOption[%gid,"Stack"] !$= "" && isObject(%array.container))
		{
			%baseCtrl = %array.container.findObjectByInternalName(%groupOption[%gid,"Stack"],true);
		}
		else if (%groupOption[%gid,"StackObj"] !$= "")
		{
			eval("%baseCtrl = "@%groupOption[%gid,"StackObj"]@";");
		}

		if (!isObject(%baseCtrl))
		{
			warnLog("Invalid Params Array Container: GroupTitle",%groupTitle,"Array obj:",%array);
			%gid++;
			continue;
		}

		if (!%baseCtrlClear[%baseCtrl] && !%array.noContainerClear)
		{
			%baseCtrl.clear();
			%baseCtrlClear[%baseCtrl] = true;
		}

		//======================================================================
		// Set the stack control in which the group pills will be added
		//----------------------------------------------------------------------
		//If Type is set to none or is empty, the base stack will be used
		if (%groupCtrlType !$= "none" && %groupCtrlType !$= "")
		{
			//------------------------------------------------
			// Get the source widgets used to add pills (must have stack as children)
			%displayType = getWord(%groupStackType,0);
			%displayOptions = getWords(%groupStackType,1);
			%displayWidget = %guiSource.findObjectByInternalName(%groupCtrlType,true);

			if (!isObject(%displayWidget))
			{
				warnLog("Invalid group control type for group:",%groupTitle,"Using default type. Type tried was:",%groupCtrlType,"Source",%guiSource);
				%groupCtrlType = %defaultStackType;
				%displayWidget = %guiSource.findObjectByInternalName(%groupCtrlType,true);

				if (!isObject(%displayWidget))
				{
					warnLog("Something is not configurated right, can't generate the default group type:",%defaultStackType);
					%gid++;
					continue;
				}
			}

			%displayCtrl = cloneObject(%displayWidget);

			if (%groupCtrlType $= "Rollout")
				%displayCtrl.caption = %groupTitle;
			else if (%groupCtrlType $= "Header")
				%displayCtrl-->title.text = %groupTitle;

			if (%groupOption[%gid,"InternalName"] !$= "")
			{
				%displayCtrl.internalName = %groupOption[%gid,"InternalName"];
			}

			if (%groupOption[%gid,"expanded"] !$= "")
			{
				%displayCtrl.expanded = %groupOption[%gid,"expanded"];
			}

			if (%groupOption[%gid,"autoCollapse"] !$= "")
			{
				%displayCtrl.autoCollapseSiblings = %groupOption[%gid,"autoCollapse"];
			}

			%baseCtrl.add(%displayCtrl);
		}
		else
		{
			%displayCtrl.addDirect = true;
			%displayCtrl = %baseCtrl;
		}

		//Prepare the DisplayCtrl for Multi Columns System if Columns set
		if (%groupOption[%gid,"Columns"] !$= "")
		{
			%columns = %groupOption[%gid,"Columns"];
			setMultiColParamOptions(%displayCtrl,%columns);
		}

		//Store the Group COntainer for the Group ID
		%groupCtrl[%gid] = %displayCtrl;
		//======================================================================
		// Group preparation completed, prepare for next group ID
		//----------------------------------------------------------------------
		%gid++;
	}

	//------------------------------------------------------------------------------
	//==============================================================================
	// GENERATE THE PARAMS FIELDS
	//------------------------------------------------
	//Group Fields Setup
	//Field 0 = Default (Global if start with $)
	//Field 1 = Title
	//Field 2 = Type
	//Field 3 = Options
	//Field 4 = SyncObjs
	//Field 5 = UpdCmd
	//Field Last = groupId
	for( %i = 0; %i < %array.count() ; %i++)
	{
		%field = %array.getKey(%i);
		%data = %array.getValue(%i);
		%pData = newScriptObject("paramDataHolder");
		%pData.Setting = %field;

		if (%array.noDefaults)
		{
			%newdata = "" TAB %data;
			%array.setVal(%field,%newdata);
			%data = %newData;
		}

		%groupFieldId = getFieldCount(%data) - 1;
		%fieldId = -1;

		if (%array.fields $= "")
			%array.fields = "Default Title Type Options syncObjs";

		%fid = -1;

		foreach$(%dataField in %array.fields)
		{
			%dataValue = getField(%data,%fid++);
			eval("%pData."@%dataField@" = %dataValue;");
		}

		if(%groupFieldId > 5)
			%pData.validation = getField(%data,%fieldId++);

		//}
		/*else {
			%pData.Default = getField(%data,%fieldId++);
			%pData.Title = getField(%data,%fieldId++);
			%pData.Type = getField(%data,%fieldId++);
			%pData.Options = getField(%data,%fieldId++);
			%pData.syncObjs = getField(%data,%fieldId++);
			%array.syncObjsField = %fieldId;

			if(%groupFieldId > 5)
				%pData.validation = getField(%data,%fieldId++);
		}
		*/

		//Make sure it have a type for building
		if (%pData.Type $= "")
		{
			%array.noSyncField[%field] = true;
			continue;
		}

		//if (%array.noDefaults)
		//%pData.Default = getField(%data,%fieldId++);
		%pData.srcData = getWord(%pData.syncObjs,0);

		if (%pData.Type $= "None")
			continue;

		if (strFind(%pData.syncObjs,">>"))
		{
			%syncFields = strReplace(%pData.syncObjs,">>","\t");
			%pData.syncObjs = getField(%syncFields,0);
			%pData.updCmd = getField(%syncFields,1);
		}

		%array.syncData[%field] = %pData.syncObjs;
		%array.updCmd[%field] = %pData.updCmd;
		%pData.Command = %array.common["Command"];
		%pData.AltCommand = %array.common["AltCommand"];

		if (%pData.Title $= "")
			%pData.Title = %pData.Setting;

		%pillSrc = %pData.Type;

		if (%pillSrc $= "ColorInt")
			%pillSrc = "Color";

		//Set the WidgetSource the same width as target
		if (%multiCol $= "")
		{
			%widgetSourceWidth = %guiSource.extent.x;
			%guiSource.setExtent(%pData.parentCtrl.extent.x,%guiSource.extent.y);
			%guiSource-->widgets.setExtent(%pData.parentCtrl.extent.x,%guiSource.extent.y);
		}

		%pData.Widget = %cloneFromGui.findObjectByInternalName(%pillSrc,true);

		//If array only, it will use existing pills
		if ((%array.arrayOnly || %groupCtrl[1] $= "") && isObject(%array.container))
		{
			%checkPill = %array.container.findObjectByInternalName(%pData.setting, true);
			%pData.pill = %checkPill;
			%pData.pill.updField = %pData.setting;
			%pData.pill.updObj = %pData.syncObjs;
			continue;
		}

		%pData.groupId = getField(%data,%groupFieldId);

		if (%pData.groupId $= "")
			%pData.groupId = "1";

		%basePillCtrl = %groupCtrl[%pData.groupId];

		if (%basePillCtrl.addDirect)
			%pData.parentCtrl = %basePillCtrl;
		else
			%pData.parentCtrl = %basePillCtrl-->stackCtrl;

		if (%pData.Type $= "CloneCtrl")
		{
			%ctrlHolder = %pData.setting.deepClone();
			%ctrlHolder.visible = 1;
			%pData.setting.visible = 0;
			%pData.parentCtrl.add(%ctrlHolder);
			%removeKeyList = strAddWord(%removeKeyList,%field);
			continue;
		}

		if (!isObject(%pData.parentCtrl))
		{
			paramLog("Param skipped due to invalid parent ctrl! Skipped setting=",%pData.Setting,%data);
			continue;
		}

		if(!isObject(%pData.Widget))
		{
			paramLog(%pData.Setting,"Couldn't find widget for setting type:",%pData.Type,"This setting building is skipped Widget Style:",%guiStyle);
			%fid++;
			continue;
		}

		%pData.pill = cloneObject(%pData.Widget);
		%pData.pill.updField = %pData.setting;
		%pData.pill.updObj = %pData.syncObjs;

		if (%array.mouseAreaClass !$= "")
		{
			%mouseArea = %pData.pill-->MouseArea;

			if (isObject(%mouseArea))
			{
				show(%mouseArea);
				%mouseArea.fitIntoParents();
				%mouseArea.superClass = %array.mouseAreaClass;
			}
		}

		%pData.pill.paramObj = %array;

		if (%multiCol > 0)
		{
			//Get the leftData Type from tooltip and set tooltip empty
			%leftData = %pData.pill.tooltip;
			%pData.pill.tooltip = "";
			//Clone the leftPill from the Souce child with internalName as leftData
			%pData.leftPill = cloneObject(%guiSource-->ColInfo.findObjectByInternalName(%leftData,true));
			//Add left pill to pill so it get update normally for buildTypes
			%pData.pill.add(%pData.leftPill);
			//Store the Left Pill Stack container in which the pill will be added
			%pData.infoCtrl = %basePillCtrl-->stackInfo;
			//Make sure the LeftPill is same height as Main Pill
			%pData.leftPill.extent.y = %pData.pill.extent.y;
		}

		//Overide aggregate if set and custom is specified
		if (%pData.pill.class !$="" && %aggregateClass !$="")
			%pData.pill.class = %aggregateClass;

		//------------------------------------------------
		//Prepare the field special options
		// The options are applied for each Field Category alone
		%pData.OptionList[%pData.Setting] = "";
		%optionsList = strreplace(%pData.Options,";;","\t");
		%optionsCount = getFieldCount(%optionsList);
		%optDiv = ">>";

		if (strFind(%optionsList,"::"))
		{
			paramLog("Old param options divider (::) detected for field:",%pData.Setting,"In array",%array.getName());
			%optDiv = "::";
		}

		for(%j=0; %j<%optionsCount; %j++)
		{
			%option = getField(%optionsList ,%j);
			%optWords = strreplace(%option,%optDiv," ");
			%optField = getWord(%optWords,0);
			%optCmd = getWords(%optWords,1);
			%pData.Option[%pData.Setting,%optField] = %optCmd;
			%pData.OptionCmd[%pData.Setting,%optField] = "."@%optField@" = \""@ %optCmd  @"\";";
			%pData.OptionList[%pData.Setting] = trim(%pData.OptionList[%pData.Setting] SPC %optField);
		}

		%pData.InternalName = %pData.Setting;
		%pData.Variable = "";

		if (getSubStr(%pData.InternalName,0,1) $= "$")
		{
			%pData.Variable = %pData.InternalName;
			%pData.InternalName = strreplace(%pData.InternalName,"$","");
			%pData.InternalName = strreplace(%pData.InternalName,"::","__");
			eval("%pData.Value = "@%pData.Variable@";");
		}

		if (%pData.Option[%pData.Setting,"variable"] !$= "")
			%pData.Variable = %pData.Option[%pData.Setting,"variable"];

		if (%pData.Variable $= "")
		{
			if(%array.prefGroup !$= "")
				%pData.Variable = %array.prefGroup @ %pData.Setting;
		}

		%tmpFieldValue = %pData.Value;
		%multiplier = 1;
		%tooltip = %pData.Option[%pData.Setting,"tooltip"];
		%tooltipDelay = 1000;
		%pData.mouseAreaClass = %pData.Option[%pData.Setting,"mouseClass"];
		//Get the category of paramCtrl (category stop at _ in type)
		%paramType = strreplace(%pData.Type,"_"," ");
		%pData.Category = getWord(%paramType,0);
		//Check for name for field type holder
		%nameOpt = %pData.Option[%pData.Setting,"name"];

		if (%nameOpt !$= "")
		{
			switch$(%nameOpt)
			{
			case "prefix":
				%prefix = %array.namePrefix;

				if (%prefix $= "")
				{
					warnLog("The param array:",%array.getName(),
							  "contain to namePrefix for auto naming using prefix mode.",
							  " The prefix will be the array internal name:",%array.internalName);
					%prefix = %array.internalName;
				}

				%pData.myNameIs = %prefix @ %pData.Setting @ %pData.Category;
			}
		}

		//=============================================================
		//Call the predefined function for the GuiCtrl type
		//-------------------------------------------------------------

		if (isFunction("buildParam"@%pData.Category))
			eval("%ctrlHolder = buildParam"@%pData.Category@"(%pData);");
		else
			paramLog("Couldn't create the param, there's no function for that control type:",%pData.Category);

		//=============================================================
		//The Pills are set for the specific type, do final update before adding to stack
		//-------------------------------------------------------------

		//If a ctrlHolder is returned, set some validation options
		if (%ctrlHolder)
		{
			if (%pData.Option[%pData.Setting,"validate"] !$= "")
			{
				%validate = %pData.Option[%pData.Setting,"validate"];
				%ctrlHolder.validateFunc = %validate;
				%ctrlHolder.friend.validateFunc = %validate;
			}

			if (%pData.validation !$= "")
			{
				%ctrlHolder.validationType = %pData.validation;
				%ctrlHolder.friend.validationType = %pData.validation;
			}
		}

		//Get the GuiCtrl which have the setting as internal name
		%fieldCtrl = %pData.pill.findObjectByInternalName(%pData.Setting,true);

		//Check some option settings and add those found to fieldCtrl
		if (%pData.Option[%pData.Setting,"superClass"] !$= "")
			%fieldCtrl.superClass = %pData.Option[%pData.Setting,"superClass"];

		if (%pData.Option[%pData.Setting,"linkSet"] !$= "")
			%fieldCtrl.linkSet = %pData.Option[%pData.Setting,"linkSet"];

		%pData.pill-->field.internalName = "fieldTitle";
		%pData.pill-->fieldTitle.canSaveDynamicFields = "1";
		%pData.pill-->fieldTitle.linkedField = %field;
		%pData.pill-->mouseArea.linkedField = %field;
		%array.pill[%field] = %pData.pill;
		%array.title[%field] =  %pData.pill-->fieldTitle.text;
		//=============================================================
		//New pill created, add it to stack
		%pData.parentCtrl.add(%pData.pill);

		if (%multiCol > 0)
		{
			if (isObject(%pData.leftPill))
				%pData.infoCtrl.add(%pData.leftPill);

			schedule(200,0,"resizeMultiColContainer",%basePillCtrl);
		}

		%array.pData[%field] = %pData;

		if (%array.paramCallback !$= "")
			eval(%array.paramCallback@"(%array,%field,%pData);");
	}

	if (%multiCol > 0)
	{
	}

	//Removed keys of ctrl we don't want to sync
	foreach$(%key in %removeKeyList)
	{
		%index = %array.getIndexFromKey(%key);
		%array.erase(%index);
	}

	if (%syncAfter)
		syncParamArray(%array);

	%guiSource.setExtent(%guiSourceOriginalExtentX,%guiSourceOriginalExtentY);
}
//------------------------------------------------------------------------------

//==============================================================================
// Multi Column Param Building Functions
//==============================================================================
//==============================================================================
//Resize the GuiFrameSetCtrl to fit the stack into
function resizeMultiColContainer(%basePillCtrl)
{
	%baseX = %basePillCtrl.extent.x;
	%stackMax = getMax(%basePillCtrl-->stackCtrl.extent.y,%basePillCtrl-->stackInfo.extent.y);
	%baseY = %stackMax + %basePillCtrl.offsetY;
	%basePillCtrl.setExtent(%baseX,%baseY);

	if (isObject(%basePillCtrl.frameSet))
	{
		%basePillCtrl.frameSet.setColumns(%basePillCtrl.frameSet.baseColumns);
	}
}
//------------------------------------------------------------------------------
function setMultiColParamOptions(%baseCtrl,%columns)
{
	%attempts = 4;
	%checkCtrl = %baseCtrl;

	while (%attempts > 0)
	{
		if (%checkCtrl.getClassName() $= "GuiFrameSetCtrl")
			break;

		%checkCtrl = %checkCtrl.getObject(0);

		if (!isObject(%checkCtrl))
			break;

		%attempts--;
	}

	if (!isObject(%checkCtrl))
		warnLog("Couln't find a GuiFrameSet to set the columns");
	else
	{
		%baseCtrl.frameSet = %checkCtrl;
		%checkCtrl.baseColumns = %columns;
	}
}
//------------------------------------------------------------------------------
