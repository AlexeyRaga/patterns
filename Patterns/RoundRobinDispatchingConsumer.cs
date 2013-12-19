using System.Collections.Generic;
using System.Linq;

namespace Patterns.Infrastructure
{
    public sealed class RoundRobinDispatchingConsumer<T> : IConsume<T>
    {
        private readonly IList<IConsume<T>> _consumers;
        private int _nextIndex = 0;

        public RoundRobinDispatchingConsumer(IEnumerable<IConsume<T>> consumers)
        {
            _consumers = consumers.ToList();
        }

        public void Consume(T instance)
        {
            _consumers[_nextIndex].Consume(instance);
            IncrementNextIndex();
        }

        private void IncrementNextIndex()
        {
            _nextIndex++;
            if (_nextIndex == _consumers.Count) _nextIndex = 0;
        }
    }
}
