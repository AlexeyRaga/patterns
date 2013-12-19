using System;

namespace Patterns.Infrastructure
{
    public class IfElseConsumer<T> : IConsume<T>
    {
        private readonly Func<T, bool> _condition;
        private readonly IConsume<T> _ifConsumer;
        private readonly IConsume<T> _elseConsumer;

        public IfElseConsumer(Func<T, bool> condition, IConsume<T> ifConsumer, IConsume<T> elseConsumer)
        {
            _condition = condition;
            _ifConsumer = ifConsumer;
            _elseConsumer = elseConsumer;
        }

        public void Consume(T value)
        {
            if (_condition(value))
                _ifConsumer.Consume(value);
            else
                _elseConsumer.Consume(value);
        }
    }
}
