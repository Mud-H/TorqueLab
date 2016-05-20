//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$LabProfileStyleActive = "";

//==============================================================================
function GLab::initProfileStyleParams(%this) {
	%gid = 0;
	%ar = Lab.createBaseParamsArray("ProfileStyle",GLab_ProfileStyleStackParams,"$Lab");
	%ar.validateSubField = true;
	%ar.style = "ColStyleB";
	//%ar.autoSyncPref = "1";
	%ar.useNewSystem = true;
	%ar.group[%gid++] = "Fill Color settings" TAB "Columns 0 130" ;
	%ar.setVal("fillColor",       "" TAB "" TAB "ColorEdit" TAB "mode>>int" TAB "GLab.styleObj" TAB %gid);
	%ar.setVal("fillColorHL",       "" TAB "" TAB "ColorEdit" TAB "mode>>int" TAB "GLab.styleObj" TAB %gid);
	%ar.setVal("fillColorNA",       "" TAB "" TAB "ColorEdit" TAB "mode>>int" TAB "GLab.styleObj" TAB %gid);
	%ar.setVal("fillColorSEL",       "" TAB "" TAB "ColorEdit" TAB "mode>>int" TAB "GLab.styleObj" TAB %gid);
	%ar.group[%gid++] = "Font Type and Color settings" TAB "Columns 0 130" ;
	%ar.setVal("fontType",     "" TAB "fontType" TAB "DropdownEdit" TAB "fieldList>>$LabFontData_List" TAB "GLab.styleObj" TAB %gid);
	//%ar.setVal("fontSize",     "" TAB "fontType" TAB "DropdownEdit" TAB "fieldList>>$LabFontData_List" TAB "GLab.styleObj" TAB %gid);
	%ar.setVal("fontColor",       "" TAB "fontColor (0)" TAB "ColorEdit" TAB "mode>>int" TAB "GLab.styleObj" TAB %gid);
	%ar.setVal("fontColorHL",       "" TAB "fontColorHL (1)" TAB "ColorEdit" TAB "mode>>int" TAB "GLab.styleObj" TAB %gid);
	%ar.setVal("fontColorNA",       "" TAB "fontColorNA (2)" TAB "ColorEdit" TAB "mode>>int" TAB "GLab.styleObj" TAB %gid);
	%ar.setVal("fontColorSEL",       "" TAB "fontColorSEL (3)" TAB "ColorEdit" TAB "mode>>int" TAB "GLab.styleObj" TAB %gid);
	%ar.setVal("fontColorLink",       "" TAB "fontColorLink (4)" TAB "ColorEdit" TAB "mode>>int" TAB "GLab.styleObj" TAB %gid);
	%ar.setVal("fontColorLinkHL",       "" TAB "fontColorLinkHL (5)" TAB "ColorEdit" TAB "mode>>int" TAB "GLab.styleObj" TAB %gid);
	%ar.group[%gid++] = "Style and layout settings" TAB "Columns 0 130";
	%ar.setVal("bitmap",     "" TAB "bitmap" TAB "FileSelect" TAB "file>>tlab/gui/styles/" TAB "GLab.styleObj" TAB %gid);
	
	buildParamsArray(%ar,false);
	Lab.profileStyleArray = %ar; ///arProfileStyleParam
}
//------------------------------------------------------------------------------

//==============================================================================
function GLab::editProfileStyle( %this,%styleObj ) {
	devLog("Edit style ",%styleObj);
	GLab.styleObj = %styleObj;
	%profile = %styleObj.internalName;
	syncParamArray(arProfileStyleParam);
}
//------------------------------------------------------------------------------
