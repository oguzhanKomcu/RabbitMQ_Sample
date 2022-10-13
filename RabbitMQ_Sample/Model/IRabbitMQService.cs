using RabbitMQ.Client;

namespace RabbitMQ_Sample.Model
{
    public interface IRabbitMQService
    {
        public void Connection();
        public IConnection GetConnection();
        public void DeclareExchange(string exchangeName, string exchangeType);
        public void DeclareQueues(string QueueName);
        public IModel CreateOrGetChannel();
        void BindQueu(string exName, string queuName, string routingKey);
        void Publish(string exname, string routingKey, object dataMessage);
        void WriteDataToEchange(string exname, string routingKey, object data);
    }
}
