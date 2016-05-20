//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
function ObjectCreatorMenu::onSelect( %this, %id, %text ) {
   if (strFind(%text,"/") && $ObjCreatorMenu_Minimal)
      return;
   
	%split = strreplace( %text, "/", " " );
	%this.schedule( 1, "navigate", %split );

}
//------------------------------------------------------------------------------
//==============================================================================
function ObjectCreator::navigateDown( %this, %folder ) {
	if ( %this.address $= "" )
		%address = %folder;
	else
		%address = %this.address SPC %folder;

	// Make sure to call navigate after this function
	%this.schedule( 1, "navigate", %address );
}
//------------------------------------------------------------------------------
//==============================================================================
function ObjectCreator::navigateUp( %this ) {
	%count = getWordCount( %this.address );
	if ( %count == 0 )
		return;

	if ( %count == 1 )
		%address = "";
	else
		%address = getWords( %this.address, 0, %count - 2 );

   %this.schedule( 1, "navigate", %address );	
}
//------------------------------------------------------------------------------

//==============================================================================
// Navigate through Creator Book Data
function ObjectCreator::navigate( %this, %address ) {
	ObjectCreatorArray.frozen = true;
	ObjectCreatorArray.clear();
	ObjectCreatorMenu.clear();
	show(ObjectCreatorArray);
	hide(ObjectCreatorIconSrc);
	%type = ObjectCreator.objType;

	if (!%this.isMethod("navigate"@%type)||%type $= "") {
		warnLog("Couldn't find a scene creator navigating function for type:",%type);
		return;
	}

	eval("%this.navigate"@%type@"(%address);");
	ObjectCreator.lastTypeAddress[%type] = %address;
	ObjectCreatorArray.sort( "arrayIconCompare" );

	for ( %i = 0; %i < ObjectCreatorArray.getCount(); %i++ ) {
		ObjectCreatorArray.getObject(%i).autoSize = false;
	}

	Lab.addFolderUpIcon(%this);
	%this.setViewId($ObjectCreator_ViewId);
	ObjectCreatorArray.refresh();
	/*if ( %this.isList ) {
		%width = ObjectCreatorArray.extent.x - 6;
		ObjectCreatorArray.colSize = %width;
		%extent = %width SPC "20";
		//%ctrl.extent = %extent;
		//%ctrl.autoSize = true;
	} else {
		%width = (ObjectCreatorArray.extent.x - 10) /2;
		//%extent = %width SPC "20";
		//%ctrl.extent = %extent;
		ObjectCreatorArray.colSize = %width;
	}*/
	//ObjectCreatorArray.frozen = false;
	//ObjectCreatorArray.refresh();
	// Recalculate the array for the parent guiScrollCtrl
	ObjectCreatorArray.getParent().computeSizes();
	%this.address = %address;
	ObjectCreatorMenu.sort();
	%str = strreplace( %address, " ", "/" );
	%r = ObjectCreatorMenu.findText( %str );

	if ( %r != -1 )
		ObjectCreatorMenu.setSelected( %r, false );
	else
		ObjectCreatorMenu.setText( %str );

	ObjectCreatorMenu.tooltip = %str;
}

