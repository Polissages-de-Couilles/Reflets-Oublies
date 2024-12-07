using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class StateMachineManager : MonoBehaviour
{
    [SerializeField] StateBase[] states;
    List<StateEntityBase> stateEntities = new List<StateEntityBase>();
    StateEntityBase currentState;

    [HideInInspector] public bool shouldSearchStates = true;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(states.Count());
        GameObject player = GameManager.Instance.Player;
        foreach (StateBase state in states)
        {
            StateEntityBase stateEntity = state.PrepareEntityInstance();
            stateEntity.InitGlobalVariables(this, gameObject, player, state.conditions, state.priority, state.isHostileState);
            stateEntities.Add(stateEntity);
        }
        setNewCurrentState(-1f);
    }

    // Update is called once per frame
    void Update()
    {
        currentState.OnUpdate();
        if (shouldSearchStates) {
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

            Debug.Log("FOUNDED STATE FOR " + gameObject + " : " + highestState);
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
}
