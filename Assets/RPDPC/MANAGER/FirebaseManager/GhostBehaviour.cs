using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBehaviour : MonoBehaviour
{
    [SerializeField] Animator animator;

    UserData _oldData;
    UserData _currentData;

    Queue<Command> commands = new Queue<Command>();

    private string currentAnim = "None";

    public void SetUpGhost(UserData newData)
    {
        _oldData = newData;
        _currentData = newData;
        StartCoroutine(Loop());
    }

    public void UpdateGhost(UserData newData)
    {
        _oldData = _currentData;
        _currentData = newData;

        if(_currentData.anim != _oldData.anim)
        {
            if(_currentData.anim != "None")
                animator.SetTrigger(_currentData.anim);
        }

        if (_currentData.position == _oldData.position) return;

        var command = new Command(_oldData, _currentData);
        //Debug.Log(command);

        commands.Enqueue(command);
    }

    private IEnumerator PlayCommand(Command command)
    {
        animator.SetFloat("x", command.Direction.x);
        animator.SetFloat("y", command.Direction.z);
        animator.SetFloat("Speed", 1);
        transform.LookAt(command.currentData.position);
        transform.eulerAngles = new(0, transform.eulerAngles.y, 0);
        //Debug.Log(command.Magnitude);
        yield return transform.DOMove(command.currentData.position, 0.001f).SetEase(Ease.Linear).WaitForCompletion();
    }

    private IEnumerator Loop()
    {
        while (true)
        {
            if(commands.Count > 0)
            {
                var command = commands.Dequeue();
                if(commands.Count > 4)
                {
                    command = commands.Dequeue();
                }
                yield return PlayCommand(command);
            }
            else
            {
                animator.SetFloat("x", 0);
                animator.SetFloat("y", 0);
                animator.SetFloat("Speed", 0);
            }
            yield return null;
        }
    }

    class Command
    {
        public UserData oldData;
        public UserData currentData;

        public Vector3 Direction => (currentData.position - oldData.position).normalized;
        public float Magnitude => (currentData.position - oldData.position).magnitude;

        public Command(UserData old, UserData current)
        {
            oldData = old;
            currentData = current;
        }

        public override string ToString()
        {
            return "End Pos : " + currentData.position + " | Old Anim : " + oldData.anim + " | Current Anim " + currentData.anim;
        }
    }
}
