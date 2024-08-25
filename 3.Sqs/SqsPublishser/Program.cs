using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using SqsPublishser;

var sqsClient = new AmazonSQSClient();

var customerCreated = new CustomerCreated
{
    Id = Guid.NewGuid(),
    FullName = "Keoma Baudin",
    Email = "keoma@keomabaudin.com",
    GitHubUsername = "kafziel4",
    DateOfBirth = new DateTime(1991, 1, 1)
};

var queueUrlResponse = await sqsClient.GetQueueUrlAsync("customers");

var sendMessageRequest = new SendMessageRequest
{
    QueueUrl = queueUrlResponse.QueueUrl,
    MessageBody = JsonSerializer.Serialize(customerCreated),
    MessageAttributes = new Dictionary<string, MessageAttributeValue>
    {
        {
            "MessageType", new MessageAttributeValue
            {
                DataType = "String",
                StringValue = nameof(CustomerCreated)
            }
        }
    }
};

await sqsClient.SendMessageAsync(sendMessageRequest);

Console.WriteLine();