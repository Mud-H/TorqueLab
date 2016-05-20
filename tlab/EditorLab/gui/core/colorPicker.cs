//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

$ColorPickerCallback = ""; // Control that we need to update
$ColorPickerCancelCallback = "";
$ColorPickerUpdateCallback = "";
$ColorCallbackType   = 1;  // ColorI



// This function pushes the color picker dialog and returns to a callback the selected value
function GetColorI( %currentColor, %callback, %root, %updateCallback, %cancelCallback )
{
	logd("GetColorI",%currentColor,"Callback",%callback);
   $ColorPickerSignal = 1; 
   $ColorPickerCallback = %callback;
   $ColorPickerCancelCallback = %cancelCallback;
   $ColorPickerUpdateCallback = %updateCallback;
   $ColorCallbackType = 1; // ColorI
   
   oldColor.color = ColorIntToFloat( %currentColor );
   myColor.color = ColorIntToFloat( %currentColor );
   
   ColorRangeSelect.showReticle = true;
   ColorBlendSelect.showReticle = true;
   
   // Set the range according to int
   ColorAlphaSelect.range = "0 255";
   
   // Set the RGBA displays accordingly
   %red = getWord(%currentColor, 0) / 255;
   %green = getWord(%currentColor, 1) / 255;
   %blue = getWord(%currentColor, 2) / 255;
   %alpha = getWord(%currentColor, 3);
   
   // set the initial range blend to correct color, no alpha needed
   // this should also set the color blend select right now
   ColorRangeSelect.baseColor = %red SPC %green SPC %blue SPC "1.0";
   ColorRangeSelect.updateColor();
   
   if(!isObject(%root))
      %root = Canvas;
  
   %root.pushDialog(ColorPickerDlg);
   
   // update the alpha value first
   ColorAlphaSelect.setValue( %alpha );
   Channel_A_Val.setText( %alpha );
}

function GetColorF( %currentColor, %callback, %root, %updateCallback, %cancelCallback )
{
	logd("GetColorF",%currentColor,"Callback",%callback);
   $ColorPickerSignal = 1; 
   $ColorPickerCallback = %callback;
   $ColorPickerCancelCallback = %cancelCallback;
   $ColorPickerUpdateCallback = %updateCallback;
   $ColorCallbackType = 2; // ColorF
   
   oldColor.color = %currentColor;  
   myColor.color = %currentColor;   
   
   ColorRangeSelect.showReticle = true;
   ColorBlendSelect.showReticle = true;
   
   // Set the range according to float
   ColorAlphaSelect.range = "0 1";
   
   // Set the RGBA displays accordingly
   %red = getWord(%currentColor, 0);
   %green = getWord(%currentColor, 1);
   %blue = getWord(%currentColor, 2);
   %alpha = getWord(%currentColor, 3);
   
   // set the initial range blend to correct color, no alpha needed
   // this should also set the color blend select right now
   ColorRangeSelect.baseColor = %red SPC %green SPC %blue SPC "1.0";
   ColorRangeSelect.updateColor();
   
   if(!isObject(%root))
      %root = Canvas;
   %root.pushDialog(ColorPickerDlg);
   
   // update the alpha value first
   ColorAlphaSelect.setValue( %alpha );
   Channel_A_Val.setText( %alpha );
}

// This function is used to update the text controls at the top
function setColorInfo()
{
   %red = Channel_R_Val.getValue();
   %green = Channel_G_Val.getValue();
   %blue = Channel_B_Val.getValue();
   
   if( $ColorCallbackType == 1)
   {
      %red = (%red / 255);
      %green = (%green / 255);
      %blue = (%blue / 255);
   }
      
   $ColorPickerSignal = 1; 
   
   ColorBlendSelect.baseColor = %red SPC %green SPC %blue SPC "1.0";
   ColorBlendSelect.updateColor();
}

