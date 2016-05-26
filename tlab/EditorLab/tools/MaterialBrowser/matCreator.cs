//==============================================================================
// TorqueLab -> Script dealing with the Material creation system
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
// Based on MaterialEditor by Dave Calabrese and Travis Vroman of Gaslight Studios
//==============================================================================

//==============================================================================
function MaterialSelector_Creator::onWake(%this)
{
    %menu = MaterialSelector_CloneMatList;
    %menu.clear();
    %menu.add("Blank Material",0);
    %count = materialSet.getCount();
    for(%i = 0; %i < %count; %i++)
    {
        // Process regular materials here
        %material = materialSet.getObject(%i);
        for( %k = 0; %k < UnlistedMaterials.count(); %k++ )
        {
            %unlistedFound = 0;
            if( UnlistedMaterials.getValue(%k) $= %material.name )
            {
                %unlistedFound = 1;
                break;
            }
        }
        if( %unlistedFound )
            continue;
        %menu.add(%material.getName(),%material.getId());
    }
    %selected = 0;
    if (isObject(MaterialSelector.selectedMaterial))
        %selected = MaterialSelector.selectedMaterial.getId();
    %menu.setSelected(%selected);
}
//------------------------------------------------------------------------------

//==============================================================================
function MaterialSelector_CloneMatList::onSelect(%this,%id,%name)
{
    if (%id $= "0" || !isObject(%id))
    {
        %file = $Pref::MaterialSelector::DefaultMaterialFile;
        %name = "NewMaterial";
    }
    else
    {
        %file = %id.getFilename();
        if (!isFile(%file))
            %file = $Pref::MaterialSelector::DefaultMaterialFile;
        %name = %id.getName()@"_clone";
    }
    MaterialSelector_CloneMatName.setText(%name);
    MaterialSelector_CloneMatFile.setText(%file);
}
//------------------------------------------------------------------------------
//==============================================================================
function MaterialSelector::CreateNewMaterialDlg(%this)
{
    %src = MaterialSelector_CloneMatList.getText();
    if (!isObject(%src))
        %createBlank = true;
    %name = MaterialSelector_CloneMatName.getText();
    if (%name $= "")
        return;
    %matName = getUniqueName(%name);
    if (%createBlank)
    {
        new Material(%matName)
        {
            diffuseMap[0] = "core/art/warnMat";
            mapTo = "unmapped_mat";
            parentGroup = RootGroup;
        };
    }
    %file = MaterialSelector_CloneMatFile.getText();
    %material.setFilename($Pref::MaterialSelector::DefaultMaterialFile);
    hide(MaterialSelector_Creator);
}
//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
// this should create a new material pretty nicely
function MaterialSelector::initNewMaterial( %this,%material )
{
    // add one to All filter
    MaterialFilterAllArray.add( "", %material.name );
    MaterialFilterAllArrayCheckbox.setText("All ( " @ MaterialFilterAllArray.count() + 1 @ " ) ");
    MaterialFilterUnmappedArray.add( "", %material.name );
    MaterialFilterUnmappedArrayCheckbox.setText("Unmapped ( " @ MaterialFilterUnmappedArray.count() + 1 @ " ) ");
    if( MaterialSelector.currentStaticFilter !$= "MaterialFilterMappedArray" )
    {
        // create the new material gui
        %container = new GuiControl()
        {
            profile = "ToolsDefaultProfile";
            Position = "0 0";
            Extent = "74 85";
            HorizSizing = "right";
            VertSizing = "bottom";
            isContainer = "1";
            new GuiTextCtrl()
            {
                position = "10 70";
                profile = "ToolsGuiTextCenterProfile";
                extent = "64 16";
                text = %material.name;
            };
        };
        %previewButton = new GuiBitmapButtonCtrl()
        {
            internalName = %material.name;
            HorizSizing = "right";
            VertSizing = "bottom";
            profile = "ToolsButtonProfile";
            position = "7 4";
            extent = "64 64";
            buttonType = "PushButton";
            bitmap = "art/textures/core/warnMat";
            Command = "";
            text = "Loading...";
            useStates = false;
            new GuiBitmapButtonCtrl()
            {
                HorizSizing = "right";
                VertSizing = "bottom";
                profile = "ToolsButtonProfile";
                position = "0 0";
                extent = "64 64";
                Variable = "";
                buttonType = "toggleButton";
                bitmap = "tlab/materialEditor/assets/cubemapBtnBorder";
                groupNum = "0";
                text = "";
            };
        };
        %previewBorder = new GuiButtonCtrl()
        {
            internalName = %material.name@"Border";
            HorizSizing = "right";
            VertSizing = "bottom";
            profile = "ToolsButtonHighlight";
            position = "3 0";
            extent = "72 88";
            Variable = "";
            buttonType = "toggleButton";
            tooltip = %material.name;
            Command = "MaterialSelector.updateSelection( $ThisControl.getParent().getObject(1).internalName, $ThisControl.getParent().getObject(1).bitmap );";
            groupNum = "0";
            text = "";
        };
        %container.add(%previewButton);
        %container.add(%previewBorder);
        // add to the gui control array
        MaterialSelector-->materialSelection.add(%container);
    }
    // select me
    MaterialSelector.updateSelection( %material, "art/textures/core/warnMat.png" );
}
//------------------------------------------------------------------------------
