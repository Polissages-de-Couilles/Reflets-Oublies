using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/IA/States/Base/PlayManyStates")]
public class PlayManyStates : StateBase
{
    [SerializeField] List<StateListItem> States = new List<StateListItem>();

    public override StateEntityBase PrepareEntityInstance()
    {
        PlayManyStatesEntity pmse = new PlayManyStatesEntity();
        pmse.Init(States);
        return pmse;
    }
}

[System.Serializable]
public struct StateListItem
{
    public float delayBeforePlay;
    public StateBase state;
}
