using Xunit;
using RestSharp;
using System.Threading.Tasks;
using System.Text.Json;
using System;
using System.Collections.Generic;
using Xunit.Abstractions;
using AltermedManager.Models.Entities;
using AltermedManager.Models.Dtos;
using System.Net;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;

public class NonFuncionalTests
{
    private string BaseUrl = "http://192.168.1.237:5000";
    private readonly ITestOutputHelper _output;
    private readonly int TestDurationSeconds = 60; // משך הבדיקה בדקות (כאן 60 שניות לדוגמה קצרה)
    private readonly int CheckIntervalMs = 1000; // כל כמה זמן לבדוק (1 שניה)
    private readonly double ExpectedAvailabilityPercentage = 99.0;
    private const string Endpoint = "api/Users";
    private readonly int PageLoadThresholdMs = 3000; // 3 שניות במילוי שניות
    private readonly int NumberOfTests = 100;
    private IWebDriver _driver;


    public NonFuncionalTests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public async Task FunctionalTest_CreateNewTreatment_ShouldBeSuccessful()
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

    [Fact]
    public async Task MyTestAsync()
    {
        var request = new RestRequest(Endpoint, Method.Get);
        var client = new RestClient(BaseUrl);
        var response = await client.ExecuteAsync(request);
        _output.WriteLine($"Average: ms");
    }

    [Fact]
    public async Task ApiPerformance_ShouldMeet90PercentUnder1_5Seconds()
    {
        var client = new RestClient(BaseUrl);
        var request = new RestRequest(Endpoint, Method.Get);

        int totalRequests = 100;
        int thresholdMs = 1500;
        int successCount = 0;
        List<long> responseTimes = new List<long>();

        for (int i = 0; i < totalRequests; i++)
        {
            var sw = Stopwatch.StartNew();
            var response = await client.ExecuteAsync(request);
            sw.Stop();

            if (response.IsSuccessful)
            {
                long time = sw.ElapsedMilliseconds;
                responseTimes.Add(time);
                if (time <= thresholdMs)
                    successCount++;
            }
            else
            {
                _output.WriteLine($"❌ Request #{i + 1} failed.");
                _output.WriteLine($"    StatusCode: {response.StatusCode}");
                _output.WriteLine($"    Error: {response.ErrorMessage}");
                _output.WriteLine($"    Content: {response.Content}");
            }
        }
        if (responseTimes.Count == 0)
        {
            Assert.False(true, $"All {totalRequests} requests failed — likely due to wrong URL or server not running.");
            return;
        }


        double percentage = (successCount / (double)totalRequests) * 100;
        double average = responseTimes.Average();

        _output.WriteLine($"Average: {average}ms");
        _output.WriteLine($"Fastest: {responseTimes.Min()}ms");
        _output.WriteLine($"Slowest: {responseTimes.Max()}ms");

        Assert.True(percentage >= 90,
            $"Only {percentage}% of requests were under {thresholdMs}ms (average: {average}ms)");
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

    [Fact]
    public async Task PageLoadTimes_ShouldAverageBelow3Seconds()
    {
        List<long> loadTimes = new List<long>();

        for (int i = 0; i < NumberOfTests; i++)
        {
            // שינוי כתובת ה-URL לדף אינטרנט אמיתי
            _driver.Navigate().GoToUrl($"{BaseUrl}/api/Treatment");

            Stopwatch sw = Stopwatch.StartNew();
            try
            {
                var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
                // שינוי מזהה האלמנט למזהה שנמצא בדף אינטרנט אמיתי
                wait.Until(driver => driver.FindElement(By.ClassName("json-formatter-container")).Displayed);
            }
            catch (WebDriverTimeoutException)
            {
                _output.WriteLine($"Warning: Page load timed out for test run {i + 1}.");
            }
            sw.Stop();
            loadTimes.Add(sw.ElapsedMilliseconds);
            _output.WriteLine($"Test run {i + 1}: {sw.ElapsedMilliseconds}ms");
            await Task.Delay(100);
        }

        if (loadTimes.Count == 0)
        {
            Assert.False(true, "No page load times were recorded.");
            return;
        }

        double averageLoadTime = loadTimes.Average();
        long fastestLoadTime = loadTimes.Min();
        long slowestLoadTime = loadTimes.Max();

        _output.WriteLine($"Total tests: {NumberOfTests}");
        _output.WriteLine($"Average page load time: {averageLoadTime}ms");
        _output.WriteLine($"Fastest page load time: {fastestLoadTime}ms");
        _output.WriteLine($"Slowest page load time: {slowestLoadTime}ms");

        Assert.True(averageLoadTime <= PageLoadThresholdMs,
            $"Average page load time ({averageLoadTime}ms) exceeded the threshold of {PageLoadThresholdMs}ms.");
    }

    public void Dispose()
    {
        _driver.Quit();
    }
}


