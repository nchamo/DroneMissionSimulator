using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using System;

[ExecuteInEditMode]
public class ElevationMapWindow : EditorWindow {

    private Terrain terrain;
    private float GSD = 20;
    private int areaWidth = 200, areaDistance = 200;
    private bool exportCellsInXml = true;

    [MenuItem("Window/Elevation Map")]
    static void Init() {
        ElevationMapWindow window = (ElevationMapWindow)GetWindow(typeof(ElevationMapWindow));
    }

    void OnGUI() {
        terrain = (Terrain)EditorGUILayout.ObjectField(terrain, typeof(Terrain), true);
        if (terrain != null) {
            GSD = EditorGUILayout.FloatField("GSD:", GSD);
            areaWidth = EditorGUILayout.IntField("Area Width:", areaWidth);
            areaDistance = EditorGUILayout.IntField("Area Distance:", areaDistance);
            exportCellsInXml = EditorGUILayout.Toggle("Export map array in XML:", exportCellsInXml);
            if (GUILayout.Button("Generate Elevation Map (it might take a while)")) {
                string folderPath = EditorUtility.OpenFolderPanel("Select where to store the elevation map", "", "");
                if (folderPath == "") {
                    EditorUtility.DisplayDialog("Error!", "You must select a folder", "OK");
                } else {
                    GenerateElevationMap(folderPath);
                    EditorUtility.DisplayDialog("Done!", string.Format("Files were created. Check the folder '{0}'.", folderPath), "OK");
                }
            }
        } else {
            EditorGUILayout.LabelField("Message:", "Please select a terrain.");
        }
    }

    void OnInspectorUpdate() {
        Repaint();
    }

    private void GenerateElevationMap(string folderPath) {
        ElevationMap elevationMap = Generator.GenerateElevationMap(terrain, areaWidth, areaWidth, GSD);
        ImageExporter.ExportGreyscaleImage(elevationMap, folderPath);
        XMLExporter.ExportXml(elevationMap, folderPath, GSD, exportCellsInXml);
    }

}