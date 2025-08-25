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
    public class PatientsRequest
    {
        private readonly ITestOutputHelper _output;
        private const string BaseUrl = "http://192.168.1.237:5000";
        private string Endpoint = "api/PatientsRequest";

        public PatientsRequest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task getAllPatientsRequest()
        {
            var getAllPatientsRequestsRequest = new RestRequest(Endpoint, Method.Get);
            var client = new RestClient(BaseUrl);
            var allPatientsRequestsResponse = await client.ExecuteAsync(getAllPatientsRequestsRequest);

            _output.WriteLine($"Status Code: {allPatientsRequestsResponse.StatusCode}");
            _output.WriteLine($"Content: {allPatientsRequestsResponse.Content}");

            // שלב 1: ודא שהבקשה הצליחה
            Assert.True(allPatientsRequestsResponse.IsSuccessful);
            Assert.False(!allPatientsRequestsResponse.IsSuccessful);
        }

        [Fact]
        public async Task getAndUpdatePatientRequestByPatientRequestId()
        {
            String id = "39e37d52-c448-41a8-83ae-7d0b18d5ef55";
            Endpoint = "api/PatientsRequest/" + id.ToString();
            var getPatientRequestRequest = new RestRequest(Endpoint, Method.Get);
            var client = new RestClient(BaseUrl);
            var getPatientRequestResponse = await client.ExecuteAsync(getPatientRequestRequest);


            PatientRequest getPatientRequest = JsonSerializer.Deserialize<PatientRequest>(
    getPatientRequestResponse.Content,
    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
   );
            getPatientRequest.answerFromDoctor = "Test";

            _output.WriteLine($"Status Code: {getPatientRequestResponse.StatusCode}");
            _output.WriteLine($"Content: {getPatientRequestResponse.Content}");

            Endpoint = "api/PatientsRequest/" + getPatientRequest.requestId;
            var updatePatientRequestRequest = new RestRequest(Endpoint, Method.Put);
            updatePatientRequestRequest.AddHeader("Content-Type", "application/json"); // Add this line
            updatePatientRequestRequest.AddJsonBody(getPatientRequest);
            var updatePatientRequestResponse = await client.ExecuteAsync(updatePatientRequestRequest);

            _output.WriteLine($"Status Code: {updatePatientRequestResponse.StatusCode}");
            _output.WriteLine($"Content: {updatePatientRequestResponse.Content}");
            // שלב 1: ודא שהבקשה הצליחה
            Assert.True(updatePatientRequestResponse.IsSuccessful);
            Assert.False(!updatePatientRequestResponse.IsSuccessful);
        }

        [Fact]
        public async Task getAllPatientsRequestByDoctorId()
        {
            String id = "e8cdb665-f4fe-49e0-953c-b1aac2d5d94e";
            Endpoint = "api/PatientsRequest/doctor/" + id;
            var getAllPatientsRequestsByDoctorIdRequest = new RestRequest(Endpoint, Method.Get);
            var client = new RestClient(BaseUrl);
            getAllPatientsRequestsByDoctorIdRequest.AddHeader("Content-Type", "application/json"); // Add this line
            getAllPatientsRequestsByDoctorIdRequest.AddJsonBody(id);
            var allPatientsRequestsByDoctorIdResponse = await client.ExecuteAsync(getAllPatientsRequestsByDoctorIdRequest);

            _output.WriteLine($"Status Code: {allPatientsRequestsByDoctorIdResponse.StatusCode}");
            _output.WriteLine($"Content: {allPatientsRequestsByDoctorIdResponse.Content}");

            // שלב 1: ודא שהבקשה הצליחה
            Assert.True(allPatientsRequestsByDoctorIdResponse.IsSuccessful);
            Assert.False(!allPatientsRequestsByDoctorIdResponse.IsSuccessful);
        }

        [Fact]
        public async Task getAllPatientsRequestById()
        {
            String id = "f6f6514e-9e03-43b2-93d3-b69fc0966cff";
            Endpoint = "api/PatientsRequest/" + id;
            var getAllPatientsRequestsByIdRequest = new RestRequest(Endpoint, Method.Get);
            var client = new RestClient(BaseUrl);
            getAllPatientsRequestsByIdRequest.AddHeader("Content-Type", "application/json"); // Add this line
            getAllPatientsRequestsByIdRequest.AddJsonBody(id);
            var allPatientsRequestsByIdResponse = await client.ExecuteAsync(getAllPatientsRequestsByIdRequest);

            _output.WriteLine($"Status Code: {allPatientsRequestsByIdResponse.StatusCode}");
            _output.WriteLine($"Content: {allPatientsRequestsByIdResponse.Content}");

            // שלב 1: ודא שהבקשה הצליחה
            Assert.True(allPatientsRequestsByIdResponse.IsSuccessful);
            Assert.False(!allPatientsRequestsByIdResponse.IsSuccessful);
        }

        [Fact]
        public async Task getAllPatientsRequestByPatientId()
        {
            String id = "1be276f8-1e2d-42be-b634-e40e59df2ac6";
            Endpoint = "api/PatientsRequest/patient/" + id;
            var getAllPatientsRequestsByPatientIdRequest = new RestRequest(Endpoint, Method.Get);
            var client = new RestClient(BaseUrl);
            getAllPatientsRequestsByPatientIdRequest.AddHeader("Content-Type", "application/json"); // Add this line
            getAllPatientsRequestsByPatientIdRequest.AddJsonBody(id);
            var allPatientsRequestsByPatientIdResponse = await client.ExecuteAsync(getAllPatientsRequestsByPatientIdRequest);

            _output.WriteLine($"Status Code: {allPatientsRequestsByPatientIdResponse.StatusCode}");
            _output.WriteLine($"Content: {allPatientsRequestsByPatientIdResponse.Content}");

            // שלב 1: ודא שהבקשה הצליחה
            Assert.True(allPatientsRequestsByPatientIdResponse.IsSuccessful);
            Assert.False(!allPatientsRequestsByPatientIdResponse.IsSuccessful);
        }

        [Fact]
        public async Task postPatientRequest()
        {
            NewPatientRequestDto patientFeedback = new()
            {
                patientId = new Guid("10f06324-612b-4d0d-aa5b-76f2be12bdb0"),
                appointmentId = new Guid("10608346-4c0c-41cb-9cd8-9be923e2620f"),
                description = "טסט",
                createdOn = DateTime.UtcNow,
                requestType = RequestType.Appointment,
                isUrgent = true,
                answerFromDoctor = "טסט",
                doctorId = new Guid("e8cdb665-f4fe-49e0-953c-b1aac2d5d94e")
            };

            var postPatientReqRequest = new RestRequest(Endpoint, Method.Post);
            var client = new RestClient(BaseUrl);
            postPatientReqRequest.AddHeader("Content-Type", "application/json"); // Add this line
            postPatientReqRequest.AddJsonBody(patientFeedback); // This might not be sending the right format

            var postPatientFeedbackRequest = await client.ExecuteAsync(postPatientReqRequest);

            _output.WriteLine($"Status Code: {postPatientFeedbackRequest.StatusCode}");
            _output.WriteLine($"Content: {postPatientFeedbackRequest.Content}");

            // שלב 1: ודא שהבקשה הצליחה
            Assert.True(postPatientFeedbackRequest.IsSuccessful);
            Assert.False(!postPatientFeedbackRequest.IsSuccessful);
        }

        [Fact]
        public async Task deletePatientRequest()
        {
            var client = new RestClient(BaseUrl);

            NewPatientRequestDto patientFeedback = new()
            {
                patientId = new Guid("10f06324-612b-4d0d-aa5b-76f2be12bdb0"),
                appointmentId = new Guid("10608346-4c0c-41cb-9cd8-9be923e2620f"),
                description = "טסט",
                createdOn = DateTime.UtcNow,
                requestType = RequestType.Appointment,
                isUrgent = true,
                answerFromDoctor = "טסט",
                doctorId = new Guid("e8cdb665-f4fe-49e0-953c-b1aac2d5d94e")
            };

            // הגדרת בקשת POST ליצירת יוזר
            var createPatientRequestRequest = new RestRequest(Endpoint, Method.Post);
            createPatientRequestRequest.AddJsonBody(patientFeedback);

            // שליחת הבקשה
            var createPatientRequestResponse = await client.ExecuteAsync(createPatientRequestRequest);
            _output.WriteLine($"Status Code: {createPatientRequestResponse.StatusCode}");
            _output.WriteLine($"Content: {createPatientRequestResponse.Content}");
            PatientRequest patientRequestCreated = JsonSerializer.Deserialize<PatientRequest>(
            createPatientRequestResponse.Content,
    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
   );

            Endpoint = "api/PatientsRequest/{" + patientRequestCreated.requestId + "}";
            var deletePatientRequestRequest = new RestRequest(Endpoint, Method.Delete);
            client = new RestClient(BaseUrl);
            var deletePatientRequestResponse = await client.ExecuteAsync(deletePatientRequestRequest);
            deletePatientRequestRequest.AddHeader("Content-Type", "application/json"); // Add this line
            _output.WriteLine($"Status Code: {deletePatientRequestResponse.StatusCode}");
            _output.WriteLine($"Content: {deletePatientRequestResponse.Content}");

            // שלב 1: ודא שהבקשה הצליחה
            Assert.True(deletePatientRequestResponse.IsSuccessful);
            Assert.False(!deletePatientRequestResponse.IsSuccessful);

        }
    }
}