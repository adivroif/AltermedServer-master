/*
using Microsoft.VisualStudio.TestPlatform.Utilities;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace UnitTestProject
{
    class RecommendationCreation
    {
        private readonly ITestOutputHelper _output;

        public RecommendationCreation(ITestOutputHelper output)
            {
            _output = output;
            }

        [Fact]
        public async Task LoopAppointments_AndCheckRecommendations()
            {
            var client = new RestClient("http://192.168.1.134:5000");

            // STEP 1: Get all appointments
            var getAppointments = new RestRequest("api/appointments", Method.Get);
            var appointmentsResponse = await client.ExecuteAsync(getAppointments);

            appointmentsResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var appointments = JsonSerializer.Deserialize<List<AppointmentDto>>(appointmentsResponse.Content!, new JsonSerializerOptions
                {
                PropertyNameCaseInsensitive = true
                });

            foreach (var appointment in appointments!)
                {
                var id = appointment.Id;

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
                }
            }




        }
    }
*/