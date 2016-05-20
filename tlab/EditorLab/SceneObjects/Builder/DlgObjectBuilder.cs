
function ObjectBuilderGui::init(%this) {
	%this.baseOffsetX       = 5;
	%this.baseOffsetY       = 5;
	%this.defaultObjectName = "";
	%this.defaultFieldStep  = 22;
	%this.columnOffset      = 110;
	%this.fieldNameExtent   = "105 18";
	%this.textEditExtent    = "122 18";
	%this.checkBoxExtent    = "13 18";
	%this.popupMenuExtent   = "122 18";
	%this.fileButtonExtent  = "122 18";
	%this.matButtonExtent   = "17 18";
	//
	%this.numControls       = 0;
	%this.lastPath          = "";
	%this.reset();
}

function ObjectBuilderGui::reset(%this) {
	%this.objectGroup       = "";
	%this.curXPos           = %this.baseOffsetX;
	%this.curYPos           = %this.baseOffsetY;
	%this.createFunction    = "";
	%this.createCallback    = "";
	%this.currentControl    = 0;
	//
	OBObjectName.setValue(%this.defaultObjectName);
	//
	%this.newObject         = 0;
	%this.objectClassName   = "";
	%this.numFields         = 0;

	//
	for(%i = 0; %i < %this.numControls; %i++) {
		%this.textControls[%i].delete();
		%this.controls[%i].delete();
	}

	%this.numControls = 0;
}

//------------------------------------------------------------------------------

function ObjectBuilderGui::createFileType(%this, %index) {
	if(%index >= %this.numFields || %this.field[%index, name] $= "") {
		error("ObjectBuilderGui::createFileType: invalid field");
		return;
	}

	//
	if(%this.field[%index, text] $= "")
		%name = %this.field[%index, name];
	else
		%name = %this.field[%index, text];

	//
	%this.textControls[%this.numControls] = new GuiTextCtrl() {
		profile = "ToolsGuiTextRightProfile";
		text = %name;
		extent = %this.fieldNameExtent;
		position = %this.curXPos @ " " @ %this.curYPos;
		modal = "1";
	};
	//
	%this.controls[%this.numControls] = new GuiButtonCtrl() {
		HorizSizing = "width";
		profile = "ToolsButtonProfile";
		extent = %this.fileButtonExtent;
		position = %this.curXPos + %this.columnOffset @ " " @ %this.curYPos;
		modal = "1";
		command = %this @ ".getFileName(" @ %index @ ");";
	};
	%val = %this.field[%index, value];
	%this.controls[%this.numControls].setValue(fileBase(%val) @ fileExt(%val));
	%this.numControls++;
	%this.curYPos += %this.defaultFieldStep;
}

function ObjectBuilderGui::getFileName(%this, %index) {
	if(%index >= %this.numFields || %this.field[%index, name] $= "") {
		error("ObjectBuilderGui::getFileName: invalid field");
		return;
	}

	%val = %this.field[%index, ext];
	//%path = filePath(%val);
	//%ext = fileExt(%val);
	%this.currentControl = %index;
	getLoadFilename( %val @ "|" @ %val, %this @ ".gotFileName", %this.lastPath );
}

function ObjectBuilderGui::gotFileName(%this, %name) {
	%index = %this.currentControl;
	%name = makeRelativePath(%name,getWorkingDirectory());
	%this.field[%index, value] = %name;
	%this.controls[%this.currentControl].setText(fileBase(%name) @ fileExt(%name));
	%this.lastPath = %name;
	// This doesn't work for button controls as getValue returns their state!
	//%this.controls[%this.currentControl].setValue(%name);
}

//------------------------------------------------------------------------------

function ObjectBuilderGui::createMaterialNameType(%this, %index) {
	if(%index >= %this.numFields || %this.field[%index, name] $= "") {
		error("ObjectBuilderGui::createMaterialNameType: invalid field");
		return;
	}

	//
	if(%this.field[%index, text] $= "")
		%name = %this.field[%index, name];
	else
		%name = %this.field[%index, text];

	//
	%this.textControls[%this.numControls] = new GuiTextCtrl() {
		profile = "ToolsGuiTextRightProfile";
		text = %name;
		extent = %this.fieldNameExtent;
		position = %this.curXPos @ " " @ %this.curYPos;
		modal = "1";
	};
	//
	%this.controls[%this.numControls] = new GuiControl() {
		HorizSizing = "width";
		profile = "ToolsDefaultProfile";
		extent = %this.textEditExtent;
		position = %this.curXPos + %this.columnOffset @ " " @ %this.curYPos;
		modal = "1";
	};
	%text = new GuiTextEditCtrl() {
		class = ObjectBuilderGuiTextEditCtrl;
		internalName = "MatText";
		HorizSizing = "width";
		profile = "ToolsTextEdit";
		extent = getWord(%this.textEditExtent,0) - getWord(%this.matButtonExtent,0) - 2 @ " " @ getWord(%this.textEditExtent,1);
		text = %this.field[%index, value];
		position = "0 0";
		modal = "1";
	};
	%this.controls[%this.numControls].addGuiControl(%text);
	%button = new GuiBitmapButtonCtrl() {
		internalName = "MatButton";
		HorizSizing = "left";
		profile = "ToolsButtonProfile";
		extent = %this.matButtonExtent;
		position = getWord(%this.textEditExtent,0) - getWord(%this.matButtonExtent,0) @ " 0";
		modal = "1";
		command = %this @ ".getMaterialName(" @ %index @ ");";
	};
	%button.setBitmap("tlab/materialEditor/assets/change-material-btn");
	%this.controls[%this.numControls].addGuiControl(%button);
	//%val = %this.field[%index, value];
	//%this.controls[%this.numControls].setValue(%val);
	//%this.controls[%this.numControls].setBitmap("tlab/materialEditor/assets/change-material-btn");
	%this.numControls++;
	%this.curYPos += %this.defaultFieldStep;
}

