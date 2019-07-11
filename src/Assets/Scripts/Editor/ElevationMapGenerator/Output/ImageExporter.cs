using UnityEngine;
using System.Collections;
using System.IO;

public class ImageExporter {

    private static readonly string IMAGE_NAME = "elevation_map.jpg";

    public static void ExportGreyscaleImage(ElevationMap elevationMap, string folderPath) {
        Texture2D image = new Texture2D(elevationMap.Width, elevationMap.Height, TextureFormat.RGB24, false);
        for (int y = 0; y < image.height; y++) {
            for (int x = 0; x < image.width; x++) {
                float elevation = elevationMap[y, x];
                if (elevation == elevationMap.NO_VALUE) {
                    image.SetPixel(x, y, Color.black);
                } else {
                    float adjusted = elevation / elevationMap.MaxElevation;
                    Color color = new Color(adjusted, adjusted, adjusted);
                    image.SetPixel(x, y, color);
                }
            }
        }

        // Apply all SetPixel calls
        image.Apply();
        byte[] data = image.EncodeToJPG();
        File.WriteAllBytes(Path.Combine(folderPath, IMAGE_NAME), data);
    }

}
