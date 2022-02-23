using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AwsServices.SQS
{
    public class SQSService
    {
        private readonly IAmazonSQS _sqsClient;

        public SQSService(string url)
        {
            var configSQS = new AmazonSQSConfig() { ServiceURL = url };
            var creds = new AnonymousAWSCredentials();
            _sqsClient = new AmazonSQSClient(creds, configSQS);
        }

        public SQSService()
        {
            var configSQS = new AmazonSQSConfig() { ServiceURL = "http://localhost:4566" };
            var creds = new AnonymousAWSCredentials();
            _sqsClient = new AmazonSQSClient(creds, configSQS);
        }

        public async Task<CreateQueueResponse> CreateQueueAsync(Dictionary<string, string> attributes, string queueName) => await _sqsClient.CreateQueueAsync(new CreateQueueRequest() { Attributes = attributes, QueueName = queueName });

        public async Task<ReceiveMessageResponse> GetMessageAsync(string queueUrl, int waitTime = 0, int maxMessages = 1)
        {
            return await _sqsClient.ReceiveMessageAsync(new ReceiveMessageRequest
            {
                QueueUrl = queueUrl,
                MaxNumberOfMessages = maxMessages,
                WaitTimeSeconds = waitTime
            });
        }

        public async Task<DeleteMessageResponse> DeleteMessageAsync(Message message, string qUrl) => await _sqsClient.DeleteMessageAsync(qUrl, message.ReceiptHandle);

        public async Task<SendMessageResponse> SendMessageAsync(string qUrl, string messageBody) => await _sqsClient.SendMessageAsync(new SendMessageRequest() { MessageGroupId = Guid.NewGuid().ToString(), MessageBody = messageBody, QueueUrl = qUrl }); 

        public async Task<bool> DoesQueueExist(string queueName)
        {
            var listQueue = await _sqsClient.ListQueuesAsync(string.Empty);
            return listQueue.QueueUrls.Where(x => x.Contains(queueName)).Any();
        }

        public async Task<string> GetQueueUrlAsync(string queueName)
        {
            var listQueue = await _sqsClient.ListQueuesAsync(string.Empty);
            return listQueue.QueueUrls.Find(x => x.Contains(queueName));
        }
    }
}
