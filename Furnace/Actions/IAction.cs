namespace Furnace.Actions;

public interface IAction
{
    public Task RunAsync();
    public event EventHandler ActionCompletedEvent;
}