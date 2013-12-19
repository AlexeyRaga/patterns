namespace Patterns.Infrastructure
{
    public interface IPublishMessages
    {
        void Publish(object message);
    }
}
