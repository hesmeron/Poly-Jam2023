using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingEdge : MonoBehaviour
{
    [SerializeField] 
    private LineRenderer _lineRenderer;

    [SerializeField] 
    private float _length;

    [SerializeField]
    private Sliceable _sliceable;

    void Update()
    {
        Vector3 start = transform.position;
        Vector3 end = start + (transform.forward * _length);
        Vector3[] points = {start, end};
        _lineRenderer.SetPositions(points);
        _sliceable.TrySlice(start, end);
    }
}
