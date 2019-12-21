using UnityEngine;
using UnityEditor;

public class ElevationMapWindow : TerrainToolWindow {

    private float GSD = 20;
    private int areaWidth = 200, areaDistance = 200;
    private bool exportArrayInXML = true;

    [MenuItem("Window/Terrain Tools/Elevation Map")]
    static void Init() {
        ElevationMapWindow window = (ElevationMapWindow)GetWindow(typeof(ElevationMapWindow));
    }

    protected override void LoadFields() {
        GSD = EditorGUILayout.FloatField("GSD:", GSD);
        areaWidth = EditorGUILayout.IntField("Area Width:", areaWidth);
        areaDistance = EditorGUILayout.IntField("Area Distance:", areaDistance);
        exportArrayInXML = EditorGUILayout.Toggle("Export map array in XML:", exportArrayInXML);
    }

    protected override string GetButtonText() {
        return "Generate Elevation Map";
    }

    protected override TaskList GetTaskList(Terrain terrain, string folderPath) {
        return TaskList.From(new TreeInstantiator(terrain, areaDistance, areaDistance))
            .With(new Raycaster(GSD))
            .With(new ElevationMapGenerator())
            .With(new ElevationMapExporter(GSD, exportArrayInXML, folderPath));
    }
}