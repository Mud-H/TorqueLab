//==============================================================================
// TorqueLab -> Universal Lab Popup Message Boxes
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$LabMsgMargin["Top"] = 36;
$LabMsgMargin["Bottom"] = 37;

$LabMsgButtons["Ok"] = "Ok";
$LabMsgButtons["OkCancel"] = "Ok" TAB "Cancel";
$LabMsgButtons["YesNo"] = "Yes" TAB "No";
$LabMsgButtons["YesNoCancel"] = "Yes" TAB "No" TAB "Cancel";
$LabMsgButtonsCoord["Ok"] = "111 75 80 24";
$LabMsgButtonsCoord["OkCancel"] = "66 68 80 24" TAB "156 68 80 24";
$LabMsgButtonsCoord["YesNo"] = "66 68 80 24" TAB "156 68 80 24";
$LabMsgButtonsCoord["YesNoCancel"] = "11 75 80 24" TAB "96 75 80 24" TAB "210 75 80 24";
//==============================================================================
function LabMsgBoxesGui::onWake( %this ) {
}
//------------------------------------------------------------------------------
//==============================================================================
function LabMsgBoxesGui::showWindow( %this,%type,%title,%message,%callback1,%callback2,%callback3 ) {
	%buttonData = $LabMsgButtons[%type];
	%buttonCoord = $LabMsgButtonsCoord[%type];
	%buttonCount = getFieldCount(%buttonData);
	// LabMsgBoxesGui.callOnChildrenNoRecurse("setVisible",false);
	pushDlg(LabMsgBoxesGui);
	%dlg = LabMsgBoxesGui-->dialogBase;
	%dlg.setVisible(true);
	%dlg.text = %title;
	%dlg-->messageArea.setText(%message);
	%dlg-->messageArea.forceReflow();
	%msgExtent = %dlg-->messageArea.getExtent();
	//%scrollExtent.y += 4;
	%dlg-->scroll.setExtent(%dlg-->scroll.extent.x, %msgExtent.y+18);
	%dlgextenty = $LabMsgMargin["Top"] + %msgExtent.y + $LabMsgMargin["Bottom"];
	%dlg.resize(%dlg.position.x,%dlg.position.y,%dlg.extent.x,%dlgextenty);

	for(%i=0; %i < 3; %i++) {
		%text = getField(%buttonData,%i);
		%coord = getField(%buttonCoord,%i);
		%callBackCmd =  %callback[%i+1];
		LabMsgBoxesGui.callbacks["button"@%i+1] = %callBackCmd;
		%button = %dlg.findObjectByInternalName("button"@%i+1,true);
		%button.text = %text;

		if (%coord $= "") {
			hide(%button);
		} else {
			show(%button);
			%button.position.x = getWord(%coord,0);
			%button.extent = getWord(%coord,2) SPC getWord(%coord,3);
			//%button.position.y = %dlg.extent.y - 12 - %button.extent.y;
		}
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function LabMsgBoxesGui::hideWindow( %this ) {
	popDlg(LabMsgBoxesGui);
}
//------------------------------------------------------------------------------
//==============================================================================
function LabMsgBoxesGui::isClosable( %this,%closable ) {
	LabMsgBoxesGui-->dialogBase.canClose = %closable;
}
//------------------------------------------------------------------------------
//==============================================================================
function LabMsgCallback( %control ) {
	%intName = %control.internalName;
	%callback = LabMsgBoxesGui.callbacks[%intName];

	if (%callback !$="")
		eval(%callback);

	LabMsgBoxesGui.hideWindow();
}
//------------------------------------------------------------------------------

//==============================================================================
// LabMsg Dialogs Creation Functions
//==============================================================================

//==============================================================================
// Basic Popup dialog with no button that can be close after a delay
function LabMsg( %title,%message,%delay,%noClose ) {
	if ( %noClose && %delay $= "") {
		warnLog("Can't make dialog unclosable because it need at least a delayed closing functions.");
		%noClose = false;
	}

	LabMsgBoxesGui.isClosable(!%noClose);
	LabMsgBoxesGui.showWindow("Popup",%title,%message);

	//Schedule a auto closing function
	if (%delay !$= "")
		LabMsgBoxesGui.schedule(%delay, "hideWindow");
}
//------------------------------------------------------------------------------

//==============================================================================
// Window with an OK button with configurable callback
function LabMsgOk(%title,%message,%callBack,%noClose ) {
	LabMsgBoxesGui.isClosable(!%noClose);
	LabMsgBoxesGui.showWindow("Ok",%title,%message,%callBack);
}
//------------------------------------------------------------------------------

//==============================================================================
// Window with a Yes and No buttons with configurable callback
function LabMsgYesNo(%title,%message,%callBackYes,%callbackNo,%noClose ) {
	LabMsgBoxesGui.isClosable(!%noClose);
	LabMsgBoxesGui.showWindow("YesNo",%title,%message,%callBackYes,%callbackNo);
}
//------------------------------------------------------------------------------

//==============================================================================
// Window with a Ok and Cancel buttons with configurable callback
function LabMsgOkCancel(%title,%message,%callBackOk,%callbackCancel,%noClose ) {
	LabMsgBoxesGui.isClosable(!%noClose);
	LabMsgBoxesGui.showWindow("OkCancel",%title,%message,%callBackOk,%callbackCancel);
}
//------------------------------------------------------------------------------
//==============================================================================
// Window with a Yes ,No and Cancel buttons with configurable callback
function LabMsgYesNoCancel(%title,%message,%callBackYes,%callbackNo,%callbackCancel,%noClose ) {
	LabMsgBoxesGui.isClosable(!%noClose);
	LabMsgBoxesGui.showWindow("YesNoCancel",%title,%message,%callBackYes,%callbackNo,%callbackCancel);
}
//------------------------------------------------------------------------------