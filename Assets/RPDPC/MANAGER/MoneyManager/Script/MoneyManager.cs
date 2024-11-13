using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyManager : MonoBehaviour 
{
    public int PlayerMonney => _playerMonney;
    private int _playerMonney;

    public TextMeshProUGUI text;

    public void ChangePlayerMonney(int monney)
    {
        _playerMonney += monney;
        text.text = _playerMonney.ToString();
    }
}
