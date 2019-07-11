using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static Mission;

public class DroneControl : MonoBehaviour {
    // This class simulates a drone flight taking cameras of a terrain.
    // We assume that the terrain is on the floor, facing up.

    #region External Configuration

    public ConfigurableSurveyArea surveyAreaConfiguration;
    public ConfigurableCameraDefinition cameraDefinitionConfiguration;
    public ConfigurableFlightSpecs flightSpecsConfiguration;
    public ConfigurableOutput outputConfiguration;

    [Header("Debug")]
    [Tooltip("Draw the flight plan grid")]
    public bool drawGrid = false;
    [Tooltip("Take screenshots. Turn off to just see the drone fly.")]
    public bool takeScreenshots = true;
    #endregion

    private MissionLog missionLog;
    private MissionDebugger missionDebugger;
    private ImageCapture imageCapture;
    private DroneMovement droneMovement;

    private int totalStepsInMission;
    private int currentStepInMission;
    private bool takeScreenshotOnLateUpdate = false;

    void Start() {
        // Assert an object was selected
        if (surveyAreaConfiguration.objectOfInterest == null) {
            throw new System.Exception("You must set an object of interest!");
        }

        // Build necessary entities
        Output output = outputConfiguration.BuildDomain();
        CameraDefinition cameraDefinition = cameraDefinitionConfiguration.BuildDomain();
        SurveyArea surveyArea = surveyAreaConfiguration.BuildDomain();

        // Set the camera
        Camera camera = GetComponent<Camera>();
        camera.usePhysicalProperties = true;
        camera.focalLength = cameraDefinition.focalLength;
        camera.sensorSize = new Vector2(cameraDefinition.sensorSizeX, cameraDefinition.sensorSizeY);

        // Calculate the flight plan
        FlightPlan flightPlan = CalculateFlightPlan(cameraDefinition, surveyArea);

        // Build helpers that will more the drone, take the images, and log everything to later geotag the images
        droneMovement = new DroneMovement(transform, flightPlan, flightSpecsConfiguration.speed);
        missionLog = new MissionLog(cameraDefinition, output);
        missionDebugger = new MissionDebugger(cameraDefinition, surveyArea, flightPlan, transform);
        imageCapture = new ImageCapture(cameraDefinition, camera, output.folderPath);

        // Save all configuration as XML
        StartCoroutine(SaveXML(cameraDefinition, surveyArea, flightPlan, output));

        // Add event listeners to react to drone states
        DroneMovement.OnReachedWaypoint += ReachedWaypoint;
        DroneMovement.OnReachedFinalWaypoint += ReachedFinalWaypoint;
    }

    // Update is called once per frame
    void Update() {
        droneMovement.MoveAndRotateDrone();

        if (drawGrid) {
            missionDebugger.DrawDebug(currentStepInMission);
        }
    }

    // LateUpdate is called once per frame, after everything was rendered
    void LateUpdate() {
        SaveScreenshotIfNecessary();
    }

    private FlightPlan CalculateFlightPlan(CameraDefinition cameraDefinition, SurveyArea surveyArea) {
        FlightPlan flightPlan = flightSpecsConfiguration.BuildMission(surveyArea, cameraDefinition).CalculateFlightPlan();
        Debug.Log(string.Format("Mission calculated. Will take {0} images.", flightPlan.flightSteps.Count));
        currentStepInMission = 0;
        totalStepsInMission = flightPlan.flightSteps.Count;
        return flightPlan;
    }

    private void ReachedWaypoint(FlightStep flightStep) {
        // Set the flag so that a screenshot is taken
        takeScreenshotOnLateUpdate = true;
        // Associate the image with the step information
        missionLog.RecordLog(GetImagePath(currentStepInMission + 1), flightStep);
        // Mark step as completed
        currentStepInMission++;
    }

    private void ReachedFinalWaypoint() {
        if (takeScreenshots) {
            StartCoroutine(GeoTagImages());
            Debug.Log(string.Format("Geotagging {0} images. This might take a while, please wait.", totalStepsInMission));
        } else {
            Quit();
        }
    }

    #region Output

    private IEnumerator SaveXML(CameraDefinition cameraDefinition, SurveyArea surveyArea, FlightPlan flightPlan, Output output) {
        Debug.Log("Writing an XML that details the mission");
        XMLWriter.WriteToXml(cameraDefinition, surveyArea, flightPlan, output);
        yield return null;
    }

    private IEnumerator GeoTagImages() {
        // Wait until all images have been saved
        do {
            yield return new WaitForSeconds(1);
        } while (Directory.GetFiles(outputConfiguration.folderPath).Length < totalStepsInMission);

        // Geotag the images
        missionLog.GeotagImagesWithLog();

        Quit();
    }

    private void SaveScreenshotIfNecessary() {
        if (takeScreenshots && takeScreenshotOnLateUpdate) {
            takeScreenshotOnLateUpdate = false;
            Debug.Log(string.Format("Capturing image. {0}/{1}", currentStepInMission, totalStepsInMission));
            StartCoroutine(imageCapture.SaveScreenshot(GetImagePath(currentStepInMission)));
        }
    }

    private string GetImagePath(int imageNumber) {
        string number = imageNumber.ToString().PadLeft(4, '0');
        return Path.Combine(outputConfiguration.folderPath, "Image_" + number + ".jpg");
    }

    #endregion

    private void Quit() {
        Debug.Log("All done!");
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

}
