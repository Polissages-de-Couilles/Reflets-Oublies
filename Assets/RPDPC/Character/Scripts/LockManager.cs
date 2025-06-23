using Cinemachine;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class LockManager : MonoBehaviour
{
    private PlayerInputEventManager PIE;

    public Lockable CurrentLockObject => currentLockObject;
    private Lockable currentLockObject;

    public Lockable NewLockableObject => newLockableObject;
    private Lockable newLockableObject;
    private bool isSearchingLock = false;

    [SerializeField] GameObject vfxLock;
    [SerializeField] GameObject vfxLockable;
    [SerializeField] Slider enemyHealth;
    [SerializeField] GameObject enemyHealthHolder;
    [SerializeField] GameObject enemyHealthFillStart;

    float currentVignetteIntensity = -1f;
    Tween t;

    private void Start()
    {
        PIE = GameManager.Instance.PlayerInputEventManager;
        PIE.PlayerInputAction.Player.Lock.performed += OnLockPress;

        CinemachineCore.CameraUpdatedEvent.AddListener(OnCameraUpdate);
    }

    public void OnDestroy()
    {

        PIE.PlayerInputAction.Player.Lock.performed -= OnLockPress;
    }

    private void OnLockPress(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.DialogueManager.isDialogueInProcess) return;
        if(t != null && t.IsComplete())
        {
            t = null;
        }

        if(t == null)
        {
            currentVignetteIntensity = GameManager.Instance.CamManager.VignetteIntensity;
        }

        GameManager.Instance.CamManager.Vignette(0.5f, 0.125f, false, true);
        GameManager.Instance.CamManager.VirtualCamera.transform.DOLocalRotate(new Vector3(60, -45, 0), 0.5f);
        t = DOTween.To(() =>
            GameManager.Instance.CamManager.VirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset,
            x => GameManager.Instance.CamManager.VirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset = x,
            new Vector3(0, 0f, 0),
            0.5f);
        isSearchingLock = true;
    }

    private void Lock()
    {
        if(currentLockObject != null && currentLockObject.Damageable != null) currentLockObject.Damageable.OnDamageTaken -= UpdateHealth;
        currentLockObject = newLockableObject;
        currentLockObject.IsLock();

        var damageable = currentLockObject.Damageable;
        enemyHealthHolder.SetActive(currentLockObject.DisplayHealth && damageable != null);
        if(currentLockObject.DisplayHealth && damageable != null)
        {
            enemyHealth.maxValue = damageable.getMaxHealth();
            enemyHealth.value = damageable.getCurrentHealth();
            enemyHealthFillStart.SetActive(damageable.getCurrentHealth() > 0);
            damageable.OnDamageTaken += UpdateHealth;
        }
    }

    private void Unlock()
    {
        currentLockObject.IsUnlock();
        if(currentLockObject != null && currentLockObject.Damageable != null) currentLockObject.Damageable.OnDamageTaken -= UpdateHealth;
        currentLockObject = null;
    }

    public void OnUnLock()
    {
        Unlock();

        if (isSearchingLock)
        {
            FindLockableObject();

            if (newLockableObject != null)
            {
                Lock();
            }
        }
    }

    public void OnLockRelease()
    {
        isSearchingLock = false;
        GameManager.Instance.CamManager.Vignette(0.28f, 0.125f, false, true);
        GameManager.Instance.CamManager.VirtualCamera.transform.DOLocalRotate(new Vector3(45, -45, 0), 0.5f);
        DOTween.To(() =>
            GameManager.Instance.CamManager.VirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset,
            x => GameManager.Instance.CamManager.VirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset = x,
            new Vector3(0, 0.5f, 0),
            0.5f);

        if (currentLockObject != null)
        {
            Unlock();
        }
    }

    private void OnCameraUpdate(CinemachineBrain arg0)
    {
        vfxLock.gameObject.SetActive(currentLockObject != null);
        if(currentLockObject != null)
        {
            Vector3 pos = currentLockObject.transform.position;
            if(currentLockObject.center != null)
            {
                pos = currentLockObject.center.position;
            }
            vfxLock.transform.position = Camera.main.WorldToScreenPoint(pos);
        }

        vfxLockable.gameObject.SetActive(newLockableObject != null && newLockableObject != currentLockObject && newLockableObject.CanBeLock);
        if(newLockableObject != null)
        {
            vfxLockable.transform.position = Camera.main.WorldToScreenPoint(newLockableObject.transform.position);
        }
    }

    private void Update()
    {
        if (PIE.PlayerInputAction.Player.Lock.WasReleasedThisFrame())
        {
            OnLockRelease();
        }

        if(isSearchingLock && GameManager.Instance.CamManager.VignetteIntensity != 0.5f)
        {
            GameManager.Instance.CamManager.Vignette(0.5f, 0.125f, false, true);
        }
        if(!isSearchingLock && GameManager.Instance.CamManager.VignetteIntensity != 0.28f)
        {
            GameManager.Instance.CamManager.Vignette(0.28f, 0.125f, false, true);
        }
        FindLockableObject();
    }

    private void UpdateHealth(float arg1, float arg2)
    {
        enemyHealth.value = arg2;
        enemyHealthFillStart.SetActive(arg2 > 0);
    }

    void FindLockableObject()
    {
        var lockableList = FindObjectsOfType<Lockable>().ToList();
        //Debug.Log(lockableList.Count);
        List<Lockable> visibleLockableObject = new();
        foreach (var lockable in lockableList)
        {
            Vector3 viewPos = Camera.main.WorldToViewportPoint(lockable.transform.position);
            if (viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1 && viewPos.z > 0 && lockable.CanBeLock)
            {
                if (lockable.CanBeLock)
                    visibleLockableObject.Add(lockable);

                //RaycastHit hit;

                //if (Physics.Raycast(this.transform.position, (lockable.transform.position - this.transform.position).normalized, out hit, Mathf.Infinity, layer))
                //{
                //    if (hit.collider.gameObject.Equals(lockable.gameObject))
                //    {
                //        if (Vector3.Distance(this.transform.position, lockable.transform.position) <= lockDistance && lockable.CanBeLock)
                //            visibleLockableObject.Add(lockable);
                //    }
                //}
            }
            else
            {
                if (lockable.IsCurrentlyLock)
                {
                    OnUnLock();
                }
                lockable.IsNotLockable();
            }
        }

        if (visibleLockableObject.Count > 0)
        {
            newLockableObject = visibleLockableObject.OrderBy(x => Vector3.Distance(GameManager.Instance.Player.transform.position, x.transform.position)).FirstOrDefault();
            if (isSearchingLock && currentLockObject == null)
            {
                Lock();
            }
            foreach (var lockable in visibleLockableObject)
            {
                if (lockable.Equals(newLockableObject))
                {
                    lockable.IsLockable();
                }
                else
                {
                    lockable.IsNotLockable();
                }
            }
        }
    }
}