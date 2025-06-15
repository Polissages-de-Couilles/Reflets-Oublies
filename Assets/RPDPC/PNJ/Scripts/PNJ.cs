using MeetAndTalk;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PDC.Localization;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using LocalizationManager = PDC.Localization.LocalizationManager;

public class PNJ : Interactible
{
    public override string Text => /*$"{LocalizationManager.LocalizeText("PNJ_UI_INTERACTION", true)} */$"{LocalizationManager.LocalizeText(_data.CharacterNameKey, true)}";

    [SerializeField] PNJData _data;
    //[SerializeField] List<ActDialogueSO> _dialogues = new();

    public DialogueContainerSO First => _first;
    [SerializeField] DialogueContainerSO _first;
    public List<DialogueContainerSO> Poll => _poll;
    [SerializeField] List<DialogueContainerSO> _poll;

    bool _isFirstTimeDialogue = true;

    public enum State
    {
        Waiting,
        Dialogue,
        Choice
    }
    public State state;

    public void Awake()
    {
        //foreach (var dialogue in _dialogues)
        //{
        //    dialogue.isFirst = true;
        //}
    }

    public override void OnInteraction()
    {
        if (!LocalizationManager.IsLocaReady || GameManager.Instance.DialogueManager.isDialogueInProcess) return;

        switch (state)
        {
            case State.Waiting:
                if((_poll.Count <= 0 && _first == null) || (_poll.Count <= 0 && _first != null && !_isFirstTimeDialogue)) return;
                GameManager.Instance.DialogueManager.OnNode += OnNode;
                GameManager.Instance.DialogueManager.EndDialogueEvent.AddListener(() => 
                { 
                    GameManager.Instance.DialogueManager.OnNode -= OnNode;
                    state = State.Waiting;
                    EventSystem.current.SetSelectedGameObject(null);
                }
                );
                var dialogue = _isFirstTimeDialogue ? _first : _poll[UnityEngine.Random.Range(0, _poll.Count)];
                _isFirstTimeDialogue = false;
                //_dialogues.Find(x => x.Act == GameManager.Instance.StoryManager.CurrentAct);
                GameManager.Instance.DialogueManager.StartDialogue(dialogue);
                break;

            case State.Dialogue:
                //GameManager.Instance.DialogueManager.SkipDialogue();
                break;

            case State.Choice:
                break;

            default:
                break;
        }
        base.OnInteraction();
    }

    private void OnNode(BaseNodeData data)
    {
        if (data is DialogueNodeData)
        {
            state = State.Dialogue;
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(GameManager.Instance.DialogueUIManager.SkipButton);
            return;
        }

        if(data is DialogueChoiceNodeData || data is TimerChoiceNodeData)
        {
            state = State.Choice;
            EventSystem.current.SetSelectedGameObject(null);
            GameManager.Instance.DialogueUIManager.OnButtonCreate += SelectChoiceButton;
            return;
        }
    }

    private void SelectChoiceButton(int textcount)
    {
        if (textcount == 0) return;
        GameManager.Instance.DialogueUIManager.OnButtonCreate -= SelectChoiceButton;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(GameManager.Instance.DialogueUIManager.ButtonContainer.transform.GetChild(0).gameObject);
    }

    
}
