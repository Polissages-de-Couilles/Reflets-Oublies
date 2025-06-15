using MeetAndTalk;
using PDC.Localization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DialogueInteraction : Interactible
{
    public DialogueContainerSO Dialogue => _dialogue;
    [SerializeField] DialogueContainerSO _dialogue;

    public enum State
    {
        Waiting,
        Dialogue,
        Choice
    }
    public State state;

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
                //_dialogues.Find(x => x.Act == GameManager.Instance.StoryManager.CurrentAct);
                GameManager.Instance.DialogueManager.StartDialogue(_dialogue);
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

        if (data is DialogueChoiceNodeData || data is TimerChoiceNodeData)
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
