using System.Collections.Generic;

namespace Patterns.Infrastructure
{
    public sealed class Multiplexer<T> : IConsume<T>
    {
        private readonly IList<IConsume<T>> _consumers = new List<IConsume<T>>();

        public void Attach(IConsume<T> consumer)
        {
            _consumers.Add(consumer);
        }

        public void Consume(T instance)
        {
            foreach (var consumer in _consumers)
            {
                consumer.Consume(instance);
            }
        }
    }
}
