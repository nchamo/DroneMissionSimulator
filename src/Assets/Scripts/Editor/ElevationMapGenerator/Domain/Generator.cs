using UnityEngine;
using System.Collections;

public class Generator {

    private static readonly string TREE_CONTAINER = "Tree Container";

    public static ElevationMap GenerateElevationMap(Terrain terrain, int areaWidth, int areaDistance, float GSD) {
        Bounds bounds = CalculateBounds(terrain, areaWidth, areaDistance);
        GameObject treeContainer = CopyTreesInBounds(terrain, bounds);
        AddMeshColliderToGeneratedTrees(treeContainer);
        ElevationMap elevationMap = CalculateElevations(bounds, GSD);
        DestroyTreeContainer();
        return elevationMap;
    }

    private static Bounds CalculateBounds(Terrain terrain, int areaWidth, int areaDistance) {
        // Try to find the center of the object of interest 
        Bounds terrainBounds;
        if (terrain.GetComponent<Collider>() != null) {
            terrainBounds = terrain.GetComponent<Collider>().bounds;
        } else if (terrain.GetComponent<Renderer>() != null) {
            terrainBounds = terrain.GetComponent<Renderer>().bounds;
        } else {
            terrainBounds = new Bounds(terrain.transform.position, terrain.transform.localScale);
        }

        return new Bounds(terrainBounds.center, new Vector3(areaWidth, terrainBounds.size.y, areaDistance));
    }

    private static GameObject CopyTreesInBounds(Terrain terrain, Bounds bounds) {
        TerrainData data = terrain.terrainData;
        float width = data.size.x;
        float distance = data.size.z;
        float altitude = data.size.y;
        // Create parent
        GameObject parent = GameObject.Find(TREE_CONTAINER);
        if (parent == null) {
            parent = new GameObject(TREE_CONTAINER);
        }
        // Create trees
        foreach (TreeInstance tree in data.treeInstances) {
            if (tree.prototypeIndex >= data.treePrototypes.Length)
                continue;
            var _tree = data.treePrototypes[tree.prototypeIndex].prefab;
            Vector3 position = new Vector3(
                tree.position.x * width,
                tree.position.y * altitude,
                tree.position.z * distance) + terrain.transform.position;
            if (bounds.Contains(position)) {
                Vector3 scale = new Vector3(tree.widthScale, tree.heightScale, tree.widthScale);
                GameObject go = Object.Instantiate(_tree, position, Quaternion.Euler(0f, Mathf.Rad2Deg * tree.rotation, 0f), parent.transform) as GameObject;
                go.transform.localScale = scale;
            }
        }

        return parent;
    }

    private static void AddMeshColliderToGeneratedTrees(GameObject treeContainer) {
        // Destroy all previous colliders
        foreach (Collider collider in treeContainer.GetComponentsInChildren<Collider>()) {
            Object.DestroyImmediate(collider);
        }

        // Assign a Mesh Collider to each tree
        foreach (Transform child in treeContainer.transform) {
            LODGroup[] lodGroups = child.GetComponentsInChildren<LODGroup>();
            if (lodGroups.Length > 0) {
                LOD lod = lodGroups[0].GetLODs()[0];
                MeshCollider meshCollider = child.gameObject.AddComponent<MeshCollider>();
                meshCollider.convex = false;
                meshCollider.sharedMesh = lod.renderers[0].GetComponent<MeshFilter>().sharedMesh;
            }
        }
    }

    private static ElevationMap CalculateElevations(Bounds areaToCover, float GSD) {
        float GSDInMeters = GSD / 100;
        ElevationMap elevationMap = new ElevationMap(Mathf.FloorToInt(areaToCover.size.x / GSDInMeters), Mathf.FloorToInt(areaToCover.size.z / GSDInMeters));

        for (int i = 0; i < elevationMap.Height; i++) {
            for (int j = 0; j < elevationMap.Width; j++) {
                float z = areaToCover.center.z - areaToCover.extents.z + i * GSDInMeters;
                float x = areaToCover.center.x - areaToCover.extents.x + j * GSDInMeters;
                Vector3 rayOrigin = new Vector3(x, areaToCover.center.y + areaToCover.size.y + 100, z);
                bool wasHit = Physics.SphereCast(rayOrigin, GSDInMeters, Vector3.down, out RaycastHit hitInfo);
                if (wasHit) {
                    elevationMap[i, j] = hitInfo.point.y;
                }
            }
        }

        return elevationMap;
    }

    private static void DestroyTreeContainer() {
        Object.DestroyImmediate(GameObject.Find(TREE_CONTAINER));
    }
}
