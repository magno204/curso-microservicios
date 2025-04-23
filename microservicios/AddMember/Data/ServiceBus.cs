using Azure.Messaging.ServiceBus;
using System.Text;
using System.Text.Json;

namespace AddMember.Data
{
    public class ServiceBus
    {
        private readonly string _connectionString;
        private readonly string _queueName;

        public ServiceBus(string connectionString, string queueName)
        {
            _connectionString = connectionString;
            _queueName = queueName;
        }

        public async Task SendMessageAsync(string name, string lastname, string birthyear)
        {
            var client = new ServiceBusClient(_connectionString);
            var sender = client.CreateSender(_queueName);

            var messageBody = ($"Name: {name}, Lastname: {lastname}, Birthyear: {birthyear}");
            var jsonMessage = JsonSerializer.Serialize(messageBody);
            var serviceBusMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(jsonMessage));

            await sender.SendMessageAsync(serviceBusMessage);
        }
    }
}