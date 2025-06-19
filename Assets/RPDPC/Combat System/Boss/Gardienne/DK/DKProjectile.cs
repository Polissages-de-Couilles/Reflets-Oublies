using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class DKProjectile : ProjectileBase
{
    [SerializeField] GameObject spear;
    [SerializeField] float spearSize;
    [SerializeField] float speed;
    [SerializeField] float startRadius;
    [SerializeField] float numberOfLinePerSeconds;
    [SerializeField] float duration;
    [SerializeField] HoleValues holeValues;

    Transform playerTransform;
    StateManager sm;
    Vector3 lastPos;

    protected override void LaunchProjectile()
    {
        //manager.launcher.GetComponent<GardienneParticleHolder>().Switch(GardienneWind.Strong);
        sm = GameManager.Instance.Player.GetComponent<StateManager>();
        playerTransform = GameManager.Instance.Player.transform;
        lastPos = playerTransform.position;
        StartCoroutine(LaunchLines());
    }

    private void LateUpdate()
    {
        if (sm.playerState == StateManager.States.dash)
        {
            manager.launcher.GetComponent<GardienneParticleHolder>().Switch(GardienneWind.ReallyStrong);
            Vector3 currentPos = playerTransform.position;
            if (Vector3.Distance(currentPos, transform.position) < Vector3.Distance(lastPos, transform.position))
            {
                Vector3 difPos = currentPos - lastPos;
                Vector3 forward = (currentPos - transform.position).normalized;
                difPos = Vector3.ProjectOnPlane(difPos, forward);

                GameManager.Instance.Player.GetComponent<CharacterController>().enabled = false;
                playerTransform.position = lastPos + difPos;
                GameManager.Instance.Player.GetComponent<CharacterController>().enabled = true;
            }
        }
        else if (sm.playerState == StateManager.States.idle)
        {
            manager.launcher.GetComponent<GardienneParticleHolder>().Switch(GardienneWind.Strong);
            Vector3 currentPos = playerTransform.position;
            if (Vector3.Distance(currentPos, transform.position) < Vector3.Distance(lastPos, transform.position))
            {
                Vector3 difPos = currentPos - lastPos;
                Vector3 forward = (currentPos - transform.position).normalized;
                Vector3 difPos2 = Vector3.ProjectOnPlane(difPos, forward);

                GameManager.Instance.Player.GetComponent<CharacterController>().enabled = false;
                playerTransform.position = lastPos + (difPos + difPos2) / 3;
                GameManager.Instance.Player.GetComponent<CharacterController>().enabled = true;
            }
        }
        lastPos = playerTransform.position;
    }

    IEnumerator LaunchLines()
    {
        for (int i = 0; i < numberOfLinePerSeconds * duration; i++)
        {
            GameObject go = new GameObject("DKLine");
            go.transform.position = transform.position;
            go.AddComponent<DKLine>().Init(manager, spear, spearSize, speed, startRadius, holeValues);

            yield return new WaitForSeconds(1/numberOfLinePerSeconds);
        }
        manager.launcher.GetComponent<GardienneParticleHolder>().Switch(GardienneWind.Static);
        Destroy(gameObject);
    }
}

[Serializable]
public struct HoleValues
{
    public HoleMode mode;
    public Vector2 randomStartAngle;
    public Vector2 randomAngleRange;
    public int randomNb;
    public List<Vector2> setAngles;
}

public enum HoleMode
{
    Random,
    Set
}
