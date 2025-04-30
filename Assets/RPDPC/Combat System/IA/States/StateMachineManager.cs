using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StateMachineManager : MonoBehaviour
{
    [SerializeField] StateBase[] states;
    List<StateEntityBase> stateEntities = new List<StateEntityBase>();
    StateEntityBase currentState;
    bool prioritizeAttack = true;

    Dictionary<StateEntityBase, int> numberOfTimeGoesIntoState = new Dictionary<StateEntityBase, int>();

    [HideInInspector] public bool shouldSearchStates = true;

    public Animator Animator => animator;
    [SerializeField] Animator animator;
    static List<StateMachineManager> alreadyStopedStateMachines = new List<StateMachineManager>();

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
            numberOfTimeGoesIntoState.Add(stateEntity, 0);
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
        Dictionary<float, List<StateEntityBase>> listStateWithAbsolutePiority = new Dictionary<float, List<StateEntityBase>>();
        foreach (StateEntityBase state in stateEntities)
        {
            if ((prioritizeAttack && (state is AttackEntity || state is ProjectileAttackEntity)) || state is StunEntity || state is TalkingEntity)
            {
                if (currentState is AttackEntity || currentState is ProjectileAttackEntity || currentState is StunEntity || currentState is TalkingEntity)
                {
                    if (state.priority > minExcluStatePrio && state.isStateValid())
                    {
                        if (listStateWithAbsolutePiority.ContainsKey(state.priority))
                        {
                            listStateWithAbsolutePiority[state.priority].Add(state);
                        }
                        else
                        {
                            listStateWithAbsolutePiority[state.priority] = new List<StateEntityBase> { state };
                        }
                    }
                }
                else if (state.isStateValid())
                {
                    if (listStateWithAbsolutePiority.ContainsKey(state.priority))
                    {
                        listStateWithAbsolutePiority[state.priority].Add(state);
                    }
                    else
                    {
                        listStateWithAbsolutePiority[state.priority] = new List<StateEntityBase> { state };
                    }
                }
            }
            else if (state is not AttackEntity && state is not ProjectileAttackEntity)
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
        }
        if (listStateForPriority.Keys.Count > 0 || listStateWithAbsolutePiority.Keys.Count > 0)
        {
            System.Random rnd = new System.Random();
            List<StateEntityBase> maxPrioList;
            if (listStateWithAbsolutePiority.Keys.Count == 0)
            {
                maxPrioList = listStateForPriority[listStateForPriority.Keys.Max()];
            }
            else
            {
                maxPrioList = listStateWithAbsolutePiority[listStateWithAbsolutePiority.Keys.Max()];
            }
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
            numberOfTimeGoesIntoState[currentState] += 1;
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
                if (!foundedState.isHostileState) 
                {
                    foundedState.RemoveHostileFromPlayerState();
                }
                currentState.OnEndState();
            }
            currentState = foundedState;
            numberOfTimeGoesIntoState[currentState] += 1;
            currentState.AddHostileToPlayerState();
            currentState.OnEnterState();
            currentState.onActionFinished += StateEnded;
        }
    }
    
    void forceDoNothing()
    {
        doNothingEntity nothing = new doNothingEntity();
        nothing.priority = 0;

        shouldSearchStates = true;
        if (currentState != null)
        {
            currentState.onActionFinished -= StateEnded;
            nothing.RemoveHostileFromPlayerState();
            currentState.OnEndState();
        }
        currentState = nothing;
        currentState.AddHostileToPlayerState();
        currentState.OnEnterState();
        currentState.onActionFinished += StateEnded;
    }

    void StateEnded()
    {
        setNewCurrentState(-1);
    }

    public StateEntityBase getCurrentState()
    {
        return currentState;
    }

    public void StopPrioritizeAttack(float duration)
    {
        prioritizeAttack = false;
        StartCoroutine(StopPrioritizeAttackEnum(duration));
    }

    IEnumerator StopPrioritizeAttackEnum(float duration)
    {
        yield return new WaitForSeconds(duration);
        prioritizeAttack = true;
    }

    public static void StopAllStateMachines()
    {
        alreadyStopedStateMachines.Clear();
        List<StateMachineManager> smm = FindObjectsByType<StateMachineManager>(FindObjectsSortMode.None).ToList();
        foreach (StateMachineManager machine in smm)
        {
            if (machine.enabled == false)
            {
                alreadyStopedStateMachines.Add(machine);
            }
            else
            {
                machine.forceDoNothing();
                machine.enabled = false;
            }
        }
    }

    public static void RestartAllStateMachines()
    {
        List<StateMachineManager> smm = FindObjectsByType<StateMachineManager>(FindObjectsSortMode.None).ToList();
        foreach (StateMachineManager machine in smm)
        {
            if (!alreadyStopedStateMachines.Contains(machine)) machine.enabled = true;
        }
    }

    public int getNbTimeStateWasAchieved(StateEntityBase seb)
    {
        return numberOfTimeGoesIntoState[seb];
    }
}
