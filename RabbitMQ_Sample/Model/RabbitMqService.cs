using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading.Channels;

namespace RabbitMQ_Sample.Model
{
    public class RabbitMqService : IRabbitMQService
    {
        private IConnection _connection;
        private bool isConnectionOpen;

        private IModel _channel;

        private IModel channel => _channel ?? (_channel = CreateOrGetChannel());

        public void Connection()
        {
            if (!isConnectionOpen || _connection == null)
            {
                //connection olusturulmasmıssa bir collection olurstur.
                _connection = GetConnection();
            }
            else
            {
                _connection.Close();
                isConnectionOpen = _connection.IsOpen;
            }
        }
        public IConnection GetConnection()
        {
            ConnectionFactory factory = new ConnectionFactory()
            {
                //rabbitmq ya giriş yapabilmek için 1.guest userName 2.guest paralomız . Host ise rabbitmqnun hosting servis bilhileridir.
                Uri = new  Uri("amqp://guest:guest@localhost:5672", UriKind.RelativeOrAbsolute)
            };
            return factory.CreateConnection();
        }

        //Bu tipte ve isimde bir exchange olustumak istediğimizi belirtiyoruz.
        public void DeclareExchange(string exchangeName , string exchangeType)
        {
            channel.ExchangeDeclare(exchangeName, exchangeType);
        }


        //Göndereceğimiz mesajı alan bir Queu olusturuyoruz.
        public void DeclareQueues(string queuName)
        {
            channel.QueueDeclare(queuName, exclusive: false);
        }

        //Exchange olusturulurken bu kanal(channel) üzerinden olusturulsun diyoruz.
        public IModel CreateOrGetChannel()
        {
          return _connection.CreateModel();
        }


        //Exchange ile queu muzu birbirine bağladık.
        public void BindQueu(string exName, string queuName, string routingKey)
        {
            channel.QueueBind(queuName,exName,  routingKey);
        }

        public void Publish(string exname, string routingKey, object dataMessage)
        {
            WriteDataToEchange(exname,routingKey,dataMessage);

        }

        public void WriteDataToEchange(string exname, string routingKey, object data)
        {
            var dataArr = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));

            channel.BasicPublish(exname, routingKey, null, dataArr);
        }
    }
}
