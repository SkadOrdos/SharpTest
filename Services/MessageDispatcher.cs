using System.Collections.Concurrent;

namespace WebSharp.Services
{
    public enum DispatcherStatus
    {
        Completed = 0,
        Unprocessed = 1,
        Canceled = 2,
        Failed = 3,
    }

    public interface IDispatcherMessage
    {
        string TypeCode { get; }

        bool Cancel { get; set; }
    }


    public abstract class DispatcherMessage : IDispatcherMessage
    {
        public abstract string TypeCode { get; }

        public virtual bool Cancel { get; set; }

        public virtual object ActionResult { get; set; }
        public virtual object ActionError { get; set; }
    }

    public interface IDispatcherClient
    {
        public DispatcherStatus DispatchMessage(IDispatcherMessage message);
    }


    public class MessageDispatcher
    {
        private static MessageDispatcher _instance;
        private static readonly object _lock = new object();

        private readonly ConcurrentDictionary<string, ConcurrentQueue<Action<IDispatcherMessage>>> subscribersMap;
        private MessageDispatcher()
        {
            subscribersMap = new ConcurrentDictionary<string, ConcurrentQueue<Action<IDispatcherMessage>>>();
        }

        public static MessageDispatcher Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new MessageDispatcher();
                    }
                    return _instance;
                }
            }
        }


        public void Subscribe(string key, Action<IDispatcherMessage> client)
        {
            subscribersMap.GetOrAdd(key, (key) => new ConcurrentQueue<Action<IDispatcherMessage>>()).Enqueue(client);
        }

        public DispatcherStatus DoAction(IDispatcherMessage message)
        {
            DispatcherStatus status = DispatcherStatus.Unprocessed;
            if (subscribersMap.TryGetValue(message.TypeCode, out var listClients))
            {
                foreach (var client in listClients)
                {
                    try
                    {
                        client.Invoke(message);
                        status = DispatcherStatus.Completed;
                        if (message.Cancel) break;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("MessageDispatcher::DoAction " + ex);
                        return DispatcherStatus.Failed;
                    }
                }
            }

            return status;
        }
    }
}
