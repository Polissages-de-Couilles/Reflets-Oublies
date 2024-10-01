using MeetAndTalk.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/Event/CameraShake")]
[System.Serializable]
public class CameraShake_Event : DialogueEventSO
{
    public float Time = 1f;
    public float Intensity = 5f;
    public override void RunEvent()
    {
        GameManager.Instance.CamManager.ShakeCamera(Intensity, Time);
    }
}
