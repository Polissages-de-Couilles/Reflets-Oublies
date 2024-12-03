using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // UI Elements
    [Header("UI Elements")]
    public GameObject Pause, Dialogue, HUD, MainMenu, Option;
    public GameObject GlobalOption, inputManetteOption, inputKeybordOption;

    // Scene Management
    [Header("Scene Management")]
    public string SceneGameToLoad, SceneMenuToLoad;

    // Player
    [Header("Player")]
    private PlayerDamageable player;
    public Slider healthSlider, dashSlider;

    // Effects
    [Header("Effects")]
    public GameObject bloodEffect;
    private Image bloodEffectImage;

    // Wave Animation
    [Header("Wave Animation")]
    public GameObject objectToSpawn, parentObject;
    public float animationDuration = 2f;

    private bool isSpawning = false;

    private void Start()
    {
        InitializePlayer();
        InitializeBloodEffect();
        StartCoroutine(SpawnAnimationCoroutine());
    }

    private void InitializePlayer()
    {
        player = GameManager.Instance.Player.GetComponent<PlayerDamageable>();
        player.OnDamageTaken += PointDeVie;

        if (healthSlider != null)
        {
            healthSlider.maxValue = player.getMaxHealth();
            healthSlider.value = player.getCurrentHealth();
        }

        PointDeVie(0, player.getCurrentHealth());
    }

    private void InitializeBloodEffect()
    {
        bloodEffectImage = bloodEffect.GetComponent<Image>();
        if (bloodEffectImage == null) return;
        UpdateOpacity();
    }

    private void PointDeVie(float degatPrit, float vieActuel)
    {
        if (healthSlider != null)
            healthSlider.value = vieActuel;

        UpdateOpacity();
    }

    public void UpdateOpacity()
    {
        if (bloodEffectImage != null)
        {
            float healthPercentage = player.getCurrentHealth() / player.getMaxHealth();
            bloodEffectImage.color = new Color(bloodEffectImage.color.r, bloodEffectImage.color.g, bloodEffectImage.color.b, 1 - healthPercentage);
        }
    }

    public void UpdateSlider()
    {
        if (healthSlider != null)
            healthSlider.value = player.getCurrentHealth();

        UpdateOpacity();
    }

    public void InfligerDegats()
    {
        player.takeDamage(10);
    }

    public void Heal()
    {
        player.heal(10);
        UpdateSlider();
    }

    public void UpdateDashSlider(int dashCount)
    {
        if (dashSlider != null)
            dashSlider.value = dashCount;
    }

    private IEnumerator SpawnAnimationCoroutine()
    {
        isSpawning = true;

        while (true)
        {
            if (parentObject != null)
            {
                Vector3 spawnPosition = new Vector3(0f, -80f, parentObject.transform.position.z);
                GameObject clone = Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
                clone.transform.SetParent(parentObject.transform);
                clone.transform.localPosition = new Vector3(0f, -80f, clone.transform.localPosition.z);

                Animator animator = clone.GetComponent<Animator>();
                if (animator != null)
                    animator.SetTrigger("PlayAnimation");

                yield return new WaitForSeconds(animationDuration);
                Destroy(clone);
            }

            yield return new WaitForSeconds(animationDuration);
        }
    }
}
