using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestVectors : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private Vector3[] vectors;
    [SerializeField] private GameObject[] points;

    [Header("Player Movement")]
    [SerializeField, Range(3, 15)] float speed = 5f;

    private Rigidbody rb;
    private float dotProduct = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        points = new GameObject[vectors.Length];

        for (int i = 0; i < vectors.Length; i++)
        {
            points[i] = Instantiate(prefab, vectors[i], Quaternion.identity);
            points[i].name = $"Clone of point {i}";
        }

        WhatAreVectors();
    }

    private void Update()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");

        Vector3 input = new Vector3(inputX, 0, inputZ);
        Vector3 movement = input * speed;

        rb.velocity = movement;

        for (int i = 0; i < points.Length; i++)
        {
            dotProduct = Vector3.Dot(transform.forward, Vector3.Normalize(points[i].transform.position - transform.position));
            Debug.DrawLine(transform.position, points[i].transform.position, (dotProduct < 0.20) ? Color.red : Color.green, 0.5f);
        }
    }

    void WhatAreVectors()
    {
        Vector3 vector1 = new Vector3(1, 3, 5);
        Debug.Log($"{vector1}");
        Vector3 vector2 = new Vector3(3, -5, 7);
        Debug.Log($"{vector2}");
        Vector3 v1plusv2 = vector1 + vector2;
        Debug.Log($"{v1plusv2}");
        Vector3 v1minusv2 = vector1 - vector2;
        Debug.Log($"{v1minusv2}");
        Vector3 v2minusv1 = vector2 - vector1;
        Debug.Log($"{v2minusv1}");
        Vector3 v1times4 = vector1 * 4;
        Debug.Log($"{v1times4}");

        Debug.Log($"Vector1 Magnitude: {vector1.magnitude}");

        var forward = transform.forward;
        var back = transform.forward * -1;
        var up = transform.up;
        var down = transform.up * -1;
        var right = transform.right;
        var left = transform.right * -1;

        float distance = Vector3.Distance(transform.position, vector1);
        Debug.Log($"Distance to Vector1: {distance}");

        Vector3 lerpFromVector = Vector3.Lerp(transform.position, vector2, 0.30f);
        Debug.Log($"Vector Lerp: {lerpFromVector}");


        Vector3 thisToVector2 = vector2 - transform.position;
        Debug.Log($"thisToVector2: {thisToVector2} normalized: {Vector3.Normalize(thisToVector2)}");
        float vectorDot = Vector3.Dot(forward, Vector3.Normalize(thisToVector2));
        Debug.Log($"Vector Dot: {vectorDot}");
    }
}
