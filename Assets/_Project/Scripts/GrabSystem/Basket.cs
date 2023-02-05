using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basket : GrabTarget
{
    [SerializeField] 
    private Grabable _grabTarget;

    [SerializeField] 
    private GrabDestination _destination;
    
    public override void OnGrab(GrabHand hand)
    {
        base.OnGrab(hand);
        Grabable grabable = Instantiate(_grabTarget);
        grabable.GrabDestination = _destination;
        hand.ForceSetTarget(grabable);
    }
}
