using UnityEngine;
using System;

[System.Serializable]
public class ConfigurableOutput {

    public enum Hemisphere {
        North,
        South
    }

    [Tooltip("The path to the folder where the images will be saved. Everything inside the folder will be deleted before starting.")]
    public string folderPath = "Images";

    [Tooltip("The geometric point (in UTM) where the images will be tagged against.")]
    public double georeferenceCenterX = 277524.5597564102;
    [Tooltip("The geometric point (in UTM) where the images will be tagged against.")]
    public double georeferenceCenterY = 5485401.149023914;

    [Tooltip("The UTM zone where the point will is.")]
    public int utmZone = 19;

    [Tooltip("The hemisphere where the point is.")]
    public Hemisphere utmHemisphere = Hemisphere.South;

    public Output BuildDomain() {
        return new Output(folderPath, new Tuple<double, double>(georeferenceCenterX, georeferenceCenterY), utmZone, utmHemisphere == Hemisphere.North ? Output.Hemisphere.North : Output.Hemisphere.South);
    }
}