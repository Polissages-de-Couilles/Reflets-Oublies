using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StagingSetActiveBoss : StagingEvent
{
    [SerializeField] Perso character;
    [SerializeField] bool isActive;

    public override void PlayEvent()
    {
        base.PlayEvent();
        if(character != Perso.None && character != Perso.Player)
        {
            var obj = GameObject.FindGameObjectWithTag(character.ToString());
            if(obj != null)
            {
                obj/*.transform.parent.gameObject*/.SetActive(isActive);
            }
        }
        OnEventFinished?.Invoke();
    }
}
