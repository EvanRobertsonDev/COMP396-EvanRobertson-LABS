using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestVectors : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private Vector3 v1;
    [SerializeField] private Vector3 v2;

    void Start()
    {
        var point1 = Instantiate(prefab, v1, Quaternion.identity, transform);
        var point2 = Instantiate(prefab, v2, Quaternion.identity, transform);
        var distance = Vector3.Distance(point1.transform.position, point2.transform.position);
        var dotProduct = Vector3.Dot(point1.transform.position, point2.transform.position);
        Debug.Log($"{dotProduct}");
        if (distance < 3 && dotProduct > 0)
        {
            Debug.Log($"Target in range");
        }
    }
}
