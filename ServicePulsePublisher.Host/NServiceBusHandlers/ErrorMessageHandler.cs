using System;
using System.Configuration;
using log4net;
using NServiceBus;
using PublishPlatform.Trello;
using ServiceControl.Contracts;
using ServicePulsePublisher.Host.FeatureToggles;

namespace ServicePulsePublisher.Host.NServiceBusHandlers
{
    public class ErrorMessageHandler : IHandleMessages<MessageFailed>
    {
        public ITrelloClient Trello { private get; set; }
        private readonly string _board = ConfigurationManager.AppSettings["TrelloBoardName"];
        private readonly string _list = ConfigurationManager.AppSettings["TrelloListName"];
        private readonly ILog _log = LogManager.GetLogger(nameof(ErrorMessageHandler));

        public void Handle(MessageFailed message)
        {
            var trelloFeatureToggle = new TrelloPostFeatureToggle();

            _log.Info($"Handling message {message.MessageType}");

            if (trelloFeatureToggle.FeatureEnabled)
            {
                var card = new TrelloCard();
                var maximumRetries = Convert.ToInt32(ConfigurationManager.AppSettings["RetryAttempts"]);
                _log.Info($"Number of attempts {message.NumberOfProcessingAttempts}");
                if (message.Status != MessageFailed.MessageStatus.ArchivedFailure && message.NumberOfProcessingAttempts < maximumRetries)
                {
                    var failureDate = message.FailureDetails.TimeOfFailure;

                    card.Name = $"{failureDate.Date.ToString("dd/MM")} - SC Error - {message.MessageType}";

                    _log.Info($"Creating new card {card.Name}");
                    card.Description = message.FailureDetails.Exception.Message;
                    _log.Info($"Creating new card {card.Description}");
                    card.Description += $"\n\n\tFrom: {message.SendingEndpoint.Name}";
                    card.Description += $"\n\tFailed on: {message.ProcessingEndpoint.Name}";
                    card.Description += $"\n\n\tMessage body: \n\n{message.MessageDetails.Body}";

                    card.UrlSource = "http://yoururl.com";

                    card.Pos = "bottom";

                    card.DueDate = null;

                    Trello.PostCard(card, _board, _list);
                    _log.Info("Card posted");
                }
            }
        }
    }
}
