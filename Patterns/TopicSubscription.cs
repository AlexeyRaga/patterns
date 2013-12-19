using System;
using System.Collections.Concurrent;
using Patterns.Contract;

namespace Patterns.Infrastructure
{
    public sealed class TopicSubscription : IPublishMessages
    {
        private readonly ConcurrentDictionary<string, Multiplexer<object>> _subscriptions = new ConcurrentDictionary<string, Multiplexer<object>>();

        public void Subscribe<T>(string topic, IConsume<T> consumer)
        {
            var multiplexer = _subscriptions.GetOrAdd(topic, type => new Multiplexer<object>());
            multiplexer.Attach(new DowncastingConsumer<T, object>(consumer));
        }

        public void Subscribe<T>(IConsume<T> consumer)
        {
            var topic = GetTopic(typeof (T));
            Subscribe(topic, consumer);
        }

        public void Publish(object message)
        {
            var topic = GetTopic(message.GetType());
            Publish(topic, message);
        }

        public void Publish(string topic, object message)
        {
            Multiplexer<object> multiplexer;
            if (!_subscriptions.TryGetValue(topic, out multiplexer)) return;

            multiplexer.Consume(message);
        }

        public void Reply(object message)
        {
            var withCorrelation = message as IMessage;
            var topic = withCorrelation != null ? withCorrelation.CorrelationId : GetTopic(message.GetType());

            Publish(topic, message);
        }

        private static string GetTopic(Type type)
        {
            return type.Name;
        }
    }
}
