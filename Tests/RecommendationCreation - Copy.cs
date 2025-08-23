using AltermedManager.Models.Dtos;
using AltermedManager.Models.Entities;
using RestSharp;
using System.Net;
using System.Text.Json;
using Xunit;
using Xunit.Abstractions;

namespace UnitTestProject
{
    public class UserCreation
    {
        private readonly ITestOutputHelper _output;

        public UserCreation(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task CreateNewUserSuccessfully()
        {
            var client = new RestClient("http://192.168.1.237:5000");

            // יצירת אובייקט של יוזר חדש
            var newUser = new NewUserDto
            {
                name = "John Doe",
                role = "patient"
            };

            // הגדרת בקשת POST ליצירת יוזר
            var createUserRequest = new RestRequest("api/Users", Method.Post);
            createUserRequest.AddHeader("Content-Type", "application/json");
            createUserRequest.AddJsonBody(newUser);

            // שליחת הבקשה
            var createUserResponse = await client.ExecuteAsync(createUserRequest);

            _output.WriteLine($"Status Code: {createUserResponse.StatusCode}");
            _output.WriteLine($"Content: {createUserResponse.Content}");


            // שלב 2: ודא שגוף התשובה אינו ריק וניתן לפרסור
            var createdUser = JsonSerializer.Deserialize<NewUserDto>(
                createUserResponse.Content!,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            // שלב 3 (אופציונלי): נסה לקבל את היוזר שיצרת כדי לאמת שהוא אכן נשמר במסד הנתונים
            var userId = createdUser.id; // נניח שיש ID ביוזר שחזר
            var getUserRequest = new RestRequest($"api/Users/{userId}", Method.Get);


            // יצירת אובייקט כתובת חדש
            var address = new Address();
            address.city = "גן יבנה"; // הגדרת העיר כ"גן יבנה"

            // יצירת אובייקט של מטופל חדש (NewPatientDto)
            var newPatient = new NewPatientDto
            {
                patientID = "206511111", // מספר תעודת זהות של המטופל
                patientName = createdUser.name, // שימוש בשם של היוזר שנוצר קודם לכן
                patientSurname = createdUser.name, // שימוש בשם של היוזר שנוצר קודם לכן (ניתן לשנות לשם משפחה אם קיים)
                patientEmail = "joen.doe@example.com", // כתובת אימייל של המטופל
                patientPhone = "0545555555", // מספר טלפון של המטופל
                patientAddress = address, // שיוך אובייקט הכתובת שיצרנו למטופל
                healthProvider = "מכבי", // קופת חולים
                gender = 'M', // מין המטופל (זכר)
                dateOfBirth = DateTime.Now // תאריך לידה (כאן מוגדר לתאריך ושעה הנוכחיים)
            };

            // הגדרת בקשת POST ליצירת יוזר
            var createPatientRequest = new RestRequest("api/Patients", Method.Post);
            createPatientRequest.AddHeader("Content-Type", "application/json");
            createPatientRequest.AddJsonBody(newPatient);

            // שליחת הבקשה
            var createPatientResponse = await client.ExecuteAsync(createPatientRequest);

            _output.WriteLine($"Status Code: {createPatientResponse.StatusCode}");
            _output.WriteLine($"Content: {createPatientResponse.Content}");

        }
    }
}