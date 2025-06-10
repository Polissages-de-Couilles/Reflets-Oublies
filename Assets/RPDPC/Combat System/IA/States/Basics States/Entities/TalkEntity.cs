using MeetAndTalk;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using LocalizationManager = PDC.Localization.LocalizationManager;
using System;

public class TalkEntity : StateEntityBase
{
    DialogueContainerSO dialogueGOOD; 
    DialogueContainerSO dialogueBAD;
    DialogueContainerSO dialogueNEUTRAL;
    List<string> ListOfTypesToDESTROY;

    public override void Init(DialogueContainerSO dialogueGOOD, DialogueContainerSO dialogueBAD, DialogueContainerSO dialogueNEUTRAL, List<string> ListOfTypesToDESTROY)
    {
        this.dialogueGOOD = dialogueGOOD;
        this.dialogueBAD = dialogueBAD;
        this.dialogueNEUTRAL = dialogueNEUTRAL;
        this.ListOfTypesToDESTROY = ListOfTypesToDESTROY;
    }

    public override void ExitState()
    {
        manager.shouldSearchStates = true;
        StateMachineManager.machineToNotDestory = null;
        onActionFinished?.Invoke();
    }

    public override void OnEndState()
    {
    }

    public override void OnEnterState()
    {
        StateMachineManager.machineToNotDestory = manager;

        foreach (string type in ListOfTypesToDESTROY)
        {
            foreach (Component comp in MonoBehaviour.FindObjectsOfType(Type.GetType(type)))
            {
                MonoBehaviour.Destroy(comp.gameObject);
            }
        }

        manager.shouldSearchStates = false;

        if (!LocalizationManager.IsLocaReady || GameManager.Instance.DialogueManager.isDialogueInProcess) return;

        GameManager.Instance.DialogueManager.OnNode += OnNode;
        GameManager.Instance.DialogueManager.EndDialogueEvent.AddListener(() =>
        {
            GameManager.Instance.DialogueManager.OnNode -= OnNode;
            EventSystem.current.SetSelectedGameObject(null);
            ExitState();
        }
        );

        switch(GameManager.Instance.MemoryManager.storyRelationState)
        {
            case StoryRelationState.Neutral:
                GameManager.Instance.DialogueManager.StartDialogue(dialogueNEUTRAL);
                break;
            case StoryRelationState.Bad:
                GameManager.Instance.DialogueManager.StartDialogue(dialogueBAD);
                break;
            case StoryRelationState.Good:
                GameManager.Instance.DialogueManager.StartDialogue(dialogueGOOD);
                break;
        }
    }

    public override void OnUpdate()
    {
    }

    private void OnNode(BaseNodeData data)
    {
        if (data is DialogueNodeData)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(GameManager.Instance.DialogueUIManager.SkipButton);
            return;
        }

        if (data is DialogueChoiceNodeData || data is TimerChoiceNodeData)
        {
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
