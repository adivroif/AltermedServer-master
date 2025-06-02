using AltermedManager.Data;
using AltermedManager.Helpers;
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

        [HttpGet("uid/{uuid}")]
        public async Task<IActionResult> GetUserByUidAsync(Guid uuid)
            {
            var authHeader = Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(authHeader))
                {
                return Unauthorized("Missing or invalid Authorization header");
                }
            var token = authHeader.Substring("Bearer ".Length).Trim();

            var firebaseUid = await FirebaseTokenValidator.VerifyTokenAsync(token);
            if (firebaseUid == null || firebaseUid != uuid.ToString())
                return Unauthorized("Invalid or mismatched token");

            var user = dbContext.Users.FirstOrDefault(u => u.id == uuid);
            if (user == null)
                {
                return NotFound();
                }
            return Ok(user);
            }

        [HttpGet("fuid/{uid}")]
        public async Task<IActionResult> GetUserByFirebaseIdAsync(String uid)
            {
            var authHeader = Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(authHeader))
                {
                return Unauthorized("Missing or invalid Authorization header");
                }
            var token = authHeader.Substring("Bearer ".Length).Trim();

            var firebaseUid = await FirebaseTokenValidator.VerifyTokenAsync(token);
            if (firebaseUid == null || firebaseUid != uid.ToString())
                return Unauthorized("Invalid or mismatched token");
            /*
            var user = new User()
                {
                name = "בדיקה  בדיקה",
                firebaseId = "edr93Eb4nmPVRNtoRgeV0ukNIiR2",
                id = System.Guid.Parse("da66b18d-fdb4-4ea4-acc6-6c67a4570cd0"),
                role = UserRoleConst.PATIENT

                };*/
            var user = dbContext.Users.FirstOrDefault(u => u.firebaseId == uid);
            if (user == null)
                {
                return NotFound();
                }
            return Ok(user);
            }


        // -----------------FOR TEST ONLY----------------------
        [HttpGet("email{email}")]
        public IActionResult GetUserByEmail(string email)
            {
            var user = dbContext.Patients.FirstOrDefault(u => u.patientEmail == email);
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
                firebaseId = newUser.firebaseId,
                id = newUser.id,
                role = newUser.role
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
