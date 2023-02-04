using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingPot : GrabDestination
{
    [SerializeField] private Vector3 _start, _end;
    protected override void OnReach(GrabTarget grabTarget)
    {
        base.OnReach(grabTarget);
        StartCoroutine(FallDownCoroutine());
    }

    IEnumerator FallDownCoroutine()
    {
        float timePassed = 0f;
        while (timePassed <= 1.5f)
        {
            yield return null;
            timePassed += Time.deltaTime;
        }
    }
}