//------------------------------------------------------------------------------
//==============================================================================
// Navigate through Creator Book Data
function ObjectCreator::navigateLevel( %this, %address ) {
	// Add groups to popup menu
	%array = Scene.array;
	%array.sortk();
	%count = %array.count();

	if ( %count > 0 ) {
		%lastGroup = "";

		for ( %i = 0; %i < %count; %i++ ) {
			%group = %array.getKey( %i );

			if ( %group !$= %lastGroup ) {
				//ObjectCreatorMenu.add( %group );

				if ( %address $= "" )
					Lab.addFolderIcon( %this,%group );
			}

			if ( %address $= %group ) {
				%args = %array.getValue( %i );
				%class = %args.val[0];
				%name = %args.val[1];
				%func = %args.val[2];
				%this.addMissionObjectIcon( %class, %name, %func );
			}

			%lastGroup = %group;
		}
	}
}
//==============================================================================
// Navigate through Creator Book Data
function ObjectCreator::navigateScripted( %this, %address ) {
	%category = getWord( %address, 1 );
	%dataGroup = "DataBlockGroup";

	for ( %i = 0; %i < %dataGroup.getCount(); %i++ ) {
		%obj = %dataGroup.getObject(%i);
		// echo ("Obj: " @ %obj.getName() @ " - " @ %obj.category );
		
		if (%obj.isMemberOfClass("ItemData") && %obj.shapeFile $= "" ){
			warnLog("ItemData not listed because no valid shape faound. ItemData",%obj.getName(),"Shape",%obj.shapeFile);			
			continue;
		}      
		  
		if ( %obj.category $= "" && %obj.category == 0  )
			continue;		

		if ( %address $= "" ) {
			%ctrl = ObjectCreatorArray.findIconCtrl( %obj.category );

			if ( %ctrl == -1 ) {
				Lab.addFolderIcon(%this, %obj.category );
				ObjectCreatorMenu.add(%obj.category);
			}
		} else if ( %address $= %obj.category ) {
			%ctrl = ObjectCreatorArray.findIconCtrl( %obj.getName() );
			//ObjectCreatorMenu.add(%obj.category @"\c2-\c3"@%obj.getName(),%obj.getId());
			if ( %ctrl == -1 )
				%this.addShapeIcon( %obj );
		}
	}
}
//==============================================================================

//==============================================================================
function ObjectCreator::addFileIcon( %this, %fullPath ) {
	%ctrl = Lab.createArrayIcon();
	%ext = fileExt( %fullPath );

	if (strFindWords(strlwr(%ext),"dae dts")) {
		%createCmd = "Scene.createMesh( \"" @ %fullPath @ "\" );";
		%type = "Mesh";
		%ctrl.createCmd = %createCmd;
	} else if (strFindWords(strlwr(%ext),"png tga jpg bmp")) {
		%type = "Image";
	} else	{
		%type = "Unknown";
	}

	%file = fileBase( %fullPath );
	%fileLong = %file @ %ext;
	%tip = %fileLong NL
			 "Size: " @ fileSize( %fullPath ) / 1000.0 SPC "KB" NL
			 "Date Created: " @ fileCreatedTime( %fullPath ) NL
			 "Last Modified: " @ fileModifiedTime( %fullPath );
	%ctrl.altCommand = "ObjectCreator.icon"@%type@"Alt($ThisControl);";
	%ctrl.iconBitmap = ( ( %ext $= ".dts" ) ? EditorIconRegistry::findIconByClassName( "TSStatic" ) : "tlab/art/icons/default/iconCollada" );
	%ctrl.text = %file;
	%ctrl.type = %type;
	//%ctrl.superClass = "ObjectCreatorIcon";
	//%ctrl.superClass = "ObjectCreatorIcon"@%type;
	%ctrl.tooltip = %tip;
	%ctrl.buttonType = "radioButton";
	%ctrl.groupNum = "-1";
	%ctrl.fullPath = %fullPath;
	ObjectCreatorArray.addGuiControl( %ctrl );
}
//------------------------------------------------------------------------------
//ObjectCreator.addFolderUpIcon






