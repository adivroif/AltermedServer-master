using AltermedManager.Models.Dtos;
using AltermedManager.Models.Entities;
using RestSharp;
using System.Net;
using System.Text.Json;
using Xunit;
using Xunit.Abstractions;

namespace UnitTestProject
{
    public class Patients
    {
        private readonly ITestOutputHelper _output;
        private const string BaseUrl = "http://10.0.0.25:5000";
        private string Endpoint = "api/Patients";

        public Patients(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task getAllPatients()
        {
            var getAllPatientsRequest = new RestRequest(Endpoint, Method.Get);
            var client = new RestClient(BaseUrl);
            var allPatientsResponse = await client.ExecuteAsync(getAllPatientsRequest);

            _output.WriteLine($"Status Code: {allPatientsResponse.StatusCode}");
            _output.WriteLine($"Content: {allPatientsResponse.Content}");

            // שלב 1: ודא שהבקשה הצליחה
            Assert.True(allPatientsResponse.IsSuccessful);
            Assert.False(!allPatientsResponse.IsSuccessful);
        }

        [Fact]
        public async Task getPatientByPatientId()
        {
            String Id = "28621ddc-7343-4656-a944-afcac0b04b89";
            Endpoint = "api/Patients/" + Id;
            var getPatientRequest = new RestRequest(Endpoint, Method.Get);
            var client = new RestClient(BaseUrl);
            var getPatientResponse = await client.ExecuteAsync(getPatientRequest);
            getPatientRequest.AddHeader("Content-Type", "application/json"); // Add this line
            getPatientRequest.AddJsonBody(Id);
            _output.WriteLine($"Status Code: {getPatientResponse.StatusCode}");
            _output.WriteLine($"Content: {getPatientResponse.Content}");

            // שלב 1: ודא שהבקשה הצליחה
            Assert.True(getPatientResponse.IsSuccessful);
            Assert.False(!getPatientResponse.IsSuccessful);
        }


        [Fact]
        public async Task getPatientByPatientName()
        {
            String Name = "איתי בר";
            Endpoint = "api/Patients/" + Name;
            var getPatientRequest = new RestRequest(Endpoint, Method.Get);
            var client = new RestClient(BaseUrl);
            var getPatientResponse = await client.ExecuteAsync(getPatientRequest);
            getPatientRequest.AddHeader("Content-Type", "application/json"); // Add this line
            getPatientRequest.AddJsonBody(Name);
            _output.WriteLine($"Status Code: {getPatientResponse.StatusCode}");
            _output.WriteLine($"Content: {getPatientResponse.Content}");

            // שלב 1: ודא שהבקשה הצליחה
            Assert.True(getPatientResponse.IsSuccessful);
            Assert.False(!getPatientResponse.IsSuccessful);
        }

        [Fact]
        public async Task postPatient()
        {
            Address address = new Address();
            address.latitude = 31.7917265;
            address.longitude = 34.7027684;
            address.houseNumber = 10;
            address.street = "רחל המשוררת";
            address.city = "גן יבנה";
            address.postalCode = "0";
            NewPatientDto patient = new()
            {

                patientID = "206666666",
                patientName = "אדיבב",
                patientSurname = "רויףף",
                patientAddress = address,
                patientEmail = "a@gmail.com",
                patientPhone = "0",
                healthProvider = "Maccabi",
                gender = 'M',
                dateOfBirth = DateTime.UtcNow,
            };
            var postPatientRequest = new RestRequest(Endpoint, Method.Post);
            var client = new RestClient(BaseUrl);
            postPatientRequest.AddHeader("Content-Type", "application/json"); // Add this line
            postPatientRequest.AddJsonBody(patient); // This might not be sending the right format

            var postPatientResponse = await client.ExecuteAsync(postPatientRequest);


            _output.WriteLine($"Status Code: {postPatientResponse.StatusCode}");
            _output.WriteLine($"Content: {postPatientResponse.Content}");
        }

        [Fact]
        public async Task deletePatient()
        {
            var client = new RestClient(BaseUrl);

            Address address = new Address();
            address.latitude = 31.7917265;
            address.longitude = 34.7027684;
            address.houseNumber = 10;
            address.street = "רחל המשוררת";
            address.city = "גן יבנה";
            address.postalCode = "0";
            NewPatientDto patient = new()
            {

                patientID = "206666666",
                patientName = "אדיבב",
                patientSurname = "רויףףף",
                patientAddress = address,
                patientEmail = "a@gmail.com",
                patientPhone = "0",
                healthProvider = "Maccabi",
                gender = 'M',
                dateOfBirth = DateTime.UtcNow,
            };

            // הגדרת בקשת POST ליצירת יוזר
            var createPatientRequest = new RestRequest(Endpoint, Method.Post);
            createPatientRequest.AddJsonBody(patient);

            // שליחת הבקשה
            var createPatientResponse = await client.ExecuteAsync(createPatientRequest);
            _output.WriteLine($"Status Code: {createPatientResponse.StatusCode}");
            _output.WriteLine($"Content: {createPatientResponse.Content}");
            Patient patientCreated = JsonSerializer.Deserialize<Patient>(
            createPatientResponse.Content,
    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
   );

            Endpoint = "api/Patients/{" + patientCreated.id + "}";
            var deletePatientRequest = new RestRequest(Endpoint, Method.Delete);
            client = new RestClient(BaseUrl);
            var deletePatientResponse = await client.ExecuteAsync(deletePatientRequest);
            deletePatientRequest.AddHeader("Content-Type", "application/json"); // Add this line
            _output.WriteLine($"Status Code: {deletePatientResponse.StatusCode}");
            _output.WriteLine($"Content: {deletePatientResponse.Content}");

            // שלב 1: ודא שהבקשה הצליחה
            Assert.True(deletePatientResponse.IsSuccessful);
            Assert.False(!deletePatientResponse.IsSuccessful);

        }
    }
}