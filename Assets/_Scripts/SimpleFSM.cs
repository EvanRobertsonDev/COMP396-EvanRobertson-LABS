using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemySimpleFSMStates
{
    Patrol,
    Chase,
    Attack
}

public class SimpleFSM : MonoBehaviour
{

    [SerializeField] EnemySimpleFSMStates currentState;
    [SerializeField] GameObject player;
    [SerializeField, Range(4f, 15f)] float distanceToChase = 10f;
    [SerializeField, Range(0.12f, 0.8f)] float fieldOfView = 0.28f;
    [SerializeField, Range(2f, 7f)] float distanceToAttack = 3f;
    [SerializeField] bool isInFront;

    void Start()
    {
        currentState = EnemySimpleFSMStates.Patrol;
        player = GameObject.FindWithTag("Player");

    }

    // Update is called once per frame
    void Update()
    {
        FiniteStateMachineRunner();
    }

    private void FiniteStateMachineRunner()
    {
        switch(currentState)
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
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    void Patrol()
    {
        Debug.Log("Patrolling");
        Vector3 relativePlayerPos = player.transform.position - transform.position;
        float distanceToPlayer = relativePlayerPos.magnitude;
        Vector3 directionToPlayer = relativePlayerPos / distanceToPlayer;

        isInFront = Vector3.Dot(transform.forward, directionToPlayer) > fieldOfView;

        if (isInFront && distanceToPlayer < distanceToChase)
        {
            currentState = EnemySimpleFSMStates.Chase;
        }
    }

    void Chase()
    {
        Debug.Log("Chasing");
    }

    void Attack()
    {

    }
}
