using AltermedManager.Controllers;
using AltermedManager.Data;
using AltermedManager.Models.Dtos;
using AltermedManager.Models.Entities;
using AltermedManager.Models.Enums;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using System.Xml.Linq;

using AltermedManager.Resources;

namespace AltermedManager.Services
    {
    public class RecommendationService
        {
        private readonly ApplicationDbContext _context;
        private readonly AppointmentService _appointmentService;
        private readonly TreatmentService _treatmentService;
        private readonly PatientsController _patientsController;
        //private readonly PatientsFeedbacksService _feedbackService;
        private readonly FeedbackAnalysisServer _feedbackAnalysis;
        private readonly INotificationsService _notificationsService;
        private readonly ILogger<AddressController> _log;
        private readonly string _filePath = Path.Combine(AppContext.BaseDirectory, "Resources", "treatmentsLevels.xml");
        private OrderedDictionary<string, int> _treatmentsLevelConfiguration = new OrderedDictionary<string, int>();

        public RecommendationService(
            ApplicationDbContext context,
            AppointmentService appointmentService,
            TreatmentService treatmentService,
            PatientsController patientsController,
            FeedbackAnalysisServer feedbackAnalysis,
            INotificationsService notificationsService,
            ILogger<AddressController> log
              )
            {
            _context = context;
            _appointmentService = appointmentService;
            _treatmentService = treatmentService;
            _patientsController = patientsController;
            _feedbackAnalysis = feedbackAnalysis;
            _notificationsService = notificationsService;
            _log = log;
            LoadTreatmentsLevelConfigurationFile(_filePath);
            }
        private void LoadTreatmentsLevelConfigurationFile(string filePath)
            {
            try
                {
                if (!File.Exists(filePath))
                    {
                    throw new FileNotFoundException($"File not found: {filePath}");
                    }
                XDocument xdoc = XDocument.Load(filePath);
                foreach (XElement element in xdoc.Descendants("TreatGroup"))
                    {
                    string? treatmentGroupName = element.Element("TreatmentGroupName")?.Value;
                    string? groupLevel = element.Element("GroupLevel")?.Value;
                    if (treatmentGroupName != null && groupLevel != null)
                        {
                        _treatmentsLevelConfiguration.Add(treatmentGroupName, int.Parse(groupLevel));
                        }
                    else
                        {
                        _log.LogError($"Error loading treatment group from XML file: {filePath}");
                        }
                    }
                }
            catch (FileNotFoundException ex)
                {
                _log.LogError(ex, "Error loading treatments level configuration file: {FilePath}", filePath);
                }
            }
        public async Task<List<Recommendation>> GetRecommendationsOfPatient(Guid patientId)
        {
            
            var recommendations = _context.Recommendations
                .Include(r => r.RecommendedTreatment) // include Treatment
                .Where(r => r.patientId == patientId)
                .ToList();

            if (recommendations is null || !recommendations.Any())
            {
                _log.LogWarning("No recommendations found for patient with ID: {PatientId}", patientId);
                return null;
            }
            return recommendations;
        }

        public async Task<Recommendation> GetRecommendation(int recommendationId)
        {
            var recommendation = _context.Recommendations.Find(recommendationId);
            if (recommendation is null)
                {
                _log.LogWarning("No recommendation found with ID: {RecommendationId}", recommendationId);
                return null;
                }
            return recommendation;
        }
        public async Task<Recommendation> UpdateRecommendation(int recommendationId,UpdateRecommendationDto updateRecommendationDto)
        {
            if(updateRecommendationDto is null)
                {
                _log.LogError("UpdateRecommendationDto is null, not available to update recommendation");
                return null;
                }
            var recommendation = _context.Recommendations.Find(recommendationId);
            if (recommendation is null)
            {
                _log.LogWarning("No recommendation found with ID: {RecommendationId}", recommendationId);
                return null;
            }
            recommendation.recommendationId = updateRecommendationDto.recommendationId;
            recommendation.patientId = updateRecommendationDto.patientId;
            recommendation.appointmentId = updateRecommendationDto.appointmentId;
            recommendation.RecommendedTreatment = updateRecommendationDto.RecommendedTreatment;
            recommendation.recommendationDate = updateRecommendationDto.recommendationDate;
            recommendation.reason = updateRecommendationDto.reason;
            recommendation.source = updateRecommendationDto.source;
            recommendation.isChosen = updateRecommendationDto.isChosen;

            _context.SaveChanges();

            bool approved = (bool)updateRecommendationDto.isChosen;
            if (approved)
                {
                //send notification to the patient
                Guid patientId = updateRecommendationDto.patientId;
                string? msgToken = _context.Users
                    .Where(u => u.id == patientId)
                    .Select(u => u.msgToken)
                    .FirstOrDefault();
                await _notificationsService.SendNewRecommendationToPatientAsync(patientId, recommendation, msgToken);

                }   
            return recommendation;
        }

    public async Task<List<Recommendation>> GetRecommendationsNotChosenOfPatient(Guid patientId)
        {
            
            var recommendations = _context.Recommendations
                .Include(r => r.RecommendedTreatment) // include Treatment
                .Where(r => r.patientId == patientId && (r.isChosen == false || r.isChosen == null))
                .ToList();

            if (recommendations is null || !recommendations.Any())
            {
                _log.LogWarning("No approved recommendations found for patient with ID: {PatientId}", patientId);
                return null;
            }
            return recommendations;
        }
        public async Task<Recommendation?> GetRecommendationsByTreatmentGroupAsync(Guid appointmentID)
            {
            var appointment = _appointmentService.GetAppointmentByUId(appointmentID);
            if (appointment != null)
                {
                var treatment = _treatmentService.GetTreatmentByUId(appointment.treatmentId);
                if (treatment != null)
                    {
                    List<Treatment> treatments;
                    var treatmentGroup = treatment.treatmentGroup;
                    var treatmentIsAdvanced = treatment.isAdvanced;

                    if (!treatmentIsAdvanced) //check the same category but another level
                        {
                        _log.LogInformation($"Treatment {treatment.treatmentName} is not advanced, trying to find in the same group but another level");
                        treatments = FindTreatmentsInCurrentGroup(treatmentGroup).Where(t => t.isAdvanced == true).ToList();
                        if (treatments is null || treatments.Count() == 0)
                            {
                            _log.LogInformation($"No advanced treatments found for treatmentId {appointment.treatmentId} in the same group");
                            }
                        else return await CreateAndSaveNewRecommendation(treatments.FirstOrDefault(), appointmentID, RecommendationReasons.AdvancedTreatmentSameGroup); //advanced treatment in current group found
                        }
                    //treatment is advanced or cant found in the same group - check another category
                    treatments = FindTreatmentsInAnotherGroup(treatmentGroup);
                    //try to find from not advanced treatments in another group
                    if (treatments is null || treatments.Count() == 0)
                        {
                        _log.LogInformation($"No treatments found for treatmentId {appointment.treatmentId} in another group");                        
                        return null;
                        }
                    List<Treatment> notAdvancedTreatments = [.. treatments.Where(t => t.isAdvanced == false)]; //ToList simplifier
                    if (notAdvancedTreatments.Count == 0)
                        {
                        _log.LogInformation($"No not advanced treatments found for treatmentId {appointment.treatmentId} in another group");

                        List<Treatment> advancedTreatments = treatments.Where(t => t.isAdvanced == true).ToList();
                        if (advancedTreatments.Count == 0)
                            {
                            _log.LogInformation($"No advanced treatments found for treatmentId {appointment.treatmentId} in another group");
                            return null;
                            }
                        return await CreateAndSaveNewRecommendation(advancedTreatments.FirstOrDefault(), appointmentID, RecommendationReasons.TreatmentFromAnotherGroup);
                        }
                    var res = await CreateAndSaveNewRecommendation(notAdvancedTreatments.FirstOrDefault(), appointmentID, RecommendationReasons.TreatmentFromAnotherGroup);
                    return res;
                    }
                else
                    {
                    _log.LogInformation($"No treatment found for treatmentId {appointment.treatmentId} in appointmentId {appointmentID}");
                    }
                }
            else
                {
                _log.LogInformation($"No appointment found for appointmentId {appointmentID}");
                }
            return null;
            }

        private List<Treatment> FindTreatmentsInCurrentGroup(string treatmentGroup)
            {
            List<Treatment> treatments = _treatmentService.GetTreatmentByCategory(treatmentGroup);
            if (treatments != null)
                {
                _log.LogInformation($"Found {treatments.Count} treatments in group {treatmentGroup}");
                return treatments;
                }
            return new List<Treatment>();
            }

        public List<Treatment> FindTreatmentsInAnotherGroup(string currentTreatmentGroup)
            {

            string? nextTreatmentGroup = null;
            if (_treatmentsLevelConfiguration.Count == 0)
                {
                _log.LogError($"No treatment group found in configuration file: {_filePath}");
                return new List<Treatment>();
                }
            else
                {
                if (_treatmentsLevelConfiguration.TryGetValue(currentTreatmentGroup, out int currentLevel))
                    {

                    do
                        {

                        _log.LogInformation($"Found treatment group {currentTreatmentGroup} with Level: {currentLevel}");
                        int nextLevel;
                        if (currentLevel == _treatmentsLevelConfiguration.Count) //the last category
                            {
                            nextLevel = _treatmentsLevelConfiguration.Values.FirstOrDefault();
                            }
                        else
                            {
                            nextLevel = currentLevel + 1;
                            }
                        //found treatment group with more advanced level than current
                        nextTreatmentGroup = _treatmentsLevelConfiguration
                            .FirstOrDefault(t => t.Value == nextLevel).Key;
                        if (nextTreatmentGroup != null)
                            {
                            _log.LogInformation($"Next treatment group found: {nextTreatmentGroup} with Level: {nextLevel}");
                            List<Treatment> treatments = FindTreatmentsInCurrentGroup(nextTreatmentGroup);
                            if (treatments != null)
                                {
                                _log.LogInformation($"Found {treatments.Count} treatments in group {nextTreatmentGroup}");
                                return treatments;
                                }
                            else
                                {
                                _log.LogInformation($"No treatments found in group {nextTreatmentGroup} with Level: {nextLevel}");
                                
                                }
                            }
                        else
                            {
                            _log.LogInformation($"No next treatment group found for {currentTreatmentGroup} with Level: {currentLevel}");
                            }

                        _log.LogInformation($"Found {_treatmentsLevelConfiguration.Count} treatment groups in configuration file");

                        }
                    while (nextTreatmentGroup != currentTreatmentGroup);

                    }
                }
            return new List<Treatment>();
            }



        /* 
         * Function will create new recommendation and save it to the database.
         * It will be used when treatment found.
         */
        public async Task<Recommendation> CreateAndSaveNewRecommendation(Treatment _newTreatment, Guid _appointmentId, string _reason)
            {
            try
                {
                var appointment = _appointmentService.GetAppointmentByUId(_appointmentId);
                if (appointment == null)
                    {
                    _log.LogError("Appointment not found for ID: {AppointmentId}", _appointmentId);
                    return null;
                    }
                var patientId = appointment.patientId;
                var patient = _patientsController.GetPatientByUId(patientId);
                if (patient == null)
                    {
                    _log.LogError("Patient not found for ID: {PatientId}", patientId);
                    return null;
                    }

                var recommendation = new Recommendation
                    {
                    patientId = patientId,
                    appointmentId = _appointmentId,
                    recommendedTreatmentId = _newTreatment.treatmentId,
                    reason = RecommendationReasons.GeneralRecommendation,
                    source = RecommendationsSources.system,
                    isChosen = null,
                    Patient = patient,
                    RecommendedTreatment = _newTreatment
                    };

                _context.Recommendations.Add(recommendation);
                _context.SaveChanges();
                _context.Entry(recommendation).Reload(); // Reload to get the generated ID and other properties

                // Send notification to doctor for approval
                Guid doctorId = appointment.doctorId;
                string? msgToken = _context.Users
                    .Where(u => u.id == doctorId)
                    .Select(u => u.msgToken)
                    .FirstOrDefault();
                int newRecommendationId = recommendation.recommendationId;
                await _notificationsService.SendDoctorRecommendationApprovalRequestAsync(doctorId, recommendation, msgToken);
                return recommendation;
                }
            catch (Exception ex)
                {
                _log.LogError(ex, "Error creating and saving new recommendation for appointmentId: {AppointmentId}", _appointmentId);
                return null;
                }
            }


        /* 
         * Function will be used to choose treatment for recommendation when there are more than one treatment.
         * It will return the treatment with the highest score.
         */
        private Treatment? ChooseTreatmentForRecommendation(List<Treatment> treatments)
            {
            if (treatments == null || treatments.Count == 0)
                {
                _log.LogInformation($"No treatments found for recommendation");
                return null;
                }
            else
                {
                _log.LogInformation($"Found {treatments.Count} treatments for recommendation");
                //choose the treatment with the highest score
                treatments = treatments.OrderByDescending(t => t.score).ToList();
                _log.LogInformation($"Treatment with the highest score: {treatments.FirstOrDefault().treatmentName}");
                return treatments.FirstOrDefault();

                }
            }

        internal void GenerateRecommendationAfterFeedback(PatientFeedback patientFeedbackEntity)
            {

            try
                {
                var appointment = _appointmentService.GetAppointmentByUId(patientFeedbackEntity.appointmentId);
                if (appointment == null)
                    {
                    _log.LogError("Appointment not found for ID: {AppointmentId}", patientFeedbackEntity.appointmentId);
                    return;
                    }

                var treatment = _treatmentService.GetTreatmentByUId(appointment.treatmentId);
                if (treatment == null)
                    {
                    _log.LogError("Treatment not found for ID: {TreatmentId}", appointment.treatmentId);
                    return;
                    }

                //generate recommendation in two phases:
                //1. find treatment based on bodyPart and score
                var bodyTreatment = FindTreatmentByBodyPart(patientFeedbackEntity.bodyPart, treatment);
                if (bodyTreatment == null)
                    {
                    //2. find treatment based on treatment group and isAdvanced or not (without body part + score)
                    // if bodyTreatment is null or empty
                    // var groupTreatments = FindTreatmentsByGroup()
                    GetRecommendationsByTreatmentGroupAsync(patientFeedbackEntity.appointmentId);
                    }
                else
                    {
                    //3. create recommendation
                    CreateAndSaveNewRecommendation(bodyTreatment, patientFeedbackEntity.appointmentId, RecommendationReasons.SimilarPatientsReportBetterResults);
                    }
                }
            catch (Exception ex)
                {
                _log.LogInformation($"Error when generationg recommendation: {ex.Message}");
                }

            }

        /* 
         * Function will get as input the body part and treatment group and will return the list of treatments
         * that treat the same part of the body. If it wil not find any treatment in current group, that can feet, 
         * it will start searching in the next group.
         */
        public Treatment? FindTreatmentByBodyPart(string bodyPart, Treatment badTreatment)
            {

            try
                {
                if (badTreatment == null)
                    {
                    throw new ArgumentNullException(nameof(badTreatment), "Treatment not found.");
                    }
                if (string.IsNullOrEmpty(bodyPart))
                    {
                    throw new ArgumentNullException(nameof(bodyPart), "Body part not found.");
                    }
                var relevantFeedbacks = _feedbackAnalysis.GetHighFeedbacksByBodyPart(bodyPart, badTreatment);
                if (relevantFeedbacks == null || relevantFeedbacks.Count == 0)
                    {
                    _log.LogInformation($"No feedbacks found for body part {bodyPart} and treatment {badTreatment.treatmentName}");
                    return null;
                    }
                var treatmentRankings = relevantFeedbacks
                    .Where(f => f.treatmentId != badTreatment.treatmentId)
                    .GroupBy(f => f.treatmentId)
                    .Select(g => new
                        {
                        TreatmentId = g.Key,
                        AverageScore = g.Average(f => f.score),
                        Count = g.Count()
                        })
                    .OrderByDescending(g => g.AverageScore)
                    .ThenByDescending(g => g.Count)
                    .ToList();

                var topTreatmentId = treatmentRankings.FirstOrDefault()?.TreatmentId;
                if (topTreatmentId == null)
                    {
                    _log.LogInformation($"No treatments found for body part {bodyPart} with better score than {badTreatment.treatmentName}.");
                    return null;
                    }
                return _treatmentService.GetTreatmentByUId((int)topTreatmentId);
                
                }
            catch (ArgumentNullException ex)
                {
                _log.LogError(ex, "Argument null exception: {Message}", ex.Message);
                return null;
                }
            catch (Exception ex)
                {
                _log.LogError(ex, "Error finding treatment by body part: {Message}", ex.Message);
                return null;
                }
            }
        }
    }

