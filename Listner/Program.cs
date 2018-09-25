using Commons.Domains;
using Newtonsoft.Json;
using System;
using System.IO;
using msmq = System.Messaging;

namespace Listner
{
    class Program
    {
        static void Main(string[] args)
        {
            ReadMessageTx();
        }
        private static void ReadMessageTx()
        {
            using (var queue = new msmq.MessageQueue(".\\private$\\practice.messagequeue.unsubscribe_tx"))
            {
                while (true)
                {
                    Console.WriteLine("Listining...");
                    var message = queue.Receive();

                    var unsunscribeMessage = JsonConvert.
                        DeserializeObject<UnsubscribeCommand>
                        (new StreamReader(message.BodyStream).ReadToEnd());

                    var workFlow = new UnsubscribeWorkFlow(unsunscribeMessage.EmailAddress);

                    Console.WriteLine($"Starting Unsubscribe for: {unsunscribeMessage.EmailAddress}");
                    workFlow.Run();
                    //  queue.Purge();
                    Console.WriteLine($"Completed Unsubscribe for: {unsunscribeMessage.EmailAddress}");
                }
            }
        }

        private static void ReadMessage()
        {
            using (var queue = new msmq.MessageQueue(".\\private$\\practice.messagequeue.unsubscribe"))
            {
                while (true)
                {
                    Console.WriteLine("Listining...");
                    var message = queue.Receive();
                    var bodyReader = new StreamReader(message.BodyStream);
                    var jsonBody = bodyReader.ReadToEnd();
                    var unsunscribeMessage = JsonConvert.DeserializeObject<UnsubscribeCommand>(jsonBody);
                    var workFlow = new UnsubscribeWorkFlow(unsunscribeMessage.EmailAddress);
                    Console.WriteLine($"Starting Unsubscribe for{unsunscribeMessage.EmailAddress}");
                    workFlow.Run();
                    //  queue.Purge();
                    Console.WriteLine($"Completed Unsubscribe for{unsunscribeMessage.EmailAddress}");
                }
            }
        }
    }

    internal class UnsubscribeWorkFlow
    {
        private string emailAddress;

        public UnsubscribeWorkFlow(string emailAddress)
        {
            this.emailAddress = emailAddress;
        }

        public void Run()
        {

        }
    }
}
