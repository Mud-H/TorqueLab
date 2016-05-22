//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
function EditorLoadingGui::onWake(%this) {
	%res = %this.getExtent();
	%resX = getWord(%res, 0);
	%resY = getWord(%res, 1);
	%dialog = %this-->Dialog;
	%dialogExtent = %dialog.getExtent();
	%dialogWidth = getWord(%dialogExtent, 0);
	%dialogHeight = getWord(%dialogExtent, 1);
	%dialogPostion = %dialog.getPosition();
	%posX = (%resX / 2) - (%dialogWidth / 2);
	%posY = (%resY / 2) - (%dialogHeight / 2);
	%dialog.setPosition(%posX, %posY);
}
//------------------------------------------------------------------------------

//==============================================================================
function EditorLoadingGui::startInit(%this) {
	canvas.pushDialog( EditorLoadingGui );
	canvas.repaint();
}
//------------------------------------------------------------------------------

//==============================================================================
function EditorLoadingGui::endInit(%this) {
	if (%this.isAwake())
		canvas.popDialog(EditorLoadingGui);
}
//------------------------------------------------------------------------------