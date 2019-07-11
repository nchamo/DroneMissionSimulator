using UnityEngine;
using System;

public class Output {

    public class Hemisphere {
        private Hemisphere(string value) { Value = value; }

        public string Value { get; set; }

        public static Hemisphere North { get { return new Hemisphere("N"); } }
        public static Hemisphere South { get { return new Hemisphere("S"); } }
    }

    public readonly string folderPath;
    public readonly Tuple<double, double> georeferenceCenter;
    public readonly int utmZone;
    public readonly Hemisphere utmHemisphere;

    public Output(string folderPath, Tuple<double, double> georeferenceCenter, int utmZone, Hemisphere utmHemisphere) {
        this.folderPath = folderPath;
        this.georeferenceCenter = georeferenceCenter;
        this.utmZone = utmZone;
        this.utmHemisphere = utmHemisphere;
    }

}
