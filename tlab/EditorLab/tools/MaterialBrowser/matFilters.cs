//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$MatBrowser_SkipTerrainMaterials = true;
function MatBrowser::clearFilterArray( %this ) {
	%filterArray = MatBrowser-->tagFilters;

	foreach(%obj in %filterArray) {
		if (%obj.internalName $= "CustomFilter" && %obj.superClass !$= "NoDeleteCtrl" )
			%deleteList = strAddWord(%deleteList,%obj.getId());
	}

	foreach$(%objID  in %deleteList)
		delObj(%objID);
}
//------------------------------------------------------------------------------
//oldmatSelector.buildStaticFilters();
function MatBrowser::buildTerrainStaticFilters( %this ) {
   %mats = ETerrainEditor.getTerrainBlocksMaterialList();
		%count = getRecordCount( %mats );
   for(%i = 0; %i < %count; %i++) {
		// Process terrain materials
	
			%matInternalName = getRecord( %mats, %i );
			%material = TerrainMaterialSet.findObjectByInternalName( %matInternalName );

			// Is there no material info for this slot?
			if ( !isObject( %material ) )
				continue;

			// Add to the appropriate filters
			MaterialFilterMappedArray.add( "", %material );
			MaterialFilterAllArray.add( "", %material );
			
			if (%material.terrainMaterials && $MatBrowser_SkipTerrainMaterials )
			continue;
   }
}
//------------------------------------------------------------------------------
//==============================================================================
// Build materials filters data by checking all materials and adding it to filter groups
//oldmatSelector.buildStaticFilters();
function MatBrowser::buildStaticFilters( %this ) {

	%this.clearFilterArray();
	MatBrowser.staticFilterObjCount = MatBrowser-->tagFilters.getCount() - 1;// Remove the header box
	//%staticFilterContainer.delete();
	// Create our category array used in the selector, this code should be taken out
	// in order to make the material selector agnostic
	delObj(MaterialFilterAllArray);
	delObj(MaterialFilterMappedArray);
	delObj(MaterialFilterUnmappedArray);
	new ArrayObject(MaterialFilterAllArray);
	new ArrayObject(MaterialFilterMappedArray);
	new ArrayObject(MaterialFilterUnmappedArray);
	%mats = "";
	%count = 0;

   //If in terrainMaterials mode, get terrain Materials list, else get materials
	if( MatBrowser.terrainMaterials ) {
	   //Used by GroundCover to select terrainMat
	   %this.buildTerrainStaticFilters();
	   return;
	}
	
   %count = materialSet.getCount();
	for(%i = 0; %i < %count; %i++) {		
		// Process regular materials here
		%material = materialSet.getObject(%i);				

		for( %k = 0; %k < UnlistedMaterials.count(); %k++ ) {
			%unlistedFound = 0;

			if( UnlistedMaterials.getValue(%k) $= %material.name ) {
				%unlistedFound = 1;
				break;
			}
		}
      // Don't build filters for unlisted mats
		if( %unlistedFound )
			continue;
			
      //------------------------------------------------------------------------
      // Add it to appropriate group is mapped or not
      
      //If Unmapped, add to unmapped
		if( %material.mapTo $= "" || %material.mapTo $= "unmapped_mat" ) {
			MaterialFilterUnmappedArray.add( "", %material.name );

			//running through the existing tag names
			for( %j = 0; %material.getFieldValue("materialTag" @ %j) !$= ""; %j++ )
				MaterialFilterUnmappedArray.add( %material.getFieldValue("materialTag" @ %j), %material.name );
		}
		 //Else add to mapped array
		else {
			MaterialFilterMappedArray.add( "", %material.name );

			for( %j = 0; %material.getFieldValue("materialTag" @ %j) !$= ""; %j++ )
				MaterialFilterMappedArray.add( %material.getFieldValue("materialTag" @ %j), %material.name );
		}
      //Add to all filters Array
		MaterialFilterAllArray.add( "", %material.name );

		for( %j = 0; %material.getFieldValue("materialTag" @ %j) !$= ""; %j++ )
			MaterialFilterAllArray.add( %material.getFieldValue("materialTag" @ %j), %material.name );
	}
    //Update the filter checkbox and add group count referemce
	MaterialFilterAllArrayCheckbox.setText("All ( " @ MaterialFilterAllArray.count() @" ) ");
	MaterialFilterMappedArrayCheckbox.setText("Mapped ( " @ MaterialFilterMappedArray.count() @" ) ");
	MaterialFilterUnmappedArrayCheckbox.setText("Unmapped ( " @ MaterialFilterUnmappedArray.count() @" ) ");
	devLog("Mat filtering built! Unmapped:",MaterialFilterUnmappedArray.count(),"Mapped:",MaterialFilterMappedArray.count(),"Total",MaterialFilterAllArray.count() );
}
//------------------------------------------------------------------------------

