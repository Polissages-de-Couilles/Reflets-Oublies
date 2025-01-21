using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StateMachineManager : MonoBehaviour
{
    [SerializeField] StateBase[] states;
    List<StateEntityBase> stateEntities = new List<StateEntityBase>();
    StateEntityBase currentState;

    [HideInInspector] public bool shouldSearchStates = true;

    public Animator Animator => animator;
    [SerializeField] Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(states.Count());
        GameObject player = GameManager.Instance.Player;
        foreach (StateBase state in states)
        {
            StateEntityBase stateEntity = state.PrepareEntityInstance();
            stateEntity.InitGlobalVariables(this, gameObject, player, state.conditions, state.priority, state.isHostileState, animator, state.animationNames);
            stateEntities.Add(stateEntity);
        }
        setNewCurrentState(-1f);
    }

    // Update is called once per frame
    void Update()
    {
        currentState.OnUpdate();
        if (shouldSearchStates)
        {
            if (currentState.isStateValid())
            {
                setNewCurrentState(currentState.priority);
            }
            else
            {
                setNewCurrentState(-1f);
            }
        }
    }

    void setNewCurrentState(float minExcluStatePrio)
    {
        //Search all valid states that have a priority above minExcluStatePrio, takes all the ones with the highest priority and chose one randomly

        Dictionary<float, List<StateEntityBase>> listStateForPriority = new Dictionary<float, List<StateEntityBase>>();
        foreach (StateEntityBase state in stateEntities)
        {
            if (state.priority > minExcluStatePrio && state.isStateValid())
            {
                if (listStateForPriority.ContainsKey(state.priority))
                {
                    listStateForPriority[state.priority].Add(state);
                }
                else
                {
                    listStateForPriority[state.priority] = new List<StateEntityBase> { state };
                }
            }
        }
        if (listStateForPriority.Keys.Count > 0)
        {
            System.Random rnd = new System.Random();
            List<StateEntityBase> maxPrioList = listStateForPriority[listStateForPriority.Keys.Max()];
            int randIndex = rnd.Next(maxPrioList.Count);
            StateEntityBase highestState = maxPrioList[randIndex];

            //Debug.Log("FOUNDED STATE FOR " + gameObject + " : " + highestState);
            if (currentState != null)
            {
                currentState.onActionFinished -= StateEnded;
                currentState.RemoveHostileFromPlayerState();
                currentState.OnEndState();
            }
            currentState = highestState;
            currentState.AddHostileToPlayerState();
            currentState.OnEnterState();
            currentState.onActionFinished += StateEnded;
        }
    }

    public void forceState(Type targetStateType)
    {
        StateEntityBase foundedState = null;

        foreach (StateEntityBase seb in stateEntities)
        {
            if (targetStateType.IsInstanceOfType(seb))
            {
                foundedState = seb;
            }
        }

        if (foundedState != null)
        {
            //Debug.Log("FORCE STATE FOR " + gameObject + " : " + foundedState);
            shouldSearchStates = true;
            if (currentState != null)
            {
                currentState.onActionFinished -= StateEnded;
                currentState.RemoveHostileFromPlayerState();
                currentState.OnEndState();
            }
            currentState = foundedState;
            currentState.AddHostileToPlayerState();
            currentState.OnEnterState();
            currentState.onActionFinished += StateEnded;
        }
    }

    void StateEnded()
    {
        setNewCurrentState(-1);
    }

    public StateEntityBase getCurrentState()
    {
        return currentState;
    }

    public StateEntityBase GetSpawnState() //Sale mais vas y pour décembre ça ira
    {
        foreach (StateEntityBase seb in stateEntities)
        {
            if (seb is MobSpawnerEntity)
            {
                return seb;
            }
        }

        return null;
    }

    //Debug pour placer des spawners

    void OnDrawGizmos()
    {
        foreach (StateBase seb in states)
        {
            MobSpawner ms = seb as MobSpawner;
            if (ms != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(new Vector3(transform.position.x + ms.spawnRange, transform.position.y, transform.position.z), new Vector3(transform.position.x - ms.spawnRange, transform.position.y, transform.position.z));
                Gizmos.DrawLine(new Vector3(transform.position.x, transform.position.y, transform.position.z + ms.spawnRange), new Vector3(transform.position.x, transform.position.y, transform.position.z - ms.spawnRange));
                break;
            }
        }
    }
}
