using MeetAndTalk;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ACT", menuName = ("Game/ACT"), order = 1)]
public class ActDialogueSO : ScriptableObject
{
    public bool isFirst { get; set; } = true;
    public Act Act => _act;
    [SerializeField] Act _act;
    public DialogueContainerSO First => _first;
    [SerializeField] DialogueContainerSO _first;
    public List<DialogueContainerSO> Poll => _poll;
    [SerializeField] List<DialogueContainerSO> _poll;

    public DialogueContainerSO GetDialogue()
    {
        if(isFirst && _first != null)
        {
            isFirst = false;
            return _first;
        }
        else
        {
            var rng = UnityEngine.Random.Range(0, _poll.Count);
            return _poll[rng];
        }
    }
}
