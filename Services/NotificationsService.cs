namespace AltermedManager.Services
    {
    using FirebaseAdmin.Messaging;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using AltermedManager.Models.Entities;
    using System;
    using Newtonsoft.Json.Linq;
    using Microsoft.EntityFrameworkCore;
    using AltermedManager.Data;

    public interface INotificationsService
        {
        Task SendDoctorRecommendationApprovalRequestAsync(Guid doctorId, Recommendation recommendation, string? msgToken);
        Task SendPatientRecommendationApprovedAsync(string token);
        Task SendDoctorReplyToPatientAsync(string token, string message);
        }

    public class NotificationsService : INotificationsService
        {
        private readonly ApplicationDbContext _context;

        public NotificationsService(ApplicationDbContext context)
            {
            _context = context;
            }

        public async Task SendDoctorRecommendationApprovalRequestAsync(Guid doctorId, Recommendation recommendation, string? msgToken)
            {
            string _title = "הודעה חדשה";            
            string _body = $"נא לבדוק המלצה חדשה למופל - {recommendation.Patient.patientName} {recommendation.Patient.patientSurname}";

            _context.StoredNotifications.Add(new StoredNotification
                {
                userId = doctorId,
                title = _title,
                body = _body,
                isRead = false
                });
            await _context.SaveChangesAsync();
            await SendRecommendationsNotificationAsync(
                    msgToken ?? string.Empty,
                    _title,
                    _body,
                    "recommendation", recommendation
                );

            }

        public async Task SendPatientRecommendationApprovedAsync(string token)
            {
           
            }

        public async Task SendDoctorReplyToPatientAsync(string token, string message)
            {
           
            }

        private async Task SendRecommendationsNotificationAsync(string token, string title, string body, string type, object data = null)
            {
            var message = new Message()
                {
                Token = token,
                Notification = new Notification
                    {
                    Title = title,
                    Body = body
                    },
                Android = new AndroidConfig
                    {
                    Priority = Priority.High,
                    Notification = new AndroidNotification
                        {
                        Sound = "default" 
                        }
                    },
                Apns = new ApnsConfig
                    {
                    Aps = new Aps
                        {
                        Sound = "default"
                        }
                    },
                Data = new Dictionary<string, string>
            {
                { "type", type },
                { "screen", type == "recommendation" ? "recommendationsDApprove" : "responses" }
            }
                };

            try
                {

                await FirebaseMessaging.DefaultInstance.SendAsync(message);
                } catch (Exception e)
                {
                Console.WriteLine("--------", e.Message);
                }
            }

        
        }

    }
