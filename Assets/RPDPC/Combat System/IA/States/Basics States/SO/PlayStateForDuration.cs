using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/IA/States/Base/PlayStateForDuration")]
public class PlayStateForDuration : StateBase
{
    [SerializeField] StateBase m_State;
    [SerializeField] float m_Duration;
    public override StateEntityBase PrepareEntityInstance()
    {
        PlayStateForDurationEntity playStateForDurationEntity = new PlayStateForDurationEntity();
        playStateForDurationEntity.Init(m_State, m_Duration);
        return playStateForDurationEntity;
    }
}
