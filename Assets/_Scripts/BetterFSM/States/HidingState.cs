using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HidingState : BaseState
{
    protected readonly GameObject obj;

    private Mesh oldMesh;

    public HidingState(NavMeshAgent agent, GameObject obj, Animator animator) : base(agent, animator)
    {
        this.obj = obj;
    }

    
    public override void OnEnter()
    {
        Debug.Log("Hiding - Found suitable object");
        //Assign mesh of object overtop of Mimic
        agent.gameObject.GetComponent<MeshFilter>().mesh = obj.GetComponent<MeshFilter>().mesh;
        agent.gameObject.GetComponent<MeshRenderer>().material = obj.GetComponent<MeshRenderer>().material;
        //Hide original object
        obj.SetActive(false);
    }

    public override void OnExit()
    {
        //Reset mesh
        agent.gameObject.GetComponent<MeshFilter>().mesh = null;

        //Make original object visible again
        obj.SetActive(true);

        Debug.Log("Stopped hiding");
    }

}
