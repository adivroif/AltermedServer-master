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
        private readonly ILogger<AddressController> _log;
        public AddressController(ApplicationDbContext dbContext, ILogger<AddressController> log)
        {
            this.dbContext = dbContext;
            _log = log;    

        }
        [HttpGet]
        public IActionResult GetAllAddresses()
        {
            var allAddresses = dbContext.Address.ToList();
            return Ok(allAddresses);
        }


        [HttpGet("addressId/{addressId}")]
        public IActionResult GetAddressByAddressId(int addressId)
        {
            var address = dbContext.Address.Find(addressId);
            if(address is null)
                {
                _log.LogWarning("Address {AddressId} not found", addressId);
                return NotFound("No address found.");
                }
            return Ok(address);
        }

        [HttpGet("lat/{lat}/lng/{lng}")]
        public IActionResult? GetAddressByLatLng(double lat, double lng)
        {
            const double eps = 0.00001;
            var address = dbContext.Address
                .AsNoTracking()
                .FirstOrDefault(a =>
                    Math.Abs(a.latitude - lat) < eps &&
                    Math.Abs(a.longitude - lng) < eps);
            if (address is null)
                {
                _log.LogWarning("Address with {Latitude} and {Longitude} not found", lat, lng);
                return NotFound("No address found.");
                }
            return Ok(address);
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
                _log.LogWarning("Address with {Id} not found", id);
                return NotFound();
            }
            dbContext.Address.Remove(address);
            dbContext.SaveChanges();
            _log.LogInformation("Address with {Id} deleted", id);
            return Ok("Address deleted");
        }
    }
}
