using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class BotMovementSpeedManager : MonoBehaviour
{
    [SerializeField] AnimationCurve speedCurve = new AnimationCurve(new Keyframe(0, 1), new Keyframe(1,1));
    [SerializeField] NavMeshAgent agent;
    float timer = 0f;
    float baseSpeed;

    // Start is called before the first frame update
    void Start()
    {
        baseSpeed = agent.speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.hasPath)
        {
            agent.speed = speedCurve.Evaluate(timer) * baseSpeed;
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0f;
        }
    }
}
