using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Amazon.S3;
using Amazon.S3.Model;

namespace WebApiTest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class S3Controller : ControllerBase
    {
        public string BucketName = "webapi-bucket-rigat";
        [HttpPost]
        public async Task Post(IFormFile formFile)
        {
            var client = new AmazonS3Client();
            var bucketRequest = new PutBucketRequest()
            {
                BucketName = BucketName,
                UseClientRegion = true
            };
            await client.PutBucketAsync(bucketRequest);
        }
    }
}