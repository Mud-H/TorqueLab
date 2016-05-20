//==============================================================================
// Lab GuiManager -> Profile Params System
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
function GLab::rebuildProfileParams( %this ) {
	exec("tlab/EditorLab/gui/editorDialogs/guiLab/profileScripts/profileParams.cs");
	%this.initProfileParams();
}
//==============================================================================
// Create and define the Profile params array
function GLab::initProfileParams( %this ) {
	$GLab_FontList = $GLab::FontList;
	%arCfg = Lab.createBaseParamsArray("GLab_Profile",GLab_ProfileParamStack);
	%arCfg.updateFunc = "GLab.updateProfileParam";
	%arCfg.finalUpdateFunc = "GLab.finalUpdateProfileParam";
	%arCfg.style = "StyleA";
	%arCfg.mouseAreaClass = "GLab_FieldInfoMouse";
	%arCfg.groupFieldId = 5;
	%arCfg.useNewSystem = true;
	%arCfg.customUpdateOnly = true;
	%arCfg.group[%gid++] = "General settings";
	%arCfg.setVal("name",       "" TAB "Name" TAB "TextEdit_75" TAB "" TAB "$GLab_SelectedObject" TAB %gid);
	%arCfg.setVal("category",       "" TAB "Category" TAB "dropdown_75" TAB "itemList>>$GLab_ProfileCategories" TAB "$GLab_SelectedObject" TAB %gid);
	%arCfg.setVal("hasBitmapArray",       "" TAB "hasBitmapArray" TAB "Checkbox" TAB "" TAB "$GLab_SelectedObject" TAB %gid);
	%arCfg.setVal("bitmap",       "" TAB "bitmap" TAB "FileSelect" TAB "callback>>GLab.getProfileBitmapFile();" TAB "$GLab_SelectedObject" TAB %gid);
	%arCfg.setVal("justify",       "" TAB "justify" TAB "dropdown" TAB "itemList>>$GLab_JustifyOptions" TAB "$GLab_SelectedObject" TAB %gid);
	%arCfg.group[%gid++] = "Color sets config";
	%arCfg.setVal("colorFont",       "" TAB "Font color set" TAB "dropdown" TAB "itemList>>$GLab_ColorGroupSetListColorFont" TAB "$GLab_SelectedObject" TAB %gid);
	%arCfg.setVal("colorFill",       "" TAB "Fill color set" TAB "dropdown" TAB "itemList>>$GLab_ColorGroupSetListColorFill" TAB "$GLab_SelectedObject" TAB %gid);
	%arCfg.group[%gid++] = "Font settings";
	%arCfg.setVal("fontType",        "" TAB "fontType" TAB "dropdownEdit" TAB "fieldList>>$GLab_FullFontList;;guiGroup>>ggFontTypesGLab" TAB "$GLab_SelectedObject"  TAB %gid);
	%arCfg.setVal("fontSize",        "" TAB "fontSize" TAB "TextEdit_sml" TAB "" TAB "$GLab_SelectedObject"  TAB %gid);
	%arCfg.setVal("fontColor",        "" TAB "fontColor" TAB "ColorEdit" TAB "mode>>int" TAB "$GLab_SelectedObject"  TAB %gid);
	%arCfg.setVal("fontColorHL",        "" TAB "fontColorHL [1]" TAB "ColorEdit" TAB "mode>>int" TAB "$GLab_SelectedObject"  TAB %gid);
	%arCfg.setVal("fontColorNA",        "" TAB "fontColorNA [2]" TAB "ColorEdit" TAB "mode>>int" TAB "$GLab_SelectedObject"  TAB %gid);
	%arCfg.setVal("fontColorSEL",        "" TAB "fontColorSEL [3]" TAB "ColorEdit" TAB "mode>>int" TAB "$GLab_SelectedObject"  TAB %gid);
	%arCfg.setVal("fontColorLink",        "" TAB "fontColorLink [4]" TAB "ColorEdit" TAB "mode>>int" TAB "$GLab_SelectedObject"  TAB %gid);
	%arCfg.setVal("fontColorLinkHL",        "" TAB "fontColorLinkHL [5]" TAB "ColorEdit" TAB "mode>>int" TAB "$GLab_SelectedObject"  TAB %gid);
	%arCfg.setVal("fontColors_6",        "" TAB "fontColor [6]" TAB "ColorEdit" TAB "mode>>int" TAB "$GLab_SelectedObject"  TAB %gid);
	%arCfg.setVal("fontColors_7",        "" TAB "fontColor [7]" TAB "ColorEdit" TAB "mode>>int" TAB "$GLab_SelectedObject"  TAB %gid);
	%arCfg.setVal("fontColors_8",        "" TAB "fontColor [8]" TAB "ColorEdit" TAB "mode>>int" TAB "$GLab_SelectedObject"  TAB %gid);
	%arCfg.setVal("fontColors_9",        "" TAB "fontColor [9]" TAB "ColorEdit" TAB "mode>>int" TAB "$GLab_SelectedObject"  TAB %gid);
	%arCfg.setVal("textOffset",       "" TAB "textOffset" TAB "TextEdit" TAB "" TAB "$GLab_SelectedObject" TAB %gid);
	// fontColor = fontColors[0]
// fontColorHL = fontColors[1]
// fontColorNA = fontColors[2]
// fontColorSEL = fontColors[3]
// fontColorLink = fontColors[4]
// fontColorLinkHL = fontColors[5]
	%arCfg.group[%gid++] = "Fill colors";
	%arCfg.setVal("opaque",       "" TAB "Filling is opaque?" TAB "Checkbox" TAB "" TAB "$GLab_SelectedObject" TAB %gid);
	%arCfg.setVal("fillColor",        "" TAB "fillColor" TAB "ColorSlider" TAB "mode>>int" TAB "$GLab_SelectedObject"  TAB %gid);
	%arCfg.setVal("fillColorHL",        "" TAB "fillColorHL" TAB "ColorSlider" TAB "mode>>int" TAB "$GLab_SelectedObject"  TAB %gid);
	%arCfg.setVal("fillColorSEL",        "" TAB "fillColorSEL" TAB "ColorSlider" TAB "mode>>int" TAB "$GLab_SelectedObject"  TAB %gid);
	%arCfg.setVal("fillColorNA",        "" TAB "fillColorNA" TAB "ColorSlider" TAB "mode>>int" TAB "$GLab_SelectedObject"  TAB %gid);
	//%arCfg.setVal("profileTest",       "" TAB "ProfileTest" TAB "dropdown" TAB "itemGroup::GameProfileGroup" TAB "$GLab_SelectedObject" TAB %gid);
	%arCfg.group[%gid++] = "Border settings";
	%arCfg.setVal("border",       "" TAB "border" TAB "TextEdit_sml" TAB "" TAB "$GLab_SelectedObject" TAB %gid);
	%arCfg.setVal("borderThickness",       "" TAB "borderThickness" TAB "TextEdit_sml" TAB "" TAB "$GLab_SelectedObject" TAB %gid);
	%arCfg.setVal("borderColor",        "" TAB "borderColor" TAB "Color" TAB "mode>>int" TAB "$GLab_SelectedObject"  TAB %gid);
	%arCfg.setVal("borderColorHL",        "" TAB "borderColorHL" TAB "Color" TAB "mode>>int" TAB "$GLab_SelectedObject"  TAB %gid);
	%arCfg.setVal("borderColorNA",        "" TAB "borderColorNA" TAB "Color" TAB "mode>>int" TAB "$GLab_SelectedObject"  TAB %gid);
	%arCfg.group[%gid++] = "Advanced settings";
	%arCfg.setVal("autoSizeWidth",       "" TAB "autoSizeWidth" TAB "Checkbox" TAB "" TAB "$GLab_SelectedObject" TAB %gid);
	%arCfg.setVal("autoSizeHeight",       "" TAB "autoSizeHeight" TAB "Checkbox" TAB "" TAB "$GLab_SelectedObject" TAB %gid);
	%arCfg.setVal("returnTab",       "" TAB "returnTab" TAB "Checkbox" TAB "" TAB "$GLab_SelectedObject" TAB %gid);
	%arCfg.setVal("tab",       "" TAB "tab" TAB "Checkbox" TAB "" TAB "$GLab_SelectedObject" TAB %gid);
	%arCfg.setVal("numbersOnly",       "" TAB "numbersOnly" TAB "Checkbox" TAB "" TAB "$GLab_SelectedObject" TAB %gid);
	%arCfg.setVal("canKeyFocus",       "" TAB "canKeyFocus" TAB "Checkbox" TAB "" TAB "$GLab_SelectedObject" TAB %gid);
	%arCfg.setVal("modal",       "" TAB "modal" TAB "Checkbox" TAB "" TAB "$GLab_SelectedObject" TAB %gid);
	%arCfg.setVal("mouseOverSelected",       "" TAB "mouseOverSelected" TAB "Checkbox" TAB "" TAB "$GLab_SelectedObject" TAB %gid);
	%arCfg.setVal("cursorColor",        "" TAB "cursorColor" TAB "Color" TAB "mode>>int" TAB "$GLab_SelectedObject"  TAB %gid);
	buildParamsArray(%arCfg,false);
	$GLab_ProfileParams = %arCfg;
}
//------------------------------------------------------------------------------

