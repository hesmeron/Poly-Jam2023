using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingPot : GrabDestination
{
    [SerializeField] private Vector3 _start, _end;
    private GrabTarget _target;

    private void OnDrawGizmosSelected()
    {
        //Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawLine(_start + transform.position, _end + transform.position);
    }

    protected override void OnReach(GrabTarget grabTarget)
    {
        base.OnReach(grabTarget);
        _target = grabTarget;
        StartCoroutine(FallDownCoroutine());
    }

    IEnumerator FallDownCoroutine()
    {
        float timePassed = 0f;
        float duration = 1.5f;
        Vector3 start = _start + transform.position;
        Vector3 end = _end + transform.position;
        Vector3 dir = (end-start);
        while (timePassed <= duration)
        {
            _target.transform.position = start + (dir *(timePassed / duration));
            yield return null;
            timePassed += Time.deltaTime;
        }
        Destroy(_target.gameObject);
    }
}
