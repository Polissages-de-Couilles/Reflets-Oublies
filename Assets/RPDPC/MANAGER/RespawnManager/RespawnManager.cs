using DG.Tweening;
using MeetAndTalk;
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

    [SerializeField] Transform _respawnPoint;
    [SerializeField] DialogueContainerSO _dialogueRevive;
    [SerializeField] Image _fade;

    bool asBeenToVillage = false;
    bool isRespawning = false;

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
        //GameManager.Instance.CamManager.Vignette(VignetteIntensity, 1f, true, true);

        if (!asBeenToVillage)
        {
            SceneManager.LoadScene("GameScene");
            isRespawning = false;
            yield break;
        }

        GameManager.Instance.RespawnManager.DeathUi.SetActive(false);

        GameObject player = GameManager.Instance.Player;
        player.transform.position = _respawnPoint.position;
        player.GetComponent<StateManager>().FORCESetPlayerState(StateManager.States.idle);
        PlayerDamageable pd = player.GetComponent<PlayerDamageable>();
        pd.heal(pd.maxHealth);
        player.GetComponent<CharacterController>().enabled = true;

        GameManager.Instance.UIManager.GetComponent<BossBarManager>().ResetBar();

        foreach (BossRespawn br in FindObjectsByType<BossRespawn>(FindObjectsSortMode.None)) br.Respawn();

        _fade.DOColor(new Color(_fade.color.r, _fade.color.g, _fade.color.b, 0f), 3f);

        if (_dialogueRevive != null) GameManager.Instance.DialogueManager.StartDialogue(_dialogueRevive);
        isRespawning = false;
    }
}
