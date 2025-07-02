namespace ChoreographyDemo;

public static class EventBus
{
    private static readonly Dictionary<Type, List<Delegate>> Handlers = new();

    public static void Subscribe<T>(Action<T> handler)
    {
        if (!Handlers.ContainsKey(typeof(T)))
            Handlers[typeof(T)] = new List<Delegate>();

        Handlers[typeof(T)].Add(handler);
    }

    public static void Publish<T>(T @event)
    {
        if (Handlers.ContainsKey(@event.GetType()))
        {
            foreach (var handler in Handlers[@event.GetType()])
                ((Action<T>)handler)(@event);
        }
    }
}