using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyManager : MonoBehaviour 
{
    public int PlayerMonney => _playerMonney;
    [SerializeField] private int _playerMonney = 100;

    public TextMeshProUGUI text;

    public void Start()
    {
        text.text = _playerMonney.ToString();
    }

    public void ChangePlayerMonney(int monney)
    {
        _playerMonney += monney;
        text.text = _playerMonney.ToString();
    }
}
