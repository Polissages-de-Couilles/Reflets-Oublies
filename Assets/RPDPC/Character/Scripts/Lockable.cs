using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lockable : MonoBehaviour
{
    public bool CanBeLock { 
        get { return canBeLock; }
        set { canBeLock = value; }
    }
    private bool canBeLock = true;
    public bool IsCurrentlyLock => isLock;
    private bool isLock = false;
    private bool isLockable = true;

    private void Update()
    {
        if (/*Vector3.Distance(GameManager.Instance.LockManager.transform.position, this.transform.position) > distanceToUnlock && isLock ||*/ isLock && !CanBeLock)
        {
            GameManager.Instance.LockManager.OnUnLock();
        }
    }

    private void OnDestroy()
    {
        if (isLock)
        {
            GameManager.Instance.LockManager.OnUnLock();
        }
    }

    public void IsLockable()
    {
        isLockable = true;
    }

    public void IsNotLockable()
    {
        isLockable = false;
    }

    public void IsLock()
    {
        isLock = true;
        Debug.Log(this.name + " is Lock");
    }

    public void IsUnlock()
    {
        isLock = false;
        Debug.Log(this.name + " is Unlock");
    }
}
