using AwsServices.SQS;
using LambdaSendToBucket;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Consumer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var sqsClient = new SQSService();

            var queueUrl = await sqsClient.GetQueueUrlAsync("teste.fifo");

            do
            {
                var msg = await sqsClient.GetMessageAsync(queueUrl);
                if(msg.Messages.Count != 0)
                {
                    Console.WriteLine("Enviando mensagem pra lambda...");
                    Console.WriteLine($"Id da mensagem: {msg.Messages[0].MessageId}\nConteudo da mensagem: {msg.Messages[0].Body}");
                    var lambdaFunction = new Function();

                    var response = await lambdaFunction.FunctionHandler(msg.Messages[0].Body, null, msg.Messages[0].MessageId);

                    if(response.HttpStatusCode == HttpStatusCode.OK)
                    {
                        Console.WriteLine("Apagando mensagem da fila...");
                        var deleteResponse = await sqsClient.DeleteMessageAsync(msg.Messages[0], queueUrl);
                        if(deleteResponse.HttpStatusCode == HttpStatusCode.OK)
                            Console.WriteLine("Mensagem apagada.");
                    }

                }

            } while (!Console.KeyAvailable);
        }
    }
}
