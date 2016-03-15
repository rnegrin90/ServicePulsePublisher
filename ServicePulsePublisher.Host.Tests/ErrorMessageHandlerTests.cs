using System;
using System.Collections.Generic;
using System.Reflection;
using NServiceBus;
using NServiceBus.Testing;
using NUnit.Framework;
using PublishPlatform.Trello;
using ServiceControl.Contracts;
using ServicePulsePublisher.Host.NServiceBusHandlers;

namespace ServicePulsePublisher.Host.Tests
{
    [TestFixture]
    public class ErrorMessageHandlerTests
    {
        [OneTimeSetUp]
        public void TestFixtureSetup()
        {
            Test.Initialize(x =>
            {
                x.AssembliesToScan(new List<Assembly>
                   {
                       Assembly.Load("ServiceControl.Contracts")
                   });
                x.Conventions().DefiningEventsAs(t => typeof(IEvent).IsAssignableFrom(t) || t.Namespace != null && t.Namespace.StartsWith("ServiceControl.Contracts"));
            });
        }

        [Test]
        [Ignore("This will post a card in Trello")]
        [Category("Integration")]
        public void Handle_OnValidErrorMessage_PostsTrelloCard()
        {
            Test.Handler<ErrorMessageHandler>()
                .WithExternalDependencies(d => d.Trello = new TrelloClient())
                .OnMessage<MessageFailed>(m =>
                {
                    m.MessageDetails = new MessageFailed.Message {Body = "this is the transaction that failed in a file format"};
                    m.MessageType = "a message type";
                    m.Status = MessageFailed.MessageStatus.Failed;
                    m.NumberOfProcessingAttempts = 1;
                    m.FailureDetails = new MessageFailed.FailureInfo {TimeOfFailure = DateTime.Now, Exception = new MessageFailed.FailureInfo.ExceptionInfo {Message = "message on the exception"} };
                    m.SendingEndpoint = new MessageFailed.Endpoint {Name = "a sender endpoint"};
                    m.ProcessingEndpoint = new MessageFailed.Endpoint {Name = "a processing endpoint"};
                });
        }
    }
}
