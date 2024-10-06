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
    Looking,
    Chase,
    Attack,
    FleeToHQ
}

[RequireComponent(typeof(NavMeshAgent))]
public class SimpleFSM : MonoBehaviour
{
    [SerializeField] private EnemySimpleFSMStates _currentState;
    [SerializeField] private Animator _animator;
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

    [SerializeField] private float _walkSpeed, _runSpeed;

    [SerializeField] private int _index = 0;

    void Start()
    {
        _animator = GetComponent<Animator>();
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
            case EnemySimpleFSMStates.Looking:
                Looking();
                break;
            default: 
                Debug.LogError("State in FSM not implemented");
                break;
        }
    }

    private void Patrol()
    {
        _animator.SetBool("IsWalking", true);
        Debug.Log("Patrolling...");
        Vector3 playerPosInRelationToGuard = _player.transform.position - transform.position;
        float distanceToPlayer = playerPosInRelationToGuard.magnitude;
        Vector3 directionToPlayer = playerPosInRelationToGuard / distanceToPlayer;

        _agent.SetDestination(_patrolPoints[_index].position);
        _agent.speed = _walkSpeed;

        Vector3 destination = _patrolPoints[_index].position;
        if (Vector3.Distance(transform.position, destination) < 2f)
        {
            TransitionStates(EnemySimpleFSMStates.Looking);
        }

        _isInFront = Vector3.Dot(transform.forward, directionToPlayer) > _fieldOfView;
        if (_isInFront && distanceToPlayer < _distanceToChase)
        {
            _animator.SetBool("IsRunning", true);
            TransitionStates(EnemySimpleFSMStates.Chase);
        }
    }

    private void Looking()
    {
        if(!_animator.GetBool("IsLooking"))
        {
            StartCoroutine("LookingAround");
        }
        _animator.SetBool("IsLooking", true);
    }

    IEnumerator LookingAround()
    {
        yield return new WaitForSeconds(2f);
        _index = (_index + 1) % _patrolPoints.Count;
        _animator.SetBool("IsLooking", false);
        TransitionStates(EnemySimpleFSMStates.Patrol);
    }

    private void Chase()
    {
        
        _agent.SetDestination(_player.transform.position);
        _agent.speed = _runSpeed;

        if (Vector3.Distance(transform.position, _player.transform.position) > _distanceToChase)
        {
            _animator.SetBool("IsRunning", false);
            TransitionStates(EnemySimpleFSMStates.Patrol);
        }
        if (Vector3.Distance(transform.position, _player.transform.position) < _distanceToAttack)
        {
            _animator.SetBool("IsAttacking", true);
            TransitionStates(EnemySimpleFSMStates.Attack);
        }
        Debug.Log("CHAAAASEEEE....");
    }
    private void Attack() 
    {
        if (Vector3.Distance(transform.position, _player.transform.position) > _distanceToAttack)
        {
            _animator.SetBool("IsAttacking", false);
            TransitionStates(EnemySimpleFSMStates.Chase);
        }
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
