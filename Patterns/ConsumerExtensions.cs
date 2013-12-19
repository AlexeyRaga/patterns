using System;
using System.Collections.Generic;

namespace Patterns.Infrastructure
{
    public static class ConsumerExtensions
    {
        public static IThreadedConsumer<T> AsThreaded<T>(this IConsume<T> consumer)
        {
            return new ThreadedConsumer<T>(consumer);
        } 

        public static IConsume<T> WithRoundRobbinDispatch<T>(this IEnumerable<IConsume<T>> consumers)
        {
            return new RoundRobinDispatchingConsumer<T>(consumers);
        } 

        public static IConsume<T> WithFairDispatch<T>(this IEnumerable<IThreadedConsumer<T>> consumers)
        {
            return new FairDispatchingConsumer<T>(consumers);
        }

        public static IConsume<T> WithTtl<T>(this IConsume<T> consumer)
        {
            return new RespectTtlConsumer<T>(consumer);
        }

        public static IConsume<T> WithFlakiness<T>(this IConsume<T> consumer, int probability)
        {
            return new FlakyConsumer<T>(consumer, probability);
        } 

        public static IConsume<T> WithFaultToletancy<T>(this IConsume<T> consumer, int maxAttemptsCount)
        {
            return new FaultToletantConsumer<T>(maxAttemptsCount, consumer);
        }

        public static IEnumerable<IThreadedConsumer<T>> Start<T>(this IEnumerable<IThreadedConsumer<T>> consumers)
        {
            foreach (var consumer in consumers) consumer.Start();
            return consumers;
        }
    }
}
