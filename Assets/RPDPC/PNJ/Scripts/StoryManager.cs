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
    Act1,
    Act2,
    Act3,
    Act4,
    Act5,
    Act6
}