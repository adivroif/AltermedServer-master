namespace AltermedManager.Services
    {
    using FirebaseAdmin.Messaging;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using AltermedManager.Models.Entities;
    using System;
    using AltermedManager.Data;
    using AltermedManager.Resources;
    using System.Globalization;
    using Google.Apis.Http;
    using AltermedManager.Models.Enums;

    public interface INotificationsService
        {
        Task SendDoctorRecommendationApprovalRequestAsync(Guid doctorId, Recommendation recommendation, string? msgToken);
        Task SendNewRecommendationToPatientAsync(Guid patientId, Recommendation recommendation, string? msgToken);
        Task SendNewPatientRequestToDoctor(Guid doctorId, PatientRequest request, string? msgToken);
        Task SendNewDoctorResponseToPatient(Guid patientId, PatientRequest request, string? msgToken);
        Task SendNotificationAsync(string token, string title, string body, string type, object data = null);
        
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
            
            string _title = Messages.NewNotification; 
            string _body = $"{Messages.NewRecommendationToDoctorForApprove} - {recommendation.Patient.patientName} {recommendation.Patient.patientSurname}";

            _context.StoredNotifications.Add(new StoredNotification
                {
                userId = doctorId,
                title = _title,
                body = _body,
                isRead = false,
                type = NotificationsTypes.RECOMMENDATION
                });
            await _context.SaveChangesAsync();
            await SendNotificationAsync(
                    msgToken ?? string.Empty,
                    _title,
                    _body,
                    "recommendation", recommendation
                );

            }

        public async Task SendNewRecommendationToPatientAsync(Guid patientId, Recommendation recommendation, string? msgToken)
            {
            string _title = Messages.NewNotification;
            string _body = $"{Messages.NewRecommendationForPatient}";

            _context.StoredNotifications.Add(new StoredNotification
                {
                userId = patientId,
                title = _title,
                body = _body,
                isRead = false,
                type = NotificationsTypes.RECOMMENDATION
                });
                await _context.SaveChangesAsync();
                await SendNotificationAsync(
                    msgToken ?? string.Empty,
                    _title,
                    _body,
                    "recommendation", recommendation
                );
            }
        public async Task SendNewPatientRequestToDoctor(Guid doctorId, PatientRequest request, string? msgToken)
            {
            string _title = Messages.NewRequest; ;
            string _body = $"{Messages.NewRequestFromPatientToDoctor}";

            _context.StoredNotifications.Add(new StoredNotification
                {
                userId = doctorId,
                title = _title,
                body = _body,
                isRead = false,
                type = NotificationsTypes.REQUEST
                });
            await _context.SaveChangesAsync();
            await SendNotificationAsync(
                    msgToken ?? string.Empty,
                    _title,
                    _body,
                    "request", request
                );

            }
        public async Task SendNewDoctorResponseToPatient(Guid patientId, PatientRequest request, string? msgToken)
            {
            string _title = Messages.NewResponse;
            string _body = $"{Messages.NewResponseFromDoctorToPatient}";

            _context.StoredNotifications.Add(new StoredNotification
                {
                userId = patientId,
                title = _title,
                body = _body,
                isRead = false,
                type = NotificationsTypes.RESPONSE
                });
            await _context.SaveChangesAsync();
            await SendNotificationAsync(
                msgToken ?? string.Empty,
                _title,
                _body,
                "request", request
            );

            }

        public async Task SendNotificationAsync(string token, string title, string body, string type, object data = null)
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
                Console.WriteLine($"-------- {e.Message}");

                }
            }



        }

    }
