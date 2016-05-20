//==============================================================================
// TorqueLab -> ETools - PhysicsTools
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
function EPhysicsTools::OnVisible(%this,%state) {
	if (%state)
		EPhysicsTools.forceInsideCtrl(EPhysicsTools.getParent());
}
//------------------------------------------------------------------------------
//==============================================================================
function EdContainerMain::onWake(%this,%state) {
	devLog("EdContainerMain::onWake(%this,%state)",%this,%state);
%this.fitIntoParents();
	%this.forceInsideCtrl(EdGuiWorldContainer);
}
//------------------------------------------------------------------------------

