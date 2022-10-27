using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarController : MonoBehaviour
{
    public Transform Transform { get { return _transform; } }

    private Transform _transform = null;
    private Rigidbody2D _rigidBody = null;
    private LineRenderer _lineRenderer = null;

    private const float ForceToAdd = 500.0f;
    private const float RotationSpeed = -500.0f;

    private void Awake()
    {
        _transform = this.transform;
        _rigidBody = GetComponent<Rigidbody2D>();
        _lineRenderer = CreateLineRenderer();
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
        _rigidBody.angularVelocity = RotationSpeed;
    }

    public void ShowTrajectoryPreviewLine()
    {
        _lineRenderer.gameObject.SetActive(true);
    }

    public void UpdateTrajectoryPreviewLineVertices(Vector2 collisionPoint)
    {
        Vector2 directionNormalized = (collisionPoint - (Vector2)transform.position).normalized;

        _lineRenderer.SetPosition(1, (Vector2)transform.position + directionNormalized * 0.5f);
        _lineRenderer.SetPosition(0, collisionPoint + directionNormalized * 1f);
        _lineRenderer.material.SetTextureOffset("_MainTex", Vector2.right * Time.time * 2f);
    }

    public void HideTrajectoryPreviewLine()
    {
        _lineRenderer.gameObject.SetActive(false);
    }

    public bool IsTrajectoryPreviewBeingDrawn()
    {
        return _lineRenderer.gameObject.activeSelf;
    }

    private LineRenderer CreateLineRenderer()
    {
        GameObject lineObject = new GameObject("Trajectory Preview");
        lineObject.SetActive(false);

        LineRenderer lineRenderer = lineObject.AddComponent<LineRenderer>();
        lineRenderer.material = Resources.Load<Material>("TrajectoryMaterial");
        lineRenderer.textureMode = LineTextureMode.Tile;
        lineRenderer.startColor = new Color(1.0f, 1.0f, 1.0f, 0.05f);
        lineRenderer.endColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        lineRenderer.positionCount = 2;

        return lineRenderer;
    }
}
