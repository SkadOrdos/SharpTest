using System.Collections.Concurrent;
using System.Text.Json;

namespace WebSharp.Services
{
    /// <summary>
    /// Service for process numbers
    /// </summary>
    public class FileWorker : IDisposable
    {
        Thread mainThread;
        CountdownEvent waitEvent = new CountdownEvent(1);
        ConcurrentQueue<ArrayData> queue = new ConcurrentQueue<ArrayData>();
        public FileWorker()
        {
            MessageDispatcher.Instance.Subscribe(TagretsMessage.CODE, (args) => Process((TagretsMessage)args));

            mainThread = new Thread(ExecutionThread);
            mainThread.Priority = ThreadPriority.Highest;
            mainThread.Start();
        }


        ArrayData lastInfo;
        internal DispatcherStatus Process(TagretsMessage args)
        {
            try
            {
                lastInfo = new ArrayData(args.Data, lastInfo);
                queue.Enqueue(lastInfo);
                waitEvent.Signal();

                return DispatcherStatus.Completed;
            }
            catch (Exception ex)
            {
                return DispatcherStatus.Unprocessed;
            }
        }

        public EventHandler<ArrayData> OnNewData;
        private void ExecutionThread()
        {
            while (true)
            {
                waitEvent.Wait();
                queue.TryDequeue(out ArrayData arrayInfo);

                if (arrayInfo != null)
                {
                    var digits = arrayInfo.Data.Cast<int>().Where(v => v > 0).ToArray();
                    File.WriteAllText("digits.json", JsonSerializer.Serialize(digits));
                    OnNewData?.Invoke(this, arrayInfo);
                }
            }
        }

        public void Dispose()
        {
            mainThread.Abort();
        }
    }
}