// return mycolor.color
function DoColorPickerCallback()
{
   eval( $ColorPickerCallback @ "(\"" @ constructNewColor(mycolor.color, $ColorCallbackType) @"\");" );
   ColorPickerDlg.getRoot().popDialog(ColorPickerDlg);   
}   

function DoColorPickerCancelCallback()
{
   ColorPickerDlg.getRoot().popDialog( ColorPickerDlg );
   if( $ColorPickerCancelCallback !$= "" )
      eval( $ColorPickerCancelCallback @ "(\"" @ constructNewColor( oldColor.color, $ColorCallbackType ) @ "\");" );
}

function DoColorPickerUpdateCallback()
{
   if( $ColorPickerUpdateCallback !$= "" )
      eval( $ColorPickerUpdateCallback @ "(\"" @ constructNewColor( myColor.color, $ColorCallbackType ) @ "\");" );
}

// this is called from ColorRangeSelect.updateColor
function updatePickerBaseColor( %location )
{
   if( $ColorPickerSignal && %location )
      %pickColor = ColorRangeSelect.baseColor;
   else
      %pickColor = ColorRangeSelect.pickColor;
   $ColorPickerSignal = 1;
   
   %red = getWord(%pickColor, 0);
   %green = getWord(%pickColor, 1);
   %blue = getWord(%pickColor, 2);
   %alpha = getWord(%pickColor, 3);
   
   ColorBlendSelect.baseColor = %red SPC %green SPC %blue SPC "1.0";
   ColorBlendSelect.updateColor();
}

// this is called from ColorBlendSelect.updateColor
function updateRGBValues( %location )
{
   //update the color based on where it came from
   if( $ColorPickerSignal && %location )
      %pickColor = ColorBlendSelect.baseColor;
   else
      %pickColor = ColorBlendSelect.pickColor;
   
   //lets prepare the color
   %red = getWord(%pickColor, 0);
   %green = getWord(%pickColor, 1);
   %blue = getWord(%pickColor, 2);  
   //the alpha should be grabbed from mycolor
   %alpha = getWord(myColor.color, 3);    
     
   // set the color!
   myColor.color = %red SPC %green SPC %blue SPC %alpha ;
   
   DoColorPickerUpdateCallback();
   
   //update differently depending on type
   if( $ColorCallbackType == 1 )
   {
      %red = mCeil(%red * 255);
      %blue = mCeil(%blue * 255);
      %green = mCeil(%green * 255);
   }
   else
   {
      %red = mFloatLength(%red, 3);
      %blue = mFloatLength(%blue, 3);
      %green = mFloatLength(%green, 3);
   }
   
   // changes current color values
   Channel_R_Val.setValue(%red); 
   Channel_G_Val.setValue(%green);
   Channel_B_Val.setValue(%blue);
   
   $ColorPickerSignal = 0;
}

function updateColorPickerAlpha( %alphaVal )
{
   //lets prepare the color
   %red = getWord(myColor.color, 0);
   %green = getWord(myColor.color, 1);
   %blue = getWord(myColor.color, 2);  
   %alpha = %alphaVal;
   
   if( $ColorCallbackType == 1 )
      %alpha = (%alpha / 255);
   
   myColor.color = %red SPC %green SPC %blue SPC %alpha ;
   
   DoColorPickerUpdateCallback();
} 

function constructNewColor(%pickColor, %colorType )
{
   %red = getWord(%pickColor, 0);
   %green = getWord(%pickColor, 1);
   %blue = getWord(%pickColor, 2);
   %alpha = ColorAlphaSelect.getValue();
   
   // Update the text controls to reflect new color
   //setColorInfo(%red, %green, %blue, %alpha);
   if ( %colorType == 1 ) // ColorI
      return mCeil( %red * 255 ) SPC mCeil( %green * 255 ) SPC mCeil( %blue * 255 ) SPC %alpha;
   else // ColorF
      return %red SPC %green SPC %blue SPC %alpha;
}
