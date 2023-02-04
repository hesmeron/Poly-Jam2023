using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class CuttingEdge : MonoBehaviour
{
    [SerializeField] 
    private LineRenderer _lineRenderer;

    [SerializeField] 
    private float _length;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Vector3 start = transform.position;
        Vector3 end = start + (transform.forward * _length);
        Gizmos.DrawLine(start, end);
    }

    void Update()
    {
        Vector3 start = transform.position;
        Vector3 end = start + (transform.forward * _length);
        Vector3[] points = {start, end};
        _lineRenderer.SetPositions(points);
        TrySlice();
    }

    [Button]
    private void TrySlice()
    {
        Vector3 start = transform.position;
        Vector3 end = start + (transform.forward * _length);
        SlicingSystem.Instance.TrySlice(start, end);
    }
}
