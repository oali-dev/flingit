using UnityEngine;

public class DragTouchProcessor : TouchProcessor
{
    private StarController _starController;
    private readonly Vector2 _starPosition;

    /// <summary>
    /// Layer mask of all objects that the star can collide with.
    /// Used in drawing the star trajectory preview by raycasting in the opposite direction of the drag vector.
    /// </summary>
    private readonly int _collidableLayerMask = LayerMask.GetMask("Collidable");

    private const float ReleaseDistance = 0.5f;

    public DragTouchProcessor(StarController starController)
    {
        DebugLogger.Log(this.GetType().Name);
        _starController = starController;
        _starPosition = starController.Transform.position;
    }

    public override void Start(InputManager.TouchInfo touchInfo) {}

    public override void Update(InputManager.TouchInfo touchInfo)
    {
        float dragDistance = GetDragDistance(touchInfo);
        if(dragDistance > ReleaseDistance)
        {
            Vector2 launchDirection = GetLaunchDirection(touchInfo);
            RaycastHit2D hit = Physics2D.Raycast(_starController.transform.position, launchDirection, Mathf.Infinity, _collidableLayerMask);
            if(hit.collider != null)
            {
                Vector2 collisionPoint = hit.point;
                _starController.DrawTrajectoryPreviewLine(collisionPoint);
            }
            else
            {
                Debug.LogError("Star trajectory raycast did not hit any anything");
            }    
        }
        else
        {
            if(_starController.IsTrajectoryPreviewBeingDrawn())
            {
                _starController.HideTrajectoryPreviewLine();
            }    
        }
    }

    public override bool HasEnded(InputManager.TouchInfo touchInfo)
    {
        return (touchInfo.touchState == InputManager.TouchState.RELEASE);
    }

    public override TouchProcessor End(InputManager.TouchInfo touchInfo)
    {
        float dragDistance = GetDragDistance(touchInfo);
        if(dragDistance > ReleaseDistance)
        {
            Vector2 launchDirection = GetLaunchDirection(touchInfo);
            _starController.LaunchStar(launchDirection);
            _starController.HideTrajectoryPreviewLine();
            return new NullTouchProcessor();
        }

        return new IdleTouchProcessor(_starController);
    }

    private float GetDragDistance(InputManager.TouchInfo touchInfo)
    {
        return Vector2.Distance(touchInfo.worldPosition, _starPosition);
    }

    private Vector2 GetLaunchDirection(InputManager.TouchInfo touchInfo)
    {
        return (_starPosition - touchInfo.worldPosition);
    }
}
