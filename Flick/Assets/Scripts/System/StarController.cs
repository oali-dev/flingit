using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarController : MonoBehaviour
{
    public Transform Transform { get { return _transform; } }

    private Transform _transform = null;
    private Rigidbody2D _rigidBody = null;
    private Vector2 _collisionPoint = Vector2.zero;

    [SerializeField]
    private LineRenderer _lineRenderer = null;

    private const float ForceToAdd = 700.0f;

    private void Awake()
    {
        _transform = this.transform;
        _rigidBody = GetComponent<Rigidbody2D>();
        if(_lineRenderer == null)
        {
            Debug.LogError("Trajectory preview line renderer not assigned.");
        }
    }

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
        _lineRenderer.material.SetTextureOffset("_MainTex", Vector2.left * Time.time * 2f);
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
