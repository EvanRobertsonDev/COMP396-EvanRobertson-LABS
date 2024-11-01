using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ScavenginingState : BaseState
{
    protected readonly GameObject obj;

    public ScavenginingState(NavMeshAgent agent, GameObject obj, Animator animator) : base(agent, animator)
    {
        this.obj = obj;
    }

    public override void OnEnter()
    {
        Debug.Log("Started scavenging");
        agent.speed = 1.3f;
        animator.Play(Walk);
    }

    public override void Update()
    {
        //Move towards mimic object
        agent.SetDestination(obj.transform.position);
    }

    public override void OnExit()
    {
        Debug.Log("Stopped scavenging");
    }

    
}
