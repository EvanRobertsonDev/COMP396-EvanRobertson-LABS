using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class BaseState : IState
{
    protected readonly Animator animator;
    protected readonly NavMeshAgent agent;

    protected static readonly int Walk = Animator.StringToHash("Walking");
    protected static readonly int Run = Animator.StringToHash("Running");

    protected BaseState(NavMeshAgent agent, Animator animator)
    {
        this.agent = agent;
        this.animator = animator;
    }

    public virtual void FixedUpdate() { }

    public virtual void OnEnter() { }

    public virtual void OnExit() { }

    public virtual void Update() { }
}
