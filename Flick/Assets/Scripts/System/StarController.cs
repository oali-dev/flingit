using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarController : MonoBehaviour
{
    public Transform Transform { get { return _transform; } }

    [SerializeField]
    private Transform _transform = null;
    [SerializeField]
    private Transform _childTransform = null;
    [SerializeField]
    private Rigidbody2D _rigidBody = null;
    [SerializeField]
    private Animator _animator = null;
    [SerializeField]
    private LineRenderer _lineRenderer = null;
    [SerializeField]
    private GameObject _rippleEffect = null;
    [SerializeField]
    private GameObject _forceField = null;

    private Coroutine ResetChildPositionCoroutine = null;
    public int _numberOfForceFieldsAllowed { private get; set; }

    public delegate void CollisionCallback();
    private CollisionCallback _onHitWall = null;
    private CollisionCallback _onCollectPoint = null;
    public CollisionCallback _OnHitForceField = null;

    private const float ForceToAdd = 700.0f;
    private const float TimeToReturnToOrigin = 0.1f;
    private const float ForceFieldPlacementDeadzone = 2.2f;
    private const string TrajectoryPreviewTexture = "_MainTex";
    private static readonly int IsReleased = Animator.StringToHash("IsReleased");
    private static readonly string ForceFieldString = "ForceField";

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _onHitWall();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(ForceFieldString))
        {
            if(_OnHitForceField != null)
            {
                _OnHitForceField();
                _rigidBody.linearVelocity = Vector2.zero;

                _rippleEffect.transform.position = _transform.position;
                _rippleEffect.SetActive(true);
                _forceField.SetActive(false);

                _numberOfForceFieldsAllowed--;
            }
            return;
        }

        _onCollectPoint();
        collision.gameObject.SetActive(false);
    }

    public void LaunchStar(Vector2 direction)
    {
        // Set the root position to where the child is visually,
        // then reset the child position to the origin again so we can prepare to launch the star
        _transform.position = _childTransform.position;
        _childTransform.localPosition = Vector3.zero;

        direction.Normalize();
        _rigidBody.AddForce(direction * ForceToAdd);
        _animator.SetBool(IsReleased, true);
        _rippleEffect.SetActive(false);
        //StartReturningChildToOrigin();
    }

    public void MoveChild(Vector2 position)
    {
        _childTransform.position = position;
    }

    public void StartReturningChildToOrigin()
    {
        ResetChildPositionCoroutine = CoroutineManager.Instance.StartCoroutine(ResetChildPositionOverTime());
    }

    public void StopReturningChildToOrigin()
    {
        if(ResetChildPositionCoroutine is null)
        {
            return;
        }
        CoroutineManager.Instance.StopCoroutine(ResetChildPositionCoroutine);
    }

    public void ShowTrajectoryPreviewLine()
    {
        _lineRenderer.gameObject.SetActive(true);
    }

    public void HideTrajectoryPreviewLine()
    {
        _lineRenderer.gameObject.SetActive(false);
    }

    public void UpdateTrajectoryPreviewLineVertices(Vector2 collisionPoint)
    {
        Vector2 directionNormalized = (collisionPoint - (Vector2)_transform.position).normalized;

        _lineRenderer.SetPosition(0, (Vector2)_transform.position + directionNormalized * 0.5f);
        _lineRenderer.SetPosition(1, collisionPoint + directionNormalized * 0.5f);
        _lineRenderer.material.SetTextureOffset(TrajectoryPreviewTexture, Vector2.left * Time.time);
    }

    public bool IsTrajectoryPreviewBeingDrawn()
    {
        return _lineRenderer.gameObject.activeSelf;
    }

    public void PlaceForceField(Vector2 position)
    {
        // Checking if position is inside the circle of radius ForceFieldPlacementDeadzone
        float xp = position.x;
        float yp = position.y;
        float xc = _transform.position.x;
        float yc = _transform.position.y;

        float  d = Mathf.Sqrt(Mathf.Pow(xp - xc, 2) + Mathf.Pow(yp - yc, 2));

        bool isPositionOutsideDeadzone = (d > ForceFieldPlacementDeadzone);
        if((_numberOfForceFieldsAllowed > 0) && isPositionOutsideDeadzone)
        {
            _forceField.transform.position = (Vector3)position;
            _forceField.SetActive(true);
        }
    }

    public void HookUpCollisionCallbacks(CollisionCallback onHitWall, CollisionCallback onCollectPoint)
    {
        _onHitWall = onHitWall;
        _onCollectPoint = onCollectPoint;
    }

    public void DestroyStar()
    {
        _transform.gameObject.SetActive(false);
    }

    private IEnumerator ResetChildPositionOverTime()
    {
        Vector2 velocity = Vector2.zero;
        while(!(_childTransform.localPosition == Vector3.zero))
        {
            _childTransform.localPosition = Vector2.SmoothDamp(_childTransform.localPosition, Vector2.zero, ref velocity, TimeToReturnToOrigin);
            yield return null;
        }

        _childTransform.localPosition = Vector3.zero;
    }
}
