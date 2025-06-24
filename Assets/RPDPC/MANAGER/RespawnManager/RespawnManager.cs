using DG.Tweening;
using MeetAndTalk;
using PDC;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RespawnManager : MonoBehaviour
{
    public GameObject DeathUi => _deathUi;
    [SerializeField] GameObject _deathUi;

    public Transform _respawnPoint;
    public ZoneManager.ZoneName _respawnZone;
    [SerializeField] DialogueContainerSO _dialogueRevive;
    [SerializeField] Image _fade;

    bool asBeenToVillage = false;
    bool isRespawning = false;

    public bool isInFinalTemple { get; set; } = false;
    public Transform _respawnPointFinalTemple;

    public float VignetteIntensity { get; set; }

    public void Start()
    {
        GameManager.Instance.ZoneManager.OnZoneChangeEvent += OnZoneChange;
        if(_fade != null)
        {
            _fade.gameObject.SetActive(true);
            _fade.DOColor(new Color(_fade.color.r, _fade.color.g, _fade.color.b, 0f), 3f);
        }
    }

    private void OnZoneChange(ZoneManager.Zone zone)
    {
        if (zone.Name == ZoneManager.ZoneName.UI_ZONE_3) asBeenToVillage = true;
        if (zone.Name == ZoneManager.ZoneName.UI_ZONE_11) isInFinalTemple = true;
    }

    public void Respawn()
    {
        if (isRespawning) return;
        isRespawning = true;
        Time.timeScale = 1f;
        StartCoroutine(RespawnCoroutine());
    }

    private IEnumerator RespawnCoroutine()
    {
        yield return null;
        yield return _fade.DOColor(new Color(_fade.color.r, _fade.color.g, _fade.color.b, 1f), 1.5f).WaitForCompletion();
        GameManager.Instance.CamManager.Vignette(0.28f, 1f, false, true);

        if (!asBeenToVillage)
        {
            isRespawning = false;
            LoadSceneManager.Instance.LoadScene("GameScene");
            yield break;
        }

        GameManager.Instance.RespawnManager.DeathUi.SetActive(false);

        GameObject player = GameManager.Instance.Player;
        player.transform.position = isInFinalTemple ? _respawnPointFinalTemple.position : _respawnPoint.position;
        player.GetComponent<StateManager>().FORCESetPlayerState(StateManager.States.idle);
        PlayerDamageable pd = player.GetComponent<PlayerDamageable>();
        pd.heal(pd.maxHealth);
        player.GetComponent<CharacterController>().enabled = true;
        player.GetComponentInChildren<Animator>().Play("Walk");

        GameManager.Instance.UIManager.GetComponent<BossBarManager>().ResetBar();

        foreach (BossRespawn br in FindObjectsByType<BossRespawn>(FindObjectsSortMode.None)) br.Respawn();

        GameManager.Instance.PotionManager.RefillPotion(true);

        //GameManager.Instance.AudioManager.ForceExitCombat();
        if(!isInFinalTemple) GameManager.Instance.ZoneManager.OnZoneChange(GameManager.Instance.ZoneManager.GetZone(_respawnZone), true);

        ResetPV();

        _fade.DOColor(new Color(_fade.color.r, _fade.color.g, _fade.color.b, 0f), 3f);

        if (_dialogueRevive != null) GameManager.Instance.DialogueManager.StartDialogue(_dialogueRevive);
        isRespawning = false;
    }

    void ResetPV()
    {
        PlayerDamageable pd = GameManager.Instance.Player.GetComponent<PlayerDamageable>();
        float desiredHealth = 100;
        foreach (MemorySO mem in GameManager.Instance.MemoryManager.EncounteredMemory)
        {
            if (mem._isTaken)
            {
                desiredHealth -= 5;
            }
            else
            {
                desiredHealth += 20;
            }
        }
        if(pd.maxHealth != desiredHealth) pd.SetMaxHealth(desiredHealth);
    }

    public void GoToCredit()
    {
        SceneManager.LoadScene(2);
    }
}
