using UnityEngine;
using UnityEditor;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class XMLWriter {
    
    public static void WriteToXml(CameraDefinition cameraDefinition, SurveyArea surveyArea, Mission.FlightPlan flightPlan, Output output) {
        SerializedDroneFlight droneFlight = new SerializedDroneFlight(cameraDefinition, surveyArea, flightPlan, output);
        XmlSerializer serializer = new XmlSerializer(typeof(SerializedDroneFlight));
        TextWriter writer = new StreamWriter(Path.Combine(output.folderPath, "droneFlight.xml"));
        serializer.Serialize(writer, droneFlight);
        writer.Close();
    }
}