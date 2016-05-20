//==============================================================================
// HelpersLab -> MissionGroup related helpers
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// Update the GuiControl data field depending of the Class
//==============================================================================

//==============================================================================
// SEP_GroundCover.getMissionGroundCover();
function getLabMissionObjectClassList( %classes,%condition )
{
	%list = checkMissionSimGroupForClass(MissionGroup,%classes,%condition);
	return %list;
}
//------------------------------------------------------------------------------
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function checLabMissionSimGroupForClass(%group,%classes,%condition )
{
	foreach(%obj in %group)
	{
		if (strFind(%classes,%obj.getClassname()))
		{
			if (%condition !$="")
			{
				%rejected = "";
				%condField = getWord(%condition,0);
				%condValue = trim(removeWord(%condition,0));
				%objValue = %obj.getFieldValue(%condField);
				devLog("ConValue",%condValue,"ObjValue",%objValue);

				if (%condValue $="" && %objValue !$= "")
					%rejected = "Obj value is not empty";
				else if (%condValue $= "NotEmpty" && %objValue $= "")
					%rejected = "Obj Value is empty";
				else if (%condValue $="isObject" && !isObject(%objValue ))
					%rejected = "ObjValue is not an object";
				else if (%condValue !$=%objValue)
					%rejected = "ObjValue don't match the condition";
			}

			if (%rejected $= "")				
				%list = strAddWord(%list,%obj.getId());
		}
		else if (%obj.getClassname() $= "SimGroup")
		{
			%listAdd = checkMissionSimGroupForClass(%obj,%classes);
			%list = strAddWord(%list,%listAdd);
		}
	}

	return %list;
}
//------------------------------------------------------------------------------


//==============================================================================
// Update the GuiControl data field depending of the Class
//==============================================================================
//----------------------------------------------------------------------------
// A function used in order to easily parse the MissionGroup for classes . I'm pretty
// sure at this point the function can be easily modified to search the any group as well.
function parseMissionGroup( %className, %childGroup )
{
	if( getWordCount( %childGroup ) == 0)
		%currentGroup = "MissionGroup";
	else
		%currentGroup = %childGroup;

	for(%i = 0; %i < (%currentGroup).getCount(); %i++)
	{
		if( (%currentGroup).getObject(%i).getClassName() $= %className )
			return true;

		if( (%currentGroup).getObject(%i).getClassName() $= "SimGroup" )
		{
			if( parseMissionGroup( %className, (%currentGroup).getObject(%i).getId() ) )
				return true;
		}
	}
}

// A variation of the above used to grab ids from the mission group based on classnames
function parseMissionGroupForIds( %className, %childGroup )
{
	if( getWordCount( %childGroup ) == 0)
		%currentGroup = "MissionGroup";
	else
		%currentGroup = %childGroup;

	for(%i = 0; %i < (%currentGroup).getCount(); %i++)
	{
		if( (%currentGroup).getObject(%i).getClassName() $= %className )
			%classIds = %classIds @ (%currentGroup).getObject(%i).getId() @ " ";

		if( (%currentGroup).getObject(%i).getClassName() $= "SimGroup" )
			%classIds = %classIds @ parseMissionGroupForIds( %className, (%currentGroup).getObject(%i).getId());
	}

	return %classIds;
}

//==============================================================================
// Mission AmbientGroup System
//==============================================================================

function setAmbientGroupId( %id )
{
	logd("setAmbientGroupId( %id )",%id);

	if (!isObject(mgAmbientGroup))
	{
		return;
	}

	if (%id $= "")
		%id = 0;

	foreach(%group in mgAmbientGroup)
	{
		foreach(%obj in %group)
			%obj.hidden = "1";
	}

	%curGroup = mgAmbientGroup.getObject(%id);

	if (!isObject(%curGroup))
	{
		%curGroup = mgAmbientGroup.getObject(0);
		%id = "0";
	}

	foreach(%obj in %curGroup)
		%obj.hidden = "0";

	SEP_AmbientManager.curAmbientId = %id;
	%name = %curGroup.internalName;

	if (%name $= "")
		%name = "Unnamed ID " @ %id;

	$Mission_AmbientGroup = %name;
	$Mission_AmbientGroupId = %id;
}
//------------------------------------------------------------------------------