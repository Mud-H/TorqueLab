//==============================================================================
// TorqueLab -> SceneEditor Inspector script
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
// Allow to manage different GUI Styles without conflict
//==============================================================================
function SceneInspector::inspect( %this, %obj ) {
   if (isObject(%obj))
   {
   if (%obj.isMemberOfClass("SimDatablock"))
      %mode = "Datablock";
   else
      %mode = "Object";
      
      
     if (%mode !$=  SceneInspector.dataMode)
      SceneEd.setInspectorDataMode(%mode);
   }
	Parent::inspect( %this, %obj );
	
}
//------------------------------------------------------------------------------
//==============================================================================
function SceneEd::manualMultiInspect( %this, %obj ) {
   if ($SceneEd_TreeMode $= "Datablock")
   {
      //Check for selected item in tree
       %dataItem = SceneDatablockTree.getSelectedItemList();
         %item = getWord(%dataItem,0);
         %db =   SceneDatablockTree.getItemValue(%item);
         if (isObject(%db))
         {
            Scene.doInspect(%db,1);
            return;
         }
   }
   
   
	if (EWorldEditor.getSelectionSize()==0)
		Scene.doInspect("",1);
	else if (EWorldEditor.getSelectionSize()==1)
		Scene.doInspect(EWorldEditor.getSelectedObject(0),1);
	else 
		for(%i=0; %i < EWorldEditor.getSelectionSize(); %i++) 
			SceneInspector.doAddInspect(EWorldEditor.getSelectedObject(%i));	
}
//------------------------------------------------------------------------------
//==============================================================================
// Inspector Compound Callbacks
//==============================================================================
//==============================================================================
function SceneInspector::onFieldAdded( %this, %object, %fieldName ) {
	logd("SceneInspector::onFieldAdded( %this, %object, %fieldName )",%this, %object, %fieldName );
	Parent::onFieldAdded( %this, %object, %fieldName );
}
//------------------------------------------------------------------------------
//==============================================================================
function SceneInspector::onFieldRemoved( %this, %object, %fieldName ) {
	logd("SceneInspector::onFieldRemoved( %this, %object, %fieldName )",%this, %object, %fieldName );
	Parent::onFieldRemoved( %this, %object, %fieldName );
}
//------------------------------------------------------------------------------
//==============================================================================
function SceneInspector::onFieldRenamed( %this, %object, %fieldName ,%newName) {
	logd("SceneInspector::onFieldRenamed( %this, %object, %fieldName,%newName )",%this, %object, %fieldName,%newName );
	Parent::onFieldRenamed( %this, %object, %fieldName,%newName );
}
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
function SceneInspector::onFieldSelected( %this, %fieldName, %fieldTypeStr, %fieldDoc ) {
	logd("SceneInspector::onFieldSelected( %this, %fieldName, %fieldTypeStr, %fieldDoc )",%this, %fieldName, %fieldTypeStr, %fieldDoc );
	Parent::onFieldSelected( %this, %fieldName, %fieldTypeStr,%fieldDoc );
}
//------------------------------------------------------------------------------
//==============================================================================
function SceneInspector::onFieldRightClick( %this, %object, %fieldName ,%newName) {
	logd("SceneInspector::onFieldRightClick( %this, %object, %fieldName,%newName )",%this, %object, %fieldName,%newName );
	Parent::onFieldRightClick( %this, %object, %fieldName,%newName );
}
//------------------------------------------------------------------------------


//------------------------------------------------------------------------------
//==============================================================================
function SceneInspector::onInspectorFieldModified( %this, %object, %fieldName, %arrayIndex, %oldValue, %newValue ) {
	logd("SceneInspector::onInspectorFieldModified( %this,  %object, %fieldName, %arrayIndex, %oldValue, %newValue )",%this, %object, %fieldName, %arrayIndex, %oldValue, %newValue );
   Parent::onInspectorFieldModified( %this, %object, %fieldName, %arrayIndex, %oldValue, %newValue );
   
	if( %object.isMemberOfClass( "SimDataBlock" ) )
   {
		 %this.onDatablockFieldModified(%object, %fieldName, %arrayIndex, %oldValue, %newValue );
		 //return;
   }

	
}
//------------------------------------------------------------------------------
//==============================================================================
function SceneInspector::onDatablockFieldModified( %this, %object, %fieldName, %arrayIndex, %oldValue, %newValue ) {
	logd("SceneInspector::onDatablockFieldModified( %this,  %object, %fieldName, %arrayIndex, %oldValue, %newValue )",%this, %object, %fieldName, %arrayIndex, %oldValue, %newValue );
	
   %object.schedule( 1, "reloadOnLocalClient" );
   
   //Tell SceneEditor Datablock Page
   SEP_DatablockPage.datablockFieldModified(%object, %fieldName, %arrayIndex, %oldValue, %newValue );

}
//------------------------------------------------------------------------------
//==============================================================================
// The following three methods are for fields that edit field value live and thus cannot record
// undo information during edits.  For these fields, undo information is recorded in advance and
// then either queued or disarded when the field edit is finished.

function SceneInspector::onInspectorPreFieldModification( %this, %fieldName, %arrayIndex ) {
	logd("SceneInspector::onInspectorPreFieldModification( %this, %fieldName, %arrayIndex )",%this, %fieldName, %arrayIndex );
	Parent::onInspectorPreFieldModification( %this, %fieldName, %arrayIndex );
}
//------------------------------------------------------------------------------
//==============================================================================
function SceneInspector::onInspectorPostFieldModification( %this , %fieldName, %arrayIndex) {
	logd("SceneInspector::onInspectorPostFieldModification( %this, %fieldName, %arrayIndex)",%this, %fieldName, %arrayIndex );
	Parent::onInspectorPostFieldModification( %this , %fieldName, %arrayIndex);
}
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
function SceneInspector::onInspectorDiscardFieldModification( %this, %fieldName, %arrayIndex ) {
	logd("SceneInspector::onInspectorDiscardFieldModification( %this, %fieldName, %arrayIndex)",%this, %fieldName, %arrayIndex );
	Parent::onInspectorDiscardFieldModification( %this , %fieldName, %arrayIndex);
}
//------------------------------------------------------------------------------
