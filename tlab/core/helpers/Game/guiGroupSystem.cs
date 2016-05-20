//==============================================================================
// HelperLab -> Scripted GuiGroup system
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
// Allow to update multiple GUIs sharing same GuiGroup at same time
// Usage:
// Add a guiGroup field with a value representing a data which is used by others
// To update a GuiControl with the same GuiGroup all at once, use doGuiGroupAction
// function in which you can specified multiple action to be called on all controls
// in the GuiGroup. Actions can be a function ("setValue(100);" or a value("value = 100;")
//==============================================================================

//==============================================================================
// Called each time a GuiControl is loaded
function GuiControl::onAdd	(%this)
{
	//Check if want to assign to a guiGroup
	if (%this.guiGroup !$= "" )
		addCtrlToGuiGroup(%this);

	if (!isObject(%this.profile))
	{
		warnLog(%this.getName(),%this.getClassName()," have an invalid profile:",%this.profile);
	}
}
//------------------------------------------------------------------------------

//==============================================================================
/// Add a GuiControl to a GuiGroup. Called in general from onAdd method but can
/// be used to add dynamically from script a Ctrl to a group
/// %ctrl: The control to add to the GuiGroup
/// %guiGroup(opt.): The GuiGroup to which the control will be assigned. If no
///      GuiGroup specified, it must be set in the ctrl already.
function addCtrlToGuiGroup(%ctrl,%guiGroup)
{
	//Leave if invalid object
	if (!isObject(%ctrl)) return;

	//Assign GuiGroup to control if specified
	if (%guiGroup !$= "")
		%ctrl.guiGroup = %guiGroup;

	//Exit if not guiGroup found
	if (%ctrl.guiGroup $= "") return;

	//Get the SimSet GuiGroup object or create if inexistant
	%group = $GuiGroup_[%ctrl.guiGroup];

	if (!isObject(%group))
	{
		%group = newSimSet("GuiGroup_"@%ctrl.guiGroup,$Group_GuiGroup);
	}
	//Return if the ctrl is already part of the group
	else if (%group.getObjectIndex(%ctrl) >= 0)
	{
		//warnLog("Object:",%ctrl.getName(),"is already in the group:",%ctrl.guiGroup);
		return;
	}

	//Add ctrl to the GuiGroup
	%group.add(%ctrl);
}
//------------------------------------------------------------------------------
//==============================================================================
/// Get the list of the Control IDs contained in a group.
/// %groupType: name of the GuiGroup to be searched
/// Return: List of Ctrl IDs seperated with space
function getGuiGroupList(%groupType)
{
	//logd("addCtrlToGuiGroup(%ctrl)",%ctrl);
	%group = $GuiGroup_[%groupType];

	if (!isObject(%group))
	{
		//warnlog("Try to call a GuiGroup action on invalid group:",%groupType);
		echo("Try to call a get ctrl list for invalid GuiGroup:" SPC %groupType);
		return "";
	}

	foreach(%ctrl in %group)
	{
		%list = strAddWord(%list,%ctrl.getId());
	}

	return %list;
}
//------------------------------------------------------------------------------
//==============================================================================
// Call an action on all GuiControl in a GuiGroup
function doGuiGroupAction(%groupType,%act1,%act2,%act3,%act4,%act5,%act6,%act7,%act8,%act9,%act10)
{
	//loge("doGuiGroupAction(%groupType,%act1,%act2,%act3,%act4,%act5,%act6,%act7,%act8,%act9,%act10)",%groupType,%act1,%act2,%act3,%act4,%act5,%act6,%act7,%act8,%act9,%act10);
	%group = $GuiGroup_[%groupType];

	if (!isObject(%group))
	{
		//warnlog("Try to call a GuiGroup action on invalid group:",%groupType);
		echo("Try to call a GuiGroup action on invalid group:" SPC %groupType);
		return;
	}

	foreach(%ctrl in %group)
	{
		%id = 1;

		while (%act[%id] !$="")
		{
			if (!strFind(%act[%id],";"))
			{
				%act[%id] = %act[%id]@";";
			}

			eval("%ctrl."@%act[%id]);
			%id++;
		}
	}

	return true;
}
//------------------------------------------------------------------------------
