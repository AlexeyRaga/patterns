using System;

namespace Patterns.Infrastructure
{
    public sealed class FlakyConsumer<T> : IConsume<T>
    {
        private readonly IConsume<T> _consumer;
        private readonly int _probability;

        private readonly Random _rnd = new Random();

        public FlakyConsumer(IConsume<T> consumer, int probability)
        {
            _consumer = consumer;
            _probability = probability;
        }

        public void Consume(T instance)
        {
            if (_rnd.Next(100) < _probability)
            {
                Console.WriteLine("Dropping message");
                return;
            }

            _consumer.Consume(instance);
        }
    }
}
