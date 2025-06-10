using MeetAndTalk;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/IA/States/Base/Talk")]
public class TalkSO : StateBase
{
    [SerializeField] DialogueContainerSO dialogueGOOD;
    [SerializeField] DialogueContainerSO dialogueBAD;
    [SerializeField] DialogueContainerSO dialogueNEUTRAL;
    [SerializeField] List<string> ListOfTypesToDESTROY;

    public override StateEntityBase PrepareEntityInstance()
    {
        TalkEntity talkEntity = new TalkEntity();
        talkEntity.Init(dialogueGOOD, dialogueBAD, dialogueNEUTRAL, ListOfTypesToDESTROY);
        return talkEntity;
    }
}
