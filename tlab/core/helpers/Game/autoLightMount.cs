//==============================================================================
// Boost! -> GuiControl Functions Helpers
// Copyright NordikLab Studio, 2013
//==============================================================================
//==============================================================================
// Schedule global on-off - Used to limit output of fast logs
//==============================================================================
$HLab_AutoLightFields = "brightness radius attenuationRatio flareType flareScale castShadows shadowType texSize";



//==============================================================================
// WORLD / SCREEN POSITIONING HELPERS
//==============================================================================
//==============================================================================
// Get the Screen coords for a 3D position
function mountLightOnShape(%shape,%node)
{
	%light = new PointLight()
	{
		radius = "20";
		isEnabled = "1";
		color = "0.996078 0.996078 0.478431 1";
		brightness = "4";
		castShadows = "0";
		staticRefreshFreq = "250";
		dynamicRefreshFreq = "8";
		priority = "1";
		animate = "1";
		animationPeriod = "1";
		animationPhase = "1";
		flareScale = "1";
		attenuationRatio = "0 1 1";
		shadowType = "DualParaboloidSinglePass";
		texSize = "512";
		overDarkFactor = "2000 1000 500 100";
		shadowDistance = "400";
		shadowSoftness = "0.15";
		numSplits = "1";
		logWeight = "0.91";
		fadeStartDistance = "0";
		lastSplitTerrainOnly = "0";
		representedInLightmap = "0";
		shadowDarkenColor = "0 0 0 -1";
		includeLightmappedGeometryInShadow = "0";
		position = "0 0 3.2";
		rotation = "1 0 0 0";
		canSave = "1";
		canSaveDynamicFields = "1";
		byGroup = "0";
	};
	MissionCleanup.add(%light);
}
//------------------------------------------------------------------------------

function regenLightShapes(%delOnly)
{
	foreach(%obj in AutoLightsSet)
	{
		%class = %obj.getClassName();

		if (%class $= "TSStatic")
		{
			%staticList = strAddWord(%staticList,%obj.getId(),true);
			continue;
		}

		if (!strFind(%class,"Light"))
			continue;

		%lightList = strAddWord(%lightList,%obj.getId(),true);
	}


	foreach$(%lightId in %lightList)
		delObj(%lightId);

	if (%delOnly)
		return;


	foreach$(%obj in %staticList)
	{
		AddLights(%obj);
	}
}

function rebuildLightShapes(%reset, %group)
{
	if (%reset && isObject(AutoMountLights))
		AutoMountLights.deleteAllObjects();

	//if (isObject(%group))
	checkLightShapes(%group);
}



function checkLightShapes(%group, %depth )
{
	if (%depth > 20)
	{
		return;
	}

	if (!isObject(%group))
		%group = mgLightShapes;

	if (!isObject(%group))
		return;

	foreach(%obj in %group)
	{
		if (%obj.isMemberOfClass("TSStatic"))
			AddLights(%obj);
		else if (%obj.isMemberOfClass("SimSet"))
			checkLightShapes(%obj,%depth++);
	}
}
function storeAutoLightRef(%group, %depth )
{
	%fieldList = "brightness radius attenuationRatio flareType flareScale castShadows shadowType texSize";

	if (%depth > 20)
	{
		return;
	}

	if (!isObject(%group))
		%group = mgLightShapes;

	if (!isObject(%group))
		return;

	foreach(%obj in %group)
	{
		if (%obj.isMemberOfClass("TSStatic"))
		{
			if (isObject(%obj.refLight))
			{
				%light = %obj.refLight;

				foreach$(%field in $HLab_AutoLightFields)
				{
					%value = %light.getFieldValue(%field);
					%stored = true;
					%obj.setFieldValue(%field,%value);
					//LabObj.update(%obj,%field,%value);
				}
			}
		}
		else if (%obj.isMemberOfClass("SimSet"))
			storeAutoLightRef(%obj,%depth++);
	}

	if (%stored)
		LabObj.saveAll();
}


function checkMountedLights(  )
{
	if (!isObject(AutoMountLights))
		return;

	foreach(%obj in AutoMountLights)
	{
		if (!isObject(%obj.myShape))
			%removeList = strAddWord(%removeList,%obj.getId());
	}

	foreach$(%id in %removeList)
		delObj(%id);
}
function AddGroupLights( %group )
{
	foreach(%obj in %group)
	{
		AddLights(%obj);
	}
}

