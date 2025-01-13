using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BotDeathManager))]
public class OnDeathVFX : MonoBehaviour
{
    [SerializeField] GameObject _onDeathVFX;
    BotDeathManager deathManager;

    public void Start()
    {
        deathManager = GetComponent<BotDeathManager>();
        deathManager.OnBotDied += VFX;
    }

    private void OnDestroy()
    {
        deathManager.OnBotDied -= VFX;
    }

    void VFX()
    {
        Instantiate(_onDeathVFX, this.transform.position, Quaternion.Euler(-90, 0, 0));
    }
}
