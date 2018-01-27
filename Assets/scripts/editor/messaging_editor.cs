using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(messaging))]
public class messaging_editor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        messaging script = (messaging)this.target;
        if(GUILayout.Button("Send Tweet!"))
        {
            script.send_tweet();
        }
    }
}
