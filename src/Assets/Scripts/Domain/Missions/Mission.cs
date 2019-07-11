using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public abstract class Mission {

    protected readonly MissionType missionType;
    protected readonly SurveyArea surveyArea;
    protected readonly CameraDefinition cameraDefinition;

    protected Mission(MissionType missionType, SurveyArea surveyArea, CameraDefinition cameraDefinition) {
        this.missionType = missionType;
        this.surveyArea = surveyArea;
        this.cameraDefinition = cameraDefinition;
    }

    public FlightPlan CalculateFlightPlan() {
        return new FlightPlan(CalculateFlightSteps(), missionType);
    }

    protected abstract List<FlightStep> CalculateFlightSteps();

    public enum MissionType {
        Grid,
        DoubleGrid
    }

    public struct FlightPlan {

        public FlightPlan(List<FlightStep> flightSteps, MissionType missionType) {
            this.flightSteps = flightSteps;
            this.missionType = missionType;
        }

        public readonly List<FlightStep> flightSteps;
        public readonly MissionType missionType;

    }

    public struct FlightStep {

        public FlightStep(Vector3 dronePosition, Quaternion droneRotation) {
            this.dronePosition = dronePosition;
            this.droneRotation = droneRotation;
        }

        public readonly Vector3 dronePosition;
        public readonly Quaternion droneRotation;
    }

}