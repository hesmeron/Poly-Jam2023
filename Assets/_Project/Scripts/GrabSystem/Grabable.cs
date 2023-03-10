using UnityEngine;

public class Grabable : GrabTarget
{
    [SerializeField] 
    private GrabDestination _grabDestination;
    public GrabDestination GrabDestination
    {
        get => _grabDestination;
        set => _grabDestination = value;
    }
    public override void OnRelease()
    {
        if (!_grabDestination.TryReach(this))
        {
            Destroy(this.gameObject);
        }
    }

    public override void OnGrabPressed(Transform handTransform)
    {
        transform.position = handTransform.position + (handTransform.forward * 0.1f);
    }
}
