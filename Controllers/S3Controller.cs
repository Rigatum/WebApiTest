using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using WebApiTest.Models;
using System.IO;

namespace WebApiTest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class S3Controller : ControllerBase
    {
        private S3Configuration S3conf = new S3Configuration();
        [HttpPost]
        public async Task Post(IFormFile formFile)
        {
            await using var newMemoryStream = new MemoryStream();
            formFile.CopyTo(newMemoryStream);
            try
            {
                var putRequest = new PutObjectRequest
                {
                    InputStream = newMemoryStream,
                    BucketName = S3conf.BucketName,
                    Key = formFile.FileName
                };
                PutObjectResponse response = await S3conf.client.PutObjectAsync(putRequest);
            }
            catch(AmazonS3Exception e)
            {
                System.Console.WriteLine("Amazon exception " + e.Message);
            }
            catch(Exception e)
            {
                System.Console.WriteLine("exception " + e.Message);
            }
        }
    }
}