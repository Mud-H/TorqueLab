//==============================================================================
// TorqueLab -> LabObj System
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
// TorqueLab universal SimObjects update (GuiInspector) and save (PersistenceManager)
//==============================================================================


//==============================================================================
//Editor Initialization callbacks
//==============================================================================

//==============================================================================
function pmUpdate(%obj,%field,%value,%fieldId)
{
	logd("pmUpdate(%obj,%field,%value,%fieldId)",%obj.getName(),%field,%value,%fieldId);

	if (!isObject(%obj))
		return false;

	if(%obj.isMemberOfClass(ArrayObject))
	{
		%initialValue = %obj.getVal(%field);
		%obj.setVal(%field,%value);
	}
	else
	{
		%initialValue = %obj.getFieldValue(%field);

		if (%fieldId !$= "")
			%obj.setFieldValue(%field,%value,%fieldId);
		else
			%obj.setFieldValue(%field,%value);

		//LabInspect.apply();
	}

	if (%initialValue !$= %value)
		return true;

	return false;
}
//------------------------------------------------------------------------------

//==============================================================================
// LabObj.set is same as update but will also add the object to Lab_PM for future save
function pmSet(%obj,%field,%value,%fieldId)
{
	%initialValue = %obj.getFieldValue(%field);
	%isDirty = pmUpdate(%obj,%field,%value,%fieldId);
	devLog("Set obj:",%obj,"Field",%field,"Value:",%value);

	if (!isObject(pmHelper))
		new PersistenceManager("pmHelper");

	$pmHelper = pmHelper;

	if (%isDirty)
	{
		pmHelper.setDirty(%obj);
		info(pmHelper.getDirtyObjectCount()," objects are dirty in pmHelper");
		return true;
	}

	return false;
}
//------------------------------------------------------------------------------

//==============================================================================
function pmListDirty()
{
	if (!isObject(pmHelper))
		return;

	pmHelper.listDirty();
}
//------------------------------------------------------------------------------

//==============================================================================
function pmSaveAll()
{
	if (!isObject(pmHelper))
		return;

	pmHelper.saveDirty();
}
//------------------------------------------------------------------------------
//==============================================================================
function pmIsDirtyObject(%obj)
{
	if (!isObject(pmHelper))
		return;

	return pmHelper.isDirty(%obj);
}
//------------------------------------------------------------------------------
//==============================================================================
function pmSaveObject(%obj,%force)
{
	if (!isObject(pmHelper))
		return;

	if (!pmHelper.isDirty(%obj))
	{
		if (%force)
		{
			info(%obj,"Object is not dirty but we will force it");
			pmHelper.setDirty(%obj);
		}
		else
		{
			info(%obj,"Object is not dirty use %force = true to force it");
			return;
		}
	}

	pmHelper.saveDirtyObject(%obj);
}
//------------------------------------------------------------------------------
//==============================================================================
function pmSetDirtyObject(%obj,%dirty)
{
	if (!isObject(pmHelper))
		return;

	%isDirty = pmHelper.isDirty(%obj);

	if (!%isDirty && %dirty)
	{
		info(%obj,"Object is not dirty but we will force it");
		pmHelper.setDirty(%obj);
	}
	else if (%isDirty && !%dirty)
	{
		info(%obj,"Removing dirty Object is not dirty but we will force it");
		pmHelper.removeDirty(%obj);
	}
}
//------------------------------------------------------------------------------
