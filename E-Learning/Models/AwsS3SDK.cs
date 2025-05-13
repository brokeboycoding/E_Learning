using Amazon.S3.Transfer;
using Amazon.S3;

namespace E_Learning.Models
{
    public class AwsS3SDK
    {
        private readonly IConfiguration _config;

        public AwsS3SDK(IConfiguration config)
        {
            _config = config;
        }

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            var accessKey = _config["AWS:AKIA5WLTTHEW3EGMFZYX"];
            var secretKey = _config["AWS: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"];
            var bucketName = _config["AWS:khangkhang12213"];
            var region = Amazon.RegionEndpoint.GetBySystemName(_config["AWS:ap-southeast-1"]);

            using var client = new AmazonS3Client(accessKey, secretKey, region);
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);

            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);

            var uploadRequest = new TransferUtilityUploadRequest
            {
                InputStream = memoryStream,
                Key = fileName,
                BucketName = bucketName,
                CannedACL = S3CannedACL.PublicRead
            };

            var transferUtility = new TransferUtility(client);
            await transferUtility.UploadAsync(uploadRequest);

            return $"https://{bucketName}.s3.{region.SystemName}.amazonaws.com/{fileName}";
        }
    }
}
