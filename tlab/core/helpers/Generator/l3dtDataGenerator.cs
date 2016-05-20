//==============================================================================
// GameLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//-----------------------------------------------------------------------------
// Load up our main GUI which lets us see the game.

//==============================================================================
/// Generate forest brush elements and items from models found in specified folder
/// %baseFolder : Folder containing the models used for forest items
/// %name : Name of the root brush group containing generated brushes
/// %prefix : Prefix added to generated brush name (usefull to use brush filter)
/// %settingContainer : GuiControl containing children with setting as internalName
function generateL3DTMaterialFiles(%folder)
{
	if (%folder $= "")
		%folder = "art/terrains/l3dt/";

	foreach(%mat in TerrainMaterialSet)
	{
		%l3dtFile = %folder @ %mat.internalName @".mat.xml";
		generateL3DTMaterialFile(%mat,%l3dtFile);
	}
}
function generateL3DTMaterialFile(%mat,%file)
{
	%ext = ".png";
	%detail = fileBase(%mat.detailMap)@%ext;
	%normal = fileBase(%mat.normalMap)@%ext;
	%fileWrite = getFileWriteObj(%file);
	%line[%lc++] = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
	%line[%lc++] = "<varlist name=\"MaterialInfo\">";
	%line[%lc++] = "	<string name=\"FileVer\">1.0</string>";
	%line[%lc++] = "	<string name=\"MaterialName\">"@%mat.internalName@"</string>";
	%line[%lc++] = "	<varlist name=\"ColMod\">";
	%line[%lc++] = "		<bool name=\"Enabled\">false</bool>";
	%line[%lc++] = "		<float name=\"r\">1</float>";
	%line[%lc++] = "		<float name=\"g\">1</float>";
	%line[%lc++] = "		<float name=\"b\">1</float>";
	%line[%lc++] = "	</varlist>";
	%line[%lc++] = "	<varlist name=\"Strata\">";
	%line[%lc++] = "		<bool name=\"Enabled\">false</bool>";
	%line[%lc++] = "		<string name=\"FileName\">desert26\\SandStrata1.png</string>";
	%line[%lc++] = "		<float name=\"VertScale\">1</float>";
	%line[%lc++] = "		<float name=\"Strength\">1</float>";
	%line[%lc++] = "	</varlist>";
	%line[%lc++] = "	<varlist name=\"SpecLM\">";
	%line[%lc++] = "		<float name=\"Strength\">0.1</float>";
	%line[%lc++] = "	</varlist>";
	%line[%lc++] = "	<varlist name=\"BumpMap\">";
	%line[%lc++] = "		<bool name=\"Enabled\">true</bool>";
	%line[%lc++] = "		<float name=\"Strength\">0.2</float>";
	%line[%lc++] = "	</varlist>";
	%line[%lc++] = "	<varlist name=\"Splat\">";
	%line[%lc++] = "		<bool name=\"AutoBase\">true</bool>";
	%line[%lc++] = "		<bool name=\"AutoNormal\">true</bool>";
	%line[%lc++] = "		<string name=\"DetailFile\">GreenCountry\\materials\\grass\\grass1_t3d_detail.jpg</string>";
	%line[%lc++] = "	</varlist>";
	%line[%lc++] = "	<varlist name=\"TextureLayers\">";
	%line[%lc++] = "		<varlist>";
	%line[%lc++] = "			<int name=\"LayerRes\">1</int>";
	%line[%lc++] = "			<varlist name=\"Texture\">";
	%line[%lc++] = "				<string name=\"FileName\">"@%detail@"</string>";
	%line[%lc++] = "				<float name=\"Weight\">0.6</float>";
	%line[%lc++] = "				<string name=\"BlendMode\">ADD</string>";
	%line[%lc++] = "			</varlist>";
	%line[%lc++] = "			<varlist name=\"BumpMap\">";
	%line[%lc++] = "				<string name=\"FileName\">"@%normal@"</string>";
	%line[%lc++] = "				<float name=\"Weight\">1</float>";
	%line[%lc++] = "			</varlist>";
	%line[%lc++] = "			<varlist name=\"ColMod\">";
	%line[%lc++] = "				<bool name=\"Enabled\">false</bool>";
	%line[%lc++] = "				<float name=\"r\">1</float>";
	%line[%lc++] = "				<float name=\"g\">1</float>";
	%line[%lc++] = "				<float name=\"b\">1</float>";
	%line[%lc++] = "			</varlist>";
	%line[%lc++] = "		</varlist>";
	%line[%lc++] = "	</varlist>";
	%line[%lc++] = "</varlist>";

	while(%line[%inc++] !$= "")
	{
		%fileWrite.writeLine(%line[%inc]);
	}

	closeFileObj(%fileWrite);
}
/*
<?xml version=\"1.0\" encoding=\"utf-8\"?>
<varlist name=\"MaterialInfo\">
	<string name=\"FileVer\">1.0</string>
	<string name=\"MaterialName\">gvGrass01</string>
	<varlist name=\"ColMod\">
		<bool name=\"Enabled\">false</bool>
		<float name=\"r\">1</float>
		<float name=\"g\">1</float>
		<float name=\"b\">1</float>
	</varlist>
	<varlist name=\"Strata\">
		<bool name=\"Enabled\">false</bool>
		<string name=\"FileName\">desert26\SandStrata1.png</string>
		<float name=\"VertScale\">1</float>
		<float name=\"Strength\">1</float>
	</varlist>
	<varlist name=\"SpecLM\">
		<float name=\"Strength\">0.1</float>
	</varlist>
	<varlist name=\"BumpMap\">
		<bool name=\"Enabled\">true</bool>
		<float name=\"Strength\">0.2</float>
	</varlist>
	<varlist name=\"Splat\">
		<bool name=\"AutoBase\">true</bool>
		<bool name=\"AutoNormal\">true</bool>
		<string name=\"DetailFile\">GreenCountry\materials\grass\grass1_t3d_detail.jpg</string>
	</varlist>
	<varlist name=\"TextureLayers\">
		<varlist>
			<int name=\"LayerRes\">1</int>
			<varlist name=\"Texture\">
				<string name=\"FileName\">GreenValley\Grass\gvGrass01_c.png</string>
				<float name=\"Weight\">0.6</float>
				<string name=\"BlendMode\">ADD</string>
			</varlist>
			<varlist name=\"BumpMap\">
				<string name=\"FileName\">GreenValley\Grass\gvGrass01_A_n.png</string>
				<float name=\"Weight\">1</float>
			</varlist>
			<varlist name=\"ColMod\">
				<bool name=\"Enabled\">false</bool>
				<float name=\"r\">1</float>
				<float name=\"g\">1</float>
				<float name=\"b\">1</float>
			</varlist>
		</varlist>
	</varlist>
</varlist>*/