//==============================================================================
// Preload filter: Need to examine what it actually do
//==============================================================================

//==============================================================================
function MatBrowser::preloadFilter( %this ) {
	%selectedFilter = "";

	for( %i = MatBrowser.staticFilterObjCount; %i < MatBrowser-->tagFilters.getCount(); %i++ ) {
		if( MatBrowser-->tagFilters.getObject(%i).getObject(0).getValue() == 1 ) {
			if( %selectedFilter $= "" )
				%selectedFilter = MatBrowser-->tagFilters.getObject(%i).getObject(0).filter;
			else
				%selectedFilter = %selectedFilter @ " " @ MatBrowser-->tagFilters.getObject(%i).getObject(0).filter;
		}
	}

	MatBrowser.loadFilter( %selectedFilter );
}
//------------------------------------------------------------------------------
//==============================================================================
// Load the filtered materials (called also when thumbnail count change)
function MatBrowser::loadFilter( %this, %selectedFilter, %staticFilter ) {
	MatSel_ListFilterText.active = 0;

	// manage schedule array properly
	if(!isObject(MatEdScheduleArray))
		new ArrayObject(MatEdScheduleArray);

	// if we select another list... delete all schedules that were created by
	// previous load
	for( %i = 0; %i < MatEdScheduleArray.count(); %i++ )
		cancel(MatEdScheduleArray.getKey(%i));

	// we have to empty out the list; so when we create new schedules, these dont linger
	MatEdScheduleArray.empty();

	// manage preview array
	if(!isObject(MatEdPreviewArray))
		new ArrayObject(MatEdPreviewArray);

	// we have to empty out the list; so when we create new guicontrols, these dont linger
	MatEdPreviewArray.empty();
	MatBrowser-->materialSelection.deleteAllObjects();
	MatBrowser-->materialPreviewPagesStack.deleteAllObjects();

	// changed to accomadate tagging. dig through the array for each tag name,
	// call unique value, sort, and we have a perfect set of materials
	//if (!isObject(%staticFilter)){
	//%this.buildStaticFilters();
	//%staticFilter = MaterialFilterAllArray;
	//}
	if (isObject(%staticFilter))
		MatBrowser.currentStaticFilter = %staticFilter;

	if (!isObject(MatBrowser.currentStaticFilter)) {		
		MatBrowser.currentStaticFilter = MaterialFilterAllArray;
	}

	MatBrowser.currentFilter = %selectedFilter;
	%filteredObjectsArray = new ArrayObject();
	%previewsPerPage = MatBrowser-->materialPreviewCountPopup.getTextById( MatBrowser-->materialPreviewCountPopup.getSelected() );

	if (%previewsPerPage $= "All")
		%previewsPerPage = "999999";

	%tagCount = getWordCount( MatBrowser.currentFilter );

	if (!isObject(MatBrowser.currentStaticFilter))
		%this.buildStaticFilters();
//---------------------------------------------------------------------------
// There's filtered tags, update filters
	if( %tagCount != 0 ) {
		for( %j = 0; %j < %tagCount; %j++ ) {
			for( %i = 0; %i < MatBrowser.currentStaticFilter.count(); %i++ ) {
				%currentTag = getWord( MatBrowser.currentFilter, %j );

				if( MatBrowser.currentStaticFilter.getKey(%i) $= %currentTag)
					%filteredObjectsArray.add( MatBrowser.currentStaticFilter.getKey(%i), MatBrowser.currentStaticFilter.getValue(%i) );
			}
		}

		%filteredObjectsArray.uniqueValue();
		%filteredObjectsArray.sortd();
		
      %this.buildPages(%filteredObjectsArray);	

		%filteredObjectsArray.delete();
	} else {
//---------------------------------------------------------------------------
// There's NO filtered tags, update filters differently...
		MatBrowser.currentStaticFilter.sortd();
		// Rebuild the static filter list without tagged materials
		%noTagArray = new ArrayObject();

		for( %i = 0; %i < MatBrowser.currentStaticFilter.count(); %i++ ) {
			if( MatBrowser.currentStaticFilter.getKey(%i) !$= "")
				continue;

			%material = MatBrowser.currentStaticFilter.getValue(%i);

			// CustomMaterials are not available for selection
			if ( !isObject( %material ) || %material.isMemberOfClass( "CustomMaterial" ) )
				continue;

			%noTagArray.add( "", %material );
		}

		%this.buildPages(%noTagArray);	
	}

	MatBrowser.loadImages( 0 );
	MatSel_ListFilterText.active = 1;
}


