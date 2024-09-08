using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Brick))]
public class BrickMove : MonoBehaviour
{
    private bool _active = true, _forward = true;
    private Transform _transform;

    [SerializeField]
    private Vector2 _dir = Vector2.right;
    private Vector2 _initPos;

    private float _pos;

    private void Awake()
    {
        _transform = GetComponent<Transform>();
        _initPos = _transform.position;
        GetComponent<Brick>().OnBreak.AddListener((_) => _active = false);
    }

    private void Update()
    {
        _pos += _forward ? Time.deltaTime : -Time.deltaTime;
        if (_forward)
        {
            if (_pos > 1f)
            {
                _forward = false;
                _pos = 1f;
            }
        }
        else if (_pos < 0f)
        {
            _forward = true;
            _pos = 0f;
        }

        _transform.position = _initPos + _dir * _pos;
    }
}
