using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(World))]
public class WorldGenerate : Editor {

    public override void OnInspectorGUI() {
        var mapGen = (World) target;

        if (DrawDefaultInspector()) {
            if (mapGen.autoUpdate) {
                mapGen.Generate();
            }
        }

        if (GUILayout.Button ("Generate")) {
            mapGen.Generate();
        }
        
        if (GUILayout.Button ("Clean Terrain")) {
            mapGen.CleanTerrain();
        }
    }
}