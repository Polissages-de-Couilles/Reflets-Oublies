using Cinemachine;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class LockManager : MonoBehaviour
{
    private PlayerInputEventManager PIE;

    public Lockable CurrentLockObject => currentLockObject;
    private Lockable currentLockObject;

    public Lockable NewLockableObject => newLockableObject;
    private Lockable newLockableObject;
    private bool isSearchingLock = false;

    private void Start()
    {
        PIE = GameManager.Instance.PlayerInputEventManager;
        PIE.PlayerInputAction.Player.Lock.performed += OnLockPress;
    }

    private void OnLockPress(InputAction.CallbackContext context)
    {
        isSearchingLock = true;
    }

    private void Lock()
    {
        currentLockObject = newLockableObject;
        currentLockObject.IsLock();

        GameManager.Instance.CamManager.VirtualCamera.transform.DOLocalRotate(new Vector3(50, -45, 0), 0.5f);
        DOTween.To(() => 
            GameManager.Instance.CamManager.VirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset, 
            x => GameManager.Instance.CamManager.VirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset = x, 
            new Vector3(0, 1.5f, 0), 
            0.5f);
    }

    private void Unlock()
    {
        currentLockObject.IsUnlock();
        currentLockObject = null;

        GameManager.Instance.CamManager.VirtualCamera.transform.DOLocalRotate(new Vector3(45, -45, 0), 0.5f);
        DOTween.To(() =>
            GameManager.Instance.CamManager.VirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset,
            x => GameManager.Instance.CamManager.VirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset = x,
            new Vector3(0, 0.5f, 0),
            0.5f);
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

        if (currentLockObject != null)
        {
            Unlock();
        }
    }

    private void Update()
    {
        if (PIE.PlayerInputAction.Player.Lock.WasReleasedThisFrame())
        {
            OnLockRelease();
        }
        FindLockableObject();
    }

    void FindLockableObject()
    {
        var lockableList = FindObjectsOfType<Lockable>().ToList();
        Debug.Log(lockableList.Count);
        List<Lockable> visibleLockableObject = new();
        foreach (var lockable in lockableList)
        {
            Vector3 viewPos = Camera.main.WorldToViewportPoint(lockable.transform.position);
            if (viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1 && viewPos.z > 0)
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
            newLockableObject = visibleLockableObject.OrderBy(x => Vector3.Distance(this.transform.position, x.transform.position)).FirstOrDefault();
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