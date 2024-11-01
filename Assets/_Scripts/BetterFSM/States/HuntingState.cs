using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HuntingState : BaseState
{
    protected readonly GameObject player;

    public HuntingState(NavMeshAgent agent, GameObject player, Animator animator) : base(agent, animator)
    {
        this.player = player;
    }

    public override void OnEnter()
    {
        Debug.Log("Started hunting");
        agent.speed = 4f;
        animator.Play(Run);
    }

    public override void Update()
    {
        //Move towards player
        agent.SetDestination(player.transform.position);
    }

    public override void OnExit()
    {
        Debug.Log("Stopped hunting");
    }
}
