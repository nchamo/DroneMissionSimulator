using UnityEngine;
using System.Collections;
using System.IO;

public class ImageCapture {

    private readonly CameraDefinition cameraDefinition;
    private readonly Camera camera;
    private readonly string folderPath;
   
    public ImageCapture(CameraDefinition cameraDefinition, Camera camera, string folderPath) {
        this.cameraDefinition = cameraDefinition;
        this.camera = camera;
        this.folderPath = folderPath;
        PrepareFolder(folderPath);
    }

    public IEnumerator SaveScreenshot(string imagePath) {
        RenderTexture rt = new RenderTexture(cameraDefinition.resolutionX, cameraDefinition.resolutionY, 24);
        Texture2D screenShot = new Texture2D(cameraDefinition.resolutionX, cameraDefinition.resolutionY, TextureFormat.RGB24, false);

        camera.targetTexture = rt;
        camera.Render();
        camera.targetTexture = null;

        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, cameraDefinition.resolutionX, cameraDefinition.resolutionY), 0, 0);
        camera.targetTexture = null;
        RenderTexture.active = null; //Added to avoid errors
        Object.Destroy(rt);

        //Split the process up
        yield return 0;

        byte[] bytes = screenShot.EncodeToJPG();
        File.WriteAllBytes(imagePath, bytes);
    }

    private static void PrepareFolder(string folderPath) {
        // Prepare the directory
        if (Directory.Exists(folderPath))
            Directory.Delete(folderPath, true);
        Directory.CreateDirectory(folderPath);
    }
}