function MatBrowser::clearMaterialFilters( %this ) {
	for( %i = MatBrowser.staticFilterObjCount; %i < MatBrowser-->filterArray.getCount(); %i++ )
		MatBrowser-->filterArray.getObject(%i).getObject(0).setStateOn(0);

	MatBrowser.loadFilter( "", "" );
}
//------------------------------------------------------------------------------
function MatBrowser::loadMaterialFilters( %this ) {
	%filteredTypesArray = new ArrayObject();
	%filteredTypesArray.duplicate( MaterialFilterAllArray );
	%filteredTypesArray.uniqueKey();
	// sort the the keys before we do anything
	%filteredTypesArray.sortkd();
	eval( MatBrowser.currentStaticFilter @ "Checkbox.setStateOn(1);" );
	// it may seem goofy why the checkbox can't be instanciated inside the container
	// reason being its because we need to store the checkbox ctrl in order to make changes
	// on it later in the function.
	%selectedFilter = "";

	for( %i = 0; %i < %filteredTypesArray.count(); %i++ ) {
		%filter = %filteredTypesArray.getKey(%i);

		if(%filter $= "")
			continue;

		%container = cloneObject(MatSelector_FilterSamples-->filterPillSample);
		%container.internalName = "CustomFilter";
		%checkbox = %container.getObject(0);
		%checkbox.text = %filter @ " ( " @ MaterialFilterAllArray.countKey(%filter) @ " )";
		%checkbox.filter = %filter;

		if (!isObject(%container)) {
			%container = new GuiControl() {
				profile = "ToolsDefaultProfile";
				Position = "0 0";
				Extent = "128 18";
				HorizSizing = "right";
				VertSizing = "bottom";
				isContainer = "1";
			};
			%checkbox = new GuiCheckBoxCtrl() {
				Profile = "ToolsCheckBoxProfile";
				position = "5 1";
				Extent = "118 18";
				Command = "";
				groupNum = "0";
				buttonType = "ToggleButton";
				text = %filter @ " ( " @ MaterialFilterAllArray.countKey(%filter) @ " )";
				filter = %filter;
				Command = "MatBrowser.preloadFilter();";
			};
			%container.add( %checkbox );
		}

		//%checkbox = %container.getObject(0);
		MatBrowser-->filterArray.add( %container );
		%tagCount = getWordCount( MatBrowser.currentFilter );

		for( %j = 0; %j < %tagCount; %j++ ) {
			if( %filter $= getWord( MatBrowser.currentFilter, %j ) ) {
				if( %selectedFilter $= "" )
					%selectedFilter = %filter;
				else
					%selectedFilter = %selectedFilter @ " " @ %filter;

				%checkbox.setStateOn(1);
			}
		}
	}

	MatBrowser.loadFilter( %selectedFilter );
	%filteredTypesArray.delete();
}
//------------------------------------------------------------------------------
// create category and update current material if there is one
function MatBrowser::createFilter( %this, %filter ) {
	if( %filter $= %existingFilters ) {
		MessageBoxOK( "Error", "Can not create blank filter.");
		return;
	}

	for( %i = MatBrowser.staticFilterObjCount; %i < MatBrowser-->filterArray.getCount() ; %i++ ) {
		%existingFilters = MatBrowser-->filterArray.getObject(%i).getObject(0).filter;

		if( %filter $= %existingFilters ) {
			MessageBoxOK( "Error", "Can not create two filters of the same name.");
			return;
		}
	}

	devLog("createFilter:",%filter);
	%container = cloneObject(MatSelector_FilterSamples-->filterPillSample);
	%container.internalName = "CustomFilter";
	%checkbox = %container.getObject(0);
	%checkbox.text = %filter @ " ( " @ MaterialFilterAllArray.countKey(%filter) @ " )";
	%checkbox.filter = %filter;

	if (!isObject(%container)) {
		%container = new GuiControl() {
			profile = "ToolsDefaultProfile";
			Position = "0 0";
			Extent = "128 18";
			HorizSizing = "right";
			VertSizing = "bottom";
			isContainer = "1";
			new GuiCheckBoxCtrl() {
				Profile = "ToolsCheckBoxProfile";
				position = "5 1";
				Extent = "118 18";
				Command = "";
				groupNum = "0";
				buttonType = "ToggleButton";
				text = %filter @ " ( " @ MaterialFilterAllArray.countKey(%filter) @ " )";
				filter = %filter;
				Command = "MatBrowser.preloadFilter();";
			};
		};
	}

	MatBrowser-->filterArray.add( %container );

	// if selection exists, lets reselect it to refresh it
	if( isObject(MatBrowser.selectedMaterial) )
		MatBrowser.updateSelection( MatBrowser.selectedMaterial, MatBrowser.selectedPreviewImagePath );

	// material category text field to blank
	MatBrowser_addFilterWindow-->tagName.setText("");
}


