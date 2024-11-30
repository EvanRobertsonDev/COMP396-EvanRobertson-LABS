using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using System.Linq;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SetWaypoints", story: "Set [waypoints] from [patrollingLocations]", category: "Action", id: "977aa4c56031916f7da16f73280a5958")]
public partial class SetWaypointsAction : Action
{
    [SerializeReference] public BlackboardVariable<List<GameObject>> Waypoints;
    [SerializeReference] public BlackboardVariable<GameObject> PatrollingLocations;

    protected override Status OnStart()
    {
        for (int i = 0; i < PatrollingLocations.Value.transform.childCount; i++)
        {
            Waypoints.Value.Add(PatrollingLocations.Value.transform.GetChild(i).gameObject);
        }
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

