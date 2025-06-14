using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Memory", menuName = ("Game/Memory"), order =1)]
public class MemorySO : ScriptableObject
{
    public Act Act;
    public Action<bool> _action;
    public bool _isTaken = false;
    public int relationValue;
    public float defenceValue;
    [SerializeField] private List<TranslatedWord> translatedWords = new List<TranslatedWord>();

    public void RunEvent(bool seeIt)
    {
        _isTaken = seeIt;
        if (_isTaken == false)
        {
            GameManager.Instance.RelationManager.ChangeValue(-relationValue);
            GameManager.Instance.Player.GetComponent<PlayerDamageable>().ChangeDefence(-defenceValue);
        }
        else
        {
            GameManager.Instance.RelationManager.ChangeValue(relationValue);
            GameManager.Instance.Player.GetComponent<PlayerDamageable>().ChangeDefence(defenceValue);
            foreach (TranslatedWord word in translatedWords)
            {
                if(!GameManager.Instance.LanguageManager.UnlockedWords.Find(x => x.Word == word.Word))
                    GameManager.Instance.LanguageManager.UnlockedWords.Add(word);
            }
        }



        _action?.Invoke(_isTaken);        
    }
}
