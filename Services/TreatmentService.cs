using AltermedManager.Controllers;
using AltermedManager.Data;
using AltermedManager.Models.Dtos;
using AltermedManager.Models.Entities;

namespace AltermedManager.Services
    {
    public class TreatmentService
        {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AddressController> _log;

        public TreatmentService(ApplicationDbContext context, ILogger<AddressController> _log)
            {
            _context = context;
            this._log = _log;
            }
        public List<Treatment> GetAllTreatments()
            {
            return _context.Treatments.ToList();
            }
        public Treatment? GetTreatmentByUId(int id)
            {
            return _context.Treatments.Find(id);
            }
        public Treatment AddTreatment(NewTreatmentDto newTreatment)
            {

            var treatmentEntity = new Treatment()
                {
                treatmentId = newTreatment.treatmentId,
                treatmentName = newTreatment.treatmentName,
                treatmentDescription = newTreatment.treatmentDescription,
                treatmentPrice = newTreatment.treatmentPrice,
                suitCategories = newTreatment.suitCategories,
                treatmentGroup = newTreatment.treatmentGroup,
                isAdvanced = newTreatment.isAdvanced,
                numOfFeedbacks = newTreatment.numOfFeedbacks,
                score = newTreatment.score
                };
            _context.Treatments.Add(treatmentEntity);
            _context.SaveChanges();
            return treatmentEntity;
            }
        public Treatment? UpdateTreatment(Guid id, UpdateTreatmentDto updateDto)
            {
            var treatment = _context.Treatments.Find(id);
            if (treatment is null)
                {
                _log.LogError("Treatment with ID {Id} not found", id);
                return null;
                }

            treatment.treatmentName = updateDto.treatmentName;
            treatment.treatmentDescription = updateDto.treatmentDescription;
            treatment.treatmentPrice = updateDto.treatmentPrice;
            treatment.suitCategories = updateDto.suitCategories;
            treatment.treatmentGroup = updateDto.treatmentGroup;
            treatment.isAdvanced = updateDto.isAdvanced;
            treatment.numOfFeedbacks = updateDto.numOfFeedbacks;
            treatment.score = updateDto.score;
            _context.SaveChanges();
            return treatment;
            }
        public bool DeleteTreatment(Guid id)
            {
            var treatment = _context.Treatments.Find(id);
            if (treatment is null)
                {
                _log.LogError("Treatment with ID {Id} not found for deletion", id);
                return false;
                }
            _context.Treatments.Remove(treatment);
            _context.SaveChanges();
            return true;
            }

        internal List<Treatment> GetTreatmentByCategory(string? groupName)
            {
            if (groupName != null)
                {
                var treatments = _context.Treatments
                    .Where(t => t.treatmentGroup == groupName)
                    .ToList();
                return treatments;
                }
            else
                {
                _log.LogWarning("Group name is null, returning empty treatment list.");
                return null;

                }
            }
        public Treatment GetTreatmentByUName(string name)
            {
            return _context.Treatments.FirstOrDefault(t => t.treatmentName == name);
            }

        /*
         * Function will update the current score of the treatment.
         * Invokes when a patient leaves feedback on the appointment.
         */
        internal void UpdateTreatmentScore(float _overallStatus, Guid _appointmentId)
            {

            var appointment = _context.Appointments
                .FirstOrDefault(t => t.appointmentId == _appointmentId);
            if (appointment != null)
                {
                var treatment = _context.Treatments
                    .FirstOrDefault(t => t.treatmentId == appointment.treatmentId);
                if (treatment != null)
                    {
                    treatment.numOfFeedbacks += 1;
                    treatment.score = (treatment.score * (treatment.numOfFeedbacks - 1) + _overallStatus) / treatment.numOfFeedbacks;
                    _context.SaveChanges();

                    }

                }
            else
                {
                _log.LogError("Appointment with ID {AppointmentId} not found", _appointmentId);
                }
            }

        }
    }



