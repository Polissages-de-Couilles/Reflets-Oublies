using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushPlayerEntity : StateEntityBase
{
    Vector3 direction;
    float force;
    float duration;
    float timer = 0f;

    public override void Init(Vector3 direction, float force, float duration)
    {
        this.direction = direction;
        this.force = force;
        this.duration = duration;
    }

    public override void ExitState()
    {
        manager.shouldSearchStates = true;
        onActionFinished?.Invoke();
    }

    public override void OnEndState()
    {
        parent.GetComponent<GardienneParticleHolder>().Switch(GardienneWind.Static);
        timer = 0f;
    }

    public override void OnEnterState()
    {
        parent.GetComponent<GardienneParticleHolder>().Switch(GardienneWind.ReallyStrong);
        manager.shouldSearchStates = false;
        player.GetComponent<PlayerStunAndKnockbackManager>().ApplyStun(duration);
    }

    public override void OnUpdate()
    {
        if (timer < duration)
        {
            //Vector3 dif = (player.transform.position - center).normalized;
            //dif.y = 0;

            player.GetComponent<CharacterController>().Move(direction * force * Time.deltaTime);
        }
        else
        {
            ExitState();
        }
        timer += Time.deltaTime;
    }
}
