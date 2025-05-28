using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestReflection : MonoBehaviour
{
    // Start is called before the first frame update
    ReflectionProbe probe;
    void Start()
    {
        probe = GetComponent<ReflectionProbe>();
    }

    // Update is called once per frame
    void Update()
    {
        probe.RenderProbe();
    }
}
