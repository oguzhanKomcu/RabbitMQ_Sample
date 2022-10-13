using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ_Sample.Model;

namespace RabbitMQ_Sample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RabbitsController : ControllerBase
    {

        private readonly IRabbitMQService rabbitMq;
        public RabbitsController(IRabbitMQService rabbitMQService )
        {

            this.rabbitMq = rabbitMQService;
        }


        [HttpPost]
        public void Connection()
        {
       
            rabbitMq.Connection();
        }


        [HttpPost("ExchangeCreater")]
        public void ExchangeCreater(string exname, string exType)
        {
            rabbitMq.DeclareExchange(exname,exType);
        }


        [HttpPost("QueuCreater")]
        public void QueuCreater(string queuName)
        {
            rabbitMq.DeclareQueues(queuName);
        }


        [HttpPost("BindQueuCreater")]
        public void BindQueuCreater(string exName, string queuName, string routingKey)
        {
            rabbitMq.BindQueu(exName,queuName,routingKey);
        }


        [HttpPost("PublishMessage")]
        public void PublishMessage(string exname, string routingKey, MessageQueu dataMessage)
        {
            rabbitMq.Publish(exname, routingKey, dataMessage);

        }

    }
}
