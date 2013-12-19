using System;
using Patterns.Contract;

namespace Patterns.Infrastructure
{
    public sealed class RespectTtlConsumer<T> : IConsume<T>
    {
        private readonly IConsume<T> _innerConsumer;

        public RespectTtlConsumer(IConsume<T> innerConsumer)
        {
            _innerConsumer = innerConsumer;
        }

        public void Consume(T instance)
        {
            if (IsExpired(instance)) return;
            _innerConsumer.Consume(instance);
        }

        private static bool IsExpired(T instance)
        {
            var expirable = instance as IExpire;
            if (expirable == null) return false;

            var expired = (expirable.ExpireAt.GetValueOrDefault(DateTime.MaxValue) < DateTime.UtcNow);

            if (expired) Console.WriteLine("Expired");
            return expired;
        }
    }
}
