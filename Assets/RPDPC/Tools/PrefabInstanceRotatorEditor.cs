using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PrefabInstanceRotator))]
public class PrefabInstanceRotatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PrefabInstanceRotator script = (PrefabInstanceRotator)target;

        if (GUILayout.Button("🔁 Appliquer rotation aléatoire"))
        {
            script.RotateAllPrefabInstances();
        }
    }
}
