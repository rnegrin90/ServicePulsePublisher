using NServiceBus;
using NServiceBus.Log4Net;
using PublishPlatform.Trello;

namespace ServicePulsePublisher.Host
{ 
    public class EndpointConfig : IConfigureThisEndpoint
    {
        public void Customize(BusConfiguration configuration)
        {
            log4net.Config.XmlConfigurator.Configure();

            configuration.UsePersistence<InMemoryPersistence>();
            configuration.UseSerialization<JsonSerializer>();
            configuration.Conventions().DefiningEventsAs(t => typeof(IEvent).IsAssignableFrom(t) || t.Namespace != null && t.Namespace.StartsWith("ServiceControl.Contracts"));
            NServiceBus.Logging.LogManager.Use<Log4NetFactory>();

            configuration.RegisterComponents(c => c.ConfigureComponent<TrelloClient>(DependencyLifecycle.InstancePerCall));
        }
    }
}
