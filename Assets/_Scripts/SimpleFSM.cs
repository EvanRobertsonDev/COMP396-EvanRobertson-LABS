using KBCore.Refs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public enum EnemySimpleFSMStates
{
    Patrol,
    Chase,
    Attack,
    FleeToHQ
}

public class SimpleFSM : MonoBehaviour
{
    [SerializeField] private EnemySimpleFSMStates _currentState;
    [SerializeField] private GameObject _player;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private GameObject _patrollingLocations;
    [SerializeField] private List<Transform> _patrolPoints = new List<Transform>();
    [SerializeField] private GameObject _HQ;
    
    [Header("Guard Stats")]
    [SerializeField, Range(8f, 15f)] 
    private float _distanceToChase = 10f;

    [SerializeField, Range(0.12f, 0.80f)]
    private float _fieldOfView = 0.28f;

    [SerializeField, Range(2f, 7f)]
    private float _distanceToAttack = 3f;

    [SerializeField] private bool _isInFront;

    private int _index = 0;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _currentState = EnemySimpleFSMStates.Patrol;
        _player = GameObject.FindWithTag("Player");
        _patrolPoints = _patrollingLocations.GetComponentsInChildren<Transform>().ToList();
    }

    void Update()
    {
        FiniteStateMachineRunner();
    }
    private void FiniteStateMachineRunner()
    {
        switch (_currentState)
        {
            case EnemySimpleFSMStates.Patrol:
                Patrol();
                break;
            case EnemySimpleFSMStates.Chase:
                Chase();
                break;
            case EnemySimpleFSMStates.Attack:
                Attack();
                break;
            case EnemySimpleFSMStates.FleeToHQ:
                Flee();
                break;
            default: 
                Debug.LogError("State in FSM not implemented");
                break;
        }
    }

    private void Patrol()
    {
        Debug.Log("Patrolling...");
        Vector3 playerPosInRelationToGuard = _player.transform.position - transform.position;
        float distanceToPlayer = playerPosInRelationToGuard.magnitude;
        Vector3 directionToPlayer = playerPosInRelationToGuard / distanceToPlayer;

        _agent.SetDestination(_patrolPoints[_index].position);

        Vector3 destination = _patrolPoints[_index].position;
        if (Vector3.Distance(transform.position, destination) < 2f)
        {
            _index = (_index + 1) % _patrolPoints.Count;
        }

        _isInFront = Vector3.Dot(transform.forward, directionToPlayer) > _fieldOfView;
        if (_isInFront && distanceToPlayer < _distanceToChase)
        {
            TransitionStates(EnemySimpleFSMStates.Chase);
        }
        if (_isInFront && distanceToPlayer < _distanceToAttack)
        {
            TransitionStates(EnemySimpleFSMStates.Attack);
        }
    }
    private void Chase()
    {
        _agent.SetDestination(_player.transform.position);

        if (Vector3.Distance(transform.position, _player.transform.position) > _distanceToChase)
        {
            TransitionStates(EnemySimpleFSMStates.Patrol);
        }
        if (Vector3.Distance(transform.position, _player.transform.position) < _distanceToAttack)
        {
            TransitionStates(EnemySimpleFSMStates.Attack);
        }
        Debug.Log("CHAAAASEEEE....");
    }
    private void Attack() 
    {
        Debug.Log("Attack....");
    }

    private void Flee()
    {
        _agent.SetDestination(_HQ.transform.position);
    }

    private void TransitionStates(EnemySimpleFSMStates state)
    {
        _currentState = state;
    }
}
