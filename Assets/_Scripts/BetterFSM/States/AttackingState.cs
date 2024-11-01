using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackingState : BaseState
{
    protected readonly GameObject player;
    int attackDamage;

    public AttackingState(NavMeshAgent agent, GameObject player, int attackDamage, Animator animator) : base(agent, animator)
    {
        this.player = player;
        this.attackDamage = attackDamage;
    }

    public override void OnEnter()
    {
        Debug.Log("Entering attack mode");
    }

    public override void Update()
    {
        animator.Play(Attack);
    }

    public override void OnExit()
    {
        Debug.Log("Stopped Attacking");
    }
}
