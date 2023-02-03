using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadBack : MonoBehaviour
{
    [SerializeField] private LevelLoader _loader;

    [SerializeField] private float _delay = 7f;

    void Update()
    {
        _delay -= Time.deltaTime;
        if (_delay <= 0)
        {
            _loader.LoadPreviousLevel();
        }
    }
}
