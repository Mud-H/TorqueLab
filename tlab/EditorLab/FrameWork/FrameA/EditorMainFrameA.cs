
function EditorFrameContent::init(%this) {
   %this.setLeftCol($Cfg_UI_Frame_SideFrameWidth);
	%this.setRightCol($Cfg_UI_Frame_ToolFrameWidth);
	
	$EditorFrameContentInit = true;

}

//==============================================================================
// Make sure all GUIs are fine once the editor is launched

function EditorFrameContent::onWake(%this) {
	EditorFrameContent.frameMinExtent(0,$LabCfg_Layout_LeftMinWidth,100);
	EditorFrameContent.frameMinExtent(2,$LabCfg_Layout_RightMinWidth,100);	
	if (!$EditorFrameContentInit)
	   %this.schedule(50,"init");
}

//------------------------------------------------------------------------------
function EditorFrameContent::onSleep(%this) {
	if (EditorGui-->SideBarContainer.isOpen) {
		$SideBar_CurentColumns = EditorFrameContent.columns;
		EditorFrameContent.lastColumns = EditorFrameContent.columns;
	}
}
//==============================================================================

//==============================================================================
// Make sure all GUIs are fine once the editor is launched
function EditorFrameContent::onResized(%this) {
	//EditorFrameMain.minExtent = %this.getExtent().x - 220 SPC "12";
	//%this.checkCol();
	if(isObject(FileBrowser))
		FileBrowser.onResized();
	
	if(isObject(ObjectCreator))
		ObjectCreator.onResized();
		
	if(isObject(SideBarVIS))
		SideBarVIS.onResized();
	
	if (isObject(ECamViewGui))
		ECamViewGui.checkArea();
	if (Lab.currentEditor.isMethod("onLayoutResized"))
	   Lab.currentEditor.onLayoutResized();
   
   
   %colPosZ = %this.columns.z;
   if (%colPosZ !$= ""){      
      %colWidthZ = 	%this.extent.x - %colPosZ;	
       %this.rightColumnSize = %colWidthZ;     
   }
}
//==============================================================================
// Initialize the Editor Frames
//==============================================================================

//==============================================================================
function Lab::setupPluginEditorFrame(%this) {	
	
	if ($LabCfg_Layout_CurrentRightWidth $= "")
	  EditorFrameContent.setRightCol();
	  
	Lab.checkPluginTools();
	
	%this.lockEditorFrameContent(false);
}
//------------------------------------------------------------------------------
//==============================================================================
// EditorFrame - Left-Right Column
//==============================================================================

//------------------------------------------------------------------------------
function EditorFrameContent::setLeftCol(%this,%colWidth,%ignoreMinWidth) {
   if (%colWidth $= "")
      %colWidth = $Cfg_UI_Frame_SideFrameWidth;
      
   if ((%colWidth<= $LabCfg_Layout_LeftMinWidth || %colWidth $= "") && !%ignoreMinWidth)
		%colWidth = $LabCfg_Layout_LeftMinWidth;
		
	%this.columns = setWord(%this.columns,1,%colWidth);
	%this.updateSizes();
	$LabCfg_Layout_CurrentLeftWidth = %colWidth;
	EditorFrameContent.leftColumnSize = %colWidth;
}

//==============================================================================

function EditorFrameContent::setRightCol(%this,%colWidth,%ignoreMinWidth) {
   if (%colWidth $= "")
      %colWidth = $Cfg_UI_Frame_ToolFrameWidth;
      
   if ((%colWidth<= $LabCfg_Layout_RightMinWidth || %colWidth $= "") && !%ignoreMinWidth)
		%colWidth = $LabCfg_Layout_RightMinWidth;
	%colStart = 	%this.extent.x - %colWidth;
	%this.columns = setWord(%this.columns,2,%colStart);
	%this.updateSizes();
	$LabCfg_Layout_CurrentRightWidth = %colWidth;
	 EditorFrameContent.rightColumnSize = %colWidth;
}

//==============================================================================
// FROM MAIN OLD FRAME
//==============================================================================


