//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$SceneEd_DatablockTreeSetMode = false;
//==============================================================================
// Add the datablocks to the treeView 
function SceneEd::populateDBTree(%this,%classList) {
	if (DbEd.populatingTree)
		return;
   DbEd.populatingTree = true;
   SceneEd.excludeClientOnlyDatablocks = 0;
   if (!$SceneEd_DatablockTreeSetMode)
   {
      SceneEd.populateDBTreeData(%classList);
      return;
   }
   if (!isObject(SceneDatablockSet))
      new SimSet(SceneDatablockSet);

	// Populate datablock tree.
	if( %this.excludeClientOnlyDatablocks )
		%set = DataBlockGroup;
	else
		%set = DataBlockSet;

	SceneDatablockTree.clear();	

	foreach( %datablock in %set ) {
		%unlistedFound = false;
		%id = %datablock.getId();
/*
		foreach( %obj in UnlistedDatablocks )
			if( %obj.getId() == %id ) {
				%unlistedFound = true;
				break;
			}

		if( %unlistedFound )
			continue;
*/

      %class = %datablock.getClassName();
      //Check if the class set is created, then add datablock to it
      %classSet = SceneDatablockSet.findObjectByInternalName(%class);	
	   if (!isObject(%classSet))
         %classSet = newSimSet("",SceneDatablockSet,%class);
   
       %classSet.add(%datablock);	
	}
    DbEd.populatingTree = false;
}
//------------------------------------------------------------------------------

//==============================================================================
// Add the datablocks to the treeView 
function SceneEd::populateDBTreeData(%this,%classList) {   
    

	SceneDatablockTree.clear();
	

	foreach( %datablock in DataBlockSet ) {
		%unlistedFound = false;
		%id = %datablock.getId();
      	%this.addExistingItem( %datablock, true );
	/*	foreach( %obj in UnlistedDatablocks )
			if( %obj.getId() == %id ) {
				%unlistedFound = true;
				break;
			}

		if( %unlistedFound )
			continue;

		if (DbEd.activeClasses !$= "" && getRecordCount(DbEd.activeClasses) < 2) {
			if (!strFind(DbEd.activeClasses,%datablock.getClassName()))
				continue;
		}
*/
	
	}

	SceneDatablockTree.sort( 0, true, false, false );
	DbEd.populatingTree = false;
	return;
	// Populate datablock type tree.
	//%classList = DbEd.activeClasses;
	//if (%classList $= "")
	%classList = enumerateConsoleClasses( "SimDatablock" );
	SceneDatablockTree.clear();

	foreach$( %datablockClass in %classList ) {
		if(    !%this.isExcludedDatablockType( %datablockClass )
				 && SceneDatablockTree.findItemByName( %datablockClass ) == 0 )
			SceneDatablockTree.insertItem( 0, %datablockClass );
	}

	SceneDatablockTree.sort( 0, false, false, false );
	
}
//------------------------------------------------------------------------------
//==============================================================================
//Add an existing datablock to the tree
function SceneEd::addExistingItem( %this, %datablock, %dontSort ) {
	%tree = SceneDatablockTree;
	// Look up class at root level.  Create if needed.
	%class = %datablock.getClassName();      
	%parentID = %tree.findItemByName( %class );
   devLog("ParentId",%parentID);
	if( %parentID == 0 )
		%parentID = %tree.insertItem( 0, %class );

	// If the datablock is already there, don't
	// do anything.
	%id = %tree.findItemByValue( %datablock.getId());

	if( %id > 0)
		return;

	// It doesn't exist so add it.
	%name = %datablock.getName();

	//if( %this.DBPM.isDirty( %datablock ) )
		//%name = %name @ " *";

   devLog("Insert items in group",%datablock.getId(),%parentID);
	%id = SceneDatablockTree.insertItem( %parentID, %name, %datablock.getId() );
	//DbEd.dbClassList = strAddWord(DbEd.dbClassList,%class,true);
	//DbEd.dbClassItemIds[%class] = strAddWord(DbEd.dbClassItemIds[%class],%id,true);
	//DbEd.dbClassItemNames[%class] = strAddWord(DbEd.dbClassItems[%class],%name,true);

	//if( !%dontSort )
		//SceneDatablockTree.sort( %parentID, false, false, false );

	return %id;
}
//------------------------------------------------------------------------------
//==============================================================================
//Check for some datablock exclusions
function SceneEd::isExcludedDatablockType( %this, %className ) {
	switch$( %className ) {
	case "SimDatablock":
		return true;

	case "SFXTrack": // Abstract.
		return true;

	case "SFXFMODEvent": // Internally created.
		return true;

	case "SFXFMODEventGroup": // Internally created.
		return true;
	}

	return false;
}
//------------------------------------------------------------------------------
