using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/IA/States/Base/FollowPlayerwhileGuarding")]
public class SOFollowPlayerWhileGuarding : SOFollowPlayer
{
    [SerializeField] string guardAnim; 
    [SerializeField] string guardHitAnim;

    public override StateEntityBase PrepareEntityInstance()
    {
        FollowPlayerWhileGuardingEntity fp = new FollowPlayerWhileGuardingEntity();
        fp.Init(isIntelligent, guardAnim, guardHitAnim);
        return fp;
    }
}
