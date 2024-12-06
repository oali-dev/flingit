public class NullTouchProcessor : TouchProcessor
{
    private StarController _starController;
    private bool _hasEnded;
    private StarController.CollisionCallback _onHitForceField = null;

    public NullTouchProcessor(StarController starController) {
        DebugLogger.Log(this.GetType().Name);
        _starController = starController;
        _hasEnded = false;
        _onHitForceField = () => { _hasEnded = true; };
        _starController._OnHitForceField += _onHitForceField;
    }

    public override void Start(InputManager.TouchInfo touchInfo) {}

    public override void Update(InputManager.TouchInfo touchInfo) {}

    public override bool HasEnded(InputManager.TouchInfo touchInfo)
    {
        return _hasEnded;
    }

    public override TouchProcessor End(InputManager.TouchInfo touchInfo)
    {
        _starController._OnHitForceField -= _onHitForceField;
        return new IdleTouchProcessor(_starController);
    }
}
