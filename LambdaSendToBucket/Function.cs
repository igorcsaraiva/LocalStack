using Amazon;
using Amazon.Lambda.Core;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using System.Threading.Tasks;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace LambdaSendToBucket
{
    public class Function
    {
        private readonly IAmazonS3 _client;

        public Function()
        {
            var configS3 = new AmazonS3Config
            {
                ServiceURL = "http://localhost:4566",
                UseHttp = true,
                ForcePathStyle = true,
                AuthenticationRegion = RegionEndpoint.SAEast1.ToString(),
            };
            var creds = new AnonymousAWSCredentials();
            _client = new AmazonS3Client(creds, configS3);
        }
        public async Task<PutObjectResponse> FunctionHandler(string input, ILambdaContext context, string messageId)
        {
            var putRequest = new PutObjectRequest
            {
                BucketName = "diretorio",
                Key = $"Arquivo {messageId}",
                ContentBody = input
            };

            PutObjectResponse response = await _client.PutObjectAsync(putRequest);

            return response;
        }
    }
}
