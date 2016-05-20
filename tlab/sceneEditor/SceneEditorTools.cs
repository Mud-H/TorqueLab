//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================



//==============================================================================
function SceneEditorTools::onPreEditorSave( %this ) {
	%this.removeToolClones();	
}
//------------------------------------------------------------------------------
//==============================================================================
function SceneEditorTools::onPostEditorSave( %this ) {
	%this.getToolClones();
}
//------------------------------------------------------------------------------
//==============================================================================

function SceneEditorTools::removeToolClones( %this ) {
	delObj(SEP_TransformRollout-->toolsStack);
	delObj(SceneEditorTools-->cloneTools);
}
//------------------------------------------------------------------------------
//==============================================================================
function SceneEditorTools::getToolClones( %this ) {
	SceneEditorTools.removeToolClones();
	if (isObject(ETransformTool))
		ETransformTool.cloneToCtrl(SEP_TransformRollout);
	//ECloneTool.cloneToCtrl(SEP_CloneToolsStack);	
	//foreach(%gui in SEP_TransformStack){
		//%gui.expanded = false;
	//}
}
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// SceneEditorTools Frame Set Scripts
//==============================================================================

function SceneEditorTools::setMeshRootFolder( %this,%folder ) {
	devLog("SceneEditorTools::setMeshRootFolder:",%folder);
	%this.meshRootFolder = %folder;
	%this-->meshRootFolder.setText(%folder);
}
//------------------------------------------------------------------------------
//==============================================================================
// SceneEditorTools.validateMeshRootFolder($ThisControl);
function SceneEditorTools::validateMeshRootFolder( %this,%ctrl ) {
	devLog("SceneEditorTools::setMeshRootFolder:",%ctrl.getValue());
}
//------------------------------------------------------------------------------
//==============================================================================
function SceneEditorTools::getMeshRootFolder( %this ) {
	getFolderName("","SceneEditorTools.setMeshRootFolder","art/");
}
//------------------------------------------------------------------------------
function SceneEditorTools::onObjectAdded( %this,%obj ) {
	//devLog("SceneEditorTools::onObjectAdded:",%obj);
}
function SceneEditorTools::onObjectRemoved( %this,%obj ) {
	//devLog("SceneEditorTools::onObjectRemoved:",%obj);
}