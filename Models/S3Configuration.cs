using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using Amazon.S3;
using WebApiTest.Models;
namespace WebApiTest.Models
{
    public class S3Configuration
    {
        public string BucketName { get; set; } = "rigat-bucker";
        public IAmazonS3 client;
        
        public S3Configuration()
        {
            client = new AmazonS3Client(CredentialsApi.PublicApiKey, CredentialsApi.SecretApiKey);
        }
    }
}