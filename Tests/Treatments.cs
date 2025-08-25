using AltermedManager.Models.Dtos;
using AltermedManager.Models.Entities;
using AltermedManager.Models.Enums;
using FluentAssertions;
using RestSharp;
using System.Net;
using System.Text.Json;
using Xunit;
using Xunit.Abstractions;

namespace UnitTestProject
{
    public class Treatments
    {
        private readonly ITestOutputHelper _output;
        private const string BaseUrl = "http://192.168.1.237:5000";
        private string Endpoint = "api/Treatment";

        public Treatments(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task getAllTreatments()
        {
            var getAllTreatmentsRequest = new RestRequest(Endpoint, Method.Get);
            var client = new RestClient(BaseUrl);
            var allTreatmentsResponse = await client.ExecuteAsync(getAllTreatmentsRequest);

            _output.WriteLine($"Status Code: {allTreatmentsResponse.StatusCode}");
            _output.WriteLine($"Content: {allTreatmentsResponse.Content}");

            // שלב 1: ודא שהבקשה הצליחה
            Assert.True(allTreatmentsResponse.IsSuccessful);
            Assert.False(!allTreatmentsResponse.IsSuccessful);
        }

        [Fact]
        public async Task getAndUpdateTreatmentByTreatmentId()
        {
            int id = 20;
            Endpoint = "api/Treatment/" + id.ToString();
            var getTreatmentRequest = new RestRequest(Endpoint, Method.Get);
            var client = new RestClient(BaseUrl);
            var getTreatmentResponse = await client.ExecuteAsync(getTreatmentRequest);


            Treatment getTreatment = JsonSerializer.Deserialize<Treatment>(
    getTreatmentResponse.Content,
    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
   );
            _output.WriteLine(getTreatment.treatmentId.ToString());

            getTreatment.isAdvanced = true;

            _output.WriteLine($"Status Code: {getTreatmentResponse.StatusCode}");
            _output.WriteLine($"Content: {getTreatmentResponse.Content}");

            Endpoint = "api/Treatment/" + getTreatment.treatmentId.ToString();
            var updateTreatmentRequest = new RestRequest(Endpoint, Method.Put);
            updateTreatmentRequest.AddHeader("Content-Type", "application/json"); // Add this line
            updateTreatmentRequest.AddJsonBody(getTreatment);
            var updateTreatmentResponse = await client.ExecuteAsync(updateTreatmentRequest);

            _output.WriteLine($"Status Code: {updateTreatmentResponse.StatusCode}");
            _output.WriteLine($"Content: {updateTreatmentResponse.Content}");
            // שלב 1: ודא שהבקשה הצליחה
            Assert.True(updateTreatmentResponse.IsSuccessful);
            Assert.False(!updateTreatmentResponse.IsSuccessful);
        }

        [Fact]
        public async Task getTreatmentById()
        {
            int id = 8;
            Endpoint = "api/Treatment/" + id;
            var getTreatmentByIdRequest = new RestRequest(Endpoint, Method.Get);
            var client = new RestClient(BaseUrl);
            getTreatmentByIdRequest.AddHeader("Content-Type", "application/json"); // Add this line
            getTreatmentByIdRequest.AddJsonBody(id.ToString());
            var treatmentByIdResponse = await client.ExecuteAsync(getTreatmentByIdRequest);

            _output.WriteLine($"Status Code: {treatmentByIdResponse.StatusCode}");
            _output.WriteLine($"Content: {treatmentByIdResponse.Content}");

            // שלב 1: ודא שהבקשה הצליחה
            Assert.True(treatmentByIdResponse.IsSuccessful);
            Assert.False(!treatmentByIdResponse.IsSuccessful);
        }

        [Fact]
        public async Task getTreatmentByName()
        {
            string name = "דיקור סיני";
            Endpoint = "api/Treatment/" + name;
            var getTreatmentByNameRequest = new RestRequest(Endpoint, Method.Get);
            var client = new RestClient(BaseUrl);
            getTreatmentByNameRequest.AddHeader("Content-Type", "application/json"); // Add this line
            getTreatmentByNameRequest.AddJsonBody(name);
            var treatmentByNameResponse = await client.ExecuteAsync(getTreatmentByNameRequest);

            _output.WriteLine($"Status Code: {treatmentByNameResponse.StatusCode}");
            _output.WriteLine($"Content: {treatmentByNameResponse.Content}");

            // שלב 1: ודא שהבקשה הצליחה
            Assert.True(treatmentByNameResponse.IsSuccessful);
            Assert.False(!treatmentByNameResponse.IsSuccessful);
        }

        [Fact]
        public async Task postTreatment()
        {
            NewTreatmentDto treatement = new()
            {
                treatmentName = "בדיקה",
                treatmentDescription = "בדיקה",
                treatmentDuration = 10,
                treatmentPrice = 0,
                treatmentGroup = "massage",
                isAdvanced = true,
                score = 0,
                treatmentPlaceId = 42,
                numOfFeedbacks = 0,
                suitCategories = [],
                treatmentId = 100
            };
            var postTreatementRequest = new RestRequest(Endpoint, Method.Post);
            var client = new RestClient(BaseUrl);
            postTreatementRequest.AddHeader("Content-Type", "application/json"); // Add this line
            postTreatementRequest.AddJsonBody(treatement); // This might not be sending the right format

            var postTreatementResponse = await client.ExecuteAsync(postTreatementRequest);


            _output.WriteLine($"Status Code: {postTreatementResponse.StatusCode}");
            _output.WriteLine($"Content: {postTreatementResponse.Content}");
        }

        [Fact]
        public async Task deleteTreatment()
        {
            var client = new RestClient(BaseUrl);

            NewTreatmentDto treatement = new()
            {
                treatmentName = "בדיקה",
                treatmentDescription = "בדיקה",
                treatmentDuration = 10,
                treatmentPrice = 0,
                treatmentGroup = "massage",
                isAdvanced = true,
                score = 0,
                treatmentPlaceId = 42,
                numOfFeedbacks = 0,
                suitCategories = [],
                treatmentId = 100
            };

            // הגדרת בקשת POST ליצירת יוזר
            var createTreatmentRequest = new RestRequest(Endpoint, Method.Post);
            createTreatmentRequest.AddJsonBody(treatement);

            // שליחת הבקשה
            var createTreatmentResponse = await client.ExecuteAsync(createTreatmentRequest);
            _output.WriteLine($"Status Code: {createTreatmentResponse.StatusCode}");
            _output.WriteLine($"Content: {createTreatmentResponse.Content}");
            Treatment treatmentCreated = JsonSerializer.Deserialize<Treatment>(
            createTreatmentResponse.Content,
    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
   );

            Endpoint = "api/Treatment/" + treatmentCreated.treatmentId;
            var deleteTreatmentRequest = new RestRequest(Endpoint, Method.Delete);
            client = new RestClient(BaseUrl);
            var deleteTreatmentResponse = await client.ExecuteAsync(deleteTreatmentRequest);
            deleteTreatmentRequest.AddHeader("Content-Type", "application/json"); // Add this line
            _output.WriteLine($"Status Code: {deleteTreatmentResponse.StatusCode}");
            _output.WriteLine($"Content: {deleteTreatmentResponse.Content}");

            // שלב 1: ודא שהבקשה הצליחה
            Assert.True(deleteTreatmentResponse.IsSuccessful);
            Assert.False(!deleteTreatmentResponse.IsSuccessful);

        }
    }
}