using AltermedManager.Data;
using AltermedManager.Models.Dtos;
using AltermedManager.Models.Entities;
using AltermedManager.Models.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AltermedManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        public UsersController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;

        }
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var allUsers = dbContext.Users.ToList();
            return Ok(allUsers);
        }


        [HttpGet("{name}")]
        public IActionResult GetUserByUName(string name)
        {
            var user = dbContext.Users.FirstOrDefault(u => u.name == name);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }




        [HttpPost]
        public IActionResult AddUser(NewUserDto newUser)
        {
            var userEntity = new User()
            {
                //appointmentId = newAppointment.appointmentId,
                name = newUser.name,
                id = newUser.id
            };

            dbContext.Users.Add(userEntity);
            dbContext.SaveChanges();

            return Ok(userEntity);
        }


        [HttpPut]
        [Route("{id:guid}")]
        public IActionResult UpdateUser(Guid id, UpdateUserDto updateUser)
        {
            var user
                = dbContext.Users.Find(id);
            if (user is null)
            {
                return NotFound();
            }
            user.id = updateUser.id;
            user.name = updateUser.name;
            dbContext.SaveChanges();
            return Ok(user);
        }


        [HttpDelete]
        [Route("{id:guid}")]
        public IActionResult DeleteUser(Guid id)
        {
            var user = dbContext.Users.Find(id);
            if (user is null)
            {
                return NotFound();
            }
            dbContext.Users.Remove(user);
            dbContext.SaveChanges();
            return Ok("User deleted");
        }
    }
}