//==============================================================================
// Profile params update and syncing
//==============================================================================
//==============================================================================
// Called when a field have been changed
function GLab::updateProfileParam( %this, %fieldData,%value,%ctrl,%paramArray,%isAltCommand,%arg3 ) {
	logc("GLab updateProfileParam Ctrl",%ctrl,"Array",%paramArray,"field",%fieldData,"IsAlt",%isAltCommand);

	if (%value $= "") {
		warnLog("Can't set a profile field to empty yet");
		return;
	}

	%fieldWords = strreplace(%fieldData,"_"," ");
	%field = getWord(%fieldWords,0);
	%fieldId = getWord(%fieldWords,1);
	%arIndex = %paramArray.getIndexFromKey(%fieldData);
	%data = %paramArray.getValue(%arIndex);
	%objGlobal = getField(%data,4);
	eval("%obj = "@%objGlobal@";");

	//As now, the %syncObj should be a valid object directly
	if (!isObject(%obj)) {
		warnlog("Profile param sync obj is invalid object!!");
		return;
	}

	if ((%field $= "colorFont" || %field $= "colorFill") && !$GLab_EmbedColorSetInProfile) {
		GLab.setProfileColorFromSet(%value,%field);
		return;
	}

	if (%noUpdate)
		return;

	GLab.updateProfileField(%obj,%fieldData,%value,%fieldId);

	if (%isAltCommand) {
		GLab.updateProfileChildsField( %obj,%fieldData);
	}

	%ctrl = %paramArray.container.findObjectByInternalName(%field,true);
	//if (isObject(%ctrl))
	// %ctrl.updateFriends();
	//%obj.setFieldValue(%field,%value);
}
//------------------------------------------------------------------------------
//==============================================================================
// Sync the current profile values into the params objects
function GLab::syncProfileParamArray( %this ) {
	%paramArray = $GLab_ProfileParams;

	for( ; %i < %paramArray.count() ; %i++) {
		%field = %paramArray.getKey(%i);
		%data = %paramArray.getValue(%i);
		%this.syncProfileField(%field,%data);
	}
}
//------------------------------------------------------------------------------
//==============================================================================
// Sync the current profile values into the params objects
function GLab::syncProfileField( %this,%fieldData,%data ) {
	devLog("syncProfileField Field",%fieldData,"Data:",%data);
	%fieldWords = strreplace(%fieldData,"_"," ");
	%field = getWord(%fieldWords,0);
	%fieldId = getWord(%fieldWords,1);
	%realField = %field;

	if (%fieldId !$= "") {
		%realField = %field@"["@%fieldId@"]";
		devLog("syncProfileField Real field set with index:",%realField);
	}

	%paramArray = $GLab_ProfileParams;

	if (%data $= "") {
		%index = %paramArray.getIndexFromKey(%fieldData);
		%data = %paramArray.getValue(%index);
	}

	%pData = %paramArray.pData[%fieldData];
	%dataPill = %paramArray.pill[%fieldData];
	%title = %paramArray.title[%fieldData];
	//%paramArray.pill[%field] = %pData;
	%value = "";
	%objGlobal = getField(%data,4);
	%dataPill-->fieldTitle.guiGroup = "GLab_FieldButtons";
	addCtrlToGuiGroup( %dataPill-->fieldTitle);
	eval("%syncObj = "@%objGlobal@";");

	if (!isObject(%syncObj)) {
		warnlog("Invalid syncobj for field:",%field,"Data",%syncObj);
		return;
	}

	%ownList = $ProfOwnedFields[%syncObj.getName()];
	%ownField = strFind(%ownList,%realField);
	%dataPill.fieldTitle = %dataPill-->fieldTitle;
	%titleText = %title;

	if (!%ownField) {
		%titleText = "*" SPC %title;
	}

	%dataPill-->fieldTitle.text = %titleText;
	%value = %syncObj.getFieldValue(%field,%fieldId);

	if (%value $= "") return;

	%ctrl = %paramArray.container.findObjectByInternalName(%fieldData,true);
	devLog("Field",%field,"Sync:",%ctrl,"Class:",%ctrl.getCLassName());
	%ctrl.setTypeValue(%value);
	%ctrl.updateFriends();
}
//------------------------------------------------------------------------------
//==============================================================================
// Profile Custom params update
//==============================================================================
//==============================================================================
// Sync the current profile values into the params objects
function GLab::getProfileBitmapFile( %this ) {
	%currentFile = $GLab_SelectedObject.bitmap;
	getLoadFilename("*.*|*.*", "GLab.setProfileBitmapFile", %currentFile);
}
//------------------------------------------------------------------------------
//==============================================================================
// Sync the current profile values into the params objects
function GLab::setProfileBitmapFile( %this,%file ) {
	%filename = makeRelativePath( %file, getMainDotCsDir() );
	%this.updateProfileField($GLab_SelectedObject,"bitmap",%filename);
	%this.syncProfileField("bitmap");
}
//------------------------------------------------------------------------------
