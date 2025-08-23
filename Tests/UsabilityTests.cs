using Xunit;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit.Abstractions;

public class UsabilityTests : IDisposable
{
    private IWebDriver _driver;
    private const string BaseUrl = "http://192.168.1.237:5000";
    private readonly int PageLoadThresholdMs = 3000; // 3 שניות במילוי שניות
    private readonly int NumberOfTests = 100;
    private readonly ITestOutputHelper _output; // לשם הדפסת פלט הבדיקה

    public UsabilityTests(ITestOutputHelper output)
    {
        _output = output;
        _driver = new ChromeDriver();
        _driver.Manage().Window.Maximize();
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