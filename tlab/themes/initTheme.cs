//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
// ->Added Gui style support
// -->Delete Profiles when reloaded
// -->Image array store in Global
//==============================================================================
if ($Cfg_TLab_Theme $= "")
   $Cfg_TLab_Theme = "Laborean";
   
if ($LabThemeLoaded)
	return;

//Lab.loadScriptsProfiles();
%profilesPath = "tlab/themes/"@$Cfg_TLab_Theme@"/";
$Cfg_TLab_ThemePath = %profilesPath;

exec(%profilesPath@"Settings.cs");
exec(%profilesPath@"profileDefaults.cs");
exec(%profilesPath@"baseProfiles.cs");

//exec("tlab/gui/profiles/defaultProfiles.cs");
%filePathScript = %profilesPath@"*.prof.cs";

for(%file = findFirstFile(%filePathScript); %file !$= ""; %file = findNextFile(%filePathScript)) {
	exec( %file );
}

exec(%profilesPath@"editorProfiles.cs");
exec(%profilesPath@"inspectorProfiles.cs");

$LabThemeLoaded = true;

//execpattern("tlab/gui/profiles/styles/*.cs");
//Lab.initProfileStyleData();

//exec("tlab/gui/profiles/ColorPanel.prof.cs");





//==============================================================================
// TorqueLab Editor Initialization Function
//==============================================================================



//==============================================================================
function updateProfileFontColor(%colorSet) {
   %color = $Theme_Color[%colorSet];
   %list = $Theme_ColorProfiles[%colorSet];
   foreach$(%profData in %list){
      //First check if start with _, if so it's a deviation of previous profile
      %profile = %profdata;
      %start = getSubStr(%profData,0,1);
      if (%start $= "_")
      {
         if (%lastProfile $= "")
            continue;
         
         %profile = %lastProfile @ %profData;
      }
      else
         %lastProfile = %profData;
      
      if (!isObject(%profile))
         continue;
      
      %profile.setFieldValue("fontColor",%color);
      
   }
}
//------------------------------------------------------------------------------