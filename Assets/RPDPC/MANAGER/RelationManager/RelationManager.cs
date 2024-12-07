using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class RelationManager : MonoBehaviour
{
    public float RelationValue => _relationValue;
    [Range(-10f,10f)]
    private float _relationValue;

    public void ChangeValue(float valueChange)
    {
        if (Mathf.Abs(_relationValue + valueChange) > 10f) valueChange = (10 - Mathf.Abs(_relationValue)) * (valueChange / Mathf.Abs(valueChange));
        _relationValue += valueChange;
    }
}