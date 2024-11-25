using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BotStunAndKnockbackManager : StunAndKnockbackManagerBase
{
    [DoNotSerialize] public bool Stunned = false;
    [SerializeField] bool canBeStunned = true;

    public override void ApplyStun(float stunDuration)
    {
        if (canBeStunned)
        {
            StopAllCoroutines();
            Stunned = true;
            StartCoroutine(cancelStun(stunDuration));
        }
    }
    IEnumerator cancelStun(float duration) 
    { 
        yield return new WaitForSeconds(duration); 
        Stunned = false; 
    }
}
