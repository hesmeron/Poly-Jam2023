using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GrabDestination : MonoBehaviour
{
    [SerializeField]
    private float _radius = 2f;

    [SerializeField] 
    private Vector3 _offset;

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1,1, 0.3f);
        Gizmos.DrawSphere(transform.position + _offset, _radius);
    }

    public void TryReach(GrabTarget target)
    {
        if (Vector3.Distance(target.transform.position, transform.position + _offset) <= _radius)
        {
            OnReach(target);
        }
    }

    protected virtual void OnReach(GrabTarget grabTarget)
    {
        grabTarget.enabled = false;
    }
}
