using UnityEngine;

/// <summary>
/// Attach this script to a point item or obstacle to make it ping-pong either vertically or horizontally.
/// </summary>
public class ObjectMover : MonoBehaviour
{
    private enum MovementDirection
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }

    [SerializeField]
    private float _movementSpeed = 5.0f;
    [SerializeField]
    private float _movementDistance = 2.0f;
    [SerializeField]
    private MovementDirection _startingMovementDirection = MovementDirection.UP;

    private Vector3 _startPosition;
    private MovementDirection _currentMovementDirection;

    private void Start()
    {
        _startPosition = transform.position;
        _currentMovementDirection = _startingMovementDirection;
    }

    private void Update()
    {
        if(_currentMovementDirection == MovementDirection.UP)
            MoveObjectUp();
        else if(_currentMovementDirection == MovementDirection.DOWN)
            MoveObjectDown();
        else if(_currentMovementDirection == MovementDirection.LEFT)
            MoveObjectLeft();
        else
            MoveObjectRight();
    }

    private void MoveObjectUp()
    {
        transform.position += new Vector3(0f, _movementSpeed * Time.deltaTime, 0f);
        if(transform.position.y >= (_startPosition.y + _movementDistance))
        {
            _currentMovementDirection = MovementDirection.DOWN;
        }
    }

    private void MoveObjectDown()
    {
        transform.position -= new Vector3(0f, _movementSpeed * Time.deltaTime, 0f);
        if(transform.position.y <= (_startPosition.y - _movementDistance))
        {
            _currentMovementDirection = MovementDirection.UP;
        }
    }

    private void MoveObjectLeft()
    {
        transform.position -= new Vector3(_movementSpeed * Time.deltaTime, 0f, 0f);
        if(transform.position.x <= (_startPosition.x - _movementDistance))
        {
            _currentMovementDirection = MovementDirection.RIGHT;
        }
    }

    private void MoveObjectRight()
    {
        transform.position += new Vector3(_movementSpeed * Time.deltaTime, 0f, 0f);
        if(transform.position.x >= (_startPosition.x + _movementDistance))
        {
            _currentMovementDirection = MovementDirection.LEFT;
        }
    }
}
