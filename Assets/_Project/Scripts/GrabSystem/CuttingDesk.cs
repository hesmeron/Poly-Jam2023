using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingDesk : GrabDestination
{
    [SerializeField] private Vector3 _slicingPosition;
    [SerializeField] 
    private GrabDestination _destination;
     private GrabTarget _grabTarget;
    private Sliceable _sliceable;

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawCube(transform.TransformPoint(_slicingPosition), new Vector3(1, 0, 1) * 0.1f);
    }

    protected override void OnReach(GrabTarget target)
    {
        base.OnReach(target);
        _grabTarget = target;
        _sliceable = target.GetComponent<Sliceable>();
        _sliceable.enabled = true;
        _sliceable.onSlice += SliceableOnonSlice;
        target.transform.position =transform.TransformPoint(_slicingPosition);
    }

    private void SliceableOnonSlice(GameObject slice)
    {
        Grabable target = slice.gameObject.AddComponent<Grabable>();
        target.Radius = _grabTarget.Radius;
        target.GrabDestination = _destination;
        Ingredient ingredient = slice.gameObject.AddComponent<Ingredient>();
        ingredient.IngredientType = _grabTarget.GetComponent<Ingredient>().IngredientType;
        //_sliceable.onSlice -= SliceableOnonSlice;

    }
}