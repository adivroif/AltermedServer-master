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
    public class AppointmentsSlots
    {
        private readonly ITestOutputHelper _output;
        private const string BaseUrl = "http://192.168.1.237:5000";
        private string Endpoint = "api/AppointmentsSlots";

        public AppointmentsSlots(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task getAllAppointmentsSlots()
        {
            _output.WriteLine("here");
            var getAllAppointmentsSlotsRequest = new RestRequest(Endpoint, Method.Get);
            var client = new RestClient(BaseUrl);
            var allAppointmentsSlotsResponse = await client.ExecuteAsync(getAllAppointmentsSlotsRequest);

            _output.WriteLine($"Status Code: {allAppointmentsSlotsResponse.StatusCode}");
            _output.WriteLine($"Content: {allAppointmentsSlotsResponse.Content}");

            // שלב 1: ודא שהבקשה הצליחה
            Assert.True(allAppointmentsSlotsResponse.IsSuccessful);
            Assert.False(!allAppointmentsSlotsResponse.IsSuccessful);
        }

        [Fact]
        public async Task getAppointmentSlotByAppointmentSlotId()
        {
            int id = 38;
            Endpoint = "api/AppointmentsSlots/" + id.ToString();
            _output.WriteLine("here");
            var getAppointmentSlotRequest = new RestRequest(Endpoint, Method.Get);
            var client = new RestClient(BaseUrl);
            var getAppointmentSlotResponse = await client.ExecuteAsync(getAppointmentSlotRequest);
            getAppointmentSlotRequest.AddHeader("Content-Type", "application/json"); // Add this line
            getAppointmentSlotRequest.AddJsonBody(id.ToString());
            _output.WriteLine($"Status Code: {getAppointmentSlotResponse.StatusCode}");
            _output.WriteLine($"Content: {getAppointmentSlotResponse.Content}");

            // שלב 1: ודא שהבקשה הצליחה
            Assert.True(getAppointmentSlotResponse.IsSuccessful);
            Assert.False(!getAppointmentSlotResponse.IsSuccessful);
        }

        [Fact]
        public async Task getAndUpdateAppointmentSlotByAppointmentSlotId()
        {
            int id = 38;
            Endpoint = "api/AppointmentsSlots/" + id.ToString();
            _output.WriteLine("here");
            var getAppointmentSlotRequest = new RestRequest(Endpoint, Method.Get);
            var client = new RestClient(BaseUrl);
            var getAppointmentSlotResponse = await client.ExecuteAsync(getAppointmentSlotRequest);
            getAppointmentSlotRequest.AddHeader("Content-Type", "application/json"); // Add this line
            getAppointmentSlotRequest.AddJsonBody(id.ToString());

            AppointmentSlots getSlot = JsonSerializer.Deserialize<AppointmentSlots>(
    getAppointmentSlotResponse.Content,
    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
   );
            _output.WriteLine($"Status Code: {getAppointmentSlotResponse.StatusCode}");
            _output.WriteLine($"Content: {getAppointmentSlotResponse.Content}");

            Endpoint = "api/AppointmentsSlots/" + getSlot.slotid.ToString();
            var updateAppointmentSlotRequest = new RestRequest(Endpoint, Method.Put);
            getSlot.starttime = "19:00";
            getSlot.endtime = "19:15";
            updateAppointmentSlotRequest.AddHeader("Content-Type", "application/json"); // Add this line
            updateAppointmentSlotRequest.AddJsonBody(getSlot);
            var updateAppointmentSlotResponse = await client.ExecuteAsync(updateAppointmentSlotRequest);

            _output.WriteLine($"Status Code: {updateAppointmentSlotResponse.StatusCode}");
            _output.WriteLine($"Content: {updateAppointmentSlotResponse.Content}");
            // שלב 1: ודא שהבקשה הצליחה
            Assert.True(updateAppointmentSlotResponse.IsSuccessful);
            Assert.False(!updateAppointmentSlotResponse.IsSuccessful);
        }

        [Fact]
        public async Task postAppointmentsSlots()
        {
            NewAppointmentSlotsDto appointmentSlots = new NewAppointmentSlotsDto();
            appointmentSlots.slotid = 1990;
            appointmentSlots.date_of_treatment = DateOnly.FromDateTime(DateTime.UtcNow);
            appointmentSlots.doctorid = new Guid("3349c2a5-d8f6-43a3-b02d-6f7b4a45390a");
            appointmentSlots.starttime = "9:00";
            appointmentSlots.endtime = "9:15";
            appointmentSlots.isbooked = 0;
            _output.WriteLine($"Status Code: " + appointmentSlots);
            var postAppointmentSlotRequest = new RestRequest(Endpoint, Method.Post);
            var client = new RestClient(BaseUrl);
            postAppointmentSlotRequest.AddHeader("Content-Type", "application/json"); // Add this line
            postAppointmentSlotRequest.AddJsonBody(appointmentSlots); // This might not be sending the right format

            var postAppointmentSlotResponse = await client.ExecuteAsync(postAppointmentSlotRequest);

            _output.WriteLine($"Status Code: {postAppointmentSlotResponse.StatusCode}");
            _output.WriteLine($"Content: {postAppointmentSlotResponse.Content}");

            // שלב 1: ודא שהבקשה הצליחה
            Assert.True(postAppointmentSlotResponse.IsSuccessful);
            Assert.False(!postAppointmentSlotResponse.IsSuccessful);
        }

        [Fact]
        public async Task deleteSlot()
        {
            NewAppointmentSlotsDto appointmentSlots = new NewAppointmentSlotsDto();
            appointmentSlots.slotid = 1990;
            appointmentSlots.date_of_treatment = DateOnly.FromDateTime(DateTime.UtcNow);
            appointmentSlots.doctorid = new Guid("3349c2a5-d8f6-43a3-b02d-6f7b4a45390a");
            appointmentSlots.starttime = "9:00";
            appointmentSlots.endtime = "9:15";
            appointmentSlots.isbooked = 0;
            _output.WriteLine($"Status Code: " + appointmentSlots);
            var postAppointmentSlotRequest = new RestRequest(Endpoint, Method.Post);
            var client = new RestClient(BaseUrl);
            postAppointmentSlotRequest.AddHeader("Content-Type", "application/json"); // Add this line
            postAppointmentSlotRequest.AddJsonBody(appointmentSlots); // This might not be sending the right format

            var postAppointmentSlotResponse = await client.ExecuteAsync(postAppointmentSlotRequest);

            _output.WriteLine($"Status Code: {postAppointmentSlotResponse.StatusCode}");
            _output.WriteLine($"Content: {postAppointmentSlotResponse.Content}");
            AppointmentSlots appointmentSlotCreated = JsonSerializer.Deserialize<AppointmentSlots>(
            postAppointmentSlotResponse.Content,
    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
   );

            Endpoint = "api/AppointmentsSlots/" + appointmentSlotCreated.slotid;
            var deleteUserRequest = new RestRequest(Endpoint, Method.Delete);
            client = new RestClient(BaseUrl);
            var deleteUserResponse = await client.ExecuteAsync(deleteUserRequest);
            deleteUserRequest.AddHeader("Content-Type", "application/json"); // Add this line
            _output.WriteLine($"Status Code: {deleteUserResponse.StatusCode}");
            _output.WriteLine($"Content: {deleteUserResponse.Content}");

            // שלב 1: ודא שהבקשה הצליחה
            Assert.True(deleteUserResponse.IsSuccessful);
            Assert.False(!deleteUserResponse.IsSuccessful);

        }
    }
}