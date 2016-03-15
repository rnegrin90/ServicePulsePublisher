using NServiceBus;
using ServiceControl.Contracts;

namespace ServicePulsePublisher.Host.NServiceBusHandlers
{
    public class HeartbeatMonitoringHandler : IHandleMessages<HeartbeatStopped>, IHandleMessages<HeartbeatRestored>
    {
        public void Handle(HeartbeatStopped message)
        {
            //throw new NotImplementedException();
        }

        public void Handle(HeartbeatRestored message)
        {
            //throw new NotImplementedException();
        }
    }
}
