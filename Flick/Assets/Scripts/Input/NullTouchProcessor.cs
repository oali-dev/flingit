public class NullTouchProcessor : TouchProcessor
{
    public NullTouchProcessor() { DebugLogger.Log(this.GetType().Name); }

    public override void Start(InputManager.TouchInfo touchInfo) {}

    public override void Update(InputManager.TouchInfo touchInfo) {}

    public override bool HasEnded(InputManager.TouchInfo touchInfo)
    {
        return false;
    }

    public override TouchProcessor End(InputManager.TouchInfo touchInfo)
    {
        return new NullTouchProcessor();
    }
}
