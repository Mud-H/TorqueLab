//==============================================================================
// TorqueLab -> TerrainMaterialManager
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
function TMG::selectL3DTAlphaXml(%this) {
	%currentFile = TMG.dataFolder;
	//Canvas.cursorOff();
	getLoadFilename("*.xml|*.*", "TMG.setL3DTAlphaXml", %currentFile);
}
//------------------------------------------------------------------------------

//==============================================================================
function TMG::setL3DTAlphaXml(%this,%file) {
	%fileCln = makeRelativePath(%file);
	%fileCln = strreplace(%fileCln,TMG.dataFolder,"");
	TMG_PageMaterialLayers-->l3dtXMLText.text = %fileCln;
	%this.readL3DTAlphaXml(%file);
}
//------------------------------------------------------------------------------
//==============================================================================
function TMG::readL3DTAlphaXml(%this,%filename) {
	if (%filename $= "")
		%filename = $LastXML;
	else
		$LastXML = %filename;

	if (!isFile(%filename))
		return;

	%baseFolder = filePath(%filename);
	%xml = new SimXMLDocument() {};
	%xml.loadFile(%filename);
	%xml.pushChildElement("AlphaData");
	%xml.pushFirstChildElement("Width");
	%width = %xml.getText();
	%xml.popElement();
	%xml.pushFirstChildElement("Height");
	%height = %xml.getText();
	%xml.popElement();
	%xml.pushFirstChildElement("LayersPerImage");
	%channels = %xml.getText();
	%xml.popElement();
	TMG.customMapList = "";
	%xml.pushFirstChildElement("ImageList");

	while(true) {
		%xml.pushFirstChildElement("Image");

		while (true) {
			%xml.pushChildElement("FileName");
			%file = strreplace(%xml.getText(),"\"","");
			%fullFile = %baseFolder@"/"@%file;
			%fullFile = makeRelativePath(%fullFile);
			TMG.addTextureMap(%fullFile,true);
			%desc = %xml.getData();
			%xml.popElement();
			%xml.pushFirstChildElement("LayerList");
			%xml.pushFirstChildElement("Layer");

			while (true) {
				%xml.pushChildElement("LayerColour");
				%color = %xml.getText();
				%xml.popElement();
				%xml.pushFirstChildElement("TexList");
				%xml.pushFirstChildElement("TextureFile");
				%texture = %xml.getText();
				%xml.popElement();
				%xml.popElement();
				%xml.pushFirstChildElement("MtlList");
				%xml.pushFirstChildElement("MtlName");
				%material = %xml.getData();
				%xml.popElement();
				%xml.popElement();
				%layer[%layerId++] = %fullFile TAB %color TAB %material;

				if (!%xml.nextSiblingElement("Layer")) break;
			}

			%xml.popElement();
			%xml.popElement();
			echo("  File" SPC %file SPC "Desc" SPC %desc);

			if (!%xml.nextSiblingElement("Image")) break;
		}

		%xml.popElement();

		if (!%xml.nextSiblingElement("ImageList")) break;
	}

	if (%layer[1] $= "") {
		warnLog("No data extracted from XML file:",%filename);
		return;
	}

	if (!TMG.appendL3DTXml)
		%this.removeAllLayerMaps();

	while( %layer[%id++] !$= "") {
		%file = getField(%layer[%id],0);
		%file = makeRelativePath(%file);
		%channel = getField(%layer[%id],1);
		%material = getField(%layer[%id],2);
		%newId = TMG.addMaterialLayer(%material);
		TMG.setLayerMapFile(%newId,%file,%channels,%channel);
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function L3DT_AppendCheck::onClick(%this) {
	TMG.appendL3DTXml = %this.isStateOn();
}
//------------------------------------------------------------------------------
