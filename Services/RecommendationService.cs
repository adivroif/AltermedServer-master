using AltermedManager.Data;
using AltermedManager.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Xml.Linq;

namespace AltermedManager.Services
    {
    public class RecommendationService
        {
        private readonly AppointmentService _appointmentService;
        private readonly TreatmentService _treatmentService;

        public RecommendationService(
            AppointmentService appointmentService,
            TreatmentService treatmentService)
            {
            _appointmentService = appointmentService;
            _treatmentService = treatmentService;
            }

        public List<Treatment>? GetRecommendationsByTreatmentGroup(Guid appointmentID)
            {
            var appointment = _appointmentService.GetAppointmentByUId(appointmentID);
            if (appointment != null)
                {
                var treatment = _treatmentService.GetTreatmentByUId(appointment.treatmentId);
                if (treatment != null)
                    {
                    var treatmentGroup = treatment.treatmentGroup;
                    var treatmentIsAdvanced = treatment.isAdvanced;

                    if (treatmentIsAdvanced) //check another category
                        {
                        List<Treatment> treatments = findTreatmentInAnotherGroup(treatmentGroup);
                        if (treatments is null)
                            {
                            Console.WriteLine($"No treatment found for treatmentId {appointment.treatmentId} in another group");
                            }
                        return treatments;


                        }
                    else //check the same category but another level
                        {

                        }
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

        public List<Treatment> findTreatmentInAnotherGroup(string treatmentGroup)
            {
            var basePath = AppContext.BaseDirectory;

            //string filePath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName, "Resources", "treatmentsLevels.xml");
            string filePath = Path.Combine(AppContext.BaseDirectory, "Resources", "treatmentsLevels.xml");

            XDocument xdoc = XDocument.Load(filePath);
            var treatmentCurrentGroup = xdoc.Descendants("TreatGroup")
                .FirstOrDefault(t => (string)t.Element("TreatmentGroupName") == treatmentGroup);
            if (treatmentCurrentGroup != null)
                {
                
                int currentLevel = int.Parse(treatmentCurrentGroup.Element("GroupLevel")?.Value ?? "0");
                Console.WriteLine($"Found {treatmentCurrentGroup} with Level: {currentLevel}");

                int nextLevel = currentLevel + 1;
                var nextTreatmentGroup = xdoc.Descendants("TreatGroup")
                                             .Where(t => (int)t.Element("GroupLevel") == nextLevel)
                                             .FirstOrDefault();
                if (nextTreatmentGroup != null)
                    {
                    string nextGroupName = (string)nextTreatmentGroup.Element("TreatmentGroupName")?.Value;
                    Console.WriteLine($"Next level treatment group: {nextGroupName} with Level: {nextLevel}");
                    return _treatmentService.GetTreatmentByCategory(nextGroupName);
                    }
                else
                    {
                    Console.WriteLine($"No treatment group found for Level {nextLevel}");
                    }


                }
            else
                {
                Console.WriteLine($"No treatment group found for {treatmentGroup}");
                }
            return null;



            }
        public List<Treatment>? findTreatmentInSameGroup(string treatmentGroup)
            {
            //TODO: implement this method
            return null;
            }

















        }
    }

