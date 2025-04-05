using JasperFx.Core.Reflection;
using Marten;
using Marten.Events.Daemon;
using Marten.Events.Daemon.Internals;
using Marten.Subscriptions;

namespace PersonLifecycle.WebApi;

public class SubscriberService(
    ILogger<SubscriberService> logger
) : ISubscription
{
    public Task<IChangeListener> ProcessEventsAsync(
        EventRange page,
        ISubscriptionController controller,
        IDocumentOperations operations,
        CancellationToken cancellationToken)
    {
        logger.LogInformation($"Starting to process events from {page.SequenceFloor} to {page.SequenceCeiling}");
        foreach (var e in page.Events)
        {
            logger.LogInformation($"Got event of type {e.Data.GetType().NameInCode()} from stream {e.StreamId}");
        }

        // If you don't care about being signaled for
        return Task.FromResult(NullChangeListener.Instance);
    }

    public ValueTask DisposeAsync()
    {
        return new ValueTask();
    }
}