using UnityEngine;
using System.Xml.Serialization;
using System.Collections.Generic;
using System;
using System.Linq;

[XmlRoot("DroneFlight")]
[Serializable]
public class SerializedDroneFlight {

    public SerializedCameraDefinition cameraDefinition;
    public SerializedSurveyArea surveyArea;
    [XmlElement(ElementName = "Georeferencing")]
    public SerializedOutput output;
    public SerializedFlightPlan flightPlan;

    public SerializedDroneFlight(CameraDefinition cameraDefinition, SurveyArea surveyArea, Mission.FlightPlan flightPlan, Output output) {
        this.cameraDefinition = new SerializedCameraDefinition(cameraDefinition);
        this.surveyArea = new SerializedSurveyArea(surveyArea);
        this.flightPlan = new SerializedFlightPlan(flightPlan);
        this.output = new SerializedOutput(output);
    }

    public SerializedDroneFlight() { }

    public class SerializedCameraDefinition {

        public float focalLength;
        public float sensorSizeX;
        public float sensorSizeY;
        public int resolutionX;
        public int resolutionY;

        public SerializedCameraDefinition(CameraDefinition cameraDefinition) {
            this.focalLength = cameraDefinition.focalLength;
            this.sensorSizeX = cameraDefinition.sensorSizeX;
            this.sensorSizeY = cameraDefinition.sensorSizeY;
            this.resolutionX = cameraDefinition.resolutionX;
            this.resolutionY = cameraDefinition.resolutionY;
        }

        public SerializedCameraDefinition() { }
    }

    public class SerializedSurveyArea {

        public float areaWidth;
        public float areaDistance;
        public int frontalOverlap;
        public int sideOverlap;

        public SerializedSurveyArea(SurveyArea surveyArea) {
            this.areaWidth = surveyArea.areaToCover.size.x;
            this.areaDistance = surveyArea.areaToCover.size.z;
            this.frontalOverlap = surveyArea.frontalOverlap;
            this.sideOverlap = surveyArea.sideOverlap;
        }

        public SerializedSurveyArea() { }
    }

    public class SerializedFlightPlan {

        public Mission.MissionType missionType;
        public List<SerializedFlightStep> flightSteps;

        public SerializedFlightPlan(Mission.FlightPlan flightPlan) {
            this.flightSteps = flightPlan.flightSteps.Select(flightStep => new SerializedFlightStep(flightStep)).ToList();
            this.missionType = flightPlan.missionType;
        }

        public SerializedFlightPlan() { }
    }

    public class SerializedFlightStep {

        public Vector3 dronePosition;
        public Vector3 droneRotation;

        public SerializedFlightStep(Mission.FlightStep flightStep) {
            this.dronePosition = flightStep.dronePosition;
            this.droneRotation = flightStep.droneRotation.eulerAngles;
        }

        public SerializedFlightStep() { }
    }


    public class SerializedOutput {

        public double georeferenceCenterX, georeferenceCenterY;
        public int utmZone;
        public string utmHemisphere;

        public SerializedOutput(Output output) {
            this.georeferenceCenterX = output.georeferenceCenter.Item1;
            this.georeferenceCenterY = output.georeferenceCenter.Item2;
            this.utmZone = output.utmZone;
            this.utmHemisphere = output.utmHemisphere.Value;
        }

        public SerializedOutput() { }
    }
}