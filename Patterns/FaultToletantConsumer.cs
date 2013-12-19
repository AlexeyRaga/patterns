using System.Threading;

namespace Patterns.Infrastructure
{
    public sealed class FaultToletantConsumer<T> : IConsume<T>
    {
        private readonly int _retryCount;
        private readonly IConsume<T> _innerConsumer;

        public FaultToletantConsumer(int retryCount, IConsume<T> consumer)
        {
            _retryCount = retryCount;
            _innerConsumer = consumer;
        }

        public void Consume(T value)
        {
            var attempt = 0;
            while (true)
            {
                try
                {
                    attempt++;
                    _innerConsumer.Consume(value);
                }
                catch
                {
                    if (attempt > _retryCount) throw;
                    Thread.Sleep(20);
                }
            }
        }
    }
}
