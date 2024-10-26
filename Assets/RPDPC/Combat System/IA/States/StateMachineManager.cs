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
    StateBase currentState;

    bool shouldSearchStates = true;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(states.Count());
        GameObject player = GameManager.Instance.Player;
        foreach (StateBase state in states)
        {
            Debug.Log("STATE");
            state.BaseInit(this, gameObject, player);
        }
        setNewCurrentState(-1f);
    }

    // Update is called once per frame
    void Update()
    {
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
        currentState.OnUpdate();
    }

    void setNewCurrentState(float minExcluStatePrio)
    {
        //Search all valid states that have a priority above minExcluStatePrio, takes all the ones with the highest priority and chose one randomly

        Dictionary<float, List<StateBase>> listStateForPriority = new Dictionary<float, List<StateBase>>();
        foreach (StateBase state in states)
        {
            if (state.priority > minExcluStatePrio && state.isStateValid())
            {
                if (listStateForPriority.ContainsKey(state.priority))
                {
                    listStateForPriority[state.priority].Add(state);
                }
                else
                {
                    listStateForPriority[state.priority] = new List<StateBase> { state };
                }
            }
        }
        if (listStateForPriority.Keys.Count > 0)
        {
            System.Random rnd = new System.Random();
            List<StateBase> maxPrioList = listStateForPriority[listStateForPriority.Keys.Max()];
            int randIndex = rnd.Next(maxPrioList.Count);
            StateBase highestState = maxPrioList[rnd.Next(maxPrioList.Count)];

            Debug.Log("FOUNDED STATE = " + highestState);
            if (currentState != null)
            {
                currentState.ExitState();
            }
            currentState = highestState;
            currentState.OnEnterState();
        }
    }
}
