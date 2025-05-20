using AltermedManager.Models.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AltermedManager.Models.Dtos
{
    public class NewAddressDto
    {

        public int Id { get; set; }
        public string street { get; set; }
        public string city { get; set; }
        public string postalCode { get; set; }
        public int houseNumber { get; set; }

        public double latitude { get; set; }
        public double longitude { get; set; }

    }
}
