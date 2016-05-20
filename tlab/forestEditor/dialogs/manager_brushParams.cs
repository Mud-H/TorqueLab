//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
//SEP_ScatterSkyManager.buildParams();
function FEP_Manager::buildBrushParams( %this ) {
	%arCfg = Lab.createBaseParamsArray("FEP_Brush",FEP_ManagerBrushProperties);
	%arCfg.updateFunc = "FEP_Manager.updateSimGroupParam";
	%arCfg.style = "StyleA";
	%arCfg.useNewSystem = true;
	%arCfg.group[%gid++] = "Group/brush settings (WIP)" TAB "Stack GroupStack";
	%arCfg.setVal("groupScaleScalar",       "" TAB "Group Scale Scalar" TAB "SliderEdit" TAB "range>>0 100" TAB "FEP_Manager.selectedBrushGroup" TAB %gid);
	%arCfg.setVal("groupSinkScalar",        "" TAB "Group Sink Scalar" TAB "SliderEdit" TAB "range>>0 100" TAB "FEP_Manager.selectedBrushGroup" TAB %gid);
	%arCfg.setVal("groupElevationScalar",   "" TAB "Group Elevation Scalar" TAB "SliderEdit" TAB "range>>0 100" TAB "FEP_Manager.selectedBrushGroup" TAB %gid);
	%arCfg.setVal("groupSlopeScalar",        "" TAB "Group Slope Scalar" TAB "SliderEdit" TAB "range>>0 100" TAB "FEP_Manager.selectedBrushGroup" TAB %gid);
	//%arCfg.group[%gid++] = "Brush settings" TAB "Stack BrushStack";
	%arCfg.setVal("brushScaleScalar",       "" TAB "Brush Scale Scalar" TAB "SliderEdit" TAB "range>>0 100" TAB "FEP_Manager.selectedBrush" TAB %gid);
	%arCfg.setVal("brushSinkScalar",        "" TAB "Brush Sink Scalar" TAB "SliderEdit" TAB "range>>0 100" TAB "FEP_Manager.selectedBrush" TAB %gid);
	%arCfg.setVal("brushElevationScalar",   "" TAB "Brush Elevation Scalar" TAB "SliderEdit" TAB "range>>0 100" TAB "FEP_Manager.selectedBrush" TAB %gid);
	%arCfg.setVal("brushSlopeScalar",        "" TAB "Brush Slope Scalar" TAB "SliderEdit" TAB "range>>0 100" TAB "FEP_Manager.selectedBrush" TAB %gid);
	buildParamsArray(%arCfg,false);
	%this.brushParamArray = %arCfg;
}
//------------------------------------------------------------------------------
