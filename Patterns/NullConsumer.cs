namespace Patterns.Infrastructure
{
    public sealed class NullConsumer<T> : IConsume<T>
    {
        public void Consume(T instance) { }
    }
}
