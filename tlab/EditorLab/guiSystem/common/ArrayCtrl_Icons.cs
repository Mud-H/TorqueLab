//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
function ArrayIconCtrl::onClick(%this) {
	logd("ArrayCtrlIcon OnClick",%this);
}
//------------------------------------------------------------------------------
function arrayIconCompare( %a, %b ) {
	if ( %a.class $= "ArrayIconCtrl" )
		if ( %b.class !$= "ArrayIconCtrl" )
			return -1;

	if ( %b.class $= "ArrayIconCtrl" )
		if ( %a.class !$= "ArrayIconCtrl" )
			return 1;

	%result = stricmp( %a.text, %b.text );
	return %result;
}

//==============================================================================
function Lab::createArrayIcon( %this,%srcObj ) {
   if (!isObject(%srcObj))
      %srcObj = %this.getArrayIconSrc();
	%ctrl = cloneObject(%srcObj);
	%ctrl.profile = "ToolsButtonArray";
	%ctrl.buttonType = "radioButton";
	%ctrl.groupNum = "-1";
	%ctrl.superClass = "ArrayIconCtrl";
	return %ctrl;
}
//------------------------------------------------------------------------------
function Lab::getArrayIconSrc( %this ) {
   %iconSrc =  new GuiIconButtonCtrl() {
                  buttonMargin = "4 4";
                  iconBitmap = "tlab/art/icons/24-assets/folder_open.png";
                  iconLocation = "Left";
                  sizeIconToButton = "0";
                  makeIconSquare = "0";
                  textLocation = "Left";
                  textMargin = "36";
                  autoSize = "0";                 
                  buttonType = "RadioButton";
                  useMouseEvents = "1";
                  position = "0 0";
                  extent = "274 28";
                  minExtent = "8 2";
                  horizSizing = "right";
                  vertSizing = "bottom";
                  profile = "ToolsButtonArray"; 
                  tooltipProfile = "GuiToolTipProfile";
                  
               };
   return %iconSrc;
}
//------------------------------------------------------------------------------
//==============================================================================
//FileBrowser.addFolderUpIcon
function Lab::addFolderUpIcon( %this,%scriptObject ) {
   %arrayCtrl = %scriptObject.arrayCtrl;
   %objName = %scriptObject.getName();
	%ctrl = %this.createArrayIcon();
	%ctrl.command = %objName@".navigateUp();";
	%ctrl.altCommand = %objName@".navigateUp();";
	%ctrl.iconBitmap = "tlab/art/icons/24-assets/folder_up.png";
	%ctrl.text = "...";
	%ctrl.tooltip = "Go to parent folder";
	%ctrl.buttonMargin = "8 1";
	%ctrl.sizeIconToButton = true;
	%ctrl.makeIconSquare = true;
	//%ctrl.class = "CreatorFolderIconBtn";
	%ctrl.buttonType = "PushButton";
	%arrayCtrl.addGuiControl( %ctrl );
	%arrayCtrl.bringToFront(%ctrl);
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::addFolderIcon( %this, %scriptObject,%text ) {
   %arrayCtrl = %scriptObject.arrayCtrl;
   if (!isObject(%arrayCtrl)||!isObject(%scriptObject))
   {
      warnLog("Trying to add a folder icon in an invalid array:",%scriptObject);
      return;
   }
   %objName = %scriptObject.getName();
	%ctrl = %this.createArrayIcon();
	%ctrl.command = %objName@".iconFolderAlt($ThisControl);";
	%ctrl.altCommand = %objName@".iconFolderAlt($ThisControl);";
	%ctrl.iconBitmap = "tlab/art/icons/24-assets/folder_open.png";
	%ctrl.text = %text;
	%ctrl.tooltip = %text;
	//%ctrl.superClass = "CreatorFolderIconBtn";
	%ctrl.buttonType = "radioButton";
	%ctrl.groupNum = "-1";
	%ctrl.buttonMargin = "6 0";
	%ctrl.sizeIconToButton = true;
	%ctrl.makeIconSquare = true;
	%arrayCtrl.addGuiControl( %ctrl );
}
//------------------------------------------------------------------------------

//==============================================================================
function GuiDynamicCtrlArrayControl::findIconCtrl( %this, %name ) {
	for ( %i = 0; %i < %this.getCount(); %i++ ) {
		%ctrl = %this.getObject( %i );

		if ( %ctrl.text $= %name )
			return %ctrl;
	}

	return -1;
}
//------------------------------------------------------------------------------
