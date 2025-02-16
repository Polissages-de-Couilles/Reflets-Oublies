using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(StateMachineManager))]
[RequireComponent(typeof(BotDamageable))]
[RequireComponent(typeof(BotStunAndKnockbackManager))]
[RequireComponent(typeof(BotDeathManager))]
[RequireComponent(typeof(FromSpawnerManager))]
[RequireComponent(typeof(MoneyDrop))]
[RequireComponent(typeof(BotMovementSpeedManager))]
[RequireComponent(typeof(GuardManager))]
[RequireComponent(typeof(Lockable))]
[RequireComponent(typeof(NavMeshModifier))]
public class BotCreator : MonoBehaviour
{
}
