using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using System.Threading.Tasks;

namespace AwsServices.S3
{
    public class S3Service
    {
        private readonly IAmazonS3 _s3Client;

        public S3Service(string url)
        {
            var configS3 = new AmazonS3Config
            {
                ServiceURL = url,
                UseHttp = true,
                ForcePathStyle = true,
                AuthenticationRegion = RegionEndpoint.SAEast1.ToString(),
            };
            var creds = new AnonymousAWSCredentials();
            _s3Client = new AmazonS3Client(creds, configS3);
        }

        public S3Service()
        {
            var configS3 = new AmazonS3Config
            {
                ServiceURL = "http://localhost:4566",
                UseHttp = true,
                ForcePathStyle = true,
                AuthenticationRegion = RegionEndpoint.SAEast1.ToString(),
            };
            var creds = new AnonymousAWSCredentials();
            _s3Client = new AmazonS3Client(creds, configS3);
        }

        public async Task<PutBucketResponse> CreateBucketAsync(string bucketName)
        {
            var putBucketRequest = new PutBucketRequest
            {
                BucketName = bucketName,
                UseClientRegion = true
            };

            return await _s3Client.PutBucketAsync(putBucketRequest);
        }

        public async Task<bool> DoesS3BucketExistasync(string bucketName)
        {
            return await AmazonS3Util.DoesS3BucketExistV2Async(_s3Client, bucketName);
        }
    }
}
