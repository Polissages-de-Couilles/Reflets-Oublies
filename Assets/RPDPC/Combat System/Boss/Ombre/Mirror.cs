using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour
{
    public int ID;
    public bool isBroken = false;

    public void SetBroken()
    {
        isBroken = true;

        GetComponent<MeshRenderer>().enabled = false;
    }
}
