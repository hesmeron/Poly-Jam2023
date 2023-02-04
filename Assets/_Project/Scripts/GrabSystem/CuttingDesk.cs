using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingDesk : GrabDestination
{
    [SerializeField] private Vector3 _slicingPosition;

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawCube(transform.TransformPoint(_slicingPosition), new Vector3(1, 0, 1) * 0.1f);
    }

    protected override void OnReach(GrabTarget target)
    {
        base.OnReach(target);
        target.GetComponent<Sliceable>().enabled = true;
        target.transform.position =transform.TransformPoint(_slicingPosition);
    }

}