using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Patterns.Infrastructure
{
    public sealed class FairDispatchingConsumer<T> : IConsume<T>
    {
        private readonly IList<IThreadedConsumer<T>> _consumers;

        public string Name { get { return "SmartDispatcher of " + typeof (T).Name; } }

        public FairDispatchingConsumer(IEnumerable<IThreadedConsumer<T>> consumers)
        {
            _consumers = consumers.ToList();
        }

        public bool TryDispatch(T instance)
        {
            var dispatchToConsumer = _consumers
                .Where(x => x.WaitingItemsCount < 5)
                .OrderBy(x=>x.WaitingItemsCount)
                .FirstOrDefault();

            if (dispatchToConsumer != null)
            {
                dispatchToConsumer.Consume(instance);
                return true;
            }

            return false;
        }

        public void Consume(T instance)
        {
            while (!TryDispatch(instance)) Thread.Sleep(100);
        }
    }
}
