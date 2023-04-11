using System.Diagnostics;
using MassTransit;
using Serilog;

namespace Sample.Components;

public class ActivityDiagnosticsPublishFilter<TMessage>
    : IFilter<PublishContext<TMessage>> where TMessage : class
{
    public Task Send(PublishContext<TMessage> context, IPipe<PublishContext<TMessage>> next)
    {
        Log.Information("Publishing baggage for activity {Activity}", Activity.Current?.Id);
        foreach (var baggage in Activity.Current.Baggage)
        {
            Log.Information("Key {Key} Value {Value}", baggage.Key, baggage.Value);
        }

        return next.Send(context);
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope("activity");
    }
}