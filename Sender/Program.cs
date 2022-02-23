using AwsServices.S3;
using AwsServices.SQS;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sender
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var s3Client = new S3Service();
            var bucketName = "diretorio";
            if (!await s3Client.DoesS3BucketExistasync(bucketName))
                await s3Client.CreateBucketAsync(bucketName);

            var sqsClient = new SQSService();
            var queueName = "teste.fifo";
            var queueUrl = await CreateQueue(queueName, sqsClient);

            Console.WriteLine("Digite Q para sair!\n");

            Console.WriteLine("Digite algo");
            var msg = Console.ReadLine();

            while (msg.ToLower() != "q")
            {
                var response = await sqsClient.SendMessageAsync(queueUrl, msg);
                Console.WriteLine($"Mensagem enviada. Id da mensagem {response.MessageId}\n");
                Console.WriteLine("Digite algo");
                msg = Console.ReadLine();
            }

        }
        private static async Task<string> CreateQueue(string queueName, SQSService sqsClient)
        {
            if (!await sqsClient.DoesQueueExist(queueName))
            {
                Console.WriteLine("Criando fila SQS...");
                var createQueue = await sqsClient.CreateQueueAsync(new Dictionary<string, string>() { { "FifoQueue", "true" } }, queueName);
                Console.WriteLine($"Fila criada, url da fila {createQueue.QueueUrl}\n");
                return createQueue.QueueUrl;
            }

            return await sqsClient.GetQueueUrlAsync(queueName);
        }
    }
}
