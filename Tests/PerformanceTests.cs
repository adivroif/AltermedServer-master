using Xunit;
using RestSharp;
using System.Diagnostics;
using Xunit.Abstractions;
using static Google.Apis.Requests.BatchRequest;

namespace UnitTestProject
    {
    public class PerformanceTests
        {

        private readonly ITestOutputHelper _output;
        private const string BaseUrl = "http://192.168.1.237:5000"; 
        private const string Endpoint = "api/Users";

        public PerformanceTests(ITestOutputHelper output)
            {
            _output = output;
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
        }
    }