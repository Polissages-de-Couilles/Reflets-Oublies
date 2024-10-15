using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageManager : MonoBehaviour
{
    public List<TranslatedWord> UnlockedWords => _unlockedWords;
    [SerializeField] private List<TranslatedWord> _unlockedWords;


}
