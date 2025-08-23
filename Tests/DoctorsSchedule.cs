using AltermedManager.Models.Dtos;
using AltermedManager.Models.Entities;
using RestSharp;
using System.Net;
using System.Text.Json;
using Xunit;
using Xunit.Abstractions;

namespace UnitTestProject
{
    public class DoctorsSchedule
    {
        private readonly ITestOutputHelper _output;
        private const string BaseUrl = "http://10.0.0.25:5000";
        private string Endpoint = "api/DoctorsSchedule";

        public DoctorsSchedule(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task getAllDoctorsSchedule()
        {
            _output.WriteLine("here");
            var getAllDoctorsScheduleRequest = new RestRequest(Endpoint, Method.Get);
            var client = new RestClient(BaseUrl);
            var allDoctorsScheduleResponse = await client.ExecuteAsync(getAllDoctorsScheduleRequest);

            _output.WriteLine($"Status Code: {allDoctorsScheduleResponse.StatusCode}");
            _output.WriteLine($"Content: {allDoctorsScheduleResponse.Content}");

            // שלב 1: ודא שהבקשה הצליחה
            Assert.True(allDoctorsScheduleResponse.IsSuccessful);
            Assert.False(!allDoctorsScheduleResponse.IsSuccessful);
        }


        [Fact]
        public async Task getDoctorScheduleByDoctorScheduleId()
        {
            String id = "8e5032ac-04c2-491f-bed2-d7ddb4460c94";
            Endpoint = "api/DoctorsSchedule/" + id;
            _output.WriteLine("here");
            var getDoctorScheduleRequest = new RestRequest(Endpoint, Method.Get);
            var client = new RestClient(BaseUrl);
            var getDoctorScheduleResponse = await client.ExecuteAsync(getDoctorScheduleRequest);
            getDoctorScheduleRequest.AddHeader("Content-Type", "application/json"); // Add this line
            getDoctorScheduleRequest.AddJsonBody(id);
            _output.WriteLine($"Status Code: {getDoctorScheduleResponse.StatusCode}");
            _output.WriteLine($"Content: {getDoctorScheduleResponse.Content}");

            // שלב 1: ודא שהבקשה הצליחה
            Assert.True(getDoctorScheduleResponse.IsSuccessful);
            Assert.False(!getDoctorScheduleResponse.IsSuccessful);
        }

        [Fact]
        public async Task postDoctorSchedule()
        {
            Address address = new()
            {
                latitude = 31.7917265,
                longitude = 34.7027684,
                houseNumber = 10,
                Id = 42,
                street = "רחל המשוררת",
                city = "גן יבנה",
                postalCode = "0",
            };
          
            NewDoctorScheduleDto doctorSchedule = new()
            {
                doctorid = "e8cdb665-f4fe-49e0-953c-b1aac2d5d94e",
                date = "19/08/2025",
                starttime = "9:00",
                endtime = "15:00",
                slotsid = [],
                addressId = 42,
                Address = address
            };

            var postDoctorScheduleRequest = new RestRequest(Endpoint, Method.Post);
            var client = new RestClient(BaseUrl);
            postDoctorScheduleRequest.AddHeader("Content-Type", "application/json"); // Add this line
            postDoctorScheduleRequest.AddJsonBody(doctorSchedule); // This might not be sending the right format

            var postDoctorSchedulerResponse = await client.ExecuteAsync(postDoctorScheduleRequest);

            _output.WriteLine($"Status Code: {postDoctorSchedulerResponse.StatusCode}");
            _output.WriteLine($"Content: {postDoctorSchedulerResponse.Content}");

            // שלב 1: ודא שהבקשה הצליחה
            Assert.True(postDoctorSchedulerResponse.IsSuccessful);
            Assert.False(!postDoctorSchedulerResponse.IsSuccessful);
        }

        [Fact]
        public async Task deleteDoctorSchedule()
        {
            Address address = new()
            {
                latitude = 31.7917265,
                longitude = 34.7027684,
                houseNumber = 10,
                Id = 42,
                street = "רחל המשוררת",
                city = "גן יבנה",
                postalCode = "0",
            };

            var client = new RestClient(BaseUrl);
            // יצירת אובייקט של יוזר חדש
            NewDoctorScheduleDto doctor = new()
            {
                scheduleid = new Guid(),
                addressId = 42,
                date = "10/09/2028",
                endtime = "10:00",
                starttime = "09:00",
                slotsid = [],
                doctorid = "e8cdb665-f4fe-49e0-953c-b1aac2d5d94e",
                Address = address
            };

            // הגדרת בקשת POST ליצירת יוזר
            var createDoctorScheduleRequest = new RestRequest(Endpoint, Method.Post);
            createDoctorScheduleRequest.AddJsonBody(doctor);

            // שליחת הבקשה
            var createDoctorScheduleResponse = await client.ExecuteAsync(createDoctorScheduleRequest);
            _output.WriteLine($"Status Code: {createDoctorScheduleResponse.StatusCode}");
            _output.WriteLine($"Content: {createDoctorScheduleResponse.Content}");
            DoctorSchedule doctorScheduleCreated = JsonSerializer.Deserialize<DoctorSchedule>(
            createDoctorScheduleResponse.Content,
    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
   );

            Endpoint = "api/DoctorsSchedule/{" + doctorScheduleCreated.scheduleid + "}";
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