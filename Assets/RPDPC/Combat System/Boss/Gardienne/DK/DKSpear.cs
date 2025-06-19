using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class DKSpear : MonoBehaviour
{
    [SerializeField] float StoppingDistance;
    GameObject child;
    ProjectileManager pm;
    Vector2 direction;
    float speed;
    float radius;
    float angle;
    DKLine line;
    bool blockUpdate = false;

    public void Init(ProjectileManager pm, Vector2 direction, float speed, float radius, float angle, DKLine line)
    {
        this.pm = pm;
        this.direction = direction;
        this.speed = speed;
        this.radius = radius;
        this.angle = angle;
        this.line = line;

        transform.rotation = Quaternion.LookRotation( -new Vector3(direction.x, 0, direction.y), Vector3.up);

        child = GetComponentInChildren<MeshRenderer>().gameObject;
        AttackCollider ac = GetComponentInChildren<Collider>().gameObject.AddComponent<AttackCollider>();
        ac.Init(false, 0, false, 0, KnockbackMode.MoveAwayFromAttackCollision, true, gameObject);
        ac.OnDamageableEnterTrigger += TriggerEnter;

        child.transform.DOLocalMove(new Vector3(0, 1.5f, 0), 1);
        child.transform.DOLocalRotate(new Vector3(-90, 0, 0), 1);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(direction.x, 0, direction.y) * speed * Time.deltaTime;
        if (!blockUpdate)
        {
            NavMeshHit hit;
            if (!NavMesh.SamplePosition(transform.position, out hit, radius, NavMesh.AllAreas) || Vector3.Distance(transform.position, line.transform.position) > StoppingDistance)
            {
                blockUpdate = true;
                StartCoroutine(endAnim());
            }
        }
    }

    IEnumerator endAnim()
    {
        line.spears.Remove(this);
        line.addAngleToOffLimit(angle);
        child.transform.DOLocalRotate(new Vector3(-135, 0, 0), 1);
        yield return child.transform.DOLocalMove(new Vector3(0, -2, -2), 1).WaitForCompletion();
        Destroy(gameObject);
    }

    void TriggerEnter(IDamageable attacked, GameObject attacker)
    {
        attacked.takeDamage(pm.damageDetail.damage, gameObject);
    }
}
