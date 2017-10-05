using System;
using Apache.NMS;
using TDL.Client.Abstractions;
using TDL.Client.Abstractions.Response;
using TDL.Client.Serialization;
using TDL.Client.Utils;

namespace TDL.Client.Transport
{
    public class RemoteBroker : IRemoteBroker
    {
        private readonly IConnection connection;
        private readonly ISession session;
        private readonly IMessageConsumer messageConsumer;
        private readonly IMessageProducer messageProducer;

        private readonly long timeout;

        public RemoteBroker(
            string hostname,
            int port,
            string uniqueId,
            long timeout)
        {
            this.timeout = timeout;

            var brokerUrl = new Uri($"tcp://{hostname}:{port}");
            var connectionFactory = new Apache.NMS.ActiveMQ.ConnectionFactory(brokerUrl);

            connection = connectionFactory.CreateConnection();
            session = connection.CreateSession(AcknowledgementMode.ClientAcknowledge);

            messageConsumer = session.CreateConsumer(session.GetQueue($"{uniqueId}.req"));
            messageProducer = session.CreateProducer(session.GetQueue($"{uniqueId}.resp"));

            connection.Start();

            messageProducer.DeliveryMode = MsgDeliveryMode.NonPersistent;
        }

        public Maybe<Request> Receive()
        {
            var textMessage = (ITextMessage) messageConsumer.Receive(TimeSpan.FromMilliseconds(timeout));
            if (textMessage == null)
            {
                return Maybe<Request>.None;
            }

            var requestJson = RequestJson.Deserialize(textMessage.Text);
            var request = requestJson.To();

            request.TextMessage = textMessage;

            return Maybe<Request>.Some(request);
        }

        public void RespondTo(Request request, IResponse response)
        {
            var serializedResponse = ResponseJson.Serialize(response);

            var textMessage = session.CreateTextMessage(serializedResponse);
            messageProducer.Send(textMessage);

            request.TextMessage.Acknowledge();
        }

        public void Dispose()
        {
            connection?.Dispose();
            session?.Dispose();
            messageConsumer?.Dispose();
            messageProducer?.Dispose();
        }
    }
}
