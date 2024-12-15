public abstract class TouchProcessor
{
    protected bool _wasPaused;

    protected TouchProcessor()
    {
        _wasPaused = false;
    }

    public abstract void Start(InputManager.TouchInfo touchInfo);
    public abstract void Update(InputManager.TouchInfo touchInfo);
    public abstract bool HasEnded(InputManager.TouchInfo touchInfo);
    public abstract TouchProcessor End(InputManager.TouchInfo touchInfo);

    public void Pause()
    {
        _wasPaused = true;
    }
}
