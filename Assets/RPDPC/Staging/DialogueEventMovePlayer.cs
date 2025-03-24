using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeetAndTalk.Event;
using System.Linq;

[CreateAssetMenu(menuName = "Dialogue/Event/MovePlayer")]
public class DialogueEventMovePlayer : DialogueEventSO
{
    public int _destinationId;

    public override void RunEvent()
    {
        var playerPos = GameManager.Instance.Player.transform.position;
        var destination = FindObjectsByType<Destination>(FindObjectsSortMode.None).ToList().Find(x => x.ID == _destinationId);
        if (destination == null) return;


    }
}
