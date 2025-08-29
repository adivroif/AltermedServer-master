using AltermedManager.Models.Dtos;
using AltermedManager.Models.Entities;
using RestSharp;
using System.Net;
using System.Text.Json;
using Xunit;
using Xunit.Abstractions;

namespace UnitTestProject
{
    public class Appointments
    {
        private readonly ITestOutputHelper _output;
        private const string BaseUrl = "http://192.168.1.237:5000";
        private string Endpoint = "api/Appointments";

        public Appointments(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task getAllAppointments()
        {
            _output.WriteLine("here");
            var getAllAppointmentsRequest = new RestRequest(Endpoint, Method.Get);
            var client = new RestClient(BaseUrl);
            var allAppointmentsResponse = await client.ExecuteAsync(getAllAppointmentsRequest);

            _output.WriteLine($"Status Code: {allAppointmentsResponse.StatusCode}");
            _output.WriteLine($"Content: {allAppointmentsResponse.Content}");

            // שלב 1: ודא שהבקשה הצליחה
            Assert.True(allAppointmentsResponse.IsSuccessful);
            Assert.False(!allAppointmentsResponse.IsSuccessful);
        }

        [Fact]
        public async Task getAllAppointmentByPatientId()
        {
            String patientId = "28621ddc-7343-4656-a944-afcac0b04b89";
            Endpoint = "api/Appointments/patient/{" + patientId + "}";
            _output.WriteLine("here");
            var getAppointmentRequest = new RestRequest(Endpoint, Method.Get);
            var client = new RestClient(BaseUrl);
            var getAppointmentResponse = await client.ExecuteAsync(getAppointmentRequest);
            getAppointmentRequest.AddHeader("Content-Type", "application/json"); // Add this line
            getAppointmentRequest.AddJsonBody(patientId);
            _output.WriteLine($"Status Code: {getAppointmentResponse.StatusCode}");
            _output.WriteLine($"Content: {getAppointmentResponse.Content}");

            // שלב 1: ודא שהבקשה הצליחה
            Assert.True(getAppointmentResponse.IsSuccessful);
            Assert.False(!getAppointmentResponse.IsSuccessful);
        }

        [Fact]
        public async Task getAllAppointmentByDoctorId()
        {
            String doctorId = "e8cdb665-f4fe-49e0-953c-b1aac2d5d94e";
            Endpoint = "api/Appointments/doctor/{" + doctorId + "}";
            _output.WriteLine("here");
            var getAppointmentRequest = new RestRequest(Endpoint, Method.Get);
            var client = new RestClient(BaseUrl);
            var getAppointmentResponse = await client.ExecuteAsync(getAppointmentRequest);
            getAppointmentRequest.AddHeader("Content-Type", "application/json"); // Add this line
            getAppointmentRequest.AddJsonBody(doctorId);
            _output.WriteLine($"Status Code: {getAppointmentResponse.StatusCode}");
            _output.WriteLine($"Content: {getAppointmentResponse.Content}");

            // שלב 1: ודא שהבקשה הצליחה
            Assert.True(getAppointmentResponse.IsSuccessful);
            Assert.False(!getAppointmentResponse.IsSuccessful);
        }

        [Fact]
        public async Task getAppointmentByAppointmentId()
        {
            String id = "0198add4-d9eb-7046-82e7-421fafd8b2b3";
            Endpoint = "api/Appointments/{" + id + "}";
            _output.WriteLine("here");
            var getAppointmentRequest = new RestRequest(Endpoint, Method.Get);
            var client = new RestClient(BaseUrl);
            var getAppointmentResponse = await client.ExecuteAsync(getAppointmentRequest);
            getAppointmentRequest.AddHeader("Content-Type", "application/json"); // Add this line
            getAppointmentRequest.AddJsonBody(id);
            _output.WriteLine($"Status Code: {getAppointmentResponse.StatusCode}");
            _output.WriteLine($"Content: {getAppointmentResponse.Content}");

            // שלב 1: ודא שהבקשה הצליחה
            Assert.True(getAppointmentResponse.IsSuccessful);
            Assert.False(!getAppointmentResponse.IsSuccessful);
        }

        [Fact]
        public async Task postAppointment()
        {
            _output.WriteLine($" Code: ");

            NewAppointmentDto appointment = new NewAppointmentDto();
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
            AppointmentSlots createdSlot = JsonSerializer.Deserialize<AppointmentSlots>(
                postAppointmentSlotResponse.Content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
               );
            Endpoint = "api/Appointments";
            Address address = new Address();
            address.latitude = 31.7917265;
            address.longitude = 34.7027684;
            address.houseNumber = 10;
            address.Id = 42;
            address.street = "רחל המשוררת";
            address.city = "גן יבנה";
            address.postalCode = "0";
            appointment.appointmentId = new Guid();
            appointment.startSlot = createdSlot.slotid;
            appointment.treatmentId = 8;
            appointment.patientId = new Guid("997bfdd1-3097-4e1c-ad7a-d48dad7f92c4");
            appointment.doctorId = new Guid("e8cdb665-f4fe-49e0-953c-b1aac2d5d94e");
            appointment.recordId = new Guid();
            appointment.duration = 30;
            appointment.Address = address;
            appointment.statusOfAppointment = 0;
            var postAppointmentRequest = new RestRequest(Endpoint, Method.Post);
            client = new RestClient(BaseUrl);
            postAppointmentRequest.AddHeader("Content-Type", "application/json"); // Add this line
            postAppointmentRequest.AddJsonBody(appointment); // This might not be sending the right format

            var postAppointmentResponse = await client.ExecuteAsync(postAppointmentRequest);

            _output.WriteLine($"Status Code: {postAppointmentResponse.StatusCode}");
            _output.WriteLine($"Content: {postAppointmentResponse.Content}");

            // שלב 1: ודא שהבקשה הצליחה
            Assert.True(postAppointmentResponse.IsSuccessful);
            Assert.False(!postAppointmentResponse.IsSuccessful);
        }
    }
}