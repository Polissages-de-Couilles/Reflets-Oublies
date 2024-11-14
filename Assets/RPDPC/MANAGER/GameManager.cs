using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PDC.Localization;
using MeetAndTalk;
using PDC;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance => _instance;
    private static GameManager _instance;
    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
    }

    public GameObject Player => _player;
    [SerializeField] GameObject _player;

    public LocalizationManager LocalizationManager => _localizationManager;
    [SerializeField] LocalizationManager _localizationManager;

    public CinemachineEffectManager CamManager => _camManager;
    [SerializeField] CinemachineEffectManager _camManager;

    public PlayerInputEventManager PlayerInputEventManager => _playerInputEventManager;
    [SerializeField] PlayerInputEventManager _playerInputEventManager;

    public LanguageManager LanguageManager => _languageManager;
    [SerializeField] LanguageManager _languageManager;

    public MoneyManager MoneyManager => _moneyManager;
    [SerializeField] private MoneyManager _moneyManager;

    public UIManager UIManager => _uiManager;
    [SerializeField] private UIManager _uiManager;

    public PotionManager PotionManager => _potionManager;
    [SerializeField] private PotionManager _potionManager;

    public RelationManager RelationManager => _relationManager;
    [SerializeField] private RelationManager _relationManager;

    public DialogueManager DialogueManager => MeetAndTalk.DialogueManager.Instance;
    public DialogueUIManager DialogueUIManager => MeetAndTalk.DialogueUIManager.Instance;

    public PdCManager PdCManager => _pdcManager;
    [SerializeField] PdCManager _pdcManager;

    public GameObject AudioDialogueGameObject => _audioDialogueGameObject;
    [SerializeField] GameObject _audioDialogueGameObject;

    //Uniquement là à but de test, ne pas utiliser pour la version final

    public void Update()
    {
        //if(Input.GetKeyDown(KeyCode.T))
        //{
        //    Player.GetComponent<PlayerDamageable>().takeDamage(20);
        //}
    }
}
