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
function LabObj::inspect(%this,%obj,%doApply) {
	LabInspect.inspect(%obj);

	if (%doApply)
		LabInspect.apply();
}
//------------------------------------------------------------------------------
//==============================================================================
function LabObj::update(%this,%obj,%field,%value,%fieldId) {
	logd("LabObj::update(%this,%obj,%field,%value,%fieldId)",%obj.getName(),%field,%value,%fieldId);
	LabInspect.inspect(%obj);

	if(%obj.isMemberOfClass(ArrayObject)) {
		%initialValue = %obj.getVal(%field);
		%obj.setVal(%field,%value);
	} else {
		%initialValue = %obj.getFieldValue(%field);

		if (%field $= "name")
			LabInspect.setObjectField(%field,%value);
		else if (%fieldId !$= "")
			%obj.setFieldValue(%field,%value,%fieldId);
		else
			%obj.setFieldValue(%field,%value);

		LabInspect.apply();
	}

	if (%initialValue !$= %value)
		return true;

	return false;
}
//------------------------------------------------------------------------------

//==============================================================================
// LabObj.set is same as update but will also add the object to Lab_PM for future save
function LabObj::set(%this,%obj,%field,%value,%fieldId) {
	%initialValue = %obj.getFieldValue(%field);
	%isDirty = %this.update(%obj,%field,%value,%fieldId);

	if (%isDirty) {
		Lab_PM.setDirty(%obj);
		//info(Lab_PM.getDirtyObjectCount()," objects are dirty in Lab_PM");
		return true;
	}

	return false;
}
//------------------------------------------------------------------------------

//==============================================================================
function LabObj::listDirty(%this) {
	Lab_PM.listDirty();
}
//------------------------------------------------------------------------------

//==============================================================================
function LabObj::saveAll(%this) {
	Lab_PM.saveDirty();
}
//------------------------------------------------------------------------------
//==============================================================================
function LabObj::isDirty(%this,%obj) {
	return Lab_PM.isDirty(%obj);
}
//------------------------------------------------------------------------------
//==============================================================================
function LabObj::save(%this,%obj,%force) {
	if (!Lab_PM.isDirty(%obj)) {
		if (%force) {
			info(%obj,"Object is not dirty but we will force it");
			Lab_PM.setDirty(%obj);
		} else {
			info(%obj,"Object is not dirty use %force = true to force it");
			return;
		}
	}

	Lab_PM.saveDirtyObject(%obj);
}
//------------------------------------------------------------------------------
//==============================================================================
function LabObj::setDirty(%this,%obj,%dirty) {
	%isDirty = Lab_PM.isDirty(%obj);

	if (!%isDirty && %dirty) {
		info(%obj,"Object is not dirty but we will force it");
		Lab_PM.setDirty(%obj);
	} else if (%isDirty && !%dirty) {
		info(%obj,"Removing dirty Object is not dirty but we will force it");
		Lab_PM.removeDirty(%obj);
	}
}
//------------------------------------------------------------------------------
//==============================================================================
// TorqueLab Global Inspector (EGlobalInspector GUI)
//==============================================================================
function LabInspect::update(%this,%obj,%field,%value,%fieldId) {
	%this.set(%obj,%field,%value,%fieldId);
}
//------------------------------------------------------------------------------
//==============================================================================
function LabInspect::set(%this,%obj,%field,%value,%fieldId) {
	LabInspect.inspect(%obj);
	%obj.setFieldValue(%field,%value,%fieldId);
	LabInspect.apply();
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::inspect(%this,%obj,%doApply) {
	LabInspect.inspect(%obj);

	if (%doApply)
		LabInspect.apply();
}
//------------------------------------------------------------------------------

//==============================================================================
function Lab::addInspect(%this,%obj) {
	LabInspect.addInspect(%obj);
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::apply(%this,%obj) {
	LabInspect.apply();
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::getInspectObject(%this) {
	return LabInspect.getInspectObject();
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::getNumInspectObjects(%this) {
	return LabInspect.getNumInspectObjects();
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::refreshInspect(%this) {
	LabInspect.refresh();
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::removeInspect(%this,%obj) {
	LabInspect.removeInspect(%obj);
}
//------------------------------------------------------------------------------

//==============================================================================
//LabObj.saveList($HLabProfile_DirtyGui_ToolsTabBookMain);
function LabObj::saveProfileList(%this,%list) {
	foreach$(%obj in %list) {
		if (!isObject(%obj))
			continue;

		Lab_PM.setDirty(%obj);
	}

	Lab_PM.saveDirty();
}
//------------------------------------------------------------------------------
//==============================================================================
//LabObj.saveList($HLabProfile_DirtyGui_ToolsTabBookMain);
function LabObj::removeField(%this,%obj,%field,%saveNow) {
   if (!isObject(%obj))
      return;
   Lab_PM.removeField(%obj,%field);
   Lab_PM.setDirty(%obj);
   if (%saveNow)
	   Lab_PM.saveDirtyObject(%obj);
}
//------------------------------------------------------------------------------
//DefineConsoleMethod( PersistenceManager, removeField, void, (const char * objName, const char * fieldName), , "(SimObject object, string fieldName)"
  //            "Remove a specific field from an object declaration.")
