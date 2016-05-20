//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// Plugin Object Params - Used set default settings and build plugins options GUI
//==============================================================================


//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function MaterialEditorPlugin::initParamsArray( %this,%array ) {
	$MaterialEdCfg = newScriptObject("MaterialEditorCfg");
	%array.group[%groupId++] = "General settings";
	%array.setVal("DefaultMaterialFile",       "10" TAB "Default Width" TAB "SliderEdit"  TAB "range>>0 100;;tickAt>>1" TAB "SceneEditorCfg" TAB %groupId);
	%array.setVal("DiffuseSuffix",       "_d" TAB "Default Diffuse suffix" TAB "TextEdit"  TAB "" TAB "SceneEditorCfg" TAB %groupId);
	%array.setVal("AutoAddNormal",       "1" TAB "Auto add normal if found" TAB "checkbox"  TAB "" TAB "SceneEditorCfg" TAB %groupId);
	%array.setVal("NormalSuffix",       "_n" TAB "Default Normal suffix" TAB "TextEdit"  TAB "" TAB "SceneEditorCfg" TAB %groupId);
	%array.setVal("AutoAddSpecular",       "1" TAB "Auto add Specular if found" TAB "checkbox"  TAB "" TAB "SceneEditorCfg" TAB %groupId);
	%array.setVal("SpecularSuffix",       "_s" TAB "Default Specular suffix" TAB "TextEdit"  TAB "" TAB "SceneEditorCfg" TAB %groupId);
	%array.setVal("AutoAddSmoothness",       "1" TAB "Auto add Smoothness if found" TAB "checkbox"  TAB "" TAB "SceneEditorCfg" TAB %groupId);
	%array.setVal("SmoothnessSuffix",       "_s" TAB "Default Smoothness suffix" TAB "TextEdit"  TAB "" TAB "SceneEditorCfg" TAB %groupId);
	%array.setVal("AutoAddAO",       "1" TAB "Auto add AO if found" TAB "checkbox"  TAB "" TAB "SceneEditorCfg" TAB %groupId);
	%array.setVal("AOSuffix",       "_s" TAB "Default AO suffix" TAB "TextEdit"  TAB "" TAB "SceneEditorCfg" TAB %groupId);
	%array.setVal("AutoAddMetalness",       "1" TAB "Auto add Metalness if found" TAB "checkbox"  TAB "" TAB "SceneEditorCfg" TAB %groupId);
	%array.setVal("MetalnessSuffix",       "_s" TAB "Default Metalness suffix" TAB "TextEdit"  TAB "" TAB "SceneEditorCfg" TAB %groupId);
	%array.setVal("AutoAddComposite",       "1" TAB "Auto add Composite if found" TAB "checkbox"  TAB "" TAB "SceneEditorCfg" TAB %groupId);
	%array.setVal("CompositeSuffix",       "_s" TAB "Default Composite suffix" TAB "TextEdit"  TAB "" TAB "SceneEditorCfg" TAB %groupId);
	%array.setVal("PBRenabled",       "1" TAB "Enable PBR Materials" TAB "checkbox"  TAB "" TAB "MaterialEditorTools.activatePBR(*val*);" TAB %groupId);
	%array.setVal("MapModePBR",       "1" TAB "PBR maps mode" TAB "slider"  TAB "range>>0 2;;ticksAt>>1" TAB "MatEd" TAB %groupId);

}

//==============================================================================
// Plugin Object Callbacks - Called from TLab plugin management scripts
//==============================================================================
//------------------------------------------------------------------------------
// Material Editor


//==============================================================================
// Plugin Object Callbacks - Called from TLab plugin management scripts
//==============================================================================

