using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryManager : MonoBehaviour
{
    public Act CurrentAct => _currentAct;
    [SerializeField] private Act _currentAct;

    [SerializeField] List<GameObject> _actObject;
    [SerializeField] List<GameObject> _goodObject;
    [SerializeField] List<GameObject> _badObject;
    [SerializeField] List<GameObject> _neutralObject;

    public void SwitchAct(Act act)
    {
        _currentAct = act;
        var state = GameManager.Instance.MemoryManager.storyRelationState;

        foreach (var a in _actObject)
        {
            a.SetActive(false);
        }
        _actObject[(int)act].SetActive(true);

        foreach (var good in _goodObject)
        {
            good.SetActive(false);
        }
        foreach (var bad in _badObject)
        {
            bad.SetActive(false);
        }
        foreach (var neutral in _neutralObject)
        {
            neutral.SetActive(false);
        }

        switch (state)
        {
            case StoryRelationState.Good:
                foreach (var good in _goodObject)
                {
                    good.SetActive(true);
                }
                break;
            case StoryRelationState.Bad:
                foreach (var bad in _badObject)
                {
                    bad.SetActive(true);
                }
                break;
            case StoryRelationState.Neutral:
                foreach (var neutral in _neutralObject)
                {
                    neutral.SetActive(true);
                }
                break;
            default:
                break;
        }
    }

    public void Start()
    {
        SwitchAct(Act.ACT_1);
    }
}

public enum Act
{
    ACT_1,
    ACT_2,
    ACT_3,
    ACT_4,
    ACT_5,
    ACT_6
}