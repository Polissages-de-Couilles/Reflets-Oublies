using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class BossWallsManager : MonoBehaviour
{
    GameObject WallsParent;

    // Start is called before the first frame update
    void Start()
    {
        WallsParent = transform.Find("Walls").gameObject;
        if (WallsParent != null)
        {
            WallsParent.SetActive(false);
        }
        StartCoroutine(test());
    }

    void ActivateWalls()
    {
        WallsParent.SetActive(true);
    }

    IEnumerator test()
    {
        yield return new WaitForSeconds(10);
        ActivateWalls();
    }
}
