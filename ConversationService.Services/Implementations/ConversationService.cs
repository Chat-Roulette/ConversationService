using ConversationService.Models;
using ConversationService.Services.Abstractions;
using RabbitMQ.Client;
using StackExchange.Redis.Extensions.Core.Abstractions;
using System.Text;
using System.Text.Json;

namespace ConversationService.Services.Implementations
{
    public class ConversationService : IConversationService
    {
        private readonly IRedisClient _redisClient;
        private readonly IConnectionFactory _connectionFactory;

        public ConversationService(IRedisClient redisClient, IConnectionFactory connectionFactory)
        {
            _redisClient = redisClient;
            _connectionFactory = connectionFactory;
        }

        public async Task<ConversationModel> CreateConversationAsync(IEnumerable<Guid> participants)
        {
            var db = _redisClient.GetDb(0);

            var conversationId = Guid.NewGuid();

            await db.ListAddToLeftAsync(conversationId.ToString(), participants.First());
            await db.ListAddToLeftAsync(conversationId.ToString(), participants.Skip(1).First());

            return new ConversationModel
            {
                Id = conversationId,
                Participants = participants
            };
        }

        public async Task<bool> DeleteConversationAsync(Guid conversationId)
        {
            var db = _redisClient.GetDb(0);

            if (!await db.ExistsAsync(conversationId.ToString()))
            {
                return false;
            };

            await db.RemoveAsync(conversationId.ToString());

            using (var connection = _connectionFactory.CreateConnection())
            {
                using (var model = connection.CreateModel())
                {

                    model.ExchangeDeclare("conversation_removed_notification", ExchangeType.Fanout);

                    var body = JsonSerializer.Serialize(new ConversationRemovedNotificationModel
                    {
                        ConversationId = conversationId
                    });

                    model.BasicPublish("conversation_removed_notification", "", true, null, Encoding.UTF8.GetBytes(body));

                    Console.WriteLine($"REMOVED: {conversationId}");
                }
            }

            return true;
        }
    }
}
