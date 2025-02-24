using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryManager : MonoBehaviour
{
    public Act CurrentAct => _currentAct;
    [SerializeField] private Act _currentAct;
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