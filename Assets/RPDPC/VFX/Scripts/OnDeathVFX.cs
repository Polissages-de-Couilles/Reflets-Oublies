using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BotDeathManager))]
public class OnDeathVFX : MonoBehaviour
{
    [SerializeField] GameObject _onDeathVFX;
    [SerializeField] float _delay = 0;
    BotDeathManager deathManager;

    public void Start()
    {
        deathManager = GetComponent<BotDeathManager>();
        deathManager.OnBotDied += VFX;
    }

    private void OnDestroy()
    {
        deathManager.OnBotDied -= VFX;
    }

    void VFX()
    {
        IEnumerator VFXCoroutine()
        {
            yield return new WaitForSeconds(_delay);
            GameManager.Instance.AudioManager.PlayDeathBot();
            Instantiate(_onDeathVFX, this.transform.position, Quaternion.Euler(-90, 0, 0));
        }
        StartCoroutine(VFXCoroutine());
    }
}
