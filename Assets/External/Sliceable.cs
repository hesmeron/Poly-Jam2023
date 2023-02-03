﻿using System;
using Assets.Scripts;
using Sirenix.OdinInspector;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class Sliceable : MonoBehaviour
{
    [SerializeField]
    private bool _isSolid = true;

    [SerializeField]
    private bool _reverseWindTriangles = false;

    [SerializeField]
    private bool _useGravity = false;

    [SerializeField]
    private bool _shareVertices = false;

    [SerializeField]
    private bool _smoothVertices = false;
    
    [SerializeField]
    private Vector3 _triggerEnterTipPosition;
    [SerializeField]
    private Vector3 _triggerExitTipPosition;

    [SerializeField] 
    private Vector3 _sliceAbleAreaStart;
    [SerializeField] 
    private Vector3 _sliceAbleAreaEnd;
    [SerializeField] 
    private float _sliceableRadius;
    
    
    public bool ReverseWireTriangles
    {
        get
        {
            return _reverseWindTriangles;
        }
        set
        {
            _reverseWindTriangles = value;
        }
    }
    
    public bool ShareVertices 
    {
        get
        {
            return _shareVertices;
        }
        set
        {
            _shareVertices = value;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(_sliceAbleAreaStart, _sliceAbleAreaEnd);
        Vector3 dir = _sliceAbleAreaEnd - _sliceAbleAreaStart;
        for (int i = 0; i < 10; i++)
        {
            Vector3 pos = _sliceAbleAreaStart + (dir * i / 10);
            Gizmos.DrawLine(pos, pos + (Vector3.up*_sliceableRadius));
            Gizmos.DrawLine(pos, pos + (Vector3.down*_sliceableRadius));
            Gizmos.DrawLine(pos, pos + (Vector3.forward*_sliceableRadius));
            Gizmos.DrawLine(pos, pos + (Vector3.back*_sliceableRadius));
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        Vector3 upperCorner = _triggerEnterTipPosition + Vector3.up;
        Gizmos.DrawLine(upperCorner, _triggerEnterTipPosition);
        Gizmos.DrawLine(upperCorner, _triggerExitTipPosition);
        Gizmos.DrawLine(_triggerExitTipPosition, _triggerEnterTipPosition);
    }

    public bool TrySlice(Vector3 origin, Vector3 end)
    {
        if (IsSliceable(origin, end))
        {
            _triggerEnterTipPosition = origin + transform.position;
            _triggerExitTipPosition = end + transform.position;
            Slice();
            return true;
        }

        return false;
    }

    [Button]
    public void Slice()
    {
        Vector3 upperCorner = _triggerEnterTipPosition + Vector3.up;
        Vector3 side1 = _triggerExitTipPosition - _triggerEnterTipPosition;
        Vector3 side2 = _triggerExitTipPosition - upperCorner;
        Vector3 normal = Vector3.Cross(side1, side2).normalized;
        Vector3 transformedNormal = ((Vector3)(transform.localToWorldMatrix.transpose * normal)).normalized;
        Vector3 transformedStartingPoint = transform.InverseTransformPoint(_triggerEnterTipPosition);
        
        Plane plane = new Plane();
        plane.SetNormalAndPosition(
            transformedNormal,
            transformedStartingPoint);

        var direction = Vector3.Dot(Vector3.up, transformedNormal);

        //Flip the plane so that we always know which side the positive mesh is on
        if (direction < 0)
        {
            plane = plane.flipped;
        }

        GameObject[] slices = Slicer.Slice(plane, gameObject);
        Vector3 newNormal = (transformedNormal  * 0.1f);
        slices[0].gameObject.transform.position -= newNormal;
        slices[1].gameObject.transform.position += newNormal;
        
#if UNITY_EDITOR
            DestroyImmediate(gameObject);
        #else
            Rigidbody rigidbody = slices[1].GetComponent<Rigidbody>();
            Destroy(gameObject);
        #endif

    }
    private bool IsSliceable(Vector3 start, Vector3 end)
    {
        Vector3 from = _sliceAbleAreaStart + transform.position;
        Vector3 to = _sliceAbleAreaEnd +  transform.position;

        Vector3 planeOrigin = (start + end) / 2;
        Vector3 direction = end - start;
        Vector2 perpendicular = Vector2.Perpendicular(new Vector2(direction.x, direction.z));
        Vector3 normal = new Vector3(perpendicular.x, 1, perpendicular.y);
        if(PointIntersectsAPlane(from, to, planeOrigin, normal, out Vector3 point))
        {
            bool closeEnough = PointToRayDistance(point, start, end) <= _sliceableRadius;
            bool inRange = (point - planeOrigin).magnitude <= direction.magnitude / 2;
            return closeEnough && inRange;
        }
        return false;
    }

    public bool PointIntersectsAPlane(Vector3 from, Vector3 to, Vector3 planeOrigin, Vector3 normal, out Vector3 result)
    {
        Vector3 translation = to - from;
        float dot = Vector3.Dot(normal, translation);
        if (Mathf.Abs(dot) > Single.Epsilon)
        {
            Vector3 distance1 = from - planeOrigin;
            float fac = -Vector3.Dot(normal, distance1) / dot;
            translation = translation * fac;
            result = from + translation;
            return true;
        }
        
        result = Vector3.zero;
        return false;
    }
    
    public static float PointToRayDistance(Vector3 point, Vector3 origin, Vector3 target) {
        var ray = new Ray(origin, target - origin);
        var cross = Vector3.Cross(ray.direction, point - ray.origin);
        Debug.Log("Distance "+ cross.magnitude);
        return cross.magnitude;
    }

}
