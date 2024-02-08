namespace CorePublishSubscribe.Subscriber;

public interface ISubscriberService
{
    Task<Task> ListenToSubject(string subject, CancellationToken cancellationToken);
}