using AltermedManager.Data;
using AltermedManager.Models.Dtos;
using AltermedManager.Models.Entities;
using AltermedManager.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace AltermedManager.Services
    {
    public class FeedbackAnalysisServer
        {
        private readonly ApplicationDbContext _context;


        public FeedbackAnalysisServer(ApplicationDbContext context)
            {
            _context = context;
            }

        /*
         * This function is based on users feedbacks.
         * The goal is to find all positive feedbacks about treatments that treat
         * a specific body part.         
         */
        public List<AnalyzedFeedback> GetHighFeedbacksByBodyPart(string bodyPart, Treatment badTreatment)
            {

            var normalizedParts = new List<string>();
            if (bodyPart.Contains("יד"))
                normalizedParts.AddRange(new[] { "יד שמאלית", "יד ימנית" });
            else if (bodyPart.Contains("רגל"))
                normalizedParts.AddRange(new[] { "רגל שמאלית", "רגל ימנית" });
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
