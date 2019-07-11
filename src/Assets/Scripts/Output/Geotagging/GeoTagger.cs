using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

internal class GeoTagger  {

    internal static void TagImages(CameraDefinition cameraDefinition, Output output, Dictionary<string, Mission.FlightStep> positionsPerImage) {
        Dictionary<string, UTMZoneCoordinates> utmCoords = MapPositionsToUTMZone(output, positionsPerImage);
        Dictionary<string, LongLatCoordidnates> longLatCoords = MapUTMZoneToLongLat(output, utmCoords);
        GeoTagImages(cameraDefinition, longLatCoords);
    }

    private static Dictionary<string, UTMZoneCoordinates> MapPositionsToUTMZone(Output output, Dictionary<string, Mission.FlightStep> positionsPerImage) {
        Dictionary<string, UTMZoneCoordinates> utmCoords = new Dictionary<string, UTMZoneCoordinates>(positionsPerImage.Count);
        foreach (KeyValuePair<string, Mission.FlightStep> entry in positionsPerImage) {
            double easting = entry.Value.dronePosition.x + output.georeferenceCenter.Item1;
            double northing = entry.Value.dronePosition.z + output.georeferenceCenter.Item2;
            double altitude = entry.Value.dronePosition.y;
            utmCoords.Add(entry.Key, new UTMZoneCoordinates(easting, northing, altitude));
        }
        return utmCoords;
    }

    private static Dictionary<string, LongLatCoordidnates> MapUTMZoneToLongLat(Output output, Dictionary<string, UTMZoneCoordinates> coordinatesPerImage) {
        Dictionary<string, LongLatCoordidnates> longLatCoords = new Dictionary<string, LongLatCoordidnates>(coordinatesPerImage.Count);
        foreach (KeyValuePair<string, UTMZoneCoordinates> entry in coordinatesPerImage) {
            LatLngUTMConverter.LatLng latlng = LatLngUTMConverter.ConvertUtmToLatLng(entry.Value.easting, entry.Value.northing, output.utmZone, output.utmHemisphere.Value);
            longLatCoords.Add(entry.Key, new LongLatCoordidnates(latlng.Lng, latlng.Lat, entry.Value.altitude));
        }
        return longLatCoords;
    }

    private static void GeoTagImages(CameraDefinition cameraDefinition, Dictionary<string, LongLatCoordidnates> coordinatesPerImage) {
        foreach (KeyValuePair<string, LongLatCoordidnates> entry in coordinatesPerImage) {
            using (Process process = new Process()) {
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.StartInfo.CreateNoWindow = true;
                process.EnableRaisingEvents = true;
                process.StartInfo.FileName = Directory.GetCurrentDirectory() + "\\exiftool.exe";
                process.StartInfo.Arguments = string.Format("-overwrite_original -GPSLatitude={0} -GPSLatitudeRef=S -GPSLongitude={1} -GPSLongitudeRef=W -GPSAltitude={2} -GPSAltitudeRef=\"Above Sea Level\" -FocalLength={3} \"{4}\""
                    , Math.Abs(entry.Value.latitude), Math.Abs(entry.Value.longitude), entry.Value.altitude, cameraDefinition.focalLength, entry.Key);
                
                process.Start();
                process.WaitForExit();
            }
        }
    }

    internal struct UTMZoneCoordinates {

        internal UTMZoneCoordinates(double easting, double northing, double altitude) {
            this.easting = easting;
            this.northing = northing;
            this.altitude = altitude;
        }

        internal readonly double easting, northing, altitude;
    }

    internal struct LongLatCoordidnates {

        internal LongLatCoordidnates(double longitude, double latitude, double altitude) {
            this.longitude = longitude;
            this.latitude = latitude;
            this.altitude = altitude;
        }

        internal readonly double longitude, latitude, altitude;
    }

}
