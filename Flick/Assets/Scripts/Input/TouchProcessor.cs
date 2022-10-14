public abstract class TouchProcessor
{
    public abstract void Start(InputManager.TouchInfo touchInfo);
    public abstract void Update(InputManager.TouchInfo touchInfo);
    public abstract bool HasEnded(InputManager.TouchInfo touchInfo);
    public abstract TouchProcessor End(InputManager.TouchInfo touchInfo);
}
