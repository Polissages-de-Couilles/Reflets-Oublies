/*using System.Collections;
using AK.Wwise;
using UnityEngine;

public class CollisionSoundZone : MonoBehaviour
{
    public AK.Wwise.Event soundEvent; // L'�v�nement sonore que nous jouerons
    public GameObject someSpecificObject; // L'objet sp�cifique qui doit activer la zone
    public string gameParameter = "combatVolume"; // Le nom du param�tre de jeu pour contr�ler le volume
    public float transitionDuration = 1f; // Dur�e de la transition de volume (en secondes)

    private bool isPlayerInside = false;
    public float currentVolume = 0f; // Volume initial, entre 0 et 100

    void Start()
    {
        uint bankID = 0;
        AkSoundEngine.LoadBank("SoundbankProject", out bankID);
        Debug.Log("SoundBank ID: " + bankID);

        // Initialiser le volume � 0 au d�marrage
        AkSoundEngine.SetRTPCValue(gameParameter, currentVolume);

        // Jouer le son d�s le d�but
        PlaySound();
    }

    void OnBankLoaded(uint bankID)
    {
        Debug.Log("SoundBank charg� avec succ�s!");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == someSpecificObject && !isPlayerInside)
        {
            isPlayerInside = true;
            // Augmenter le volume � 100 lors de l'entr�e dans la zone
            StartCoroutine(ChangeVolumeSmoothly(100f));
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == someSpecificObject && isPlayerInside)
        {
            isPlayerInside = false;
            // Diminuer le volume � 0 lors de la sortie de la zone
            StartCoroutine(ChangeVolumeSmoothly(0f));
        }
    }

    void PlaySound()
    {
        soundEvent.Post(gameObject); // Lancer le son d�s le d�but
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
            AkSoundEngine.SetRTPCValue(gameParameter, currentVolume); // Mettre � jour le volume

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // S'assurer que la valeur finale est exactement celle vis�e
        currentVolume = targetVolume;
        AkSoundEngine.SetRTPCValue(gameParameter, currentVolume);
    }
}
*/
using UnityEngine;
using AK.Wwise;  // Assure-toi que Wwise est bien r�f�renc�
using System;  // Assure-toi que Wwise est bien r�f�renc�

public class PlayAudioOnStart : MonoBehaviour
{
    // R�f�rence publique pour l'�v�nement audio de type AK.Wwise.Event
    public AK.Wwise.Event AudioEvent;
    public AK.Wwise.Event PauseEvent;
    public AK.Wwise.Event ResumeEvent;

    // Volume allant de 0 � 100, avec une valeur par d�faut de 75
    [Range(0f, 100f)]  // Pour rendre le volume ajustable dans l'inspecteur Unity
    public float volume = 75f;

    [Tooltip("Nom du param�tre RTPC pour contr�ler le volume")]
    public string RTPC;

    // Start est appel� au lancement du jeu
    void Start()
    {
        // Joue l'�v�nement audio et obtient son ID pour l'utiliser ult�rieurement
        if (AudioEvent != null)
        {
            AudioEvent.Post(gameObject);
            PauseEvent.Post(gameObject);
        }
        else
        {
            Debug.LogError("L'�v�nement audio n'est pas assign� dans l'inspecteur.");
        }
    }

    // Update est appel� � chaque frame
    void Update()
    {
        // Ajuste la valeur du RTPC en fonction de la variable volume
        AkSoundEngine.SetRTPCValue(RTPC, volume);
    }

    // Lorsqu'un autre objet entre en collision avec le trigger
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entr�e dans la zone");

        // Joue l'�v�nement audio lorsque l'objet entre dans la zone
        if (AudioEvent != null)
        {
            ResumeEvent.Post(gameObject);
            Debug.Log("�v�nement audio jou�.");
        }
        else
        {
            Debug.LogWarning("L'�v�nement audio n'est pas assign�.");
        }
    }

    // Lorsqu'un autre objet sort de la zone de collision
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Sortie de la zone");
        PauseEvent.Post(gameObject);
    }
}

