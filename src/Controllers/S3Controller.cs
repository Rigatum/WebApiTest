using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using WebApiTest.Models.S3;
using WebApiTest.Models;
using System.IO;

namespace WebApiTest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class S3Controller : ControllerBase
    {
        private S3Configuration S3conf = new S3Configuration();
        [HttpPost("UploadFile")]
        public async Task UploadFileAsync(IFormFile formFile)
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
                System.Console.WriteLine("app exception " + e.Message);
            }
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllFilesAsync()
        {
            IEnumerable<S3ObjectDto> s3Objects; 
            try
            {
                var request = new ListObjectsV2Request
                {
                    BucketName = S3conf.BucketName,
                    MaxKeys = 10,
                };
                ListObjectsV2Response response;

                do
                {
                    response = await S3conf.client.ListObjectsV2Async(request);

                    s3Objects = response.S3Objects.Select(s =>
                    {
                        var urlRequest = new GetPreSignedUrlRequest()
                        {
                            BucketName = S3conf.BucketName,
                            Key = s.Key,
                            Expires = DateTime.UtcNow.AddMinutes(1)
                        };
                        return new S3ObjectDto()
                        {
                            Name = s.Key.ToString(),
                            PresignedUrl = S3conf.client.GetPreSignedURL(urlRequest)
                        };
                    });
                }
                while (response.IsTruncated);
                return Ok(s3Objects);
            }
            catch (AmazonS3Exception ex)
            {
                Console.WriteLine($"Error encountered on server. Message:'{ex.Message}' getting list of objects.");
                return BadRequest();
            }
            catch(Exception e)
            {
                System.Console.WriteLine("app exception " + e.Message);
                return BadRequest();
            }
            
        }

        [HttpGet("GetByKey")]
        public async Task<IActionResult> GetFileByKeyAsync(string key)
        {
            var s3Object = await S3conf.client.GetObjectAsync(S3conf.BucketName, key);
            return File(s3Object.ResponseStream, s3Object.Headers.ContentType, key);
        }

        [HttpDelete("DeleteFile")]
        public async Task<IActionResult> DeleteFuleAsync(string key)
        {
            await S3conf.client.DeleteObjectAsync(S3conf.BucketName, key);
            return NoContent();
        }
    }
}