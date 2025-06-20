using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordEnabler : MonoBehaviour
{
    [SerializeField] MeshRenderer sword;
    [SerializeField] GameObject plantedSword;
    [SerializeField] AttackManager attackManager;

    private void Start()
    {
        sword.enabled = false;
        attackManager.canAttack = false;
        GetComponent<Lever>().LeverActivation += LeverActivated;
    }

    private void LeverActivated()
    {
        plantedSword.SetActive(false);
        sword.enabled = true;
        attackManager.canAttack = true;
    }
}
