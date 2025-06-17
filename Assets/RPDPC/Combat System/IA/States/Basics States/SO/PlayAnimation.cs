using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/IA/States/Base/PlayAnimation")]
public class PlayAnimationSO : StateBase
{
    [SerializeField] string animName;
    [SerializeField] AnimationClip clip;
    [SerializeField] float setDuration;
    [SerializeField] int layer;
    [SerializeField] float speed = 1;
    [SerializeField] string speedPararemerName;


    public override StateEntityBase PrepareEntityInstance()
    {
        PlayAnimationEntity pae = new PlayAnimationEntity();
        pae.Init(animName, clip, setDuration, layer, speed, speedPararemerName);
        return pae;
    }
}
