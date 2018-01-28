using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(feed))]
public class feed_editor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        feed script = (feed)target;
        if(GUILayout.Button("Refresh feed"))
        {
            script.refresh_feed();
        }
    }
}
