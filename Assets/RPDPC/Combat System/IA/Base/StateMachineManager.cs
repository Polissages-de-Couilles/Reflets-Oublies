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

    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameManager.Instance.Player;
        setNewCurrentState(-1f);
        foreach (StateBase state in states)
        {
            state.Init(gameObject, player);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState.isStateValid())
        {
            setNewCurrentState(currentState.priority);
        }
        else 
        {
            setNewCurrentState(-1f);
        }
        currentState.OnUpdate();
    }

    void setNewCurrentState(float minExcluStatePrio)
    {
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
            List<StateBase> maxPrioList = listStateForPriority[new List<float>(listStateForPriority.Keys).Max()];
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
