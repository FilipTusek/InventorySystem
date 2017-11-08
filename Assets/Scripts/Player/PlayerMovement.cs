﻿using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    public float movementSpeed = 5.0f;

    private float _moveHorizontal;
    private float _moveVertical;

    private Animator _playerAC;
    private Rigidbody2D _rigidbody2D;
        
    private Vector3 _movementDirection;

	void Awake ()
    {
        _playerAC = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();

        _moveHorizontal = 0.0f;
        _moveVertical = 0.0f;
	}

	void Update ()
    {
        _moveHorizontal = Input.GetAxisRaw("Horizontal");
        _moveVertical = Input.GetAxisRaw("Vertical");
	}

    void FixedUpdate()
    {
        Move();
        PlayAnimation();
    }

    private void Move()
    {
        _movementDirection = new Vector3(_moveHorizontal, _moveVertical, 0.0f).normalized;
        _rigidbody2D.velocity = _movementDirection * movementSpeed;
    }

    private void PlayAnimation()
    {
        _playerAC.SetFloat("MoveHorizontal", _moveHorizontal);
        _playerAC.SetFloat("MoveVertical", _moveVertical);

        _playerAC.SetFloat ("MovementSpeed", _movementDirection.magnitude);
    }
}