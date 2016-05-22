//==============================================================================
// TorqueLab -> Initialize editor plugins
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// Called 500 ms after EditorGui onWake (Often overwrite by FrameWork package
function FW::initialSetup(%this)
{
    Lab.closeDisabledPluginsBin();
    show(LabPluginBar);
    show(LabPaletteBar);
    LabPluginArray.reorderChild( LabPluginArray-->SceneEditorPlugin,LabPluginArray.getObject(0));
    LabPluginArray.refresh();
    FW.checkEditorCore();
}
//------------------------------------------------------------------------------

//==============================================================================
// Called 500 ms after EditorGui onWake (Often overwrite by FrameWork package
function FW::checkLaunchedLayout(%this)
{
    FW.checkEditorCore();
    Lab.schedule(100,"initSideBar");
    skipPostFx(false);
    ETools.initTools();
    Lab.initAllToolbarGroups();
    Lab.initToolbarTrash();
    ObjectBuilderGui.init();
    EditorGui-->DisabledPluginsBox.callOnChildrenNoRecurse("setVisible",true);
}
//------------------------------------------------------------------------------
function Lab::setDefaultLayoutSize( %this, %onlyIfSmaller )
{
    EditorGui-->SideBarContainer.minExtent = $LabCfg_Layout_LeftMinWidth SPC "100";
    EditorGui-->ToolsContainer.minExtent = $LabCfg_Layout_RightMinWidth SPC "100";
    if (!%onlyIfSmaller || $LabCfg_Layout_RightMinWidth >  EditorGui-->ToolsContainer.extent.x)
        EditorGui-->ToolsContainer.setExtent($LabCfg_Layout_RightMinWidth,EditorGui-->ToolsContainer.extent.y);
    if (!%onlyIfSmaller || $LabCfg_Layout_LeftMinWidth >  EditorGui-->SideBarContainer.extent.x)
        EditorGui-->SideBarContainer.setExtent($LabCfg_Layout_LeftMinWidth,EditorGui-->SideBarContainer.extent.y);
}
//==============================================================================
// Make sure all GUIs are fine once the editor is launched
function FW::onResized(%this)
{
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
    if (%colPosZ !$= "")
    {
        %colWidthZ = 	%this.extent.x - %colPosZ;
        %this.rightColumnSize = %colWidthZ;
    }
}



//==============================================================================
// Initialize the Editor Frames
//==============================================================================


//==============================================================================
// EditorFrame - Left-Right Column
//==============================================================================

//==============================================================================
// Make sure all GUIs are fine once the editor is launched
function FW::checkEditorCore(%this)
{
    foreach$(%coreGui in $FWCoreGuiList)
    {
        if (!isObject(%coreGui))
            %coreGui = %this.resetEditorCoreGui(%coreGui);
        if (!isObject(%coreGui))
        {
           warnLog("A core UI features check have failed. The missing item is:",%coreGui);
           continue;
        }
        
        %coreGui.superClass = "EditorGuiContainers";
        %parentInt = $FWCoreGuiParent[%coreGui.getName()];
        %parent = EditorGui.findObjectByInternalName(%parentInt@"Container",true);
        if (isObject(%parent))
            %parent.add(%coreGui);
    }
}
//------------------------------------------------------------------------------

//==============================================================================
// FROM MAIN OLD FRAME
//==============================================================================

//==============================================================================
// Make sure all GUIs are fine once the editor is launched
function FW::resetEditorCoreGui(%this,%name)
{
    %file = "tlab/EditorLab/gui/editorCore/"@%name@".gui";
    devLog("File",%file);
    if (!isFile(%file))
        return;
    %parentInt = $FWCoreGuiParent[%name];
    %parent = EditorGui.findObjectByInternalName(%parentInt@"Container",true);
    devLog("ParentInt",%parentInt,"Obj",%parent);
    if (!isObject(%parent))
        return;
    delObj(%name);
    delObj(%name@"Gui");
    exec(%file);
    %parent.add(%name);
    %name.defaultGui = %name@"Gui";
    return %name;
}
//------------------------------------------------------------------------------
