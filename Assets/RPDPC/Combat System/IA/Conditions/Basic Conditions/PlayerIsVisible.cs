using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.RuleTile.TilingRuleOutput;

[CreateAssetMenu(menuName = "Game/IA/Conditions/Base/Player/PlayerIsVisible")]
public class PlayerIsVisible : ConditionBase
{
    GameObject parent;
    GameObject player;
    CharacterController playerCollider;
    Collider parentCollider;
    List<Vector3> directions;
    StateManager sm;
    [SerializeField] bool fulfillIfTalking;
    [SerializeField] public float viewAngle;
    public override void Init(GameObject parent, GameObject player, StateEntityBase seb)
    {
        this.parent = parent;
        this.player = player;
        playerCollider = player.GetComponent<CharacterController>();
        sm = this.player.GetComponent<StateManager>();
        parentCollider = parent.GetComponent<Collider>();
    }

    public override bool isConditionFulfilled()
    {
        if (sm.playerState == StateManager.States.talk && !fulfillIfTalking)
        {
            return false;
        }

        directions = new List<Vector3> {
            playerCollider.bounds.center,
            playerCollider.bounds.center + new Vector3(0, playerCollider.height / 2 - 0.1f, 0),
            playerCollider.bounds.center + new Vector3(0, -playerCollider.height / 2 + 0.1f, 0),
            playerCollider.bounds.center + new Vector3(playerCollider.radius- 0.1f, 0, 0),
            playerCollider.bounds.center + new Vector3(-playerCollider.radius + 0.1f, 0, 0),
            //player.transform.position + new Vector3(-playerCollider.radius/ 2, playerCollider.height / 4, 0),          Points formant un losange, que j'ai jugÈs inutiles, mais je les laisse au cas oÅEen fait Áa l'est
            //player.transform.position + new Vector3(-playerCollider.radius/ 2, -playerCollider.height / 4, 0),
            //player.transform.position + new Vector3(playerCollider.radius / 2, playerCollider.height / 4, 0),
            //player.transform.position + new Vector3(playerCollider.radius/ 2, -playerCollider.height / 4, 0),
        };

        Quaternion quat = Quaternion.FromToRotation(new Vector3(0, 0, 1), new Vector3(player.transform.forward.x,0, player.transform.forward.z));
        foreach (Vector3 d in directions)
        {
            if (CheckRayCast(d)) { return true; }
        }
        return false;
    }

    bool CheckRayCast(Vector3 direction)
    {
        if(parent.TryGetComponent(out NavMeshAgent agent))
        {
            NavMeshPath navMeshPath = new NavMeshPath();
            if(!agent.CalculatePath(player.transform.position, navMeshPath)) return false;
        }

        RaycastHit hit;
        float RayHeight = 0f;
        if (parentCollider != null) 
        {
            RayHeight = parentCollider.bounds.size.y / 2;
        }
        Vector3 origin = parent.transform.position + new Vector3(0, RayHeight, 0);
        if (Physics.Raycast(origin, -(origin - direction), out hit, Mathf.Infinity, -1, QueryTriggerInteraction.Ignore))
        {
            Debug.DrawLine(origin, hit.point);
            if (hit.collider.gameObject == player && Vector3.Angle(parent.transform.forward.normalized, new Vector3(-(origin - direction).x, 0, -(origin - direction).z).normalized) <= viewAngle/2)
            {
                return true;
            }
        }
        return false;
    }
}
