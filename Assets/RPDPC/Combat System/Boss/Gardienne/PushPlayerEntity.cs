using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushPlayerEntity : StateEntityBase
{
    Vector3 direction;
    float force;
    float duration;
    float durationOfLiberation;
    float timer = 0f;

    public override void Init(Vector3 direction, float force, float duration, float durationOfLiberation)
    {
        this.direction = direction;
        this.force = force;
        this.duration = duration;
        this.durationOfLiberation = durationOfLiberation;
    }

    public override void ExitState()
    {
        manager.shouldSearchStates = true;
        onActionFinished?.Invoke();
    }

    public override void OnEndState()
    {
        timer = 0f;
    }

    public override void OnEnterState()
    {
        parent.GetComponent<GardienneParticleHolder>().Switch(GardienneWind.ReallyStrong, true);
        manager.shouldSearchStates = false;
        player.GetComponent<PlayerStunAndKnockbackManager>().ApplyStun(duration);
        manager.StartCoroutine(DOPush());
    }

    public override void OnUpdate()
    {
        if (timer > durationOfLiberation)
        {
            ExitState();
        }
    }

    public IEnumerator DOPush()
    {
        while (timer < duration)
        {
            //Vector3 dif = (player.transform.position - center).normalized;
            //dif.y = 0;

            player.GetComponent<CharacterController>().Move(direction * force * Time.deltaTime);
            yield return null;
            timer += Time.deltaTime;
        }
        yield return null;
        parent.GetComponent<GardienneParticleHolder>().Unlock();
        parent.GetComponent<GardienneParticleHolder>().Switch(GardienneWind.Strong);
    }
}
