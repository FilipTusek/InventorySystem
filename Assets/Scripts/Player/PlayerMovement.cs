using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.Analytics;

public class PlayerMovement : MonoBehaviour {

    public float movementSpeed = 5.0f;
    public float DistanceTraveled;

    private float _moveHorizontal;
    private float _moveVertical;    

    private int _analyticsEventCalls = 0;

    private Animator _playerAC;
    private Rigidbody2D _rigidbody2D;
        
    private Vector3 _movementDirection;
    private Vector3 _lastPosition;

    private DragAndDropManager _dragAndDropManager;

	void Awake ()
    {
        _playerAC = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();

        _moveHorizontal = 0.0f;
        _moveVertical = 0.0f;
	}

    private void Start ( )
    {
        _dragAndDropManager = DragAndDropManager.instance;
        _lastPosition = transform.position;
    }

    void Update ()
    {
        if (!_dragAndDropManager.TouchInputEnabled)
        {
            _moveHorizontal = Input.GetAxisRaw ("Horizontal");
            _moveVertical = Input.GetAxisRaw ("Vertical");
        }
        else
        {
            _moveHorizontal = CrossPlatformInputManager.GetAxis ("Horizontal");
            _moveVertical = CrossPlatformInputManager.GetAxis ("Vertical");
        }

        float distance = Vector3.Distance (_lastPosition, transform.position);
        _lastPosition = transform.position;
        DistanceTraveled += distance;

        if(DistanceTraveled >= 10.0f)
        {
            if (_analyticsEventCalls < 5)
            {
                Analytics.CustomEvent ("Traveled 10 units");
                _analyticsEventCalls++;
            }
            DistanceTraveled = 0.0f;
        }
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
