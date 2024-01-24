using System;
using Assets.Scripts;
using UnityEngine;

public class Sliceable : MonoBehaviour
{
    public event Action<GameObject> onSlice;
    [SerializeField]
    private bool _reverseWindTriangles = false;

    [SerializeField]
    private bool _shareVertices = false;

    [SerializeField] 
    private GrabTarget _grabTarget;

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
    private Vector3 _testOrigin;

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
        Gizmos.DrawSphere(_testCross, 0.05f);
        Gizmos.DrawSphere(_testOrigin, 0.05f);
        Gizmos.DrawLine(_testCross, _testOrigin);
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

    public void Slice(Vector3 from, Vector3 to, Vector3 normal)
    {
        /*
        GameObject[] slices = Slicer.Slice(gameObject, from, to, Vector3.up);
        Vector3 newNormal = (normal  * 0.1f);
        slices[0].gameObject.transform.position += newNormal;
        slices[1].gameObject.transform.position -= newNormal;
        onSlice?.Invoke(slices[0]);
        onSlice?.Invoke(slices[1]);
#if UNITY_EDITOR
            DestroyImmediate(gameObject);
        #else
            Rigidbody rigidbody = slices[1].GetComponent<Rigidbody>();
            Destroy(gameObject);
        #endif
*/
    }

    public bool DoSlice(Vector3 start, Vector3 end)
    {
        Vector3 from = _sliceAbleAreaStart + transform.position;
        Vector3 to = _sliceAbleAreaEnd + transform.position;

        Vector3 planeOrigin = (start + end) / 2;
        planeOrigin += Vector3.up;
        planeOrigin = new Vector3(planeOrigin.x, 0, planeOrigin.z);
        _testOrigin = planeOrigin;
        Vector3 direction = end - start;
        Vector2 perpendicular = Vector2.Perpendicular(new Vector2(direction.x, direction.z));
        Vector3 normal = new Vector3(perpendicular.x, 0, perpendicular.y);
        
        if (Trigonometry.PointIntersectsAPlane(from, to, planeOrigin, normal, out Vector3 point))
        {
            _testCross = point;
            float distance = PointToRayDistance(point, start, end);
            bool closeEnough = distance <= _sliceableRadius;
            bool inRange = (point - planeOrigin).magnitude <= direction.magnitude / 2;
         
            if (closeEnough && inRange)
            {
                Slice(from, to, normal);
                return true;
            }
        }
        return false;
    }

    public static float PointToRayDistance(Vector3 point, Vector3 origin, Vector3 target) {
        var ray = new Ray(origin, target - origin);
        var cross = Vector3.Cross(ray.direction, point - ray.origin);
        return cross.magnitude;
    }

}
