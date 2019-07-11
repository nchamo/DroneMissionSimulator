using UnityEngine;
using System.Collections.Generic;
using System.Xml;
using System.IO;

public class XMLExporter {

    private static readonly string XML_NAME = "elevation_map.xml";

    public static void ExportXml(ElevationMap elevationMap, string folderPath, float gsd, bool exportCells) {
        using (XmlWriter writer = XmlWriter.Create(Path.Combine(folderPath, XML_NAME))) {
            writer.WriteStartDocument();
            writer.WriteStartElement("ElevationMap");
            WriteProperties(writer, elevationMap, gsd);
            if (exportCells) {
                WriteCellsInNumpyFormat(writer, elevationMap);
            }
            writer.WriteEndElement();
            writer.WriteEndDocument();
        }
    }

    private static void WriteProperties(XmlWriter writer, ElevationMap elevationMap, float gsd) {
        writer.WriteStartElement("Properties");
        writer.WriteElementString("GSD", gsd.ToString());
        writer.WriteElementString("Width", elevationMap.Width.ToString());
        writer.WriteElementString("Height", elevationMap.Height.ToString());
        writer.WriteElementString("MaxElevation", elevationMap.MaxElevation.ToString());
        writer.WriteElementString("MinElevation", elevationMap.MinElevation.ToString());
        writer.WriteElementString("NoValue", elevationMap.NO_VALUE.ToString());
        writer.WriteEndElement();
    }

    private static void WriteCellsInNumpyFormat(XmlWriter writer, ElevationMap elevationMap) {
        string[] rows = new string[elevationMap.Height];
        for (int i = 0; i < elevationMap.Height; i++) {
            float[] columns = new float[elevationMap.Width];
            for (int j = 0; j < elevationMap.Width; j++) {
                columns[j] = elevationMap[i, j];
            }
            rows[i] = string.Join(",", columns);
        }
        writer.WriteStartElement("Map");
        writer.WriteElementString("ArrayInNumpyFormat", string.Join("\n", rows));
        writer.WriteEndElement();
    }
}
