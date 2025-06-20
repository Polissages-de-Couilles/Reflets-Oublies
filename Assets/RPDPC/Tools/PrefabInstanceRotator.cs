using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class PrefabInstanceRotator : MonoBehaviour
{
    [Tooltip("Prefab source à cibler")]
    public GameObject targetPrefab;

    [Tooltip("Inclure les objets dont le nom commence par celui du prefab")]
    public bool matchByNameIfNotLinked = true;

    [Tooltip("Rotations possibles en Y")]
    public List<float> allowedRotations = new List<float> { 0f, 90f, 180f, 270f };

    public void RotateAllPrefabInstances()
    {
        if (targetPrefab == null)
        {
            Debug.LogWarning("Aucun prefab défini dans targetPrefab.");
            return;
        }

        string baseName = targetPrefab.name;
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        int count = 0;

        foreach (GameObject obj in allObjects)
        {
            if (!obj.activeInHierarchy)
                continue;

            GameObject prefabRoot = PrefabUtility.GetCorrespondingObjectFromSource(obj);

            // Cas : objet lié au prefab
            if (prefabRoot == targetPrefab)
            {
                ApplyRotation(obj);
                count++;
            }
            // Cas : nom commence par le nom du prefab
            else if (matchByNameIfNotLinked && obj.name.StartsWith(baseName))
            {
                Debug.LogWarning($"⚠️ Objet '{obj.name}' non lié au prefab, mais son nom commence par '{baseName}'");
                ApplyRotation(obj);
                count++;
            }
        }

        Debug.Log($"✅ Rotation appliquée à {count} objet(s) correspondant à '{baseName}'");
    }

    private void ApplyRotation(GameObject obj)
    {
        Undo.RecordObject(obj.transform, "Rotation aléatoire prefab");
        float randomY = allowedRotations[Random.Range(0, allowedRotations.Count)];
        obj.transform.rotation = Quaternion.Euler(0f, randomY, 0f);
    }
}
