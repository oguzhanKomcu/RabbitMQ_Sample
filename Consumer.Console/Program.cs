using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

internal class Program
{

    private static string connectionString = "amqp://guest:guest@localhost:5672";
    private static string queueName;
    private static IConnection connection;
    private static IModel _channel;

    private static IModel channel => _channel ?? (_channel = CreateOrGetChannel());
    private static void Main(string[] args)
    {



        // oguzq1 : Olusturmuş oldugum queu muzun ismi
        queueName = args.Length > 0 ? args[0] : "oguzq1";

        connection = GetConnection();



        var consumerEvent = new EventingBasicConsumer(channel);

        consumerEvent.Received += (ch, e) =>
        {
            var byteArr = e.Body.ToArray();
            var bodyStr = Encoding.UTF8.GetString(byteArr);

            Console.WriteLine($"Received Data: {bodyStr}");

            channel.BasicAck(e.DeliveryTag, false);
        };

        //false : Datanın alınıp alınılmadıgını producera gönderilsin mi diye soruyor.
        //consumerEvent : gelen olayı dinler. Yukarıda byte satatsını alıp stringe cevirdik ve veriyi console bastırdık
        channel.BasicConsume(queueName, false, consumerEvent);

        Console.WriteLine($"{queueName} listening....\n\n\n");



        Console.ReadLine();
    }
    private static IModel CreateOrGetChannel()
    {
        return connection.CreateModel();
    }
    public static IConnection GetConnection()
    {
        ConnectionFactory factory = new ConnectionFactory()
        {
            //rabbitmq ya giriş yapabilmek için 1.guest userName 2.guest paralomız . Host ise rabbitmqnun hosting servis bilhileridir.
            Uri = new Uri("amqp://guest:guest@localhost:5672", UriKind.RelativeOrAbsolute)
        };
        return factory.CreateConnection();
    }
}