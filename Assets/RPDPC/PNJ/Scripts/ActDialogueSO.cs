using MeetAndTalk;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "NAME_ACT_NUMBER", menuName = ("Game/ACT"), order = 1)]
public class ActDialogueSO : ScriptableObject
{
    public bool isFirst { get; set; } = true;

    public Act Act => _act;
    [SerializeField] Act _act;
    public DialogueContainerSO First => _first;
    [SerializeField] DialogueContainerSO _first;
    public List<DialogueContainerSO> Poll => _poll;
    [SerializeField] List<DialogueContainerSO> _poll;

    [SerializeField] List<ActDialogueSO> _sameFirstDialogue = new();

    public DialogueContainerSO GetDialogue()
    {
        bool otherFirst = _sameFirstDialogue.Count > 0 && _sameFirstDialogue.Any(x => !x.isFirst);

        if(!otherFirst && isFirst && _first != null)
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

    public void OnValidate()
    {
        foreach(var same in _sameFirstDialogue)
        {
            if(!same._sameFirstDialogue.Contains(this))
            {
                same._sameFirstDialogue.Add(this);
            }
        }

        if(!_first.name.Contains(_act.ToString()))
        {
            _first.name = (_first.name + "_" + _act.ToString());
        }

        foreach(var dialogue in _poll)
        {
            if(!dialogue.name.Contains(_act.ToString()))
            {
                dialogue.name = (dialogue.name + "_" + _act.ToString());
            }
        }
    }
}
