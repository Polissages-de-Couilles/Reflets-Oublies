using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

[CreateAssetMenu(menuName = "Game/IA/Conditions/Base/Player/PlayerIsVisible")]
public class PlayerIsVisible : ConditionBase
{
    GameObject parent;
    GameObject player;
    CapsuleCollider playerCollider;
    List<Vector3> directions;
    [SerializeField] float viewAngle;
    public override void Init(GameObject parent, GameObject player)
    {
        this.parent = parent;
        this.player = player;
        playerCollider = player.GetComponent<CapsuleCollider>();
    }

    public override bool isConditionFulfilled()
    {
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
        if (Physics.Raycast(parent.transform.position, -(parent.transform.position - direction), out hit, Mathf.Infinity))
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
