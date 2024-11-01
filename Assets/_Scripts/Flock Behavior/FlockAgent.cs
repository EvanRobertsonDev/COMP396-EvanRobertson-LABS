using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockAgent : MonoBehaviour
{
    public float Speed;
    List<Transform> group = new List<Transform>();
    bool isTurning = false;

    private void Start()
    {
        Speed = Random.Range(FlockManager.Instance.MinSpeed, FlockManager.Instance.MaxSpeed);
    }

    private void Update()
    {
        isTurning = !FlockManager.Instance.SwimLimits.Contains(transform.position);
        if (isTurning)
        {
            Vector3 newDirection = FlockManager.Instance.transform.position - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(newDirection),
                FlockManager.Instance.RotationSpeed * Time.deltaTime);
        }
        else
        {
            if (Random.Range(0, 100) < 40)
            {
                Speed = Random.Range(FlockManager.Instance.MinSpeed, FlockManager.Instance.MaxSpeed);
            }
            ApplyRules();
        }
        transform.Translate(0, 0, Speed * Time.deltaTime);
    }

    void ApplyRules()
    {
        Vector3 groupCenter = Vector3.zero;
        Vector3 groupAvoid = Vector3.zero;
        float groupSpeed = 0.01f;
        float neighborDistance = 0;
        int groupSize = 0;
        group.Clear();

        foreach (GameObject fish in FlockManager.Instance.School)
        {
            if (fish == this.gameObject) continue;
            neighborDistance = Vector3.Distance(fish.transform.position, transform.position);
            if (neighborDistance >= FlockManager.Instance.NeighborDistance) continue;

            groupCenter += fish.transform.position;
            groupSize++;

            group.Add(fish.transform);

            if (neighborDistance < FlockManager.Instance.AvoidDistance)
            {
                groupAvoid += transform.position - fish.transform.position;
            }

            FlockAgent fishAgent = fish.GetComponent<FlockAgent>();
            groupSpeed += fishAgent.Speed;
        }

        if (groupSize > 0)
        {
            groupCenter = groupCenter / groupSize;
            Speed = groupSpeed / groupSize;
            if (Speed > FlockManager.Instance.MaxSpeed)
            {
                Speed = FlockManager.Instance.MaxSpeed;
            }

            Vector3 newDirection = (groupCenter + groupAvoid) - transform.position;
            if (newDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(newDirection),
                    FlockManager.Instance.RotationSpeed * Time.deltaTime);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (Transform friend in group)
        {
            Gizmos.DrawLine(transform.position, friend.transform.position);
        }
    }
}
