using System;
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
    private Vector3 _testFrom;
    [SerializeField]
    private Vector3 _testTo;

    [SerializeField] 
    private Vector3 _sliceAbleAreaStart;
    [SerializeField] 
    private Vector3 _sliceAbleAreaEnd;
    [SerializeField] 
    private float _sliceableRadius;

    private Vector3 _testCross;

    public Vector3 SliceAbleAreaStart => transform.TransformPoint(_sliceAbleAreaStart);

    public Vector3 SliceAbleAreaEnd => transform.TransformPoint(_sliceAbleAreaEnd);


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
        Gizmos.color = Color.blue;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawLine(_sliceAbleAreaStart, _sliceAbleAreaEnd);
        Vector3 dir = _sliceAbleAreaEnd - _sliceAbleAreaStart;
        /*
        for (int i = 0; i < 10; i++)
        {
            Vector3 pos = _sliceAbleAreaStart + (dir * i / 10);
            Gizmos.DrawLine(pos, pos + (Vector3.up*_sliceableRadius));
            Gizmos.DrawLine(pos, pos + (Vector3.down*_sliceableRadius));
            Gizmos.DrawLine(pos, pos + (Vector3.forward*_sliceableRadius));
            Gizmos.DrawLine(pos, pos + (Vector3.back*_sliceableRadius));
        }*/
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        Vector3 upperCorner = _testFrom + Vector3.up;
        Gizmos.DrawLine(upperCorner, _testFrom);
        Gizmos.DrawLine(upperCorner, _testTo);
        Gizmos.DrawLine(_testTo, _testFrom);
    }

    private void Start()
    {
        SlicingSystem.Instance.Sliceables.Add(this);
    }
    
    public void Initialize(Vector3 start, Vector3 end, float radius = 0.5f)
    {
        _sliceAbleAreaStart = transform.InverseTransformPoint(start);
        _sliceAbleAreaEnd = transform.InverseTransformPoint(end);
        _sliceableRadius = radius;
    }

    public void Slice(Plane plane, Vector3 crossPoint, Vector3 origin, Vector3 normal)
    {
        GameObject[] slices = Slicer.Slice(plane, gameObject, crossPoint, origin, normal);
        //slices[0].gameObject.transform.position += newNormal;
        //slices[1].gameObject.transform.position -= newNormal;
#if UNITY_EDITOR
            DestroyImmediate(gameObject);
        #else
            Rigidbody rigidbody = slices[1].GetComponent<Rigidbody>();
            Destroy(gameObject);
        #endif

    }

    public bool DoSlice(Vector3 start, Vector3 end)
    {
        Vector3 from = _sliceAbleAreaStart + transform.position;
        Vector3 to = _sliceAbleAreaEnd + transform.position;
        /*
        Vector3 upperCorner = start + Vector3.up;
        Vector3 side1 = start - end;
        Vector3 side2 = ((start + end) /3)- upperCorner;
        Vector3 normal = Vector3.Cross(side1, side2).normalized;
        Vector3 transformedNormal = ((Vector3)(transform.localToWorldMatrix.transpose * normal)).normalized;
        Vector3 transformedStartingPoint = transform.InverseTransformPoint(_triggerEnterTipPosition);
        */
        Vector3 planeOrigin = (start + end) / 2;
        //planeOrigin += Vector3.up;
        planeOrigin = new Vector3(planeOrigin.x, 0, planeOrigin.z);
        Vector3 direction = end - start;
        Vector2 perpendicular = Vector2.Perpendicular(new Vector2(direction.x, direction.z));
        Vector3 normal = new Vector3(perpendicular.x, 0, perpendicular.y);

        if (PointIntersectsAPlane(from, to, planeOrigin, normal, out Vector3 point))
        {
            Plane plane = new Plane();
            plane.SetNormalAndPosition(normal, planeOrigin);
            float distance = PointToRayDistance(point, start, end);
            bool closeEnough = distance <= _sliceableRadius;
            bool inRange = (point - planeOrigin).magnitude <= direction.magnitude / 2;
            if (closeEnough && inRange)
            {
                Slice(plane, point, transform.InverseTransformPoint(planeOrigin), transform.InverseTransformDirection(normal));
                return true;
            }
        }

        return false;
    }

    public static bool PointIntersectsAPlane(Vector3 from, Vector3 to, Vector3 planeOrigin, Vector3 normal, out Vector3 result)
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
