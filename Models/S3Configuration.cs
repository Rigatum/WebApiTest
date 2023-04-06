using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using Amazon.S3;

namespace WebApiTest.Models
{
    public class S3Configuration
    {
        public string BucketName { get; init; } = "rigat-bucker";
        public string keyName { get; set; } = "AKIAW26X7DYZTT5BF3UD";
        public string Region { get; set; } = "us-east-1";
        public IAmazonS3 client;
        
        public S3Configuration()
        {
            client = new AmazonS3Client("AKIAW26X7DYZTT5BF3UD", "hb4kKVC/gvQt4B0kLzEw3V6uCs0TsHFpFfSpoes5");
        }
    }
}