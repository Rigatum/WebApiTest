using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;

namespace WebApiTest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class S3Controller : ControllerBase
    {
        public string BucketName = "rigat-bucker";
        [HttpPost]
        public async Task Post(IFormFile formFile)
        {
            var client = new AmazonS3Client();
            var bucketExist = await AmazonS3Util.DoesS3BucketExistV2Async(client, BucketName);
            if (!bucketExist)
            {
                var bucketRequest = new PutBucketRequest()
                {
                    BucketName = BucketName,
                    UseClientRegion = true
                };
                await client.PutBucketAsync(bucketRequest);
            }
            var objectRequest = new PutObjectRequest()
            {
                BucketName = BucketName,
                Key = formFile.FileName,
                InputStream = formFile.OpenReadStream(),
            };
            var response = client.PutObjectAsync(objectRequest);
        }
    }
}