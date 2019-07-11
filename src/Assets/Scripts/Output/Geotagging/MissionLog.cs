using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using static Mission;

public class MissionLog {

    private readonly Output output;
    private readonly CameraDefinition cameraDefinition;
    private Dictionary<string, FlightStep> droneInfoPerImage = new Dictionary<string, FlightStep>();

    public MissionLog(CameraDefinition cameraDefinition, Output output) {
        this.cameraDefinition = cameraDefinition;
        this.output = output;
    }

    public void RecordLog(string imagePath, FlightStep droneInfo) {
        droneInfoPerImage.Add(imagePath, droneInfo);
    }

    public void GeotagImagesWithLog() {
        GeoTagger.TagImages(cameraDefinition, output, droneInfoPerImage);
    }

}