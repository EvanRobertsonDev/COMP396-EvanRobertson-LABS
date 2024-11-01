using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public class Mimic : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject[] mimicObjects;
    [SerializeField] private int attackDamage;

    private StateMachine stateMachine;

    [SerializeField] private int fullHealth;
    [SerializeField] private int currentHealth;

    [SerializeField] private float hidingTime;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        mimicObjects = GameObject.FindGameObjectsWithTag("MimicObj");
    }

    private void Start()
    {
        stateMachine = new StateMachine();
        var scavState = new ScavenginingState(agent, GetClosestMimicSpot(), anim);
        var hidingState = new HidingState(agent, GetClosestMimicSpot(), anim);
        var huntState = new HuntingState(agent, player, anim);
        var attackState = new AttackingState(agent, player, attackDamage, anim);
        var retreatState = new RetreatingState(agent, player, anim);

        //Hide from player when close to copyable object
        stateMachine.AddTransition(scavState, hidingState, new FuncPredicate(() =>
            Vector3.Distance(transform.position, GetClosestMimicSpot().transform.position) < 1));

        //Hunt player if they are close
        stateMachine.AddTransition(scavState, huntState, new FuncPredicate(() =>
            Vector3.Distance(transform.position, player.transform.position) < 3));

        //Attack player if they are close
        stateMachine.AddTransition(huntState, attackState, new FuncPredicate(() =>
            Vector3.Distance(transform.position, player.transform.position) < 2));

        //Hunt player if they too far to attack
        stateMachine.AddTransition(attackState, huntState, new FuncPredicate(() =>
            Vector3.Distance(transform.position, player.transform.position) > 2));

        //Attack player if they are close
        stateMachine.AddTransition(hidingState, attackState, new FuncPredicate(() =>
            Vector3.Distance(transform.position, player.transform.position) < 2));

        //Run away from player if health isn't full
        stateMachine.AddTransition(attackState, retreatState, new FuncPredicate(() =>
            IsFullHealth()));

        //Return to scavenging after getting far away from player
        stateMachine.AddTransition(retreatState, scavState, new FuncPredicate(() =>
            Vector3.Distance(transform.position, player.transform.position) > 10));

        //Look for new object to mimic if uninterupted for 2 mins
        stateMachine.AddTransition(hidingState, scavState, new FuncPredicate(() =>
            hidingTime > 120));

        //Switch back to scavenging if player is far away
        stateMachine.AddTransition(huntState, scavState, new FuncPredicate(() =>
            Vector3.Distance(transform.position, player.transform.position) > 10));

        stateMachine.SetState(scavState);
    }

    //Returns closest mimic object
    GameObject GetClosestMimicSpot()
    {
        GameObject nearest = null;

        foreach (GameObject obj in mimicObjects)
        {
            if (nearest == null) nearest = obj;

            if (Vector3.Distance(agent.transform.position, obj.transform.position) <
                Vector3.Distance(agent.transform.position, nearest.transform.position))
            {
                nearest = obj;
            }
        }

        return nearest;
    }

    private void Update()
    {
        hidingTime += Time.deltaTime;

        if (hidingTime > 120) hidingTime = 0;

        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }

    //Checks if health is full
    bool IsFullHealth()
    {
        return currentHealth < fullHealth;
    }
}
