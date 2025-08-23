using Xunit;
using RestSharp;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Xunit.Abstractions;

public class ReliabilityTests
{
    private const string BaseUrl = "http://192.168.1.237:5000";
    private readonly int TestDurationSeconds = 60; // משך הבדיקה בדקות (כאן 60 שניות לדוגמה קצרה)
    private readonly int CheckIntervalMs = 1000; // כל כמה זמן לבדוק (1 שניה)
    private readonly double ExpectedAvailabilityPercentage = 99.0;
    private readonly ITestOutputHelper _output;
    private const string Endpoint = "api/Users";


    public ReliabilityTests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public async Task SystemAvailability_ShouldBeAtLeast99Percent()
    {
        var client = new RestClient(BaseUrl);
        var request = new RestRequest(Endpoint, Method.Get); // נקודת קצה לבדיקת סטטוס המערכת
        int successfulResponses = 0;
        int totalAttempts = 0;

        _output.WriteLine($"Starting availability test for {TestDurationSeconds} seconds...");

        Stopwatch testTimer = Stopwatch.StartNew();
        while (testTimer.Elapsed.TotalSeconds <= TestDurationSeconds)
        {
            totalAttempts++;
            try
            {
                var response = await client.ExecuteAsync(request);
                if (response.IsSuccessful && (int)response.StatusCode >= 200 && (int)response.StatusCode < 300)
                {
                    successfulResponses++;
                }
                else
                {
                    _output.WriteLine($"❌ Unsuccessful response at {DateTime.Now}: Status {response.StatusCode}, Error: {response.ErrorMessage}");
                }
            }
            catch (Exception ex)
            {
                _output.WriteLine($"❌ Exception during availability check at {DateTime.Now}: {ex.Message}");
            }

            await Task.Delay(CheckIntervalMs); // המתן לפני הבדיקה הבאה
        }
        testTimer.Stop();

        if (totalAttempts == 0)
        {
            Assert.False(true, "No availability checks were performed.");
            return;
        }

        double actualAvailability = (successfulResponses / (double)totalAttempts) * 100;

        _output.WriteLine($"Total checks: {totalAttempts}");
        _output.WriteLine($"Successful responses: {successfulResponses}");
        _output.WriteLine($"Actual Availability: {actualAvailability:F2}%");

        Assert.True(actualAvailability >= ExpectedAvailabilityPercentage,
            $"System availability ({actualAvailability:F2}%) is below the expected {ExpectedAvailabilityPercentage}%.");
    }


}