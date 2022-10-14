using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public LevelData levelData;

    private InputManager _inputManager = null;
    private LevelInstance _levelInstance = null;

    private void Awake()
    {
        // TODO: Don't do this
        GameObject starGameObject = GameObject.Find("Star");
        StarController starController = starGameObject.GetComponent<StarController>();

        _inputManager = new InputManager(starController);
        _levelInstance = new LevelInstance(levelData);
    }

    // Create collidable boarders along each edge of the camera using an edge collider
    // Need to call after WaitForEndOfFrame because Pixel Perfect Camera changes the viewport in LateUpdate
    // TODO: Make this not bad
    private IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        Camera camera = Camera.main;
        Vector2 bottomLeft = camera.ViewportToWorldPoint(new Vector3(0, 0, camera.nearClipPlane));
        Vector2 topLeft = camera.ViewportToWorldPoint(new Vector3(0, 1, camera.nearClipPlane));
        Vector2 topRight = camera.ViewportToWorldPoint(new Vector3(1, 1, camera.nearClipPlane));
        Vector2 bottomRight = camera.ViewportToWorldPoint(new Vector3(1, 0, camera.nearClipPlane));

        Vector2[] edgeColliderPoints = new[] { bottomLeft, topLeft, topRight, bottomRight, bottomLeft };
        List<Vector2> pointList = new List<Vector2>(edgeColliderPoints);
        camera.gameObject.AddComponent<EdgeCollider2D>().SetPoints(pointList);
    }

    private void Update()
    {
        _inputManager.ProcessInput();
    }
}
