namespace Patterns.Infrastructure
{
    public interface IConsume<in T>
    {
        void Consume(T value);
    }
}