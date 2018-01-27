using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(keyboard))]
public class keyboard_editor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        keyboard script = (keyboard)target;
        if(GUILayout.Button("Rebuild keyboard"))
        {
            script.create_keyboard();
        }
    }
}
