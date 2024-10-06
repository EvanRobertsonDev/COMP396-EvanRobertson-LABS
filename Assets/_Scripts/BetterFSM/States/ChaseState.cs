using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseState : BaseState
{
    protected readonly GameObject player;

    public ChaseState(NavMeshAgent agent, GameObject player, Animator animator) 
        : base(agent, animator)
    {
        this.player = player;
    }

    public override void OnEnter()
    {
        Debug.Log("Chasing");
        agent.speed = 4f;
        animator.Play(Run);
    }

    public override void Update()
    {
        agent.SetDestination(player.transform.position);
    }

    public override void OnExit()
    {
        Debug.Log("Stopping chase");
    }
}
