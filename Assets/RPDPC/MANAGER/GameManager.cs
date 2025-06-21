using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PDC.Localization;
using MeetAndTalk;
using PDC;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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

    public MemoryManager MemoryManager => _memoryManager;
    [SerializeField] private MemoryManager _memoryManager;

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

    public InputActionAsset InputAction => _inputAction;
    [SerializeField] InputActionAsset _inputAction;

    public VFXManager VFXManager => _vfxManager;
    [SerializeField] private VFXManager _vfxManager;

    public LockManager LockManager => _lockManager;
    [SerializeField] private LockManager _lockManager;

    public StoryManager StoryManager => _storyManager;
    [SerializeField] private StoryManager _storyManager;

    public AudioSettings AudioManager => _audioSettings;
    [SerializeField] private AudioSettings _audioSettings;

    public FirebaseManager FirebaseManager => _firebaseManager;
    [SerializeField] private FirebaseManager _firebaseManager;

    public ZoneManager ZoneManager => _zoneManager;
    [SerializeField] private ZoneManager _zoneManager;

    public RespawnManager RespawnManager => _respawnManager;
    [SerializeField] private RespawnManager _respawnManager;

    public Image FadeObject => _fadeObject;
    [SerializeField] Image _fadeObject;

    public void Start()
    {
        //Application.ForceCrash(0);
    }

#if UNITY_EDITOR
    //Uniquement lÅEÅEbut de test, ne pas utiliser pour la version final
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            //Player.GetComponent<PlayerDamageable>().takeDamage(20, null);
        }
    }
#endif
}
