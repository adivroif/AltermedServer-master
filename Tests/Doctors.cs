using AltermedManager.Models.Dtos;
using AltermedManager.Models.Entities;
using RestSharp;
using System.Net;
using System.Text.Json;
using Xunit;
using Xunit.Abstractions;

namespace UnitTestProject
{
    public class Doctors
    {
        private readonly ITestOutputHelper _output;
        private const string BaseUrl = "http://10.0.0.25:5000";
        private string Endpoint = "api/Doctors";

        public Doctors(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task getAllDoctors()
        {
            _output.WriteLine("here");
            var getAllDoctorsRequest = new RestRequest(Endpoint, Method.Get);
            var client = new RestClient(BaseUrl);
            var allDoctorsResponse = await client.ExecuteAsync(getAllDoctorsRequest);

            _output.WriteLine($"Status Code: {allDoctorsResponse.StatusCode}");
            _output.WriteLine($"Content: {allDoctorsResponse.Content}");

            // שלב 1: ודא שהבקשה הצליחה
            Assert.True(allDoctorsResponse.IsSuccessful);
            Assert.False(!allDoctorsResponse.IsSuccessful);
        }

        [Fact]
        public async Task getDoctorByDoctorId()
        {
            String id = "e8cdb665-f4fe-49e0-953c-b1aac2d5d94e";
            Endpoint = "api/Doctors/{" + id + "}";
            _output.WriteLine("here");
            var getDoctorRequest = new RestRequest(Endpoint, Method.Get);
            var client = new RestClient(BaseUrl);
            var getDoctorResponse = await client.ExecuteAsync(getDoctorRequest);
            getDoctorRequest.AddHeader("Content-Type", "application/json"); // Add this line
            getDoctorRequest.AddJsonBody(id);
            _output.WriteLine($"Status Code: {getDoctorResponse.StatusCode}");
            _output.WriteLine($"Content: {getDoctorResponse.Content}");

            // שלב 1: ודא שהבקשה הצליחה
            Assert.True(getDoctorResponse.IsSuccessful);
            Assert.False(!getDoctorResponse.IsSuccessful);
        }

        [Fact]
        public async Task getDoctorByDoctorName()
        {
            String Name = "דוד כהן";
            Endpoint = "api/Doctors/" + Name;
            _output.WriteLine("here");
            var getDoctorRequest = new RestRequest(Endpoint, Method.Get);
            var client = new RestClient(BaseUrl);
            var getDoctorResponse = await client.ExecuteAsync(getDoctorRequest);
            getDoctorRequest.AddHeader("Content-Type", "application/json"); // Add this line
            getDoctorRequest.AddJsonBody(Name);
            _output.WriteLine($"Status Code: {getDoctorResponse.StatusCode}");
            _output.WriteLine($"Content: {getDoctorResponse.Content}");

            // שלב 1: ודא שהבקשה הצליחה
            Assert.True(getDoctorResponse.IsSuccessful);
            Assert.False(!getDoctorResponse.IsSuccessful);

        }

        [Fact]
        public async Task postDoctor()
        {
            NewDoctorDto doctor = new()
            {
                doctorName = "אדיבב",
                doctorSurname = "רויף",
                doctorLicense = "1111",
                specList = [],
                Email = "adiv@gmail.com",
                Phone = "0544444444",
                scheduleId = Guid.Empty,
                placesWorking = []
            };

            var postDoctorRequest = new RestRequest(Endpoint, Method.Post);
            var client = new RestClient(BaseUrl);
            postDoctorRequest.AddHeader("Content-Type", "application/json"); // Add this line
            postDoctorRequest.AddJsonBody(doctor); // This might not be sending the right format

            var postDoctorResponse = await client.ExecuteAsync(postDoctorRequest);

            _output.WriteLine($"Status Code: {postDoctorResponse.StatusCode}");
            _output.WriteLine($"Content: {postDoctorResponse.Content}");

            // שלב 1: ודא שהבקשה הצליחה
            Assert.True(postDoctorResponse.IsSuccessful);
            Assert.False(!postDoctorResponse.IsSuccessful);
        }

        [Fact]
        public async Task deleteDoctor()
        {
            var client = new RestClient(BaseUrl);
            // יצירת אובייקט של יוזר חדש
            NewDoctorDto doctor = new()
            {
                doctorName = "אדיבב",
                doctorSurname = "רויף",
                doctorLicense = "1111",
                specList = [],
                Email = "adiv@gmail.com",
                Phone = "0544444444",
                scheduleId = Guid.Empty,
                placesWorking = []
            };

            // הגדרת בקשת POST ליצירת יוזר
            var createDoctorRequest = new RestRequest(Endpoint, Method.Post);
            createDoctorRequest.AddJsonBody(doctor);

            // שליחת הבקשה
            var createDoctorResponse = await client.ExecuteAsync(createDoctorRequest);
            _output.WriteLine($"Status Code: {createDoctorResponse.StatusCode}");
            _output.WriteLine($"Content: {createDoctorResponse.Content}");
            Doctor doctorCreated = JsonSerializer.Deserialize<Doctor>(
            createDoctorResponse.Content,
    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
   );

            Endpoint = "api/Doctors/{" + doctorCreated.DoctorId + "}";
            var deleteDoctorRequest = new RestRequest(Endpoint, Method.Delete);
            client = new RestClient(BaseUrl);
            var deleteDoctorResponse = await client.ExecuteAsync(deleteDoctorRequest);
            deleteDoctorRequest.AddHeader("Content-Type", "application/json"); // Add this line
            _output.WriteLine($"Status Code: {deleteDoctorResponse.StatusCode}");
            _output.WriteLine($"Content: {deleteDoctorResponse.Content}");

            // שלב 1: ודא שהבקשה הצליחה
            Assert.True(deleteDoctorResponse.IsSuccessful);
            Assert.False(!deleteDoctorResponse.IsSuccessful);

        }
    }
}