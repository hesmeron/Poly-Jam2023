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
    
    public void TrySlice(Vector3 start, Vector3 end)
    {
        List<Sliceable> toremove = new List<Sliceable>();
        foreach (var sliceable in _sliceables)
        {
            if (sliceable.DoSlice(start, end))
            {
                toremove.Add(sliceable);
            }
        }

        foreach (var sliceable in toremove)
        {
            _sliceables.Remove(sliceable);
        }
    }
}
