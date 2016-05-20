//==============================================================================
// TorqueLab GUI -> WidgetBuilder system
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
// Create quickly a set of GUI based on a template
//==============================================================================
function widgetBuild() {
	toggleEdDlg(LabWidgetBuilderDlg);
}
function GuiLab::initWidgetBuilder(%this) {
	LWBD_StyleMenu.clear();
	%selectedId = GuiLab.selectedWidgetStyle;

	foreach(%ctrl in WidgetCollectionGui) {
		if (%ctrl.internalName $= "")
			continue;

		if (!isObject(%selectedId))
			%selectedId = %ctrl.getId();

		LWBD_StyleMenu.add(%ctrl.internalName,%ctrl.getId());
		devLog("Style added:",%ctrl.internalName,"Id",%ctrl.getId());
	}

	LWBD_StyleMenu.setSelected(%selectedId);
}

function GuiLab::selectWidgetStyle(%this,%style) {
	loga("GuiLab::selectWidgetStyle(%this,%style)",%this,%style);
	LWBD_TypeMenu.clear();
	GuiLab.selectedWidgetStyle = %style;

	foreach(%ctrl in %style) {
		//if (%ctrl.internalName $= "")
		//  continue;
		%wordName = strreplace(%ctrl.getName(),"_"," ");
		devLog("CheckWord0:",getWord(%wordName,0));

		if (getWord(%wordName,0) !$= "wParams")
			continue;

		%typeName = removeWord(%wordName,0);

		if (%selectedId $= "")
			%selectedId = %ctrl.getId();

		LWBD_TypeMenu.add(%typeName,%ctrl.getId());
		devLog("Type added:",%typeName,"Id",%ctrl.getId());
	}

	LWBD_TypeMenu.setSelected(%selectedId);
}

function GuiLab::initWidgetStyle(%this,%type) {
}

function GuiLab::selectWidgetType(%this,%type) {
	loga("GuiLab::selectWidgetType(%this,%type)",%this,%type);
	GuiLab.selectedWidgetType = %type;
	%stack = %type-->widgets;
	LWBD_WidgetMenu.clear();

	foreach(%ctrl in %stack) {
		if (%ctrl.internalName $= "")
			continue;

		if (%selectedId $= "")
			%selectedId = %ctrl.getId();

		LWBD_WidgetMenu.add(%ctrl.internalName,%ctrl.getId());
		devLog("Widget added:",%ctrl.internalName,"Id",%ctrl.getId());
	}

	LWBD_WidgetMenu.setSelected(%selectedId);
}


function GuiLab::selectWidget(%this,%widget) {
	loga("GuiLab::selectWidget(%this,%widget)",%this,%widget);
	GuiLab.selectedWidget = %widget;
	%this.initWidgetSetup(%widget);
	LWBD_PreviewStack.clear();
	GuiLab.widgetPreview = cloneObject(%widget);
	LWBD_PreviewStack.add(GuiLab.widgetPreview);

	foreach(%ctrl in %widget)
		%this.checkWidgetSubCtrl(%ctrl);
}

function GuiLab::checkWidgetSubCtrl(%this,%ctrl) {
	loga("GuiLab::checkWidgetSubCtrl(%this,%ctrl)",%this,%ctrl);

	foreach(%subctrl in %ctrl)
		%this.checkWidgetSubCtrl(%subctrl);

	devLog("Widget contain class:",%ctrl.getClassName(),"Int",%ctrl.internalName);
}


function GuiLab::initWidgetSetup(%this,%widget) {
	loga("GuiLab::initWidgetSetup(%this,%widget)",%this,%widget);
	LWBD_WidgetSetupStack-->display.setText("");
	LWBD_WidgetSetupStack-->options.setText("");
	LWBD_WidgetSetupStack-->defaultData.setText("");
	LWBD_WidgetSetupStack-->linkData.setText("");
	%intName = %widget.internalName;
	%pData = newScriptObject("widgetBuildData");
	%pData.Setting = %field;
	%pData.Default = "";
	%pData.Title = "";
	%pData.Type = %intName;
	%pData.Options = "";
	%pData.syncObjs = "";
	%pData.srcData = "";
	%paramType = strreplace(%intName,"_"," ");
	%pData.Category = getWord(%paramType,0);
}

function GuiLab::addWidgetToCurrent(%this) {
	loga("GuiLab::addWidgetToCurrent(%this)",%this);
	%widget = GuiLab.widgetPreview;
	%selection =  GuiEditor.getSelection();

	while(isObject(%selection.getObject(%i))) {
		%control = %selection.getObject(%i);
		%newCtrl = cloneObject(%widget);
		%control.add(%newCtrl);
	}

	delObj(GuiLab.widgetPreview);
}

function GuiLab::updateWidgetOption(%this,%type) {
	loga("GuiLab::updateWidgetOption(%this,%type)",%this,%type);
	%data = LWBD_WidgetSetupStack.findObjectByInternalName(%type,true);
	%value = %data.getText();

	switch$(%type) {
	case "display":
		widgetBuildData.title = %value;

	case "options":
		widgetBuildData.Options = %value;

	case "default":
		widgetBuildData.Default = %value;

	case "linkData":
		widgetBuildData.syncObjs = %value;
	}

	devLog("updateWidgetOption type:",%type,"Value:",%value);
}
