using AltermedManager.Data;
using AltermedManager.Models.Dtos;
using AltermedManager.Models.Entities;
using AltermedManager.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AltermedManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        public AddressController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;

        }
        [HttpGet]
        public IActionResult GetAllAddresses()
        {
            var allDoctors = dbContext.Address.ToList();
            return Ok(allDoctors);
        }


        [HttpGet("addressId/{addressId}")]
        public IActionResult GetAddressByAddressId(int addressId)
        {
            Address address = dbContext.Address.Find(addressId);
            return address == null ? NotFound("No address found.") : Ok(address);
        }

        [HttpGet("lat/{lat}/lng/{lng}")]
        public IActionResult? GetAddressByLatLng(double lat, double lng)
        {
            Address? address = dbContext.Address.FirstOrDefault(a => a.latitude == lat && a.longitude == lng);
            return address == null ? NotFound("No appointments found.") : Ok(address);
        }



        [HttpPost]
        public IActionResult AddAddress(NewAddressDto address)
        {
            var addressEntity = new Address()
            {
                city = address.city,
                houseNumber = address.houseNumber,
                latitude = address.latitude,
                longitude = address.longitude,
                postalCode = address.postalCode,
                street = address.street, 
            };
            dbContext.Address.Add(addressEntity);
            dbContext.SaveChanges();

            return Ok(addressEntity);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public IActionResult DeleteAddress(int id)
        {
            var address = dbContext.Address.Find(id);
            if(address is null)
            {
                return NotFound();
            }
            dbContext.Address.Remove(address);
            dbContext.SaveChanges();
            return Ok("Address deleted");
        }
    }
}
