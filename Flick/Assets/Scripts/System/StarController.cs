using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarController : MonoBehaviour
{
    public Transform Transform { get { return _transform; } }

    [SerializeField]
    private Transform _transform = null;
    [SerializeField]
    private Rigidbody2D _rigidBody = null;
    [SerializeField]
    private Animator _animator = null;
    [SerializeField]
    private LineRenderer _lineRenderer = null;

    private Vector2 _collisionPoint = Vector2.zero;

    private const float ForceToAdd = 700.0f;
    private const string TrajectoryPreviewTexture = "_MainTex";
    private static readonly int IsReleased = Animator.StringToHash("IsReleased");

    private void OnCollisionEnter2D(Collision2D collision)
    {
        DebugLogger.Log("Hit");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DebugLogger.Log("Trigger");
        collision.gameObject.SetActive(false);
    }

    public void LaunchStar(Vector2 direction)
    {
        direction.Normalize();
        _rigidBody.AddForce(direction * ForceToAdd);
        _animator.SetBool(IsReleased, true);
    }

    public void ShowTrajectoryPreviewLine()
    {
        _lineRenderer.gameObject.SetActive(true);
    }

    public void UpdateTrajectoryPreviewLineVertices(Vector2 collisionPoint)
    {
        Vector2 directionNormalized = (collisionPoint - (Vector2)transform.position).normalized;

        _lineRenderer.SetPosition(0, (Vector2)transform.position + directionNormalized * 0.5f);
        _lineRenderer.SetPosition(1, collisionPoint + directionNormalized * 0.5f);
        _lineRenderer.material.SetTextureOffset(TrajectoryPreviewTexture, Vector2.left * Time.time * 2f);
    }

    public void HideTrajectoryPreviewLine()
    {
        _lineRenderer.gameObject.SetActive(false);
    }

    public bool IsTrajectoryPreviewBeingDrawn()
    {
        return _lineRenderer.gameObject.activeSelf;
    }
}
