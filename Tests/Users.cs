using AltermedManager.Models.Dtos;
using AltermedManager.Models.Entities;
using RestSharp;
using System.Net;
using System.Text.Json;
using Xunit;
using Xunit.Abstractions;

namespace UnitTestProject
{
    public class Users
    {
        private readonly ITestOutputHelper _output;
        private const string BaseUrl = "http://10.0.0.25:5000";
        private string Endpoint = "api/Users";

        public Users(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task getAllUsers()
        {
            var getAllUsersRequest = new RestRequest(Endpoint, Method.Get);
            var client = new RestClient(BaseUrl);
            var allUsersResponse = await client.ExecuteAsync(getAllUsersRequest);

            _output.WriteLine($"Status Code: {allUsersResponse.StatusCode}");
            _output.WriteLine($"Content: {allUsersResponse.Content}");

            // שלב 1: ודא שהבקשה הצליחה
            Assert.True(allUsersResponse.IsSuccessful);
            Assert.False(!allUsersResponse.IsSuccessful);
        }

        [Fact]
        public async Task getDoctorByUserId()
        {
            String id = "28621ddc-7343-4656-a944-afcac0b04b89";
            Endpoint = "api/Users/by-user/" + id;
            var getUserRequest = new RestRequest(Endpoint, Method.Get);
            var client = new RestClient(BaseUrl);
            var getUserResponse = await client.ExecuteAsync(getUserRequest);
            getUserRequest.AddHeader("Content-Type", "application/json"); // Add this line
            getUserRequest.AddJsonBody(id);
            _output.WriteLine($"Status Code: {getUserResponse.StatusCode}");
            _output.WriteLine($"Content: {getUserResponse.Content}");

            // שלב 1: ודא שהבקשה הצליחה
            Assert.True(getUserResponse.IsSuccessful);
            Assert.False(!getUserResponse.IsSuccessful);
        }

        [Fact]
        public async Task getDoctorByUserName()
        {
            String Name = "דוד כהן";
            Endpoint = "api/Users/" + Name;
            var getUserRequest = new RestRequest(Endpoint, Method.Get);
            var client = new RestClient(BaseUrl);
            var getUserResponse = await client.ExecuteAsync(getUserRequest);
            getUserRequest.AddHeader("Content-Type", "application/json"); // Add this line
            getUserRequest.AddJsonBody(Name);
            _output.WriteLine($"Status Code: {getUserResponse.StatusCode}");
            _output.WriteLine($"Content: {getUserResponse.Content}");

            // שלב 1: ודא שהבקשה הצליחה
            Assert.True(getUserResponse.IsSuccessful);
            Assert.False(!getUserResponse.IsSuccessful);

        }

        [Fact]
        public async Task getDoctorByUserEmail()
        {
            String email = "T@example.com";
            Endpoint = "api/Users/email" + email;
            var getUserRequest = new RestRequest(Endpoint, Method.Get);
            var client = new RestClient(BaseUrl);
            var getUserResponse = await client.ExecuteAsync(getUserRequest);
            getUserRequest.AddHeader("Content-Type", "application/json"); // Add this line
            getUserRequest.AddJsonBody(email);
            _output.WriteLine($"Status Code: {getUserResponse.StatusCode}");
            _output.WriteLine($"Content: {getUserResponse.Content}");

            // שלב 1: ודא שהבקשה הצליחה
            Assert.True(getUserResponse.IsSuccessful);
            Assert.False(!getUserResponse.IsSuccessful);

        }

        [Fact]
        public async Task postUser()
        {
            var client = new RestClient(BaseUrl);
            // יצירת אובייקט של יוזר חדש
            var newUser = new NewUserDto
            {
                name = "John Doe",
                role = "patient",
                firebaseId = "edr93Eb4nmPVRNtoRgeV0ukNIiR2"
            };

            // הגדרת בקשת POST ליצירת יוזר
            var createUserRequest = new RestRequest(Endpoint, Method.Post);
            createUserRequest.AddJsonBody(newUser);

            // שליחת הבקשה
            var createUserResponse = await client.ExecuteAsync(createUserRequest);

            _output.WriteLine($"Status Code: {createUserResponse.StatusCode}");
            _output.WriteLine($"Content: {createUserResponse.Content}");

            // שלב 1: ודא שהבקשה הצליחה
            Assert.True(createUserResponse.IsSuccessful);
        }

        [Fact]
        public async Task postUserUpdateToken()
        {

            String Name = "John Doe";
            Endpoint = "api/Users/" + Name;
            var getUserRequest = new RestRequest(Endpoint, Method.Get);
            var client = new RestClient(BaseUrl);
            var getUserResponse = await client.ExecuteAsync(getUserRequest);
            getUserRequest.AddHeader("Content-Type", "application/json"); // Add this line
            getUserRequest.AddJsonBody(Name);

            User getUser = JsonSerializer.Deserialize<User>(
            getUserResponse.Content,
    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
   );
            _output.WriteLine(getUser.id.ToString() );
            var updateToken = new {
                token = "fqwNk3_jR1unY11ioOy1Ro:APA91bFTncBl9gzPpqnJec9FvAoD-DcwIeMoMJ_DIW2SM1bLsVfIwiEOwNfhNBR9bkSKv5XcXsmPP7VVk1ezgvjQFnMvwiprL3LLpmCuFzXU04XqDwhHJuQ",
                uid = getUser.id
            };
            Endpoint = "api/Users/update-token";

            // הגדרת בקשת POST ליצירת יוזר
            var createUserRequest = new RestRequest(Endpoint, Method.Post);
            createUserRequest.AddJsonBody(updateToken);
            // שליחת הבקשה
            var createUserResponse = await client.ExecuteAsync(createUserRequest);
            getUserRequest.AddHeader("Content-Type", "application/json"); // Add this line

            _output.WriteLine($"Status Code: {createUserResponse.StatusCode}");
            _output.WriteLine($"Content: {createUserResponse.Content}");

            // שלב 1: ודא שהבקשה הצליחה
            Assert.True(createUserResponse.IsSuccessful);
        }

        [Fact]
        public async Task deleteUser()
        {
            var client = new RestClient(BaseUrl);
            // יצירת אובייקט של יוזר חדש
            var newUser = new NewUserDto
            {
                name = "John Doe",
                role = "patient",
                firebaseId = "edr93Eb4nmPVRNtoRgeV0ukNIiR2"
            };

            // הגדרת בקשת POST ליצירת יוזר
            var createUserRequest = new RestRequest(Endpoint, Method.Post);
            createUserRequest.AddJsonBody(newUser);

            // שליחת הבקשה
            var createUserResponse = await client.ExecuteAsync(createUserRequest);
            _output.WriteLine($"Status Code: {createUserResponse.StatusCode}");
            _output.WriteLine($"Content: {createUserResponse.Content}");
            User userCreated = JsonSerializer.Deserialize<User>(
            createUserResponse.Content,
    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
   );

            Endpoint = "api/Users/{"+userCreated.id+"}";
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