public class TapTouchProcessor : TouchProcessor
{
    private StarController _starController;

    public TapTouchProcessor(StarController starController)
    {
        DebugLogger.Log(this.GetType().Name);
        _starController = starController;
    }
    public override void Start(InputManager.TouchInfo touchInfo) {}

    public override void Update(InputManager.TouchInfo touchInfo) {}

    public override bool HasEnded(InputManager.TouchInfo touchInfo)
    {
        return _wasPaused || (touchInfo.touchState == InputManager.TouchState.RELEASE);
    }

    public override TouchProcessor End(InputManager.TouchInfo touchInfo)
    {
        if(!_wasPaused)
        {
            _starController.PlaceForceField(touchInfo.worldPosition);
        }
        return new IdleTouchProcessor(_starController);
    }
}
