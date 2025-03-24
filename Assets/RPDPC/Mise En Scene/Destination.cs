using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destination : MonoBehaviour
{
    public int ID => _id;
    [SerializeField] int _id;
    public Vector3 Position => transform.position;
}
