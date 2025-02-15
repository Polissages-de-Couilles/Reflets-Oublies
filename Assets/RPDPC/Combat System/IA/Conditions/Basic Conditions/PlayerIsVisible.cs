using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

[CreateAssetMenu(menuName = "Game/IA/Conditions/Base/Player/PlayerIsVisible")]
public class PlayerIsVisible : ConditionBase
{
    GameObject parent;
    GameObject player;
    CharacterController playerCollider;
    CapsuleCollider parentCollider;
    List<Vector3> directions;
    StateManager sm;
    [SerializeField] bool fulfillIfTalking;
    [SerializeField] float viewAngle;
    public override void Init(GameObject parent, GameObject player)
    {
        this.parent = parent;
        this.player = player;
        playerCollider = player.GetComponent<CharacterController>();
        sm = this.player.GetComponent<StateManager>();
        parentCollider = parent.GetComponent<CapsuleCollider>();
    }

    public override bool isConditionFulfilled()
    {
        if (sm.playerState == StateManager.States.talk && !fulfillIfTalking)
        {
            return false;
        }

        directions = new List<Vector3> {
            player.transform.position,
            player.transform.position + new Vector3(0, playerCollider.height / 2, 0),
            player.transform.position + new Vector3(0, -playerCollider.height / 2, 0),
            player.transform.position + new Vector3(playerCollider.radius, 0, 0),
            player.transform.position + new Vector3(-playerCollider.radius, 0, 0),
            //player.transform.position + new Vector3(-playerCollider.radius/ 2, playerCollider.height / 4, 0),          Points formant un losange, que j'ai jugés inutiles, mais je les laisse au cas où en fait ça l'est
            //player.transform.position + new Vector3(-playerCollider.radius/ 2, -playerCollider.height / 4, 0),
            //player.transform.position + new Vector3(playerCollider.radius / 2, playerCollider.height / 4, 0),
            //player.transform.position + new Vector3(playerCollider.radius/ 2, -playerCollider.height / 4, 0),
        };

        Quaternion quat = Quaternion.FromToRotation(new Vector3(0, 0, 1), new Vector3(player.transform.forward.x,0, player.transform.forward.z));
        foreach (Vector3 d in directions)
        {
            if (CheckRayCast( d)) { return true; }
        }
        return false;
    }

    bool CheckRayCast(Vector3 direction)
    {
        RaycastHit hit;
        float RayHeight = 0f;
        if (parentCollider != null) 
        {
            RayHeight = parentCollider.height / 2;
        }
        if (Physics.Raycast(parent.transform.position + new Vector3(0, RayHeight, 0), -(parent.transform.position - direction), out hit, Mathf.Infinity))
        {
            Debug.DrawLine(parent.transform.position, hit.point);
            //Debug.Log(Vector3.Angle(parent.transform.forward.normalized, -(parent.transform.position - direction).normalized));
            if (hit.collider.gameObject == player && Vector3.Angle(parent.transform.forward.normalized, -(parent.transform.position - direction).normalized) <= viewAngle/2)
            {
                return true;
            }
        }
        return false;
    }
}
