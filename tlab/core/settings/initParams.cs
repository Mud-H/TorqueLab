//==============================================================================
// GameLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

$LabParamsStyle = "StyleA";
//==============================================================================
// New ParamsArray creation system (to replace newParamsArray over time)
function Lab::createParamsCategory(%this,%category,%categoryDisplay) {
    if (%category $= "")
      return;
   if (%categoryDisplay $= "")
      %categoryDisplay = %category;
      
   $LabParamCategoryDisplay[%category] = %categoryDisplay;
   
}
//==============================================================================
// New ParamsArray creation system (to replace newParamsArray over time)
function Lab::createParamsArray(%this,%category,%typeData,%container) {
	if (%category $= "" || %typeData $= "") {
		warnLog("You need to specify a category and group for each params array");
		return;
	}
   
   
   //%categoryData and %typeData can be in form of "Shortname" TAB "Display Name"  
   %categoryDisplay =  $LabParamCategoryDisplay[%category];
   if (%categoryDisplay $= "")
	   %categoryDisplay = %category;
	   
   %type = getField(%typeData,0);
   %typeDisplay =  getField(%typeData,1);
   if (%typeDisplay $= "")
	   %typeDisplay = %type @ " settings";
		
   %fullname = %category@"_"@%type;	
   if (!isObject(%container))
       %container = %fullname@"ParamStack";	
	%arrayName = getUniqueName("ar_"@%fullname);

	%array = newArrayObject(%arrayName,LabParamsGroup,"ParamArray");
	%array.internalName = %fullname;
	%array.displayName = %typeDisplay;
	%array.container = %container;
	%array.paramCallback = "Lab.onParamBuild";
	
	%array.group = %category;
	%array.category = %category;
	%array.type = %type;
	%array.set = %fullname;
	%array.useNewSystem = true;
	
	
//==============================================================================
//TO BE CHECKED
	%array.fields = "Default Title Type Options syncObjs";
	%array.syncObjsField = 4;
//==============================================================================
	//If no cfgObject supplied, simply use the new array as object
	if (!isObject(%cfgObject))
		%cfgObject = %array;

	%array.cfgObject = %cfgObject;
	%array.groupLink = %fullname;
   %array.cfgData = %fullname;
   
   %array.prefBase = "$Cfg_"@%fullname@"_";

   %prefGroup = %fullname;

	%array.prefGroup = %prefGroup;
	%array.updateFunc = "LabParams.updateParamArrayCtrl";
	//%array.common["command"] = "syncParamArrayCtrl($ThisControl,\"LabParams.updateParamArrayCtrl\",\""@%fullName@"\",\"\",\"\");";
	//%array.common["altCommand"] = "syncParamArrayCtrl($ThisControl,\"LabParams.updateParamArrayCtrl\",\""@%fullName@"\",\"true\",\"\");";
	return %array;
}
//------------------------------------------------------------------------------
//==============================================================================
// Basic Params using helpers system called here to make sure widgets are loaded
function Lab::createBaseParamsArray(%this,%paramName,%container) {
   %paramWords = strReplace(%paramName,"_"," ");
   %cat =  getWord(%paramWords,0); 
    %type =  getWord(%paramWords,1); 
    if (%type $= "")
    {
       %cat =  "base"; 
      %type =  getWord(%paramWords,0);        
    }
  
	%arCfg = Lab.createParamsArray(%cat,%type,%container);	
	if (!isObject(wParams_StyleA))
	   exec("tlab/EditorLab/gui/LabWidgetsGui.gui");
	%arCfg.style = "StyleA";
	return %arCfg;
}

//==============================================================================
//Initialize plugin data
function Lab::onParamBuild(%this,%array,%field,%paramData) {
}
//------------------------------------------------------------------------------
//==============================================================================
//Initialize plugin data
function Lab::onParamPluginBuild(%this,%array,%field,%paramData) {
	%plugin = %array.pluginObj;
	%cfgValue = %plugin.getCfg(%field);

	if (%cfgValue $= "")
		%plugin.setCfg(%field,%paramData.Default);
}
//------------------------------------------------------------------------------
/*
Kept in case problem with plugins since they were still using it
//==============================================================================
//Initialize plugin data
function Lab::newParamsArray(%this,%nameFlds,%groupFlds,%cfgObject,%useLongName) {
	if (%nameFlds $= "") {
		warnLog("You need to specify a name for the settings which is unique in this type");
		return;
	}

	%name = getField(%nameFlds,0);
	%nameCode = getField(%nameFlds,1);

	if (%nameCode $= "")
		%nameCode = %name;

	%group = getField(%groupFlds,0);
	%groupCode = getField(%groupFlds,1);

	if (%groupCode $= "")
		%groupCode = %group;

	if (%container $= "")
		%container = %name@"ParamStack";

	%name = strreplace(%name," ","_");
	%arrayName = %name;

	if (%useLongName)
		%arrayName = %groupCode@%nameCode;

	%fullName = %arrayName@"_Param";
	%array = newArrayObject(%fullName,LabParamsGroup,"ParamArray");
	%array.internalName = %name;
	%array.displayName = %name;
	%array.container = %container;
	%array.paramCallback = "Lab.onParamBuild";
	%array.fields = "Default Title Type Options syncObjs";
	%array.syncObjsField = 4;
	%array.group = %group;
	%array.set = %nameCode;
	%array.useNewSystem = true;
	%pData.Default = getField(%data,%fieldId++);
	%pData.Title = getField(%data,%fieldId++);
	%pData.Type = getField(%data,%fieldId++);
	%pData.Options = getField(%data,%fieldId++);
	%pData.syncObjs = getField(%data,%fieldId++);

	//If no cfgObject supplied, simply use the new array as object
	if (!isObject(%cfgObject))
		%cfgObject = %array;

	%array.cfgObject = %cfgObject;
	%array.groupLink = %groupCode@"_"@%nameCode;
%array.cfgData = %array.groupLink;
	if (%prefGroup $= "")
		%prefGroup = %name;

	%array.prefGroup = %prefGroup;
	%array.updateFunc = "LabParams.updateParamArrayCtrl";
	//%array.common["command"] = "syncParamArrayCtrl($ThisControl,\"LabParams.updateParamArrayCtrl\",\""@%fullName@"\",\"\",\"\");";
	//%array.common["altCommand"] = "syncParamArrayCtrl($ThisControl,\"LabParams.updateParamArrayCtrl\",\""@%fullName@"\",\"true\",\"\");";
	return %array;
}
//------------------------------------------------------------------------------
*/