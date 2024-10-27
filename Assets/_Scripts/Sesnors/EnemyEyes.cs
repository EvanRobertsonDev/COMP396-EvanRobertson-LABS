using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyEyes : Perception
{
    [SerializeField] float distance = 16;
    [SerializeField] float angle = 12;
    [SerializeField] float height = 10;
    [SerializeField] int scanFrequency = 32;
    [SerializeField] LayerMask trackableLayers;
    [SerializeField] LayerMask occlusionLayers;
    [SerializeField] List<GameObject> objectsInSight = new List<GameObject>();

    Collider[] colliders = new Collider[50];
    int count;
    float scanInterval;
    float scanTimer;

    public bool TrackableIsInSight;

    protected override void Initialize()
    {
        scanInterval = 1.0f / scanFrequency;

        TrackableIsInSight = false;
    }

    protected override void UpdatePerception()
    {
        scanTimer -= Time.deltaTime;
        if (scanTimer <= 0)
        {
            scanTimer += scanInterval;
            Scan();
        }
    }

    void Scan()
    {
        count = Physics.OverlapSphereNonAlloc(transform.position, distance, colliders, trackableLayers, QueryTriggerInteraction.Collide);

        objectsInSight.Clear();
        for (int i = 0; i < count; i++)
        {
            var obj = colliders[i].gameObject;
            if (IsInSight(obj))
            {
                objectsInSight.Add(obj);

                TrackableIsInSight = true;
            }
            else
            {
                TrackableIsInSight = false;
            }
        }
    }

    bool IsInSight(GameObject obj)
    {
        Vector3 origin = transform.position;
        Vector3 destination = obj.transform.position;
        Vector3 direction = destination - origin;

        if (direction.y < 0 || direction.y > height) return false;
        direction.y = 0;

        float deltaAngle = Vector3.Angle(direction, transform.forward);
        if (deltaAngle > angle) return false;

        origin.y += height * 0.5f;
        destination.y = origin.y;

        if (Physics.Linecast(origin, destination, occlusionLayers)) return false;

        return true;
    }

    private void OnDrawGizmos()
    {
        if (!visualDebug) return;

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, distance);
        Gizmos.color = Color.red;
        for (int i = 0; i < count; i++) {
            Gizmos.DrawSphere(colliders[i].transform.position, 0.3f);
        }
    }
}
