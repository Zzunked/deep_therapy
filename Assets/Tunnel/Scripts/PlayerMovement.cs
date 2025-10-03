using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _speed = 5;
    [SerializeField] private Transform _transform;
    private bool _isFacingRight = true;
    private float _xInput;

    // Update is called once per frame
    private void Update()
    {
        _xInput = Input.GetAxis("Horizontal");
    }

    private void FixedUpdate()
    {
        _rb.linearVelocity = new Vector2(_xInput * _speed, 0);

        ChangeAnimation();
        Turn();
    }

    private void Turn()
    {
        if (_isFacingRight && _xInput < 0)
        {
            Vector3 rotator = new Vector3(_transform.rotation.x, 180f, _transform.rotation.z);
            _transform.rotation = Quaternion.Euler(rotator);
            _isFacingRight = false;
        }
        else if (!_isFacingRight && _xInput > 0f)
        {
            Vector3 rotator = new Vector3(_transform.rotation.x, 0f, _transform.rotation.z);
            _transform.rotation = Quaternion.Euler(rotator);
            _isFacingRight = true;
        }
    }

    private void ChangeAnimation()
    {
        if (Math.Abs(_xInput) > 0)
        {
            _animator.SetBool("isWalking", true);
        }
        else
        {
            _animator.SetBool("isWalking", false);
        }
    }
}
