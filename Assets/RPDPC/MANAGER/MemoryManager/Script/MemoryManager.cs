using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryManager : MonoBehaviour
{
    public List<MemorySO> AllMemory => _allMemory;
    [SerializeField] private List<MemorySO> _allMemory = new List<MemorySO>();

    public List<MemorySO> EncounteredMemory => encounteredMemory;
    private List<MemorySO> encounteredMemory;

    public StoryRelationState storyRelationState = StoryRelationState.Neutral;

    private void Awake()
    {
        foreach (MemorySO mem in _allMemory) {
            mem._isTaken = false;
        }
    }

    public void AddEncounteredMemory (MemorySO mem) 
    {
        encounteredMemory.Add(mem);
        GameManager.Instance.FirebaseManager.OnChoiceInStory(mem.Act, mem._isTaken);
        SetStoryRelationState();
    }

    void SetStoryRelationState()
    {
        int nbGood = 0;
        int nbBad = 0;
        foreach (MemorySO mem in encounteredMemory) 
        {
            if (mem._isTaken) { nbGood++; }
            else { nbBad++; }
        }

        if ((nbGood != 0 && nbBad != 0) || nbBad == nbGood)
        {
            storyRelationState = StoryRelationState.Neutral;
            return;
        }
        if (nbGood > nbBad)
        {
            storyRelationState = StoryRelationState.Good;
            return;
        }
        if (nbBad > nbGood)
        {
            storyRelationState = StoryRelationState.Bad;
            return;
        }
    }
}

public enum StoryRelationState
{
    Good,
    Bad,
    Neutral
}
