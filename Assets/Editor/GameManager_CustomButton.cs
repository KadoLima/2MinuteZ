using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(GameManager))]
public class SaveSystem_CustomButton : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GameManager saveLoader = (GameManager)target;

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        if (GUILayout.Button("DELETE SAVED GAME"))
        {
            GameManager.DeleteData();
        }
    }
}

