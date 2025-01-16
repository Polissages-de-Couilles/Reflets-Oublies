using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabSocketManager : MonoBehaviour
{
    public int grabID;
    bool isGrabing;
    Transform grabTarget;

    private void Update()
    {
        if (isGrabing)
        {
            grabTarget.position = gameObject.transform.position;
            grabTarget.rotation = gameObject.transform.rotation * Quaternion.Euler(0, 180, 0);
        }
    }

    public void LaunchGrab(GameObject grabTarget, SOAttack.GrabDetails gd, float duration)
    {
        StartCoroutine(Grab(grabTarget, gd, duration));
        isGrabing = true;
    }

    IEnumerator Grab(GameObject grabTarget, SOAttack.GrabDetails gd, float duration)
    {
        isGrabing = true;
        this.grabTarget = grabTarget.transform;
        grabTarget.GetComponent<CharacterController>().enabled = false;

        yield return new WaitForSeconds(duration);


        grabTarget.GetComponent<PlayerStunAndKnockbackManager>().ApplyStun(gd.grabStunDuration);
        grabTarget.GetComponent<CharacterController>().enabled = true;
        isGrabing = false;
        grabTarget.GetComponent<CharacterController>().Move(gd.grabReleaseForce);
    }
}
