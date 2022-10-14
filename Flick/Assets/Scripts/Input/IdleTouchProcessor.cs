using UnityEngine;

public class IdleTouchProcessor : TouchProcessor
{
    private StarController _starController;

    public IdleTouchProcessor(StarController starController)
    {
        DebugLogger.Log(this.GetType().Name);
        _starController = starController;
    }

    public override void Start(InputManager.TouchInfo touchInfo) {}

    public override void Update(InputManager.TouchInfo touchInfo) {}

    public override bool HasEnded(InputManager.TouchInfo touchInfo)
    {
        return (touchInfo.touchState == InputManager.TouchState.START);
    }

    public override TouchProcessor End(InputManager.TouchInfo touchInfo)
    {
        if(touchInfo.collider != null)
        {
            return new DragTouchProcessor(_starController);
        }

        return new TapTouchProcessor(_starController);
    }
}
