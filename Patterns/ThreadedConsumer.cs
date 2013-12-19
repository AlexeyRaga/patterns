using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Patterns.Infrastructure
{
    public interface IThreadedConsumer<in T> : IConsume<T>
    {
        int WaitingItemsCount { get; }
        void Start();
        void Stop();
    }

    public sealed class ThreadedConsumer<T> : IThreadedConsumer<T>, IDisposable
    {
        private readonly CancellationTokenSource _cancellation = new CancellationTokenSource();

        private readonly IConsume<T> _innerConsumer;
        private readonly ConcurrentQueue<T> _queue = new ConcurrentQueue<T>();
        
        public int WaitingItemsCount { get { return _queue.Count; } }

        public ThreadedConsumer(IConsume<T> innerConsumer)
        {
            _innerConsumer = innerConsumer;
        }

        public void Consume(T instance)
        {
            _queue.Enqueue(instance);
        }

        public void Start()
        {
            Task.Factory.StartNew(ConsumeNextWaitingItem, _cancellation.Token);
        }

        private void ConsumeNextWaitingItem()
        {
            while (!_cancellation.IsCancellationRequested)
            {
                T payload;
                if (_queue.TryDequeue(out payload))
                {
                    _innerConsumer.Consume(payload);
                }
                else
                {
                    Thread.Sleep(100);
                }
            }
        }

        public void Stop() { _cancellation.Cancel(); }

        public void Dispose() { Stop(); }
    }
}