//==============================================================================
function Lab::lockEditorFrameContent(%this,%locked) {
   $LabCfg_EditorUI_ToolFrameLocked = %locked;
   EditorFrameContent.borderEnable = %locked ? "alwaysOff" : "alwaysOn";
   EditorFrameContent.borderMovable = %locked ? "alwaysOff" : "alwaysOn";
	if (%locked) {	
		EditorFrameContent.borderWidth = "0";	
	} else {
	  EditorFrameContent.borderWidth = $LabGui_EditorFrameMain_BorderWidth; 
	}	
	EditorFrameContent.updateSizes();	
}
//------------------------------------------------------------------------------
/*
//-----------------------------------------------------------------------------
DefineEngineMethod( GuiFrameSetCtrl, frameBorder, void, ( S32 index, const char* state ), ( "dynamic" ),
   "Override the <i>borderEnable</i> setting for this frame.\n\n"
   "@param index  Index of the frame to modify\n"
   "@param state  New borderEnable state: \"on\", \"off\" or \"dynamic\"\n" )
{
   object->frameBorderEnable( index, state );
}

DefineEngineMethod( GuiFrameSetCtrl, frameMovable, void, ( S32 index, const char* state ), ( "dynamic" ),
   "Override the <i>borderMovable</i> setting for this frame.\n\n"
   "@param index  Index of the frame to modify\n"
   "@param state  New borderEnable state: \"on\", \"off\" or \"dynamic\"\n" )
{
   object->frameBorderMovable( index, state );
}

DefineEngineMethod( GuiFrameSetCtrl, frameMinExtent, void, ( S32 index, S32 width, S32 height ),,
   "Set the minimum width and height for the frame. It will not be possible "
   "for the user to resize the frame smaller than this.\n\n"
   "@param index  Index of the frame to modify\n"
   "@param width  Minimum width in pixels\n"
   "@param height Minimum height in pixels\n" )
{
   Point2I extent( getMax( 0, width ), getMax( 0, height ) );
   object->frameMinExtent( index, extent);
}

DefineEngineMethod( GuiFrameSetCtrl, framePadding, void, ( S32 index, RectSpacingI padding ),,
   "Set the padding for this frame. Padding introduces blank space on the inside "
   "edge of the frame.\n\n"
   "@param index     Index of the frame to modify\n"
   "@param padding   Frame top, bottom, left, and right padding\n" )
{
   object->framePadding( index, padding);
}

DefineEngineMethod( GuiFrameSetCtrl, getFramePadding, RectSpacingI, ( S32 index ),,
   "Get the padding for this frame.\n\n"
   "@param index     Index of the frame to query\n" )
{
   return object->getFramePadding( index );
}

DefineEngineMethod( GuiFrameSetCtrl, addColumn, void, (),,
   "Add a new column.\n\n" )
{
   Vector<S32> * columns = object->columnOffsets();
   columns->push_back(0);
   object->balanceFrames();
}

DefineEngineMethod( GuiFrameSetCtrl, addRow, void, (),,
   "Add a new row.\n\n" )
{
   Vector<S32> * rows = object->rowOffsets();
   rows->push_back(0);
   object->balanceFrames();
}

DefineEngineMethod( GuiFrameSetCtrl, removeColumn, void, (),,
   "Remove the last (rightmost) column.\n\n" )
{
   Vector<S32> * columns = object->columnOffsets();

   if(columns->size() > 0)
   {
      columns->setSize(columns->size() - 1);
      object->balanceFrames();
   }
   else
      Con::errorf(ConsoleLogEntry::General, "No columns exist to remove");
}

DefineEngineMethod( GuiFrameSetCtrl, removeRow, void, (),,
   "Remove the last (bottom) row.\n\n" )
{
   Vector<S32> * rows = object->rowOffsets();

   if(rows->size() > 0)
   {
      rows->setSize(rows->size() - 1);
      object->balanceFrames();
   }
   else
      Con::errorf(ConsoleLogEntry::General, "No rows exist to remove");
}

DefineEngineMethod( GuiFrameSetCtrl, getColumnCount, S32, (),,
   "Get the number of columns.\n\n"
   "@return The number of columns\n" )
{
   return(object->columnOffsets()->size());
}

DefineEngineMethod( GuiFrameSetCtrl, getRowCount, S32, (),,
   "Get the number of rows.\n\n"
   "@return The number of rows\n" )
{
   return(object->rowOffsets()->size());
}

DefineEngineMethod( GuiFrameSetCtrl, getColumnOffset, S32, ( S32 index ),,
   "Get the horizontal offset of a column.\n\n"
   "@param index Index of the column to query\n"
   "@return Column offset in pixels\n" )
{
   if(index < 0 || index > object->columnOffsets()->size())
   {
      Con::errorf(ConsoleLogEntry::General, "Column index out of range");
      return(0);
   }
   return((*object->columnOffsets())[index]);
}

DefineEngineMethod( GuiFrameSetCtrl, getRowOffset, S32, ( S32 index ),,
   "Get the vertical offset of a row.\n\n"
   "@param index Index of the row to query\n"
   "@return Row offset in pixels\n" )
{
   if(index < 0 || index > object->rowOffsets()->size())
   {
      Con::errorf(ConsoleLogEntry::General, "Row index out of range");
      return(0);
   }
   return((*object->rowOffsets())[index]);
}

DefineEngineMethod( GuiFrameSetCtrl, setColumnOffset, void, ( S32 index, S32 offset ),,
   "Set the horizontal offset of a column.\n\n"
   "Note that column offsets must always be in increasing order, and therefore "
   "this offset must be between the offsets of the colunns either side.\n"
   "@param index  Index of the column to modify\n"
   "@param offset New column offset\n" )
{
   Vector<S32> & columns = *(object->columnOffsets());

   if(index < 0 || index > columns.size())
   {
      Con::errorf(ConsoleLogEntry::General, "Column index out of range");
      return;
   }

   // check the offset
   if(((index > 0) && (offset < columns[index-1])) ||
      ((index < (columns.size() - 1)) && (offset > columns[index+1])))
   {
      Con::errorf(ConsoleLogEntry::General, "Invalid column offset");
      return;
   }

   columns[index] = offset;
   object->updateSizes();
}

DefineEngineMethod( GuiFrameSetCtrl, setRowOffset, void, ( S32 index, S32 offset ),,
   "Set the vertical offset of a row.\n\n"
   "Note that row offsets must always be in increasing order, and therefore "
   "this offset must be between the offsets of the rows either side.\n"
   "@param index  Index of the row to modify\n"
   "@param offset New row offset\n" )
{
   Vector<S32> & rows = *(object->rowOffsets());

   if(index < 0 || index > rows.size())
   {
      Con::errorf(ConsoleLogEntry::General, "Row index out of range");
      return;
   }

   // check the offset
   if(((index > 0) && (offset < rows[index-1])) ||
      ((index < (rows.size() - 1)) && (offset > rows[index+1])))
   {
      Con::errorf(ConsoleLogEntry::General, "Invalid row offset");
      return;
   }

   rows[index] = offset;
   object->updateSizes();
}

DefineEngineMethod( GuiFrameSetCtrl, updateSizes, void, (),,
   "Recalculates child control sizes." )
{
   object->updateSizes();
}
*/