//------------------------------------------------------------------------------
function MatBrowser::updateFilterCount( %this, %tag, %add ) {
	for( %i = MatBrowser.staticFilterObjCount; %i < MatBrowser-->filterArray.getCount() ; %i++ ) {
		if( %tag $= MatBrowser-->filterArray.getObject(%i).getObject(0).filter ) {
			// Get the filter count and apply the operation
			%idx = getWord( MatBrowser-->filterArray.getObject(%i).getObject(0).getText(), 2 );

			if( %add )
				%idx++;
			else
				%idx--;

			MatBrowser-->filterArray.getObject(%i).getObject(0).setText( %tag @ " ( "@ %idx @ " )");
		}
	}
}

//------------------------------------------------------------------------------
function MatBrowser::switchStaticFilters( %this, %staticFilter) {
	switch$(%staticFilter) {
	case "MaterialFilterAllArray":
		MaterialFilterAllArrayCheckbox.setStateOn(1);
		MaterialFilterMappedArrayCheckbox.setStateOn(0);
		MaterialFilterUnmappedArrayCheckbox.setStateOn(0);

	case "MaterialFilterMappedArray":
		MaterialFilterMappedArrayCheckbox.setStateOn(1);
		MaterialFilterAllArrayCheckbox.setStateOn(0);
		MaterialFilterUnmappedArrayCheckbox.setStateOn(0);

	case "MaterialFilterUnmappedArray":
		MaterialFilterUnmappedArrayCheckbox.setStateOn(1);
		MaterialFilterAllArrayCheckbox.setStateOn(0);
		MaterialFilterMappedArrayCheckbox.setStateOn(0);
	}

	// kinda goofy were passing a class variable... we can't do an empty check right now
	// on load filter because we actually pass "" as a filter...
	MatBrowser.loadFilter( MatBrowser.currentFilter, %staticFilter );
}
