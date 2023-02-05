using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbedFromPlace : GrabTarget
{
    [SerializeField]
    private GrabDestination _grabDestination;

    [SerializeField] private Vector3 _startPoisition;

    private void Awake()
    {
        _startPoisition = transform.position;
    }
    
    public override void OnGrabPressed(Transform target)
    {
        transform.position = target.position;
        if (_grabDestination)
        {
            _grabDestination.TryReach(this);
        }
    }

    public override void OnRelease()
    {
        PlaceBack();
    }

    public void PlaceBack()
    {
        transform.position = _startPoisition;
    }
}
