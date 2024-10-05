using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PDC.Localization;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance => _instance;
    private static GameManager _instance;
    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
    }

    public LocalizationManager LocalizationManager => _localizationManager;
    [SerializeField] LocalizationManager _localizationManager;

    public CinemachineEffectManager CamManager => _camManager;
    [SerializeField] CinemachineEffectManager _camManager;

    //Uniquement là à but de test, ne pas utiliser pour la version final
    public void Update()
    {
        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    CamManager.ShakeCamera(5f, 1f);
        //}
    }
}
