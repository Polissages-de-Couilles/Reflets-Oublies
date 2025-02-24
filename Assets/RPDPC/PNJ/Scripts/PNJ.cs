using MeetAndTalk;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PDC.Localization;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using LocalizationManager = PDC.Localization.LocalizationManager;
using DG.Tweening.Core.Easing;

public class PNJ : Interactible
{
    public override string Text => $"{LocalizationManager.LocalizeText("PNJ_UI_INTERACTION", true)} {LocalizationManager.LocalizeText(_data.CharacterNameKey, true)}";

    [SerializeField] PNJData _data;
    [SerializeField] List<ActDialogueSO> _dialogues;

    public enum State
    {
        Waiting,
        Dialogue,
        Choice
    }
    public State state;

    public void Awake()
    {
        foreach (var dialogue in _dialogues)
        {
            dialogue.isFirst = true;
        }
    }

    public override void OnInteraction()
    {
        if (!LocalizationManager.IsLocaReady || GameManager.Instance.DialogueManager.isDialogueInProcess) return;

        switch (state)
        {
            case State.Waiting:
                GameManager.Instance.DialogueManager.OnNode += OnNode;
                GameManager.Instance.DialogueManager.EndDialogueEvent.AddListener(() => 
                { 
                    GameManager.Instance.DialogueManager.OnNode -= OnNode;
                    state = State.Waiting;
                    EventSystem.current.SetSelectedGameObject(null);
                }
                );
                var listDialogue = _dialogues.Find(x => x.Act == GameManager.Instance.StoryManager.CurrentAct);
                GameManager.Instance.DialogueManager.StartDialogue(listDialogue.GetDialogue());
                break;

            case State.Dialogue:
                //GameManager.Instance.DialogueManager.SkipDialogue();
                break;

            case State.Choice:
                break;

            default:
                break;
        }
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
