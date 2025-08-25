using AltermedManager.Models.Dtos;
using AltermedManager.Models.Entities;
using FluentAssertions;
using RestSharp;
using System.Net;
using System.Text.Json;
using Xunit;
using Xunit.Abstractions;

namespace UnitTestProject
{
    public class Recommendations
    {
        private readonly ITestOutputHelper _output;
        private const string BaseUrl = "http://192.168.1.237:5000";
        private string Endpoint = "api/Recommendations";

        public Recommendations(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task getAllRecommendationsById()
        {
            int id = 89;
            Endpoint = "api/Recommendations/byRecommendationId/" + id;
            var getAllRecommendationsByIdRequest = new RestRequest(Endpoint, Method.Get);
            var client = new RestClient(BaseUrl);
            getAllRecommendationsByIdRequest.AddHeader("Content-Type", "application/json"); // Add this line
            getAllRecommendationsByIdRequest.AddJsonBody(id.ToString());
            var allRecommendationsByIdResponse = await client.ExecuteAsync(getAllRecommendationsByIdRequest);

            _output.WriteLine($"Status Code: {allRecommendationsByIdResponse.StatusCode}");
            _output.WriteLine($"Content: {allRecommendationsByIdResponse.Content}");

            // שלב 1: ודא שהבקשה הצליחה
            Assert.True(allRecommendationsByIdResponse.IsSuccessful);
            Assert.False(!allRecommendationsByIdResponse.IsSuccessful);
        }

        [Fact]
        public async Task getAllRecommendationsByPatientId()
        {
            String id = "1be276f8-1e2d-42be-b634-e40e59df2ac6";
            Endpoint = "api/Recommendations/byPatient/" + id;
            var getAllRecommendationsByPatientIdRequest = new RestRequest(Endpoint, Method.Get);
            var client = new RestClient(BaseUrl);
            getAllRecommendationsByPatientIdRequest.AddHeader("Content-Type", "application/json"); // Add this line
            getAllRecommendationsByPatientIdRequest.AddJsonBody(id);
            var allRecommendationsByPatientIdResponse = await client.ExecuteAsync(getAllRecommendationsByPatientIdRequest);

            _output.WriteLine($"Status Code: {allRecommendationsByPatientIdResponse.StatusCode}");
            _output.WriteLine($"Content: {allRecommendationsByPatientIdResponse.Content}");

            // שלב 1: ודא שהבקשה הצליחה
            Assert.True(allRecommendationsByPatientIdResponse.IsSuccessful);
            Assert.False(!allRecommendationsByPatientIdResponse.IsSuccessful);
        }

        [Fact]
        public async Task getAllRecommendationsNotChosenByPatientId()
        {
            String id = "1be276f8-1e2d-42be-b634-e40e59df2ac6";
            Endpoint = "api/Recommendations/byPatient/ischosen/" + id;
            var getAllRecommendationsNotChosenByPatientIdRequest = new RestRequest(Endpoint, Method.Get);
            var client = new RestClient(BaseUrl);
            getAllRecommendationsNotChosenByPatientIdRequest.AddHeader("Content-Type", "application/json"); // Add this line
            getAllRecommendationsNotChosenByPatientIdRequest.AddJsonBody(id);
            var allRecommendationsNotChosenByPatientIdResponse = await client.ExecuteAsync(getAllRecommendationsNotChosenByPatientIdRequest);

            _output.WriteLine($"Status Code: {allRecommendationsNotChosenByPatientIdResponse.StatusCode}");
            _output.WriteLine($"Content: {allRecommendationsNotChosenByPatientIdResponse.Content}");

            // שלב 1: ודא שהבקשה הצליחה
            Assert.True(allRecommendationsNotChosenByPatientIdResponse.IsSuccessful);
            Assert.False(!allRecommendationsNotChosenByPatientIdResponse.IsSuccessful);
        }
        /*
        [Fact]
        public async Task LoopAppointments_AndCheckRecommendations()
        {
            var client = new RestClient("http://192.168.1.237:5000");

            // STEP 1: Get all appointments
            var getAppointments = new RestRequest("api/Appointments", Method.Get);
            var appointmentsResponse = await client.ExecuteAsync(getAppointments);

            appointmentsResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var appointments = JsonSerializer.Deserialize<List<NewAppointmentDto>>(appointmentsResponse.Content!, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            foreach (var appointment in appointments!)
            {
                var id = appointment.appointmentId;

                // STEP 2: Get recommendation for each appointment
                var recRequest = new RestRequest($"api/Recommendations/byAppoint/{id}", Method.Get);
                var recResponse = await client.ExecuteAsync(recRequest);

                _output.WriteLine($"Appointment #{id}: Status {recResponse.StatusCode}");
                _output.WriteLine($"Content: {recResponse.Content}");

                if (recResponse.StatusCode == HttpStatusCode.OK)
                {
                    // Optional: parse recommendation and make assertions
                    var content = recResponse.Content!;
                    if (content.Contains("massage")) _output.WriteLine("💡 Got a massage-related recommendation.");
                }
                else
                {
                    _output.WriteLine($"⚠️ Failed for appointment {id}");
                }
        */
            }


        }