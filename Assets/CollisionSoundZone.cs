using UnityEngine;
using AK.Wwise; 
using System.Collections;

public class PlayAudioOnStart : MonoBehaviour
{
    public AK.Wwise.Event AudioEvent;
    public AK.Wwise.Event PauseEvent;
    public AK.Wwise.Event ResumeEvent;

    [Tooltip("Nom du paramètre RTPC")]
    public string RTPC;

    [Range(0f, 100f)]
    public float volume = 0f;

    private bool FirstTimeEnter = true;

    // Paramètres pour la transition de volume
    private const float targetVolume = 100f;
    private const float transitionTime = 1f; 
    private const float updateInterval = 0.001f; 

    
    

    private void OnTriggerEnter(Collider other)
    {
        if (FirstTimeEnter == true)
        {
            FirstTimeEnter = false;
            AudioEvent.Post(gameObject);
        }

        Debug.Log("Entrée dans la zone");
        StopAllCoroutines();
        StartCoroutine(TransitionVolumeEnter());

    }

    IEnumerator TransitionVolumeEnter()
    {
        float startTime = Time.time;
        ResumeEvent.Post(gameObject);
        volume = 0;

        while (volume < targetVolume)
        {
            // Calcule le pourcentage de la transition
            float progress = (Time.time - startTime) / transitionTime;

            // Limite la progression à 1 (100%)
            progress = Mathf.Clamp01(progress);

            // Met à jour le volume actuel en fonction de la progression
            volume = Mathf.Lerp(0f, targetVolume, progress);

            // Met à jour le RTPC pendant la transition
            AkSoundEngine.SetRTPCValue(RTPC, volume);

            // Attends un court instant avant de mettre à jour
            yield return new WaitForSeconds(updateInterval);
        }

        volume = targetVolume;
        AkSoundEngine.SetRTPCValue(RTPC, volume);
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Sortie de la zone");
        StopAllCoroutines();
        StartCoroutine(TransitionVolumeExit());
    }

    IEnumerator TransitionVolumeExit()
    {
        float startTime = Time.time;
        volume = 100;

        while (volume > 0)
        {
            // Calcule le pourcentage de la transition
            float progress = (Time.time - startTime) / transitionTime;

            // Limite la progression à 1 (100%)
            progress = Mathf.Clamp01(progress);

            // Met à jour le volume actuel en fonction de la progression (100 à 0)
            volume = Mathf.Lerp(targetVolume, 0f, progress);

            // Met à jour le RTPC pendant la transition
            AkSoundEngine.SetRTPCValue(RTPC, volume);

            // Attends un court instant avant de mettre à jour
            yield return new WaitForSeconds(updateInterval);
        }
        volume = 0;
        AkSoundEngine.SetRTPCValue(RTPC, volume);
        PauseEvent.Post(gameObject);
    }
}
