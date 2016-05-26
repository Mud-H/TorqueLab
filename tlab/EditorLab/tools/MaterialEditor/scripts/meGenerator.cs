//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
// Based on MaterialEditor by Dave Calabrese and Travis Vroman of Gaslight Studios
//==============================================================================
$MEP_ShowMap["Diffuse"] = true;

//==============================================================================
function LabMat::generateMapPills(%this) {
   
   MEP_TextureMapStack.clear();
   foreach$(%map in $LabMat_TextureMaps){
      %pill = cloneObject(MEP_MapContainerSrc,"MEP_"@%map@"Container",%map,MEP_TextureMapStack);
      %bitmap = %pill-->bitmap;
      %bitmap.internalName = %map @ "MapDisplayBitmap";
      
      %bitmapType = %pill-->bitmapType;
      %bitmapType.command = "LabMat.updateTextureMap(\""@%map@"\", 1);";
      %bitmapType.internalName = %map;
      
      %fileName = %pill-->mapFileName;
      %fileName.internalName = %map @ "MapNameText";     
      
      %file = %pill-->mapFile;
      %file.internalName = %map @ "FileNameText";
      
      %pill-->mapName.text = %map@":"; 
           
      %colorTintSwatch = %pill-->colorTintSwatchSrc;
      %colorTintSwatch.internalName = "colorTintSwatch";
      %colorTitle = %pill-->colorTitle;
      if (%map !$= "Diffuse"){
         delObj(%colorTintSwatch);
         delObj(%colorTitle);
      }
      
      %pill-->editIcon.command = "LabMat.updateTextureMap(\""@%map@"\", 1);";
      %pill-->deleteIcon.command = "LabMat.updateTextureMap(\""@%map@"\", 0);";
      %pill.visible = $MEP_ShowMap[%map];
   }
}
//------------------------------------------------------------------------------
