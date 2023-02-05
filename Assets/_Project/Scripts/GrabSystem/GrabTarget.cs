using UnityEngine;
using UnityEngine.InputSystem.XR;

public abstract class GrabTarget : MonoBehaviour
{
    [SerializeField]
    private float _radius;

    private GrabHand _hand;

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

    private void Start()
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

    public void ForceRelease()
    {
        _hand.ForceRelease();
    }
    public virtual void OnRelease(){}
    public virtual void OnGrabPressed(Transform transform){}

    public virtual void OnGrab(GrabHand hand)
    {
        _hand = hand;
    }
}
