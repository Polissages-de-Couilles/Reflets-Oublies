using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Mirror : MonoBehaviour
{
    public int ID;
    public bool isBroken = false;
    public Mesh destroyedMesh;

    public void SetBroken()
    {
        isBroken = true;

        GetComponentInChildren<MeshFilter>().mesh = destroyedMesh;
        foreach (Material mat in GetComponentInChildren<MeshRenderer>().materials)
        {
            Debug.Log("COLOR " + mat.GetColor("_BaseColor"));
            mat.SetColor("_BaseColor", mat.GetColor("_BaseColor").WithAlpha(0.5f));
        }
        
    }
}
