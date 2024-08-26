using System.Text.Json;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;

namespace Movies.Api;

public class DataSeeder
{
    public async Task ImportDataAsync()
    {
        var dynamoDb = new AmazonDynamoDBClient();
        var lines = await File.ReadAllLinesAsync("./movies.csv");
        for (int i = 0; i < lines.Length; i++)
        {
            if (i == 0)
            {
                continue; //Skip header
            }

            var line = lines[i];
            var commaSplit = line.Split(',');

            var title = commaSplit[0];
            var year = int.Parse(commaSplit[1]);
            var ageRestriction = int.Parse(commaSplit[2]);
            var rottenTomatoes = int.Parse(commaSplit[3]);

            var movie1 = new Movie1
            {
                Id = Guid.NewGuid(),
                Title = title,
                AgeRestriction = ageRestriction,
                ReleaseYear = year,
                RottenTomatoesPercentage = rottenTomatoes
            };
            
            var movie2 = new Movie2
            {
                Id = Guid.NewGuid(),
                Title = title,
                AgeRestriction = ageRestriction,
                ReleaseYear = year,
                RottenTomatoesPercentage = rottenTomatoes
            };
            
            var movie1AsJson = JsonSerializer.Serialize(movie1);
            var item1AsDocument = Document.FromJson(movie1AsJson);
            var item1AsAttributes = item1AsDocument.ToAttributeMap();
            
            var movie2AsJson = JsonSerializer.Serialize(movie2);
            var item2AsDocument = Document.FromJson(movie2AsJson);
            var item2AsAttributes = item2AsDocument.ToAttributeMap();
            
            var createItemRequest1 = new PutItemRequest
            {
                TableName = "movies-year-title",
                Item = item1AsAttributes
            };

            var createItemRequest2 = new PutItemRequest
            {
                TableName = "movies-title-rotten",
                Item = item2AsAttributes
            };

            var response1 = await dynamoDb.PutItemAsync(createItemRequest1);
            var response2 = await dynamoDb.PutItemAsync(createItemRequest2);
            await Task.Delay(300);
        }
    }
}
