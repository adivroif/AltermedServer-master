using AltermedManager.Controllers;
using AltermedManager.Data;
using AltermedManager.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace YourProjectName.Controllers
{
    // A simple class to represent the incoming request data
    public class ForgotPasswordRequest
    {
        public string? Email { get; set; }
    }

    public class ForgotPasswordSmsRequest
    {
        public string? PhoneNumber { get; set; }
    }

    [ApiController]
    [Route("[controller]")] // This will make the controller accessible at /Auth
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public AuthController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpPost("forgot-password-sms")] // New endpoint for SMS-based password reset
        public async Task<IActionResult> ForgotPasswordSms([FromBody] ForgotPasswordSmsRequest request)
        {
            if (string.IsNullOrEmpty(request.PhoneNumber))
            {
                return BadRequest(new { message = "Phone number is required" });
            }

            Console.WriteLine(request.PhoneNumber);
            // Search for the patient in the database using their phone number
            Patient? user = await dbContext.Patients
                                          .Where(p => p.patientPhone == request.PhoneNumber)
                                          .FirstOrDefaultAsync();

            if (user != null)
            {
                // Generate a 6-digit OTP
                var otp = new Random().Next(100000, 999999).ToString();
                var expiration = DateTime.UtcNow.AddMinutes(5); // OTP is valid for 5 minutes

                // Send the OTP via SMS
                await SendPasswordResetSmsAsync(request.PhoneNumber, otp);

                return Ok(new { message = "OTP sent successfully to your phone number." });
            }
            else
            {
                // For security, return a success message even if the user is not found.
                Console.WriteLine($"Password reset requested for a non-existent phone number: {request.PhoneNumber}");
                return Ok(new { message = "If a matching account was found, an OTP has been sent to your phone." });
            }
        }

        /// <summary>
        /// A dummy function to simulate sending an SMS.
        /// In a real-world application, you would use a third-party SMS service provider here.
        /// </summary>
        /// <param name="phoneNumber">The recipient's phone number.</param>
        /// <param name="otp">The one-time password to be sent.</param>
        private async Task SendPasswordResetSmsAsync(string phoneNumber, string otp)
        {
            // You need to install Twilio NuGet package:
            // dotnet add package Twilio
            // Or in Visual Studio: right-click your project -> Manage NuGet Packages -> Search for Twilio and install.

            // Replace with your Twilio Account SID and Auth Token
            // You can find these in your Twilio Console dashboard.
            var accountSid ="AC92def8778897c4eca4c34bfe99242ed2";
            var authToken = "3c58c2916e5c7ea240303c531f89d1ef";
            var twilioPhoneNumber = "+18286723858"; // Your Twilio phone number

            if (string.IsNullOrEmpty(accountSid) || string.IsNullOrEmpty(authToken))
            {
                Console.WriteLine("Twilio credentials are not set. SMS will not be sent.");
                return;
            }

            TwilioClient.Init(accountSid, authToken);

            try
            {
                var message = await MessageResource.CreateAsync(
                    to: new Twilio.Types.PhoneNumber(phoneNumber),
                    from: new Twilio.Types.PhoneNumber(twilioPhoneNumber),
                    body: $"Your AlterMed password reset code is: {otp}"
                );
                Console.WriteLine($"SMS sent successfully with SID: {message.Sid}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending SMS: {ex.Message}");
                // Handle the exception, for example, by logging it.
            }
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            if (string.IsNullOrEmpty(request.Email))
            {
                return BadRequest(new { message = "Email is required" });
            }
            Console.WriteLine(request.Email);
            Patient? user = await dbContext.Patients
                                          .Where(p => p.patientEmail == request.Email)
                                          .FirstOrDefaultAsync();
            if (user != null)
            {
                var token = Guid.NewGuid().ToString();
                var expiration = DateTime.UtcNow.AddMinutes(15);

                var resetLink = $"AlterMed://reset-password?token={token}";

                await SendPasswordResetEmailAsync(request.Email, resetLink);

                return Ok(new { message = "Password reset link sent successfully" });
            }
            else
            {
                Console.WriteLine($"Password reset requested for a non-existent email: {request.Email}");
                return Ok(new { message = "If a matching account was found, a reset link has been sent to your email." });
            }
        }

        private async Task SendPasswordResetEmailAsync(string toEmail, string resetLink)
        {
            // *** IMPORTANT: Replace the following with your Gmail credentials. ***
            var fromEmail = "adivroif@gmail.com";
            var fromPassword = "xrof jouq majt tjuu";

            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail, fromPassword)
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(fromEmail),
                Subject = "בקשת איפוס סיסמה",
                Body = $@"<h1>איפוס סיסמה</h1><p>לחץ על הקישור הבא כדי לאפס את סיסמתך:</p><a href='{resetLink}'>{resetLink}</a>",
                IsBodyHtml = true,
            };

            mailMessage.To.Add(toEmail);

            try
            {
                await client.SendMailAsync(mailMessage);
                Console.WriteLine($"מייל נשלח בהצלחה ל- {toEmail}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"שגיאה בשליחת מייל: {ex.Message}");
            }
        }
    }
}
