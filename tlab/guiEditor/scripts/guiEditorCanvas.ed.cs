//-----------------------------------------------------------------------------
// Copyright (c) 2012 GarageGames, LLC
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to
// deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
// sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
// IN THE SOFTWARE.
//-----------------------------------------------------------------------------


//=============================================================================================
//    Event Handlers.
//=============================================================================================

//---------------------------------------------------------------------------------------------

function GuiEditCanvas::onAdd( %this ) {
	// %this.setWindowTitle("Torque Gui Editor");
	//%this.onCreateMenu();
}

//---------------------------------------------------------------------------------------------

function GuiEditCanvas::onRemove( %this ) {
	if( isObject( GuiEditorGui.menuGroup ) )
		GuiEditorGui.delete();

	// cleanup
	//%this.onDestroyMenu();
}


//---------------------------------------------------------------------------------------------
/// Called before onSleep when the canvas content is changed
function GuiEditor::move(%this,%x,%y) {
	// GuiEditor.moveSelection(%x SPC %y);
	GuiEditor.moveSelection(%x ,%y);
}

//---------------------------------------------------------------------------------------------

function GuiEditCanvas::onWindowClose(%this) {
	%this.quit();
}

//=============================================================================================
//    Menu Commands.
//=============================================================================================

//---------------------------------------------------------------------------------------------

function GuiEditCanvas::create( %this ) {
	GuiEditorNewGuiDialog.init( "NewGui", "GuiControl" );
	Canvas.pushDialog( GuiEditorNewGuiDialog );
}

//---------------------------------------------------------------------------------------------

function GuiEditCanvas::load( %this, %filename ) {
	%newRedefineBehavior = "replaceExisting";

	if( isDefined( "$GuiEditor::loadRedefineBehavior" ) ) {
		// This trick allows to choose different redefineBehaviors when loading
		// GUIs.  This is useful, for example, when loading GUIs that would lead to
		// problems when loading with their correct names because script behavior
		// would immediately attach.
		//
		// This allows to also edit the GUI editor's own GUI inside itself.
		%newRedefineBehavior = $GuiEditor::loadRedefineBehavior;
	}

	// Allow stomping objects while exec'ing the GUI file as we want to
	// pull the file's objects even if we have another version of the GUI
	// already loaded.
	%oldRedefineBehavior = $Con::redefineBehavior;
	$Con::redefineBehavior = %newRedefineBehavior;
	// Load up the gui.
	exec( %fileName );
	$Con::redefineBehavior = %oldRedefineBehavior;

	// The GUI file should have contained a GUIControl which should now be in the instant
	// group. And, it should be the only thing in the group.
	if( !isObject( %guiContent ) ) {
		LabMsgOk( getEngineName(),
					 "You have loaded a Gui file that was created before this version.  It has been loaded but you must open it manually from the content list dropdown");
		return 0;
	}

	GuiEditor.openForEditing( %guiContent );
	GuiEditorStatusBar.print( "Loaded '" @ %filename @ "'" );
}

//---------------------------------------------------------------------------------------------

function GuiEditCanvas::openInTorsion( %this ) {
	if( !GuiEditorContent.getCount() )
		return;

	%guiObject = GuiEditorContent.getObject( 0 );
	EditorOpenDeclarationInTorsion( %guiObject );
}

//---------------------------------------------------------------------------------------------

function GuiEditCanvas::open( %this ) {
	devLog("Open!!");
	%openFileName = GuiBuilder::getOpenName();

	if( %openFileName $= "" )
		return;

	// Make sure the file is valid.
	if ((!isFile(%openFileName)) && (!isFile(%openFileName @ ".dso")))
		return;

	%this.load( %openFileName );
}

//---------------------------------------------------------------------------------------------

