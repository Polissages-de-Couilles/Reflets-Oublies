using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPDPC_Manager : MonoBehaviour
{
    public List<TranslatedWord> UnlockedWords => _unlockedWords;
    [SerializeField] private List<TranslatedWord> _unlockedWords;


}
