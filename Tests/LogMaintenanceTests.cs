using Xunit;
using RestSharp;
using System.Threading.Tasks;
using Xunit.Abstractions;
using System.Collections.Generic;
using System.Net;
using System.Text.Json; // Added to handle JSON serialization
using System;
using AltermedManager.Models.Entities;
using AltermedManager.Models.Dtos; // Assuming this namespace contains TreatmentDto

public class LogMaintenanceTests
{
    private const string BaseUrl = "http://192.168.1.237:5000";
    private readonly ITestOutputHelper _output;

    public LogMaintenanceTests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public async Task VerifyErrorLoggingCompleteness()
    {
        var client = new RestClient(BaseUrl);
        var invalidTreatments = new List<NewTreatmentDto>
        {
            // Invalid data to cause an error
            new NewTreatmentDto { treatmentName = "", treatmentPrice = -10, treatmentDuration = 30 },
            new NewTreatmentDto { treatmentName = "Test", treatmentPrice = 100, treatmentDuration = -10 }
        };

        var errorEvents = new List<string>();
        foreach (var input in invalidTreatments)
        {
            var request = new RestRequest("api/Treatment", Method.Post);

            // Corrected line: Use AddJsonBody with a typed object
            request.AddJsonBody(input);

            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            var response = await client.ExecuteAsync(request);
            stopwatch.Stop();


            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            errorEvents.Add($"Bad Request for input '{JsonSerializer.Serialize(input)}' at {DateTime.Now}");
            _output.WriteLine(errorEvents.Count.ToString());

        }

        _output.WriteLine("Simulated errors have been sent to the server. Now, analyzing logs...");

        // Theoretical part: This part is for conceptual understanding and requires
        // a separate logging system like Elasticsearch or Splunk to be fully functional.
        var actualLoggedErrors = GetErrorsFromLoggingSystem(errorEvents);

        int documentedErrors = 0;
        foreach (var expectedError in errorEvents)
        {
            if (IsErrorDocumentedPrecisely(expectedError, actualLoggedErrors))
            {
                documentedErrors++;
            }
        }

        double accuracy = ((double)documentedErrors / errorEvents.Count) * 100;

        _output.WriteLine($"Total simulated errors: {errorEvents.Count}");
        _output.WriteLine($"Accurately documented errors: {documentedErrors}");
        _output.WriteLine($"Logging accuracy: {accuracy:F2}%");

        Assert.True(accuracy >= 95.0, $"The logging accuracy ({accuracy:F2}%) is below the required 95%.");
    }

    // Theoretical functions (implementations will vary based on your logging system)
    private List<string> GetErrorsFromLoggingSystem(List<string> expectedErrors)
    {
        return new List<string>();
    }

    private bool IsErrorDocumentedPrecisely(string expectedError, List<string> actualLoggedErrors)
    {
        return false;
    }
}

