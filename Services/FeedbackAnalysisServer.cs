using AltermedManager.Controllers;
using AltermedManager.Data;
using AltermedManager.Models.Dtos;
using AltermedManager.Models.Entities;
using AltermedManager.Models.Enums;
using AltermedManager.Resources;
using Microsoft.EntityFrameworkCore;

namespace AltermedManager.Services
    {
    public class FeedbackAnalysisServer
        {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AddressController> _log;

        public FeedbackAnalysisServer(ApplicationDbContext context, ILogger<AddressController>log)
            {
            _context = context;
            _log = log;
            }

        /*
         * This function is based on users feedbacks.
         * The goal is to find all positive feedbacks about treatments that treat
         * a specific body part.         
         */
        public List<AnalyzedFeedback> GetHighFeedbacksByBodyPart(string bodyPart, Treatment badTreatment)
            {

            var normalizedParts = new List<string>();
            if (bodyPart.Contains(BodyPartsForReport.Arm))
                normalizedParts.AddRange(new[] { BodyPartsForReport.LeftArm, BodyPartsForReport.RightArm });
            else if (bodyPart.Contains(BodyPartsForReport.Leg))
                normalizedParts.AddRange(new[] { BodyPartsForReport.LeftLeg, BodyPartsForReport.RightLeg });
            else
                normalizedParts.Add(bodyPart);

            var feedbacks = (from feedback in _context.PatientFeedbacks
                             where normalizedParts.Contains(feedback.bodyPart) 
                                && feedback.overallStatus >= TreatmentsConst.TREATMENTS_MIN_SCORE
                             join appointment in _context.Appointments
                                on feedback.appointmentId equals appointment.appointmentId
                             select new AnalyzedFeedback
                                { treatmentId = appointment.treatmentId,
                                score = feedback.overallStatus,
                                bodyPart = feedback.bodyPart
                                }).ToList();
            return feedbacks;
            }

        }
    }
