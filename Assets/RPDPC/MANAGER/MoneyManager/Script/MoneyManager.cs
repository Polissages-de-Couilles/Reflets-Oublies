using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyManager : MonoBehaviour 
{
    public int PlayerMonney => _playerMonney;
    [SerializeField] private int _playerMonney;

    public TextMeshProUGUI text;


    private void Awake()
    {
        _playerMonney = 100;
    }
    public void Start()
    {
        if(text != null)
        {
            text.text = _playerMonney.ToString();
        }
    }

    public void ChangePlayerMonney(int monney)
    {
        _playerMonney += monney;
        text.text = _playerMonney.ToString();
    }
}
