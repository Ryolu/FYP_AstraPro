using UnityEngine;
using UnityEditor;

public class CollectDependencies : EditorWindow
{
    static GameObject obj = null;


    [MenuItem("Tools/Check Dependencies")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        CollectDependencies window = (CollectDependencies)GetWindow(typeof(CollectDependencies), false, "Dependencies");
        window.Show();
    }

    void OnGUI()
    {
        obj = EditorGUILayout.ObjectField("Find Dependency", obj, typeof(GameObject), true) as GameObject;

        if (obj)
        {
            Object[] roots = new Object[] { obj };

            if (GUI.Button(new Rect(3, 25, position.width - 6, 20), "Check Dependencies"))
                Selection.objects = EditorUtility.CollectDependencies(roots);
        }
        else
            EditorGUI.LabelField(new Rect(3, 25, position.width - 6, 20), "Missing:", "Select an object first");
    }

    void OnInspectorUpdate()
    {
        Repaint();
    }
}