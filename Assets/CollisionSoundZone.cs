using System.Collections;
using System.Collections.Generic;
using AK.Wwise;
using UnityEngine;

public class CollisionSoundZone : MonoBehaviour
{
    void Start()
    {
        uint bankID = 0;
        AkSoundEngine.LoadBank("SoundbankProject", out bankID);
        Debug.Log("SoundBank ID: " + bankID);
    }

    void OnBankLoaded(uint bankID)
    {
        Debug.Log("SoundBank chargé avec succès!");
    }

    public AK.Wwise.Event soundEvent;
    public GameObject someSpecificObject;
    private bool isPlayerInside = false;
    

    void OnTriggerEnter(Collider other)
    {
        

        if (other.gameObject == someSpecificObject && !isPlayerInside) 
        {
            isPlayerInside = true;
            PlaySound();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == someSpecificObject && isPlayerInside) 
        {
            isPlayerInside = false;
            StopSound();
        }
    }

    void PlaySound()
    {
        soundEvent.Post(gameObject);
        Debug.Log("Sound started!");
    }

    void StopSound()
    {
        AkSoundEngine.StopAll();
        Debug.Log("Sound stopped!");
    }
}
