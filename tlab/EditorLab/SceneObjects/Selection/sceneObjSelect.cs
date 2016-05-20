//==============================================================================
// TorqueLab -> Editor Gui General Scripts
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// Sync the different Selection related Gui
function Scene::syncSelectionGui(%this) {
	// Inform the camera
	Lab.OrbitCameraChange(EWorldEditor.getSelectionSize(), EWorldEditor.getSelectionCentroid());	
	EditorGuiStatusBar.setSelectionObjectsByCount(EWorldEditor.getSelectionSize());
}
//------------------------------------------------------------------------------
//==============================================================================
// Sync the different Selection related Gui
function Scene::onSelectionChanged(%this) {
	// Inform the camera
	Scene.syncSelectionGui();
	Lab.DoSelectionCallback("Transform");
}
//------------------------------------------------------------------------------


//==============================================================================
// Scene Select Callbacks
//==============================================================================
//==============================================================================
// Shouldn't be used anymore OR could be use to select an object globally.
function Scene::selectObject(%this, %obj,%isLast,%source) {
	devLog("Scene::selectObject %obj,%isLast,%source",%obj,%isLast,%source);
	//Add an object to selection manually (Without using WorldEditor or SceneTree)	
	%this.onAddSelection(%obj,%isLast,%source);
/*	
if (%isLast $= "")
		%isLast = true;
	if (isObject(%source)) {
		%sourceClass = %source.getClassName();
		switch$(%sourceClass)
		{
			case "WorldEditor":
			case "GuiTreeViewCtrl":
				EWorldEditor.selectObject(%obj);	
				%ignoreTree = %source;	
		}
		
	}
	
	
	foreach$(%tree in Scene.sceneTrees) {
		if (isObject(%ignoreTree))
		{
			if (%tree.getId() $= %ignoreTree.getId()) {			
				continue;
			}
		}
		%tree.setSelectedItem( %obj,false,true);
		continue;
		//Replaced with faster above^^
		%item = %tree.findItemByObjectId(%obj.getId());	
		if (%item $= "-1")
			continue;	
		//%tree.setSelectedObject(%item,%isLast);
	}
	
	//Scene.onSelect(%obj);
	*/
}
//------------------------------------------------------------------------------
//==============================================================================
// Shouldn't be used anymore OR could be use to select an object globally.
function Scene::unselectObject(%this, %obj,%source) {
   devLog("Scene::unselectObject %obj,%source",%obj,%source);
	//Add an object to selection manually (Without using WorldEditor or SceneTree)	
	%this.onRemoveSelection(%obj,%source);
   }
//------------------------------------------------------------------------------
//==============================================================================
// Add Obj to Sel - Called when WorldEditor or a SceneTree selected an object
//==============================================================================
//==============================================================================
// Called from SceneObjectTrees onAddSelection Callback
function Scene::onAddSelection(%this, %obj, %isLastSelection,%source) 
{
	logd("Scene::onAddSelection(%this, %obj, %isLastSelection,%source)",%this, %obj, %isLastSelection,%source);
	if (isObject(%source)) {
		%sourceClass = %source.getClassName();
		switch$(%sourceClass)
		{
			case "WorldEditor":
			SceneEditorTree.setSelectedItem( %obj,false,true);
			EWorldEditor.selectObject( %obj );
			case "GuiTreeViewCtrl":
				%source.setSelectedItem( %obj,false,true);
				EWorldEditor.selectObject( %obj );
		}
		
	}
	//No source, selection provided from script so update WorldEditor and Trees
	else {
		SceneEditorTree.setSelectedItem( %obj,false,true);
		EWorldEditor.selectObject( %obj );
	}
	%plugin = Lab.currentEditor;
	if (%plugin.isMethod("onSelectObject"))
		%plugin.onSelectObject(%obj);
		
   if (LabMaterialEditor.isAwake())
   {
      if (%obj.getId() !$= LabMat.currentObject.getId())
      {
         LabMat.setCurrentObject(%obj);
      }
   }
		/*
if (isObject(MaterialEditorPreviewWindow))
		if ( MaterialEditorPreviewWindow.isVisible() )
			MaterialEditorTools.prepareActiveObject();*/
			
	if (%plugin.isMethod("on"@%obj.getClassName()@"Selected"))
		eval("%plugin.on"@%obj.getClassName()@"Selected(%obj);");
		
	if (!$Cfg_Common_Objects_autoInspect)
		return;
	
	if (!$Cfg_Common_Objects_allowMultiInspect)
	{
		//Simply set current as inspect
		Scene.doInspect(%obj);	
		return;
	}
	%this.doAddInspect(%obj,%isLastSelection);
}
//==============================================================================
// Remove Obj From Sel - Called when WorldEditor or a SceneTree unselected an object
//==============================================================================
//==============================================================================
function Scene::onRemoveSelection(%this, %obj,%source) {
	logd("Scene::onRemoveSelection",%obj);
	if (isObject(%source)) {
		%sourceClass = %source.getClassName();
		switch$(%sourceClass)
		{
			case "WorldEditor":
			SceneEditorTree.removeSelection(%obj);
			case "GuiTreeViewCtrl":			
			EWorldEditor.unselectObject( %obj );
		}
		
	}
	else
	  EWorldEditor.unselectObject( %obj );
	//EWorldEditor.unselectObject(%obj);
	%plugin = Lab.currentEditor;
	if (%plugin.isMethod("on"@%obj.getClassName()@"Unselect"))
		eval("%plugin.on"@%obj.getClassName()@"Unselect(%obj);");

	if (%plugin.isMethod("onUnselect"))
		%plugin.onUnselect(%obj);	
			
	Scene.onSelectionChanged();
	//EWorldEditor.unselectObject(%obj);
	//%this.onUnselect(%obj);
	//Scene.doRemoveInspect(%obj);
}	
//------------------------------------------------------------------------------

