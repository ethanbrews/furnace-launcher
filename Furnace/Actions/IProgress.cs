namespace Furnace.Actions;

public interface IProgress
{
    public event EventHandler<int> ProgressChangedEvent;
}