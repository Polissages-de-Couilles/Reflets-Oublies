using UnityEngine;

[CreateAssetMenu(menuName = "Game/IA/States/Base/FollowPlayer")]
public class SOFollowPlayer : StateBase
{
    [Tooltip("Does the bot have to go to the last known place even if the conditions are not met anymore")]
    [SerializeField] bool isIntelligent;

    public override StateEntityBase PrepareEntityInstance()
    {
        FollowPlayerEntity fp = new FollowPlayerEntity();
        fp.Init(isIntelligent, null, null, false, new Vector3(), 0, false, false, new Vector2(), null, 0, 0, 0, Vector2.zero, 0, null, false, animationNames);
        return fp;
    }
}
