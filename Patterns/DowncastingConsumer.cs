namespace Patterns.Infrastructure
{
    public sealed class DowncastingConsumer<TExpected, TBase> : IConsume<TBase> where TExpected: TBase
    {
        private readonly IConsume<TExpected> _innerConsumer;

        public DowncastingConsumer(IConsume<TExpected> innerConsumer)
        {
            _innerConsumer = innerConsumer;
        }

        public void Consume(TBase instance)
        {
            if (!(instance is TExpected)) return;

            _innerConsumer.Consume((TExpected) instance);
        }
    }
}