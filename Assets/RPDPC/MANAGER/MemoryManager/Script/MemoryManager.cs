using DG.Tweening;
using MeetAndTalk.GlobalValue;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ZoneManager;

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

    [SerializeField] RectTransform _newWordHolder;


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
            StartCoroutine(DisplayNewWord());
        }
        else
        {
            _source.PlayOneShot(_buff);
        }

        if(GameManager.Instance.Player.TryGetComponent(out PlayerDamageable damageable))
        {
            damageable.SetMaxHealth(damageable.getMaxHealth() + (mem._isTaken ? -5 : 20));
            if(!mem._isTaken) StartCoroutine(Heal(damageable));
        }

        GameManager.Instance.PotionManager.AddMaxPotion(true);
        GameManager.Instance.PotionManager.RefillPotion(true);

        Debug.Log(Mathf.Clamp(GameManager.Instance.MemoryManager.EncounteredMemory.FindAll(x => x._isTaken).Count + 1, 0, 5) + " > " + GameManager.Instance.PotionManager.MaxPotion);
        bool canBuy = Mathf.Clamp(GameManager.Instance.MemoryManager.EncounteredMemory.FindAll(x => x._isTaken).Count + 1, 0, 5) > GameManager.Instance.PotionManager.MaxPotion;
        var manager = Resources.Load<GlobalValueManager>("GlobalValue");
        manager.LoadFile();
        manager.Set("CAN_BUY_POTION_NB", (canBuy ? 1 : 0).ToString());

        SetStoryRelationState();
    }

    IEnumerator Heal(PlayerDamageable damageable)
    {
        yield return new WaitForSeconds(_buff.length);
        damageable.heal(damageable.getMaxHealth() - damageable.getCurrentHealth());
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

        if (nbBad == 0 && nbGood > 0)
        {
            storyRelationState = StoryRelationState.Good;
            return;
        }
        if (nbGood == 0 && nbBad > 0)
        {
            storyRelationState = StoryRelationState.Bad;
            return;
        }

        storyRelationState = StoryRelationState.Neutral;
        return;
    }

    IEnumerator DisplayNewWord()
    {
        var pos = _newWordHolder.localPosition;
        var t = _newWordHolder.DOLocalMove(pos - new Vector3(0, 175f, 0), 1f);
        yield return t.WaitForCompletion();
        yield return new WaitForSeconds(1f);
        t = _newWordHolder.DOLocalMove(pos, 1f);
        yield return t.WaitForCompletion();
    }
}

public enum StoryRelationState
{
    Good,
    Bad,
    Neutral
}
