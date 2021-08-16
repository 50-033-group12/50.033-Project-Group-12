using System;
using PlayerManagement;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CurtainController))]
public class CurtainControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        EditorGUILayout.Separator();
        EditorGUILayout.LabelField("(DEBUG) Open/Close Curtains", EditorStyles.boldLabel);
        CurtainController myScript = (CurtainController)target;
        if (GUILayout.Button("Open Curtains"))
        {
            myScript.OpenCurtain(() => { Debug.Log("Curtains opened"); });
        }
        if (GUILayout.Button("Close Curtains"))
        {
            myScript.CloseCurtain(() => { Debug.Log("Curtains closed"); });
        }
    }
}