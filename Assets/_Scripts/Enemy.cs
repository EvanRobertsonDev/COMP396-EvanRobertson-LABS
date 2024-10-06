using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject[] _patrolPoints;

    private StateMachine _stateMachine;

    private void Awake()
    {
        _player = GameObject.FindWithTag("Player");
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        _stateMachine = new StateMachine();
        var patrolState = new PatrolState(_agent, _animator, _patrolPoints);
        var chaseState = new ChaseState(_agent, _player, _animator);

        _stateMachine.AddTransition(patrolState, chaseState, new FuncPredicate(() => 
            Vector3.Distance(_player.transform.position, transform.position) < 2.5f));

        _stateMachine.AddTransition(chaseState, patrolState, new FuncPredicate(() =>
            Vector3.Distance(_player.transform.position, transform.position) > 15f));

        _stateMachine.SetState(patrolState);
    }

    private void Update()
    {
        _stateMachine.Update();
    }

    private void FixedUpdate()
    {
        _stateMachine.FixedUpdate();
    }
}
