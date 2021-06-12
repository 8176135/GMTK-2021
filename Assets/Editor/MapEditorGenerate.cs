using System;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(MapGen))]
public class MapGeneratorEditor : Editor {

    public override void OnInspectorGUI() {
        var mapGen = (MapGen) target;

        if (DrawDefaultInspector()) {
            if (mapGen.autoUpdate) {
                mapGen.GenerateMap();
            }
        }

        if (GUILayout.Button ("Generate")) {
            mapGen.GenerateMap();
        }
    }
}
