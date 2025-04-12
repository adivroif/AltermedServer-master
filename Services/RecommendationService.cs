using AltermedManager.Data;
using AltermedManager.Models.Entities;
using AltermedManager.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Xml.Linq;

namespace AltermedManager.Services
    {
    public class RecommendationService
        {
        private readonly AppointmentService _appointmentService;
        private readonly TreatmentService _treatmentService;
        private readonly string _filePath = Path.Combine(AppContext.BaseDirectory, "Resources", "treatmentsLevels.xml");
        private OrderedDictionary<string, int> _treatmentsLevelConfiguration = new OrderedDictionary<string, int>();

        public RecommendationService(
            AppointmentService appointmentService,
            TreatmentService treatmentService)
            {
            _appointmentService = appointmentService;
            _treatmentService = treatmentService;
            loadTreatmentsLevelConfigurationFile(_filePath);



            }

        private void loadTreatmentsLevelConfigurationFile(string filePath)
            {
            try
                {
                if (!File.Exists(filePath))
                    {

                    throw new FileNotFoundException($"File not found: {filePath}");

                    }
                XDocument xdoc = XDocument.Load(filePath);
                foreach( XElement element in xdoc.Descendants("TreatGroup")) {
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

        public List<Treatment> GetRecommendationsByTreatmentGroup(Guid appointmentID)
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

                    if(!treatmentIsAdvanced) //check the same category but another level
                        {
                        Console.WriteLine($"Try to find treatment in the same group - another level");
                        treatments = findAdvancedTreatmentInCurrentGroup(treatmentGroup);

                        if (treatments is null || treatments.Count() == 0)
                            {
                            Console.WriteLine($"No treatment found for treatmentId {appointment.treatmentId} in the same group");

                            }
                        else return treatments; //advanced treatment in current group found

                        } 
                    //treatment is advanced or cant found in the same group - check another category
                     treatments = findTreatmentInAnotherGroup(treatmentGroup);
                     if (treatments is null)
                            {
                            Console.WriteLine($"No treatment found for treatmentId {appointment.treatmentId} in another group");
                            return new List<Treatment>();
                            }
                     return treatments;

                        
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
            return new List<Treatment>();
            }

        private List<Treatment> findAdvancedTreatmentInCurrentGroup(string treatmentGroup)
            {
             List<Treatment> treatments = _treatmentService.GetTreatmentByCategory(treatmentGroup);
             if (treatments != null)
                {
                Console.WriteLine($"Found {treatments.Count} treatments in group {treatmentGroup}");
                return treatments.Where(t => t.isAdvanced == true).ToList();
                }
             return new List<Treatment>();
            }

        public List<Treatment> findTreatmentInAnotherGroup(string currentTreatmentGroup)
            {


            if (_treatmentsLevelConfiguration.Count == 0)
                {
                Console.WriteLine($"No treatment group found in configuration file");
                return new List<Treatment>();
                }
            else
                {
                if (_treatmentsLevelConfiguration.TryGetValue(currentTreatmentGroup, out int currentLevel))
                    {
                    Console.WriteLine($"Found treatment group {currentTreatmentGroup} with Level: {currentLevel}");
                    int nextLevel;
                    if (currentLevel == _treatmentsLevelConfiguration.Count) //the last category
                        {
                        nextLevel = _treatmentsLevelConfiguration.Values.FirstOrDefault();
                        }
                    else { 
                    nextLevel = currentLevel + 1;
                    }
                    //found treatment group with more advanced level tnan current
                    var nextTreatmentGroup = _treatmentsLevelConfiguration
                        .FirstOrDefault(t => t.Value == nextLevel).Key;
                    if (nextTreatmentGroup != null)
                        {
                        Console.WriteLine($"Next level treatment group: {nextTreatmentGroup} with Level: {nextLevel}");
                        List<Treatment> treatments = _treatmentService.GetTreatmentByCategory(nextTreatmentGroup);
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


                }
                return new List<Treatment>();
            }
        public List<Treatment>? findTreatmentInSameGroup(string treatmentGroup)
            {
            //TODO: implement this method
            return null;
            }

















        }
    }

