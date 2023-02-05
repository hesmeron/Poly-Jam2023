using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrabHand : MonoBehaviour
{
    [SerializeField]
    public InputAction _grabAction;

    [SerializeField] 
    private GameObject _showcase;

    //[SerializeField]
    //public InputAction _releaseAction;
    
    private GrabTarget _target;

    private bool _isHolding = false;
    // Start is called before the first frame update
    void Awake()
    {
        _grabAction.performed += GrabActionOnPerformed;
        _grabAction.canceled += ReleaseActionOnperformed;
        _grabAction.Enable();
    }

    public void ForceSetTarget(GrabTarget target)
    {
        _target = target;
        _isHolding = true;
    }
    
    public void ForceRelease()
    {
        _target = null;
        _isHolding = false;
    }

    [Button]
    private void ReleaseActionOnperformed(InputAction.CallbackContext obj)
    {
        if (_isHolding)
        {
            _isHolding = false;
            _showcase.SetActive(true);
            _target.OnRelease();
        }

    }

    
    private void Update()
    {
        if (_isHolding)
        {
            _target.OnGrabPressed(transform);
        }
    }

    [Button]
    private void GrabActionOnPerformed(InputAction.CallbackContext obj)
    {
        if (!_isHolding)
        {
            _showcase.SetActive(false);
            if (GrabSystem.Instance.FindValidTarget(transform.position, out GrabTarget target))
            {
                _target = target;
                _target.OnGrab(this);
                _isHolding = true;
            }
        }
    }
}
