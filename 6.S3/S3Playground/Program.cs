﻿using System.Text;
using Amazon.S3;
using Amazon.S3.Model;

var s3Client = new AmazonS3Client();

// await using var inputStream = new FileStream("./face.jpg", FileMode.Open, FileAccess.Read);
//
// var putObjectRequest = new PutObjectRequest
// {
//     BucketName = "keomaawscourse",
//     Key = "images/face.jpg",
//     ContentType = "image/jpeg",
//     InputStream = inputStream
// };

// await using var inputStream = new FileStream("./movies.csv", FileMode.Open, FileAccess.Read);
//
// var putObjectRequest = new PutObjectRequest
// {
//     BucketName = "keomaawscourse",
//     Key = "files/movies.csv",
//     ContentType = "text/csv",
//     InputStream = inputStream
// };
//
// await s3Client.PutObjectAsync(putObjectRequest);

var getObjectRequest = new GetObjectRequest
{
    BucketName = "keomaawscourse",
    Key = "files/movies.csv"
};

var response = await s3Client.GetObjectAsync(getObjectRequest);

using var memoryStream = new MemoryStream();
response.ResponseStream.CopyTo(memoryStream);

var text = Encoding.Default.GetString(memoryStream.ToArray());

Console.WriteLine(text);