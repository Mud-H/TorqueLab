//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================


//=============================================================================================
//    Activation.
//=============================================================================================


function Lab::cloneGuiEditor(%this) {
	if ($InGuiEditor) {
		%loadEditor = true;
		Lab.lastGuiEditSource = GuiEditor.lastContent;
		GuiEditCanvas.quit();
	}

	delObj(CloneEditorGui);
	%cloneEditor = GuiEditorGui.deepClone();
	%cloneEditor.setName("CloneEditorGui");
	%file = "tlab/guiEditor/gui/CloneEditorGui.gui";
	%fileWrite = getFileWriteObj(%file);
	%fileWrite.writeLine("//--- OBJECT WRITE BEGIN ---");
	%fileWrite.writeObject(%cloneEditor, "%guiContent = ");
	%fileWrite.writeLine("//--- OBJECT WRITE END ---");
	closeFileObj(%fileWrite);
	GuiEditorContentList.init();

	if (%loadEditor)
		schedule(500,"","GuiEdit");
}

function Lab::convertClonedGuiEditor(%this) {
	if ($InGuiEditor)
		GuiEd.closeEditor();

	%fileWrite = getFileWriteObj("tlab/guiEditor/gui/backup/guiEditor.tmp.gui");
	%fileWrite.writeLine("//--- OBJECT WRITE BEGIN ---");
	%fileWrite.writeObject(GuiEditorGui, "%guiContent = ");
	%fileWrite.writeLine("//--- OBJECT WRITE END ---");
	closeFileObj(%fileWrite);
	delObj(CloneEditorGui);
	exec("tlab/guiEditor/gui/CloneEditorGui.gui");
	delObj(GuiEditorGui);
	%guiEditor = CloneEditorGui.deepClone();
	%guiEditor.setName("GuiEditorGui");
	%fileWrite = getFileWriteObj("tlab/guiEditor/gui/guiEditor.ed.gui");
	%fileWrite.writeLine("//--- OBJECT WRITE BEGIN ---");
	%fileWrite.writeObject(%guiEditor, "%guiContent = ");
	%fileWrite.writeLine("//--- OBJECT WRITE END ---");
	closeFileObj(%fileWrite);
	%fileRead = getFileReadObj("tlab/guiEditor/gui/guiEditor.ed.gui");

	while( !%fileRead.isEOF() ) {
		%line = %fileRead.readLine();

		//new GuiControl(GuiEditorContent2) {
		if (strFind(%line,"(GuiEd") && !strFind(%line,"(GuiEditorGui")) {
			%linefields = strreplace(%line,"(","\t");
			%linefields = strreplace(%linefields,")","\t");
			%name = trim(getField(%linefields,1));
			devLog("Name found:",%name,"In Fields",%linefields);
			%len = strlen(%name);
			%defaultName = getSubStr(%name,0,%len-1);
			devLog("DefaultName set to:",%defaultName);
			%newLine = strreplace(%line,%name,%defaultName);
			devLog("Line changed from:",%line,"to:",%newLine);
			%line = %newLine;
		}

		%finalLine[%id++] = %line;
	}

	closeFileObj(%fileRead);
	%fileWrite = getFileWriteObj("tlab/guiEditor/gui/guiEditor.ed.gui");

	for(%i = 1; %i <=%id; %i++) {
		%fileWrite.writeLine(%finalLine[%i]);
	}

	closeFileObj(%fileWrite);
	delObj(GuiEditorGui);
	exec("tlab/guiEditor/gui/guiEditor.ed.gui");
	addGuiEditorCtrl();
	Lab.initMenu(GuiEdMenu);

	if (%loadEditor)
		GuiEd.schedule(500,"launchEditor",true);
}

function addGuiEditorCtrl(%reset) {
	if (%reset)
		delObj(GuiEditor);

	if (!isObject(GuiEditor)) {
		new GuiEditCtrl(GuiEditor) {
			snapToControls = "1";
			snapToGuides = "1";
			snapToCanvas = "1";
			snapToEdges = "1";
			snapToCenters = "1";
			snapSensitivity = "2";
			fullBoxSelection = "0";
			drawBorderLines = "1";
			drawGuides = "1";
			position = "0 0";
			extent = "640 480";
			minExtent = "8 2";
			horizSizing = "width";
			vertSizing = "height";
			profile = "ToolsTextEdit";
			visible = "1";
			active = "1";
			tooltipProfile = "ToolsToolTipProfile";
			hovertime = "1000";
			isContainer = "0";
			canSave = "1";
			canSaveDynamicFields = "0";
		};
	}

	GuiEditorRegion.add(GuiEditor);
}