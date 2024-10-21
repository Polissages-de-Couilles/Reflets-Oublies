using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyManager : MonoBehaviour 
{
    public int PlayerMonney => _playerMonney;
    private int _playerMonney;

    public void ChangePlayerMonney(int monney)
    {
        _playerMonney += monney;
    }
}
