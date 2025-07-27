using Microsoft.Extensions.Logging;

namespace Demo.DecoratedHandlers.Gen;

// todo ensure not visible in user code
// add log publication via hosted service
public class DelayedLog
{
    List<Action<ILogger>> _logs = [];

    internal void Add(Action<ILogger> log)
    {
        Logs.Add(log);
    }

    private List<Action<ILogger>> Logs
    {
        get
        {
            if (_logs != null) return _logs;

            throw new NotSupportedException("Logs can't be accessed after Erase() has been called.");
        }
    }

    public void Apply(ILogger logger)
    {
        foreach (var action in Logs)
        {
            action(logger);
        }
    }

    public void Erase()
    {
        _logs = null;
    }

    public int Count()
    {
        return Logs.Count;
    }
}