//==============================================================================
function Scene::onMultiSelectDone(%this, %set) {
	logd("Scene::onMultiSelectDone",%set);
	%count = %set.getCount();	
	if (%count > 0){
		
	foreach( %obj in %set ) {		
		SceneEditorTree.setSelectedItem( %obj,false,true);		
	}	
	Scene.doInspect(%set.getObject(0));
	}
	Scene.syncSelectionGui();
}	

//------------------------------------------------------------------------------
/*
//==============================================================================
function Scene::onSelect(%this, %obj) {
	logd("Scene::onSelect",%obj);

	if (%obj.getClassName() $= "SimGroup") {
		Scene.setActiveSimGroup( %obj );
	}
	Scene.doInspect(%obj);
	
	%plugin = Lab.currentEditor;
	if (%plugin.isMethod("onSelectObject"))
		%plugin.onSelectObject(%obj);

	if (%plugin.isMethod("on"@%obj.getClassName()@"Selected"))
		eval("%plugin.on"@%obj.getClassName()@"Selected(%obj);");
	
	// Update the materialEditorList
	$Lab::materialEditorList = %obj.getId();

	// Used to help the Material Editor( the M.E doesn't utilize its own TS control )
	// so this dirty extension is used to fake it
	if (isObject(MaterialEditorPreviewWindow))
		if ( MaterialEditorPreviewWindow.isVisible() )
			MaterialEditorTools.prepareActiveObject();

	$WorldEditor_LastSelZ = %obj.position.z;
	
	Scene.onSelectionChanged();
}
//------------------------------------------------------------------------------
*/

//==============================================================================
// Scene Unselect Callbacks
//==============================================================================
/*
//==============================================================================
function Scene::unselectObject(%this, %obj,%source) {
	logd("Scene::unselectObject %obj,%source",%obj,%source);
	
	foreach$(%tree in Scene.sceneTrees) {		
		if (%tree.isItemSelected(%obj))
			%tree.removeSelection(%obj);
	}
		
	Scene.doRemoveInspect(%obj);
	Scene.onUnSelect(%obj);
	
}
//------------------------------------------------------------------------------

//==============================================================================
function Scene::onUnselect(%this, %obj) {
	logd("Scene::onUnSelect",%obj);		

	//EWorldEditor.unselectObject(%obj);
	%plugin = Lab.currentEditor;
	if (%plugin.isMethod("on"@%obj.getClassName()@"Unselected"))
		eval("%plugin.on"@%obj.getClassName()@"Unselected(%obj);");

	if (%plugin.isMethod("onUnselectObject"))
		%plugin.onUnselectObject(%obj);	
			
	Scene.onSelectionChanged();

}
//------------------------------------------------------------------------------
*/

//------------------------------------------------------------------------------


function Scene::setDirty(%this) {
	EWorldEditor.isDirty = true;
}

//------------------------------------------------------------------------------