//==============================================================================
function ObjectCreator::addMissionObjectIcon( %this, %class, %name, %buildfunc ) {
	%ctrl = Lab.createArrayIcon();
	// If we don't find a specific function for building an
	// object then fall back to the stock one
	%method = "build" @ %buildfunc;
	

	if( !ObjectBuilderGui.isMethod( %method ) )
		%method = "build" @ %class;

	if( !ObjectBuilderGui.isMethod( %method ) )
		%method = "build" @ %class;

	if( !ObjectBuilderGui.isMethod( %method ) ) {
		%func = "build" @ %buildfunc;

		if (isFunction("build" @ %buildfunc))
			%cmd = "return build" @ %buildfunc @ "();";
		else
			%cmd = "return new " @ %class @ "();";
	} else {
		%cmd = "ObjectBuilderGui." @ %method @ "();";
	}

	%ctrl.altCommand = "ObjectBuilderGui.newObjectCallback = \"ObjectCreator.onFinishCreateObject\"; Scene.createObject( \"" @ %cmd @ "\" );";
	%ctrl.iconBitmap = EditorIconRegistry::findIconByClassName( %class );
	%ctrl.text = %name;
	%ctrl.class = "CreatorMissionObjectIconBtn";
	%ctrl.tooltip = %class;
	%ctrl.buttonType = "radioButton";
	%ctrl.groupNum = "-1";
	%ctrl.type = "MissionObject";
	ObjectCreatorArray.addGuiControl( %ctrl );
}
//------------------------------------------------------------------------------
//==============================================================================
function ObjectCreator::addShapeIcon( %this, %datablock ) {
	%ctrl = Lab.createArrayIcon();
	%name = %datablock.getName();
	%class = %datablock.getClassName();
	%cmd = %class @ "::create(" @ %name @ ");";
	%shapePath = ( %datablock.shapeFile !$= "" ) ? %datablock.shapeFile : %datablock.shapeName;
	%createCmd = "Scene.createObject( \\\"" @ %cmd @ "\\\" );";
	%ctrl.altCommand = "ColladaImportDlg.showDialog( \"" @ %shapePath @ "\", \"" @ %createCmd @ "\" );";
	%ctrl.iconBitmap = EditorIconRegistry::findIconByClassName( %class );
	%ctrl.text = %name;
	%ctrl.class = "CreatorShapeIconBtn";
	%ctrl.tooltip = %name;
	%ctrl.buttonType = "radioButton";
	%ctrl.groupNum = "-1";
	ObjectCreatorArray.addGuiControl( %ctrl );
}
//------------------------------------------------------------------------------
//==============================================================================
function ObjectCreator::addStaticIcon( %this, %fullPath ) {
	%ctrl = %this.createArrayIcon();
	%ext = fileExt( %fullPath );
	%file = fileBase( %fullPath );
	%fileLong = %file @ %ext;
	%tip = %fileLong NL
			 "Size: " @ fileSize( %fullPath ) / 1000.0 SPC "KB" NL
			 "Date Created: " @ fileCreatedTime( %fullPath ) NL
			 "Last Modified: " @ fileModifiedTime( %fullPath );
	%createCmd = "Scene.createStatic( \\\"" @ %fullPath @ "\\\" );";
	%ctrl.altCommand = "ColladaImportDlg.showDialog( \"" @ %fullPath @ "\", \"" @ %createCmd @ "\" );";
	%ctrl.iconBitmap = ( ( %ext $= ".dts" ) ? EditorIconRegistry::findIconByClassName( "TSStatic" ) : "tlab/art/icons/default/iconCollada" );
	%ctrl.text = %file;
	%ctrl.class = "CreatorStaticIconBtn";
	%ctrl.tooltip = %tip;
	%ctrl.buttonType = "radioButton";
	%ctrl.groupNum = "-1";
	ObjectCreatorArray.addGuiControl( %ctrl );
}
//------------------------------------------------------------------------------
//==============================================================================
function ObjectCreator::addPrefabIcon( %this, %fullPath ) {
	%ctrl = %this.createArrayIcon();
	%ext = fileExt( %fullPath );
	%file = fileBase( %fullPath );
	%fileLong = %file @ %ext;
	%tip = %fileLong NL
			 "Size: " @ fileSize( %fullPath ) / 1000.0 SPC "KB" NL
			 "Date Created: " @ fileCreatedTime( %fullPath ) NL
			 "Last Modified: " @ fileModifiedTime( %fullPath );
	%ctrl.altCommand = "Scene.createPrefab( \"" @ %fullPath @ "\" );";
	%ctrl.iconBitmap = EditorIconRegistry::findIconByClassName( "Prefab" );
	%ctrl.text = %file;
	%ctrl.class = "CreatorPrefabIconBtn";
	%ctrl.tooltip = %tip;
	%ctrl.buttonType = "radioButton";
	%ctrl.groupNum = "-1";
	ObjectCreatorArray.addGuiControl( %ctrl );
}
//------------------------------------------------------------------------------