//==============================================================================
// Called when TorqueLab is launched for first time
function MaterialEditorPlugin::onPluginLoaded( %this ) {	
	%this.customPalette = "SceneEditorPalette";
	%map = new ActionMap();
	%map.bindCmd( keyboard, "1", "EWorldEditorNoneModeBtn.performClick();", "" );  // Select
	%map.bindCmd( keyboard, "2", "EWorldEditorMoveModeBtn.performClick();", "" );  // Move
	%map.bindCmd( keyboard, "3", "EWorldEditorRotateModeBtn.performClick();", "" );  // Rotate
	%map.bindCmd( keyboard, "4", "EWorldEditorScaleModeBtn.performClick();", "" );  // Scale
	%map.bindCmd( keyboard, "f", "FitToSelectionBtn.performClick();", "" );// Fit Camera to Selection
	%map.bindCmd( keyboard, "z", "EditorGuiStatusBar.setCamera(\"Standard Camera\");", "" );// Free Camera
	%map.bindCmd( keyboard, "n", "ToggleNodeBar->renderHandleBtn.performClick();", "" );// Render Node
	%map.bindCmd( keyboard, "shift n", "ToggleNodeBar->renderTextBtn.performClick();", "" );// Render Node Text
	%map.bindCmd( keyboard, "alt s", "MaterialEditorTools.save();", "" );// Save Material
	//%map.bindCmd( keyboard, "delete", "ToggleNodeBar->renderTextBtn.performClick();", "" );// delete Material
	%map.bindCmd( keyboard, "g", "ESnapOptions-->GridSnapButton.performClick();" ); // Grid Snappping
	%map.bindCmd( keyboard, "t", "SnapToBar->objectSnapDownBtn.performClick();", "" );// Terrain Snapping
	%map.bindCmd( keyboard, "b", "SnapToBar-->objectSnapBtn.performClick();" ); // Soft Snappping
	%map.bindCmd( keyboard, "v", "SceneEditorToolbar->boundingBoxColBtn.performClick();", "" );// Bounds Selection
	%map.bindCmd( keyboard, "o", "EToolbarObjectCenterDropdown->objectBoxBtn.performClick(); objectCenterDropdown.toggle();", "" );// Object Center
	%map.bindCmd( keyboard, "p", "EToolbarObjectCenterDropdown->objectBoundsBtn.performClick(); objectCenterDropdown.toggle();", "" );// Bounds Center
	%map.bindCmd( keyboard, "k", "EToolbarObjectTransformDropdown->objectTransformBtn.performClick(); EToolbarObjectTransformDropdown.toggle();", "" );// Object Transform
	%map.bindCmd( keyboard, "l", "EToolbarObjectTransformDropdown->worldTransformBtn.performClick(); EToolbarObjectTransformDropdown.toggle();", "" );// World Transform
	MaterialEditorPlugin.map = %map;
	MaterialEditorTools.fileSpec = "Torque Material Files (materials.cs)|materials.cs|All Files (*.*)|*.*|";
	MaterialEditorTools.textureFormats = "Image Files (*.png, *.jpg, *.dds, *.bmp, *.gif, *.jng. *.tga)|*.png;*.jpg;*.dds;*.bmp;*.gif;*.jng;*.tga|All Files (*.*)|*.*|";
	MaterialEditorTools.modelFormats = "DTS Files (*.dts)|*.dts";
	MaterialEditorTools.lastTexturePath = "";
	MaterialEditorTools.lastTextureFile = "";
	MaterialEditorTools.lastModelPath = "";
	MaterialEditorTools.lastModelFile = "";
	MaterialEditorTools.currentMaterial = "";
	MaterialEditorTools.lastMaterial = "";
	MaterialEditorTools.currentCubemap = "";
	MaterialEditorTools.currentObject = "";
	MaterialEditorTools.livePreview = "1";
	MaterialEditorTools.currentLayer = "0";
	MaterialEditorTools.currentMode = "Material";
	MaterialEditorTools.currentMeshMode = "EditorShape";
	new ArrayObject(UnlistedCubemaps);
	UnlistedCubemaps.add( "unlistedCubemaps", matEdCubeMapPreviewMat );
	UnlistedCubemaps.add( "unlistedCubemaps", WarnMatCubeMap );
	//MaterialEditor persistence manager
	
	show(MaterialEditorPreviewWindow);
	MaterialEditorTools.establishMaterials();
	MatEd_PreviewOptions.expanded = false;
	
	//MaterialEditorTools.rows = "0 230";
	//MaterialEditorTools.updateSizes();
	
	MaterialEditorTools.initGui();
}
//------------------------------------------------------------------------------
//==============================================================================
// Called when the Plugin is activated (Active TorqueLab plugin)
function MaterialEditorPlugin::onActivated( %this ) {
	//MaterialEditorTools.rows = "0 230";
	//MaterialEditorTools.updateSizes();
	if($gfx::wireframe) {
		$wasInWireFrameMode = true;
		$gfx::wireframe = false;
	} else {
		$wasInWireFrameMode = false;
	}

	MaterialEditorTools.activateGui();
	WEditorPlugin.onActivated();
	MaterialEditorTools-->propertiesOptions.expanded = 0;
	SceneEditorToolbar.setVisible( true );
	%selObj = EWorldEditor.getSelectedObject(0);
	if (isObject(%selObj))
	   MaterialEditorTools.currentObject = %selObj;
   else if (!isObject(MaterialEditorTools.currentObject))
	   MaterialEditorTools.currentObject = $Lab::materialEditorList;
	// Execute the back end scripts that actually do the work.
	MaterialEditorTools.open();
	Parent::onActivated(%this);
	hide(MEP_CallbackArea);
	hide(matEd_addCubemapWindow);
	matEd_addCubemapWindow.setVisible(0);
	show(MaterialEditorPreviewWindow);

}
//------------------------------------------------------------------------------
//==============================================================================
// Called when the Plugin is deactivated (active to inactive transition)
function MaterialEditorPlugin::onDeactivated( %this ) {
	if($wasInWireFrameMode)
		$gfx::wireframe = true;

	WEditorPlugin.onDeactivated();

	// if we quit, restore with notDirty
	if(MaterialEditorTools.materialDirty) {
		//keep on doing this
		MaterialEditorTools.copyMaterials( notDirtyMaterial, materialEd_previewMaterial );
		MaterialEditorTools.copyMaterials( notDirtyMaterial, MaterialEditorTools.currentMaterial );
		MaterialEditorTools.guiSync( materialEd_previewMaterial );
		materialEd_previewMaterial.flush();
		materialEd_previewMaterial.reload();
		MaterialEditorTools.currentMaterial.flush();
		MaterialEditorTools.currentMaterial.reload();
	}

	if( isObject(MaterialEditorTools.currentMaterial) ) {
		MaterialEditorTools.lastMaterial = MaterialEditorTools.currentMaterial.getName();
	}

	MaterialEditorTools.setMaterialNotDirty();
	// First delete the model so that it releases
	// material instances that use the preview materials.
	matEd_previewObjectView.deleteModel();
	// Now we can delete the preview materials and shaders
	// knowing that there are no matinstances using them.
	matEdCubeMapPreviewMat.delete();
	materialEd_previewMaterial.delete();
	materialEd_justAlphaMaterial.delete();
	materialEd_justAlphaShader.delete();
	$MaterialEditor_MaterialsLoaded = false;
	SceneEditorToolbar.setVisible( false );
	Parent::onDeactivated(%this);
	//hide(MaterialEditorPreviewWindow);
}
//------------------------------------------------------------------------------
//==============================================================================
// Called from TorqueLab after plugin is initialize to set needed settings
function MaterialEditorPlugin::onPluginCreated( %this ) {
}
//------------------------------------------------------------------------------
//==============================================================================
// Called from TorqueLab after plugin is initialize to set needed settings
function MaterialEditorPlugin::onSelectObject( %this,%obj ) {
	devLog("MaterialEditorPlugin::onSelectObject",%obj);
	MatEd.setActiveObject( %obj );
}
//------------------------------------------------------------------------------

//==============================================================================
// Called when the mission file has been saved
function MaterialEditorPlugin::onSaveMission( %this, %file ) {
}
//------------------------------------------------------------------------------
//==============================================================================
// Called when TorqueLab is closed
function MaterialEditorPlugin::onEditorSleep( %this ) {
}
//------------------------------------------------------------------------------
//==============================================================================
//Called when editor is selected from menu
function MaterialEditorPlugin::onEditMenuSelect( %this, %editMenu ) {
	WEditorPlugin.onEditMenuSelect( %editMenu );
}
//------------------------------------------------------------------------------
//==============================================================================
//Called when editor frames has been resized
function MaterialEditorPlugin::onLayoutResized( %this ) {
   logd("MaterialEditorPlugin::onLayoutResized");   
}
//------------------------------------------------------------------------------