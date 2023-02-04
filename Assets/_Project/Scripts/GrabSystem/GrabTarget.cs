using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabTarget : MonoBehaviour
{
    [SerializeField]
    private float _radius;

    [SerializeField] private GrabDestination _grabDestination;

    public GrabDestination GrabDestination
    {
        get => _grabDestination;
        set => _grabDestination = value;
    }

    public float Radius
    {
        get => _radius;
        set => _radius = value;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, _radius);
    }

    private void OnEnable()
    {
        GrabSystem.Instance.Targets.Add(this);
    }
    private void OnDisable()
    {
        GrabSystem.Instance.Targets.Remove(this);
    }

    public bool IsInBounds(Vector3 position)
    {
        return Vector3.Distance(position, transform.position) <= _radius;
    }

    public void ReachedDestination()
    {
        _grabDestination.TryReach(this);
    }
}
