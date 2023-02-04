using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabSystem : MonoBehaviour
{
    private static GrabSystem _instance;
    public static GrabSystem Instance => _instance;

    [SerializeField]
    private List<GrabTarget> _targets = new List<GrabTarget>();

    public List<GrabTarget> Targets => _targets;

    private void Awake()
    {
        _instance = this;
    }

    public bool FindValidTarget(Vector3 position, out GrabTarget grabTarget)
    {
        foreach (var target in _targets)
        {
            if (target.IsInBounds(position))
            {
                grabTarget = target;
                return true;
            }
        }

        grabTarget = null;
        return false;
    }
}
