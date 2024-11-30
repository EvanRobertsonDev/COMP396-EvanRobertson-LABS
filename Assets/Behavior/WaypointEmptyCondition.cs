using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "WaypointEmpty", story: "Is [Waypoints] empty", category: "Conditions", id: "bc97ec3ccc7ef98b80b34b0272468c3d")]
public partial class WaypointEmptyCondition : Condition
{
    [SerializeReference] public BlackboardVariable<List<GameObject>> Waypoints;

    public override bool IsTrue()
    {
        return Waypoints.Value.Count == 0;
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
