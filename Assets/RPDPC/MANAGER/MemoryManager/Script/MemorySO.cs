using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Memory", menuName = ("Game/Memory"), order =1)]
public class MemorySO : ScriptableObject
{
    public Action _action;
    public bool _disposed = false;
    public int relationValue;
    public int defenceValue;
    [SerializeField] private List<TranslatedWord> translatedWords = new List<TranslatedWord>();

    public void RunEvent()
    {
        _action?.Invoke();
    }
}
