using System;
using System.Collections;
using UnityEngine;

public class PlayButtonSound : MonoBehaviour
{
    public void footstepsSound()
    {
        AkSoundEngine.PostEvent("Play_footsteps", gameObject);
    }

    public void voiceSound()
    {
        AkSoundEngine.PostEvent("Play_Jack_heinn", gameObject);
    }

    public void musiqueSound()
    {
        AkSoundEngine.PostEvent("Play_Dynamic_Ambiance", gameObject);
    }
}
