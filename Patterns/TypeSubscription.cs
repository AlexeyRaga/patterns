using System;
using System.Collections.Concurrent;

namespace Patterns.Infrastructure
{
    public sealed class TypeSubscription : IPublishMessages
    {
        private readonly ConcurrentDictionary<Type, Multiplexer<object>> _subscriptions = new ConcurrentDictionary<Type, Multiplexer<object>>();

        public void Publish(object message)
        {
            var type = message.GetType();

            Multiplexer<object> multiplexer;
            if (!_subscriptions.TryGetValue(type, out multiplexer)) return;

            multiplexer.Consume(message);
        }

        public void Subscribe<T>(IConsume<T> consumer)
        {
            var multiplexer = _subscriptions.GetOrAdd(typeof(T), type => new Multiplexer<object>());
            multiplexer.Attach(new DowncastingConsumer<T, object>(consumer));
        }
    }
}
