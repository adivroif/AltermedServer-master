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
using System.Xml.Linq;

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
        private readonly string _filePath = Path.Combine(AppContext.BaseDirectory, "Resources", "treatmentsLevels.xml");
        private OrderedDictionary<string, int> _treatmentsLevelConfiguration = new OrderedDictionary<string, int>();

        public RecommendationService(
            ApplicationDbContext context,
            AppointmentService appointmentService,
            TreatmentService treatmentService,
            PatientsController patientsController,
            FeedbackAnalysisServer feedbackAnalysis
              )
            {
            _context = context;
            _appointmentService = appointmentService;
            _treatmentService = treatmentService;
            _patientsController = patientsController;
            _feedbackAnalysis = feedbackAnalysis;
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
                        Console.WriteLine($"Error loading treatment group from XML file");
                        }
                    }
                }
            catch (FileNotFoundException ex)
                {
                Console.WriteLine($"Error loading file: {ex.Message}");
                }
            }
        public async Task<List<Recommendation>> GetRecommendationsOfPatient(Guid patientId)
        {
            // ב־.NET בצד השרת
            var recommendations = _context.Recommendations
                .Include(r => r.RecommendedTreatment) // כולל את ה־Treatment
                .Where(r => r.patientId == patientId)
                .ToList();

            if (recommendations is null || !recommendations.Any())
            {
                return null;
            }
            return recommendations;
        }

        public async Task<Recommendation> GetRecommendation(int recommendationId)
        {
            var recommendation = _context.Recommendations.Find(recommendationId);

            return recommendation;
        }
        public async Task<Recommendation> UpdateRecommendation(int recommendationId,UpdateRecommendationDto updateRecommendationDto)
        {

            var recommendation = _context.Recommendations.Find(recommendationId);
            if (recommendation is null)
            {
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
            return recommendation;
        }

    public async Task<List<Recommendation>> GetRecommendationsNotChosenOfPatient(Guid patientId)
        {
            // ב־.NET בצד השרת
            var recommendations = _context.Recommendations
                .Include(r => r.RecommendedTreatment) // כולל את ה־Treatment
                .Where(r => r.patientId == patientId && (r.isChosen == false || r.isChosen == null))
                .ToList();

            if (recommendations is null || !recommendations.Any())
            {
                return null;
            }
            return recommendations;
        }
        public Recommendation? GetRecommendationsByTreatmentGroup(Guid appointmentID)
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
                        Console.WriteLine($"Try to find treatment in the same group - another level");
                        treatments = FindTreatmentsInCurrentGroup(treatmentGroup).Where(t => t.isAdvanced == true).ToList();
                        if (treatments is null || treatments.Count() == 0)
                            {
                            Console.WriteLine($"No advanced treatments found for treatmentId {appointment.treatmentId} in the same group");
                            }
                        else return CreateAndSaveNewRecommendation(treatments.FirstOrDefault(), appointmentID, "More advanced treatment"); //advanced treatment in current group found
                        }
                    //treatment is advanced or cant found in the same group - check another category
                    treatments = FindTreatmentsInAnotherGroup(treatmentGroup);
                    //try to find from not advanced treatments in another group
                    if (treatments is null || treatments.Count() == 0)
                        {
                        Console.WriteLine($"No treatment found for treatmentId {appointment.treatmentId} in another group");
                        //TODO: CHECK IN NEXT GROUP MAY BE
                        return null;
                        }
                    List<Treatment> notAdvancedTreatments = treatments.Where(t => t.isAdvanced == false).ToList();
                    if (notAdvancedTreatments.Count == 0)
                        {
                        Console.WriteLine($"No light treatments found for treatmentId {appointment.treatmentId} in another group");
                        List<Treatment> advancedTreatments = treatments.Where(t => t.isAdvanced == true).ToList();
                        if (advancedTreatments.Count == 0)
                            {
                            Console.WriteLine($"No advanced treatments found for treatmentId {appointment.treatmentId} in another group");
                            return null;
                            }
                        return CreateAndSaveNewRecommendation(advancedTreatments.FirstOrDefault(), appointmentID, "More advanced treatment in another group");
                        }
                    return CreateAndSaveNewRecommendation(notAdvancedTreatments.FirstOrDefault(), appointmentID, "Treatment in another group");
                    }
                else
                    {
                    Console.WriteLine($"No treatment found for treatmentId {appointment.treatmentId}");
                    }
                }
            else
                {
                Console.WriteLine($"No appointment found for appointmentId {appointmentID}");
                }
            return null;
            }

        private List<Treatment> FindTreatmentsInCurrentGroup(string treatmentGroup)
            {
            List<Treatment> treatments = _treatmentService.GetTreatmentByCategory(treatmentGroup);
            if (treatments != null)
                {
                Console.WriteLine($"Found {treatments.Count} treatments in group {treatmentGroup}");
                return treatments;
                }
            return new List<Treatment>();
            }

        public List<Treatment> FindTreatmentsInAnotherGroup(string currentTreatmentGroup)
            {

            string? nextTreatmentGroup = null;
            if (_treatmentsLevelConfiguration.Count == 0)
                {
                Console.WriteLine($"No treatment group found in configuration file");
                return new List<Treatment>();
                }
            else
                {
                if (_treatmentsLevelConfiguration.TryGetValue(currentTreatmentGroup, out int currentLevel))
                    {

                    do
                        {
                        Console.WriteLine($"Found treatment group {currentTreatmentGroup} with Level: {currentLevel}");
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
                            Console.WriteLine($"Next level treatment group: {nextTreatmentGroup} with Level: {nextLevel}");
                            List<Treatment> treatments = FindTreatmentsInCurrentGroup(nextTreatmentGroup);
                            if (treatments != null)
                                {
                                Console.WriteLine($"Found {treatments.Count} treatments in group {nextTreatmentGroup}");
                                return treatments;
                                }
                            else
                                {
                                Console.WriteLine($"No treatment found for group {nextTreatmentGroup}");
                                }
                            }
                        else
                            {
                            Console.WriteLine($"No treatment group found for {currentTreatmentGroup}");
                            }
                        Console.WriteLine($"Found {_treatmentsLevelConfiguration.Count} treatment groups in configuration file");

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
        public Recommendation CreateAndSaveNewRecommendation(Treatment _newTreatment, Guid _appointmentId, string _reason)
            {
            var appointment = _appointmentService.GetAppointmentByUId(_appointmentId);
            if (appointment == null)
                {
                throw new ArgumentNullException(nameof(appointment), "Appointment not found.");
                }
            var patientId = appointment.patientId;
            var patient = _patientsController.GetPatientByUId(patientId);
            if (patient == null)
                {
                throw new Exception("Patient not found for the provided ID.");
                }

            var recommendation = new Recommendation
                {
                patientId = patientId,
                appointmentId = _appointmentId,
                recommendedTreatmentId = _newTreatment.treatmentId,
                reason = "General Recommendation",
                source = _reason,
                isChosen = null,
                Patient = patient,
                RecommendedTreatment = _newTreatment
                };

            _context.Recommendations.Add(recommendation);
            _context.SaveChanges();
            return recommendation;
            }


        /* 
         * Function will be used to choose treatment for recommendation when there are more than one treatment.
         * It will return the treatment with the highest score
         */
        private Treatment? ChooseTreatmentForRecommendation(List<Treatment> treatments)
            {
            if (treatments == null || treatments.Count == 0)
                {
                Console.WriteLine($"No treatment found for recommendation");
                return null;
                }
            else
                {
                Console.WriteLine($"Found {treatments.Count} treatments for recommendation");
                //choose the treatment with the highest score
                treatments = treatments.OrderByDescending(t => t.score).ToList();
                Console.WriteLine($"Treatment with the highest score: {treatments.FirstOrDefault().treatmentName}");
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
                    throw new ArgumentNullException(nameof(appointment), "Appointment not found.");
                    }

                var treatment = _treatmentService.GetTreatmentByUId(appointment.treatmentId);
                if (treatment == null)
                    {
                    throw new ArgumentNullException(nameof(treatment), "Treatment not found.");
                    }

                //generate recommendation in two phases:
                //1. find treatment based on bodyPart and score
                var bodyTreatment = FindTreatmentByBodyPart(patientFeedbackEntity.bodyPart, treatment);
                if (bodyTreatment == null)
                    {
                    //2. find treatment based on treatment group and isAdvanced or not (without body part + score)
                    // if bodyTreatment is null or empty
                    // var groupTreatments = FindTreatmentsByGroup()
                    GetRecommendationsByTreatmentGroup(patientFeedbackEntity.appointmentId);
                    }
                else
                    {
                    //3. create recommendation
                    CreateAndSaveNewRecommendation(bodyTreatment, patientFeedbackEntity.appointmentId, "Similar patients reported better results");
                    }
                }
            catch (ArgumentNullException ex)
                {
                Console.WriteLine($"Error: {ex.Message}");
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
                    Console.WriteLine($"No feedback found for body part {bodyPart}");
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
                    Console.WriteLine($"No treatment found for body part {bodyPart}");
                    return null;
                    }
                return _treatmentService.GetTreatmentByUId((int)topTreatmentId);
                
                }
            catch (ArgumentNullException ex)
                {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
                }
            catch (Exception ex)
                {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
                }
            }
        }
    }

