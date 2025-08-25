using Xunit;
using RestSharp;
using System.Threading.Tasks;
using System.Text.Json;
using System;
using System.Collections.Generic;
using Xunit.Abstractions;
using AltermedManager.Models.Entities;

public class TreatmentPostTest
{
    private const string BaseUrl = "http://192.168.1.237:5000";
    private readonly ITestOutputHelper _output;

    public TreatmentPostTest(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public async Task FunctionalityTest()
    {
        // 1. Prepare the data for the new treatment
        var newTreatment = new Treatment
        {
            treatmentId = 20,
            treatmentName = "טיפול חדש לאוטומציה",
            treatmentDescription = "תיאור טיפול שנוצר על ידי בדיקה אוטומטית.",
            treatmentPrice = 150,
            treatmentDuration = 45,
            suitCategories = new List<string> { "adults", "youth" },
            treatmentGroup = "massage",
            isAdvanced = true,
            treatmentPlaceId = 42
        };

        var client = new RestClient(BaseUrl);
        var request = new RestRequest("api/Treatment", Method.Post);

        // 2. Add the JSON body to the request
        request.AddJsonBody(newTreatment);

        _output.WriteLine("Sending POST request to create a new treatment...");

        // 3. Execute the request
        var response = await client.ExecuteAsync(request);

        // 4. Verify the status code (Functional Check)
        Assert.True(response.IsSuccessful, $"Failed to create a new treatment. Status code: {response.StatusCode}");
        _output.WriteLine($"Successfully received response. Status code: {response.StatusCode}");

        // 5. Verify the returned data (Functional Check)
        var createdTreatment = JsonSerializer.Deserialize<Treatment>(response.Content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        _output.WriteLine($"Successfully received response. Status code: {response.Content}");

        Assert.NotNull(createdTreatment);
        Assert.True(createdTreatment.treatmentId > 0, "New treatment should have a valid ID.");
        Assert.Equal(newTreatment.treatmentName, createdTreatment.treatmentName);
        Assert.Equal(newTreatment.treatmentPrice, createdTreatment.treatmentPrice);
        Assert.Equal(newTreatment.treatmentDuration, createdTreatment.treatmentDuration);
        Assert.Equal(newTreatment.treatmentGroup, createdTreatment.treatmentGroup);
        Assert.Equal(newTreatment.isAdvanced, createdTreatment.isAdvanced);
        Assert.Equal(newTreatment.treatmentPlaceId, createdTreatment.treatmentPlaceId);



    }
}