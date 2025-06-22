using MeetAndTalk.GlobalValue;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryManager : MonoBehaviour
{
    public List<MemorySO> AllMemory => _allMemory;
    [SerializeField] private List<MemorySO> _allMemory = new List<MemorySO>();

    public List<MemorySO> EncounteredMemory => encounteredMemory;
    [SerializeField] List<MemorySO> encounteredMemory = new();

    public StoryRelationState storyRelationState = StoryRelationState.Neutral;

    [SerializeField] AudioClip _buff;
    [SerializeField] AudioClip _debuff;
    [SerializeField] AudioSource _source;


    private void Awake()
    {
        foreach (MemorySO mem in _allMemory) {
            mem._isTaken = false;
        }
    }

    public void AddEncounteredMemory(MemorySO mem)
    {
        encounteredMemory.Add(mem);
        GameManager.Instance.FirebaseManager.OnChoiceInStory(mem.Act, mem._isTaken);
        var corruption = (float)encounteredMemory.FindAll(x => !x._isTaken).Count / (float)_allMemory.Count;
        StartCoroutine(GameManager.Instance.CamManager.ColorCurves(corruption, 2f));

        if(mem._isTaken)
        {
            _source.PlayOneShot(_debuff);
        }
        else
        {
            _source.PlayOneShot(_buff);
        }

        if(GameManager.Instance.Player.TryGetComponent(out PlayerDamageable damageable))
        {
            damageable.SetMaxHealth(damageable.getMaxHealth() + (mem._isTaken ? -10 : 20));
        }

        Debug.Log(Mathf.Clamp(GameManager.Instance.MemoryManager.EncounteredMemory.FindAll(x => x._isTaken).Count + 1, 0, 5) + " > " + GameManager.Instance.PotionManager.MaxPotion);
        bool canBuy = Mathf.Clamp(GameManager.Instance.MemoryManager.EncounteredMemory.FindAll(x => x._isTaken).Count + 1, 0, 5) > GameManager.Instance.PotionManager.MaxPotion;
        var manager = Resources.Load<GlobalValueManager>("GlobalValue");
        manager.LoadFile();
        manager.Set("CAN_BUY_POTION_NB", (canBuy ? 1 : 0).ToString());

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

        storyRelationState = StoryRelationState.Neutral;
        return;
    }
}

public enum StoryRelationState
{
    Good,
    Bad,
    Neutral
}
