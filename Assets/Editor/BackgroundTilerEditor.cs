using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BackgroundTiler))]
public class BackgroundTilerEditor : Editor
{
    public override void OnInspectorGUI() {
        var tiler = (BackgroundTiler) target;

        if (DrawDefaultInspector()) {
            if (tiler.autoUpdate) {
                tiler.Destroy();
                tiler.Create();
            }
        }

        if (GUILayout.Button ("Create")) {
            tiler.Create();
        }
        
        if (GUILayout.Button ("Destroy")) {
            tiler.Destroy();
        }
    }
}