function ObjectBuilderGui::getMaterialName(%this, %index) {
	if(%index >= %this.numFields || %this.field[%index, name] $= "") {
		error("ObjectBuilderGui::getMaterialName: invalid field");
		return;
	}

	%this.currentControl = %index;
	MatBrowser.showDialog(%this @ ".gotMaterialName", "name");
}

function ObjectBuilderGui::gotMaterialName(%this, %name) {
	%index = %this.currentControl;
	%this.field[%index, value] = %name;
	%this.controls[%index]-->MatText.setText(%name);
}

//------------------------------------------------------------------------------

function ObjectBuilderGui::createDataBlockType(%this, %index) {
	if(%index >= %this.numFields || %this.field[%index, name] $= "") {
		error("ObjectBuilderGui::createDataBlockType: invalid field");
		return;
	}

	//
	if(%this.field[%index, text] $= "")
		%name = %this.field[%index, name];
	else
		%name = %this.field[%index, text];

	//
	%this.textControls[%this.numControls] = new GuiTextCtrl() {
		profile = "ToolsGuiTextRightProfile";
		text = %name;
		extent = %this.fieldNameExtent;
		position = %this.curXPos @ " " @ %this.curYPos;
		modal = "1";
	};
	//
	%this.controls[%this.numControls] = new GuiPopupMenuCtrl() {
		HorizSizing = "width";
		profile = "ToolsDropdownProfile";
		extent = %this.popupMenuExtent;
		position = %this.curXPos + %this.columnOffset @ " " @ %this.curYPos;
		modal = "1";
		maxPopupHeight = "200";
	};
	%classname = getWord(%this.field[%index, value], 0);
	%classname_alt = getWord(%this.field[%index, value], 1);
	%this.controls[%this.numControls].add("", -1);

	// add the datablocks
	for(%i = 0; %i < DataBlockGroup.getCount(); %i++) {
		%obj = DataBlockGroup.getObject(%i);

		if( isMemberOfClass( %obj.getClassName(), %classname ) || isMemberOfClass ( %obj.getClassName(), %classname_alt ) )
			%this.controls[%this.numControls].add(%obj.getName(), %i);
	}

	%this.controls[%this.numControls].setValue(getWord(%this.field[%index, value], 1));
	%this.numControls++;
	%this.curYPos += %this.defaultFieldStep;
}

function ObjectBuilderGui::createBoolType(%this, %index) {
	if(%index >= %this.numFields || %this.field[%index, name] $= "") {
		error("ObjectBuilderGui::createBoolType: invalid field");
		return;
	}

	//
	if(%this.field[%index, value] $= "")
		%value = 0;
	else
		%value = %this.field[%index, value];

	//
	if(%this.field[%index, text] $= "")
		%name = %this.field[%index, name];
	else
		%name = %this.field[%index, text];

	//
	%this.textControls[%this.numControls] = new GuiTextCtrl() {
		profile = "ToolsGuiTextRightProfile";
		text = %name;
		extent = %this.fieldNameExtent;
		position = %this.curXPos @ " " @ %this.curYPos;
		modal = "1";
	};
	//
	%this.controls[%this.numControls] = new GuiCheckBoxCtrl() {
		profile = "ToolsCheckBoxProfile";
		extent = %this.checkBoxExtent;
		position = %this.curXPos + %this.columnOffset @ " " @ %this.curYPos;
		modal = "1";
	};
	%this.controls[%this.numControls].setValue(%value);
	%this.numControls++;
	%this.curYPos += %this.defaultFieldStep;
}

function ObjectBuilderGuiTextEditCtrl::onGainFirstResponder(%this) {
	%this.selectAllText();
}

function ObjectBuilderGui::createStringType(%this, %index) {
	if(%index >= %this.numFields || %this.field[%index, name] $= "") {
		error("ObjectBuilderGui::createStringType: invalid field");
		return;
	}

	//
	if(%this.field[%index, text] $= "")
		%name = %this.field[%index, name];
	else
		%name = %this.field[%index, text];

	//
	%this.textControls[%this.numControls] = new GuiTextCtrl() {
		profile = "ToolsGuiTextRightProfile";
		text = %name;
		extent = %this.fieldNameExtent;
		position = %this.curXPos @ " " @ %this.curYPos;
		modal = "1";
	};
	//
	%this.controls[%this.numControls] = new GuiTextEditCtrl() {
		class = ObjectBuilderGuiTextEditCtrl;
		HorizSizing = "width";
		profile = "ToolsTextEdit";
		extent = %this.textEditExtent;
		text = %this.field[%index, value];
		position = %this.curXPos + %this.columnOffset @ " " @ %this.curYPos;
		modal = "1";
	};
	%this.numControls++;
	%this.curYPos += %this.defaultFieldStep;
}

