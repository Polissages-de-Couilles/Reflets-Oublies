using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeetAndTalk.Event;

[CreateAssetMenu(menuName = "Dialogue/Event/AudioDialogue")]
public class AudioDialogueEvent : DialogueEventSO
{
    public string AudioName;
    private bool firstNode;
    private uint soundID;

    public override void RunEvent()
    {
        firstNode = false;
        soundID = AkSoundEngine.PostEvent(AudioName, GameManager.Instance.AudioDialogueGameObject);
        GameManager.Instance.DialogueManager.OnNode += (node) =>
        {
            if (!firstNode)
            {
                firstNode = true;
                return;
            }
            AkSoundEngine.StopPlayingID(soundID);
        };
    }
}
