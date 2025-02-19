using MeetAndTalk;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ACT", menuName = ("Game/ACT"), order = 1)]
public class ActDialogueSO : ScriptableObject
{
    [SerializeField] private DialogueContainerSO First;
    [SerializeField] List<DialogueContainerSO> Poll;
}
