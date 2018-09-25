using Commons.Domains;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using msmq = System.Messaging;

namespace Messanger
{
    class Program
    {
        static void Main(string[] args)
        {
            var _email = Console.ReadLine();
            Console.WriteLine(_email);

            var unsubscribeCommand = new UnsubscribeCommand
            {
                EmailAddress = _email
            };
            SendMessageTx(unsubscribeCommand);
            Console.WriteLine("Message is send to MSMQ");
            Console.ReadKey();
        }
        private static void SendMessageTx(UnsubscribeCommand unsubscribeCommand)
        {
            using (var queue = new msmq.MessageQueue(".\\private$\\practice.messagequeue.unsubscribe_tx"))
            {
                var message = new msmq.Message();
                var jsonBody = JsonConvert.SerializeObject(unsubscribeCommand);
                message.BodyStream = new MemoryStream(Encoding.Default.GetBytes(jsonBody));
                var tx = new msmq.MessageQueueTransaction();
                tx.Begin();
                queue.Send(message, tx);
                tx.Commit();
            }
        }
        private static void SendMessage(UnsubscribeCommand unsubscribeCommand)
        {
            using (var queue = new msmq.MessageQueue(".\\private$\\practice.messagequeue.unsubscribe"))
            {
                var message = new msmq.Message();
                var jsonBody = JsonConvert.SerializeObject(unsubscribeCommand);
                message.BodyStream = new MemoryStream(Encoding.Default.GetBytes(jsonBody));
                queue.Send(message);
            }
        }
    }


}
