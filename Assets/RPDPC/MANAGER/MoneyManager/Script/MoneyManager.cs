using DG.Tweening;
using MeetAndTalk.GlobalValue;
using TMPro;
using UnityEngine;

public class MoneyManager : MonoBehaviour 
{
    public int PlayerMonney => _playerMonney;
    [SerializeField] private int _playerMonney;

    public TextMeshProUGUI text;
    GlobalValueInt goldGlobalValue;
    GlobalValueManager manager;


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
        manager = Resources.Load<GlobalValueManager>("GlobalValue");
        manager.LoadFile();
        manager.Set("GOLD", _playerMonney.ToString());
        //goldGlobalValue = manager.IntValues.Find(x => x.ValueName.Equals("GOLD"));
        //goldGlobalValue.Value = _playerMonney;
    }

    public void ChangePlayerMonney(int monney)
    {
        _playerMonney += monney;
        manager.LoadFile();
        manager.Set("GOLD", _playerMonney.ToString());
        //goldGlobalValue.Value = _playerMonney;
        text.text = _playerMonney.ToString();
        text.transform.DOShakePosition(10, 0.2f);
    }
}
