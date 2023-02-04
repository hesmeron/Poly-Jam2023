using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class SlicingSystem : MonoBehaviour
{
    private static SlicingSystem _instance;
    public static SlicingSystem  Instance => _instance;
    private readonly List<Sliceable> _sliceables = new List<Sliceable>();

    public List<Sliceable> Sliceables => _sliceables;

    private void OnDrawGizmos()
    {
        
    }

    private void Awake()
    {
        _instance = this;
    }
    
    public void TrySlice(Vector3 start, Vector3 end, out Sliceable closestValid)
    {
        closestValid = null;
        float smallesDistance = 0f;
        bool hasValidTarget = false;
        Vector3 slicePoint = Vector3.zero;
        foreach (var sliceable in _sliceables)       
        {
            if (sliceable.DoSlice(start, end, out float distance, out Vector3 crossPoint))
            {
                if (!hasValidTarget)
                {
                    hasValidTarget = true;
                    closestValid = sliceable;
                    slicePoint = crossPoint;
                }
                else if (smallesDistance > distance)
                {
                    smallesDistance = distance;
                    closestValid = sliceable;
                    slicePoint = crossPoint;
                }
            }
        }

        if (hasValidTarget)
        {
            closestValid.Slice(start, end, slicePoint);
        }
    }


}