function AddLights( %obj )
{
	if (!isObject(MissionGroup))
	{
		warnLog("Can't add lights, no MissionGroup found.");
		return;
	}

	if (!isObject(%obj))
	{
		warnLog("Can't add lights, invalid object supplied:",%obj);
		return;
	}

	if (%obj.refLight !$= "" && !isObject(%obj.refLight))
		%obj.refLight = "";

	// Get a TSShapeConstructor for this object (use the ShapeLab
	// utility functions to create one if it does not already exist).
	%shapePath = getObjectShapeFile( %obj );
	%shape = findConstructor( %shapePath );

	if ( !isObject( %shape ) )
		%shape = createConstructor( %shapePath );

	if ( !isObject( %shape ) )
	{
		echo( "Failed to create TSShapeConstructor for " @ %obj.getId() );
		return;
	}

	%lightsGroup = "Lights";

	if ( %shape.getNodeIndex( %lightsGroup ) $= "-1" )
		return;

	%objTransform = %obj.getTransform();

	if (!isObject(AutoMountLights))
	{
		newSimGroup("AutoMountLights");
	}

	if (!isObject(AutoLightsSet))
	{
		newSimSet("AutoLightsSet");
	}

	if (!isObject(mgLightShapes))
	{
		newSimGroup("mgLightShapes");
	}

	if (!isObject(mgAutoLightGroup))
	{
		newSimGroup("mgAutoLightGroup");
	}

	//%addTo = MissionGroup;
	//if (isObject(mgLights))
	//%addTo = mgLights;
	MissionGroup.add(mgAutoLightGroup);
	mgAutoLightGroup.add(	AutoMountLights );
	mgAutoLightGroup.add(	mgLightShapes );

	if (%obj.name $= "")
	{
		%name = getUniqueName("LightShape_");
		%obj.setName(%name);
	}

	%objParent = %obj.parentGroup;

	if (!isObject(%objParent))
	{
		%subGroup = mgLightShapes;
	}
	else
	{
		%subGroupInt = %objParent.internalName;

		if(%subGroupInt $= "")
			%subGroupInt = %objParent.getName();

		if(%subGroupInt $= "")
			%subGroupInt = %objParent.getId();

		%subGroup = mgLightShapes.findObjectByInternalName(%subGroupInt,true);

		if (!isObject(%subGroup))
		{
			%subGroup = new  SimGroup([%subGroupInt]);
		}

		mgLightShapes.add(%subGroup);
	}

	%lightCreated = false;
	%shapeName = fileBase(%obj.shapeName);
	%count = %shape.getNodeChildCount( %lightsGroup );

	for ( %i = 0; %i < %count; %i++ )
	{
		// get node transform in object space, then transform to world space
		%child = %shape.getNodeChildName( %lightsGroup, %i );
		%txfm = %shape.getNodeTransform( %child, true );
		%txfm = MatrixMultiply( %objTransform, %txfm );

		if (isObject(%obj.myLight[%i]))
		{
			%light = %obj.myLight[%i];
			%light.position = getWords( %txfm, 0, 2 );
			%light.rotation = getWords( %txfm, 3, 6 );
		}
		else
		{
			// create a new light at the object node
			%light = new PointLight()
			{
				position = getWords( %txfm, 0, 2 );
				rotation = getWords( %txfm, 3, 6 );
				radius = "20";
				isEnabled = "1";
				color = "0.996078 0.996078 0.478431 1";
				brightness = "1";
				shadowType = "DualParaboloidSinglePass";
				texSize = "512";
			};
			%light.myShape = %obj.getName();
			%light.constructObj = %shape;
			%obj.myLight[%i] = %light;
			%light.internalName = %shapeName;
			%light.myNodeId = %i;
		}

		if (!isObject(%obj.refLight))
			%obj.refLight = %light;

		AutoMountLights.add( %light );
		AutoLightsSet.add( %light );
		%lightCreated = true;

		foreach$(%field in $HLab_AutoLightFields)
		{
			%value = %obj.getFieldValue(%field);

			if (%value $= "")
				continue;

			%light.setFieldValue(%field,%value);
		}
	}

	if(%lightCreated)
	{
		AutoLightsSet.add(%obj);
		%subGroup.add(%obj);
	}
}
/*
new PointLight()
{
   radius = "12";
   color = "0.996078 0.996078 0.478431 1";
   brightness = "0.72";
   shadowType = "DualParaboloidSinglePass";
   numSplits = "1";
   position = "-72.5 -136.7 37.7784";
   internalName = "StreetLightPoleA";
   exposure = "15";
   constructObj = "33816";
   byGroup = "0";
   myNodeId = "0";
   myShape = "LightShape573";
};
*/
