using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;

public class BucketManager
{
    private string bucketName;
    private static IAmazonS3 client;

    public BucketManager(string bucketName, RegionEndpoint region)
    {
        this.bucketName = bucketName;
        client = new AmazonS3Client(region);
    }

    public async Task<string> GetURL(string fileName)
    {
        // Get a pre-signed URL for the object
        var preSignedUrlRequest = new GetPreSignedUrlRequest
        {
            BucketName = bucketName,
            Key = fileName,
            Verb = HttpVerb.GET,
            Expires = DateTime.Now.AddHours(1) // Set expiration time in seconds (1 hour)
        };

        String url = await client.GetPreSignedURLAsync(preSignedUrlRequest);
        return url;
    }
    public async Task<string> Upload(string filePath, string contentType)
    {
        string key = Path.GetFileName(filePath);
        try
        {
            var putRequest = new PutObjectRequest
            {
                BucketName = bucketName,
                Key = key,
                FilePath = filePath,
                ContentType = contentType 
            };
            
            PutObjectResponse response2 = await client.PutObjectAsync(putRequest);
        }
        catch (AmazonS3Exception e)
        {
            Console.WriteLine(
                    "Error encountered ***. Message:'{0}' when writing an object"
                    , e.Message);
        }
        catch (Exception e)
        {
            Console.WriteLine(
                "Unknown encountered on server. Message:'{0}' when writing an object"
                , e.Message);
        }
        return key;
    }

    public async Task<string> Upload(byte[] binaryData, string contentType, string path="")
    {
        string key = Guid.NewGuid().ToString(); // Generate a unique key for the object
        if (!String.IsNullOrEmpty(path))
        {
            key = path + "/" + key;
        }
        try
        {
            using (var memoryStream = new MemoryStream(binaryData))
            {
                var putRequest = new PutObjectRequest
                {
                    BucketName = bucketName,
                    Key = key,
                    InputStream = memoryStream,
                    ContentType = contentType
                };
                PutObjectResponse response2 = await client.PutObjectAsync(putRequest);
            }
        }
        catch (AmazonS3Exception e)
        {
            Console.WriteLine(
                    "Error encountered ***. Message:'{0}' when writing an object"
                    , e.Message);
        }
        catch (Exception e)
        {
            Console.WriteLine(
                    "Unknown encountered on server. Message:'{0}' when writing an object"
                    , e.Message);
        }
        return key;
    }
}