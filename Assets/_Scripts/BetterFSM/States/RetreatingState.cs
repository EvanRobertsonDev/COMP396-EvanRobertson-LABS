using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RetreatingState : BaseState
{
    protected readonly GameObject player;
    public RetreatingState(NavMeshAgent agent, GameObject player, Animator animator) : base(agent, animator)
    {
        this.player = player;
    }

    public override void OnEnter()
    {
        Debug.Log("Started retreat");
        agent.speed = 4f;
        animator.Play(Run);
    }

    public override void Update()
    {
        //Run away from player
        agent.SetDestination(agent.transform.position-player.transform.position);
    }

    public override void OnExit()
    {
        Debug.Log("Stopped retreat");
    }
}