//------------------------------------------------------------------------------

function ObjectBuilderGui::adjustSizes(%this) {
	if(%this.numControls == 0)
		%this.curYPos = 0;

	OBTargetWindow.extent = getWord(OBTargetWindow.extent, 0) SPC %this.curYPos + 88;
	OBContentWindow.extent = getWord(OBContentWindow.extent, 0) SPC %this.curYPos;
	OBOKButton.position = getWord(OBOKButton.position, 0) SPC %this.curYPos + 57;
	OBCancelButton.position = getWord(OBCancelButton.position, 0) SPC %this.curYPos + 57;
}

function ObjectBuilderGui::process(%this) {
	if(%this.objectClassName $= "") {
		error("ObjectBuilderGui::process: classname is not specified");
		return;
	}

	OBTargetWindow.text = "Create Object: " @ %this.objectClassName;
	%name = getUniqueName(%this.objectClassName@"_1");
	OBObjectName.setText(%name);

	//
	for(%i = 0; %i < %this.numFields; %i++) {
		switch$(%this.field[%i, type]) {
		case "TypeBool":
			%this.createBoolType(%i);

		case "TypeDataBlock":
			%this.createDataBlockType(%i);

		case "TypeFile":
			%this.createFileType(%i);

		case "TypeMaterialName":
			%this.createMaterialNameType(%i);

		default:
			%this.createStringType(%i);
		}
	}

	// add the controls
	for(%i = 0; %i < %this.numControls; %i++) {
		OBContentWindow.add(%this.textControls[%i]);
		OBContentWindow.add(%this.controls[%i]);
	}

	//
	%this.adjustSizes();
	//
	Canvas.pushDialog(%this);
}

function ObjectBuilderGui::processNewObject(%this, %obj) {
	if ( %this.createCallback !$= "" )
		eval( %this.createCallback );

	// Skip out if nothing was created.
	if ( !isObject( %obj ) )
		return;

	// Add the object to the group.
	if( %this.objectGroup !$= "" )
		%this.objectGroup.add( %obj );
	else
		Scene.getActiveSimGroup().add( %obj );

	// If we were given a callback to call after the
	// object has been created, do so now.  Also clear
	// the callback to make sure it's valid only for
	// a single invocation.
	%callback = %this.newObjectCallback;
	%this.newObjectCallback = "";

	if( %callback !$= "" )
		eval( %callback @ "( " @ %obj @ " );" );
}

function ObjectBuilderGui::onOK(%this) {
	// Error out if the given object name is not valid or not unique.
	%objectName = OBObjectName.getValue();

	if( !Lab::validateObjectName( %objectName, false ))
		return;

	// get current values
	for(%i = 0; %i < %this.numControls; %i++) {
		// uses button type where getValue returns button state!
		if (%this.field[%i, type] $= "TypeFile") {
			if (strchr(%this.field[%i, value],"*") !$= "")
				%this.field[%i, value] = "";

			continue;
		}

		if (%this.field[%i, type] $= "TypeMaterialName") {
			%this.field[%i, value] = %this.controls[%i]-->MatText.getValue();
			continue;
		}

		%this.field[%i, value] = %this.controls[%i].getValue();
	}

	// If we have a special creation function then
	// let it do the construction.
	if ( %this.createFunction !$= "" )
		eval( %this.createFunction );
	else {
		// Else we use the memento.
		%memento = %this.buildMemento();
		eval( %memento );
	}

	if(%this.newObject != 0)
		%this.processNewObject(%this.newObject);

	%this.reset();
	Canvas.popDialog(%this);
}

function ObjectBuilderGui::onCancel(%this) {
	%this.reset();
	Canvas.popDialog(%this);
}

function ObjectBuilderGui::addField(%this, %name, %type, %text, %value, %ext) {
	%this.field[%this.numFields, name] = %name;
	%this.field[%this.numFields, type] = %type;
	%this.field[%this.numFields, text] = %text;
	%this.field[%this.numFields, value] = %value;
	%this.field[%this.numFields, ext] = %ext;
	%this.numFields++;
}
// *-OFF
function ObjectBuilderGui::buildMemento(%this) {
	// Build the object into a string.
	%this.memento = %this @ ".newObject = new " @ %this.objectClassName @ "(" @ OBObjectName.getValue() @ ") { ";

	for( %i = 0; %i < %this.numFields; %i++ )
		%this.memento = %this.memento @ %this.field[%i, name] @ " = \"" @ %this.field[%i, value] @ "\"; ";

	%this.memento = %this.memento @ "};";
	return %this.memento;
}
// *-ON
