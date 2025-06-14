using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyChild : StagingEvent
{
    public override void PlayEvent()
    {
        base.PlayEvent();

        if(transform.childCount > 0)
        {
            for(int i = transform.childCount - 1; i >= 0; i--)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }

        OnEventFinished?.Invoke();
    }
}
