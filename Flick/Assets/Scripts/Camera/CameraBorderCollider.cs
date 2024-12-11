using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBorderCollider : MonoBehaviour
{
    [SerializeField]
    public Camera _camera = null;
    [SerializeField]
    public EdgeCollider2D _edgeCollider = null;

    private IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();

        // Create collidable borders along each edge of the camera using an edge collider
        // Need to call after WaitForEndOfFrame because Pixel Perfect Camera changes the viewport in LateUpdate
        CreateCameraBorderColliders();
    }

    private void CreateCameraBorderColliders()
    {
        Vector2 bottomLeft = _camera.ViewportToWorldPoint(new Vector3(0, 0, _camera.nearClipPlane));
        Vector2 topLeft = _camera.ViewportToWorldPoint(new Vector3(0, 1, _camera.nearClipPlane));
        Vector2 topRight = _camera.ViewportToWorldPoint(new Vector3(1, 1, _camera.nearClipPlane));
        Vector2 bottomRight = _camera.ViewportToWorldPoint(new Vector3(1, 0, _camera.nearClipPlane));

        Vector2[] edgeColliderPoints = new[] { bottomLeft, topLeft, topRight, bottomRight, bottomLeft };
        List<Vector2> pointList = new List<Vector2>(edgeColliderPoints);
        _edgeCollider.SetPoints(pointList);
        _edgeCollider.enabled = true;
    }
}
