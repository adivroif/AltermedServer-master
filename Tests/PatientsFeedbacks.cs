using AltermedManager.Models.Dtos;
using AltermedManager.Models.Entities;
using RestSharp;
using System.Net;
using System.Text.Json;
using Xunit;
using Xunit.Abstractions;

namespace UnitTestProject
{
    public class PatientsFeedbacks
    {
        private readonly ITestOutputHelper _output;
        private const string BaseUrl = "http://192.168.1.237:5000";
        private string Endpoint = "api/PatientsFeedbacks";

        public PatientsFeedbacks(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task getAllPatientsFeedbacks()
        {
            var getAllPatientsFeedbacksRequest = new RestRequest(Endpoint, Method.Get);
            var client = new RestClient(BaseUrl);
            var allPatientsFeedbacksResponse = await client.ExecuteAsync(getAllPatientsFeedbacksRequest);

            _output.WriteLine($"Status Code: {allPatientsFeedbacksResponse.StatusCode}");
            _output.WriteLine($"Content: {allPatientsFeedbacksResponse.Content}");

            // שלב 1: ודא שהבקשה הצליחה
            Assert.True(allPatientsFeedbacksResponse.IsSuccessful);
            Assert.False(!allPatientsFeedbacksResponse.IsSuccessful);
        }

        [Fact]
        public async Task getAllPatientsFeedbacksByPatientId()
        {
            string Id = "10f06324-612b-4d0d-aa5b-76f2be12bdb0";
            Endpoint = "api/PatientsFeedbacks/patientId" + Id;

            var getAllPatientsFeedbacksByPatientIdRequest = new RestRequest(Endpoint, Method.Get);
            var client = new RestClient(BaseUrl);
            getAllPatientsFeedbacksByPatientIdRequest.AddHeader("Content-Type", "application/json"); // Add this line
            getAllPatientsFeedbacksByPatientIdRequest.AddJsonBody(Id);
            var allPatientsFeedbacksByPatientIdResponse = await client.ExecuteAsync(getAllPatientsFeedbacksByPatientIdRequest);

            _output.WriteLine($"Status Code: {allPatientsFeedbacksByPatientIdResponse.StatusCode}");
            _output.WriteLine($"Content: {allPatientsFeedbacksByPatientIdResponse.Content}");

            // שלב 1: ודא שהבקשה הצליחה
            Assert.True(allPatientsFeedbacksByPatientIdResponse.IsSuccessful);
            Assert.False(!allPatientsFeedbacksByPatientIdResponse.IsSuccessful);
        }

        [Fact]
        public async Task getAndUpdatePatientFeedbackByPatientFeedbacksId()
        {
            String id = "fe7036c2-472d-45d8-b3a1-db2f336a9691";
            Endpoint = "api/PatientsFeedbacks/" + id.ToString();
            var getPatientFeedbacksRequest = new RestRequest(Endpoint, Method.Get);
            var client = new RestClient(BaseUrl);
            var getPatientFeedbacksResponse = await client.ExecuteAsync(getPatientFeedbacksRequest);


            PatientFeedback getPatientFeedbacks = JsonSerializer.Deserialize<PatientFeedback>(
    getPatientFeedbacksResponse.Content,
    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
   );
            getPatientFeedbacks.bodyPart = "ראש";

            _output.WriteLine($"Status Code: {getPatientFeedbacksResponse.StatusCode}");
            _output.WriteLine($"Content: {getPatientFeedbacksResponse.Content}");

            Endpoint = "api/PatientsFeedbacks/" + getPatientFeedbacks.feedbackId;
            var updatePatientFeedbacksRequest = new RestRequest(Endpoint, Method.Put);
            updatePatientFeedbacksRequest.AddHeader("Content-Type", "application/json"); // Add this line
            updatePatientFeedbacksRequest.AddJsonBody(getPatientFeedbacks);
            var updatePatientFeedbacksResponse = await client.ExecuteAsync(updatePatientFeedbacksRequest);

            _output.WriteLine($"Status Code: {updatePatientFeedbacksResponse.StatusCode}");
            _output.WriteLine($"Content: {updatePatientFeedbacksResponse.Content}");
            // שלב 1: ודא שהבקשה הצליחה
            Assert.True(updatePatientFeedbacksResponse.IsSuccessful);
            Assert.False(!updatePatientFeedbacksResponse.IsSuccessful);
        }

        [Fact]
        public async Task getPatientFeedbacksByAppointmentId()
        {
            string Id = "10608346-4c0c-41cb-9cd8-9be923e2620f";
            Endpoint = "api/PatientsFeedbacks/appointmentId/" + Id;

            var getPatientFeedbacksByAppointmentIdRequest = new RestRequest(Endpoint, Method.Get);
            var client = new RestClient(BaseUrl);
            getPatientFeedbacksByAppointmentIdRequest.AddHeader("Content-Type", "application/json"); // Add this line
            getPatientFeedbacksByAppointmentIdRequest.AddJsonBody(Id);
            var getPatientFeedbacksByAppointmentIdResponse = await client.ExecuteAsync(getPatientFeedbacksByAppointmentIdRequest);

            _output.WriteLine($"Status Code: {getPatientFeedbacksByAppointmentIdResponse.StatusCode}");
            _output.WriteLine($"Content: {getPatientFeedbacksByAppointmentIdResponse.Content}");

            // שלב 1: ודא שהבקשה הצליחה
            Assert.True(getPatientFeedbacksByAppointmentIdResponse.IsSuccessful);
            Assert.False(!getPatientFeedbacksByAppointmentIdResponse.IsSuccessful);
        }

        [Fact]
        public async Task getPatientFeedbacksById()
        {
            string Id = "495b4f7e-e8a8-461f-8f34-b96fd4cd89c5";
            Endpoint = "api/PatientsFeedbacks/" + Id;

            var getPatientFeedbacksByIdRequest = new RestRequest(Endpoint, Method.Get);
            var client = new RestClient(BaseUrl);
            getPatientFeedbacksByIdRequest.AddHeader("Content-Type", "application/json"); // Add this line
            getPatientFeedbacksByIdRequest.AddJsonBody(Id);
            var getPatientFeedbacksByIdResponse = await client.ExecuteAsync(getPatientFeedbacksByIdRequest);

            _output.WriteLine($"Status Code: {getPatientFeedbacksByIdResponse.StatusCode}");
            _output.WriteLine($"Content: {getPatientFeedbacksByIdResponse.Content}");

            // שלב 1: ודא שהבקשה הצליחה
            Assert.True(getPatientFeedbacksByIdResponse.IsSuccessful);
            Assert.False(!getPatientFeedbacksByIdResponse.IsSuccessful);
        }

        [Fact]
        public async Task postPatientFeedback()
        {
            NewPatientFeedbackDto patientFeedback = new()
            {
                patientId = new Guid("10f06324-612b-4d0d-aa5b-76f2be12bdb0"),
                appointmentId = new Guid("10608346-4c0c-41cb-9cd8-9be923e2620f"),
                overallStatus = 3,
                newSymptoms = "",
                comments = "Good One!",
                createdOn = DateTime.UtcNow,
                bodyPart = "רגל שמאל"
            };

            var postPatientFeedbackRequest = new RestRequest(Endpoint, Method.Post);
            var client = new RestClient(BaseUrl);
            postPatientFeedbackRequest.AddHeader("Content-Type", "application/json"); // Add this line
            postPatientFeedbackRequest.AddJsonBody(patientFeedback); // This might not be sending the right format

            var postPatientFeedbackResponse = await client.ExecuteAsync(postPatientFeedbackRequest);

            _output.WriteLine($"Status Code: {postPatientFeedbackResponse.StatusCode}");
            _output.WriteLine($"Content: {postPatientFeedbackResponse.Content}");

            // שלב 1: ודא שהבקשה הצליחה
            Assert.True(postPatientFeedbackResponse.IsSuccessful);
            Assert.False(!postPatientFeedbackResponse.IsSuccessful);
        }

        [Fact]
        public async Task deletePatientFeedback()
        {
            var client = new RestClient(BaseUrl);

            NewPatientFeedbackDto patientFeedback = new()
            {
                patientId = new Guid("10f06324-612b-4d0d-aa5b-76f2be12bdb0"),
                appointmentId = new Guid("10608346-4c0c-41cb-9cd8-9be923e2620f"),
                overallStatus = 3,
                newSymptoms = "",
                comments = "Good One!",
                createdOn = DateTime.UtcNow,
                bodyPart = "רגל שמאל"
            };

            // הגדרת בקשת POST ליצירת יוזר
            var createPatientFeedbackRequest = new RestRequest(Endpoint, Method.Post);
            createPatientFeedbackRequest.AddJsonBody(patientFeedback);

            // שליחת הבקשה
            var createPatientFeedbackResponse = await client.ExecuteAsync(createPatientFeedbackRequest);
            _output.WriteLine($"Status Code: {createPatientFeedbackResponse.StatusCode}");
            _output.WriteLine($"Content: {createPatientFeedbackResponse.Content}");
            PatientFeedback patientFeedbackCreated = JsonSerializer.Deserialize<PatientFeedback>(
            createPatientFeedbackResponse.Content,
    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
   );

            Endpoint = "api/PatientsFeedbacks/{" + patientFeedbackCreated.feedbackId + "}";
            var deletePatientFeedbackRequest = new RestRequest(Endpoint, Method.Delete);
            client = new RestClient(BaseUrl);
            var deletePatientFeedbackResponse = await client.ExecuteAsync(deletePatientFeedbackRequest);
            deletePatientFeedbackRequest.AddHeader("Content-Type", "application/json"); // Add this line
            _output.WriteLine($"Status Code: {deletePatientFeedbackResponse.StatusCode}");
            _output.WriteLine($"Content: {deletePatientFeedbackResponse.Content}");

            // שלב 1: ודא שהבקשה הצליחה
            Assert.True(deletePatientFeedbackResponse.IsSuccessful);
            Assert.False(!deletePatientFeedbackResponse.IsSuccessful);

        }
    }
}