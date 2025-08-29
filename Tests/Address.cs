using AltermedManager.Models.Dtos;
using AltermedManager.Models.Entities;
using RestSharp;
using System.Net;
using System.Text.Json;
using Xunit;
using Xunit.Abstractions;

namespace UnitTestProject
{
    public class Addresses
    {
        private readonly ITestOutputHelper _output;
        private const string BaseUrl = "http://10.0.0.25:5000";
        private string Endpoint = "api/Address";

        public Addresses(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task getAddressByAddressId()
        {
            int addressId = 42;
            Endpoint = "api/Address/addressId/" + addressId.ToString();
            _output.WriteLine("here");
            var getAddressRequest = new RestRequest(Endpoint, Method.Get);
            var client = new RestClient(BaseUrl);
            var getAddressResponse = await client.ExecuteAsync(getAddressRequest);
            getAddressRequest.AddHeader("Content-Type", "application/json"); // Add this line
            getAddressRequest.AddJsonBody(addressId.ToString());
            _output.WriteLine($"Status Code: {getAddressResponse.StatusCode}");
            _output.WriteLine($"Content: {getAddressResponse.Content}");

            // שלב 1: ודא שהבקשה הצליחה
            Assert.True(getAddressResponse.IsSuccessful);
            Assert.False(!getAddressResponse.IsSuccessful);
        }

        [Fact]
        public async Task getAddressByLatLng()
        {
            double lat = 32.1789672;
            double lng = 34.9099126;
            Endpoint = "api/Address/lat/" + lat.ToString() + "/lng/" + lng.ToString();
            _output.WriteLine("here");
            var getAddressRequest = new RestRequest(Endpoint, Method.Get);
            var client = new RestClient(BaseUrl);
            var getAddressResponse = await client.ExecuteAsync(getAddressRequest);
            getAddressRequest.AddHeader("Content-Type", "application/json"); // Add this line
            var locationData = new { lat = lat.ToString(), lng = lng.ToString() };
            getAddressRequest.AddJsonBody(locationData);
            _output.WriteLine($"Status Code: {getAddressResponse.StatusCode}");
            _output.WriteLine($"Content: {getAddressResponse.Content}");

            // שלב 1: ודא שהבקשה הצליחה
            Assert.True(getAddressResponse.IsSuccessful);
            Assert.False(!getAddressResponse.IsSuccessful);
        }

        [Fact]
        public async Task getAllAddresses()
        {
            _output.WriteLine("here");
            var getAllAddressesRequest = new RestRequest(Endpoint, Method.Get);
            var client = new RestClient(BaseUrl);
            var allAddressesResponse = await client.ExecuteAsync(getAllAddressesRequest);

            _output.WriteLine($"Status Code: {allAddressesResponse.StatusCode}");
            _output.WriteLine($"Content: {allAddressesResponse.Content}");

            // שלב 1: ודא שהבקשה הצליחה
            Assert.True(allAddressesResponse.IsSuccessful);
            Assert.False(!allAddressesResponse.IsSuccessful);
        }

        [Fact]
        public async Task postAddress()
        {
            _output.WriteLine($" Code: ");

            NewAddressDto address = new NewAddressDto();
            address.street = "רחל המשוררת";
            address.city = "גן יבנה";
            address.postalCode = "0";
            address.houseNumber = 12;
            address.latitude = 31.7917266;
            address.longitude = 34.7027685;
            var postAddressRequest = new RestRequest(Endpoint, Method.Post);
            var client = new RestClient(BaseUrl);
            postAddressRequest.AddHeader("Content-Type", "application/json"); // Add this line
            postAddressRequest.AddJsonBody(address); // This might not be sending the right format

            var postAddressResponse = await client.ExecuteAsync(postAddressRequest);

            _output.WriteLine($"Status Code: {postAddressResponse.StatusCode}");
            _output.WriteLine($"Content: {postAddressResponse.Content}");

            // שלב 1: ודא שהבקשה הצליחה
            Assert.True(postAddressResponse.IsSuccessful);
            Assert.False(!postAddressResponse.IsSuccessful);
        }

        [Fact]
        public async Task deleteAddress()
        {
            var client = new RestClient(BaseUrl);

            NewAddressDto address = new NewAddressDto();
            address.street = "רחל המשוררת";
            address.city = "גן יבנה";
            address.postalCode = "0";
            address.houseNumber = 12;
            address.latitude = 31.7917266;
            address.longitude = 34.7027685;

            // הגדרת בקשת POST ליצירת יוזר
            var createAddressRequest = new RestRequest(Endpoint, Method.Post);
            createAddressRequest.AddJsonBody(address);

            // שליחת הבקשה
            var createAddressResponse = await client.ExecuteAsync(createAddressRequest);
            _output.WriteLine($"Status Code: {createAddressResponse.StatusCode}");
            _output.WriteLine($"Content: {createAddressResponse.Content}");
            Address addressCreated = JsonSerializer.Deserialize<Address>(
            createAddressResponse.Content,
    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
   );

            Endpoint = "api/Address/" + addressCreated.Id;
            var deleteAddressRequest = new RestRequest(Endpoint, Method.Delete);
            client = new RestClient(BaseUrl);
            var deleteAddressResponse = await client.ExecuteAsync(deleteAddressRequest);
            deleteAddressRequest.AddHeader("Content-Type", "application/json"); // Add this line
            _output.WriteLine($"Status Code: {deleteAddressResponse.StatusCode}");
            _output.WriteLine($"Content: {deleteAddressResponse.Content}");

            // שלב 1: ודא שהבקשה הצליחה
            Assert.True(deleteAddressResponse.IsSuccessful);
            Assert.False(!deleteAddressResponse.IsSuccessful);

        }
    }
}