using UnityEngine;

[CreateAssetMenu(menuName = "Game/IA/States/Base/FollowPlayer")]
public class SOFollowPlayer : StateBase
{
    [Tooltip("Does the bot have to go to the last known place even if the conditions are not met anymore")]
    [SerializeField] protected bool isIntelligent;
    [SerializeField] protected bool shouldStopWhenNear;
    [SerializeField] protected float stopNearDistance;
    [SerializeField] protected bool shouldStopWhenFar;
    [SerializeField] protected float stopFarDistance;

    public override StateEntityBase PrepareEntityInstance()
    {
        FollowPlayerEntity fp = new FollowPlayerEntity();
        fp.Init(isIntelligent, shouldStopWhenNear, stopNearDistance, shouldStopWhenFar, stopFarDistance);
        return fp;
    }
}
