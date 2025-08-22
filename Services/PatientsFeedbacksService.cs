using AltermedManager.Data;
using AltermedManager.Models.Dtos;
using AltermedManager.Models.Entities;
using AltermedManager.Models.Enums;
using AltermedManager.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AltermedManager.Controllers
    {

    public class PatientsFeedbacksService
        {
        private readonly ApplicationDbContext dbContext;
        private readonly TreatmentService _treatmentService;
        private readonly RecommendationService _recommendationService;
        private readonly ILogger<AddressController> _log;

        public PatientsFeedbacksService(ApplicationDbContext dbContext, TreatmentService treatmentService,
            RecommendationService recommendationService, ILogger<AddressController> log)
            {
            this.dbContext = dbContext;
            _treatmentService = treatmentService;
            _recommendationService = recommendationService;
            _log = log;
            }

        public List<PatientFeedback> GetAllPatientsFeedbacks()
            {
            return dbContext.PatientFeedbacks.ToList();
            }

        public async Task<List<PatientFeedback>> GetFeedbacksByPatientId(Guid patientID)
            {
            var feedbacks = await dbContext.PatientFeedbacks
                .Where(f => f.patientId == patientID)
                .ToListAsync();

            return feedbacks;
            }

        public async Task<List<PatientFeedback>> GetFeedbackByAppointmentId(Guid appointmentID)
        {
            var feedbacks = await dbContext.PatientFeedbacks
                .Where(f => f.appointmentId == appointmentID)
                .ToListAsync();

            return feedbacks;
        }

        public PatientFeedback? GetFeedbackByUId(Guid id)
            {
            return dbContext.PatientFeedbacks.Find(id);

            }


        public PatientFeedback AddPatientFeedback(NewPatientFeedbackDto newPatientFeedback)
            {
            var patientFeedbackEntity = new PatientFeedback()
                {
                patientId = newPatientFeedback.patientId,
                appointmentId = newPatientFeedback.appointmentId,
                overallStatus = newPatientFeedback.overallStatus,
                comments = newPatientFeedback.comments,
                createdOn = newPatientFeedback.createdOn,
                newSymptoms = newPatientFeedback.newSymptoms,
                bodyPart = newPatientFeedback.bodyPart
                };
            dbContext.PatientFeedbacks.Add(patientFeedbackEntity);
            dbContext.SaveChanges();
            _treatmentService.UpdateTreatmentScore(newPatientFeedback.overallStatus, newPatientFeedback.appointmentId);
            if (newPatientFeedback.overallStatus <= TreatmentsConst.TREATMENTS_MIN_SCORE)
                _recommendationService.GenerateRecommendationAfterFeedback(patientFeedbackEntity);
            return patientFeedbackEntity;
            }

        public PatientFeedback? UpdatePatientFeedback(Guid id, UpdatePatientFeedbackDto updatePatientFeedbackDto)
            {
            var feedback = dbContext.PatientFeedbacks.Find(id);
            if (feedback is null)
                {
                _log.LogError("Patient feedback with ID {Id} not found for update", id);
                return null;
                }

            feedback.patientId = updatePatientFeedbackDto.patientId;
            feedback.appointmentId = updatePatientFeedbackDto.appointmentId;
            feedback.overallStatus = updatePatientFeedbackDto.overallStatus;
            feedback.comments = updatePatientFeedbackDto.comments;
            feedback.createdOn = updatePatientFeedbackDto.createdOn;
            feedback.newSymptoms = updatePatientFeedbackDto.newSymptoms;
            feedback.bodyPart = updatePatientFeedbackDto.bodyPart;

            dbContext.SaveChanges();
            return feedback;
            }


        public PatientFeedback? DeletePatientFeedback(Guid id)
            {
            var feedback = dbContext.PatientFeedbacks.Find(id);
            if (feedback is null)
                {
                _log.LogWarning("Patient feedback with ID {Id} not found for deletion", id);
                return null;
                }
            dbContext.PatientFeedbacks.Remove(feedback);
            dbContext.SaveChanges();
            return feedback; //return deleted feedback
            }

        }
    }