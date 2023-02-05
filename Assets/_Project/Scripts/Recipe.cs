using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recipe : MonoBehaviour
{
    [SerializeField]
    int[] _requirements = new int[0];

    [SerializeField] 
    private GrabbedFromPlace _grabbedFromPlace;

    [SerializeField]
    private GameObject _destroyedVersion;

    private Material _material;

    private void Start()
    {
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        _material = new Material(renderer.material);
        renderer.material = _material;
    }


    public bool FitsRequirements(int[] ingredients)
    {
        for(int i=0; i < _requirements.Length; i++)
        {
            if (_requirements[i] != ingredients[i])
            {
                Instantiate(_destroyedVersion).transform.position = transform.position;
                _grabbedFromPlace.ForceRelease();
                _grabbedFromPlace.PlaceBack();
                return false;
            }
        }
        
        _material.SetColor("_Tint", Color.green);
        _grabbedFromPlace.ForceRelease();
        _grabbedFromPlace.PlaceBack();
        _grabbedFromPlace.enabled = false;
        return true;
    }
}
