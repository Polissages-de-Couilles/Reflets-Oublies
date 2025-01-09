/*using System.Collections;
using AK.Wwise;
using UnityEngine;

public class CollisionSoundZone : MonoBehaviour
{
    public AK.Wwise.Event soundEvent; // L'événement sonore que nous jouerons
    public GameObject someSpecificObject; // L'objet spécifique qui doit activer la zone
    public string gameParameter = "combatVolume"; // Le nom du paramètre de jeu pour contrôler le volume
    public float transitionDuration = 1f; // Durée de la transition de volume (en secondes)

    private bool isPlayerInside = false;
    public float currentVolume = 0f; // Volume initial, entre 0 et 100

    void Start()
    {
        uint bankID = 0;
        AkSoundEngine.LoadBank("SoundbankProject", out bankID);
        Debug.Log("SoundBank ID: " + bankID);

        // Initialiser le volume à 0 au démarrage
        AkSoundEngine.SetRTPCValue(gameParameter, currentVolume);

        // Jouer le son dès le début
        PlaySound();
    }

    void OnBankLoaded(uint bankID)
    {
        Debug.Log("SoundBank chargé avec succès!");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == someSpecificObject && !isPlayerInside)
        {
            isPlayerInside = true;
            // Augmenter le volume à 100 lors de l'entrée dans la zone
            StartCoroutine(ChangeVolumeSmoothly(100f));
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == someSpecificObject && isPlayerInside)
        {
            isPlayerInside = false;
            // Diminuer le volume à 0 lors de la sortie de la zone
            StartCoroutine(ChangeVolumeSmoothly(0f));
        }
    }

    void PlaySound()
    {
        soundEvent.Post(gameObject); // Lancer le son dès le début
        Debug.Log("Sound started!");
    }

    // Coroutine pour effectuer la transition fluide du volume
    private IEnumerator ChangeVolumeSmoothly(float targetVolume)
    {
        float startVolume = currentVolume;
        float elapsedTime = 0f;

        // Transition entre le volume actuel et le volume cible
        while (elapsedTime < transitionDuration)
        {
            currentVolume = Mathf.Lerp(startVolume, targetVolume, elapsedTime / transitionDuration);
            AkSoundEngine.SetRTPCValue(gameParameter, currentVolume); // Mettre à jour le volume

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // S'assurer que la valeur finale est exactement celle visée
        currentVolume = targetVolume;
        AkSoundEngine.SetRTPCValue(gameParameter, currentVolume);
    }
}
*/
using UnityEngine;
using AK.Wwise;  // Assure-toi que Wwise est bien référencé
using System;  // Assure-toi que Wwise est bien référencé

public class PlayAudioOnStart : MonoBehaviour
{
    // Référence publique pour l'événement audio de type AK.Wwise.Event
    public AK.Wwise.Event AudioEvent;
    public AK.Wwise.Event PauseEvent;
    public AK.Wwise.Event ResumeEvent;

    // Volume allant de 0 à 100, avec une valeur par défaut de 75
    [Range(0f, 100f)]  // Pour rendre le volume ajustable dans l'inspecteur Unity
    public float volume = 75f;

    [Tooltip("Nom du paramètre RTPC pour contrôler le volume")]
    public string RTPC;

    // Start est appelé au lancement du jeu
    void Start()
    {
        // Joue l'événement audio et obtient son ID pour l'utiliser ultérieurement
        if (AudioEvent != null)
        {
            AudioEvent.Post(gameObject);
            PauseEvent.Post(gameObject);
        }
        else
        {
            Debug.LogError("L'événement audio n'est pas assigné dans l'inspecteur.");
        }
    }

    // Update est appelé à chaque frame
    void Update()
    {
        // Ajuste la valeur du RTPC en fonction de la variable volume
        AkSoundEngine.SetRTPCValue(RTPC, volume);
    }

    // Lorsqu'un autre objet entre en collision avec le trigger
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entrée dans la zone");

        // Joue l'événement audio lorsque l'objet entre dans la zone
        if (AudioEvent != null)
        {
            ResumeEvent.Post(gameObject);
            Debug.Log("Événement audio joué.");
        }
        else
        {
            Debug.LogWarning("L'événement audio n'est pas assigné.");
        }
    }

    // Lorsqu'un autre objet sort de la zone de collision
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Sortie de la zone");
        PauseEvent.Post(gameObject);
    }
}

