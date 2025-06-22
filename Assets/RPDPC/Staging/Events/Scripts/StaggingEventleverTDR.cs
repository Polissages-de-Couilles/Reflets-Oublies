using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaggingEventleverTDR : StagingEvent
{
    private int activateLever = 0;
    [SerializeField] private DoorNoInteraction OpenDoor;
    public override void PlayEvent()
    {
        base.PlayEvent();
        activateLever++;
        if(activateLever == 2) StartCoroutine(OpenDoorCoroutine());
        else OnEventFinished?.Invoke();
    }

    IEnumerator OpenDoorCoroutine()
    {
        yield return new WaitForSeconds(1f);
        OpenDoor.OpenDoor();
        yield return new WaitForSeconds(1f);
        OnEventFinished?.Invoke();
    }

}
