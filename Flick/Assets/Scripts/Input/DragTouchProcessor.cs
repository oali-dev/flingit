using UnityEngine;

public class DragTouchProcessor : TouchProcessor
{
    private StarController _starController;
    private readonly Vector2 _starPosition;

    private const float ReleaseDistance = 1.5f;
    private static readonly int CollidableLayerMask = LayerMask.GetMask("Collidable");

    public DragTouchProcessor(StarController starController)
    {
        DebugLogger.Log(this.GetType().Name);
        _starController = starController;
        _starPosition = starController.Transform.position;
    }

    public override void Start(InputManager.TouchInfo touchInfo)
    {
        _starController.StopReturningChildToOrigin();
    }

    public override void Update(InputManager.TouchInfo touchInfo)
    {
        Vector2 launchDirection = GetLaunchDirection(touchInfo);
        float dragDistance = GetDragDistance(touchInfo);
        if(dragDistance > ReleaseDistance)
        {
            RaycastHit2D hit = Physics2D.Raycast(_starPosition, launchDirection, Mathf.Infinity, CollidableLayerMask);
            if(hit.collider != null)
            {
                Vector2 collisionPoint = hit.point;
                _starController.UpdateTrajectoryPreviewLineVertices(collisionPoint);
            }
            else
            {
                Debug.LogError("Star trajectory raycast did not hit any anything");
            }

            if(!_starController.IsTrajectoryPreviewBeingDrawn())
            {
                _starController.ShowTrajectoryPreviewLine();
            }
        }
        else
        {
            if(_starController.IsTrajectoryPreviewBeingDrawn())
            {
                _starController.HideTrajectoryPreviewLine();
            }    
        }

        Vector2 touchWorldPositionClamped = Vector2.ClampMagnitude(-launchDirection, ReleaseDistance);
        _starController.MoveChild(touchWorldPositionClamped);
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
        return _starPosition - touchInfo.worldPosition;
    }
}
