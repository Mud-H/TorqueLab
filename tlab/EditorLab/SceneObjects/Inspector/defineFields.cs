//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
// From: GuiInspector::addInspectObject check for custom field for inspect object
//==============================================================================
//==============================================================================

//==============================================================================
function LevelInfo::onDefineFieldTypes( %this ) {
	%this.setFieldType("Desc", "TypeString");
	%this.setFieldType("DescLines", "TypeS32");
	%this.setFieldType("testCommand", "TypeCommand");
	%this.setFieldType("MatTest", "TypeMaterialName");
}
//------------------------------------------------------------------------------
//==============================================================================
function SimObject::onDefineFieldTypes( %this ) {
	%this.setFieldType("Locked", "TypeBool");
}
//------------------------------------------------------------------------------

//==============================================================================
// List of available types references (from code: consoleTypes.h)
/*
// Define Core Console Types
DefineConsoleType( TypeBool, bool )
DefineConsoleType( TypeBoolVector, Vector<bool>)
DefineConsoleType( TypeS8,  S8 )
DefineConsoleType( TypeS32, S32 )
DefineConsoleType( TypeS32Vector, Vector<S32> )
DefineConsoleType( TypeF32, F32 )
DefineConsoleType( TypeF32Vector, Vector<F32> )
DefineUnmappedConsoleType( TypeString, const char * ) // plain UTF-8 strings are not supported in new interop
DefineConsoleType( TypeCaseString, const char * )
DefineConsoleType( TypeRealString, String )
DefineConsoleType( TypeCommand, String )
DefineConsoleType( TypeFilename, const char * )
DefineConsoleType( TypeStringFilename, String )

/// A universally unique identifier.
DefineConsoleType( TypeUUID, Torque::UUID )

/// A persistent ID that is associated with an object.  This type cannot
/// be used to reference PIDs of other objects.
DefineUnmappedConsoleType( TypePID, SimPersistID* );

/// TypeImageFilename is equivalent to TypeStringFilename in its usage,
/// it exists for the benefit of GuiInspector, which will provide a custom
/// InspectorField for this type that can display a texture preview.
DefineConsoleType( TypeImageFilename, String )

/// TypePrefabFilename is equivalent to TypeStringFilename in its usage,
/// it exists for the benefit of GuiInspector, which will provide a 
/// custom InspectorField for this type.
DefineConsoleType( TypePrefabFilename, String )

/// TypeShapeFilename is equivalent to TypeStringFilename in its usage,
/// it exists for the benefit of GuiInspector, which will provide a 
/// custom InspectorField for this type.
DefineConsoleType( TypeShapeFilename, String )

/// TypeMaterialName is equivalent to TypeRealString in its usage,
/// it exists for the benefit of GuiInspector, which will provide a 
/// custom InspectorField for this type.
DefineConsoleType( TypeMaterialName, String )

/// TypeTerrainMaterialIndex is equivalent to TypeS32 in its usage,
/// it exists for the benefit of GuiInspector, which will provide a 
/// custom InspectorField for this type.
DefineConsoleType( TypeTerrainMaterialIndex, S32 )

/// TypeTerrainMaterialName is equivalent to TypeString in its usage,
/// it exists for the benefit of GuiInspector, which will provide a 
/// custom InspectorField for this type.
DefineConsoleType( TypeTerrainMaterialName, const char * )

/// TypeCubemapName is equivalent to TypeRealString in its usage,
/// but the Inspector will provide a drop-down list of CubemapData objects.
DefineConsoleType( TypeCubemapName, String )

DefineConsoleType( TypeParticleParameterString, const char * )

DefineConsoleType( TypeFlag, S32 )
DefineConsoleType( TypeColorI, ColorI )
DefineConsoleType( TypeColorF, ColorF )
DefineConsoleType( TypeSimObjectName, SimObject* )
DefineConsoleType( TypeShader, GFXShader * )

/// A persistent reference to an object.  This reference indirectly goes
/// through the referenced object's persistent ID.
DefineConsoleType( TypeSimPersistId, SimPersistID* )

/// Special field type for SimObject::objectName
DefineConsoleType( TypeName, const char* )

/// TypeRectUV is equivalent to TypeRectF in its usage,
/// it exists for the benefit of GuiInspector, which will provide a 
/// custom InspectorField for this type.
DefineConsoleType( TypeRectUV, RectF )
*/